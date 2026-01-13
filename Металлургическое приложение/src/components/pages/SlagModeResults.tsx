import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Separator } from '../ui/separator';
import { Badge } from '../ui/badge';
import { 
  Droplets, 
  Flame, 
  Thermometer, 
  Activity, 
  TrendingUp,
  Factory,
  Layers,
  BarChart3
} from 'lucide-react';

// Интерфейс на основе C# модели ResponseData
export interface SlagModeResponseData {
  slagBasicity1: number;
  slagBasicity2: number;
  slagBasicity3: number;
  slagBasicityKulikov: number;
  slagOut: number;
  materialCons: number;
  totalAglo: number;
  propAglo23: number;
  propAglo4: number;
  propAglo234: number;
  propSsgpo: number;
  propLeb: number;
  propKach: number;
  propMix: number;
  propOre: number;
  propWeldSlag: number;
  propBfAddict: number;
  propMinInclude: number;
  totalProp: number;
  viscosity_1400: number;
  viscosity_1450: number;
  viscosity_1500: number;
  viscosity_1550: number;
  temp_7_Puaz: number;
  gradient_7_25: number;
  gradient_1400_1500: number;
  slagTemperature: number;
  slagTemperature_25Puaz: number;
  currSlagViscosity: number;
  balSlagMass: number;
  caOBalSlagMass: number;
  totalSInOre: number;
  sActivity: number;
  sDistribution: number;
  sContentInCastIron: number;
  castIronTemp: number;
}

interface SlagModeResultsProps {
  data: SlagModeResponseData;
}

