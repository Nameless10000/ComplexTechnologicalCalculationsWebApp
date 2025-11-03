using System.Diagnostics.Metrics;
using BaseLib.Models2;
using BaseLib.Models2.Aglom;
using BaseLib.Models2.Aglom.Outputs;
using BaseLib.Models2.Base.Outputs;

namespace BaseLib;

public class BlastFurnaceSmeltingGasDynamicModeXLLibrary : IMathLibrary<RequestModelV2, ResponseModelV2>
{
    public ResponseModelV2 Calculate(RequestModelV2 request)
    {
        var aglomOutput = CalcAgloms(request);
        var blastFurnanceOutput = CalcBlastFurnanceOutput(request, aglomOutput);

        var response = new ResponseModelV2()
        {
            AglomOutput = aglomOutput,
            BlastFurnanceOutputModel = blastFurnanceOutput
        };

        return response;
    }

    private BlastFurnanceOutputModel CalcBlastFurnanceOutput(RequestModelV2 request, AglomOutputModel aglomOutputModel)
    {
        var furnanceGeometry = CalcFurnanceGeometry(request);
        var carbonBalance = CalcCarbonBalance(request);
        var thermalParameters = CaclThermalParameters(request);
        var materialConsumption = CalcMaterialConsumption(request, carbonBalance);
        var chargeAndPacking = CalcChargeAndPacking(request, aglomOutputModel, materialConsumption);
        var blastParameters = CalcBlastParamters(request, carbonBalance);
        var hearthGas = CalcHearthGas(request, blastParameters, carbonBalance, thermalParameters, materialConsumption);
        var intermediateGas = CalcIntermediateGas(request, carbonBalance, hearthGas);
        var hydrodynamicsLower = CalcHydrodynamicsLower(request, blastParameters, intermediateGas, furnanceGeometry, chargeAndPacking, hearthGas);
        var topGas = CalcTopGas(request, materialConsumption, intermediateGas, carbonBalance);
        var hydrodynamicsUpper = CalcHydrodynamicsUpper(request, furnanceGeometry, hydrodynamicsLower, chargeAndPacking,
            thermalParameters, hearthGas, intermediateGas, topGas, out var rashodDutKrit);

        blastParameters.Rashod_Dut_Krit = rashodDutKrit;
        
        var blastFurnanceOutput = new BlastFurnanceOutputModel
        {
            FurnaceGeometry = furnanceGeometry,
            CarbonBalance = carbonBalance,
            MaterialConsumption = materialConsumption,
            BlastParameters = blastParameters,
            ChargeAndPacking = chargeAndPacking,
            HydrodynamicsLower = hydrodynamicsLower,
            HearthGas = hearthGas,
            ThermalParameters = thermalParameters,
            IntermediateGas1000 = intermediateGas,
            TopGas = topGas,
            HydrodynamicsUpper = hydrodynamicsUpper
        };

        return blastFurnanceOutput;
    }

