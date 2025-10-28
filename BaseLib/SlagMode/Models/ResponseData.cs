using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLib.SlagMode.Models
{
    public class ResponseData
    {
        [DisplayName("Основность шлака CaO / SiO2")]
        public double slagBasicity1 { get; set; }

        [DisplayName("Основность шлака (CaO + MgO) / SiO2")]
        public double slagBasicity2 { get; set; }

        [DisplayName("Основность шлака (CaO + MgO) / (SiO2 + Al2O3)")]
        public double slagBasicity3 { get; set; }

        [DisplayName("Основность шлака по Куликову")]
        public double slagBasicityKulikov { get; set; }

        [DisplayName("Расчётный выход шлака")]
        public double slagOut { get; set; }

        [DisplayName("Расход материалов")]
        public double materialCons { get; set; }

        [DisplayName("Всего агломерата с фабрик")]
        public double totalAglo { get; set; }

        [DisplayName("Доля агломерата с фабрик 2 и 3")]
        public double propAglo23 { get; set; }

        [DisplayName("Доля агломерата с фабрики 4")]
        public double propAglo4 { get; set; }

        [DisplayName("Доля местного агломерата")]
        public double propAglo234
        {
            get => _propAglo234;
            set
            {
                _propAglo234 = propAglo23 + propAglo4;
            }
        }

        private double _propAglo234;

        [DisplayName("Доля окатышей ССГПО")]
        public double propSsgpo { get; set; }

        [DisplayName("Доля окатышей Лебединского гОК")]
        public double propLeb { get; set; }

        [DisplayName("Доля окатышей Качканарского ГОК")]
        public double propKach { get; set; }

        [DisplayName("Доля окатышей Михайловского ГОК")]
        public double propMix { get; set; }

        [DisplayName("Доля руды")]
        public double propOre { get; set; }

        [DisplayName("Доля сварочного шлака")]
        public double propWeldSlag { get; set; }

        [DisplayName("Доля доменного присада")]
        public double propBFAddict { get; set; }

        [DisplayName("Доля королька")]
        public double propMinInclude { get; set; }

        [DisplayName("Общее количество аглометара")]
        public double totalProp
        {
            get => _totalProp;
            set
            {
                _totalProp = propAglo23 + propAglo4 + propSsgpo + propLeb + propKach + propMix + propOre + propWeldSlag + propBFAddict + propMinInclude;
            }
        }

        private double _totalProp;

        [DisplayName("Вязкость шлака при 1400 °С")]
        public double Viscosity_1400 { get; set; }

        [DisplayName("Вязкость шлака при 1450 °С")]
        public double Viscosity_1450 { get; set; }

        [DisplayName("Вязкость шлака при 1500 °С")]
        public double Viscosity_1500 { get; set; }

        [DisplayName("Вязкость шлака при 1550 °С")]
        public double Viscosity_1550 { get; set; }

        [DisplayName("Темпеоаьуоа шлака при 7 пуаз")]
        public double Temp_7_puaz { get; set; }

        [DisplayName("Градиент при 7-25 Пуаз")]
        public double Gradient_7_25 { get; set; }

        [DisplayName("Градиент при 1400-1500 °С")]
        public double Gradient_1400_1500 { get; set; }

        [DisplayName("Температура шлака")]
        public double SlagTemperature { get; set; }

        [DisplayName("Температура шлака (при 25 пуаз), °С")]
        public double slagTemperature_25puaz { get; set; }

        [DisplayName("Вязкость шлака при текущей температуре")]
        public double CurrSlagViscosity { get; set; }

        [DisplayName("Расчётный выход шлака по балансу шлакообразующих компонентов")]
        public double BalSlagMass { get; set; }

        [DisplayName("Выход шлака (баланс CaO")]
        public double CaOBalSlagMass { get; set; }

        [DisplayName("Масса серы, вносимая в печь, кг/т чугуна")]
        public double TotalSInOre { get; set; }

        [DisplayName("Коэффициент активности серы в чугуне")]
        public double SActivity { get; set; }

        [DisplayName("Коэффициент распределение серы")]
        public double SDistribution { get; set; }

        [DisplayName("Содержание серы в чугуне, %")]
        public double SContentInCastIron { get; set; }

        [DisplayName("Температура чугуна")]
        public double CastIronTemp { get; set; }
    }
}