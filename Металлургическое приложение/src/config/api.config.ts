// Конфигурация API
export const API_CONFIG = {
  // Базовый URL API сервера
  BASE_URL: typeof import.meta !== 'undefined' && import.meta.env?.VITE_API_BASE_URL 
    ? import.meta.env.VITE_API_BASE_URL 
    : 'http://localhost:5000/api',
  
  // Эндпоинты
  ENDPOINTS: {
    AUTH: {
      LOGIN: '/auth/login',
      REGISTER: '/auth/register',
      LOGOUT: '/auth/logout',
    },
    GAS_DYNAMIC: {
      CALCULATE: '/calculations/gas-dynamic/calculate',
      PRESET: '/calculations/gas-dynamic/preset',
    },
  },
  
  // Таймауты
  TIMEOUT: 30000, // 30 секунд
  
  // Тестовые учетные данные (для обхода сервера)
  TEST_CREDENTIALS: {
    email: 'admin@mail.ru',
    password: 'admin',
    userData: {
      nickname: 'Admin',
      email: 'admin@mail.ru',
    }
  }
};