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
async function fetchWithTimeout(
  url: string,
  options: RequestInit = {},
  timeout = API_CONFIG.TIMEOUT
): Promise<Response> {
  const controller = new AbortController();
  const id = setTimeout(() => controller.abort(), timeout);
  
  try {
    const response = await fetch(url, {
      ...options,
      signal: controller.signal,
      credentials: "include"
    });
    clearTimeout(id);
    return response;
  } catch (error) {
    clearTimeout(id);
    throw error;
  }
}

// Сервис авторизации
export const authService = {
  // Вход в систему
  async login(credentials: LoginCredentials): Promise<User> {
    // Проверка тестовых учетных данных
    if (
      credentials.email === API_CONFIG.TEST_CREDENTIALS.email &&
      credentials.password === API_CONFIG.TEST_CREDENTIALS.password
    ) {
      return API_CONFIG.TEST_CREDENTIALS.userData;
    }
    
    // Реальный запрос к серверу
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.LOGIN}`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(credentials),
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка авторизации');
      }
      
      const data = await response.json();
      return data.user;
    } catch (error) {
      // Если сервер недоступен, показываем ошибку
      console.error('Server error:', error);
      throw new Error('Неверные учетные данные');
    }
  },
  
  // Регистрация
  async register(data: RegisterData): Promise<User> {
    try {
      const response = await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.REGISTER}`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(data),
        }
      );
      
      if (!response.ok) {
        throw new Error('Ошибка регистрации');
      }
      
      const result = await response.json();
      return result.user;
    } catch (error) {
      // Mock регистрация при недоступности сервера
      console.error('Server error:', error);
      return {
        username: data.username,
        email: data.email,
      };
    }
  },
  
  // Выход
  async logout(): Promise<void> {
    try {
      await fetchWithTimeout(
        `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.AUTH.LOGOUT}`,
        {
          method: 'POST',
        }
      );
    } catch (error) {
      console.error('Logout error:', error);
    }
  },
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
      throw new Error('Не удалось выполнить расчет. Проверьте соединение с сервером.');
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
      throw new Error('Не удалось получить пресет. Проверьте соединение с сервером.');
    }
  }
};
