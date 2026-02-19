const API_BASE = import.meta.env.VITE_API_URL ?? '/api';

export interface AssignedSurvey {
  id: string;
  title: string;
  dueDate: string;
  status: string;
  priority: string;
  locationName?: string;
  assignedAt: string;
}

export interface SurveyDetail {
  id: string;
  title: string;
  dueDate: string;
  status: string;
  priority: string;
  locationName?: string;
  assignedAt?: string;
}

export async function getAssignedSurveys(token: string): Promise<AssignedSurvey[]> {
  const res = await fetch(`${API_BASE}/surveys/assigned`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (res.status === 401) throw new Error('Unauthorized');
  if (res.status === 403) throw new Error('Forbidden');
  if (!res.ok) throw new Error('Failed to load surveys: ' + res.status);
  return res.json();
}

export async function getSurveyById(token: string, id: string): Promise<SurveyDetail> {
  const res = await fetch(`${API_BASE}/surveys/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (res.status === 401) throw new Error('Unauthorized');
  if (res.status === 403) throw new Error('Forbidden');
  if (res.status === 404) throw new Error('Not found');
  if (!res.ok) throw new Error('Failed to load survey: ' + res.status);
  return res.json();
}

export interface ChecklistItemDto {
  id: string;
  text: string;
  isRequired: boolean;
  sortOrder: number;
}

export interface ChecklistResponseDto {
  itemId: string;
  value: string;
  notes: string | null;
  updatedAt: string;
}

export interface SurveyChecklistDto {
  surveyId: string;
  status: string;
  items: ChecklistItemDto[];
  responses: ChecklistResponseDto[];
}

export interface SubmitResultDto {
  surveyId: string;
  status: string;
  submittedAt: string;
}

export interface ValidationErrorDto {
  code: string;
  message: string;
  missingRequiredItemIds: string[];
}

export async function getSurveyChecklist(token: string, surveyId: string): Promise<SurveyChecklistDto> {
  const res = await fetch(`${API_BASE}/surveys/${surveyId}/checklist`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (res.status === 401) throw new Error('Unauthorized');
  if (res.status === 403) throw new Error('Forbidden');
  if (res.status === 404) throw new Error('Not found');
  if (!res.ok) throw new Error('Failed to load checklist: ' + res.status);
  return res.json();
}

export async function putChecklistResponse(
  token: string,
  surveyId: string,
  itemId: string,
  value: string,
  notes?: string | null
): Promise<{ itemId: string; updatedAt: string }> {
  const res = await fetch(`${API_BASE}/surveys/${surveyId}/responses/${itemId}`, {
    method: 'PUT',
    headers: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ value, notes: notes ?? null }),
  });
  if (res.status === 401) throw new Error('Unauthorized');
  if (res.status === 403) throw new Error('Forbidden');
  if (!res.ok) throw new Error('Failed to save: ' + res.status);
  return res.json();
}

export async function submitSurvey(token: string, surveyId: string): Promise<SubmitResultDto> {
  const res = await fetch(`${API_BASE}/surveys/${surveyId}/submit`, {
    method: 'POST',
    headers: { Authorization: `Bearer ${token}` },
  });
  if (res.status === 401) throw new Error('Unauthorized');
  if (res.status === 403) throw new Error('Forbidden');
  if (res.status === 400) {
    const body = await res.json() as ValidationErrorDto;
    const err = new Error(body.message) as Error & { missingRequiredItemIds?: string[] };
    err.missingRequiredItemIds = body.missingRequiredItemIds ?? [];
    throw err;
  }
  if (!res.ok) throw new Error('Submit failed: ' + res.status);
  return res.json();
}
