using System.Diagnostics.Metrics;
using BaseLib.Models2;
using BaseLib.Models2.Aglom.Outputs;
using BaseLib.Models2.Base.Outputs;

namespace BaseLib;

public class BlastFurnaceSmeltingGasDynamicModeXLLibrary : IMathLibrary<RequestModelV2, ResponseModelV2>
{
    public ResponseModelV2 Calculate(RequestModelV2 request)
    {
        var aglomOutput = CalcAgloms(request);
        var blastFurnanceOutput = CalcBlastFurnanceOutput(request);

        var response = new ResponseModelV2()
        {
            AglomOutput = aglomOutput,
            BlastFurnanceOutputModel = blastFurnanceOutput
        };

        return response;
    }

    private BlastFurnanceOutputModel CalcBlastFurnanceOutput(RequestModelV2 request)
    {
        var furnanceGeometry = CalcFurnanceGeometry(request);
        var carbonBalance = CalcCarbonBalance(request);
        var materialConsumption = CalcMaterialConsumption(request, carbonBalance);
        var blastParameters = CalcBlastParamters(request, carbonBalance);

        var blastFurnanceOutput = new BlastFurnanceOutputModel
        {
            FurnaceGeometry = furnanceGeometry,
            CarbonBalance = carbonBalance,
            MaterialConsumption = materialConsumption,
            BlastParameters = blastParameters
        };

        return blastFurnanceOutput;
    }

    private BlastParameters CalcBlastParamters(RequestModelV2 request, CarbonBalance carbonBalance)
    {
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;
        var productionParameters = request.BlastFurnaceInput.Production;
        var thermalAndPressureParameters = request.BlastFurnaceInput.ThermalAndPressure;
        var furnanceGeometry = request.BlastFurnaceInput.Geometry;
        
        var rashod_Dut_Koks = 0.9333 / (0.01 * fuelAndBlastParameters.Kislorod_dut + 0.00063 * fuelAndBlastParameters.Vlazhn_dut);
        
        var rashod_Dut_Prir_Gaz = 0.5 / (0.01 * fuelAndBlastParameters.Kislorod_dut + 0.00063 * fuelAndBlastParameters.Vlazhn_dut);

        var rashod_Dut_Sum = rashod_Dut_Koks +
                             fuelAndBlastParameters.Udeln_prir_gaz / carbonBalance.C_Out_Furm * rashod_Dut_Prir_Gaz;

        var rashod_Dut_Udeln = rashod_Dut_Sum * carbonBalance.C_Out_Furm;
        
        var rashod_Dut_Minut =rashod_Dut_Udeln * productionParameters.Proizvodit_chugun / 1440;

        var speed_Dut_Furm =
            ((rashod_Dut_Minut +
              (fuelAndBlastParameters.Udeln_prir_gaz * productionParameters.Proizvodit_chugun / 1440)) *
             (thermalAndPressureParameters.Temp_dut + 273) * 77.73) / (furnanceGeometry.Kolvo_furm *
                                                                       furnanceGeometry.Diam_furm *
                                                                       furnanceGeometry.Diam_furm *
                                                                       (1 + thermalAndPressureParameters
                                                                           .Davlen_izb_dut));
        var vyazkost_Dut = -9.1 * 1e-5 + thermalAndPressureParameters.Temp_dut * 2.65 * 10e-7;

        var reinolds = 0.001 * speed_Dut_Furm * furnanceGeometry.Diam_furm / vyazkost_Dut;
        
        var blastParameters = new BlastParameters
        {
            Rashod_Dut_Koks = rashod_Dut_Koks,
            Rashod_Dut_Krit = null, // TODO: досчитать позднее
            Rashod_Dut_Prir_Gaz = rashod_Dut_Prir_Gaz,
            Rashod_Dut_Sum = rashod_Dut_Sum,
            Rashod_Dut_Udeln = rashod_Dut_Udeln,
            Rashod_Dut_Minut = rashod_Dut_Minut,
            Speed_Dut_Furm = speed_Dut_Furm,
            Vyazkost_Dut = vyazkost_Dut,
            Reinolds = reinolds
            
        };

        return blastParameters;
    }

    private CarbonBalance CalcCarbonBalance(RequestModelV2 request)
    {
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;
        var composition = request.BlastFurnaceInput.Composition;

        var c_Input = fuelAndBlastParameters.Udeln_koks * fuelAndBlastParameters.C_neletuch * 0.01;
        
        var c_Out_Vosstan = composition.Fe_chugun * 10
                                                  * fuelAndBlastParameters.Stepen_pryamogo_vost 
                                                  * 12 / 56 + 10 
                                                    * composition.Mn_chugun 
                                                    * 12 / 55 + 10 
                                                    * composition.P_chugun 
                                                    * 60 / 62 + 10 
                                                    * composition.Si_chugun
                                                    * 24 / 28 + 10
                                                    * composition.S_shlak
                                                    * 12 / 32;

        var c_Out_Metan = 0.008 * c_Input;

        var c_Out_Chugun = 10 * composition.C_chugun;

        var c_Out_Furm = c_Input - (c_Out_Vosstan + c_Out_Chugun + c_Out_Metan);
        
        var carbonBalance = new CarbonBalance
        {
            C_Input = c_Input,
            C_Out_Chugun = c_Out_Chugun,
            C_Out_Metan = c_Out_Metan,
            C_Out_Vosstan = c_Out_Vosstan,
            C_Out_Furm = c_Out_Furm
        };

        return carbonBalance;
    }

