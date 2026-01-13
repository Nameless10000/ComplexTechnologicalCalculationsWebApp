import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { 
  LineChart, Line, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, 
  ResponsiveContainer, PieChart, Pie, Cell, AreaChart, Area, RadarChart, 
  PolarGrid, PolarAngleAxis, PolarRadiusAxis, Radar 
} from 'recharts';
import { TrendingUp, BarChart3, Activity, Flame, Wind, Gauge, Factory, Droplets } from 'lucide-react';

interface GasDynamicChartsProps {
  results: any;
}

const VIBRANT_COLORS = {
  blue: '#3b82f6',
  red: '#ef4444',
  green: '#10b981',
  orange: '#f97316',
  purple: '#a855f7',
  cyan: '#06b6d4',
  pink: '#ec4899',
  amber: '#f59e0b',
  emerald: '#10b981',
  indigo: '#6366f1',
  yellow: '#eab308',
  lime: '#84cc16',
  teal: '#14b8a6',
  rose: '#f43f5e',
  sky: '#0ea5e9'
};

export function GasDynamicCharts({ results }: GasDynamicChartsProps) {
  if (!results) {
    return (
      <Card>
        <CardContent className="py-12">
          <div className="text-center text-muted-foreground">
            Выполните расчет для отображения графиков
          </div>
        </CardContent>
      </Card>
    );
  }

  // Данные для графика углеродного баланса
  const carbonBalanceData = [
    {
      name: 'Приход с коксом',
      value: results.blastFurnace?.carbonBalance?.c_Input || 0,
      fill: VIBRANT_COLORS.blue
    },
    {
      name: 'Прямое восстановление',
      value: results.blastFurnace?.carbonBalance?.c_Out_Vosstan || 0,
      fill: VIBRANT_COLORS.orange
    },
    {
      name: 'Растворение в чугуне',
      value: results.blastFurnace?.carbonBalance?.c_Out_Chugun || 0,
      fill: VIBRANT_COLORS.green
    },
    {
      name: 'Образование метана',
      value: results.blastFurnace?.carbonBalance?.c_Out_Metan || 0,
      fill: VIBRANT_COLORS.purple
    },
    {
      name: 'Сгорание у фурм',
      value: results.blastFurnace?.carbonBalance?.c_Out_Furm || 0,
      fill: VIBRANT_COLORS.red
    },
  ];

  // Данные для графика состава колошникового газа (круговая диаграмма)
  const topGasComposition = [
    { name: 'CO₂', value: results.blastFurnace?.topGas?.kolgaz_CO2 || 0, fill: VIBRANT_COLORS.red },
    { name: 'CO', value: results.blastFurnace?.topGas?.kolgaz_CO || 0, fill: VIBRANT_COLORS.blue },
    { name: 'H₂', value: results.blastFurnace?.topGas?.kolgaz_H2 || 0, fill: VIBRANT_COLORS.green },
    { name: 'CH₄', value: results.blastFurnace?.topGas?.kolgaz_CH4 || 0, fill: VIBRANT_COLORS.amber },
    { name: 'N₂', value: results.blastFurnace?.topGas?.kolgaz_N2 || 0, fill: VIBRANT_COLORS.purple },
  ];

  // Данные для графика температур по зонам (area chart)
  const temperatureData = [
    {
      zone: 'Горновой газ',
      temperature: results.blastFurnace?.hearthGas?.temp_Teor || 0,
    },
    {
      zone: 'Нижняя зона',
      temperature: results.blastFurnace?.hearthGas?.temp_Sredn_Niz || 0,
    },
    {
      zone: 'Верхняя зона',
      temperature: (results.blastFurnace?.hearthGas?.temp_Sredn_Niz || 0) * 0.7, // Примерная оценка
    },
    {
      zone: 'Колошник',
      temperature: results.blastFurnace?.thermalParameters?.temp_Verh || 150,
    },
  ];

  // Данные для графика перепадов давления
  const pressureData = [
    {
      zone: 'Нижняя зона',
      pressure: results.blastFurnace?.hydrodynamicsLower?.perepad_Niz_Itog || 0,
      percentage: (results.blastFurnace?.hydrodynamicsLower?.perepad_Niz_Dolya || 0) * 100
    },
    {
      zone: 'Верхняя зона',
      pressure: results.blastFurnace?.hydrodynamicsUpper?.perepad_Verh_Itog || 0,
      percentage: (results.blastFurnace?.hydrodynamicsUpper?.perepad_Verh_Dolya || 0) * 100
    },
  ];

  // Данные для радарной диаграммы объемных долей материалов
  const chargeCompositionData = [
    {
      material: 'Агломерат',
      value: (results.blastFurnace?.chargeAndPacking?.shihta_Dolya_Aglo || 0) * 100,
    },
    {
      material: 'Окатыши',
      value: (results.blastFurnace?.chargeAndPacking?.shihta_Dolya_Okat || 0) * 100,
    },
    {
      material: 'Кокс',
      value: (results.blastFurnace?.chargeAndPacking?.shihta_Dolya_Koks || 0) * 100,
    },
  ];

  // Данные для графика скоростей фильтрации
  const velocityData = [
    {
      zone: 'Нижняя зона',
      velocity: results.blastFurnace?.hydrodynamicsLower?.speed_Filtr_Niz || 0,
    },
    {
      zone: 'Верхняя зона',
      velocity: results.blastFurnace?.hydrodynamicsUpper?.speed_Filtr_Verh || 0,
    },
    {
      zone: 'Горн',
      velocity: results.blastFurnace?.hydrodynamicsUpper?.speed_Filtr_Gorn || 0,
    },
    {
      zone: 'Распар',
      velocity: results.blastFurnace?.hydrodynamicsUpper?.speed_Filtr_Raspar || 0,
    },
    {
      zone: 'Колошник',
      velocity: results.blastFurnace?.hydrodynamicsUpper?.speed_Filtr_Koloshnik || 0,
    },
  ];

  // Данные для сравнения расхода материалов
  const materialConsumptionData = [
    {
      material: 'Агломерат',
      value: results.blastFurnace?.materialConsumption?.udeln_Aglo || 0,
    },
    {
      material: 'Окатыши',
      value: results.blastFurnace?.materialConsumption?.udeln_Okat || 0,
    },
    {
      material: 'Известняк',
      value: (results.blastFurnace?.materialConsumption?.udeln_Izvest || 0) / 1000,
    },
    {
      material: 'Кокс',
      value: results.blastFurnace?.materialConsumption?.udeln_Koks_1000 || 0,
    },
  ];

  // Данные для графика выхода газов
  const gasOutputData = [
    {
      type: 'Фурменный газ',
      volume: results.blastFurnace?.hearthGas?.furmgaz_Udeln || 0,
    },
    {
      type: 'Промежуточный (1000°C)',
      volume: (results.blastFurnace?.intermediateGas1000?.volume_Sum_1000 || 0) / 1000,
    },
    {
      type: 'Колошниковый газ',
      volume: results.blastFurnace?.topGas?.udeln_Kolgaz || 0,
    },
  ];

  return (
    <div className="space-y-6">
      {/* График углеродного баланса - красивая столбчатая диаграмма */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Activity className="size-5 text-emerald-500" />
            Углеродный баланс доменной плавки
          </CardTitle>
          <CardDescription>Распределение углерода в процессе (кг на тонну чугуна)</CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={350}>
            <BarChart data={carbonBalanceData}>
              <defs>
                {carbonBalanceData.map((item, index) => (
                  <linearGradient key={`gradient-${index}`} id={`colorGradient${index}`} x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor={item.fill} stopOpacity={0.9}/>
                    <stop offset="95%" stopColor={item.fill} stopOpacity={0.6}/>
                  </linearGradient>
                ))}
              </defs>
              <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
              <XAxis 
                dataKey="name" 
                angle={-15} 
                textAnchor="end" 
                height={100} 
                className="text-xs"
              />
              <YAxis 
                label={{ value: 'кг', angle: -90, position: 'insideLeft' }}
                className="text-sm"
              />
              <Tooltip 
                contentStyle={{ 
                  backgroundColor: 'hsl(var(--background))', 
                  border: '1px solid hsl(var(--border))',
                  borderRadius: '8px'
                }}
                formatter={(value: number) => [value.toFixed(2) + ' кг', 'Количество']}
              />
              <Bar dataKey="value" radius={[8, 8, 0, 0]}>
                {carbonBalanceData.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={`url(#colorGradient${index})`} />
                ))}
              </Bar>
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      <div className="grid gap-6 md:grid-cols-2">
        {/* Круговая диаграмма состава колошникового газа */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Wind className="size-5 text-sky-500" />
              Состав колошникового газа
            </CardTitle>
            <CardDescription>Процентное соотношение компонентов выходящего газа</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={320}>
              <PieChart>
                <Pie
                  data={topGasComposition}
                  cx="50%"
                  cy="50%"
                  labelLine={true}
                  label={({ name, value }) => `${name}: ${value.toFixed(1)}%`}
                  outerRadius={100}
                  fill="#8884d8"
                  dataKey="value"
                  strokeWidth={2}
                  stroke="hsl(var(--background))"
                >
                  {topGasComposition.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.fill} />
                  ))}
                </Pie>
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'hsl(var(--background))', 
                    border: '1px solid hsl(var(--border))',
                    borderRadius: '8px'
                  }}
                  formatter={(value: number) => [value.toFixed(2) + '%', 'Доля']}
                />
              </PieChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Радарная диаграмма объемных долей шихты */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Factory className="size-5 text-purple-500" />
              Объемные доли шихтовых материалов
            </CardTitle>
            <CardDescription>Распределение компонентов шихты по объему</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={320}>
              <RadarChart data={chargeCompositionData}>
                <PolarGrid stroke={VIBRANT_COLORS.purple} strokeOpacity={0.3} />
                <PolarAngleAxis 
                  dataKey="material" 
                  className="text-sm"
                />
                <PolarRadiusAxis 
                  angle={90} 
                  domain={[0, 100]}
                />
                <Radar 
                  name="Объемная доля, %" 
                  dataKey="value" 
                  stroke={VIBRANT_COLORS.purple} 
                  fill={VIBRANT_COLORS.purple} 
                  fillOpacity={0.6}
                  strokeWidth={2}
                />
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'hsl(var(--background))', 
                    border: '1px solid hsl(var(--border))',
                    borderRadius: '8px'
                  }}
                  formatter={(value: number) => [value.toFixed(2) + '%', 'Доля']}
                />
              </RadarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* График температурного профиля печи - area chart */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Flame className="size-5 text-red-500" />
            Температурный профиль доменной печи
          </CardTitle>
          <CardDescription>Изменение температуры газа по зонам печи</CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={350}>
            <AreaChart data={temperatureData}>
              <defs>
                <linearGradient id="colorTemp" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor={VIBRANT_COLORS.red} stopOpacity={0.8}/>
                  <stop offset="95%" stopColor={VIBRANT_COLORS.orange} stopOpacity={0.2}/>
                </linearGradient>
              </defs>
              <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
              <XAxis 
                dataKey="zone" 
                className="text-sm"
              />
              <YAxis 
                label={{ value: 'Температура, °C', angle: -90, position: 'insideLeft' }}
                className="text-sm"
              />
              <Tooltip 
                contentStyle={{ 
                  backgroundColor: 'hsl(var(--background))', 
                  border: '1px solid hsl(var(--border))',
                  borderRadius: '8px'
                }}
                formatter={(value: number) => [value.toFixed(1) + '°C', 'Температура']}
              />
              <Area 
                type="monotone" 
                dataKey="temperature" 
                stroke={VIBRANT_COLORS.red} 
                strokeWidth={3}
                fill="url(#colorTemp)"
              />
            </AreaChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      {/* График перепадов давления */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Gauge className="size-5 text-cyan-500" />
            Перепады давления по зонам печи
          </CardTitle>
          <CardDescription>Распределение гидравлического сопротивления</CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={pressureData}>
              <defs>
                <linearGradient id="pressureGradient" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor={VIBRANT_COLORS.cyan} stopOpacity={0.9}/>
                  <stop offset="95%" stopColor={VIBRANT_COLORS.blue} stopOpacity={0.6}/>
                </linearGradient>
              </defs>
              <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
              <XAxis dataKey="zone" className="text-sm" />
              <YAxis 
                yAxisId="left"
                label={{ value: 'Перепад, атм', angle: -90, position: 'insideLeft' }}
                className="text-sm"
              />
              <YAxis 
                yAxisId="right"
                orientation="right"
                label={{ value: 'Доля, %', angle: 90, position: 'insideRight' }}
                className="text-sm"
              />
              <Tooltip 
                contentStyle={{ 
                  backgroundColor: 'hsl(var(--background))', 
                  border: '1px solid hsl(var(--border))',
                  borderRadius: '8px'
                }}
              />
              <Legend />
              <Bar 
                yAxisId="left"
                dataKey="pressure" 
                fill="url(#pressureGradient)" 
                name="Перепад давления, атм"
                radius={[8, 8, 0, 0]}
              />
              <Bar 
                yAxisId="right"
                dataKey="percentage" 
                fill={VIBRANT_COLORS.amber}
                fillOpacity={0.7}
                name="Доля от общего, %"
                radius={[8, 8, 0, 0]}
              />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      <div className="grid gap-6 md:grid-cols-2">
        {/* График скоростей фильтрации */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <TrendingUp className="size-5 text-indigo-500" />
              Скорости фильтрации газа
            </CardTitle>
            <CardDescription>Динамика скоростей по зонам печи</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={320}>
              <LineChart data={velocityData}>
                <defs>
                  <linearGradient id="velocityGradient" x1="0" y1="0" x2="1" y2="0">
                    <stop offset="0%" stopColor={VIBRANT_COLORS.indigo} stopOpacity={1}/>
                    <stop offset="100%" stopColor={VIBRANT_COLORS.cyan} stopOpacity={1}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                <XAxis 
                  dataKey="zone" 
                  angle={-15} 
                  textAnchor="end" 
                  height={80}
                  className="text-xs"
                />
                <YAxis 
                  label={{ value: 'м/с', angle: -90, position: 'insideLeft' }}
                  className="text-sm"
                />
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'hsl(var(--background))', 
                    border: '1px solid hsl(var(--border))',
                    borderRadius: '8px'
                  }}
                  formatter={(value: number) => [value.toFixed(3) + ' м/с', 'Скорость']}
                />
                <Line 
                  type="monotone" 
                  dataKey="velocity" 
                  stroke="url(#velocityGradient)"
                  strokeWidth={3}
                  dot={{ fill: VIBRANT_COLORS.indigo, r: 5, strokeWidth: 2, stroke: 'hsl(var(--background))' }}
                  activeDot={{ r: 7 }}
                />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* График выхода газов */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Droplets className="size-5 text-teal-500" />
              Удельный выход газов
            </CardTitle>
            <CardDescription>Объем газов на тонну чугуна</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={320}>
              <BarChart data={gasOutputData} layout="vertical">
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                <XAxis 
                  type="number"
                  label={{ value: 'м³/т чугуна', position: 'insideBottom', offset: -5 }}
                  className="text-sm"
                />
                <YAxis 
                  dataKey="type" 
                  type="category" 
                  width={150}
                  className="text-sm"
                />
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'hsl(var(--background))', 
                    border: '1px solid hsl(var(--border))',
                    borderRadius: '8px'
                  }}
                  formatter={(value: number) => [value.toFixed(2) + ' м³/т', 'Объем']}
                />
                <Bar 
                  dataKey="volume" 
                  radius={[0, 8, 8, 0]}
                >
                  <Cell fill={VIBRANT_COLORS.teal} />
                  <Cell fill={VIBRANT_COLORS.cyan} />
                  <Cell fill={VIBRANT_COLORS.sky} />
                </Bar>
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* График расхода материалов */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <BarChart3 className="size-5 text-pink-500" />
            Удельный расход материалов
          </CardTitle>
          <CardDescription>Расход сырья на производство 1 тонны чугуна</CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={350}>
            <BarChart data={materialConsumptionData}>
              <defs>
                {[VIBRANT_COLORS.green, VIBRANT_COLORS.blue, VIBRANT_COLORS.amber, VIBRANT_COLORS.pink].map((color, index) => (
                  <linearGradient key={`mat-gradient-${index}`} id={`matGradient${index}`} x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor={color} stopOpacity={0.9}/>
                    <stop offset="95%" stopColor={color} stopOpacity={0.6}/>
                  </linearGradient>
                ))}
              </defs>
              <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
              <XAxis 
                dataKey="material"
                className="text-sm"
              />
              <YAxis 
                label={{ value: 'т/т чугуна', angle: -90, position: 'insideLeft' }}
                className="text-sm"
              />
              <Tooltip 
                contentStyle={{ 
                  backgroundColor: 'hsl(var(--background))', 
                  border: '1px solid hsl(var(--border))',
                  borderRadius: '8px'
                }}
                formatter={(value: number) => [value.toFixed(3) + ' т/т', 'Расход']}
              />
              <Bar dataKey="value" radius={[8, 8, 0, 0]}>
                {materialConsumptionData.map((entry, index) => (
                  <Cell key={`mat-cell-${index}`} fill={`url(#matGradient${index})`} />
                ))}
              </Bar>
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>
    </div>
  );
}
