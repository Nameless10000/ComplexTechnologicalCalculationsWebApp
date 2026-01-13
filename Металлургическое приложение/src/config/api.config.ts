// Конфигурация API
export const API_CONFIG = {
  // Базовый URL API сервера
  BASE_URL: typeof import.meta !== 'undefined' && import.meta.env?.VITE_API_BASE_URL 
    ? import.meta.env.VITE_API_BASE_URL 
    : 'https://localhost:7206',
  
  // Эндпоинты
  ENDPOINTS: {
    AUTH: {
      LOGIN: '/auth/Authorize',
      REGISTER: '/auth/SignUo',
      LOGOUT: '/auth/logout',
    },
    GAS_DYNAMIC: {
      CALCULATE: '/GasDynamic/Calculate',
      GET_PRESET: '/GasDynamic/GetPreset',
      MARK_AS_PRESET: '/GasDynamic/MarkCalculationAsPreset',
      LOAD_CALCULATION: '/GasDynamic/LoadCalculation',
      GET_HISTORY: '/GasDynamic/GetCalculationsHistory',
    },
    SLAG_MODE: {
      CALCULATE: '/SlagMode/Calculate',
      GET_PRESET: '/SlagMode/GetPreset',
      LOAD_CALCULATION: '/SlagMode/LoadCalculation',
      GET_HISTORY: '/SlagMode/GetCalculationsHistory',
      GET_COMPONENTS: '/SlagMode/GetChargeComponents',
    },
  },
  
  // Таймауты
  TIMEOUT: 30000, // 30 секунд
  
  // Тестовые учетные данные (для обхода сервера)
  TEST_CREDENTIALS: {
    email: 'admin@mail.ru',
    password: 'admin',
    userData: {
      username: 'Admin',
      email: 'admin@mail.ru',
    }
  }
};