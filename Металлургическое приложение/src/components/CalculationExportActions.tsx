import { FileText, Sheet } from 'lucide-react';
import { featureApi } from '../services/calculation-features';
import { Button } from './ui/button';

export function CalculationExportActions({ id }: { id: string }) {
  return <div className="calculation-result-actions" aria-label="Экспорт отчёта">
    <Button asChild variant="outline"><a href={featureApi.exportUrl(id, 'pdf')}><FileText className="size-4" />Скачать PDF</a></Button>
    <Button asChild variant="outline"><a href={featureApi.exportUrl(id, 'xlsx')}><Sheet className="size-4" />Скачать Excel</a></Button>
  </div>;
}
