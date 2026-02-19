import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRole?: string;
}

export function ProtectedRoute({ children, requiredRole }: ProtectedRouteProps) {
  const { isAuthenticated, user, loading } = useAuth();
  const location = useLocation();

  if (loading) {
    return <div className="loading">Loading…</div>;
  }

  if (!isAuthenticated || !user) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (requiredRole && user.role !== requiredRole) {
    const roleHome = roleHomePath(user.role);
    return <Navigate to={roleHome} replace />;
  }

  return <>{children}</>;
}

export function roleHomePath(role: string): string {
  switch (role) {
    case 'VP':
      return '/vp';
    case 'Manager':
      return '/manager';
    case 'Surveyor':
      return '/surveyor';
    default:
      return '/';
  }
}