export function SlagModeResults({ data }: SlagModeResultsProps) {
  return (
    <div className="space-y-6">
      {/* Основность шлака */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Droplets className="size-5 text-blue-500" />
            Основность шлака
          </CardTitle>
          <CardDescription>
            Различные показатели основности доменного шлака
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            <div className="p-4 border border-border rounded-lg bg-muted/30">
              <p className="text-sm text-muted-foreground mb-1">CaO / SiO₂</p>
              <p className="text-2xl font-semibold">{data.slagBasicity1.toFixed(3)}</p>
            </div>
            <div className="p-4 border border-border rounded-lg bg-muted/30">
              <p className="text-sm text-muted-foreground mb-1">(CaO + MgO) / SiO₂</p>
              <p className="text-2xl font-semibold">{data.slagBasicity2.toFixed(3)}</p>
            </div>
            <div className="p-4 border border-border rounded-lg bg-muted/30">
              <p className="text-sm text-muted-foreground mb-1">(CaO + MgO) / (SiO₂ + Al₂O₃)</p>
              <p className="text-2xl font-semibold">{data.slagBasicity3.toFixed(3)}</p>
            </div>
            <div className="p-4 border border-border rounded-lg bg-blue-500/10">
              <p className="text-sm text-muted-foreground mb-1">По Куликову</p>
              <p className="text-2xl font-semibold text-blue-600 dark:text-blue-400">
                {data.slagBasicityKulikov.toFixed(3)}
              </p>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Выход шлака и расход материалов */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Factory className="size-5 text-purple-500" />
            Выход шлака и расход материалов
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Расчётный выход шлака</p>
              <p className="text-2xl font-semibold">{data.slagOut.toFixed(2)}</p>
              <p className="text-xs text-muted-foreground mt-1">кг/т чугуна</p>
            </div>
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Расход материалов</p>
              <p className="text-2xl font-semibold">{data.materialCons.toFixed(2)}</p>
              <p className="text-xs text-muted-foreground mt-1">кг/т чугуна</p>
            </div>
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Выход по балансу ШО</p>
              <p className="text-2xl font-semibold">{data.balSlagMass.toFixed(2)}</p>
              <p className="text-xs text-muted-foreground mt-1">кг/т чугуна</p>
            </div>
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Выход по балансу CaO</p>
              <p className="text-2xl font-semibold">{data.caOBalSlagMass.toFixed(2)}</p>
              <p className="text-xs text-muted-foreground mt-1">кг/т чугуна</p>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Распределение компонентов шихты */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Layers className="size-5 text-green-500" />
            Распределение компонентов шихты
          </CardTitle>
          <CardDescription>
            Доли различных компонентов железорудного сырья
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Всего агломерата с фабрик</span>
                  <Badge variant="secondary">{data.totalAglo.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-green-500" 
                    style={{ width: `${Math.min(data.totalAglo, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Агломерат фабрик 2 и 3</span>
                  <Badge variant="outline">{data.propAglo23.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-blue-500" 
                    style={{ width: `${Math.min(data.propAglo23, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Агломерат фабрики 4</span>
                  <Badge variant="outline">{data.propAglo4.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-indigo-500" 
                    style={{ width: `${Math.min(data.propAglo4, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Окатыши ССГПО</span>
                  <Badge variant="outline">{data.propSsgpo.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-purple-500" 
                    style={{ width: `${Math.min(data.propSsgpo, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Окатыши Лебединский ГОК</span>
                  <Badge variant="outline">{data.propLeb.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-pink-500" 
                    style={{ width: `${Math.min(data.propLeb, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Окатыши Качканарский ГОК</span>
                  <Badge variant="outline">{data.propKach.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-orange-500" 
                    style={{ width: `${Math.min(data.propKach, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Окатыши Михайловский ГОК</span>
                  <Badge variant="outline">{data.propMix.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-red-500" 
                    style={{ width: `${Math.min(data.propMix, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Руда</span>
                  <Badge variant="outline">{data.propOre.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-yellow-500" 
                    style={{ width: `${Math.min(data.propOre, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Сварочный шлак</span>
                  <Badge variant="outline">{data.propWeldSlag.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-teal-500" 
                    style={{ width: `${Math.min(data.propWeldSlag, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Доменный присад</span>
                  <Badge variant="outline">{data.propBfAddict.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-cyan-500" 
                    style={{ width: `${Math.min(data.propBfAddict, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm">Королёк</span>
                  <Badge variant="outline">{data.propMinInclude.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-slate-500" 
                    style={{ width: `${Math.min(data.propMinInclude, 100)}%` }}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <span className="text-sm font-semibold">Местный агломерат (2+3+4)</span>
                  <Badge>{data.propAglo234.toFixed(2)}%</Badge>
                </div>
                <div className="h-2 bg-muted rounded-full overflow-hidden">
                  <div 
                    className="h-full bg-emerald-500" 
                    style={{ width: `${Math.min(data.propAglo234, 100)}%` }}
                  />
                </div>
              </div>
            </div>

            <Separator />

            <div className="p-4 border border-border rounded-lg bg-primary/5">
              <div className="flex justify-between items-center">
                <span className="font-semibold">Общая доля всех компонентов</span>
                <Badge variant="default" className="text-base px-3 py-1">
                  {data.totalProp.toFixed(2)}%
                </Badge>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Вязкость шлака */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Activity className="size-5 text-orange-500" />
            Вязкость шлака
          </CardTitle>
          <CardDescription>
            Зависимость вязкости от температуры
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
              <div className="p-4 border border-border rounded-lg">
                <p className="text-sm text-muted-foreground mb-1">При 1400°C</p>
                <p className="text-2xl font-semibold">{data.viscosity_1400.toFixed(3)}</p>
                <p className="text-xs text-muted-foreground mt-1">Па·с</p>
              </div>
              <div className="p-4 border border-border rounded-lg">
                <p className="text-sm text-muted-foreground mb-1">При 1450°C</p>
                <p className="text-2xl font-semibold">{data.viscosity_1450.toFixed(3)}</p>
                <p className="text-xs text-muted-foreground mt-1">Па·с</p>
              </div>
              <div className="p-4 border border-border rounded-lg">
                <p className="text-sm text-muted-foreground mb-1">При 1500°C</p>
                <p className="text-2xl font-semibold">{data.viscosity_1500.toFixed(3)}</p>
                <p className="text-xs text-muted-foreground mt-1">Па·с</p>
              </div>
              <div className="p-4 border border-border rounded-lg">
                <p className="text-sm text-muted-foreground mb-1">При 1550°C</p>
                <p className="text-2xl font-semibold">{data.viscosity_1550.toFixed(3)}</p>
                <p className="text-xs text-muted-foreground mt-1">Па·с</p>
              </div>
            </div>

            <Separator />

            <div className="grid gap-4 md:grid-cols-2">
              <div className="p-4 border border-border rounded-lg bg-orange-500/10">
                <p className="text-sm text-muted-foreground mb-1">Вязкость при ��екущей температуре</p>
                <p className="text-2xl font-semibold text-orange-600 dark:text-orange-400">
                  {data.currSlagViscosity.toFixed(3)} Па·с
                </p>
              </div>
              <div className="p-4 border border-border rounded-lg">
                <p className="text-sm text-muted-foreground mb-1">Температура при 7 пуаз</p>
                <p className="text-2xl font-semibold">{data.temp_7_Puaz.toFixed(1)}°C</p>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Температурные параметры */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Thermometer className="size-5 text-red-500" />
            Температурные параметры
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Температура шлака</p>
              <p className="text-2xl font-semibold">{data.slagTemperature.toFixed(1)}°C</p>
            </div>
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">T при 25 пуаз</p>
              <p className="text-2xl font-semibold">{data.slagTemperature_25Puaz.toFixed(1)}°C</p>
            </div>
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Температура чугуна</p>
              <p className="text-2xl font-semibold">{data.castIronTemp.toFixed(1)}°C</p>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Градиенты */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <TrendingUp className="size-5 text-teal-500" />
            Градиенты
          </CardTitle>
          <CardDescription>
            Изменение параметров в диапазонах
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2">
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Градиент при 7-25 Пуаз</p>
              <p className="text-2xl font-semibold">{data.gradient_7_25.toFixed(4)}</p>
            </div>
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Градиент при 1400-1500°C</p>
              <p className="text-2xl font-semibold">{data.gradient_1400_1500.toFixed(4)}</p>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Баланс серы */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Flame className="size-5 text-yellow-500" />
            Баланс серы
          </CardTitle>
          <CardDescription>
            Распределение и содержание серы в процессе
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Масса S в печи</p>
              <p className="text-2xl font-semibold">{data.totalSInOre.toFixed(3)}</p>
              <p className="text-xs text-muted-foreground mt-1">кг/т чугуна</p>
            </div>
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Активность S</p>
              <p className="text-2xl font-semibold">{data.sActivity.toFixed(4)}</p>
            </div>
            <div className="p-4 border border-border rounded-lg">
              <p className="text-sm text-muted-foreground mb-1">Коэфф. распределения S</p>
              <p className="text-2xl font-semibold">{data.sDistribution.toFixed(2)}</p>
            </div>
            <div className="p-4 border border-border rounded-lg bg-yellow-500/10">
              <p className="text-sm text-muted-foreground mb-1">Содержание S в чугуне</p>
              <p className="text-2xl font-semibold text-yellow-600 dark:text-yellow-400">
                {data.sContentInCastIron.toFixed(4)}%
              </p>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
