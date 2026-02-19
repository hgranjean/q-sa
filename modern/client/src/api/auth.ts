const API_BASE = import.meta.env.VITE_API_URL ?? '/api';

export interface UserDto {
  id: string;
  email: string;
  displayName: string;
  role: string;
}

export interface DevLoginResponse {
  token: string;
  user: UserDto;
}

export async function devLogin(email: string, role?: string): Promise<DevLoginResponse> {
  const res = await fetch(`${API_BASE}/auth/dev-login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, role }),
  });
  if (!res.ok) {
    const err = await res.json().catch(() => ({}));
    throw new Error(err?.error?.message ?? `Login failed: ${res.status}`);
  }
  return res.json();
}

export async function getMe(token: string): Promise<UserDto> {
  const res = await fetch(`${API_BASE}/me`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!res.ok) throw new Error('Session invalid');
  return res.json();
}

export async function pingVp(token: string): Promise<{ role: string; message: string }> {
  const res = await fetch(`${API_BASE}/vp/ping`, { headers: { Authorization: `Bearer ${token}` } });
  if (!res.ok) throw new Error('VP ping failed');
  return res.json();
}

export async function pingManager(token: string): Promise<{ role: string; message: string }> {
  const res = await fetch(`${API_BASE}/manager/ping`, { headers: { Authorization: `Bearer ${token}` } });
  if (!res.ok) throw new Error('Manager ping failed');
  return res.json();
}

export async function pingSurveyor(token: string): Promise<{ role: string; message: string }> {
  const res = await fetch(`${API_BASE}/surveyor/ping`, { headers: { Authorization: `Bearer ${token}` } });
  if (!res.ok) throw new Error('Surveyor ping failed');
  return res.json();
}
