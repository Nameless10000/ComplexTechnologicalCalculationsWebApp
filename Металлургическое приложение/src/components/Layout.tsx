import { Link, useLocation } from 'react-router-dom';
import { Avatar, AvatarFallback } from './ui/avatar';
import { Button } from './ui/button';
import { LogOut, Moon, Sun, Wind, Flame, Scale, Zap, Droplets } from 'lucide-react';
import { useTheme } from './ThemeProvider';

interface User {
  nickname: string;
  email: string;
}

interface LayoutProps {
  user: User;
  onLogout: () => void;
  children: React.ReactNode;
}

const navItems = [
  { path: '/gas-dynamic', label: 'Газодинамический режим', icon: Wind },
  { path: '/heat-balance', label: 'Теплообмен в печи', icon: Flame },
  { path: '/mass-balance', label: 'Массовый баланс', icon: Scale },
  { path: '/reduction', label: 'Восстановительные процессы', icon: Zap },
  { path: '/slag-mode', label: 'Шлаковый режим', icon: Droplets },
];

export function Layout({ user, onLogout, children }: LayoutProps) {
  const location = useLocation();
  const { theme, toggleTheme } = useTheme();

  const getInitials = (nickname: string) => {
    return nickname.slice(0, 2).toUpperCase();
  };

  return (
    <div className="flex min-h-screen bg-background">
      {/* Sidebar */}
      <aside className="w-64 border-r border-border bg-card">
        <div className="flex flex-col h-full">
          {/* Header */}
          <div className="p-6 border-b border-border">
            <h1 className="text-xl tracking-tight mb-1">Металлургические расчеты</h1>
            <p className="text-sm text-muted-foreground">Инженерные инструменты</p>
          </div>

          {/* User Info */}
          <div className="p-4 border-b border-border">
            <div className="flex items-center gap-3">
              <Avatar>
                <AvatarFallback className="bg-primary text-primary-foreground">
                  {getInitials(user.nickname)}
                </AvatarFallback>
              </Avatar>
              <div className="flex-1 min-w-0">
                <p className="text-sm truncate">{user.nickname}</p>
                <p className="text-xs text-muted-foreground truncate">{user.email}</p>
              </div>
            </div>
          </div>

          {/* Navigation */}
          <nav className="flex-1 p-4">
            <ul className="space-y-1">
              {navItems.map((item) => {
                const Icon = item.icon;
                const isActive = location.pathname === item.path;
                return (
                  <li key={item.path}>
                    <Link
                      to={item.path}
                      className={`flex items-center gap-3 px-3 py-2 rounded-lg transition-colors ${
                        isActive
                          ? 'bg-primary text-primary-foreground'
                          : 'hover:bg-accent hover:text-accent-foreground'
                      }`}
                    >
                      <Icon className="size-5" />
                      <span className="text-sm">{item.label}</span>
                    </Link>
                  </li>
                );
              })}
            </ul>
          </nav>

          {/* Footer Actions */}
          <div className="p-4 border-t border-border space-y-2">
            <Button
              variant="outline"
              className="w-full justify-start"
              onClick={toggleTheme}
            >
              {theme === 'dark' ? (
                <>
                  <Sun className="size-4 mr-2" />
                  Светлая тема
                </>
              ) : (
                <>
                  <Moon className="size-4 mr-2" />
                  Темная тема
                </>
              )}
            </Button>
            <Button
              variant="destructive"
              className="w-full justify-start"
              onClick={onLogout}
            >
              <LogOut className="size-4 mr-2" />
              Выход
            </Button>
          </div>
        </div>
      </aside>

      {/* Main Content */}
      <main className="flex-1 overflow-auto">
        <div className="container mx-auto p-8">
          {children}
        </div>
      </main>
    </div>
  );
}