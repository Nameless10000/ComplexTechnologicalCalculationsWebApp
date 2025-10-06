using BaseLib.Models;

namespace BaseLib;

public class BlastFurnaceSmeltingGasDynamicModeLibrary : IMathLibrary<RequestData, ResponseData>
{
    public ResponseData Calulate(RequestData request)
    {
        // Л1.1
        var Cprish = 0.01 * request.k * request.Cnel;

        // Л1.2
        var Cpr = request._Fe * 10 * request.Rd * (12.0 / 56.0)
                  + 10 * request._P * (60.0 / 62.0)
                  + 10 * request._Mn * (12.0 / 55.0)
                  + 10 * request._Si * (24.0 / 28.0)
                  + 10 * request._S * (12.0 / 32.0);

        var Cch = 10 * request.C;

        var Cch4 = 0.008 * Cprish;

        var Cf = Cprish - (Cpr + Cch + Cch4);

        var sigma = request.Vpg / Cf;

        var Vd_ = 0.9333 / (0.01 * request.W + 0.00062 * request.Fd);

        var Vd__ = 0.5 / (0.01 * request.W + 0.00063 * request.Fd);

        var VdSum = Vd_ + request.Vpg / Cf * Vd__;

        var Qd = VdSum * Cf;

        var QdO2 = Qd * request.W * 0.01;

        var QdN2 = Qd * (100 - request.W) * 0.01;

        var QdP = Qd * request.P / 1440;

        var m = (request.VdKIP - QdP) / request.VdKIP * 100;

        var Vg_ = 1.8667 + Vd_ * (1 - 0.01 * request.W + 0.00124 * request.Fd);

        var Vg__ = 3 + Vd__ * (1 - 0.01 * request.W + 0.00124 * request.Fd);

        var Vgg = Vg_ + request.Vpg / Cf * Vg__;

        var Qgg = Cf * Vgg;

        var _C__ = request._CH4_;

        var _H2__ = 2 * request._CH4_;

        var Vco = 1.8667 + request.Vpg / Cf * _C__;

        var Vh2 = ((0.9333 + 0.5 * (request.Vpg / Cf) * _C__) / (0.01 * request.W + 0.00124 * request.Fd))
            * 0.00124 * request.Fd + request.Vpg / Cf * _H2__;

        var Vn2 = ((0.9333 + 0.5 * (request.Vpg / Cf) * _C__) / (0.01 * request.W + 0.00124 * request.Fd))
                  * (1 - 0.01 * request.W);

        var _CO_ = Vco / (Vco + Vh2 + Vn2);

        var _H2_ = Vh2 / (Vco + Vh2 + Vn2);

        var _N2_ = 1 - (_CO_ + _H2_);

        var Qco = Vco * Cf;

        var Qh2 = Vh2 * Cf;

        var Qn2 = Vn2 * Cf;

        var Vn21000 = Qn2;

        var QggSum = Qco + Qh2 + Qn2;

        var VcoPV = 10 * 22.4 * (request._Fe * request.Rd / 56
                                 + 2 * request._Si / 28
                                 + request._Mn / 55
                                 + 5 * request._P / 32
                                 + 3 * request._Cr / 32
                                 + 2 * request._Ti / 48
                                 + request._S / 32);

        var Vco1000 = Qco + VcoPV;

        var nco = request.CO2KG / (request.CO2KG + request.COKG);

        var nh2 = 0.88 * nco + 0.1;

        var Vh21000 = Vh2 * Cf * (1 - nh2);

        var Vgas1000 = Vco1000 + Vh21000 + Vn21000;

        var CO1000 = Vco1000 / Vgas1000;

        var H21000 = Vh21000 / Vgas1000;

        var N21000 = 1 - (CO1000 + H21000);

        var r0 = 28.0 / 22.4 * N21000 + 28 / 22.4 * CO1000 + 2 / 22.4 + H21000;

        var Qg1000 = Vgas1000 * request.P / (24 * 60 * 60);

        var _Dh_ = (request.Dg + request.Dp) / 2;

        var Wh = 4 * Qg1000 / (Math.PI * _Dh_ * _Dh_);

        var C0 = 1.2897 + 0.000121 * request.Td;

        var Ch2o = 1.4560 + 0.000282 * request.Td;

        var Id = C0 * request.Td - 0.00124 * request.Fd * (10800 - Ch2o * request.Td);

        var Ic = request.Ck * request.Tc;

        var C0tt = (request.Wc + Vd_ * Id + Ic + m * (request.Ws + Vd__ * Id))
                   / (Vg_ + m * Vg__);

        var Tt = 165 + 0.6113 * C0tt;

        var Th = (Tt + 1e3) / 2;

        var Vd = (Qd * request.P) / 1440;

        var W = ((Vd + request.Vpg * request.P / 1440) * (request.Td + 273) * 77.73)
                / (request.n * request.Df2 * (1 + request.Pd));

        var Vt = -9.1 * 1e-5 + 2.65 * 1e-7 * request.Td;

        var Re = 0.001 * W * request.Df2 / Vt;

        var lambdaTP = 0.0032 + 0.221 / Math.Pow(Re, 0.237);

        var deltaRvfPA = request.n
            * lambdaTP * request.L / (0.001 * request.Df2)
            * r0
            * W * W / 2
            * (request.Td + 273) / 273
            * 1 / (1 + request.Pd);

        var deltaRvf = deltaRvfPA / 980665;

        var deltaRh = request.DeltaRhISM - deltaRvf;

        var alfa = deltaRh / (deltaRh + request.DeltaPh);

        var Pg1000 = request.Pd - deltaRvf - request.DeltaPh;

        var _Ph_ = (request.Pd - deltaRvf + 1 + Pg1000 + 1) / 2;

        var HiiiH = request.h3 + request.hp + 0.5;

        var deK = 100 / request.AiKs
            .Zip(request.DiKs, (a, b) => a / b)
            .Sum();

        var ek = 0.3 * Math.Pow(0.1 * deK, 0.252);

        var Vshl = request.Ushl / request.pshl;

        var Vshl_ = Vshl * request.mu * request.nu;

        var Vk = request.k / request.YhK;

        var Vob = Vk * ek;

        var Vob_ = Vob - Vshl_;

        var ek_ = Vob_ / Vk;

        var lambdah = deltaRh / (
            (Wh * Wh / 2)
            * r0
            * (HiiiH / deK)
            * ((1 - ek_) / Math.Pow(ek_, 3))
            * ((Th + 273) / 273)
            * (1 / _Ph_)
        );

        var _Sh_ = (Math.PI * _Dh_ * _Dh_) / 4;

        var kH = request.VdKIP / (Wh * _Sh_);

        var Ah = lambdah
                 * (HiiiH / deK)
                 * (r0 / 2)
                 * Math.Pow(1 / (_Sh_ * kH), 2)
                 * (1 - ek_) / Math.Pow(ek_, 3)
                 * (Th + 273) / 273
                 * (1 / _Ph_);

        var deltaRhRasch = Ah * Vd * Vd;

        var Vco2I = 0.01 * request.Gi * request.PMPPi * (22.4 / 44);

        var Vco2KB = Vco1000 * nco;

        var Vco2KG = Vco2KB + Vco2I;

        var VcoKG = Vco1000 - Vco2KB;

        var Vn2KG = Vn21000;

        var Vh2KG = Vh21000;

        var Vch4KG = Cch4 * 22.4 / 12;

        var Vkg = Vco2KG + VcoKG + Vn2KG + Vh2KG + Vch4KG;

        var CO2 = Vco2KG / Vkg;

        var CO = VcoKG / Vkg;

        var H2 = Vh2KG / Vkg;

        var CH4 = Vch4KG / Vkg;

        var N2 = 1 - (CO2 + CO + H2 + CH4);

        var r0V = (44 / 22.4) * CO2
                  + (28 / 22.4) * CO
                  + (2 / 22.4) * H2
                  + (16 / 22.4) * CH4
                  + (28 / 22.4) * N2;

        var QkgC = (Vkg * request.P) / (24 * 60 * 60);

        var _Dv_ = (request.Dk + request.Dr) / 2;

        var Wv = 4 * QkgC / (Math.PI * _Dv_ * _Dv_);

        var Gok = request.Gjrm * request.alfaokAgl;

        var Gagl = request.Gjrm - Gok;

        var Gk = 0.001 * request.k;

        var deAgl = 100 / request.AiAgls
            .Zip(request.DiAgls, (a, b) => a / b)
            .Sum();

        var eAgl = 1 - (1 - request.ej) * request.ak -
                   request.EiAgls
                       .Zip(request.AiAgls, request.DiAgls)
                       .Select(x => (ei: x.First, ai: x.Second, di: x.Third))
                       .Aggregate(0.0, (acc, cur) => acc += (1 - cur.ei) * cur.ai
                                                                         * (1.582
                                                                            - 2.416 * (cur.di / request.Dk)
                                                                            + 1.485 * Math.Pow(cur.di / request.Dk, 2)
                                                                            + 0.18 * (request.ak / cur.ai)
                                                                            - 0.015 * Math.Pow(request.ak / cur.ai, 2)
                                                                         ));

        var eok = 0.4 - 0.25 * request.M_05;

        var Gsh = Gagl + Gok * Gk;

        var Vagl = Gagl / request.YhAgl;

        var Vok = Gok / request.YhOk;

        var Vk2 = Gk / request.YhK;

        var Vsh = Vagl + Vok * Vk2;

        var YhSH = Gsh / Vsh;

        var alfaAgl = Vagl / Vsh;

        var alfaOk = Vok / Vsh;

        var alfaK = Vk2 / alfaAgl;

        var eShB = (Vagl * eAgl + Vok * eok + Vk2 * ek) / Vsh;

        var deB = 1 / (alfaAgl / deAgl + alfaOk / request.deOk + alfaK / deK);

        var HshB = request.hsh + request.hk - request.hzas;

        var _tB_ = (request.Tkg + 1e3) / 2;

        var _PgB_ = ((Pg1000 + 1) + (request.Pkg + 1)) / 2;

        var lambdaB = request.deltaRb / (
            (Wv * Wv) / 2
            * r0V
            * (HshB / request.deBSh)
            * ((1 - request.eb) / Math.Pow(request.eb, 3))
            * ((_tB_ + 273) / 273)
            * (1 / _PgB_)
        );

        var kb = 4 * request.VdKIP / (Wv * Math.PI * _Dv_ * _Dv_);

        var Ab = lambdaB
                 * (HshB / deB)
                 * (r0V / 2)
                 * Math.Pow(4 / (Math.PI * _Dv_ * _Dv_ * kb), 2)
                 * ((1 - request.eb) / Math.Pow(request.eb, 3))
                 * ((_tB_ + 273) / 273)
                 * (1 / _PgB_);

        var deltaRbRasch = Ab * Vd * Vd;

        var Ha = request.hz + request.hp + request.hsh + request.hk - request.hzas + 0.5;

        var SU = 1e3 * (request.Pd - deltaRvf - request.Pkg) / (Ha * YhSH);

        var deltaPkp = request.SUkp * Ha * YhSH / 1e3;

        var VdKP = Math.Sqrt(deltaPkp / (Ah + Ab));

        var Wgorn = 4 * Qgg * request.P / (24 * 60 * 60 * Math.PI * request.Dg * request.Dg);

        var WgornFact = Wgorn * (Tt + 273) / (273 * (1 + request.Pd) * ek);

        var Wrasp = 4 * Vgas1000 * request.P / (24 * 60 * 60 * Math.PI * request.Dg * request.Dg);

        var WraspFact = Wrasp * (1e3 + 273) / (273 * (1 + Pg1000) * ek_);

        var Wkolosh = 4 * Vkg * request.P / (24 * 60 * 60 * Math.PI * request.Dk * request.Dk);

        var WkoloshFact = Wkolosh * (request.Tkg + 273) / (273 * (1 + request.Pkg) * eShB);

        return new ResponseData
        {
            SU = SU,
            DeltaPkp = deltaPkp,
            VdKP = VdKP,
            Wgorn = Wgorn,
            WgornFact = WgornFact,
            Wrasp = Wrasp,
            WraspFact = WraspFact,
            Wkolosh = Wkolosh,
            WkoloshFact = WkoloshFact
        };
    }
}