from dataclasses import dataclass


@dataclass
class BlastFurnaceInput:
    Si: float
    Mn: float
    S: float
    P: float
    Ti: float
    Cr: float
    V: float
    C: float

    T_iron: float
    C_iron: float

    rd: float

    coke_rate: float
    coke_ash: float
    coke_sulfur: float
    coke_volatiles: float
    coke_moisture: float

    hot_blast_temp: float
    blast_humidity: float
    oxygen_content: float

    gas_consumption: float
    gas_CH4: float
    gas_C2H6: float
    gas_CO2: float
    gas_C_CH4: float
    gas_H2_CH4: float

    limestone_rate: float
    limestone_moisture: float
    limestone_loss_on_ignition: float

    slag_rate: float
    slag_sulfur: float
    slag_heat_capacity: float

    top_gas_temp: float
    top_CO2: float
    top_CO: float
    top_H2: float
    top_N2: float

    ore_rate: float
    pellets_rate: float
    ore_moisture: float