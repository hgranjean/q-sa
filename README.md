Quality Smart Assistant
=======================

Quality Management platform focused on surveillance, assessments, and completion analytics.

## Brand Intent
- Product name: Quality Smart Assistant
- Product focus: Quality Management
- Outcome focus: improve assessment quality, completion rates, and operational visibility

## Modern MVP Artifacts
- API contract: `openapi.yaml`
- Backend: `modern/` — Clean Architecture (Domain, Application, Infrastructure, WebApi)
- Frontend: `modern/client` — React (Vite + TypeScript + React Router)

## Running locally (dev)

**Backend (stub auth, no DB required):**
```bash
cd modern
dotnet run --project QualitySmartAssistant.Api
```
API: http://localhost:5251 (Swagger: http://localhost:5251/swagger)

**Frontend:**
```bash
cd modern/client
npm install
npm run dev
```
App: http://localhost:5173 — use dev login (e.g. vp@example.com, manager@example.com, surveyor@example.com). The dev server proxies `/api` to the backend.

**Optional:** Set `VITE_API_URL=http://localhost:5251` if not using the proxy.

## Immediate MVP Scope
- **Auth (Vertical Slice 1):** Dev login, `GET /me`, role-based nav (VP / Manager / Surveyor), JWT stub auth. See `.github/copilot-instructions.md` for architecture and “Definition of Done”.
- Identity context: `GET /auth/me`, `GET /me`
- Survey lifecycle: `POST /surveys`, `POST /surveys/{surveyId}/publish`
- Assignment lifecycle: `POST /assignments`
- Response capture: `POST /responses`
- Reporting: `GET /reports/completion`
