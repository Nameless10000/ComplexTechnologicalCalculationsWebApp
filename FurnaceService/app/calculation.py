from app.models import BlastFurnaceInput
from app.intermediate import IntermediateCalculations
from app.heat_balance import HeatBalanceFull
from app.validation import validate
import math


def calculate(data: dict) -> dict:
    validate(data)
    furnace_input = BlastFurnaceInput(**data)

    intermediate = IntermediateCalculations(
        furnace_input
    )
    if intermediate.C_burned() <= 0:
        raise ValueError("Расход углерода у фурм должен быть больше нуля.")

    heat_balance = HeatBalanceFull(
        furnace_input,
        intermediate
    )

    if not all(math.isfinite(value) for value in heat_balance.get_balance().values()):
        raise ValueError("Результат содержит неконечные числа.")
    return {
        "heat_balance": heat_balance.get_balance()
    }