    private TopGas CalcTopGas(RequestModelV2 request, 
        MaterialConsumption materialConsumption, 
        IntermediateGas1000 intermediateGas1000,
        CarbonBalance carbonBalance)
    {
        var materialProperties = request.BlastFurnaceInput.Materials;
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;
        var productionParameters = request.BlastFurnaceInput.Production;

        var volume_CO2_Izvest =
            0.01 * materialConsumption.Udeln_Izvest * 22.4 / 44 * materialProperties.Poteri_prokalivanie;

        var volume_CO2_Kvost = intermediateGas1000.Volume_CO_1000 * fuelAndBlastParameters.Stepen_CO;

        var volume_CO2_Kolgaz = volume_CO2_Izvest + volume_CO2_Kvost;

        var volume_CO_Kolgaz = intermediateGas1000.Volume_CO_1000 - volume_CO2_Kvost;

        var volume_CH4_Kolgaz = carbonBalance.C_Out_Metan * 22.4 / 12;

        var volume_N2_Kolgaz = intermediateGas1000.Volume_N2_1000;
        
        var volume_H2_Kolgaz = intermediateGas1000.Volume_H2_1000;

        var udeln_Kolgaz = volume_CO2_Kolgaz + volume_CO_Kolgaz + volume_CH4_Kolgaz + volume_N2_Kolgaz +
                           volume_H2_Kolgaz;

        var kolgaz_CO2 = volume_CO2_Kolgaz / udeln_Kolgaz;
        
        var kolgaz_CO = volume_CO_Kolgaz / udeln_Kolgaz;

        var kolgaz_H2 = volume_H2_Kolgaz / udeln_Kolgaz;
        
        var kolgaz_CH4 = volume_CH4_Kolgaz / udeln_Kolgaz;
        
        var kolgaz_N2 = 1 - (kolgaz_H2 + kolgaz_CH4 + kolgaz_CO + kolgaz_CO2);

        var kolgaz_Plotn = 44 / 22.4 * kolgaz_CO2 + 28 / 22.4 * kolgaz_CO + 2 / 22.4 * kolgaz_H2 +
                           16 / 22.4 * kolgaz_CH4 + 28 / 22.4 * kolgaz_N2;
        
        var kolgaz_Minut =udeln_Kolgaz * productionParameters.Proizvodit_chugun / (24 * 60 * 60);
        
        var topGas = new TopGas
        {
            Volume_CO2_Izvest = volume_CO2_Izvest,
            Volume_CO2_Kvost = volume_CO2_Kvost,
            Volume_CO2_Kolgaz = volume_CO2_Kolgaz,
            Volume_CO_Kolgaz = volume_CO_Kolgaz,
            Volume_CH4_Kolgaz = volume_CH4_Kolgaz,
            Volume_N2_Kolgaz = volume_N2_Kolgaz,
            Volume_H2_Kolgaz = volume_H2_Kolgaz,
            Udeln_Kolgaz = udeln_Kolgaz,
            Kolgaz_CO2 = kolgaz_CO2,
            Kolgaz_CO = kolgaz_CO,
            Kolgaz_H2 = kolgaz_H2,
            Kolgaz_CH4 = kolgaz_CH4,
            Kolgaz_N2 = kolgaz_N2,
            Kolgaz_Plotn = kolgaz_Plotn,
            Kolgaz_Minut = kolgaz_Minut
        };

        return topGas;
    }

