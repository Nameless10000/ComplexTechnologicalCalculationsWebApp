from app.models import BlastFurnaceInput
from app.intermediate import IntermediateCalculations


class HeatBalanceFull:
    def __init__(
        self,
        bf_input: BlastFurnaceInput,
        intermediate: IntermediateCalculations
    ):
        self.bf_input = bf_input
        self.calc = intermediate
        self.values = {}

        self._calculate_incoming()
        self._calculate_consumption()
        self._calculate_additional()
        self._calculate_final()

    def _calculate_incoming(self):
        bf_input = self.bf_input
        calc = self.calc

        self.values["C4"] = (
            calc.C_burned() *
            9800 *
            0.001
        )

        self.values["C6"] = (
            1.2897 +
            0.000121 *
            bf_input.hot_blast_temp
        )

        self.values["C7"] = (
            1.2897 +
            0.000121 *
            bf_input.hot_blast_temp
        )

        self.values["C8"] = (
            1.456 +
            0.000282 *
            bf_input.hot_blast_temp
        )

        self.values["C9"] = (
            0.001 *
            calc.specific_blast_rate() *
            (
                (
                    0.01 *
                    bf_input.oxygen_content *
                    self.values["C6"]
                    +
                    (
                        1 -
                        0.01 *
                        bf_input.oxygen_content
                    ) *
                    self.values["C7"]
                )
                *
                (
                    1 -
                    0.00124 *
                    bf_input.blast_humidity
                )
                +
                0.00124 *
                bf_input.blast_humidity *
                self.values["C8"]
            )
            *
            bf_input.hot_blast_temp
        )

        self.values["C11"] = (
            0.001 *
            bf_input.gas_consumption *
            (
                0.01 *
                (
                    1657 *
                    bf_input.gas_CH4
                    +
                    6046 *
                    bf_input.gas_C2H6
                    -
                    12644 *
                    bf_input.gas_CO2
                )
            )
        )

        self.values["C13"] = (
            1128 *
            0.00001 *
            bf_input.limestone_rate *
            bf_input.limestone_loss_on_ignition
        )

        self.values["C15"] = (
            self.values["C4"] +
            self.values["C9"] +
            self.values["C11"] +
            self.values["C13"]
        )

        self.values["C5"] = (
            self.values["C4"] /
            self.values["C15"]
        )

        self.values["C10"] = (
            self.values["C9"] /
            self.values["C15"]
        )

        self.values["C12"] = (
            self.values["C11"] /
            self.values["C15"]
        )

        self.values["C14"] = (
            self.values["C13"] /
            self.values["C15"]
        )

        self.values["C16"] = (
            self.values["C5"] +
            self.values["C10"] +
            self.values["C12"] +
            self.values["C14"]
        )

    def _calculate_consumption(self):
        bf_input = self.bf_input
        calc = self.calc

        self.values["C19"] = (
            0.01 *
            calc.Fe_content() *
            bf_input.rd *
            2716
        )

        self.values["C21"] = (
            0.01 *
            (
                5220 * bf_input.Mn +
                22600 * bf_input.Si +
                15490 * bf_input.P +
                36167 * bf_input.Ti +
                7982 * bf_input.V
            )
        )

        self.values["C23"] = (
            1734 *
            0.00001 *
            bf_input.slag_rate *
            bf_input.slag_sulfur
        )

        self.values["C25"] = (
            1731 *
            0.0001 *
            (
                0.00124 *
                bf_input.blast_humidity *
                calc.specific_blast_rate()
                +
                0.01 *
                bf_input.gas_consumption *
                (
                    2 * bf_input.gas_CH4 +
                    3 * bf_input.gas_C2H6
                )
            )
            *
            calc.H2_usage_degree()
        )

        self.values["C27"] = (
            bf_input.C_iron *
            bf_input.T_iron
        )

        self.values["C29"] = (
            0.001 *
            bf_input.slag_rate *
            bf_input.slag_heat_capacity *
            (
                bf_input.T_iron +
                50
            )
        )

        self.values["C31"] = (
            1.24 *
            0.0000001 *
            calc.specific_blast_rate() *
            bf_input.blast_humidity *
            6912
        )

        self.values["C33"] = (
            4042 *
            0.000001 *
            bf_input.limestone_rate *
            bf_input.limestone_loss_on_ignition
        )

        self.values["C35"] = (
            2452 *
            0.00001 *
            (
                bf_input.ore_rate *
                bf_input.ore_moisture
                +
                bf_input.limestone_rate *
                bf_input.limestone_moisture
                +
                bf_input.coke_rate *
                bf_input.coke_moisture
            )
        )

        self.values["C37"] = (
            1.2938 +
            0.0000895 *
            bf_input.top_gas_temp
        )

        self.values["C38"] = (
            1.6448 +
            0.0007065 *
            bf_input.top_gas_temp
        )

        self.values["C39"] = 1.3012

        self.values["C40"] = (
            1.4743 +
            0.0002205 *
            bf_input.top_gas_temp
        )

        self.values["C41"] = 1.308

        self.values["C42"] = (
            0.00001 *
            (
                (
                    bf_input.top_CO2 *
                    self.values["C38"]
                    +
                    bf_input.top_CO *
                    self.values["C37"]
                    +
                    bf_input.top_N2 *
                    self.values["C41"]
                    +
                    bf_input.top_H2 *
                    self.values["C39"]
                )
                *
                calc.top_gas_total()
                +
                (
                    bf_input.ore_rate *
                    bf_input.ore_moisture
                    +
                    bf_input.limestone_rate *
                    bf_input.limestone_moisture
                    +
                    bf_input.coke_rate *
                    bf_input.coke_moisture
                    +
                    calc.top_gas_total() *
                    bf_input.top_H2 *
                    calc.H2_usage_degree() /
                    (
                        1 -
                        calc.H2_usage_degree()
                    )
                )
                *
                self.values["C40"]
            )
            *
            bf_input.top_gas_temp
        )

        self.values["C44"] = (
            self.values["C15"]
            -
            (
                self.values["C19"] +
                self.values["C21"] +
                self.values["C23"] +
                self.values["C25"] +
                self.values["C27"] +
                self.values["C29"] +
                self.values["C31"] +
                self.values["C33"] +
                self.values["C35"] +
                self.values["C42"]
            )
        )

        self.values["C46"] = sum([
            self.values["C19"],
            self.values["C21"],
            self.values["C23"],
            self.values["C25"],
            self.values["C27"],
            self.values["C29"],
            self.values["C31"],
            self.values["C33"],
            self.values["C35"],
            self.values["C42"],
            self.values["C44"],
        ])

        self.values["C43"] = (
            self.values["C42"] /
            self.values["C46"]
        )

        self.values["C45"] = (
            self.values["C44"] /
            self.values["C46"]
        )

    def _calculate_additional(self):

        self.values["C20"] = (
            self.values["C19"] /
            self.values["C46"]
        )

        self.values["C22"] = (
            self.values["C21"] /
            self.values["C46"]
        )

        self.values["C24"] = (
            self.values["C23"] /
            self.values["C46"]
        )

        self.values["C26"] = (
            self.values["C25"] /
            self.values["C46"]
        )

        self.values["C28"] = (
            self.values["C27"] /
            self.values["C46"]
        )

        self.values["C30"] = (
            self.values["C29"] /
            self.values["C46"]
        )

        self.values["C32"] = (
            self.values["C31"] /
            self.values["C46"]
        )

        self.values["C34"] = (
            self.values["C33"] /
            self.values["C46"]
        )

        self.values["C36"] = (
            self.values["C35"] /
            self.values["C46"]
        )

        self.values["C47"] = sum([
            self.values["C20"],
            self.values["C22"],
            self.values["C24"],
            self.values["C26"],
            self.values["C28"],
            self.values["C30"],
            self.values["C32"],
            self.values["C34"],
            self.values["C36"],
            self.values["C43"],
            self.values["C45"],
        ])

    def _calculate_final(self):
        bf_input = self.bf_input
        calc = self.calc

        self.values["C50"] = (
            self.values["C4"] +
            self.values["C9"] -
            self.values["C42"]
        )

        self.values["C51"] = (
            self.values["C50"] /
            (
                calc.C_burned() *
                0.001
            )
        )

        self.values["C52"] = (
            100 -
            bf_input.rd -
            calc.C_direct_Fe() -
            bf_input.gas_consumption -
            bf_input.limestone_rate
        )

        self.values["C55"] = (
            0.01 *
            self.values["C19"] /
            bf_input.rd
        )

        self.values["C56"] = (
            self.values["C55"] *
            1000
        ) / (
            self.values["C51"] *
            0.01 *
            self.values["C52"]
        )

        self.values["C59"] = (
            0
            if bf_input.limestone_rate == 0
            else
            self.values["C45"] /
            bf_input.limestone_rate *
            10
        )

        self.values["C60"] = (
            self.values["C59"] *
            1000
        ) / (
            self.values["C51"] *
            0.01 *
            self.values["C52"]
        )

        self.values["C61"] = (
            self.values["C60"] *
            bf_input.limestone_rate /
            10
        )

        self.values["C64"] = (
            self.values["C9"] /
            bf_input.oxygen_content *
            10
        )

        self.values["C65"] = (
            self.values["C64"] *
            1000
        ) / (
            self.values["C51"] *
            0.01 *
            self.values["C52"]
        )

        self.values["C68"] = (
            self.values["C11"] /
            bf_input.gas_consumption *
            10
        )

        self.values["C69"] = (
            self.values["C68"] *
            1000
        ) / (
            self.values["C51"] *
            0.01 *
            self.values["C52"]
        )

        self.values["C72"] = (
            self.values["C31"] /
            bf_input.blast_humidity
        )

        self.values["C73"] = (
            self.values["C72"] *
            1000
        ) / (
            self.values["C51"] *
            0.01 *
            self.values["C52"]
        )

        self.values["C76"] = (
            self.values["C29"] /
            bf_input.slag_rate *
            10
        )

        self.values["C77"] = (
            self.values["C76"] *
            1000
        ) / (
            self.values["C51"] *
            0.01 *
            self.values["C52"]
        )

        self.values["C80"] = (
            self.values["C15"] /
            100
        )

        self.values["C81"] = (
            self.values["C80"] *
            1000
        ) / (
            self.values["C51"] *
            0.01 *
            self.values["C52"]
        )

    def get_balance(self) -> dict:
        """Возвращает словарь всех расчетных Cxx."""
        return self.values