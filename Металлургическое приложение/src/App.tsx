import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { Layout } from './components/Layout';
import { LoginPage } from './components/LoginPage';
import { GasDynamicPage } from './components/pages/GasDynamicPage';
import { HeatBalancePage } from './components/pages/HeatBalancePage';
import { MassBalancePage } from './components/pages/MassBalancePage';
import { ReductionPage } from './components/pages/ReductionPage';
import { SlagModePage } from './components/pages/SlagModePage';
import { ThemeProvider } from './components/ThemeProvider';

interface User {
  nickname: string;
  email: string;
}

export default function App() {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Проверка сохраненной сессии
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      setUser(JSON.parse(savedUser));
    }
    setLoading(false);
  }, []);

  const handleLogin = (userData: User) => {
    setUser(userData);
    localStorage.setItem('user', JSON.stringify(userData));
  };

  const handleLogout = () => {
    setUser(null);
    localStorage.removeItem('user');
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-background">
        <div className="animate-pulse">Загрузка...</div>
      </div>
    );
  }

  return (
    <ThemeProvider>
      <Router>
        <Routes>
          <Route
            path="/login"
            element={
              user ? (
                <Navigate to="/" replace />
              ) : (
                <LoginPage onLogin={handleLogin} />
              )
            }
          />
          <Route
            path="/*"
            element={
              user ? (
                <Layout user={user} onLogout={handleLogout}>
                  <Routes>
                    <Route path="/" element={<Navigate to="/gas-dynamic" replace />} />
                    <Route path="/gas-dynamic" element={<GasDynamicPage />} />
                    <Route path="/heat-balance" element={<HeatBalancePage />} />
                    <Route path="/mass-balance" element={<MassBalancePage />} />
                    <Route path="/reduction" element={<ReductionPage />} />
                    <Route path="/slag-mode" element={<SlagModePage />} />
                  </Routes>
                </Layout>
              ) : (
                <Navigate to="/login" replace />
              )
            }
          />
        </Routes>
      </Router>
    </ThemeProvider>
  );
}