using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.SlagMode;

public class Response : Entity
{
        [DisplayName("Основность шлака CaO / SiO2")]
        public double SlagBasicity1 { get; set; }

        [DisplayName("Основность шлака (CaO + MgO) / SiO2")]
        public double SlagBasicity2 { get; set; }

        [DisplayName("Основность шлака (CaO + MgO) / (SiO2 + Al2O3)")]
        public double SlagBasicity3 { get; set; }

        [DisplayName("Основность шлака по Куликову")]
        public double SlagBasicityKulikov { get; set; }

        [DisplayName("Расчётный выход шлака")]
        public double SlagOut { get; set; }

        [DisplayName("Расход материалов")]
        public double MaterialCons { get; set; }

        [DisplayName("Всего агломерата с фабрик")]
        public double TotalAglo { get; set; }

        [DisplayName("Доля агломерата с фабрик 2 и 3")]
        public double PropAglo23 { get; set; }

        [DisplayName("Доля агломерата с фабрики 4")]
        public double PropAglo4 { get; set; }

        [DisplayName("Доля местного агломерата")]
        public double PropAglo234 => PropAglo23 + PropAglo4;

        [DisplayName("Доля окатышей ССГПО")]
        public double PropSsgpo { get; set; }

        [DisplayName("Доля окатышей Лебединского гОК")]
        public double PropLeb { get; set; }

        [DisplayName("Доля окатышей Качканарского ГОК")]
        public double PropKach { get; set; }

        [DisplayName("Доля окатышей Михайловского ГОК")]
        public double PropMix { get; set; }

        [DisplayName("Доля руды")]
        public double PropOre { get; set; }

        [DisplayName("Доля сварочного шлака")]
        public double PropWeldSlag { get; set; }

        [DisplayName("Доля доменного присада")]
        public double PropBfAddict { get; set; }

        [DisplayName("Доля королька")]
        public double PropMinInclude { get; set; }

        [DisplayName("Общее количество агломерата")]
        public double TotalProp => PropAglo23 + PropAglo4 + PropSsgpo + PropLeb + PropKach + PropMix + PropOre + PropWeldSlag + PropBfAddict + PropMinInclude;

        [DisplayName("Вязкость шлака при 1400 °С")]
        public double Viscosity_1400 { get; set; }

        [DisplayName("Вязкость шлака при 1450 °С")]
        public double Viscosity_1450 { get; set; }

        [DisplayName("Вязкость шлака при 1500 °С")]
        public double Viscosity_1500 { get; set; }

        [DisplayName("Вязкость шлака при 1550 °С")]
        public double Viscosity_1550 { get; set; }

        [DisplayName("Температура шлака при 7 пуаз")]
        public double Temp_7_Puaz { get; set; }

        [DisplayName("Градиент при 7-25 Пуаз")]
        public double Gradient_7_25 { get; set; }

        [DisplayName("Градиент при 1400-1500 °С")]
        public double Gradient_1400_1500 { get; set; }

        [DisplayName("Температура шлака")]
        public double SlagTemperature { get; set; }

        [DisplayName("Температура шлака (при 25 пуаз), °С")]
        public double SlagTemperature_25Puaz { get; set; }

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
        
        [ForeignKey(nameof(Request))]
        public int RequestID { get; set; }
        
        public Request Request { get; set; }
}