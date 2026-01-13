import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, AreaChart, Area } from 'recharts';
import { Activity } from 'lucide-react';
import { SlagModeResponseData } from './SlagModeResults';

interface SlagModeChartsProps {
  data: SlagModeResponseData;
}

export function SlagModeCharts({ data }: SlagModeChartsProps) {
  // Данные для графика вязкости в зависимости от температуры
  const viscosityData = [
    { temperature: 1400, viscosity: data.viscosity_1400 },
    { temperature: 1450, viscosity: data.viscosity_1450 },
    { temperature: 1500, viscosity: data.viscosity_1500 },
    { temperature: 1550, viscosity: data.viscosity_1550 },
  ];

  // Данные для графика распределения компонентов
  const componentsData = [
    { name: 'Агломерат фабрик 2+3', value: data.propAglo23, color: '#3b82f6' },
    { name: 'Агломерат фабрики 4', value: data.propAglo4, color: '#6366f1' },
    { name: 'Окатыши ССГПО', value: data.propSsgpo, color: '#a855f7' },
    { name: 'Окатыши Лебединский ГОК', value: data.propLeb, color: '#ec4899' },
    { name: 'Окатыши Качканарский ГОК', value: data.propKach, color: '#f97316' },
    { name: 'Окатыши Михайловский ГОК', value: data.propMix, color: '#ef4444' },
    { name: 'Руда', value: data.propOre, color: '#eab308' },
    { name: 'Прочие', value: data.propWeldSlag + data.propBfAddict + data.propMinInclude, color: '#64748b' },
  ].filter(item => item.value > 0); // Показываем только компоненты с ненулевыми значениями

  // Данные для графика основности шлака
  const basicityData = [
    { type: 'CaO/SiO₂', value: data.slagBasicity1 },
    { type: '(CaO+MgO)/SiO₂', value: data.slagBasicity2 },
    { type: '(CaO+MgO)/(SiO₂+Al₂O₃)', value: data.slagBasicity3 },
    { type: 'По Куликову', value: data.slagBasicityKulikov },
  ];

  return (
    <div className="space-y-6">
      {/* График вязкости шлака */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Activity className="size-5 text-orange-500" />
            Зависимость вязкости шлака от температуры
          </CardTitle>
          <CardDescription>
            Изменение вязкости шлака в диапазоне температур 1400-1550°C
          </CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={350}>
            <AreaChart data={viscosityData}>
              <defs>
                <linearGradient id="colorViscosity" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor="#f97316" stopOpacity={0.8}/>
                  <stop offset="95%" stopColor="#f97316" stopOpacity={0.1}/>
                </linearGradient>
              </defs>
              <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
              <XAxis 
                dataKey="temperature" 
                label={{ value: 'Температура, °C', position: 'insideBottom', offset: -5 }}
                className="text-sm"
              />
              <YAxis 
                label={{ value: 'Вязкость, Па·с', angle: -90, position: 'insideLeft' }}
                className="text-sm"
              />
              <Tooltip 
                contentStyle={{ 
                  backgroundColor: 'hsl(var(--background))', 
                  border: '1px solid hsl(var(--border))',
                  borderRadius: '8px'
                }}
                formatter={(value: number) => [value.toFixed(3) + ' Па·с', 'Вязкость']}
                labelFormatter={(label) => `Температура: ${label}°C`}
              />
              <Area 
                type="monotone" 
                dataKey="viscosity" 
                stroke="#f97316" 
                strokeWidth={2}
                fill="url(#colorViscosity)"
              />
            </AreaChart>
          </ResponsiveContainer>

          <div className="mt-4 p-4 border border-border rounded-lg bg-muted/30">
            <div className="grid gap-2 md:grid-cols-2">
              <div>
                <p className="text-sm text-muted-foreground">Температура при 7 пуаз</p>
                <p className="text-lg font-semibold">{data.temp_7_Puaz.toFixed(1)}°C</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Градиент (7-25 Пуаз)</p>
                <p className="text-lg font-semibold">{data.gradient_7_25.toFixed(4)}</p>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* График основности шлака */}
      <Card>
        <CardHeader>
          <CardTitle>Показатели основности шлака</CardTitle>
          <CardDescription>
            Различные коэффициенты основности доменного шлака
          </CardDescription>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={basicityData}>
              <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
              <XAxis 
                dataKey="type" 
                className="text-xs"
                angle={-15}
                textAnchor="end"
                height={80}
              />
              <YAxis 
                label={{ value: 'Основность', angle: -90, position: 'insideLeft' }}
                className="text-sm"
              />
              <Tooltip 
                contentStyle={{ 
                  backgroundColor: 'hsl(var(--background))', 
                  border: '1px solid hsl(var(--border))',
                  borderRadius: '8px'
                }}
                formatter={(value: number) => [value.toFixed(3), 'Основность']}
              />
              <Line 
                type="monotone" 
                dataKey="value" 
                stroke="#3b82f6" 
                strokeWidth={2}
                dot={{ fill: '#3b82f6', r: 5 }}
                activeDot={{ r: 7 }}
              />
            </LineChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      {/* График распределения компонентов */}
      {componentsData.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle>Распределение компонентов шихты</CardTitle>
            <CardDescription>
              Процентное соотношение различных компонентов железорудного сырья
            </CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={350}>
              <AreaChart data={componentsData}>
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                <XAxis 
                  dataKey="name" 
                  className="text-xs"
                  angle={-15}
                  textAnchor="end"
                  height={100}
                />
                <YAxis 
                  label={{ value: 'Доля, %', angle: -90, position: 'insideLeft' }}
                  className="text-sm"
                />
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'hsl(var(--background))', 
                    border: '1px solid hsl(var(--border))',
                    borderRadius: '8px'
                  }}
                  formatter={(value: number) => [value.toFixed(2) + '%', 'Доля']}
                />
                <Area 
                  type="monotone" 
                  dataKey="value" 
                  stroke="#10b981" 
                  strokeWidth={2}
                  fill="#10b981"
                  fillOpacity={0.3}
                />
              </AreaChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      )}
    </div>
  );
}