    private HydrodynamicsUpper CalcHydrodynamicsUpper(RequestModelV2 request, 
        FurnaceGeometry outGeometry,
        HydrodynamicsLower hydrodynamicsLower,
        ChargeAndPacking chargeAndPacking,
        ThermalParameters thermalParameters,
        HearthGas hearthGas,
        IntermediateGas1000 intermediateGas1000,
        TopGas topGas, out double rashod_Dut_Krit)
    {
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;
        var thermalAndPressureParameters = request.BlastFurnaceInput.ThermalAndPressure;
        var productionParameters = request.BlastFurnaceInput.Production;
        var furnaceGeometry = request.BlastFurnaceInput.Geometry;
        
        var speed_Filtr_Verh = 4 * topGas.Kolgaz_Minut / (Math.PI * outGeometry.Diam_Verh * outGeometry.Diam_Verh);

        var davlen_Verh = ((hydrodynamicsLower.Davlen_Izb_1000 + 1) +
                           (thermalAndPressureParameters.Davlen_izb_koloshnik_gaz + 1)) / 2;

        var koef_Soprot_Verh = thermalAndPressureParameters.Perepad_verh / (Math.Pow(speed_Filtr_Verh, 2) / 2 *
            topGas.Kolgaz_Plotn * outGeometry.Shihta_Height_Verh / chargeAndPacking.Shihta_Diam_Verh *
            (1 - chargeAndPacking.Shihta_Porozn_Verh) / Math.Pow(chargeAndPacking.Shihta_Porozn_Verh, 3) *
            (thermalParameters.Temp_Verh + 273) / 273 * 1 / davlen_Verh);

        var koef_Proporc_Dut_Filtr_Verh = 4 * fuelAndBlastParameters.Rashod_dut /
                                          (speed_Filtr_Verh * Math.PI * outGeometry.Diam_Verh * outGeometry.Diam_Verh);

        var koef_Av = koef_Soprot_Verh * outGeometry.Shihta_Height_Verh / chargeAndPacking.Shihta_Diam_Verh *
            topGas.Kolgaz_Plotn / 2 *
            Math.Pow(4 / (Math.PI * outGeometry.Diam_Verh * outGeometry.Diam_Verh * koef_Proporc_Dut_Filtr_Verh), 2) *
            (1 - chargeAndPacking.Shihta_Porozn_Verh) / Math.Pow(chargeAndPacking.Shihta_Porozn_Verh, 3) *
            (thermalParameters.Temp_Verh + 273) / 273 * 1 / davlen_Verh;

        var stepen_Urav = 1000 *
                          (thermalAndPressureParameters.Davlen_izb_dut - hydrodynamicsLower.Poteri_Furm -
                           thermalAndPressureParameters.Davlen_izb_koloshnik_gaz) /
                          (outGeometry.Height_Aktiv * chargeAndPacking.Massa_Nasyp_Shihta);

        var perepad_Davlen = productionParameters.Stepen_urav_krit * outGeometry.Height_Aktiv *
            chargeAndPacking.Massa_Nasyp_Shihta / 1000;

        rashod_Dut_Krit = Math.Sqrt(perepad_Davlen / (hydrodynamicsLower.Koef_An + koef_Av));

        var speed_Filtr_Gorn = 4 * hearthGas.Furmgaz_Udeln * productionParameters.Proizvodit_chugun /
                               (Math.PI * Math.Pow(furnaceGeometry.Diam_gorn, 2) * 24 * 60 * 60);

        var speed_Real_Verh = speed_Filtr_Gorn * (hearthGas.Temp_Teor + 273) /
                              (273 * (1 + thermalAndPressureParameters.Davlen_izb_dut) * chargeAndPacking.Porozn_Koks);

        var speed_Filtr_Raspar = 4 * intermediateGas1000.Volume_Sum_1000 * productionParameters.Proizvodit_chugun /
                                 (24 * 60 * 60 * Math.PI * furnaceGeometry.Diam_raspar * furnaceGeometry.Diam_raspar);

        var speed_Real_Raspar = speed_Filtr_Raspar * (1000 + 273) /
                                (273 * (1 + hydrodynamicsLower.Davlen_Izb_1000) * chargeAndPacking.Porozn_Sloy_Korrekt);

        var speed_Filtr_Koloshnik = 4 * topGas.Udeln_Kolgaz * productionParameters.Proizvodit_chugun /
                                    (24 * 60 * 60 * Math.PI * furnaceGeometry.Diam_koloshnik *
                                     furnaceGeometry.Diam_koloshnik);

        var speed_Real_Koloshnik = speed_Filtr_Koloshnik * (thermalAndPressureParameters.Temp_koloshnik_gaz + 273) /
                                   (273 * (1 + thermalAndPressureParameters.Davlen_izb_koloshnik_gaz) *
                                    chargeAndPacking.Shihta_Porozn_Verh);
        
        var hydrodynamicsUpper = new HydrodynamicsUpper
        {
            Speed_Filtr_Verh = speed_Filtr_Verh,
            Davlen_Verh = davlen_Verh,
            Koef_Soprot_Verh = koef_Soprot_Verh,
            Koef_Proporc_Dut_Filtr_Verh = koef_Proporc_Dut_Filtr_Verh,
            Koef_Av = koef_Av,
            Stepen_Urav = stepen_Urav,
            Perepad_Davlen = perepad_Davlen,
            Speed_Filtr_Gorn = speed_Filtr_Gorn,
            Speed_Real_Verh = speed_Real_Verh,
            Speed_Filtr_Raspar = speed_Filtr_Raspar,
            Speed_Real_Raspar = speed_Real_Raspar,
            Speed_Filtr_Koloshnik = speed_Filtr_Koloshnik,
            Speed_Real_Koloshnik = speed_Real_Koloshnik
        };

        return hydrodynamicsUpper;
    }

