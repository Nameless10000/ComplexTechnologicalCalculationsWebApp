import { useEffect, useState } from 'react';
import { Layers } from 'lucide-react';
import { PageHeading } from '../CalculationPageHeader';
import { Link } from 'react-router-dom';
import { featureApi, moduleNames, modulePaths } from '../../services/calculation-features';
import { Button } from '../ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '../ui/card';

export function SavedCalculationsPage() {
  const [module, setModule] = useState(new URLSearchParams(window.location.search).get('module') || 'furnace');
  const [rows, setRows] = useState<any[]>([]); const [selected, setSelected] = useState<string[]>([]);
  const [comparison, setComparison] = useState<any>(null); const [error, setError] = useState(''); const [skip, setSkip] = useState(0); const [health, setHealth] = useState<any>(null);
  const refresh = async () => { try { setRows(await featureApi.history(module, skip)); setError(''); } catch (e: any) { setError(e.message); } };
  useEffect(() => { setSelected([]); setComparison(null); void refresh(); }, [module, skip]);
  const compare = async () => { try { setComparison(await featureApi.compare(selected[0], selected[1])); setError(''); } catch (e: any) { setError(e.message); } };
  const differences = (values: any[]) => values.length ? <div className="overflow-x-auto"><table className="w-full text-sm"><thead><tr>{['Параметр', 'A', 'B', 'B − A', 'Отклонение, %'].map(x => <th key={x} className="text-left p-2">{x}</th>)}</tr></thead><tbody>{values.map(row => <tr key={row.parameter} className="border-t"><td className="p-2">{row.parameter}</td><td>{row.missingLeft ? 'нет поля' : String(row.left ?? '—')}</td><td>{row.missingRight ? 'нет поля' : String(row.right ?? '—')}</td><td>{row.absoluteDifference ?? '—'}</td><td>{row.percentageDifference ?? 'не определено'}</td></tr>)}</tbody></table></div> : <p>Отличий нет.</p>;
  return <div className="calculation-page space-y-6"><PageHeading title="Сохранённые расчёты" description="История, сравнение результатов и отчёты" icon={Layers} />
    <div className="flex flex-wrap items-center gap-3"><label>Модуль <select aria-label="Модуль" className="border rounded p-2 bg-background" value={module} onChange={e => { setModule(e.target.value); setSkip(0); }}>{Object.entries(moduleNames).map(([key, value]) => <option key={key} value={key}>{value}</option>)}</select></label><Button variant="outline" onClick={refresh}>Обновить историю</Button><Button disabled={selected.length !== 2} onClick={compare}>Сравнить A и B</Button><Button variant="outline" onClick={async () => { try { setHealth(await featureApi.health()); } catch { setError('Web API недоступен.'); } }}>Состояние сервисов</Button></div>
    <p>Выберите два расчёта. Процентное отклонение считается относительно A; при A = 0 оно не определено.</p>
    {error && <p role="alert" className="text-destructive">{error}</p>}
    {health && <Card><CardHeader><CardTitle>Сервисы: {health.status}</CardTitle></CardHeader><CardContent>{health.checks?.map((check: any) => <p key={check.name}>{check.name}: {check.status} — {check.description}</p>)}</CardContent></Card>}
    {!rows.length && <p>Сохранённых расчётов пока нет.</p>}
    {rows.map(row => <Card key={row.id}><CardContent className="pt-4 space-y-2">{row.requestId === "demo-linked-aglom" && <p className="text-sm text-muted-foreground">Демонстрационный результат Aglom — для проверки переноса полей</p>}<label className="flex gap-2"><input type="checkbox" aria-label={`Выбрать расчёт ${row.id}`} checked={selected.includes(row.id)} disabled={!selected.includes(row.id) && selected.length >= 2} onChange={e => setSelected(e.target.checked ? [...selected, row.id] : selected.filter(id => id !== row.id))} /><span>{new Date(row.createdAt).toLocaleString('ru-RU')} · {row.userName} · История: {row.status === 'Saved' ? 'сохранена' : 'обрабатывается'}</span></label><p className="text-xs text-muted-foreground">ID: {row.id} · CorrelationId: {row.correlationId}</p><div className="flex flex-wrap gap-4"><Link className="underline" to={`${modulePaths[row.module]}?calculationId=${row.id}`}>Загрузить входы</Link><a className="underline" href={featureApi.exportUrl(row.id, 'pdf')}>PDF</a><a className="underline" href={featureApi.exportUrl(row.id, 'xlsx')}>Excel</a>{row.module === 'aglom-mode' && <Link className="underline" to={`/slag-mode?sourceCalculationId=${row.id}`}>Перейти к Slag Mode</Link>}</div><details><summary className="cursor-pointer">Входные данные и результат</summary><pre className="text-xs whitespace-pre-wrap overflow-auto">{JSON.stringify({ input: row.input, output: row.output }, null, 2)}</pre></details></CardContent></Card>)}
    <div className="flex gap-2"><Button variant="outline" disabled={!skip} onClick={() => setSkip(Math.max(0, skip - 50))}>Предыдущая страница</Button><Button variant="outline" disabled={rows.length < 50} onClick={() => setSkip(skip + 50)}>Следующая страница</Button></div>
    {comparison && <Card><CardHeader><CardTitle>Сравнение расчётов</CardTitle></CardHeader><CardContent className="space-y-4"><p>A: {comparison.leftId}<br />B: {comparison.rightId}</p><h2>Входные параметры</h2>{differences(comparison.inputs)}<h2>Результаты</h2>{differences(comparison.results)}</CardContent></Card>}
  </div>;
}