    private MaterialConsumption CalcMaterialConsumption(RequestModelV2 request, CarbonBalance carbonBalance)
    {
        var productionParameters = request.BlastFurnaceInput.Production;
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;

        var udeln_Okat = productionParameters.Udeln_zhelezorud * productionParameters.Dolya_okat;

        var udeln_Aglo = productionParameters.Udeln_zhelezorud - udeln_Okat;

        var rashod_Prir_Gaz = fuelAndBlastParameters.Udeln_prir_gaz / carbonBalance.C_Out_Furm;

        var udeln_Koks_1000 = 1e-3 * fuelAndBlastParameters.Udeln_koks;

        var Udeln_Sum = udeln_Aglo + udeln_Okat + udeln_Koks_1000;
        
        var materialConsumption = new MaterialConsumption()
        {
            Udeln_Izvest = productionParameters.Udeln_izvest,
            Udeln_Okat = udeln_Okat,
            Udeln_Aglo = udeln_Aglo,
            Rashod_Prir_Gaz = rashod_Prir_Gaz,
            Udeln_Sum = Udeln_Sum,
            Udeln_Koks_1000 = udeln_Koks_1000
        };

        return materialConsumption;
    }

    private FurnaceGeometry CalcFurnanceGeometry(RequestModelV2 request)
    {
        var geometryParams = request.BlastFurnaceInput.Geometry;
        
        var diam_niz = (geometryParams.Diam_gorn + geometryParams.Diam_raspar) /
                       2;
        var s_sech_niz = Math.PI * diam_niz * diam_niz / 4;

        var diam_verh = (geometryParams.Diam_raspar + geometryParams.Diam_koloshnik) / 2;

        var height_aktiv = geometryParams.Height_koloshnik + geometryParams.Height_raspar +
            geometryParams.Height_shahta + geometryParams.Height_zaplechik - geometryParams.Uroven_zasypi + 0.5;

        var height_Shihta_Niz = geometryParams.Height_zaplechik + geometryParams.Height_raspar + 0.5;

        var shihta_Height_Verh = geometryParams.Height_koloshnik + geometryParams.Height_shahta -
                                 geometryParams.Uroven_zasypi;
        
        var furnanceGeometry = new FurnaceGeometry()
        {
            Diam_Niz = diam_niz,
            S_Sech_Niz = s_sech_niz,
            Diam_Verh = diam_verh,
            Height_Aktiv = height_aktiv,
            Height_Shihta_Niz = height_Shihta_Niz,
            Shihta_Height_Verh = shihta_Height_Verh
        };

        return furnanceGeometry;
    }

    private AglomOutputModel CalcAgloms(RequestModelV2 request)
    {
        var biggestAglomFractionPart = request
            .AglomInput
            .AglomContents
            .MaxBy(x => x.MinFractionSize)!
            .FractionPart;

        var aglomPorosity = request.AglomInput.AglomContents.Aggregate(0.0, (acc, curr) =>
        {
            return (1 - curr.Porosity) * curr.FractionPart
                                       * (1.582 - 2.416 * 50 / 50 + 1.485 * 50 * 50 / (50 * 50)
                                                                  + 0.18 * (biggestAglomFractionPart / curr.FractionPart)
                                          - 0.015 * (biggestAglomFractionPart / curr.FractionPart)
                                                  * (biggestAglomFractionPart / curr.FractionPart)) + acc;
        });

        var biggestOkatFrationPart = request
            .AglomInput
            .OkatContents
            .MaxBy(x => x.MinFractionSize)!
            .FractionPart;
        
        var okatPorosity = request.AglomInput.OkatContents.Aggregate(0.0, (acc, curr) =>
        {
            return (1 - curr.Porosity) * curr.FractionPart
                                       * (1.582 - 2.416 * 50 / 50 + 1.485 * 50 * 50 / (50 * 50)
                                                                  + 0.18 * (biggestOkatFrationPart / curr.FractionPart)
                                          - 0.015 * (biggestOkatFrationPart / curr.FractionPart)
                                                  * (biggestOkatFrationPart / curr.FractionPart)) + acc;
        });

        return new AglomOutputModel
        {
            AglomPorosity = aglomPorosity,
            OkatPorosity = okatPorosity
        };
    }
}