    private IntermediateGas1000 CalcIntermediateGas(RequestModelV2 request, 
        CarbonBalance carbonBalance, 
        HearthGas hearthGas)
    {
        var compositionParameters = request.BlastFurnaceInput.Composition;
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;
        var productionParameters = request.BlastFurnaceInput.Production;

        var volume_CO_Pvost = 10 * 22.4 *
                              (compositionParameters.Fe_chugun * fuelAndBlastParameters.Stepen_pryamogo_vost / 56 +
                               compositionParameters.Mn_chugun / 55 + 2 * compositionParameters.Si_chugun / 28 +
                               compositionParameters.S_shlak / 32);

        var volume_CO_1000 = hearthGas.Furmgaz_CO_V * carbonBalance.C_Out_Furm + volume_CO_Pvost;

        var volume_H2_1000 = hearthGas.Furmgaz_H2_V * carbonBalance.C_Out_Furm *
                             (1 - fuelAndBlastParameters.Stepen_vodorod);

        var volume_N2_1000 = carbonBalance.C_Out_Furm * hearthGas.Furmgaz_N2_V;

        var volume_Sum_1000 = volume_CO_1000 + volume_H2_1000 + volume_N2_1000;

        var domengaz_CO_1000 = volume_CO_1000 / volume_Sum_1000;
        
        var domengaz_H2_1000 = volume_H2_1000 / volume_Sum_1000;
        
        var domengaz_N2_1000 = volume_N2_1000 / volume_Sum_1000;

        var domengaz_Plotn_1000 =
            28 / 22.4 * domengaz_N2_1000 + 28 / 22.4 * domengaz_CO_1000 + 2 / 22.4 * domengaz_H2_1000;

        var domengaz_Rashod_1000 = volume_Sum_1000 * productionParameters.Proizvodit_chugun / (24 * 60 * 60);
        
        var intermediateGas = new IntermediateGas1000
        {
            Volume_CO_Pvost = volume_CO_Pvost,
            Volume_CO_1000 = volume_CO_1000,
            Volume_H2_1000 = volume_H2_1000,
            Volume_N2_1000 = volume_N2_1000,
            Volume_Sum_1000 = volume_Sum_1000,
            Domengaz_CO_1000 = domengaz_CO_1000,
            Domengaz_H2_1000 = domengaz_H2_1000,
            Domengaz_N2_1000 = domengaz_N2_1000,
            Domengaz_Plotn_1000 = domengaz_Plotn_1000,
            Domengaz_Rashod_1000 = domengaz_Rashod_1000
        };

        return intermediateGas;
    }

    private ThermalParameters CaclThermalParameters(RequestModelV2 request)
    {
        var thermalAndPressureParameters = request.BlastFurnaceInput.ThermalAndPressure;
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;

        var teploemk_2atom = 1.2897 + 0.000121 * thermalAndPressureParameters.Temp_dut;

        var teploemk_Voda = 1.456 + 0.000282 * thermalAndPressureParameters.Temp_dut;

        var temp_Verh = (thermalAndPressureParameters.Temp_koloshnik_gaz + 1000) / 2;

        var teplosod_Dut = teploemk_2atom * thermalAndPressureParameters.Temp_dut - 0.00124 *
            fuelAndBlastParameters.Vlazhn_dut * (10800 - teploemk_Voda * thermalAndPressureParameters.Temp_dut);

        var teplosod_Koks = thermalAndPressureParameters.Temp_koks * thermalAndPressureParameters.Teploemk_koks;
        
        var thermalParameters = new ThermalParameters
        {
            Teploemk_2atom = teploemk_2atom,
            Teploemk_Voda = teploemk_Voda,
            Temp_Verh = temp_Verh,
            Teplosod_Dut = teplosod_Dut,
            Teplosod_Koks = teplosod_Koks
        };

        return thermalParameters;
    }

