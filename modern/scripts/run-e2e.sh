#!/usr/bin/env bash
# Run E2E: start API (E2E) + frontend (E2E env), wait for health, run Playwright.
# Usage: from repo root or modern: ./modern/scripts/run-e2e.sh  OR  from modern: ./scripts/run-e2e.sh
set -e
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
MODERN_ROOT="$(dirname "$SCRIPT_DIR")"
CLIENT_DIR="$MODERN_ROOT/client"
cd "$CLIENT_DIR"
node scripts/run-e2e-local.cjs
