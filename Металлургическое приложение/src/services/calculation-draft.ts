export interface CalculationDraft {
  version: 1;
  inputs: any;
  output: any;
  activeTab: string;
  receipt: any;
  sourceCalculationId: string | null;
}

function draftKey(module: string): string | null {
  const user = JSON.parse(localStorage.getItem('user') || 'null');
  const owner = user?.email || user?.username;
  return owner ? `ctc:calculation-draft:v1:${encodeURIComponent(owner)}:${module}` : null;
}

export function readCalculationDraft(module: string): CalculationDraft | null {
  try {
    const key = draftKey(module);
    const draft = key ? JSON.parse(localStorage.getItem(key) || 'null') : null;
    return draft?.version === 1 && draft.inputs && typeof draft.inputs === 'object' && !Array.isArray(draft.inputs)
      && typeof draft.activeTab === 'string' ? draft : null;
  } catch { return null; }
}

export function writeCalculationDraft(module: string, draft: CalculationDraft): boolean {
  try {
    const key = draftKey(module);
    if (!key) return false;
    localStorage.setItem(key, JSON.stringify(draft));
    return true;
  } catch { return false; }
}
