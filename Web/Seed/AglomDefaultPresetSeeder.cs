using AutoMapper;
using BaseLib.AglomMode.Models;
using Console;
using Console.DTO;
using Core.Contexts;
using Core.Models;
using Core.Models.AglomMode;
using Microsoft.EntityFrameworkCore;

namespace Web.Seed;

public static class AglomDefaultPresetSeeder
{
    public static async Task SeedAsync(AgloDBContext dbContext, IMapper mapper)
    {
        var presetExists = await dbContext.AglomRequests
            .AnyAsync(x => x.CreatorID == 0);

        if (presetExists)
        {
            return;
        }

        var request = mapper.Map<AglomRequestDB>(CreateDefaultRequestData());
        var response = mapper.Map<AglomResponseDB>(new AglomResponseData
        {
            Components = CreateDefaultResponseComponents()
        });
        var createdAt = DateTime.UtcNow;

        request.AglomResponse = response;

        ApplyAudit(request, createdAt);
        ApplyAudit(response, createdAt);
        ApplyAudit(request.Cocksick, createdAt);
        ApplyAudit(request.FluxAdditions, createdAt);
        ApplyAudit(request.StartEnter, createdAt);
        ApplyAudit(request.ZolaOfCocksick, createdAt);

        foreach (var component in request.ShihtaComponents)
        {
            ApplyAudit(component, createdAt);
        }

        foreach (var component in response.Components)
        {
            ApplyAudit(component, createdAt);
        }

        dbContext.AglomRequests.Add(request);
        await dbContext.SaveChangesAsync();
    }

    private static void ApplyAudit(Entity entity, DateTime createdAt)
    {
        entity.CreatorID = 0;
        entity.CreationDateTime = createdAt;
    }

    private static AglomRequestData CreateDefaultRequestData()
    {
        return new AglomRequestData
        {
            Cocksick = new Cocksick
            {
                Weight = 4,
                PercentSera = 0.45,
                PercentZola = 17,
                PercentValotiles = 0.8
            },
            StartEnter = new StartEnter
            {
                osnovnost = 1.4,
                DolomyteInAgl = 0,
                FeOinAgl = 12
            },
            FluxAdditions = new FluxAdditions
            {
                DolomyteAl2O3 = 0.04,
                DolomyteCaO = 34.8,
                DolomyteMgO = 18,
                DolomyteSiO2 = 0.13,
                IzvestnyakAl2O3 = 0.05,
                IzvestnyakCaO = 51.8,
                IzvestnyakMgO = 3.2,
                IzvestnyakSiO2 = 0.17
            },
            ZolaOfCocksick = new ZolaOfCocksick
            {
                Fe = 5.4,
                CaO = 7.8,
                SiO2 = 48.1,
                Al2O3 = 24.6,
                MgO = 2,
                P = 0.04
            },
            UserId = 0,
            ShihtaComponents =
            [
                CreateShihtaComponent("к-т ММС-2", 0.0672, 9.8, 1.6, 61, 30, 3.61, 6.6, 0.94, 1.77, 0.21, 1.44, 0.017, 0.09, 0.014, 0.23),
                CreateShihtaComponent("Лебед.к-т", 0.0423, 9.5, 0.28, 68, 26.1, 0.28, 5.24, 0.3, 0.21, 0, 0.05, 0.012, 0.005, 0.01, 0.04),
                CreateShihtaComponent("ССГОК возвр.", 0, 9.8, 3, 61, 0, 4.29, 4.22, 1.12, 1.44, 0.27, 0.3, 0.01, 0.066, 0.066, 0.27),
                CreateShihtaComponent("ССГОК к-т", 0.377, 9, 0.78, 66, 29.7, 1.09, 3.6, 0.92, 1.33, 0.29, 0.418, 0.014, 0.009, 0.014, 0.22),
                CreateShihtaComponent("Стойл.к-т", 0.0613, 8.5, 1.2, 66, 28, 0.35, 6.3, 0.32, 0.3, 0, 0.031, 0.012, 0.007, 0.007, 0.04),
                CreateShihtaComponent("Коршуновский к-т", 0.12, 9.6, 1.2, 63, 23.3, 1.39, 3.1, 3.18, 2.46, 0.28, 0.011, 0.078, 0.006, 0.012, 0.12),
                CreateShihtaComponent("Мих.к-т", 0.0046, 9.2, 0.6, 66, 24.6, 0.34, 7.32, 0.28, 0.16, 0, 0.012, 0.012, 0.006, 0.018, 0.02),
                CreateShihtaComponent("Ковдор. к-т", 0.0022, 7, 0.7, 63, 23, 0.2, 0.99, 4.8, 1.96, 1.11, 0.3, 0.12, 0.006, 0.006, 1.11),
                CreateShihtaComponent("А/руда ММК", 0.0452, 1.5, 4.2, 50, 4, 6.68, 13.63, 1.68, 3.35, 0.25, 3.2, 0.046, 0.006, 0.018, 0.19),
                CreateShihtaComponent("Богосл.а/р.", 0, 1.9, 3.4, 54, 0, 6.01, 9.62, 1.32, 2.83, 0.11, 2.39, 0.028, 0.006, 0.017, 0.21),
                CreateShihtaComponent("Стойл.а/р.", 0.0433, 8.2, 11.2, 52, 5, 1.1, 11.2, 0.8, 2.5, 0.24, 0.12, 0.043, 0.008, 0.017, 0.07),
                CreateShihtaComponent("Михайл.а/р.", 0.0762, 7.6, 5.5, 55, 3.9, 1.38, 13.43, 0.32, 1.55, 0.11, 0.431, 0.027, 0.017, 0.013, 0.04),
                CreateShihtaComponent("\"Атансор\".а/р.", 0, 2.7, 8.1, 56, 0, 0.36, 6.06, 0.39, 3.2, 0, 0.07, 0.058, 0, 0, 0),
                CreateShihtaComponent("Белор.а/р.", 0, 8.5, 9.5, 43, 0, 0.18, 19.7, 0.6, 5.1, 23, 0.66, 0.05, 0.005, 0.005, 0.23),
                CreateShihtaComponent("Злат. Агл-т", 0, 0, 0, 56, 0, 8.15, 5.51, 3.36, 0.88, 0.13, 0.06, 0.038, 0.112, 0.112, 0.13),
                CreateShihtaComponent("Отс.ССГПО", 0.0007, 6.7, 2, 61, 1.5, 4.2, 4.15, 1, 1.45, 0.29, 0.23, 0.01, 0.027, 0.027, 0.29),
                CreateShihtaComponent("Шлам 4а/ф", 0.0155, 9.6, 2, 57, 12.5, 8.8, 9.1, 1.8, 1.4, 0.2, 0.16, 0.019, 0.017, 0.02, 0.02),
                CreateShihtaComponent("Агломел.", 0.0151, 0, 0.2, 58, 12.7, 7.96, 5.79, 1.71, 1.54, 0.2, 0.07, 0.023, 0.012, 0.021, 0.21),
                CreateShihtaComponent("Окалина.", 0.0181, 8.7, 2.96, 71, 48, 3.7, 2.09, 0.56, 0.42, 0.1, 0.05, 0.03, 0.059, 0.04, 0.45),
                CreateShihtaComponent("Шлам ВФУ", 0.0127, 8.7, 10.2, 54, 12.2, 4.96, 4.99, 1.43, 1.61, 0.26, 0.308, 0.023, 0.009, 0.68, 0.2),
                CreateShihtaComponent("К.шлак маг.", 0.0123, 5.6, 0, 40, 10.4, 19.67, 13.94, 7.88, 3.41, 0.75, 0.084, 0.16, 0.27, 0, 2.1),
                CreateShihtaComponent("Бак.сид.", 0.033, 4.8, 31.2, 35, 36.4, 3.45, 6.07, 8.03, 2.08, 0.1, 0.082, 0.22, 0.005, 0.013, 1.66),
                CreateShihtaComponent("Известь", 0.0247, 0, 12, 0, 0, 85, 0.1, 5, 0.3, 0, 0, 0, 0, 0, 0)
            ]
        };
    }

