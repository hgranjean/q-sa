import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react';
import { getMe, devLogin, type UserDto } from '../api/auth';

const TOKEN_KEY = 'qsa_token';

interface AuthState {
  token: string | null;
  user: UserDto | null;
  loading: boolean;
  error: string | null;
}

interface AuthContextValue extends AuthState {
  login: (email: string, role?: string) => Promise<UserDto>;
  logout: () => void;
  isAuthenticated: boolean;
  hasRole: (role: string) => boolean;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, setState] = useState<AuthState>({
    token: localStorage.getItem(TOKEN_KEY),
    user: null,
    loading: true,
    error: null,
  });

  const logout = useCallback(() => {
    localStorage.removeItem(TOKEN_KEY);
    setState({ token: null, user: null, loading: false, error: null });
  }, []);

  const login = useCallback(async (email: string, role?: string): Promise<UserDto> => {
    setState((s) => ({ ...s, loading: true, error: null }));
    try {
      const { token, user } = await devLogin(email, role);
      localStorage.setItem(TOKEN_KEY, token);
      setState({ token, user, loading: false, error: null });
      return user;
    } catch (e) {
      setState((s) => ({
        ...s,
        loading: false,
        error: e instanceof Error ? e.message : 'Login failed',
      }));
      throw e;
    }
  }, []);

  useEffect(() => {
    const token = state.token;
    if (!token) {
      setState((s) => ({ ...s, loading: false }));
      return;
    }
    getMe(token)
      .then((user) => setState((s) => ({ ...s, user, loading: false, error: null })))
      .catch(() => {
        localStorage.removeItem(TOKEN_KEY);
        setState({ token: null, user: null, loading: false, error: null });
      });
  }, [state.token]);

  const value = useMemo<AuthContextValue>(
    () => ({
      ...state,
      login,
      logout,
      isAuthenticated: !!state.token && !!state.user,
      hasRole: (role: string) => (state.user?.role === role) ?? false,
    }),
    [state, login, logout]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
}
