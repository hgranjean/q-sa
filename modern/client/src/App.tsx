import { Navigate, Route, BrowserRouter, Routes, Link } from 'react-router-dom';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { ProtectedRoute, roleHomePath } from './components/ProtectedRoute';
import { LoginPage } from './pages/LoginPage';
import { VpHomePage } from './pages/VpHomePage';
import { ManagerHomePage } from './pages/ManagerHomePage';
import { SurveyorSurveysPage } from './pages/SurveyorSurveysPage';
import { SurveyDetailPage } from './pages/SurveyDetailPage';
import './App.css';

function AppShell() {
  const { user, logout, isAuthenticated } = useAuth();

  if (!isAuthenticated || !user) {
    return (
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    );
  }

  const navItems =
    user.role === 'VP'
      ? [{ to: '/vp', label: 'Exec Dashboard' }]
      : user.role === 'Manager'
        ? [{ to: '/manager', label: 'Management Dashboard' }]
        : [{ to: '/surveyor', label: 'My Surveys' }];

  return (
    <div className="app-shell">
      <header className="top-bar">
        <nav className="nav-links">
          {navItems.map(({ to, label }) => (
            <Link key={to} to={to}>
              {label}
            </Link>
          ))}
        </nav>
        <div className="user-bar">
          <span>{user.displayName}</span>
          <span className="role-badge">{user.role}</span>
          <button type="button" onClick={logout}>
            Logout
          </button>
        </div>
      </header>
      <main className="main">
        <Routes>
          <Route path="/" element={<Navigate to={roleHomePath(user.role)} replace />} />
          <Route path="/vp" element={<ProtectedRoute requiredRole="VP"><VpHomePage /></ProtectedRoute>} />
          <Route path="/manager" element={<ProtectedRoute requiredRole="Manager"><ManagerHomePage /></ProtectedRoute>} />
          <Route path="/surveyor" element={<ProtectedRoute requiredRole="Surveyor"><SurveyorSurveysPage /></ProtectedRoute>} />
          <Route path="/surveyor/surveys/:id" element={<ProtectedRoute requiredRole="Surveyor"><SurveyDetailPage /></ProtectedRoute>} />
          <Route path="/login" element={<Navigate to={roleHomePath(user.role)} replace />} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </main>
    </div>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppShell />
      </AuthProvider>
    </BrowserRouter>
  );
}
