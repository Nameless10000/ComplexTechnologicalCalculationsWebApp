import { API_CONFIG } from '../config/api.config';

interface User {
  username: string;
  email: string;
}

interface LoginCredentials {
  email: string;
  password: string;
}

interface RegisterData extends LoginCredentials {
  username: string;
}

// Вспомогательная функция для HTTP запросов
export async function fetchWithTimeout(
  url: string,
  options: RequestInit = {},
  timeout = API_CONFIG.TIMEOUT
): Promise<Response> {
  const controller = new AbortController();
  const id = setTimeout(() => controller.abort(), timeout);
  
  try {
    const headers = new Headers(options.headers);
    const sourceId = new URLSearchParams(window.location.search).get('sourceCalculationId');
    if (sourceId && url.includes('/SlagMode/Calculate')) headers.set('X-Source-Calculation-Id', sourceId);
    const response = await fetch(url, {
      ...options,
      headers,
      signal: controller.signal,
      credentials: "include"
    });
    clearTimeout(id);
    if (!response.ok) {
      const body = await response.json().catch(() => ({}));
      const details = body.details ? `\n${JSON.stringify(body.details)}` : '';
      const trace = body.traceId ? `\ntraceId: ${body.traceId}` : '';
      throw new Error(`${body.message || 'Не удалось выполнить запрос.'}${details}${trace}`);
    }
    const calculationId = response.headers.get('X-Calculation-Id');
    if (calculationId) {
      const module = url.includes('/Furnace/') ? 'furnace' : url.includes('/AglomMode/') ? 'aglom-mode' : url.includes('/SlagMode/') ? 'slag-mode' : 'gas-dynamic';
      window.dispatchEvent(new CustomEvent('calculation-completed', { detail: { module, id: calculationId, status: response.headers.get('X-History-Status'), correlationId: response.headers.get('X-Correlation-Id') } }));
    }
    return response;
  } catch (error) {
    clearTimeout(id);
    throw error;
  }
}

// Authentication uses the server cookie; local fallback cannot provide record ownership.
export const authService = {
  async login(credentials: LoginCredentials): Promise<User> {
    const response = await fetchWithTimeout(`${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.LOGIN}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(credentials) });
    return (await response.json()).user;
  },
  async register(data: RegisterData): Promise<User> {
    const response = await fetchWithTimeout(`${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.REGISTER}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) });
    return (await response.json()).user;
  },
  async logout(): Promise<void> { await fetchWithTimeout(`${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.LOGOUT}`, { method: 'POST' }); },
  async me(): Promise<User> { return (await (await fetchWithTimeout(`${API_CONFIG.BASE_URL}/Auth/Me`)).json()).user; }
};

// Сервис расчетов газодинамики
export const gasDynamicService = {
  async calculate(inputData: any): Promise<any> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.GAS_DYNAMIC.CALCULATE}`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(inputData),
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка расчета');
      }
      
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Calculation error:', error);
      throw error;
    }
  },

  async getPreset() :Promise<any> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.GAS_DYNAMIC.GET_PRESET}`,
        {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          }
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка получения пресета');
      }
      
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Preset error:', error);
      throw error;
    }
  }
};

export const slagModeService = {
  async calculate(inputData: any): Promise<any> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.SLAG_MODE.CALCULATE}`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(inputData),
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка расчета');
      }
      
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Calculation error:', error);
      throw error;
    }
  },

  async getPreset() :Promise<any> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.SLAG_MODE.GET_PRESET}`,
        {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          }
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка получения пресета');
      }
      
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Preset error:', error);
      throw error;
    }
  },

  async getComponents() :Promise<any> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.SLAG_MODE.GET_COMPONENTS}`,
        {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          }
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка получения шихтовых материалов');
      }
      
      const data = await response.json();

      if (data.message) {
        throw new Error(data.message);
      }

      return data;
    } catch (error) {
      console.error('Preset error:', error);
      throw error;
    }
  }
};

export const aglomModeService = {
  async calculate(inputData: any): Promise<any> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AGLOM_MODE.CALCULATE}`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(inputData),
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка расчета');
      }
      
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Calculation error:', error);
      throw error;
    }
  },

  async getPreset() :Promise<any> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AGLOM_MODE.GET_PRESET}`,
        {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          }
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка получения пресета');
      }
      
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Preset error:', error);
      throw error;
    }
  }
};

export const furnaceService = {
  async calculate(inputData: any): Promise<any> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.FURNACE.CALCULATE}`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(inputData),
        }
      );

      if (!response.ok) {
        throw new Error('Ошибка расчета');
      }

      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Calculation error:', error);
      throw error;
    }
  },
};