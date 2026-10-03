import { Link } from 'react-router-dom';
import { ArrowRight } from 'lucide-react';
import { moduleNames, modulePaths } from '../services/calculation-features';
import { Button } from './ui/button';

const nextModules: Record<string, string[]> = {
  'aglom-mode': ['slag-mode'],
  'slag-mode': ['furnace'],
  'gas-dynamic': ['furnace'],
};

export function CalculationNextStage({ module, receipt }: { module: string; receipt: any }) {
  if (!nextModules[module]) return null;
  return <div className="space-y-2 rounded-lg border p-4">
    <p className="font-medium">Перенести результаты далее</p>
    <p className="text-sm text-muted-foreground">Следующий модуль откроется с заполненными связанными полями. Остальные параметры проверьте перед расчётом.</p>
    <div className="flex flex-wrap gap-2">{nextModules[module].map(target => receipt?.id
      ? <Button asChild key={target}><Link to={`${modulePaths[target]}?sourceCalculationId=${receipt.id}`}><ArrowRight className="size-4 mr-2" />{moduleNames[target]}</Link></Button>
      : <Button key={target} disabled>{moduleNames[target]}</Button>)}</div>
    {!receipt?.id && <p className="text-sm text-muted-foreground">Для переноса нужен результат, сохранённый сервером. Выполните расчёт или откройте его из истории.</p>}
  </div>;
}
