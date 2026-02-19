/**
 * Starts API (E2E profile), frontend (E2E env), waits for health + UI, runs Playwright, then exits.
 * Usage: from modern/client: node scripts/run-e2e-local.cjs
 * Or: npm run e2e:local
 */
const { spawn } = require('child_process');
const path = require('path');
const http = require('http');

const ROOT = path.resolve(__dirname, '../..'); // modern/
const API_DIR = path.join(ROOT, 'QualitySmartAssistant.Api');
const CLIENT_DIR = path.join(ROOT, 'client');

const API_URL = 'http://localhost:5070';
const UI_URL = 'http://localhost:5173';
const HEALTH_PATH = '/health';
const POLL_MS = 500;
const MAX_WAIT_MS = 60000;

function waitFor(url, pathname = '/') {
  const target = url + pathname;
  const start = Date.now();
  return new Promise((resolve, reject) => {
    function poll() {
      if (Date.now() - start > MAX_WAIT_MS) {
        reject(new Error(`Timeout waiting for ${target}`));
        return;
      }
      const req = http.request(target, { method: 'GET' }, (res) => {
        res.resume();
        if (res.statusCode === 200) return resolve();
        setTimeout(poll, POLL_MS);
      });
      req.on('error', () => setTimeout(poll, POLL_MS));
      req.end();
    }
    poll();
  });
}

function main() {
  const dotnetArgs = process.env.CI
    ? ['run', '--launch-profile', 'E2E', '--no-build', '-c', 'Release']
    : ['run', '--launch-profile', 'E2E'];
  const apiProc = spawn('dotnet', dotnetArgs, { cwd: API_DIR, stdio: 'inherit', shell: true });
  const frontendEnv = {
    ...process.env,
    VITE_E2E: 'true',
    VITE_API_URL: API_URL,
  };
  const frontendProc = spawn('npm', ['run', 'dev'], {
    cwd: CLIENT_DIR,
    env: frontendEnv,
    stdio: 'inherit',
    shell: true,
  });

  function killAll(code) {
    apiProc.kill();
    frontendProc.kill();
    process.exit(code);
  }

  apiProc.on('error', (err) => {
    console.error('Failed to start API:', err);
    killAll(1);
  });
  frontendProc.on('error', (err) => {
    console.error('Failed to start frontend:', err);
    killAll(1);
  });

  Promise.all([
    waitFor(API_URL, HEALTH_PATH),
    waitFor(UI_URL),
  ])
    .then(() => {
      const pwEnv = { ...process.env, PLAYWRIGHT_BASE_URL: UI_URL };
      const pw = spawn('npx', ['playwright', 'test'], {
        cwd: CLIENT_DIR,
        env: pwEnv,
        stdio: 'inherit',
        shell: true,
      });
      pw.on('close', (code) => killAll(code ?? 0));
    })
    .catch((err) => {
      console.error(err.message);
      killAll(1);
    });
}

main();