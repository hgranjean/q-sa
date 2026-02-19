import { useEffect, useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { pingManager } from '../api/auth';

export function ManagerHomePage() {
  const { user, token } = useAuth();
  const [ping, setPing] = useState<{ role: string; message: string } | null>(null);
  const [pingError, setPingError] = useState<string | null>(null);

  useEffect(() => {
    if (!token) return;
    pingManager(token)
      .then(setPing)
      .catch((e) => setPingError(e instanceof Error ? e.message : 'Failed'));
  }, [token]);

  return (
    <div className="role-home">
      <h1>Management Dashboard</h1>
      <p>Welcome, {user?.displayName} ({user?.role}).</p>
      {ping && <p className="ping-ok">Role ping: {ping.role} — {ping.message}</p>}
      {pingError && <p className="ping-err">Ping error: {pingError}</p>}
    </div>
  );
}
