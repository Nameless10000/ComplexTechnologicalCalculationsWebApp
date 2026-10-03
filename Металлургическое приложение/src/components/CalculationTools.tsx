import { useEffect, useRef, useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { featureApi, camelInput } from '../services/calculation-features';
import { readCalculationDraft, writeCalculationDraft } from '../services/calculation-draft';
import { Button } from './ui/button';
import { Input } from './ui/input';
import { Card, CardContent } from './ui/card';

export function CalculationTools({ module, inputs, onLoad, output, onRestoreOutput, activeTab, onRestoreTab, onReceipt, initializing = false }: {
  module: string; inputs: any; onLoad: (input: any) => void;
  output: any; onRestoreOutput: (output: any) => void;
  activeTab: string; onRestoreTab: (tab: string) => void; onReceipt?: (receipt: any) => void; initializing?: boolean;
}) {
  const { search } = useLocation();
  const [presets, setPresets] = useState<any[]>([]);
  const [selected, setSelected] = useState('');
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [error, setError] = useState('');
  const [message, setMessage] = useState('');
  const [busy, setBusy] = useState(false);
  const [receipt, setReceipt] = useState<any>(null);
  const [ready, setReady] = useState(false);
  const [storageError, setStorageError] = useState(false);
  const sourceRef = useRef<string | null>(null);
  const callbacks = useRef({ onLoad, onRestoreOutput, onRestoreTab, onReceipt });
  callbacks.current = { onLoad, onRestoreOutput, onRestoreTab, onReceipt };
  const loadRef = useRef(onLoad); loadRef.current = onLoad;
  const snapshot = useRef<any>(null);
  snapshot.current = { version: 1, inputs, output, activeTab, receipt, sourceCalculationId: sourceRef.current };
  const readyRef = useRef(ready && !initializing); readyRef.current = ready && !initializing;
  const refresh = async () => setPresets(await featureApi.presets(module));
  const run = async (operation: () => Promise<void>) => {
    setBusy(true); setError(''); setMessage('');
    try { await operation(); } catch (e: any) { setError(e.message || 'Не удалось выполнить действие.'); } finally { setBusy(false); }
  };
  useEffect(() => {
    let cancelled = false;
    setReady(false); setReceipt(null); setError(''); setMessage('');
    featureApi.presets(module).then(rows => { if (!cancelled) setPresets(rows); }).catch((e) => { if (!cancelled) setError(e.message); });
    const params = new URLSearchParams(window.location.search);
    const id = params.get('calculationId');
    const sourceId = params.get('sourceCalculationId');
    const draft = readCalculationDraft(module);
    sourceRef.current = sourceId || (!id ? draft?.sourceCalculationId : null) || null;
    if (!id && draft) {
      loadRef.current(draft.inputs);
      if (!sourceId) {
        callbacks.current.onRestoreOutput(draft.output);
        callbacks.current.onRestoreTab(draft.activeTab);
        setReceipt(draft.receipt);
      }
      setMessage('Данные восстановлены из этого браузера. Изменения сохраняются автоматически.');
    }
    if (sourceId && module === 'furnace') {
      callbacks.current.onRestoreOutput(null);
      callbacks.current.onRestoreTab('inputs');
      featureApi.transition(sourceId, draft?.inputs || snapshot.current.inputs, 'furnace').then(row => {
        if (cancelled) return;
        loadRef.current(row.input);
        setMessage(`${row.message} Перенесены: ${row.mappedFields.join(', ')}.`);
        setReady(true);
      }).catch(e => { if (!cancelled) setError(e.message); });
    } else if (!id) setReady(true);
    if (id) featureApi.calculation(id).then(row => {
      if (cancelled) return;
      if (row.module !== module) throw new Error('Расчёт относится к другому модулю.');
      loadRef.current(module === 'furnace' ? row.input : camelInput(row.input));
      callbacks.current.onRestoreOutput(module === 'furnace' ? row.output : camelInput(row.output));
      callbacks.current.onRestoreTab('results');
      setReceipt({ id: row.id, module: row.module, status: row.status, correlationId: row.correlationId });
      sourceRef.current = row.sourceCalculationId;
      setMessage('Сохранённый расчёт открыт. Для нового расчёта нажмите «Рассчитать».');
      setReady(true);
    }).catch(e => { if (!cancelled) setError(e.message); });
    const completed = (event: Event) => { const data = (event as CustomEvent).detail; if (data.module === module) setReceipt(data); };
    window.addEventListener('calculation-completed', completed);
    return () => { cancelled = true; window.removeEventListener('calculation-completed', completed); };
  }, [module, search]);
  useEffect(() => {
    if (!receipt || receipt.status === 'Saved') return;
    let cancelled = false; let attempts = 0; let timer: ReturnType<typeof setTimeout>;
    const poll = async () => {
      try { const row = await featureApi.calculation(receipt.id); if (!cancelled && row.status === 'Saved') { setReceipt({ ...receipt, status: row.status }); return; } }
      catch { /* Keep the pending status; history can be refreshed explicitly. */ }
      if (!cancelled && ++attempts < 6) timer = setTimeout(poll, Math.min(30000, 3000 * 2 ** attempts));
    };
    timer = setTimeout(poll, 3000);
    return () => { cancelled = true; clearTimeout(timer); };
  }, [receipt?.id, receipt?.status]);
  useEffect(() => { callbacks.current.onReceipt?.(receipt); }, [receipt]);
  useEffect(() => {
    if (!ready || initializing) return;
    const timer = setTimeout(() => setStorageError(!writeCalculationDraft(module, snapshot.current)), 300);
    return () => clearTimeout(timer);
  }, [module, ready, initializing, inputs, output, activeTab, receipt]);
  useEffect(() => {
    const flush = () => { if (readyRef.current) writeCalculationDraft(module, snapshot.current); };
    window.addEventListener('pagehide', flush);
    return () => { flush(); window.removeEventListener('pagehide', flush); };
  }, [module]);
  const save = (update: boolean) => run(async () => {
    const row = await featureApi.savePreset({ module, name, description, payload: inputs }, update ? selected : undefined);
    await refresh(); setSelected(row.id); setMessage('Шаблон сохранён.');
  });
  return <Card>
    <CardContent className="space-y-3 pt-6">
      <details>
      <summary className="cursor-pointer text-base font-medium">Шаблоны и сохранённые расчёты</summary>
      <div className="space-y-3 pt-4">
      <div className="grid gap-3 md:grid-cols-3">
        <label>Шаблон<select aria-label="Шаблон" value={selected} className="w-full border rounded p-2 bg-background" onChange={e => { setSelected(e.target.value); const row = presets.find(p => p.id === e.target.value); setName(row?.name || ''); setDescription(row?.description || ''); }}><option value="">Новый шаблон</option>{presets.map(row => <option key={row.id} value={row.id}>{row.name}</option>)}</select></label>
        <label>Название<Input value={name} maxLength={200} onChange={e => setName(e.target.value)} /></label>
        <label>Описание<Input value={description} maxLength={2000} onChange={e => setDescription(e.target.value)} /></label>
      </div>
      <div className="flex flex-wrap gap-2">
        <Button disabled={busy || !name.trim()} onClick={() => save(false)}>Сохранить новый шаблон</Button>
        <Button variant="outline" disabled={busy || !selected} onClick={() => run(async () => { const row = presets.find(p => p.id === selected); loadRef.current(module === 'furnace' ? row.payload : camelInput(row.payload)); setMessage('Шаблон загружен.'); })}>Загрузить шаблон</Button>
        <Button variant="outline" disabled={busy || !selected || !name.trim()} onClick={() => save(true)}>Обновить выбранный</Button>
        <Button variant="outline" disabled={busy || !selected} onClick={() => run(async () => { await featureApi.deletePreset(selected); setSelected(''); await refresh(); setMessage('Шаблон удалён.'); })}>Удалить шаблон</Button>
        <Link className="underline p-2" to={`/calculations?module=${module}`}>История, сравнение и экспорт</Link>
      </div>
      </div>
      </details>
      {receipt && <div className="space-y-1"><p role="status">Расчёт выполнен. История: {receipt.status === 'Saved' ? 'сохранена' : 'обрабатывается асинхронно'}.</p><p className="text-xs text-muted-foreground">CorrelationId: {receipt.correlationId}</p><a className="underline mr-4" href={featureApi.exportUrl(receipt.id, 'pdf')}>Скачать PDF</a><a className="underline" href={featureApi.exportUrl(receipt.id, 'xlsx')}>Скачать Excel</a>{module === 'aglom-mode' && <Link className="underline ml-4" to={`/slag-mode?sourceCalculationId=${receipt.id}`}>Перейти к Slag Mode</Link>}</div>}
      {storageError && <p role="alert" className="text-destructive">Браузер не разрешил сохранить данные. Проверьте доступ к локальному хранилищу.</p>}
      {message && <p role="status">{message}</p>}{error && <p role="alert" className="text-destructive whitespace-pre-wrap">{error}</p>}
    </CardContent>
  </Card>;
}
