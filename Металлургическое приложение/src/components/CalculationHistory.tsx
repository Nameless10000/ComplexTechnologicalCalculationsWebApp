import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { ScrollArea } from './ui/scroll-area';
import { Button } from './ui/button';
import { Trash2, History, Clock } from 'lucide-react';
import { CalculationHistoryItem } from '../hooks/useCalculationHistory';
import { Badge } from './ui/badge';

interface CalculationHistoryProps {
  history: CalculationHistoryItem[];
  onRemove: (id: string) => void;
  onClear: () => void;
}

export function CalculationHistory({ history, onRemove, onClear }: CalculationHistoryProps) {
  const formatDate = (timestamp: string) => {
    const date = new Date(timestamp);
    const now = new Date();
    const diff = now.getTime() - date.getTime();
    const minutes = Math.floor(diff / 60000);
    const hours = Math.floor(diff / 3600000);
    const days = Math.floor(diff / 86400000);

    if (minutes < 1) return 'Только что';
    if (minutes < 60) return `${minutes} мин. назад`;
    if (hours < 24) return `${hours} ч. назад`;
    if (days < 7) return `${days} дн. назад`;
    
    return date.toLocaleDateString('ru-RU', {
      day: 'numeric',
      month: 'short',
      year: date.getFullYear() !== now.getFullYear() ? 'numeric' : undefined,
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  return (
    <Card className="h-full">
      <CardHeader>
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-2">
            <History className="size-5" />
            <CardTitle>История расчетов</CardTitle>
          </div>
          {history.length > 0 && (
            <Button
              variant="ghost"
              size="sm"
              onClick={onClear}
              className="text-muted-foreground hover:text-destructive"
            >
              Очистить
            </Button>
          )}
        </div>
        <CardDescription>
          Сохраненные результаты предыдущих расчетов
        </CardDescription>
      </CardHeader>
      <CardContent>
        <ScrollArea className="h-[calc(100vh-280px)] pr-4">
          {history.length === 0 ? (
            <div className="flex flex-col items-center justify-center py-12 text-center">
              <History className="size-12 text-muted-foreground/50 mb-3" />
              <p className="text-muted-foreground">История пуста</p>
              <p className="text-sm text-muted-foreground mt-1">
                Выполните расчет и сохраните результат
              </p>
            </div>
          ) : (
            <div className="space-y-3">
              {history.map((item) => (
                <div
                  key={item.id}
                  className="group p-4 border border-border rounded-lg hover:border-primary/50 hover:bg-accent/50 transition-colors"
                >
                  <div className="flex items-start justify-between gap-2 mb-2">
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <Clock className="size-3.5" />
                      <span>{formatDate(item.timestamp)}</span>
                    </div>
                    <Button
                      variant="ghost"
                      size="icon"
                      className="size-7 opacity-0 group-hover:opacity-100 transition-opacity -mt-1 -mr-1"
                      onClick={() => onRemove(item.id)}
                    >
                      <Trash2 className="size-3.5 text-destructive" />
                    </Button>
                  </div>
                  
                  {item.note && (
                    <p className="mb-2 line-clamp-2">{item.note}</p>
                  )}
                  
                  <div className="text-sm text-muted-foreground bg-muted/50 p-2 rounded border border-border/50">
                    <p className="line-clamp-3">{item.results}</p>
                  </div>
                </div>
              ))}
            </div>
          )}
        </ScrollArea>
      </CardContent>
    </Card>
  );
}
