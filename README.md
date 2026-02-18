Quality Smart Assistant
=======================

Quality Management platform focused on surveillance, assessments, and completion analytics.

## Brand Intent
- Product name: Quality Smart Assistant
- Product focus: Quality Management
- Outcome focus: improve assessment quality, completion rates, and operational visibility

## Modern MVP Artifacts
- API contract: `openapi.yaml`
- Minimal API scaffold: `modern/QualitySmartAssistant.Api`

## Immediate MVP Scope
- Identity context: `GET /auth/me`
- Survey lifecycle: `POST /surveys`, `POST /surveys/{surveyId}/publish`
- Assignment lifecycle: `POST /assignments`
- Response capture: `POST /responses`
- Reporting: `GET /reports/completion`
