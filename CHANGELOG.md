# Changelog

## [Unreleased]

### Added — Vertical Slice 1: Auth & Role-Based Navigation

- **Backend (.NET)**
  - Clean Architecture under `modern/`: `Qsa.Domain`, `Qsa.Application`, `Qsa.Infrastructure`, `QualitySmartAssistant.Api`.
  - Domain: `Role` enum (VP, Manager, Surveyor), `User` (Id, Email, DisplayName, Role).
  - Application: `AuthenticateDevUserCommand` / `GetCurrentUserQuery`, handlers, `IUserStore`, `ITokenService`, `IUserContext`, `UserDto` / `AuthResult`.
  - Infrastructure: `InMemoryUserStore` (seeded vp@example.com, manager@example.com, surveyor@example.com), `JwtTokenService`, `UserContext` (from HTTP claims), JWT Bearer auth and policies VPOnly / ManagerOnly / SurveyorOnly.
  - API: `POST /auth/dev-login`, `GET /me`, `GET /auth/me` (backward compat), `GET /auth/health`, `GET /vp/ping`, `GET /manager/ping`, `GET /surveyor/ping` (policy-protected). CORS for local dev.
  - Config: `appsettings.Development.json` — `Auth:UseDevAuth`, `Jwt:Issuer`, `Audience`, `SigningKey`.
- **Frontend (React)**
  - Vite + React + TypeScript app in `modern/client`.
  - Auth API client: `devLogin`, `getMe`, role ping helpers; token in `localStorage`; on load, `GET /me` restores session or clears it.
  - `AuthContext` for login/logout and current user; role-based routing with `ProtectedRoute` (VP / Manager / Surveyor).
  - Login page (dev): dropdown for seeded users, optional role override; redirect to role home after login.
  - App shell: top bar with user display name, role badge, logout; left nav by role (Exec Dashboard / Management Dashboard / My Surveys).
  - Placeholder role homes: VpHomePage, ManagerHomePage, SurveyorHomePage; each calls its `/ping` endpoint to verify auth.
  - Vite proxy: `/api` → backend (e.g. http://localhost:5251).
- **Tests**
  - `Qsa.Application.Tests`: unit tests for `AuthenticateDevUserCommandHandler` (success, user not found, role override).
- **Docs / Tooling**
  - `.github/copilot-instructions.md`: Clean Architecture boundaries, vertical slice delivery, use-case naming, auth/roles, Definition of Done for a slice.
  - README: run steps for `dotnet run` (API) and `npm run dev` (client).

### Added — Vertical Slice 2: Surveyor Work Queue (Assigned Surveys)

- **Backend (.NET)**
  - Domain: `Survey` (Id, Title, LocationName, DueDate, Status, Priority), `SurveyAssignment` (SurveyId, SurveyorUserId, AssignedAt), `SurveyStatus` enum (NotStarted, InProgress, Submitted, Completed), `SurveyPriority` enum (Low, Medium, High).
  - Application: `ISurveyRepository` (ListAssignedSurveysAsync, GetSurveyByIdAsync, IsSurveyAssignedToUserAsync, GetAssignedAtAsync), `ListAssignedSurveysQuery` / `GetSurveyQuery` with handlers; `AssignedSurveyDto`, `SurveyDetailDto`. Authorization in Application: Surveyor role only; surveyor sees only their assigned surveys.
  - Infrastructure: `InMemorySurveyRepository` with 6 seeded surveys across 2 surveyors (usr_svy_01, usr_svy_02); second surveyor user `surveyor2@example.com` added to `InMemoryUserStore`.
  - API: `GET /surveys/assigned` (SurveyorOnly), `GET /surveys/{id}` (SurveyorOnly, 404 if not found, 403 if not assigned). Endpoints call Application handlers; map `UnauthorizedAccessException` to 403.
- **Frontend (React)**
  - `src/api/surveys.ts`: `getAssignedSurveys(token)`, `getSurveyById(token, id)`; handle 401/403/404.
  - Surveyor work queue: `SurveyorSurveysPage` at `/surveyor` — table (Title, Location, Due Date, Status, Priority), default sort by due date then priority; row click → `/surveyor/surveys/:id`.
  - `SurveyDetailPage` at `/surveyor/surveys/:id` — fields from SurveyDetailDto; placeholders “Checklist” and “Findings” (Coming in Slice 3/4). 401 forces logout; 403 shows “You do not have access”.
  - Login: `surveyor2@example.com` added to seeded users dropdown.
- **Tests**
  - `ListAssignedSurveysQueryHandlerTests`: returns only current surveyor’s surveys; non-Surveyor throws.
  - `GetSurveyQueryHandlerTests`: 404 when survey unknown; 403 when not assigned; success returns SurveyDetailDto.
- **Docs**
  - `.github/copilot-instructions.md`: Slice 2 Definition of Done, layering reminders, repository/DTO naming conventions.

### Added — Vertical Slice 3: Complete Survey Checklist (Autosave + Submit)

- **Docs**
  - `docs/vertical-slices/VS3-legacy-observations.md`: Legacy recon (useful assets, conflicts with mental model, mapping decisions). Principle: legacy guides, does not restrict.
- **Backend (.NET)**
  - Domain: `ChecklistResponseValue` (Pass, Fail, NA), `ChecklistItem`, `SurveyChecklist`, `SurveyResponse`; `Survey.Status` made settable for submit.
  - Application: `ISurveyAssignmentAuthorizer`, `IChecklistProvider`, `ISurveyResponseStore`, `ISurveyLifecycle`; `GetSurveyChecklistQuery`, `UpsertChecklistResponseCommand`, `SubmitSurveyCommand` with handlers; `ChecklistValidationException` (missingRequiredItemIds); DTOs for checklist, responses, submit result, validation error.
  - Infrastructure: `SurveyAssignmentAuthorizer`, `InMemoryChecklistProvider` (12 items per survey, deterministic Guids), `InMemorySurveyResponseStore`, `InMemorySurveyLifecycle`; `ISurveyRepository.SetSurveyStatusAsync`.
  - API: `GET /surveys/{id}/checklist`, `PUT /surveys/{id}/responses/{itemId}`, `POST /surveys/{id}/submit`; 400 with `missingRequiredItemIds` on submit validation failure.
- **Frontend (React)**
  - Survey Detail: Checklist section with items (Pass/Fail/NA + notes), debounced autosave (600ms), save state (Saving/Saved/Error), Submit button (disabled while unsaved); 400 handling with highlight of missing required items; on success, confirmation and read-only.
  - API client: `getSurveyChecklist`, `putChecklistResponse`, `submitSurvey`; `ValidationErrorDto` for 400 body.
- **Tests**
  - `SubmitSurveyCommandHandlerTests`: throws `ChecklistValidationException` with missing ids when required unanswered; returns SubmitResult when all required answered.
  - `UpsertChecklistResponseCommandHandlerTests`: 403 when not assigned; upserts and returns ResponseSavedDto when assigned.
- **Instructions**
  - `.github/copilot-instructions.md`: “Legacy guides not restricts”; use-case contracts reflect mental model; no legacy types in Application/Domain; Slice 3 Definition of Done.