    private HearthGas CalcHearthGas(RequestModelV2 request, 
        BlastParameters blastParameters, 
        CarbonBalance carbonBalance, 
        ThermalParameters thermalParameters,
        MaterialConsumption materialConsumption)
    {
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;
        var thermalAndPressureParameters = request.BlastFurnaceInput.ThermalAndPressure;

        var furmgaz_Koks = 1.8667 + blastParameters.Rashod_Dut_Koks * (1 - 0.01 * fuelAndBlastParameters.Kislorod_dut +
                                                                       0.00124 * fuelAndBlastParameters.Vlazhn_dut);
        
        var furmgaz_Prir_Gaz = 3 + blastParameters.Rashod_Dut_Prir_Gaz * (1 - 0.01 * fuelAndBlastParameters.Kislorod_dut +
                                                                               0.00124 * fuelAndBlastParameters.Vlazhn_dut);

        var furmgaz_Sum =furmgaz_Koks + fuelAndBlastParameters.Udeln_prir_gaz / carbonBalance.C_Out_Furm * furmgaz_Prir_Gaz;

        var furmgaz_Udeln = furmgaz_Sum * carbonBalance.C_Out_Furm;

        var furmgaz_CO_V = 1.8667 + fuelAndBlastParameters.Udeln_prir_gaz / carbonBalance.C_Out_Furm *
            fuelAndBlastParameters.C_prir_gaz;

        var furmgaz_H2_V =
            (0.9333 + 0.5 * fuelAndBlastParameters.Udeln_prir_gaz / carbonBalance.C_Out_Furm * 1) /
            (0.01 * fuelAndBlastParameters.Kislorod_dut + 0.00124 * fuelAndBlastParameters.Vlazhn_dut) * 0.00124 *
            fuelAndBlastParameters.Vlazhn_dut + fuelAndBlastParameters.Udeln_prir_gaz / carbonBalance.C_Out_Furm *
            fuelAndBlastParameters.H2_prir_gaz;

        var furmgaz_N2_V = (0.9333 + 0.5 * fuelAndBlastParameters.Udeln_prir_gaz / carbonBalance.C_Out_Furm * 1) /
                           (0.01 * fuelAndBlastParameters.Kislorod_dut + 0.00124 * fuelAndBlastParameters.Vlazhn_dut) *
                           (1 - 0.01 * fuelAndBlastParameters.Kislorod_dut);

        var furmgaz_CO = furmgaz_CO_V / (furmgaz_CO_V + furmgaz_H2_V + furmgaz_N2_V);
        
        var furmgaz_H2 = furmgaz_H2_V / (furmgaz_CO_V + furmgaz_H2_V + furmgaz_N2_V);
        
        var furmgaz_N2 = furmgaz_N2_V / (furmgaz_CO_V + furmgaz_H2_V + furmgaz_N2_V);

        var teplosod_Furmgaz =/*(thermalAndPressureParameters.Teplota_nepoln_koks + 
                               blastParameters.Rashod_Dut_Koks * C107+C108+C109*(База!C31+C80*C107))/(C83+C109*C84)*/
        (thermalAndPressureParameters.Teplota_nepoln_koks +
         blastParameters.Rashod_Dut_Koks * thermalParameters.Teplosod_Dut + thermalParameters.Teplosod_Koks +
             materialConsumption.Rashod_Prir_Gaz * (thermalAndPressureParameters.Teplota_nepoln_prir_gaz +
                                                    blastParameters.Rashod_Dut_Prir_Gaz *
                                                    thermalParameters.Teplosod_Dut)) / (furmgaz_Koks +
                materialConsumption.Rashod_Prir_Gaz * furmgaz_Prir_Gaz);

        var temp_Teor = 165 + 0.6113 * teplosod_Furmgaz;

        var temp_Sredn_Niz = (temp_Teor + 1000) / 2;
        
        var hearthGas = new HearthGas
        {
            Furmgaz_Koks = furmgaz_Koks,
            Furmgaz_Prir_Gaz = furmgaz_Prir_Gaz,
            Furmgaz_Sum = furmgaz_Sum,
            Furmgaz_Udeln = furmgaz_Udeln,
            Furmgaz_CO_V = furmgaz_CO_V,
            Furmgaz_H2_V = furmgaz_H2_V,
            Furmgaz_N2_V = furmgaz_N2_V,
            Furmgaz_CO = furmgaz_CO,
            Furmgaz_H2 = furmgaz_H2,
            Furmgaz_N2 = furmgaz_N2,
            Teplosod_Furmgaz = teplosod_Furmgaz,
            Temp_Teor = temp_Teor,
            Temp_Sredn_Niz = temp_Sredn_Niz
        };

        return hearthGas;
    }

