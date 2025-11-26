import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Activity, Flame, Gauge, Package, Droplets, ArrowUpDown, TrendingUp, Factory, Zap, Wind as WindIcon } from 'lucide-react';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';
import { GasDynamicCharts } from './GasDynamicCharts';

interface GasDynamicResultsProps {
  results: any | null;
}

export function GasDynamicResults({ results }: GasDynamicResultsProps) {
  // Используем переданные результаты или mock данные
  const data = results || {
    aglomOutput: {
      aglomPorosity: 0,
      okatPorosity: 0
    },
    blastFurnace: null
  };
  
  // Если нет результатов расчета
  if (!results || !results.blastFurnace) {
    return (
      <Card>
        <CardContent className="py-12">
          <div className="text-center text-muted-foreground">
            Результаты расчета будут отображены после выполнения расчета
          </div>
        </CardContent>
      </Card>
    );
  }

  interface ResultRowProps {
    label: string;
    value: number;
    unit?: string;
    precision?: number;
  }

  function ResultRow({ label, value, unit = '', precision = 2 }: ResultRowProps) {
    return (
      <div className="flex justify-between items-center py-2 px-3 hover:bg-accent/50 rounded transition-colors">
        <span className="text-sm text-muted-foreground">{label}</span>
        <span className="font-mono">
          {value.toFixed(precision)} {unit}
        </span>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Порозность */}
      <Card className="border-primary/20">
        <CardHeader>
          <div className="flex items-center gap-2">
            <Package className="size-5 text-blue-500" />
            <CardTitle>Пористость материалов</CardTitle>
          </div>
          <CardDescription>Расчетные значения пористости</CardDescription>
        </CardHeader>
        <CardContent className="space-y-1">
          <ResultRow
            label="Порозность Агломерата"
            value={data.aglomOutput.aglomPorosity}
            precision={3}
          />
          <ResultRow
            label="Порозность Окатышей"
            value={data.aglomOutput.okatPorosity}
            precision={3}
          />
        </CardContent>
      </Card>

      <Tabs defaultValue="carbon" className="w-full">
        <TabsList className="grid w-full grid-cols-5">
          <TabsTrigger value="carbon">Углерод</TabsTrigger>
          <TabsTrigger value="charge">Шихта</TabsTrigger>
          <TabsTrigger value="gas">Газы</TabsTrigger>
          <TabsTrigger value="hydro">Гидродинамика</TabsTrigger>
          <TabsTrigger value="thermal">Тепло</TabsTrigger>
        </TabsList>

        {/* Углеродный баланс */}
        <TabsContent value="carbon" className="space-y-6">
          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Activity className="size-5 text-green-500" />
                <CardTitle>Углеродный баланс</CardTitle>
              </div>
              <CardDescription>Распределение углерода в процессе</CardDescription>
            </CardHeader>
            <CardContent className="space-y-1">
              <ResultRow
                label="Количество углерода (С), пришедшего в печь с коксом"
                value={data.blastFurnace.carbonBalance.c_Input}
                unit="кг"
              />
              <ResultRow
                label="Расход С на прямое восстановление оксидов Fe, Mn, Si, а также на десульфурацию"
                value={data.blastFurnace.carbonBalance.c_Out_Vosstan}
                unit="кг"
              />
              <ResultRow
                label="Растворяется углерода в чугуну"
                value={data.blastFurnace.carbonBalance.c_Out_Chugun}
                unit="кг"
              />
              <ResultRow
                label="Расход С на образование метана"
                value={data.blastFurnace.carbonBalance.c_Out_Metan}
                unit="кг"
              />
              <ResultRow
                label="Количество С сгорающего у фурм"
                value={data.blastFurnace.carbonBalance.c_Out_Furm}
                unit="кг"
              />
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Factory className="size-5 text-orange-500" />
                <CardTitle>Расход материалов</CardTitle>
              </div>
              <CardDescription>Удельные расходы компонентов</CardDescription>
            </CardHeader>
            <CardContent className="space-y-1">
              <ResultRow
                label="Удельный расход агломерата"
                value={data.blastFurnace.materialConsumption.udeln_Aglo}
                unit="т/т чугуна"
              />
              <ResultRow
                label="Удельный расход окатышей"
                value={data.blastFurnace.materialConsumption.udeln_Okat}
                unit="т/т чугуна"
              />
              <ResultRow
                label="Удельный расход известняка"
                value={data.blastFurnace.materialConsumption.udeln_Izvest}
                unit="кг/т чугуна"
              />
              <ResultRow
                label="Удельный расход кокса, выраженный в т/т чугуна"
                value={data.blastFurnace.materialConsumption.udeln_Koks_1000}
                unit="т/т чугуна"
              />
              <ResultRow
                label="Масса шихты на 1 тонну чугуна"
                value={data.blastFurnace.materialConsumption.udeln_Sum}
                unit="т/т чугуна"
              />
              <ResultRow
                label="Расход природного газа в расчёте на 1кг кокса"
                value={data.blastFurnace.materialConsumption.rashod_Prir_Gaz}
                unit="м³/кг"
              />
            </CardContent>
          </Card>
        </TabsContent>

        {/* Шихта и упаковка */}
        <TabsContent value="charge" className="space-y-6">
          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Package className="size-5 text-purple-500" />
                <CardTitle>Объёмы и доли шихтовых материалов</CardTitle>
              </div>
              <CardDescription>Параметры загрузки печи</CardDescription>
            </CardHeader>
            <CardContent className="grid gap-4 md:grid-cols-2">
              <div className="space-y-1">
                <ResultRow
                  label="Объём агломерата на 1т чугуна"
                  value={data.blastFurnace.chargeAndPacking.volume_Aglo_1chugun}
                  unit="м³"
                />
                <ResultRow
                  label="Объём окатышей на 1т чугуна"
                  value={data.blastFurnace.chargeAndPacking.volume_Okat_1chugun}
                  unit="м³"
                />
                <ResultRow
                  label="Объём кокса на 1т чугуна"
                  value={data.blastFurnace.chargeAndPacking.volume_Koks_1chugun}
                  unit="м³"
                />
                <ResultRow
                  label="Суммарный объём шихты"
                  value={data.blastFurnace.chargeAndPacking.volume_Sum_1chugun}
                  unit="м³"
                />
              </div>
              <div className="space-y-1">
                <ResultRow
                  label="Объёмная доля агломерата"
                  value={data.blastFurnace.chargeAndPacking.shihta_Dolya_Aglo}
                  precision={3}
                />
                <ResultRow
                  label="Объёмная доля окатышей"
                  value={data.blastFurnace.chargeAndPacking.shihta_Dolya_Okat}
                  precision={3}
                />
                <ResultRow
                  label="Объёмная доля кокса"
                  value={data.blastFurnace.chargeAndPacking.shihta_Dolya_Koks}
                  precision={3}
                />
                <ResultRow
                  label="Насыпная масса слоя шихты"
                  value={data.blastFurnace.chargeAndPacking.massa_Nasyp_Shihta}
                  unit="кг/м³"
                />
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Gauge className="size-5 text-cyan-500" />
                <CardTitle>Структурные параметры</CardTitle>
              </div>
              <CardDescription>Размеры и пористость слоев</CardDescription>
            </CardHeader>
            <CardContent className="grid gap-4 md:grid-cols-2">
              <div className="space-y-1">
                <ResultRow
                  label="Порозность слоя шихты в верхней зоне"
                  value={data.blastFurnace.chargeAndPacking.shihta_Porozn_Verh}
                  precision={3}
                />
                <ResultRow
                  label="Эквивалентный диаметр куска шихты в верхней зоне"
                  value={data.blastFurnace.chargeAndPacking.shihta_Diam_Verh}
                  unit="мм"
                />
                <ResultRow
                  label="Эквивалентный диаметр кусков кокса"
                  value={data.blastFurnace.chargeAndPacking.diam_Koks}
                  unit="мм"
                />
                <ResultRow
                  label="Порозность слоя кокса"
                  value={data.blastFurnace.chargeAndPacking.porozn_Koks}
                  precision={3}
                />
              </div>
              <div className="space-y-1">
                <ResultRow
                  label="Удельный объём образующегося шлака"
                  value={data.blastFurnace.chargeAndPacking.volume_Udeln_Shlak}
                  unit="м³"
                />
                <ResultRow
                  label="Скорректированная порозность слоя"
                  value={data.blastFurnace.chargeAndPacking.porozn_Sloy_Korrekt}
                  precision={3}
                />
                <ResultRow
                  label="Эквивалентный диаметр куска агломерата"
                  value={data.blastFurnace.chargeAndPacking.diam_Aglo}
                  unit="мм"
                />
                <ResultRow
                  label="Порозность слоя агломерата"
                  value={data.blastFurnace.chargeAndPacking.porozn_Aglo}
                  precision={3}
                />
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <TrendingUp className="size-5 text-indigo-500" />
                <CardTitle>Геометрия печи</CardTitle>
              </div>
              <CardDescription>Расчетные размеры зон печи</CardDescription>
            </CardHeader>
            <CardContent className="space-y-1">
              <ResultRow
                label="Среднее значение поперечного сечения нижней зоны печи"
                value={data.blastFurnace.furnaceGeometry.s_Sech_Niz}
                unit="м²"
              />
              <ResultRow
                label="Средний диаметр нижней части доменной печи"
                value={data.blastFurnace.furnaceGeometry.diam_Niz}
                unit="м"
              />
              <ResultRow
                label="Средний диаметр верхней части печи"
                value={data.blastFurnace.furnaceGeometry.diam_Verh}
                unit="м"
              />
              <ResultRow
                label="Высота слоя шихты в нижней зоне печи"
                value={data.blastFurnace.furnaceGeometry.height_Shihta_Niz}
                unit="м"
              />
              <ResultRow
                label="Высота слоя шихты в верхней зоне печи"
                value={data.blastFurnace.furnaceGeometry.shihta_Height_Verh}
                unit="м"
              />
              <ResultRow
                label="Активная высота слоя шихты продуваемая газами"
                value={data.blastFurnace.furnaceGeometry.height_Aktiv}
                unit="м"
              />
            </CardContent>
          </Card>
        </TabsContent>

        {/* Газы */}
        <TabsContent value="gas" className="space-y-6">
          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Flame className="size-5 text-red-500" />
                <CardTitle>Горновой газ</CardTitle>
              </div>
              <CardDescription>Параметры фурменного (горнового) газа</CardDescription>
            </CardHeader>
            <CardContent className="grid gap-4 md:grid-cols-2">
              <div className="space-y-1">
                <ResultRow
                  label="Выход фурменного газа на 1кг С кокса"
                  value={data.blastFurnace.hearthGas.furmgaz_Koks}
                  unit="м³/кг"
                />
                <ResultRow
                  label="Выход фурменного газа при конверсии 1м³ природного газа"
                  value={data.blastFurnace.hearthGas.furmgaz_Prir_Gaz}
                  unit="м³/м³"
                />
                <ResultRow
                  label="Суммарный выход фурменного газа"
                  value={data.blastFurnace.hearthGas.furmgaz_Sum}
                  unit="м³"
                />
                <ResultRow
                  label="Удельный выход фурменного (горнового) газа"
                  value={data.blastFurnace.hearthGas.furmgaz_Udeln}
                  unit="м³/т"
                />
              </div>
              <div className="space-y-1">
                <ResultRow
                  label="Содержание CO в горновом газе"
                  value={data.blastFurnace.hearthGas.furmgaz_CO}
                  unit="м³"
                />
                <ResultRow
                  label="Содержание H₂ в горновом газе"
                  value={data.blastFurnace.hearthGas.furmgaz_H2}
                  unit="м³"
                />
                <ResultRow
                  label="Содержание N₂ в горновом газе"
                  value={data.blastFurnace.hearthGas.furmgaz_N2}
                  unit="м³"
                />
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Flame className="size-5 text-yellow-500" />
                <CardTitle>Тепловые параметры горнового газа</CardTitle>
              </div>
            </CardHeader>
            <CardContent className="space-y-1">
              <ResultRow
                label="Теплосодержание горновых газов при теоретической температуре горения"
                value={data.blastFurnace.hearthGas.teplosod_Furmgaz}
                unit="кДж/м³"
              />
              <ResultRow
                label="Теоретическая температура горения"
                value={data.blastFurnace.hearthGas.temp_Teor}
                unit="°C"
              />
              <ResultRow
                label="Средняя температура газов в нижней зоне печи"
                value={data.blastFurnace.hearthGas.temp_Sredn_Niz}
                unit="°C"
              />
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Zap className="size-5 text-amber-500" />
                <CardTitle>Промежуточный газ на горизонте 1000°C</CardTitle>
              </div>
              <CardDescription>Состав доменного газа при t=1000°C</CardDescription>
            </CardHeader>
            <CardContent className="grid gap-4 md:grid-cols-2">
              <div className="space-y-1">
                <ResultRow
                  label="Объём CO₂ при восстановлении"
                  value={data.blastFurnace.intermediateGas1000.volume_CO_Pvost}
                  unit="м³"
                />
                <ResultRow
                  label="Объём CO при t=1000°C"
                  value={data.blastFurnace.intermediateGas1000.volume_CO_1000}
                  unit="м³"
                />
                <ResultRow
                  label="Объём H₂ при t=1000°C"
                  value={data.blastFurnace.intermediateGas1000.volume_H2_1000}
                  unit="м³"
                />
                <ResultRow
                  label="Объём N₂ при t=1000°C"
                  value={data.blastFurnace.intermediateGas1000.volume_N2_1000}
                  unit="м³"
                />
                <ResultRow
                  label="Суммарный объём при t=1000°C"
                  value={data.blastFurnace.intermediateGas1000.volume_Sum_1000}
                  unit="м³"
                />
              </div>
              <div className="space-y-1">
                <ResultRow
                  label="Состав газа CO на горизонте t=1000°C"
                  value={data.blastFurnace.intermediateGas1000.domengaz_CO_1000}
                  unit="%"
                />
                <ResultRow
                  label="Состав газа H₂ на горизонте t=1000°C"
                  value={data.blastFurnace.intermediateGas1000.domengaz_H2_1000}
                  unit="%"
                />
                <ResultRow
                  label="Состав газа N₂ на горизонте t=1000°C"
                  value={data.blastFurnace.intermediateGas1000.domengaz_N2_1000}
                  unit="%"
                />
                <ResultRow
                  label="Плотность газа на горизонте 1000°C при н.у."
                  value={data.blastFurnace.intermediateGas1000.domengaz_Plotn_1000}
                  unit="кг/м³"
                />
                <ResultRow
                  label="Секундный расход доменного газа на горизонте 1000°C"
                  value={data.blastFurnace.intermediateGas1000.domengaz_Rashod_1000}
                  unit="кг/с"
                />
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <WindIcon className="size-5 text-sky-500" />
                <CardTitle>Колошниковый газ</CardTitle>
              </div>
              <CardDescription>Параметры выходящего газа</CardDescription>
            </CardHeader>
            <CardContent className="grid gap-4 md:grid-cols-2">
              <div className="space-y-1">
                <ResultRow
                  label="Объём CO₂ при разложении известняка"
                  value={data.blastFurnace.topGas.volume_CO2_Izvest}
                  unit="м³"
                />
                <ResultRow
                  label="Объём CO₂ при косвенном восстановлении"
                  value={data.blastFurnace.topGas.volume_CO2_Kvost}
                  unit="м³"
                />
                <ResultRow
                  label="Объём CO₂ в колошниковом газе"
                  value={data.blastFurnace.topGas.volume_CO2_Kolgaz}
                  unit="м³"
                />
                <ResultRow
                  label="Объём CO в колошниковом газе"
                  value={data.blastFurnace.topGas.volume_CO_Kolgaz}
                  unit="м³"
                />
                <ResultRow
                  label="Объём CH₄ в колошниковом газе"
                  value={data.blastFurnace.topGas.volume_CH4_Kolgaz}
                  unit="м³"
                />
                <ResultRow
                  label="Объём N₂ в колошниковом газе"
                  value={data.blastFurnace.topGas.volume_N2_Kolgaz}
                  unit="м³"
                />
                <ResultRow
                  label="Объём H₂ в колошниковом газе"
                  value={data.blastFurnace.topGas.volume_H2_Kolgaz}
                  unit="м³"
                />
              </div>
              <div className="space-y-1">
                <ResultRow
                  label="Удельный выход колошникового газа при н.у."
                  value={data.blastFurnace.topGas.udeln_Kolgaz}
                  unit="м³/т"
                />
                <ResultRow
                  label="Расчётный состав: CO₂"
                  value={data.blastFurnace.topGas.kolgaz_CO2}
                  unit="%"
                />
                <ResultRow
                  label="Расчётный состав: CO"
                  value={data.blastFurnace.topGas.kolgaz_CO}
                  unit="%"
                />
                <ResultRow
                  label="Расчётный состав: H₂"
                  value={data.blastFurnace.topGas.kolgaz_H2}
                  unit="%"
                />
                <ResultRow
                  label="Расчётный состав: CH₄"
                  value={data.blastFurnace.topGas.kolgaz_CH4}
                  unit="%"
                />
                <ResultRow
                  label="Расчётный состав: N₂"
                  value={data.blastFurnace.topGas.kolgaz_N2}
                  unit="%"
                />
                <ResultRow
                  label="Плотность колошникового газа при н.у."
                  value={data.blastFurnace.topGas.kolgaz_Plotn}
                  unit="кг/м³"
                />
                <ResultRow
                  label="Секундный выход колошникового газа"
                  value={data.blastFurnace.topGas.kolgaz_Minut}
                  unit="кг/с"
                />
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Гидродинамика */}
        <TabsContent value="hydro" className="space-y-6">
          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <ArrowUpDown className="size-5 text-blue-600" />
                <CardTitle>Гидродинамика нижней зоны</CardTitle>
              </div>
              <CardDescription>Параметры течения газа в нижней части печи</CardDescription>
            </CardHeader>
            <CardContent className="grid gap-4 md:grid-cols-2">
              <div className="space-y-1">
                <ResultRow
                  label="Скорость фильтрации газового потока при н.у."
                  value={data.blastFurnace.hydrodynamicsLower.speed_Filtr_Niz}
                  unit="м/с"
                />
                <ResultRow
                  label="Значение коэффициента трения"
                  value={data.blastFurnace.hydrodynamicsLower.tren_Koef}
                  precision={0}
                />
                <ResultRow
                  label="Значение коэффициента трения с учетом потерь на тракте"
                  value={data.blastFurnace.hydrodynamicsLower.tren_Sum}
                  precision={0}
                />
                <ResultRow
                  label="Потери напора дутья на воздушных фурмах"
                  value={data.blastFurnace.hydrodynamicsLower.poteri_Furm}
                  unit="атм"
                />
                <ResultRow
                  label="Нижний перепад давления газов по высоте слоя шихты"
                  value={data.blastFurnace.hydrodynamicsLower.perepad_Niz_Itog}
                  unit="атм"
                />
              </div>
              <div className="space-y-1">
                <ResultRow
                  label="Доля нижнего перепада давления от общего перепада"
                  value={data.blastFurnace.hydrodynamicsLower.perepad_Niz_Dolya}
                  precision={3}
                />
                <ResultRow
                  label="Избыточное давление газа на горизонте 1000°C"
                  value={data.blastFurnace.hydrodynamicsLower.davlen_Izb_1000}
                  unit="ати"
                />
                <ResultRow
                  label="Среднее значение абсолютного давления газа в нижней зоне"
                  value={data.blastFurnace.hydrodynamicsLower.davlen_Niz}
                  unit="атм"
                />
                <ResultRow
                  label="Коэффициент сопротивления"
                  value={data.blastFurnace.hydrodynamicsLower.koef_Soprot_Niz}
                  precision={0}
                />
                <ResultRow
                  label="Коэффициент АН"
                  value={data.blastFurnace.hydrodynamicsLower.koef_An}
                />
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <ArrowUpDown className="size-5 text-teal-600" />
                <CardTitle>Гидродинамика верхней зоны</CardTitle>
              </div>
              <CardDescription>Параметры течения газа в верхней части печи</CardDescription>
            </CardHeader>
            <CardContent className="grid gap-4 md:grid-cols-2">
              <div className="space-y-1">
                <ResultRow
                  label="Скорость фильтрации в верхней зоне"
                  value={data.blastFurnace.hydrodynamicsUpper.speed_Filtr_Verh}
                  unit="м/с"
                />
                <ResultRow
                  label="Скорость фильтрации через верхнюю часть горна при н.у."
                  value={data.blastFurnace.hydrodynamicsUpper.speed_Filtr_Gorn}
                  unit="м/с"
                />
                <ResultRow
                  label="Действительная скорость движения газа в верхней части горна"
                  value={data.blastFurnace.hydrodynamicsUpper.speed_Real_Verh}
                  unit="м/с"
                />
                <ResultRow
                  label="Скорость фильтрации в области распара"
                  value={data.blastFurnace.hydrodynamicsUpper.speed_Filtr_Raspar}
                  unit="м/с"
                />
                <ResultRow
                  label="Скорость движения газа в области распара"
                  value={data.blastFurnace.hydrodynamicsUpper.speed_Real_Raspar}
                  unit="м/с"
                />
                <ResultRow
                  label="Скорость фильтрации через колошник"
                  value={data.blastFurnace.hydrodynamicsUpper.speed_Filtr_Koloshnik}
                  unit="м/с"
                />
              </div>
              <div className="space-y-1">
                <ResultRow
                  label="Скорость движения газа через колошник"
                  value={data.blastFurnace.hydrodynamicsUpper.speed_Real_Koloshnik}
                  unit="м/с"
                />
                <ResultRow
                  label="Абсолютное среднее давление в верхней зоне"
                  value={data.blastFurnace.hydrodynamicsUpper.davlen_Verh}
                  unit="атм"
                />
                <ResultRow
                  label="Коэффициент сопротивления слоя шихты"
                  value={data.blastFurnace.hydrodynamicsUpper.koef_Soprot_Verh}
                  precision={0}
                />
                <ResultRow
                  label="Коэффициент АВ"
                  value={data.blastFurnace.hydrodynamicsUpper.koef_Av}
                />
                <ResultRow
                  label="Перепад давления газов по высоте слоя шихты"
                  value={data.blastFurnace.hydrodynamicsUpper.perepad_Davlen}
                  unit="атм"
                />
                <ResultRow
                  label="Степень уравновешивания шихты"
                  value={data.blastFurnace.hydrodynamicsUpper.stepen_Urav}
                  unit="%"
                />
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Тепловые параметры */}
        <TabsContent value="thermal" className="space-y-6">
          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Flame className="size-5 text-orange-600" />
                <CardTitle>Тепловые параметры</CardTitle>
              </div>
              <CardDescription>Теплофизические характеристики процесса</CardDescription>
            </CardHeader>
            <CardContent className="space-y-1">
              <ResultRow
                label="Теплоёмкость двуатомных газов при температуре горячего дутья"
                value={data.blastFurnace.thermalParameters.teploemk_2atom}
                unit="кДж/(кг·К)"
              />
              <ResultRow
                label="Теплоёмкость паров воды при температуре горячего дутья"
                value={data.blastFurnace.thermalParameters.teploemk_Voda}
                unit="кДж/(кг·К)"
              />
              <ResultRow
                label="Теплосодержание горячего дутья за вычетом теплоты разложения влаги дутья"
                value={data.blastFurnace.thermalParameters.teplosod_Dut}
                unit="кДж/м³"
              />
              <ResultRow
                label="Теплосодержание углерода кокса, пришедшего к фурмам"
                value={data.blastFurnace.thermalParameters.teplosod_Koks}
                unit="кДж/кг"
              />
              <ResultRow
                label="Средняя температура в верхней зоне"
                value={data.blastFurnace.thermalParameters.temp_Verh}
                unit="°C"
              />
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}