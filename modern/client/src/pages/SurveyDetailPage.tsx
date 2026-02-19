import { useCallback, useEffect, useRef, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import {
  getSurveyById,
  getSurveyChecklist,
  putChecklistResponse,
  submitSurvey,
  type SurveyDetail,
  type SurveyChecklistDto,
  type ChecklistItemDto,
} from '../api/surveys';

const SAVE_DEBOUNCE_MS = 600;
const VALUE_OPTIONS = ['Pass', 'Fail', 'NA'] as const;

type SaveState = 'idle' | 'saving' | 'saved' | 'error';

export function SurveyDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user, token, logout } = useAuth();
  const [survey, setSurvey] = useState<SurveyDetail | null>(null);
  const [checklist, setChecklist] = useState<SurveyChecklistDto | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [missingRequiredIds, setMissingRequiredIds] = useState<string[]>([]);
  const [submitSuccess, setSubmitSuccess] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [localResponses, setLocalResponses] = useState<Record<string, { value: string; notes: string }>>({});
  const [saveState, setSaveState] = useState<SaveState>('idle');
  const [pendingSaves, setPendingSaves] = useState<Set<string>>(new Set());
  const debounceRefs = useRef<Record<string, ReturnType<typeof setTimeout>>>({});
  const localResponsesRef = useRef<Record<string, { value: string; notes: string }>>({});
  localResponsesRef.current = localResponses;

  const isSubmitted = checklist?.status === 'Submitted' || checklist?.status === 'Completed' || submitSuccess;
  const hasUnsaved = pendingSaves.size > 0;

  const flushSave = useCallback(
    (itemId: string) => {
      const current = localResponsesRef.current[itemId];
      if (!token || !id || !current?.value) return;
      const { value, notes } = current;
      setSaveState('saving');
      putChecklistResponse(token, id, itemId, value, notes || null)
        .then(() => {
          setSaveState('saved');
          setPendingSaves((prev) => {
            const next = new Set(prev);
            next.delete(itemId);
            return next;
          });
          setTimeout(() => setSaveState('idle'), 1500);
        })
        .catch(() => {
          setSaveState('error');
        });
    },
    [token, id]
  );

  const scheduleSave = useCallback(
    (itemId: string, value: string, notes: string) => {
      if (debounceRefs.current[itemId]) clearTimeout(debounceRefs.current[itemId]);
      debounceRefs.current[itemId] = setTimeout(() => {
        flushSave(itemId);
        delete debounceRefs.current[itemId];
      }, SAVE_DEBOUNCE_MS);
      setLocalResponses((prev) => ({ ...prev, [itemId]: { value, notes } }));
      setPendingSaves((prev) => new Set(prev).add(itemId));
    },
    [flushSave]
  );

  useEffect(() => {
    if (!token || !id) return;
    getSurveyById(token, id)
      .then(setSurvey)
      .catch((e) => {
        const msg = e instanceof Error ? e.message : 'Failed';
        if (msg === 'Unauthorized') {
          logout();
          navigate('/login', { replace: true });
          return;
        }
        setError(msg);
      });
  }, [token, id, logout, navigate]);

  useEffect(() => {
    if (!token || !id) return;
    getSurveyChecklist(token, id)
      .then((c) => {
        setChecklist(c);
        const initial: Record<string, { value: string; notes: string }> = {};
        c.responses.forEach((r) => {
          initial[r.itemId] = { value: r.value, notes: r.notes ?? '' };
        });
        setLocalResponses(initial);
      })
      .catch((e) => {
        const msg = e instanceof Error ? e.message : 'Failed';
        if (msg === 'Unauthorized') {
          logout();
          navigate('/login', { replace: true });
          return;
        }
        setError(msg);
      });
  }, [token, id, logout, navigate]);

  const handleSubmit = () => {
    if (!token || !id || hasUnsaved) return;
    const requiredIds = checklist?.items.filter((i) => i.isRequired).map((i) => i.id) ?? [];
    const answered = new Set(
      checklist?.items.filter((i) => localResponses[i.id]?.value).map((i) => i.id) ?? []
    );
    const missing = requiredIds.filter((rid) => !answered.has(rid));
    if (missing.length > 0) {
      setMissingRequiredIds(missing);
      setSubmitError('Please answer all required items before submitting.');
      return;
    }
    setSubmitting(true);
    setSubmitError(null);
    setMissingRequiredIds([]);
    submitSurvey(token, id)
      .then(() => {
        setSubmitSuccess(true);
        setChecklist((c) => (c ? { ...c, status: 'Submitted' } : null));
      })
      .catch((e: Error & { missingRequiredItemIds?: string[] }) => {
        if (e.missingRequiredItemIds?.length) {
          setMissingRequiredIds(e.missingRequiredItemIds);
          setSubmitError(e.message);
        } else {
          setSubmitError(e.message);
        }
      })
      .finally(() => setSubmitting(false));
  };

  if (user?.role !== 'Surveyor') {
    return (
      <div className="role-home">
        <p className="ping-err">Not authorized.</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="role-home" data-testid="survey-detail-error">
        <p className="ping-err" data-testid="validation-banner">
          {error === 'Forbidden' ? 'You do not have access to this survey.' : error === 'Unauthorized' ? 'Please log in again.' : error}
        </p>
        <button type="button" onClick={() => navigate('/surveyor')}>
          Back to My Surveys
        </button>
      </div>
    );
  }

  if (!survey || !checklist) return <div className="loading" data-testid="survey-detail-loading">Loading…</div>;

  return (
    <div className="role-home survey-detail" data-testid="survey-detail-page">
      <h1>{survey.title}</h1>
      <button type="button" onClick={() => navigate('/surveyor')} className="back-link" data-testid="back-to-surveys">
        ← Back to My Surveys
      </button>
      <dl className="detail-fields">
        <dt>Due date</dt>
        <dd>{survey.dueDate}</dd>
        <dt>Status</dt>
        <dd>{checklist.status}</dd>
        <dt>Priority</dt>
        <dd>{survey.priority}</dd>
        {survey.locationName && (
          <>
            <dt>Location</dt>
            <dd>{survey.locationName}</dd>
          </>
        )}
        {survey.assignedAt && (
          <>
            <dt>Assigned at</dt>
            <dd>{new Date(survey.assignedAt).toLocaleString()}</dd>
          </>
        )}
      </dl>

      <section className="checklist-section" data-testid="checklist-section">
        <h2>Checklist</h2>
        {saveState !== 'idle' && (
          <p className="save-status" data-testid="checklist-save-status">
            {saveState === 'saving' && 'Saving…'}
            {saveState === 'saved' && 'Saved'}
            {saveState === 'error' && 'Save failed. Try again.'}
          </p>
        )}
        {submitError && <p className="ping-err" data-testid="validation-banner">{submitError}</p>}
        {submitSuccess && <p className="submit-success" data-testid="submit-success">Survey submitted successfully.</p>}

        <ul className="checklist-items">
          {checklist.items.map((item) => (
            <ChecklistRow
              key={item.id}
              item={item}
              value={localResponses[item.id]?.value ?? ''}
              notes={localResponses[item.id]?.notes ?? ''}
              disabled={isSubmitted}
              isMissing={missingRequiredIds.includes(item.id)}
              onValueChange={(value) => scheduleSave(item.id, value, localResponses[item.id]?.notes ?? '')}
              onNotesChange={(notes) => scheduleSave(item.id, localResponses[item.id]?.value ?? '', notes)}
            />
          ))}
        </ul>

        {!isSubmitted && (
          <div className="submit-actions">
            <button
              type="button"
              onClick={handleSubmit}
              disabled={hasUnsaved || submitting}
              className="submit-btn"
              data-testid="submit-survey-btn"
            >
              {submitting ? 'Submitting…' : 'Submit Survey'}
            </button>
            {hasUnsaved && <span className="hint">Save all changes before submitting.</span>}
          </div>
        )}
      </section>

      <section className="placeholder-section">
        <h2>Findings</h2>
        <p className="placeholder">Coming in Slice 3/4.</p>
      </section>
    </div>
  );
}

