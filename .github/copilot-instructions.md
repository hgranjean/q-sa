# Copilot / Coding Agent Instructions — Quality Smart Assistant (QSA)

This repo is being refactored toward **Clean Architecture**. Follow these boundaries and patterns so changes stay consistent and maintainable.

---

## 1. Clean Architecture Boundaries

- **Domain** (`modern/Src/Qsa.Domain`): Pure domain model. **No references** to other projects, no HTTP, no EF, no infrastructure. Only entities, value objects, and domain enums (e.g. `Role`, `User`).
- **Application** (`modern/Src/Qsa.Application`): Use cases (commands/queries), DTOs, and interfaces. References **Domain only**. No `HttpContext`, no EF, no controllers. Handlers depend on abstractions (e.g. `IUserStore`, `ITokenService`, `IUserContext`).
- **Infrastructure** (`modern/Src/Qsa.Infrastructure`): Implements Application interfaces (e.g. `InMemoryUserStore`, `JwtTokenService`, `UserContext`). Contains adapters for auth, persistence, external services. References Application + Domain.
- **WebApi** (`modern/QualitySmartAssistant.Api`): Endpoints only (minimal APIs or controllers). Registers services, configures auth/CORS, maps HTTP to application commands/queries. References **Application + Infrastructure** (not Domain directly for new code; use Application DTOs).

**Rule:** Outer layers depend inward. Domain must have zero dependencies on other layers. Do not leak `HttpContext` or EF into Application; use `IUserContext` and repository interfaces.

---

## 2. Vertical Slice Delivery

Deliver features as **vertical slices** (UI → API → Application → Domain → Infrastructure):

- For each slice: implement the **full flow** in one PR: React UI + API endpoints + Application handler(s) + minimal domain changes (if any) + infrastructure adapter(s) where needed.
- Prefer **adapting existing code** over rewriting. Avoid big-bang refactors.
- Keep slices **minimal and incremental**; only add what the slice requires.

---

## 3. Use-Case Naming and Structure

- **Commands:** `VerbNounCommand` (e.g. `AuthenticateDevUserCommand`) + `VerbNounCommandHandler`.
- **Queries:** `GetXQuery` (e.g. `GetCurrentUserQuery`) + `GetXQueryHandler`.
- **DTOs** live in **Application**, not Domain. Domain holds entities and value types only.
- Handlers are registered in DI and invoked from the API layer (or a thin mediator if one is introduced later).

---

## 4. Auth and Roles

- **Roles** are a **Domain enum** (e.g. `Qsa.Domain.Identity.Role`: VP, Manager, Surveyor).
- **Claims mapping** (JWT claims, cookie claims) belongs in **Infrastructure/WebApi**, never in Domain.
- Do not pass `HttpContext` into Application; use the **`IUserContext`** abstraction so the current user can be resolved from claims in Infrastructure.

---

## 5. Prefer Modifying Existing Code

- Prefer **modifying existing code** over creating parallel implementations or duplicate layers.
- If you touch code that violates boundaries (e.g. Domain depending on EF), fix only what the current slice touches; do not refactor unrelated areas in the same PR unless required.

---

## 6. Definition of Done for a Slice

Before considering a slice complete, ensure:

- [ ] **`GET /me`** works (authenticated user returns current user DTO).
- [ ] **Role-based route guards** work in the React app (VP/Manager/Surveyor see the right routes and land on the right home).
- [ ] **Seeded dev users** work (e.g. `vp@example.com`, `manager@example.com`, `surveyor@example.com` with stub auth).
- [ ] **At least one policy-protected endpoint per role** is present and verified (e.g. `/vp/ping`, `/manager/ping`, `/surveyor/ping`).
- [ ] **Minimal tests** pass (e.g. unit test for the main command/query handler).
- [ ] **README** (or relevant doc) is updated with run steps for API and client (`dotnet run`, `npm run dev` / `pnpm dev`).

---

## 7. Modern Stack Layout (Reference)

- **Backend:** `modern/QualitySmartAssistant.sln` — `Qsa.Domain`, `Qsa.Application`, `Qsa.Infrastructure`, `QualitySmartAssistant.Api`, `Qsa.Application.Tests`.
- **Frontend:** `modern/client` — Vite + React + TypeScript, React Router v6, auth via `AuthContext` and token in `localStorage`. API base URL via proxy (`/api` → backend) or `VITE_API_URL`.
- **API base:** Development: backend at `http://localhost:5251`, frontend at `http://localhost:5173` with proxy to `/api` or CORS to backend.

