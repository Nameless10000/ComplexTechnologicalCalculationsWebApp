from app.models import BlastFurnaceInput


class IntermediateCalculations:
    def __init__(self, bf_input: BlastFurnaceInput):
        self.d = bf_input

    # C5
    def Fe_content(self) -> float:
        """Содержание Fe в чугуне [%]."""
        return 100 - (
            self.d.Si +
            self.d.Mn +
            self.d.S +
            self.d.P +
            self.d.Ti +
            self.d.Cr +
            self.d.V +
            self.d.C
        )

    # C6
    def C_direct_Fe(self) -> float:
        """Расход C на прямое восстановление Fe [кг/т чугуна]."""
        return self.Fe_content() * 10 * self.d.rd * 12 / 56

    # C7
    def C_direct_impurities(self) -> float:
        """Расход C на прямое восстановление примесей чугуна [кг/т]."""
        return 10 * (
            self.d.Mn * 12 / 55 +
            self.d.P * 60 / 62 +
            self.d.Si * 24 / 28 +
            self.d.S * 12 / 32 +
            self.d.V * 60 / 110 +
            self.d.Ti * 12 / 48 +
            self.d.Cr * 48 / 104
        )

    # C8
    def nonvolatile_in_coke(self) -> float:
        """Количество нелетучих элементов в коксе [%]."""
        return 100 - (
            self.d.coke_ash +
            self.d.coke_sulfur +
            self.d.coke_volatiles
        )

    # C9
    def C_from_coke(self) -> float:
        """Количество углерода, пришедшего в печь с коксом [кг/т]."""
        return 0.01 * self.d.coke_rate * self.nonvolatile_in_coke()

    # C10
    def C_methane(self) -> float:
        """Расход C на образование метана [кг/т]."""
        return 0.008 * self.C_from_coke()

    # C11
    def C_in_iron(self) -> float:
        """Растворяется углерода в чугуне [кг/т]."""
        return 10 * self.d.C

    # C12
    def C_burned(self) -> float:
        """Количество C, сгорающего у фурм [кг/т]."""
        return self.C_from_coke() - (
            self.C_in_iron() +
            self.C_direct_Fe() +
            self.C_direct_impurities() +
            self.C_methane()
        )

    # C13
    def dry_blast_per_kg_C(self) -> float:
        """Расход сухого дутья на 1 кг C кокса [м3/кг Cf]."""
        return 0.9333 / (
            0.01 * self.d.oxygen_content +
            0.00062 * self.d.blast_humidity
        )

    # C14
    def dry_blast_for_gas(self) -> float:
        """Расход сухого дутья для конверсии 1 м3 природного газа [м3/м3]."""
        return 0.5 / (
            0.01 * self.d.oxygen_content +
            0.00062 * self.d.blast_humidity
        )

    # C15
    def gas_consumption_per_C(self) -> float:
        """Расход природного газа на 1 кг углерода кокса."""
        return self.d.gas_consumption / self.C_burned()

    # C16
    def total_dry_blast(self) -> float:
        """Суммарный расход сухого дутья [м3/кг Cf]."""
        return (
            self.dry_blast_per_kg_C() +
            self.dry_blast_for_gas() *
            self.gas_consumption_per_C()
        )

    # C17
    def specific_blast_rate(self) -> float:
        """Расчетный удельный расход дутья [м3/т чугуна]."""
        return self.total_dry_blast() * self.C_burned()

    # C18
    def CO_blast_gas(self) -> float:
        """Состав CO в горновом газе [м3/кг Cf]."""
        return (
            1.8667 +
            (self.d.gas_consumption / self.C_burned()) *
            self.d.gas_C_CH4
        )

    # C19
    def H2_blast_gas(self) -> float:
        """Состав H2 в горновом газе [м3/кг Cf]."""
        tmp = (
            0.9333 +
            0.5 *
            (self.d.gas_consumption / self.C_burned()) *
            1
        )

        return (
            tmp /
            (
                0.01 * self.d.oxygen_content +
                0.00124 * self.d.blast_humidity
            ) *
            0.00124 *
            self.d.blast_humidity
            +
            (self.d.gas_consumption / self.C_burned()) *
            self.d.gas_H2_CH4
        )

    # C20
    def N2_blast_gas(self) -> float:
        """Состав N2 в горновом газе [м3/кг Cf]."""
        tmp = (
            0.9333 +
            0.5 *
            (self.d.gas_consumption / self.C_burned()) *
            1
        )

        return (
            tmp /
            (
                0.01 * self.d.oxygen_content +
                0.00124 * self.d.blast_humidity
            ) *
            (1 - 0.01 * self.d.oxygen_content)
        )


    # C21
    def CO_from_oxides(self) -> float:
        """CO, образующийся при восстановлении оксидов [м3/т]."""

        Fe = self.Fe_content()
        rd = self.d.rd
        Mn = self.d.Mn
        slag_sulfur = self.d.slag_sulfur

        return 10 * 22.4 * (
            Fe * rd / 56 +
            Mn / 55 +
            2 * self.d.Si / 28 +
            slag_sulfur / 32
        )

    # C22
    def CO_usage_degree(self) -> float:
        """Степень использования CO в печи."""
        return self.d.top_CO2 / (
            self.d.top_CO2 +
            self.d.top_CO
        )

    # C23
    def H2_usage_degree(self) -> float:
        """Степень использования H2 в печи."""
        return 0.88 * self.CO_usage_degree() + 0.1


    # C24
    def CO_volume_1000(self) -> float:
        return (
            self.CO_blast_gas() *
            self.C_burned() +
            self.CO_from_oxides()
        )

    # C25
    def H2_volume_1000(self) -> float:
        return (
            self.H2_blast_gas() *
            self.C_burned() *
            (1 - self.H2_usage_degree())
        )

    # C26
    def N2_volume_1000(self) -> float:
        return self.N2_blast_gas() * self.C_burned()

    # C27
    def CO2_limestone(self) -> float:
        """Объём CO2 при разложении известняка [м3/т]."""
        return (
            0.01 *
            self.d.limestone_rate *
            22.4 /
            44 *
            self.d.limestone_loss_on_ignition
        )

    # C28
    def CO2_indirect_reduction(self) -> float:
        """Объём CO2 при косвенном восстановлении оксидов железа [м3/т]."""
        return (
            self.CO_volume_1000() *
            self.CO_usage_degree()
        )


    # C29
    def top_gas_CO2(self) -> float:
        return (
            self.CO2_indirect_reduction() +
            self.CO2_limestone()
        )

    # C30
    def top_gas_CO(self) -> float:
        return (
            self.CO_volume_1000() -
            self.CO2_indirect_reduction()
        )

    # C31
    def top_gas_CH4(self) -> float:
        return self.C_methane() * 22.4 / 12

    # C32
    def top_gas_N2(self) -> float:
        return self.N2_volume_1000()

    # C33
    def top_gas_H2(self) -> float:
        return self.H2_volume_1000()

    # C34
    def top_gas_total(self) -> float:
        return (
            self.top_gas_CO2() +
            self.top_gas_CO() +
            self.top_gas_CH4() +
            self.top_gas_N2() +
            self.top_gas_H2()
        )