function ChecklistRow({
  item,
  value,
  notes,
  disabled,
  isMissing,
  onValueChange,
  onNotesChange,
}: {
  item: ChecklistItemDto;
  value: string;
  notes: string;
  disabled: boolean;
  isMissing: boolean;
  onValueChange: (v: string) => void;
  onNotesChange: (n: string) => void;
}) {
  return (
    <li className={`checklist-row ${isMissing ? 'missing-required' : ''}`} data-testid={`checklist-item-${item.id}`}>
      <div className="checklist-item-header">
        <span className="item-text">{item.text}</span>
        {item.isRequired && <span className="required-badge">Required</span>}
      </div>
      <div className="checklist-item-body">
        <div className="value-options">
          {VALUE_OPTIONS.map((opt) => (
            <label key={opt} className="value-option">
              <input
                type="radio"
                name={`item-${item.id}`}
                value={opt}
                checked={value === opt}
                onChange={() => onValueChange(opt)}
                disabled={disabled}
                data-testid={`checklist-item-${item.id}-value-${opt}`}
              />
              {opt}
            </label>
          ))}
        </div>
        <div className="notes-row">
          <label>
            Notes
            <input
              type="text"
              value={notes}
              onChange={(e) => onNotesChange(e.target.value)}
              placeholder="Optional"
              disabled={disabled}
              className="notes-input"
              data-testid={`checklist-item-${item.id}-notes`}
            />
          </label>
        </div>
      </div>
    </li>
  );
}
