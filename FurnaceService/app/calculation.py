from app.models import BlastFurnaceInput
from app.intermediate import IntermediateCalculations
from app.heat_balance import HeatBalanceFull


def calculate(data: dict) -> dict:
    furnace_input = BlastFurnaceInput(**data)

    intermediate = IntermediateCalculations(
        furnace_input
    )

    heat_balance = HeatBalanceFull(
        furnace_input,
        intermediate
    )

    return {
        "heat_balance": heat_balance.get_balance()
    }