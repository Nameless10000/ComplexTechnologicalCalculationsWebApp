import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { LineChart, Line, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import { TrendingUp, BarChart3, Activity } from 'lucide-react';

interface GasDynamicChartsProps {
  results: any;
}

const COLORS = ['#3b82f6', '#ef4444', '#10b981', '#f59e0b', '#8b5cf6'];

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
    },
    {
      name: 'Прямое восстановление',
      value: results.blastFurnace?.carbonBalance?.c_Out_Vosstan || 0,
    },
    {
      name: 'Растворение в чугуне',
      value: results.blastFurnace?.carbonBalance?.c_Out_Chugun || 0,
    },
    {
      name: 'Образование метана',
      value: results.blastFurnace?.carbonBalance?.c_Out_Metan || 0,
    },
    {
      name: 'Сгорание у фурм',
      value: results.blastFurnace?.carbonBalance?.c_Out_Furm || 0,
    },
  ];

  // Данные для графика состава газа
  const hearthGasComposition = [
    { name: 'CO', value: results.blastFurnace?.hearthGas?.furmgaz_CO || 0 },
    { name: 'H₂', value: results.blastFurnace?.hearthGas?.furmgaz_H2 || 0 },
    { name: 'N₂', value: results.blastFurnace?.hearthGas?.furmgaz_N2 || 0 },
  ];

  // Данные для графика колошникового газа
  const topGasComposition = [
    { name: 'CO₂', value: results.blastFurnace?.topGas?.kolgaz_CO2 || 0, fill: '#ef4444' },
    { name: 'CO', value: results.blastFurnace?.topGas?.kolgaz_CO || 0, fill: '#3b82f6' },
    { name: 'H₂', value: results.blastFurnace?.topGas?.kolgaz_H2 || 0, fill: '#10b981' },
    { name: 'CH₄', value: results.blastFurnace?.topGas?.kolgaz_CH4 || 0, fill: '#f59e0b' },
    { name: 'N₂', value: results.blastFurnace?.topGas?.kolgaz_N2 || 0, fill: '#8b5cf6' },
  ];

  // Данные для графика температур по зонам
  const temperatureData = [
    {
      zone: 'Горновой газ (теор.)',
      temperature: results.blastFurnace?.hearthGas?.temp_Teor || 0,
    },
    {
      zone: 'Нижняя зона (средн.)',
      temperature: results.blastFurnace?.hearthGas?.temp_Sredn_Niz || 0,
    },
    {
      zone: 'Колошниковый газ',
      temperature: results.blastFurnace?.thermalParameters?.temp_Verh || 0,
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

  // Данные для графика расхода материалов
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
      material: 'Кокс',
      value: results.blastFurnace?.materialConsumption?.udeln_Koks_1000 || 0,
    },
  ];

  return (
    <div className="space-y-6">
      {/* График углеродного баланса */}
      <Card>
        <CardHeader>
          <div className="flex items-center gap-2">
            <BarChart3 className="size-5 text-blue-500" />
            <CardTitle>Углеродный баланс</CardTitle>
          </div>
          <CardDescription>Распределение углерода в процессе плавки</CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={carbonBalanceData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="name" angle={-15} textAnchor="end" height={100} />
              <YAxis label={{ value: 'кг', angle: -90, position: 'insideLeft' }} />
              <Tooltip />
              <Bar dataKey="value" fill="#3b82f6" />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      <div className="grid gap-6 md:grid-cols-2">
        {/* Состав горнового газа */}
        <Card>
          <CardHeader>
            <div className="flex items-center gap-2">
              <Activity className="size-5 text-orange-500" />
              <CardTitle>Состав горнового газа</CardTitle>
            </div>
            <CardDescription>Распределение компонентов</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={250}>
              <PieChart>
                <Pie
                  data={hearthGasComposition}
                  cx="50%"
                  cy="50%"
                  labelLine={false}
                  label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(1)}%`}
                  outerRadius={80}
                  fill="#8884d8"
                  dataKey="value"
                >
                  {hearthGasComposition.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Состав колошникового газа */}
        <Card>
          <CardHeader>
            <div className="flex items-center gap-2">
              <Activity className="size-5 text-green-500" />
              <CardTitle>Состав колошникового газа</CardTitle>
            </div>
            <CardDescription>Компоненты выходящего газа, %</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={250}>
              <BarChart data={topGasComposition} layout="vertical">
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis type="number" />
                <YAxis dataKey="name" type="category" width={50} />
                <Tooltip />
                <Bar dataKey="value" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* График температур */}
      <Card>
        <CardHeader>
          <div className="flex items-center gap-2">
            <TrendingUp className="size-5 text-red-500" />
            <CardTitle>Температурный профиль печи</CardTitle>
          </div>
          <CardDescription>Распределение температур по зонам</CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={temperatureData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="zone" />
              <YAxis label={{ value: '°C', angle: -90, position: 'insideLeft' }} />
              <Tooltip />
              <Legend />
              <Bar dataKey="temperature" fill="#ef4444" name="Температура, °C" />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      {/* График скоростей */}
      <Card>
        <CardHeader>
          <div className="flex items-center gap-2">
            <TrendingUp className="size-5 text-cyan-500" />
            <CardTitle>Скорости фильтрации газа</CardTitle>
          </div>
          <CardDescription>Скорости по зонам печи</CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={velocityData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="zone" angle={-15} textAnchor="end" height={80} />
              <YAxis label={{ value: 'м/с', angle: -90, position: 'insideLeft' }} />
              <Tooltip />
              <Legend />
              <Line 
                type="monotone" 
                dataKey="velocity" 
                stroke="#06b6d4" 
                strokeWidth={2}
                name="Скорость, м/с"
                dot={{ fill: '#06b6d4', r: 4 }}
              />
            </LineChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      {/* График расхода материалов */}
      <Card>
        <CardHeader>
          <div className="flex items-center gap-2">
            <BarChart3 className="size-5 text-purple-500" />
            <CardTitle>Удельный расход материалов</CardTitle>
          </div>
          <CardDescription>Расход на 1 тонну чугуна</CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={materialConsumptionData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="material" />
              <YAxis label={{ value: 'т/т чугуна', angle: -90, position: 'insideLeft' }} />
              <Tooltip />
              <Bar dataKey="value" fill="#8b5cf6" />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>
    </div>
  );
}
