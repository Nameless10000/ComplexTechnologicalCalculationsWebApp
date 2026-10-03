import type { LucideIcon } from 'lucide-react';
import { AlertCircle, Calculator, Info, Loader2 } from 'lucide-react';
import { Button } from './ui/button';
import { Alert, AlertDescription } from './ui/alert';
import './calculation-page.css';

export function PageHeading({ title, description, icon: Icon }: {
  title: string; description: string; icon: LucideIcon;
}) {
  return <header>
    <div className="flex items-center gap-3 mb-2">
      <Icon className="size-8 text-primary shrink-0" />
      <h1 className="text-3xl">{title}</h1>
    </div>
    <p className="text-muted-foreground">{description}</p>
  </header>;
}

export function CalculationPageHeader({ title, description, icon, onCalculate, isCalculating = false, error, notice }: {
  title: string; description: string; icon: LucideIcon; onCalculate?: () => void;
  isCalculating?: boolean; error?: string; notice?: string;
}) {
  return <>
    <PageHeading title={title} description={description} icon={icon} />
    <div className="calculation-toolbar" aria-label="Действия расчёта">
      <p className="text-sm text-muted-foreground">
        {onCalculate ? 'Расчёт использует данные всех вкладок.' : 'Этот расчётный модуль пока недоступен.'}
      </p>
      <Button type="button" size="lg" onClick={onCalculate} disabled={isCalculating || !onCalculate}>
        {isCalculating ? <Loader2 className="size-4 mr-2 animate-spin" /> : <Calculator className="size-4 mr-2" />}
        {isCalculating ? 'Выполняется расчёт…' : 'Рассчитать'}
      </Button>
    </div>
    {notice && <Alert role="status"><Info className="size-4" /><AlertDescription>{notice}</AlertDescription></Alert>}
    {error && <Alert variant="destructive" role="alert"><AlertCircle className="size-4" /><AlertDescription className="whitespace-pre-wrap">{error}</AlertDescription></Alert>}
  </>;
}
