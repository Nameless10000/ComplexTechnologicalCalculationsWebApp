using AutoMapper;
using BaseLib.SlagMode.Models;
using Core.Contexts;
using Core.Models;
using Core.Models.SlagMode;
using Microsoft.EntityFrameworkCore;

namespace Web.Seed;

public static class SlagModeDefaultPresetSeeder
{
    public static async Task SeedAsync(SlagModeDBContext dbContext, IMapper mapper)
    {
        var presetExists = await dbContext.Responses
            .AnyAsync(x => x.CreatorID == 0);

        if (presetExists)
        {
            return;
        }

        var requestData = CreateDefaultRequestData();
        var responseData = CreateDefaultResponseData();
        var request = mapper.Map<Request>(requestData);
        var response = mapper.Map<Response>(responseData);
        var createdAt = DateTime.UtcNow;

        response.Request = request;

        ApplyAudit(request, createdAt);
        ApplyAudit(response, createdAt);
        ApplyAudit(request.CastIron, createdAt);
        ApplyAudit(request.InputCoke, createdAt);
        ApplyAudit(request.Slag, createdAt);

        foreach (var component in request.Components)
        {
            ApplyAudit(component, createdAt);
        }

        dbContext.Responses.Add(response);
        await dbContext.SaveChangesAsync();
    }

    private static void ApplyAudit(Entity entity, DateTime createdAt)
    {
        entity.CreatorID = 0;
        entity.CreationDateTime = createdAt;
    }

    private static RequestData CreateDefaultRequestData()
    {
        return new RequestData
        {
            User = new UserAuthData
            {
                UserName = "Login",
                Password = "Password"
            },
            Iron = new InputCastIronForCalc
            {
                Si = 0.512,
                S = 0.016,
                Mn = 0.2,
                C = 4.702,
                Ti = 0.01,
                Cr = 0,
                Temp = 1450
            },
            Slag = new InputSlagForCalc
            {
                CaO = 40.9,
                SiO2 = 36.56,
                TiO2 = 0.01,
                Al2O3 = 11.43,
                MgO = 7.392
            },
            Coke = new InputCokeForCalcs
            {
                Consumption = 419.8,
                Sulfur = 0.428,
                AshAmount = 12.7,
                AshCaOFraction = 7.8,
                AshSiO2Fraction = 48.1,
                AshAl2O3Fraction = 24.6,
                AshMgOFraction = 2
            },
            Components =
            [
                new InputChargeComponentsForCalc
                {
                    Sourcename = "Agglomerate23",
                    Consumption = 441.5,
                    Fe = 58.5,
                    SiO2 = 5.88,
                    Al2O3 = 1.75,
                    CaO = 8.72,
                    MgO = 1.63,
                    S = 0.028,
                    MnO = 0.19,
                    TiO2 = 0.24,
                },
                new InputChargeComponentsForCalc
                {
                    Sourcename = "Agglomerate4",
                    Consumption = 485.9,
                    Fe = 58.3,
                    SiO2 = 5.95,
                    Al2O3 = 1.76,
                    CaO = 8.86,
                    MgO = 1.64,
                    S = 0.028,
                    MnO = 0.19,
                    TiO2 = 0.24,
                },
                new InputChargeComponentsForCalc
                {
                    Sourcename = "Ssgpo",
                    Consumption = 568.7,
                    Fe = 62.6,
                    SiO2 = 3.7,
                    Al2O3 = 1.21,
                    CaO = 4.02,
                    MgO = 0.99,
                    S = 0.067,
                    MnO = 0.16,
                    TiO2 = 0.32,
                },
                new InputChargeComponentsForCalc
                {
                    Sourcename = "Lebedinskiy",
                    Consumption = 54.7,
                    Fe = 65.7,
                    SiO2 = 5.17,
                    Al2O3 = 0.25,
                    CaO = 0.4,
                    MgO = 0.22,
                    S = 0.01,
                    MnO = 0.05,
                    TiO2 = 0,
                },
                new InputChargeComponentsForCalc
                {
                    Sourcename = "Kachkanarsiy",
                    Consumption = 54.1,
                    Fe = 60.4,
                    SiO2 = 4.36,
                    Al2O3 = 2.59,
                    CaO = 1.28,
                    MgO = 2.9,
                    S = 0.02,
                    MnO = 0.23,
                    TiO2 = 2.66,
                },
                new InputChargeComponentsForCalc
                {
                    Sourcename = "Mixailovskiy",
                    Consumption = 48.5,
                    Fe = 63.3,
                    SiO2 = 7.25,
                    Al2O3 = 0.23,
                    CaO = 1.49,
                    MgO = 0.25,
                    S = 0.01,
                    MnO = 0.04,
                    TiO2 = 0,
                }
            ]
        };
    }

    private static ResponseData CreateDefaultResponseData()
    {
        return new ResponseData
        {
            SlagBasicity1 = 1.1187089715536105,
            SlagBasicity2 = 1.3208971553610502,
            SlagBasicity3 = 1.0062929777036882,
            SlagBasicityKulikov = 1.2446003148807527,
            SlagOut = 263.9732909819013,
            MaterialCons = 1568.6164909614838,
            TotalAglo = 927.4,
            PropAglo23 = 0.26702552316438855,
            PropAglo4 = 0.29387927906132816,
            PropSsgpo = 0.3439579049231886,
            PropLeb = 0.03308334341357204,
            PropKach = 0.03272045482037015,
            PropMix = 0.029333494617152536,
            PropOre = 0,
            PropWeldSlag = 0,
            PropBfAddict = 0,
            PropMinInclude = 0,
            Viscosity_1400 = 6.703502526969734,
            Viscosity_1450 = 4.030430354464176,
            Viscosity_1500 = 2.77640269624368,
            Viscosity_1550 = 2.1129864804405654,
            Temp_7_Puaz = 1396.3855818462755,
            Gradient_7_25 = 0.2225587107350592,
            Gradient_1400_1500 = 0.03927099830726054,
            SlagTemperature = 1488.3385600000001,
            SlagTemperature_25Puaz = 1315.5080464734733,
            CurrSlagViscosity = 2.9982111721413673,
            BalSlagMass = 263.9732909819013,
            CaOBalSlagMass = 256.15142487066544,
            TotalSInOre = 2.4246469315316554,
            SActivity = 5.060329763376613,
            SDistribution = 0,
            SContentInCastIron = 0.016,
            CastIronTemp = 1450
        };
    }
}
