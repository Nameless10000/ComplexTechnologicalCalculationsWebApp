using BaseLib.Models2;
using BaseLib.Models2.Aglom;
using BaseLib.Models2.Base.Inputs;
using Core.Contexts;
using Core.Models.GasDynamic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Web.Seed;

public static class GasDynamicDefaultPresetSeeder
{
    public static async Task SeedAsync(GasDynamicDBContext dbContext)
    {
        var presetExists = await dbContext.CalculationModels
            .AnyAsync(x => x.OwnerId == 0 && x.IsPreset);

        if (presetExists)
        {
            return;
        }

        var requestModel = CreateDefaultRequestModel();

        dbContext.CalculationModels.Add(new CalculationModel
        {
            SerializedInput = JsonConvert.SerializeObject(requestModel),
            SerializedOutput = JsonConvert.SerializeObject(new ResponseModelV2()),
            OwnerId = 0,
            CreatorID = 0,
            CreationDateTime = DateTime.UtcNow,
            IsPreset = true
        });

        await dbContext.SaveChangesAsync();
    }

    private static RequestModelV2 CreateDefaultRequestModel()
    {
        return new RequestModelV2
        {
            AglomInput = new AglomInputModel
            {
                KoksContents =
                [
                    new KoksContent { MinFractionSize = 80, FractionPercentage = 19.5 },
                    new KoksContent { MinFractionSize = 60, FractionPercentage = 42 },
                    new KoksContent { MinFractionSize = 40, FractionPercentage = 30 },
                    new KoksContent { MinFractionSize = 25, FractionPercentage = 6 },
                    new KoksContent { MinFractionSize = 0, FractionPercentage = 2.5 }
                ],
                AglomContents =
                [
                    new AglomContent { MinFractionSize = 50, FractionPercentage = 2.1, Porosity = 0.485 },
                    new AglomContent { MinFractionSize = 25, FractionPercentage = 5.7, Porosity = 0.457 },
                    new AglomContent { MinFractionSize = 10, FractionPercentage = 35.5, Porosity = 0.45 },
                    new AglomContent { MinFractionSize = 5, FractionPercentage = 43.1, Porosity = 0.45 },
                    new AglomContent { MinFractionSize = 0, FractionPercentage = 13.6, Porosity = 0.344 },
                ],
                OkatContents =
                [
                    new OkatContent { MinFractionSize = 50, FractionPercentage = 2.1, Porosity = 0.485 },
                    new OkatContent { MinFractionSize = 25, FractionPercentage = 5.7, Porosity = 0.457 },
                    new OkatContent { MinFractionSize = 10, FractionPercentage = 15, Porosity = 0.45 },
                    new OkatContent { MinFractionSize = 5, FractionPercentage = 56, Porosity = 0.45 },
                    new OkatContent { MinFractionSize = 0, FractionPercentage = 21.2, Porosity = 0.344 },
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
    }
}