    private HydrodynamicsLower CalcHydrodynamicsLower(RequestModelV2 request, 
        BlastParameters blastParameters, 
        IntermediateGas1000 intermediateGas1000,
        FurnaceGeometry outGeometry,
        ChargeAndPacking chargeAndPacking,
        HearthGas hearthGas)
    {
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;
        var geometry = request.BlastFurnaceInput.Geometry;
        var thermalAndPressureParameters = request.BlastFurnaceInput.ThermalAndPressure;
        
        var tren_Koef = 0.0032 + (0.221 / Math.Pow(blastParameters.Reinolds, 0.237));

        var tren_Sum = tren_Koef * (100 + fuelAndBlastParameters.Poteri_dut) / 100;

        var poteri_Furm = (geometry.Kolvo_furm *
                           (tren_Sum * geometry.Dlina_furm * 0.001 / (0.001 * geometry.Diam_furm)) *
                           intermediateGas1000.Domengaz_Plotn_1000 *
                           (blastParameters.Speed_Dut_Furm * blastParameters.Speed_Dut_Furm / 2) *
                           ((thermalAndPressureParameters.Temp_dut + 273) / 273) *
                           (1 / (1 + thermalAndPressureParameters.Davlen_izb_dut))) / 98066.5;

        var perepad_Niz_Itog = thermalAndPressureParameters.Perepad_niz - poteri_Furm;

        var perepad_Niz_Dolya = perepad_Niz_Itog / (perepad_Niz_Itog + thermalAndPressureParameters.Perepad_verh);

        var davlen_Izb_1000 = thermalAndPressureParameters.Davlen_izb_dut - tren_Sum - perepad_Niz_Itog;

        var davlen_Niz = ((thermalAndPressureParameters.Davlen_izb_dut - poteri_Furm + 1) + (davlen_Izb_1000 + 1)) / 2;

        var speed_Filtr_Niz = 4 * intermediateGas1000.Domengaz_Rashod_1000 /
                              (Math.PI * outGeometry.Diam_Niz * outGeometry.Diam_Niz);

        var koef_Soprot_Niz = perepad_Niz_Itog / ((Math.Pow(speed_Filtr_Niz, 2) / 2) *
            (intermediateGas1000.Domengaz_Plotn_1000 * outGeometry.Height_Shihta_Niz / chargeAndPacking.Diam_Koks) *
            ((1 - chargeAndPacking.Porozn_Sloy_Korrekt) / Math.Pow(chargeAndPacking.Porozn_Sloy_Korrekt, 3)) *
            (hearthGas.Temp_Sredn_Niz + 273) / 273 * (1 / davlen_Niz));

        var koef_Proporc_Dut_Filtr_Niz = fuelAndBlastParameters.Rashod_dut / (speed_Filtr_Niz * outGeometry.S_Sech_Niz);

        var koef_An = koef_Soprot_Niz * outGeometry.Height_Shihta_Niz / chargeAndPacking.Diam_Koks *
            intermediateGas1000.Domengaz_Plotn_1000 / 2 *
            Math.Pow(1 / (outGeometry.S_Sech_Niz * koef_Proporc_Dut_Filtr_Niz), 2) *
            (1 - chargeAndPacking.Porozn_Sloy_Korrekt) / Math.Pow(chargeAndPacking.Porozn_Sloy_Korrekt, 3) *
            (hearthGas.Temp_Sredn_Niz + 273) / 273 * 1 / davlen_Niz;
        
        var hydrodynamicsLower = new HydrodynamicsLower
        {
            Speed_Filtr_Niz = speed_Filtr_Niz,
            Tren_Koef = tren_Koef,
            Tren_Sum = tren_Sum,
            Poteri_Furm = poteri_Furm,
            Perepad_Niz_Itog = perepad_Niz_Itog,
            Perepad_Niz_Dolya = perepad_Niz_Dolya,
            Davlen_Izb_1000 =  davlen_Izb_1000,
            Davlen_Niz = davlen_Niz,
            Koef_Soprot_Niz = koef_Soprot_Niz,
            Koef_Proporc_Dut_Filtr_Niz = koef_Proporc_Dut_Filtr_Niz,
            Koef_An = koef_An
        };

        return hydrodynamicsLower;
    }

