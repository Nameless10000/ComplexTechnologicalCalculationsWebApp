import { useState } from 'react';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from './ui/dialog';
import { Button } from './ui/button';
import { Textarea } from './ui/textarea';
import { Label } from './ui/label';
import { Save } from 'lucide-react';

interface SaveCalculationDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSave: (note: string) => void;
  resultsPreview: string;
}

export function SaveCalculationDialog({
  open,
  onOpenChange,
  onSave,
  resultsPreview,
}: SaveCalculationDialogProps) {
  const [note, setNote] = useState('');

  const handleSave = () => {
    onSave(note);
    setNote('');
    onOpenChange(false);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Сохранить расчет в историю</DialogTitle>
          <DialogDescription>
            Добавьте заметку для удобного поиска этого расчета в будущем
          </DialogDescription>
        </DialogHeader>
        
        <div className="space-y-4 py-4">
          <div className="space-y-2">
            <Label htmlFor="note">Заметка о расчете</Label>
            <Textarea
              id="note"
              placeholder="Например: Расчет для печи №3, режим с повышенным дутьем..."
              value={note}
              onChange={(e) => setNote(e.target.value)}
              rows={3}
            />
          </div>
          
          <div className="space-y-2">
            <Label>Основные результаты</Label>
            <div className="text-sm text-muted-foreground bg-muted p-3 rounded-lg border border-border max-h-32 overflow-y-auto">
              {resultsPreview}
            </div>
          </div>
        </div>

        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Отмена
          </Button>
          <Button onClick={handleSave}>
            <Save className="size-4 mr-2" />
            Сохранить
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
