using System.Diagnostics;
using BaseLib;
using BaseLib.Models2;
using BaseLib.Models2.Aglom;
using BaseLib.Models2.Base.Inputs;
using Core.Contexts;
using Newtonsoft.Json;

namespace Test;

/// <summary>
/// Тесты для либы BlastFurnaceSmeltingGasDynamicMode
/// </summary>
public class BlastFurnaceSmeltingGasDynamicModeTest
{
    private readonly BlastFurnaceSmeltingGasDynamicModeXLLibrary _library;
    private readonly TBalDBContext _dbContext;

    // Пример получения сервисов/репо/контекстов в тестовую среду
    public BlastFurnaceSmeltingGasDynamicModeTest(TBalDBContext dbContext, BlastFurnaceSmeltingGasDynamicModeXLLibrary library)
    {
        _library = library;
    }
    
    [Fact]
    public void Test1()
    {
        var aglomInputData = new RequestModelV2
        {
            AglomInput = new AglomInputModel
            {
                KoksContents =
                [
                    new KoksContent
                    {
                        MinFractionSize = 80,
                        FractionPercentage = 19.5
                    },
                    new KoksContent
                    {
                        MinFractionSize = 60,
                        FractionPercentage = 42
                    },
                    new KoksContent
                    {
                        MinFractionSize = 40,
                        FractionPercentage = 30
                    },
                    new KoksContent
                    {
                        MinFractionSize = 25,
                        FractionPercentage = 6
                    },
                    new KoksContent
                    {
                        MinFractionSize = 0,
                        FractionPercentage = 2.5
                    }
                ],
                AglomContents = 
                [
                    new AglomContent
                    {
                        MinFractionSize = 50,
                        FractionPercentage = 2.1,
                        Porosity = 0.485,
                    },
                    new AglomContent
                    {
                        MinFractionSize = 25,
                        FractionPercentage = 5.7,
                        Porosity = 0.457,
                    },
                    new AglomContent
                    {
                        MinFractionSize = 10,
                        FractionPercentage = 35.5,
                        Porosity = 0.45,
                    },
                    new AglomContent
                    {
                        MinFractionSize = 5,
                        FractionPercentage = 43.1,
                        Porosity = 0.45,
                    },
                    new AglomContent
                    {
                        MinFractionSize = 0,
                        FractionPercentage = 13.6,
                        Porosity = 0.344,
                    },
                ],
                OkatContents = 
                [
                    new OkatContent
                    {
                        MinFractionSize = 50,
                        FractionPercentage = 2.1,
                        Porosity = 0.485,
                    },
                    new OkatContent
                    {
                        MinFractionSize = 25,
                        FractionPercentage = 5.7,
                        Porosity = 0.457,
                    },
                    new OkatContent
                    {
                        MinFractionSize = 10,
                        FractionPercentage = 15,
                        Porosity = 0.45,
                    },
                    new OkatContent
                    {
                        MinFractionSize = 5,
                        FractionPercentage = 56,
                        Porosity = 0.45,
                    },
                    new OkatContent
                    {
                        MinFractionSize = 0,
                        FractionPercentage = 21.2,
                        Porosity = 0.344,
                    },
                ]
            },
            BlastFurnaceInput = new BlastFurnaceInputModel
            {
                Composition = new CompositionParameters
                {
                    Fe_chugun = 94,
                    Mn_chugun = 0.19,
                    P_chugun = 0.043,
                    Si_chugun = 0.53,
                    S_shlak = 1.03,
                    C_chugun = 4.7
                },
                FuelAndBlast = new FuelAndBlastParameters
                {
                    Udeln_koks = 420,
                    C_neletuch = 87,
                    Stepen_pryamogo_vost = 0.35,
                    Kislorod_dut = 24,
                    Vlazhn_dut = 6.36,
                    Udeln_prir_gaz = 105,
                    C_prir_gaz = 1,
                    H2_prir_gaz = 2,
                    Stepen_vodorod = 0.4,
                    Stepen_CO = 0.42,
                    Rashod_dut = 2997,
                    Poteri_dut = 10
                },
                Production = new ProductionParameters
                {
                    Proizvodit_chugun = 3430,
                    Udeln_zhelezorud = 1.594,
                    Udeln_izvest = 0,
                    Stepen_urav_krit = 57,
                    Dolya_okat = 0
                },
                Geometry = new FurnaceGeometry
                {
                    Diam_gorn = 7.2,
                    Diam_raspar = 8.2,
                    Diam_koloshnik = 5.8,
                    Height_zaplechik = 3,
                    Height_shahta = 16,
                    Height_koloshnik = 2.8,
                    Uroven_zasypi = 1.7,
                    Height_raspar = 2,
                    Kolvo_furm = 16,
                    Diam_furm = 170,
                    Dlina_furm = 400
                },
                ThermalAndPressure = new ThermalAndPressureParameters
                {
                    Temp_dut = 1140,
                    Teploemk_koks = 1.65,
                    Temp_koks = 1500,
                    Teplota_nepoln_koks = 9800,
                    Teplota_nepoln_prir_gaz = 1590,
                    Davlen_izb_dut = 2.79,
                    Davlen_izb_koloshnik_gaz = 1.32,
                    Temp_koloshnik_gaz = 300,
                    Perepad_niz = 1.152,
                    Perepad_verh = 0.414
                },
                Materials = new MaterialProperties
                {
                    Udeln_vyhod_shlak = 260,
                    Plotn_shlak = 2400,
                    Massa_koks_kg = 450,
                    Massa_aglo = 1.7,
                    Massa_okat = 2,
                    Porozn_aglo = 0.35,
                    Porozn_okat = 0.46,
                    Poteri_prokalivanie = 50
                }
            }
        };

        var response = _library.Calculate(aglomInputData);
        Debug.WriteLine(JsonConvert.SerializeObject(response));
        
        Assert.Equal(0.652, response.AglomOutput.AglomPorosity, 3);
        Assert.Equal(0.71, response.AglomOutput.OkatPorosity, 3);

        var blastResult = response.BlastFurnanceOutputModel;
        
        Assert.Equal(46.543, blastResult.FurnaceGeometry.S_Sech_Niz, 0);
        Assert.Equal(1.594, blastResult.MaterialConsumption.Udeln_Aglo, 3);
        Assert.Equal(0.0, blastResult.MaterialConsumption.Udeln_Okat, 0);
        Assert.Equal(0.0, blastResult.MaterialConsumption.Udeln_Izvest, 0);
        Assert.Equal(365.4, blastResult.CarbonBalance.C_Input, 1);
        Assert.Equal(79.736, blastResult.CarbonBalance.C_Out_Vosstan, 3);
        Assert.Equal(47.0, blastResult.CarbonBalance.C_Out_Chugun, 0);
        Assert.Equal(2.923, blastResult.CarbonBalance.C_Out_Metan, 3);
        Assert.Equal(235.741, blastResult.CarbonBalance.C_Out_Furm, 3);
        Assert.Equal(3.825, blastResult.BlastParameters.Rashod_Dut_Koks, 3);
        Assert.Equal(2.049, blastResult.BlastParameters.Rashod_Dut_Prir_Gaz, 3);
        Assert.Equal(4.738, blastResult.BlastParameters.Rashod_Dut_Sum, 3);
        Assert.Equal(1116.841, blastResult.BlastParameters.Rashod_Dut_Udeln, 3);
        Assert.Equal(4.804, blastResult.HearthGas.Furmgaz_Koks, 3);
        Assert.Equal(4.573, blastResult.HearthGas.Furmgaz_Prir_Gaz, 3);
        Assert.Equal(6.841, blastResult.HearthGas.Furmgaz_Sum, 3);
        Assert.Equal(1612.664, blastResult.HearthGas.Furmgaz_Udeln, 3);
        Assert.Equal(2.312, blastResult.HearthGas.Furmgaz_CO_V, 3);
        Assert.Equal(0.928, blastResult.HearthGas.Furmgaz_H2_V, 3);
        Assert.Equal(3.544, blastResult.HearthGas.Furmgaz_N2_V, 3);
        Assert.Equal(0.341, blastResult.HearthGas.Furmgaz_CO, 3);
        Assert.Equal(0.137, blastResult.HearthGas.Furmgaz_H2, 3);
        Assert.Equal(0.522, blastResult.HearthGas.Furmgaz_N2, 3);
        Assert.Equal(148.064, blastResult.IntermediateGas1000.Volume_CO_Pvost, 3);
        Assert.Equal(693.121, blastResult.IntermediateGas1000.Volume_CO_1000, 3);
        Assert.Equal(131.202, blastResult.IntermediateGas1000.Volume_H2_1000, 3);
        Assert.Equal(835.515, blastResult.IntermediateGas1000.Volume_N2_1000, 3);
        Assert.Equal(1659.838, blastResult.IntermediateGas1000.Volume_Sum_1000, 3);
        Assert.Equal(0.418, blastResult.IntermediateGas1000.Domengaz_CO_1000, 3);
        Assert.Equal(0.079, blastResult.IntermediateGas1000.Domengaz_H2_1000, 3);
        Assert.Equal(0.503, blastResult.IntermediateGas1000.Domengaz_N2_1000, 3);
        Assert.Equal(1.158, blastResult.IntermediateGas1000.Domengaz_Plotn_1000, 3);
        Assert.Equal(65.894, blastResult.IntermediateGas1000.Domengaz_Rashod_1000, 3);
        Assert.Equal(7.7, blastResult.FurnaceGeometry.Diam_Niz, 1);
        Assert.Equal(1.416, blastResult.HydrodynamicsLower.Speed_Filtr_Niz, 2);
        Assert.Equal(1.428, blastResult.ThermalParameters.Teploemk_2atom, 3);
        Assert.Equal(1.777, blastResult.ThermalParameters.Teploemk_Voda, 3);
        Assert.Equal(1558.317, blastResult.ThermalParameters.Teplosod_Dut, 3);
        Assert.Equal(2475.0, blastResult.ThermalParameters.Teplosod_Koks, 0);
        Assert.Equal(0.445, blastResult.MaterialConsumption.Rashod_Prir_Gaz, 3);
        Assert.Equal(2977.098, blastResult.HearthGas.Teplosod_Furmgaz, 3);
        Assert.Equal(1984.9, blastResult.HearthGas.Temp_Teor, 1);
        Assert.Equal(1492.45, blastResult.HearthGas.Temp_Sredn_Niz, 2);
        Assert.Equal(2660.254, blastResult.BlastParameters.Rashod_Dut_Minut, 3);
        Assert.Equal(182.398, blastResult.BlastParameters.Speed_Dut_Furm, 3);
        Assert.Equal(0.000211, blastResult.BlastParameters.Vyazkost_Dut, 6); // 2.11e-4 → 6 знаков, чтобы сохранить 3 значащих
        Assert.Equal(146886.0, blastResult.BlastParameters.Reinolds, 0); // 1.5e5 → целое достаточно
        Assert.Equal(0.0164, blastResult.HydrodynamicsLower.Tren_Koef, 4);
        Assert.Equal(0.0180, blastResult.HydrodynamicsLower.Tren_Sum, 4);
        Assert.Equal(0.182, blastResult.HydrodynamicsLower.Poteri_Furm, 3);
        Assert.Equal(0.97, blastResult.HydrodynamicsLower.Perepad_Niz_Itog, 2);
        Assert.Equal(0.701, blastResult.HydrodynamicsLower.Perepad_Niz_Dolya, 3);
        Assert.Equal(1.802, blastResult.HydrodynamicsLower.Davlen_Izb_1000, 3);
        Assert.Equal(3.205, blastResult.HydrodynamicsLower.Davlen_Niz, 3);
        Assert.Equal(5.5, blastResult.FurnaceGeometry.Height_Shihta_Niz, 1);
        Assert.Equal(54.694, blastResult.ChargeAndPacking.Diam_Koks, 3);
        Assert.Equal(0.460, blastResult.ChargeAndPacking.Porozn_Koks, 3);
        Assert.Equal(0.108, blastResult.ChargeAndPacking.Volume_Udeln_Shlak, 3);
        Assert.Equal(0.933, blastResult.ChargeAndPacking.Volume_Udeln_Koks, 3);
        Assert.Equal(0.430, blastResult.ChargeAndPacking.Volume_Udeln_Nasadzka, 3);
        Assert.Equal(0.321, blastResult.ChargeAndPacking.Volume_Udeln_Ost, 3);
        Assert.Equal(0.344, blastResult.ChargeAndPacking.Porozn_Sloy_Korrekt, 3);
        Assert.Equal(0.256, blastResult.HydrodynamicsLower.Koef_Soprot_Niz, 2);
        Assert.Equal(45.482, blastResult.HydrodynamicsLower.Koef_Proporc_Dut_Filtr_Niz, 3);
        Assert.Equal(1.08E-07, blastResult.HydrodynamicsLower.Koef_An, 9); // 2 значащих → 1.08 × 10⁻⁷
        Assert.Equal(0.0, blastResult.TopGas.Volume_CO2_Izvest, 0);
        Assert.Equal(291.111, blastResult.TopGas.Volume_CO2_Kvost, 3);
        Assert.Equal(291.111, blastResult.TopGas.Volume_CO2_Kolgaz, 3);
        Assert.Equal(402.010, blastResult.TopGas.Volume_CO_Kolgaz, 3);
        Assert.Equal(5.457, blastResult.TopGas.Volume_CH4_Kolgaz, 3);
        Assert.Equal(835.515, blastResult.TopGas.Volume_N2_Kolgaz, 3);
        Assert.Equal(131.202, blastResult.TopGas.Volume_H2_Kolgaz, 3);
        Assert.Equal(1665.295, blastResult.TopGas.Udeln_Kolgaz, 3);
        Assert.Equal(0.175, blastResult.TopGas.Kolgaz_CO2, 3);
        Assert.Equal(0.241, blastResult.TopGas.Kolgaz_CO, 3);
        Assert.Equal(0.0788, blastResult.TopGas.Kolgaz_H2, 4);
        Assert.Equal(0.00328, blastResult.TopGas.Kolgaz_CH4, 5);
        Assert.Equal(0.502, blastResult.TopGas.Kolgaz_N2, 3);
        Assert.Equal(1.282, blastResult.TopGas.Kolgaz_Plotn, 3);
        Assert.Equal(66.111, blastResult.TopGas.Kolgaz_Minut, 3);
        Assert.Equal(7.0, blastResult.FurnaceGeometry.Diam_Verh, 1);
        Assert.Equal(1.719, blastResult.HydrodynamicsUpper.Speed_Filtr_Verh, 2);
        Assert.Equal(7.445, blastResult.ChargeAndPacking.Diam_Aglo, 3);
        Assert.Equal(0.337, blastResult.ChargeAndPacking.Porozn_Aglo, 3);
        Assert.Equal(0.366, blastResult.ChargeAndPacking.Porozn_Okat, 3);
        Assert.Equal(0.42, blastResult.MaterialConsumption.Udeln_Koks_1000, 2);
        Assert.Equal(2.014, blastResult.MaterialConsumption.Udeln_Sum, 3);
        Assert.Equal(0.938, blastResult.ChargeAndPacking.Volume_Aglo_1chugun, 3);
        Assert.Equal(0.0, blastResult.ChargeAndPacking.Volume_Okat_1chugun, 0);
        Assert.Equal(0.933, blastResult.ChargeAndPacking.Volume_Koks_1chugun, 3);
        Assert.Equal(1.871, blastResult.ChargeAndPacking.Volume_Sum_1chugun, 3);
        Assert.Equal(1.076, blastResult.ChargeAndPacking.Massa_Nasyp_Shihta, 3);
        Assert.Equal(0.501, blastResult.ChargeAndPacking.Shihta_Dolya_Aglo, 3);
        Assert.Equal(0.0, blastResult.ChargeAndPacking.Shihta_Dolya_Okat, 0);
        Assert.Equal(0.499, blastResult.ChargeAndPacking.Shihta_Dolya_Koks, 3);
        Assert.Equal(0.399, blastResult.ChargeAndPacking.Shihta_Porozn_Verh, 3);
        Assert.Equal(13.082, blastResult.ChargeAndPacking.Shihta_Diam_Verh, 3);
        Assert.Equal(17.1, blastResult.FurnaceGeometry.Shihta_Height_Verh, 1);
        Assert.Equal(650.0, blastResult.ThermalParameters.Temp_Verh, 0);
        Assert.Equal(2.561, blastResult.HydrodynamicsUpper.Davlen_Verh, 3);
        Assert.Equal(0.0133, blastResult.HydrodynamicsUpper.Koef_Soprot_Verh, 3);
        Assert.Equal(45.333, blastResult.HydrodynamicsUpper.Koef_Proporc_Dut_Filtr_Verh, 3);
        Assert.Equal(4.61E-08, blastResult.HydrodynamicsUpper.Koef_Av, 10); // 2 значащих → 4.61 × 10⁻⁸
        Assert.Equal(22.6, blastResult.FurnaceGeometry.Height_Aktiv, 1);
        Assert.Equal(52.945, blastResult.HydrodynamicsUpper.Stepen_Urav, 3);
        Assert.Equal(1.387, blastResult.HydrodynamicsUpper.Perepad_Davlen, 3);
        Assert.Equal(2999.891, blastResult.BlastParameters.Rashod_Dut_Krit, 1);
        Assert.Equal(1.573, blastResult.HydrodynamicsUpper.Speed_Filtr_Gorn, 2);
        Assert.Equal(7.45, blastResult.HydrodynamicsUpper.Speed_Real_Verh, 2);
        Assert.Equal(1.248, blastResult.HydrodynamicsUpper.Speed_Filtr_Raspar, 3);
        Assert.Equal(6.03, blastResult.HydrodynamicsUpper.Speed_Real_Raspar, 2);
        Assert.Equal(2.503, blastResult.HydrodynamicsUpper.Speed_Filtr_Koloshnik, 2);
        Assert.Equal(5.682, blastResult.HydrodynamicsUpper.Speed_Real_Koloshnik, 1);
    }
}