    private ChargeAndPacking CalcChargeAndPacking(RequestModelV2 request,
        AglomOutputModel aglomOutputModel,
        MaterialConsumption materialConsumption)
    {
        var aglomInputModel = request.AglomInput;
        var materialProperties = request.BlastFurnaceInput.Materials;
        var fuelAndBlastParameters = request.BlastFurnaceInput.FuelAndBlast;
        
        var diam_koks = 100 / aglomInputModel.KoksContents.Aggregate(0.0, (acc, curr) =>
        {
            var coef = curr.MinFractionSize switch
            {
                80 => 80,
                60 => 70,
                40 => 50,
                25 => 32.5,
                0 => 12.5
            };

            return acc + curr.FractionPercentage / coef;
        });

        var diam_aglo = 100 / aglomInputModel.AglomContents.Aggregate(0.0, (acc, curr) =>
        {
            var coef = curr.MinFractionSize switch
            {
                50 => 50,
                25 => 32.5,
                10 => 17.5,
                5 => 7.5,
                0 => 2.5
            };

            return acc + curr.FractionPercentage / coef;
        });

        var volume_Aglo_1chugun = materialConsumption.Udeln_Aglo / materialProperties.Massa_aglo;

        var volume_Okat_1chugun = materialConsumption.Udeln_Okat / materialProperties.Massa_okat;

        var volume_Koks_1chugun = materialConsumption.Udeln_Koks_1000 / materialProperties.Massa_koks_t;

        var volume_Sum_1chugun = volume_Aglo_1chugun + volume_Okat_1chugun + volume_Koks_1chugun;

        var massa_Nasyp_Shihta = materialConsumption.Udeln_Sum / volume_Sum_1chugun;

        var biggestAglomFraction = aglomInputModel.AglomContents.MaxBy(x => x.MinFractionSize)!;
        var porozn_Aglo = 1 - (1 - biggestAglomFraction.Porosity) * biggestAglomFraction.FractionPart -
                          aglomOutputModel.AglomPorosity;

        var porozn_Koks = 0.3 * Math.Pow(0.1 * diam_koks, 0.252);

        var porozn_Okat = 0.4 - 0.25 * 13.6 / 100;

        var volume_Udeln_Shlak = materialProperties.Udeln_vyhod_shlak / materialProperties.Plotn_shlak;

        var volume_Udeln_Koks = fuelAndBlastParameters.Udeln_koks / materialProperties.Massa_koks_kg;

        var volume_Udeln_Nasadzka = porozn_Koks * volume_Koks_1chugun;

        var volume_Udeln_Ost = volume_Udeln_Nasadzka - volume_Udeln_Shlak;

        var shihta_Dolya_Aglo = volume_Aglo_1chugun / volume_Sum_1chugun;
        
        var shihta_Dolya_Koks = volume_Koks_1chugun / volume_Sum_1chugun;
        
        var shihta_Dolya_Okat = volume_Okat_1chugun / volume_Sum_1chugun;

        var shihta_Porozn_Verh =
            (volume_Aglo_1chugun * porozn_Aglo + porozn_Koks * volume_Koks_1chugun +
             volume_Okat_1chugun * porozn_Okat) / volume_Sum_1chugun;

        var shihta_Diam_Verh = 1 / (shihta_Dolya_Aglo / diam_aglo + shihta_Dolya_Koks / diam_koks);

        var porozn_Sloy_Korrekt = volume_Udeln_Ost / volume_Udeln_Koks;
            
            
        var chargeAndPacking = new ChargeAndPacking
        {
            Diam_Koks = diam_koks,
            Diam_Aglo = diam_aglo,
            Volume_Aglo_1chugun = volume_Aglo_1chugun,
            Volume_Okat_1chugun = volume_Okat_1chugun,
            Volume_Koks_1chugun = volume_Koks_1chugun,
            Volume_Sum_1chugun = volume_Sum_1chugun,
            Massa_Nasyp_Shihta = massa_Nasyp_Shihta,
            Porozn_Aglo = porozn_Aglo,
            Porozn_Koks = porozn_Koks,
            Porozn_Okat = porozn_Okat,
            Volume_Udeln_Shlak = volume_Udeln_Shlak,
            Volume_Udeln_Koks = volume_Udeln_Koks,
            Volume_Udeln_Nasadzka = volume_Udeln_Nasadzka,
            Volume_Udeln_Ost = volume_Udeln_Ost,
            Porozn_Sloy_Korrekt = porozn_Sloy_Korrekt,
            Shihta_Dolya_Aglo = shihta_Dolya_Aglo,
            Shihta_Dolya_Koks = shihta_Dolya_Koks,
            Shihta_Dolya_Okat = shihta_Dolya_Okat,
            Shihta_Porozn_Verh = shihta_Porozn_Verh,
            Shihta_Diam_Verh = shihta_Diam_Verh
        };

        return chargeAndPacking;
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
        var vyazkost_Dut = -9.1 * 1e-5 + thermalAndPressureParameters.Temp_dut * 2.65 * 1e-7;

        var reinolds = 0.001 * speed_Dut_Furm * furnanceGeometry.Diam_furm / vyazkost_Dut;
        
        var blastParameters = new BlastParameters
        {
            Rashod_Dut_Koks = rashod_Dut_Koks,
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
            .MaxBy(x => x.MinFractionSize)!;

        var aglomPorosity = request.AglomInput.AglomContents.Aggregate(0.0, (acc, curr) =>
        {
            double coef1, coef2;

            switch (curr.MinFractionSize)
            {
                case 50:
                    coef1 = coef2 = 50;
                    break;
                case 25:
                    coef1 = 32.5;
                    coef2 = 38.5;
                    break;
                case 10:
                    coef1 = coef2 = 17.5;
                    break;
                case 5:
                    coef1 = coef2 = 7.5;
                    break;
                case 0:
                    coef1 = coef2 = 2.5;
                    break;
                default:
                    coef1 = coef2 = 0.0;
                    break;
            }
            var result = (1.0 - curr.Porosity) * curr.FractionPart
                                             * (1.582 - 2.416 * coef1 / 50.0 + 1.485 * coef2 * coef2 / (50.0 * 50.0)
                                                                        + 0.18 * (biggestAglomFractionPart.FractionPart /
                                                                            curr.FractionPart)
                                                - 0.015 * (biggestAglomFractionPart.FractionPart / curr.FractionPart)
                                                        * (biggestAglomFractionPart.FractionPart / curr.FractionPart));
            return result+ acc;
        });

        var biggestOkatFrationPart = request
            .AglomInput
            .OkatContents
            .MaxBy(x => x.MinFractionSize)!
            .FractionPart;
        
        var okatPorosity = request.AglomInput.OkatContents.Aggregate(0.0, (acc, curr) =>
        {
            double coef1, coef2;

            switch (curr.MinFractionSize)
            {
                case 50:
                    coef1 = coef2 = 50;
                    break;
                case 25:
                    coef1 = 32.5;
                    coef2 = 38.5;
                    break;
                case 10:
                    coef1 = coef2 = 17.5;
                    break;
                case 5:
                    coef1 = coef2 = 7.5;
                    break;
                case 0:
                    coef1 = coef2 = 2.5;
                    break;
                default:
                    coef1 = coef2 = 0.0;
                    break;
            }

            var result = (1 - curr.Porosity) * curr.FractionPart
                                                       * (1.582 - 2.416 * coef1 / 50 + 1.485 * coef2 * coef2 / (50 * 50)
                                                          + 0.18 * (biggestOkatFrationPart / curr.FractionPart)
                                                          - 0.015 * (biggestOkatFrationPart / curr.FractionPart)
                                                                  * (biggestOkatFrationPart / curr.FractionPart));
            return result + acc;
        });

        return new AglomOutputModel
        {
            AglomPorosity = aglomPorosity,
            OkatPorosity = okatPorosity
        };
    }
}