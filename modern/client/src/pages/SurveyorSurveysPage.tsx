import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { getAssignedSurveys, type AssignedSurvey } from '../api/surveys';

export function SurveyorSurveysPage() {
  const { user, token, logout } = useAuth();
  const navigate = useNavigate();
  const [surveys, setSurveys] = useState<AssignedSurvey[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!token) return;
    getAssignedSurveys(token)
      .then((list) => {
        const sorted = [...list].sort((a, b) => {
          const d = new Date(a.dueDate).getTime() - new Date(b.dueDate).getTime();
          if (d !== 0) return d;
          const pOrder = { High: 0, Medium: 1, Low: 2 };
          return (pOrder[a.priority as keyof typeof pOrder] ?? 1) - (pOrder[b.priority as keyof typeof pOrder] ?? 1);
        });
        setSurveys(sorted);
      })
      .catch((e) => {
        const msg = e instanceof Error ? e.message : 'Failed to load';
        if (msg === 'Unauthorized') {
          logout();
          navigate('/login', { replace: true });
          return;
        }
        setError(msg);
      })
      .finally(() => setLoading(false));
  }, [token, logout, navigate]);

  if (user?.role !== 'Surveyor') {
    return (
      <div className="role-home">
        <p className="ping-err">Not authorized. Only Surveyors can view assigned surveys.</p>
      </div>
    );
  }

  if (loading) return <div className="loading">Loading surveys…</div>;
  if (error) return <div className="ping-err">Error: {error}. {error === 'Unauthorized' ? 'Please log in again.' : ''}</div>;

  return (
    <div className="role-home surveyor-queue" data-testid="surveyor-surveys-page">
      <h1>My Surveys</h1>
      <p>Welcome, {user?.displayName}. Assigned surveys (due date ↑, then priority).</p>
      <table className="surveys-table" data-testid="surveys-table">
        <thead>
          <tr>
            <th>Title</th>
            <th>Location</th>
            <th>Due Date</th>
            <th>Status</th>
            <th>Priority</th>
          </tr>
        </thead>
        <tbody>
          {surveys.map((s) => (
            <tr key={s.id} data-testid={`survey-row-${s.id}`} onClick={() => navigate(`/surveyor/surveys/${s.id}`)} className="row-clickable">
              <td>{s.title}</td>
              <td>{s.locationName ?? '—'}</td>
              <td>{s.dueDate}</td>
              <td>{s.status}</td>
              <td>{s.priority}</td>
            </tr>
          ))}
        </tbody>
      </table>
      {surveys.length === 0 && <p data-testid="no-surveys">No surveys assigned.</p>}
    </div>
  );
}
