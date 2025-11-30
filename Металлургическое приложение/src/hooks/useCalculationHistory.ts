import { useState, useEffect } from 'react';

export interface CalculationHistoryItem {
  id: string;
  timestamp: string;
  note: string;
  results: string;
  type: string;
}

export function useCalculationHistory(calculationType: string) {
  const storageKey = `calculation-history-${calculationType}`;
  
  const [history, setHistory] = useState<CalculationHistoryItem[]>(() => {
    if (typeof window === 'undefined') return [];
    
    try {
      const stored = localStorage.getItem(storageKey);
      return stored ? JSON.parse(stored) : [];
    } catch {
      return [];
    }
  });

  useEffect(() => {
    localStorage.setItem(storageKey, JSON.stringify(history));
  }, [history, storageKey]);

  const addToHistory = (note: string, results: string) => {
    const newItem: CalculationHistoryItem = {
      id: Date.now().toString(),
      timestamp: new Date().toISOString(),
      note,
      results,
      type: calculationType,
    };
    
    setHistory(prev => [newItem, ...prev].slice(0, 50)); // Храним последние 50 записей
  };

  const removeFromHistory = (id: string) => {
    setHistory(prev => prev.filter(item => item.id !== id));
  };

  const clearHistory = () => {
    setHistory([]);
  };

  return {
    history,
    addToHistory,
    removeFromHistory,
    clearHistory,
  };
}
