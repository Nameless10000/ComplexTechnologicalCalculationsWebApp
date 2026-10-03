import { API_CONFIG } from '../config/api.config';
import { fetchWithTimeout } from './api.service';

export function camelInput(value: any): any {
  if (Array.isArray(value)) return value.map(camelInput);
  if (value && typeof value === 'object') return Object.fromEntries(Object.entries(value).map(([key, item]) => [key[0].toLowerCase() + key.slice(1), camelInput(item)]));
  return value;
}

async function request(path: string, options?: RequestInit) {
  const response = await fetchWithTimeout(`${API_CONFIG.BASE_URL}${path}`, options);
  return response.status === 204 ? null : response.json();
}

export const featureApi = {
  presets: (module: string) => request(`/presets?module=${encodeURIComponent(module)}`),
  savePreset: (input: any, id?: string) => request(id ? `/presets/${id}` : '/presets', { method: id ? 'PUT' : 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(input) }),
  deletePreset: (id: string) => request(`/presets/${id}`, { method: 'DELETE' }),
  history: (module: string, skip = 0) => request(`/calculations?module=${encodeURIComponent(module)}&skip=${skip}&take=50`),
  calculation: (id: string) => request(`/calculations/${id}`),
  compare: (left: string, right: string) => request(`/calculations/compare?leftId=${left}&rightId=${right}`),
  transition: (id: string, input: any, target = 'slag-mode') => request(`/calculations/${id}/transition/${target}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(input) }),
  exportUrl: (id: string, format: string) => `${API_CONFIG.BASE_URL}/calculations/${id}/export?format=${format}`,
  health: async () => (await fetch(`${API_CONFIG.BASE_URL}/health/ready`, { credentials: 'include' })).json(),
};

export const moduleNames: Record<string, string> = { 'aglom-mode': 'Агломерационная шихта', 'slag-mode': 'Шлаковый режим', 'gas-dynamic': 'Газодинамический режим', furnace: 'Тепловой баланс' };
export const modulePaths: Record<string, string> = { 'aglom-mode': '/sinter-charge', 'slag-mode': '/slag-mode', 'gas-dynamic': '/gas-dynamic', furnace: '/heat-balance' };
