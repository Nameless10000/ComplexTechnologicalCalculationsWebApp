import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Droplets } from 'lucide-react';
import { Button } from '../ui/button';

export function SlagModePage() {
  return (
    <div className="space-y-6">
      <div>
        <div className="flex items-center gap-3 mb-2">
          <Droplets className="size-8 text-green-500" />
          <h1 className="text-3xl">Шлаковый режим</h1>
        </div>
        <p className="text-muted-foreground">
          Расчет состава и свойств доменного шлака
        </p>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Входные параметры</CardTitle>
            <CardDescription>
              Исходные данные для расчета шлакового режима
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="min-h-[300px] flex items-center justify-center border-2 border-dashed border-border rounded-lg">
              <p className="text-muted-foreground">Форма ввода данных</p>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Выходные параметры</CardTitle>
            <CardDescription>
              Результаты расчета шлакового режима
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="min-h-[300px] flex items-center justify-center border-2 border-dashed border-border rounded-lg">
              <p className="text-muted-foreground">Результаты расчета</p>
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="flex justify-end">
        <Button size="lg">Выполнить расчет</Button>
      </div>
    </div>
  );
}