    private static ShihtaComponent CreateShihtaComponent(
        string name,
        double weight,
        double wet,
        double pmpp,
        double fe,
        double feO,
        double caO,
        double siO2,
        double mgO,
        double al2O3,
        double tiO2,
        double s,
        double p,
        double cr,
        double zn,
        double mnO)
    {
        return new ShihtaComponent
        {
            Name = name,
            Weight = weight,
            Wet = wet,
            PMPP = pmpp,
            Fe = fe,
            FeO = feO,
            CaO = caO,
            SiO2 = siO2,
            MgO = mgO,
            Al2O3 = al2O3,
            TiO2 = tiO2,
            S = s,
            P = p,
            Cr = cr,
            Zn = zn,
            MnO = mnO
        };
    }

    private static List<ComponentInfo> CreateDefaultResponseComponents()
    {
        return
        [
            CreateComponentInfo("Шихта", 97.646, 58.088, 0.465, 0.034, 22.384, 4.392, 5.708, 1.53, 1.566, 0.232, 0.214, 0.022, 3.36, 0, 0, 0),
            CreateComponentInfo("Известняк", 7.766, 0, 0, 0, 0, 4.023, 0.013, 0.004, 0.249, 0, 0, 0, 3.478, 0, 0, 0),
            CreateComponentInfo("Доломит", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
            CreateComponentInfo("Коксик", 4, 0, 0.018, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
            CreateComponentInfo("Зола Коксика", 0.68, 0.037, 0, 0, 0, 0.053, 0.327, 0.167, 0.014, 0, 0, 0, 0, 0, 0, 0),
            CreateComponentInfo("Итог", 110.092, 58.124, 0.048, 0.035, 12, 8.468, 6.048, 1.701, 1.828, 0.232, 0.214, 0.022, 6.837, 69.701, 100.298, 1.4)
        ];
    }

    private static ComponentInfo CreateComponentInfo(
        string name,
        double componentOfShihta,
        double fe,
        double s,
        double p,
        double feO,
        double caO,
        double siO2,
        double al2O3,
        double mgO,
        double mnO,
        double tiO2,
        double zn,
        double pmpp,
        double fe2O3,
        double oxideSum,
        double caOSiO2)
    {
        return new ComponentInfo
        {
            ComponentName = name,
            ReportComponentOfShihta = componentOfShihta,
            ReportFe = fe,
            ReportS = s,
            ReportP = p,
            ReportFeO = feO,
            ReportCaO = caO,
            ReportSiO2 = siO2,
            ReportAl2O3 = al2O3,
            ReportMgO = mgO,
            ReportMnO = mnO,
            ReportTiO2 = tiO2,
            ReportZn = zn,
            ReportPMPP = pmpp,
            ReportFe2O3 = fe2O3,
            ReportOxideSum = oxideSum,
            ReportCaO_SiO2 = caOSiO2
        };
    }
}
