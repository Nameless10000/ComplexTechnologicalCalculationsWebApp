import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Scale, BookmarkPlus } from 'lucide-react';
import { Button } from '../ui/button';
import { useCalculationHistory } from '../../hooks/useCalculationHistory';
import { CalculationHistory } from '../CalculationHistory';
import { SaveCalculationDialog } from '../SaveCalculationDialog';
import { useState } from 'react';
import { CalculationPageHeader } from '../CalculationPageHeader';
import { Tabs, TabsList, TabsTrigger, TabsContent } from '../ui/tabs';

export function MassBalancePage() {
  const { history, addToHistory, removeFromHistory, clearHistory } = useCalculationHistory('mass-balance');
  const [saveDialogOpen, setSaveDialogOpen] = useState(false);
  const [calculationResults, setCalculationResults] = useState<any>(null);
  
  const handleSaveToHistory = (note: string) => {
    if (calculationResults) {
      addToHistory(note, 'Результаты массового баланса');
    }
  };

  return (
    <div className="calculation-page space-y-6">
      <CalculationPageHeader icon={Scale} title="Массовый баланс доменной плавки"
        description="Расчёт материальных потоков и баланса масс" />

      <div className="calculation-workspace">
        <div className="space-y-6">
          <Tabs defaultValue="inputs" className="w-full">
            <TabsList><TabsTrigger value="inputs">Входные данные</TabsTrigger><TabsTrigger value="results">Результаты</TabsTrigger></TabsList>
            <TabsContent value="inputs">
            <Card>
              <CardHeader>
                <CardTitle>Входные параметры</CardTitle>
                <CardDescription>
                  Исходные данные для расчета материального баланса
                </CardDescription>
              </CardHeader>
              <CardContent>
                <div className="min-h-[300px] flex items-center justify-center border-2 border-dashed border-border rounded-lg">
                  <p className="text-muted-foreground">Форма ввода данных</p>
                </div>
              </CardContent>
            </Card>
            </TabsContent>
            <TabsContent value="results">
            <Card>
              <CardHeader>
                <CardTitle>Выходные параметры</CardTitle>
                <CardDescription>
                  Результаты расчета массового баланса
                </CardDescription>
              </CardHeader>
              <CardContent>
                <div className="min-h-[300px] flex items-center justify-center border-2 border-dashed border-border rounded-lg">
                  <p className="text-muted-foreground">Результаты расчета</p>
                </div>
              </CardContent>
            </Card>
            </TabsContent>
          </Tabs>
        </div>

        <div className="calculation-history">
          <CalculationHistory
            history={history}
            onRemove={removeFromHistory}
            onClear={clearHistory}
          />
        </div>
      </div>
      
      <SaveCalculationDialog
        open={saveDialogOpen}
        onOpenChange={setSaveDialogOpen}
        onSave={handleSaveToHistory}
        resultsPreview="Результаты массового баланса"
      />
    </div>
  );
}
