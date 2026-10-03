import math
from dataclasses import fields
from app.models import BlastFurnaceInput


def validate(data):
    if not isinstance(data, dict):
        raise ValueError("Ожидается JSON-объект.")
    names = {field.name for field in fields(BlastFurnaceInput)}
    if set(data) != names:
        raise ValueError(f"Проверьте поля: отсутствуют {sorted(names - set(data))}; неизвестны {sorted(set(data) - names)}.")
    for name, value in data.items():
        if isinstance(value, bool) or not isinstance(value, (int, float)) or not math.isfinite(value):
            raise ValueError(f"{name}: нужно конечное число.")
        if value < 0:
            raise ValueError(f"{name}: значение не может быть отрицательным.")
    percentages = ("Si Mn S P Ti Cr V C coke_ash coke_sulfur coke_volatiles coke_moisture oxygen_content "
                   "gas_CH4 gas_C2H6 gas_CO2 limestone_moisture limestone_loss_on_ignition slag_sulfur "
                   "top_CO2 top_CO top_H2 top_N2 ore_moisture").split()
    for name in percentages:
        if data[name] > 100:
            raise ValueError(f"{name}: процент должен быть в диапазоне 0–100.")
    if data["rd"] > 1:
        raise ValueError("rd: доля должна быть в диапазоне 0–1.")
    for names in (("Si", "Mn", "S", "P", "Ti", "Cr", "V", "C"),
                  ("coke_ash", "coke_sulfur", "coke_volatiles"),
                  ("gas_CH4", "gas_C2H6", "gas_CO2"), ("top_CO2", "top_CO", "top_H2", "top_N2")):
        if sum(data[name] for name in names) > 100.001:
            raise ValueError(f"{', '.join(names)}: сумма долей превышает 100%.")
    # The original final-coefficient formulas are undefined at these zeros.
    for name in ("rd", "gas_consumption", "blast_humidity", "slag_rate", "oxygen_content", "coke_rate"):
        if data[name] == 0:
            raise ValueError(f"{name}: для существующих формул значение должно быть больше нуля.")
    if data["top_CO2"] + data["top_CO"] == 0:
        raise ValueError("top_CO2 + top_CO: сумма должна быть больше нуля.")
