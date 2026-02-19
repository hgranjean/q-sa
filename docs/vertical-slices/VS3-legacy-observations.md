# Vertical Slice 3 — Legacy Observations (Guidance Only)

**Principle:** Legacy code is *descriptive*, not *prescriptive*. Use it to discover terminology and structure; do not let it restrict the target mental model (Surveyor completes checklist with Pass/Fail/NA, autosave, submit).

---

## Useful legacy assets to reuse

| Asset | Location | Use |
|-------|----------|-----|
| **Survey / QuestionGroup / Question** concepts | `Atum.Domain.Surveillance/Domain/SurveyManagement/` (Survey.cs, QuestionGroup.cs, Question.cs) | Terminology: survey has groups and questions; questions have text, rank/order. |
| **Question types** | QuestionType.cs (YesNo, TrueFalse, SelectOne, OpenText, etc.) | Suggests we may later support multiple response types; for VS3 we standardize on Pass/Fail/NA. |
| **Response** (question + answer) | Atum.Domain.Surveillance Response.cs (QuestionId, ResponseChoiceId, AnswerKey, Text) | Idea of “response per question” and optional text (notes). |
| **SurveyManager.AcceptResponse** | SurveyManager.cs | Pattern: accept a response for a question. |
| **openapi.yaml** | Repo root | CreateSurveyRequest has QuestionGroupInput / QuestionInput (code, text, type, isRequired, sortOrder)—aligns with checklist items having text, required, sortOrder. |
| **Modern Qsa.Domain.Surveys** | modern/Src/Qsa.Domain/Surveys/ | Survey (Id, Title, DueDate, Status, Priority), SurveyStatus, SurveyAssignment. Reuse; extend with checklist/response where needed. |

---

## Conflicts with mental model

| Legacy behavior | Mental-model target | Resolution |
|-----------------|---------------------|------------|
| Response is **choice-based** (ResponseChoice, AnswerKey) | Response is **Pass / Fail / NA** + optional notes | Define `ChecklistResponseValue` (Pass, Fail, NA) and `SurveyResponse` (ItemId, Value, Notes, UpdatedAt) in **new** Domain. No legacy Response type in Application/Domain. |
| No explicit **required** on questions | Required items must be answered before submit | Add `IsRequired` on checklist items; validate in Submit use case and return `missingRequiredItemIds` on 400. |
| Legacy uses **int** ids (Question.Id, ResponseChoiceId) | Stable **Guid** for items and responses | Use Guid for ChecklistItem.Id and SurveyResponse keying (surveyId, userId, itemId). |
| Survey “submit” not clearly modeled in legacy Surveillance | Submit sets status to Submitted and locks editing | Add `SurveyLifecycle` / status transition in Application; Infrastructure updates status and optionally timestamps. |
| QuestionGroup / Question hierarchy | Flat checklist items (id, text, required, sortOrder) | Expose a **flat** list of items for the UI; grouping can be a future slice. |

---

## Mapping decisions (legacy → new)

- **Checklist items:** New Domain `ChecklistItem` (Id, Text, IsRequired, SortOrder). Not mapped from legacy Question; seeded in Infrastructure with deterministic Guids. If we later integrate legacy surveys, an **adapter** in Infrastructure will map legacy Question → ChecklistItem (and optionally group by QuestionGroup).
- **Responses:** New Domain `SurveyResponse` (SurveyId, SurveyorUserId, ItemId, Value [Pass|Fail|NA], Notes?, UpdatedAt). No use of legacy Response or ResponseChoice in Application/Domain. Storage key: (surveyId, userId, itemId).
- **Survey status:** Already have `SurveyStatus` (NotStarted, InProgress, Submitted, Completed) in modern Domain. Use it; submit sets status to Submitted.
- **Endpoints:** New routes `GET /surveys/{id}/checklist`, `PUT /surveys/{id}/responses/{itemId}`, `POST /surveys/{id}/submit`. Do **not** reuse legacy SurveyController or response payloads; keep API contract aligned with mental model (checklist items, Pass/Fail/NA, missingRequiredItemIds).
- **Authorization:** Reuse existing pattern: Surveyor + assignment check (EnsureUserAssignedAsync). No legacy auth in Application.

---

## Future migration path (if integrating legacy)

- **Option A:** Build an adapter in Infrastructure that implements `IChecklistProvider` by loading legacy Survey/QuestionGroups/Questions and mapping to Domain ChecklistItem (and optionally `SurveyChecklist`). Map legacy Response rows to `SurveyResponse` (e.g. map Yes→Pass, No→Fail, N/A→NA if present).
- **Option B:** Keep in-memory implementation for dev/demo; add a second Infrastructure implementation (e.g. `LegacyChecklistProvider`) behind the same interface when ready to migrate.
- **TODO (post-VS3):** Document in this file the exact legacy table/entity names and field mapping once a migration is planned.
