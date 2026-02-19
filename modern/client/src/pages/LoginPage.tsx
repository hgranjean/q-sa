import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { roleHomePath } from '../components/ProtectedRoute';

const SEEDED_USERS = [
  { email: 'vp@example.com', label: 'VP User' },
  { email: 'manager@example.com', label: 'Manager User' },
  { email: 'surveyor@example.com', label: 'Surveyor User' },
  { email: 'surveyor2@example.com', label: 'Surveyor Two' },
];

const isE2E = import.meta.env.VITE_E2E === 'true';
const defaultEmail = isE2E ? 'surveyor@example.com' : SEEDED_USERS[0].email;

export function LoginPage() {
  const [email, setEmail] = useState(defaultEmail);
  const [role, setRole] = useState('');
  const { login, loading, error } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const from = (location.state as { from?: { pathname: string } })?.from?.pathname;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const user = await login(email, role || undefined);
      navigate(from ?? roleHomePath(user.role), { replace: true });
    } catch {
      // error already in state
    }
  };

  return (
    <div className="login-page" data-testid="login-page">
      <h1>Quality Smart Assistant</h1>
      <p>Dev login (stub auth)</p>
      <form onSubmit={handleSubmit} data-testid="login-form">
        <label>
          User
          <select data-testid="login-user-select" value={email} onChange={(e) => setEmail(e.target.value)}>
            {SEEDED_USERS.map((u) => (
              <option key={u.email} value={u.email}>
                {u.label} ({u.email})
              </option>
            ))}
          </select>
        </label>
        <label>
          Override role (optional)
          <select data-testid="login-role-select" value={role} onChange={(e) => setRole(e.target.value)}>
            <option value="">— use account role —</option>
            <option value="VP">VP</option>
            <option value="Manager">Manager</option>
            <option value="Surveyor">Surveyor</option>
          </select>
        </label>
        {error && <p className="error" data-testid="login-error">{error}</p>}
        <button type="submit" disabled={loading} data-testid="login-submit">
          {loading ? 'Signing in…' : 'Sign in'}
        </button>
      </form>
    </div>
  );
}
