# Run E2E: start API (E2E) + frontend (E2E env), wait for health, run Playwright.
# Usage: from repo root or modern: ./modern/scripts/run-e2e.ps1  OR  from modern: ./scripts/run-e2e.ps1
$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -LiteralPath $MyInvocation.MyCommand.Path
$modernRoot = Split-Path -LiteralPath $scriptDir
$clientDir = Join-Path $modernRoot "client"
Push-Location $clientDir
try {
    node scripts/run-e2e-local.cjs
} finally {
    Pop-Location
}