---

## 8. Vertical Slice 2 — Surveyor Work Queue (Assigned Surveys)

### Slice 2 Definition of Done

- [ ] **`GET /surveys/assigned`** returns only the current surveyor’s assigned surveys (filtered by `IUserContext.UserId`).
- [ ] **`GET /surveys/{id}`** enforces assignment: 404 if survey not found, 403 if survey exists but is not assigned to the current surveyor.
- [ ] **React** shows the work queue table (title, location, due date, status, priority) and a detail page for a survey; row click navigates to detail.
- [ ] **Seeded data** covers at least two surveyors with distinct assigned surveys (e.g. `surveyor@example.com`, `surveyor2@example.com`).
- [ ] **Tests** cover: list returns only current surveyor’s surveys; GetSurvey returns 403 when not assigned; GetSurvey returns 404 when id unknown.

### Layering (Slice 2)

- **WebApi** uses Application only: call query handlers; map `UnauthorizedAccessException` to 403, null to 404. No direct repository access in endpoints.
- **Application** uses `IUserContext` (UserId, Role) and repository interfaces (e.g. `ISurveyRepository`). Authorization rules live here (e.g. “Surveyor only”, “only surveys assigned to current user”).
- **Infrastructure** implements repositories (e.g. `InMemorySurveyRepository`) and holds seed data. Repository returns Domain entities; Application maps to DTOs.

### Naming Conventions (Slice 2)

- **Queries:** `ListAssignedSurveysQuery`, `GetSurveyQuery` (and handlers with same name + `Handler`).
- **DTOs:** `AssignedSurveyDto`, `SurveyDetailDto` (suffix `Dto`; live in Application).
- **Domain:** Enums for status/priority (e.g. `SurveyStatus`, `SurveyPriority`); entities `Survey`, `SurveyAssignment` in Domain, no DTOs or HTTP concerns.
- **Repository interface:** `ISurveyRepository` in Application/Common/Interfaces; methods like `ListAssignedSurveysAsync(surveyorUserId)`, `GetSurveyByIdAsync(id)`, `IsSurveyAssignedToUserAsync(surveyId, userId)`.

---

## 9. Legacy Guides, Does Not Restrict

- **Legacy code is descriptive, not prescriptive.** Use legacy to discover terminology, structures, and useful logic. Do **not** let legacy limitations dictate target behavior.
- **Use-case contracts reflect the mental model.** Application interfaces and DTOs define the intended behavior (e.g. checklist with Pass/Fail/NA, required items, submit validation). Legacy gets **adapted** to these contracts in Infrastructure (adapters/mapping), not the other way around.
- **No legacy types in Application or Domain.** Domain and Application use only types defined in the modern stack. Infrastructure may reference legacy assemblies only to implement Application interfaces and map to/from Domain.
- When legacy conflicts with the mental model, implement the mental model and document mapping decisions (e.g. in `docs/vertical-slices/VS3-legacy-observations.md`).

---

## 10. Vertical Slice 3 — Complete Survey Checklist (Autosave + Submit)

### Slice 3 Definition of Done

- [ ] **GET /surveys/{id}/checklist** returns items (id, text, isRequired, sortOrder), current user’s responses (value, notes, updatedAt), and survey status.
- [ ] **PUT /surveys/{id}/responses/{itemId}** (body: value Pass|Fail|NA, notes optional) upserts response; 403 if not assigned; 400 if survey already submitted or invalid value.
- [ ] **POST /surveys/{id}/submit** validates all required items answered; on failure returns **400** with `{ code, message, missingRequiredItemIds: [] }`; on success returns `{ surveyId, status: "Submitted", submittedAt }`.
- [ ] **Assignment authorization** enforced for all three endpoints (Surveyor + assigned to user); 403 when not assigned.
- [ ] **React:** Checklist section on Survey Detail with Pass/Fail/NA and notes; debounced autosave with visible save state; Submit with client-side pre-check and server 400 handling (highlight missing required items); on success, show confirmation and lock editing.
- [ ] **Tests:** Submit fails when required unanswered (assert `missingRequiredItemIds`); Submit succeeds when required answered; Upsert 403 when not assigned; Upsert persists (handler + store behavior or integration).
