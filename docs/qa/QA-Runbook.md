# QA Runbook — E2E and Manual QA Cycle

This runbook describes how to run the automated E2E suite and perform a lightweight QA pass for the Surveyor workflow (Vertical Slices 2 and 3).

---

## 1. Running QA Locally

### Prerequisites

- **.NET 8** SDK
- **Node.js 20+** and npm
- Backend and frontend build at least once (see below)

### One-command E2E (recommended)

From the **frontend** directory, start API + frontend in E2E mode and run Playwright:

```bash
cd modern/client
npm run e2e:local
```

This will:

1. Start the API with launch profile **E2E** on `http://localhost:5070`
2. Start the Vite dev server with `VITE_E2E=true` and `VITE_API_URL=http://localhost:5070` on `http://localhost:5173`
3. Wait for `GET http://localhost:5070/health` and the UI to respond
4. Run the Playwright test suite (Chromium)
5. Exit and stop both processes

### Alternative: script from repo root or `modern`

- **Windows (PowerShell):**  
  `./modern/scripts/run-e2e.ps1` (from repo root) or `./scripts/run-e2e.ps1` (from `modern/`)
- **Linux/macOS:**  
  `./modern/scripts/run-e2e.sh` (from repo root) or `./scripts/run-e2e.sh` (from `modern/`)

### E2E only (services already running)

If the API and frontend are already running in E2E mode:

```bash
cd modern/client
npm run e2e
```

Or with UI:

```bash
npm run e2e:ui
```

Set (or leave default) base URL:

- `PLAYWRIGHT_BASE_URL=http://localhost:5173` (default)

### Manual E2E mode (for exploratory QA)

1. **Backend (E2E):**
   - From `modern/QualitySmartAssistant.Api`:  
     `dotnet run --launch-profile E2E`  
   - API: `http://localhost:5070`

2. **Frontend (E2E):**
   - From `modern/client`:  
     `VITE_E2E=true VITE_API_URL=http://localhost:5070 npm run dev`  
   - UI: `http://localhost:5173`

3. Open `http://localhost:5173`, log in with a seeded user (see below), and run through the QA checklist.

---

## 2. Expected Seeded Users (E2E / Dev)

When running in **E2E** or **Dev** mode with stub auth, these users exist:

| Email                   | Display Name   | Role     | Use in QA |
|-------------------------|----------------|----------|-----------|
| `vp@example.com`        | VP User        | VP       | VP flows  |
| `manager@example.com`  | Manager User   | Manager  | Manager flows |
| `surveyor@example.com` | Surveyor User  | Surveyor | **Primary for E2E** — sees assigned surveys, checklist, submit |
| `surveyor2@example.com`| Surveyor Two   | Surveyor | Second surveyor; different assignment set |

- **E2E tests** use `surveyor@example.com` (preselected when `VITE_E2E=true`).
- **Surveyor 1** (`surveyor@example.com`) is assigned: Facility Audit – Chicago West, Infection Prevention Q1, Safety Compliance Review, **E2E Survey A**.
- **Surveyor 2** (`surveyor2@example.com`) is assigned: Environmental Survey – Building A, Quarterly Quality Check, Facility Audit – Chicago West (Follow-up).

---

## 3. Seeded Surveys and IDs (E2E)

Deterministic IDs used by E2E:

- **E2E Survey A**  
  - ID: `e2e00001-0000-0000-0000-000000000001`  
  - Assigned to `surveyor@example.com`; used in the **happy path** test.

- **Safety Compliance Review**  
  - ID: `a1000003-0000-0000-0000-000000000003`  
  - Used in the **validation** test (submit without required items).

- **Unassigned survey** (for auth-guard test):  
  - ID: `a1000004-0000-0000-0000-000000000004`  
  - Assigned to Surveyor 2 only; Surveyor 1 gets 403 when opening by URL.

---

## 4. What the E2E Tests Cover

| Test | Description |
|------|-------------|
| **Happy path** | Login as surveyor → list assigned surveys → open **E2E Survey A** → answer required items → wait for “Saved” → refresh → confirm answers persisted → complete all required items → submit → confirm success and submit button hidden. |
| **Validation** | Open **Safety Compliance Review** → submit without filling required items → assert validation banner and missing-required row highlight. |
| **Auth guard** | Login as surveyor → navigate directly to unassigned survey URL → assert 403 / “do not have access” message. |

---

## 5. Interpreting E2E Failures

- **Where to look**
  - Terminal: Playwright prints failing test name and assertion.
  - **HTML report:** If run with `CI=true` (e.g. in CI), report is written to `modern/client/playwright-report/`. Open `index.html` in a browser.
  - On CI failure, the workflow uploads the `playwright-report` artifact; download and open `index.html`.

- **Typical causes**
  - API or frontend not ready: ensure health and UI wait steps succeeded (or increase wait in `run-e2e-local.cjs`).
  - Port in use: ensure 5070 and 5173 are free.
  - Wrong env: E2E mode must use `VITE_E2E=true` and `VITE_API_URL=http://localhost:5070` for the frontend and API on 5070.
  - Flaky timing: tests wait on “Saved” and visibility; if autosave or UI is slow, consider increasing timeouts in `playwright.config.ts` or the spec.

- **Running a single test**  
  From `modern/client`:  
  `npx playwright test -g "happy path"`  
  (or the test title substring).

---

## 6. QA Pass Checklist (Manual)

Use this when doing a manual QA pass (E2E mode, seeded data):

- [ ] **Slice 2 – List**  
  Log in as `surveyor@example.com` → Surveyor sees assigned surveys (e.g. E2E Survey A, Infection Prevention Q1, Safety Compliance Review, Facility Audit – Chicago West).

- [ ] **Slice 3 – Checklist**  
  Open a survey → checklist loads with required/optional items and Pass/Fail/NA and notes.

- [ ] **Autosave**  
  Change an answer → “Saved” (or equivalent) appears; refresh page → re-open survey → answers are still there.

- [ ] **Submit validation**  
  Open a survey → leave at least one required item unanswered → Submit → validation message and highlighted missing items.

- [ ] **Submit success**  
  Complete all required items → Submit → success message and status/UI reflects submitted (e.g. submit button hidden or disabled).

---

## 7. Bug Report Template

When reporting a bug found during QA or E2E:

```markdown
**Environment**
- [ ] Local E2E / [ ] CI
- OS: 
- Browser (if UI): 

**User**
- Seed user (e.g. surveyor@example.com): 
- Survey ID or title (if applicable): 

**Steps**
1. 
2. 
3. 

**Expected**
 

**Actual**
 

**Screenshots / logs**
- (attach or paste Playwright trace, screenshot, or console/network logs)
```

---

## 8. Reports and Artifacts

| Item | Location |
|------|----------|
| Playwright config | `modern/client/playwright.config.ts` |
| E2E specs | `modern/client/tests/e2e/` |
| HTML report (when `CI` set) | `modern/client/playwright-report/` (open `index.html`) |
| CI artifact (on failure) | GitHub Actions (or your CI) → workflow run → Artifacts → `playwright-report` |

---

## 9. CI

- **Workflow:** `.github/workflows/e2e.yml`
- **Triggers:** Push and pull requests to `main` and `develop`
- **Steps:** Restore/build .NET solution → install frontend deps → install Playwright Chromium → start API (E2E, 5070) and frontend (E2E env, 5173) → wait for health and UI → run `playwright test` headless → on failure, upload `playwright-report` artifact.

Stable ports: **API 5070**, **UI 5173**.
