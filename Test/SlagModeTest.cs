

using BaseLib.SlagMode;
using BaseLib.SlagMode.Models;
using Microsoft.Extensions.Options;

namespace Test
{
    /// <summary>
    /// Тесты для либы SlagMode
    /// </summary>
    public class SlagModeTest (IOptions<ExternalServerDomain> serverAddress)
    {
        [Fact]
        public void GetKey()
        {
            var _mathLib = new SlagMode(serverAddress);

            var key = _mathLib.GetTokenFromServer(
                new UserAuthData 
                {
                    UserName = "Login",
                    Password = "Password"
                });
            var res = key.Split('.');
            Assert.Equal(3, res.Length);
        }

        [Fact]
        public void Calculate()
        {
            var _mathLib = new SlagMode(serverAddress);

            var requestData = PrepareData();
            var res = _mathLib.Calculate(requestData);
            AssertResponseDataEquals(res);
        }
        
        public static ResponseData GetResultData()
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
        
        private static void AssertResponseDataEquals(ResponseData expected, double tolerance = 1e-6)
        {
            var actual = GetResultData();
            Assert.Equal(expected.SlagBasicity1, actual.SlagBasicity1, tolerance);
            Assert.Equal(expected.SlagBasicity2, actual.SlagBasicity2, tolerance);
            Assert.Equal(expected.SlagBasicity3, actual.SlagBasicity3, tolerance);
            Assert.Equal(expected.SlagBasicityKulikov, actual.SlagBasicityKulikov, tolerance);
            Assert.Equal(expected.SlagOut, actual.SlagOut, tolerance);
            Assert.Equal(expected.MaterialCons, actual.MaterialCons, tolerance);
            Assert.Equal(expected.TotalAglo, actual.TotalAglo, tolerance);
            Assert.Equal(expected.PropAglo23, actual.PropAglo23, tolerance);
            Assert.Equal(expected.PropAglo4, actual.PropAglo4, tolerance);
            Assert.Equal(expected.PropSsgpo, actual.PropSsgpo, tolerance);
            Assert.Equal(expected.PropLeb, actual.PropLeb, tolerance);
            Assert.Equal(expected.PropKach, actual.PropKach, tolerance);
            Assert.Equal(expected.PropMix, actual.PropMix, tolerance);
            Assert.Equal(expected.PropOre, actual.PropOre, tolerance);
            Assert.Equal(expected.PropWeldSlag, actual.PropWeldSlag, tolerance);
            Assert.Equal(expected.PropBfAddict, actual.PropBfAddict, tolerance);
            Assert.Equal(expected.PropMinInclude, actual.PropMinInclude, tolerance);
            Assert.Equal(expected.Viscosity_1400, actual.Viscosity_1400, tolerance);
            Assert.Equal(expected.Viscosity_1450, actual.Viscosity_1450, tolerance);
            Assert.Equal(expected.Viscosity_1500, actual.Viscosity_1500, tolerance);
            Assert.Equal(expected.Viscosity_1550, actual.Viscosity_1550, tolerance);
            Assert.Equal(expected.Temp_7_Puaz, actual.Temp_7_Puaz, tolerance);
            Assert.Equal(expected.Gradient_7_25, actual.Gradient_7_25, tolerance);
            Assert.Equal(expected.Gradient_1400_1500, actual.Gradient_1400_1500, tolerance);
            Assert.Equal(expected.SlagTemperature, actual.SlagTemperature, tolerance);
            Assert.Equal(expected.SlagTemperature_25Puaz, actual.SlagTemperature_25Puaz, tolerance);
            Assert.Equal(expected.CurrSlagViscosity, actual.CurrSlagViscosity, tolerance);
            Assert.Equal(expected.BalSlagMass, actual.BalSlagMass, tolerance);
            Assert.Equal(expected.CaOBalSlagMass, actual.CaOBalSlagMass, tolerance);
            Assert.Equal(expected.TotalSInOre, actual.TotalSInOre, tolerance);
            Assert.Equal(expected.SActivity, actual.SActivity, tolerance);
            Assert.Equal(expected.SDistribution, actual.SDistribution, tolerance);
            Assert.Equal(expected.SContentInCastIron, actual.SContentInCastIron, tolerance);
            Assert.Equal(expected.CastIronTemp, actual.CastIronTemp, tolerance);
        }
        
        public static RequestData PrepareData()
        {
            return new RequestData
            {
                User = new UserAuthData 
                {
                    UserName = "Login",
                    Password = "Password"
                },

                CastIron = new InputCastIronForCalc
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

                InputCoke = new InputCokeForCalcs
                {
                    Consumption = 419.8,
                    Sulfur = 0.428,
                    AshAmount = 12.7,
                    AshCaOFraction = 7.8,
                    AshSiO2Fraction = 48.1,
                    AshAl2O3Fraction = 24.6,
                    AshMgOFraction = 2
                },

                Components = new List<InputChargeComponentsForCalc>
                {
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
                }
            };
        }
    }
}
