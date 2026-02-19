import { test, expect } from '@playwright/test';

const E2E_SURVEY_ID = 'e2e00001-0000-0000-0000-000000000001';
const SURVEY_FOR_VALIDATION_ID = 'a1000003-0000-0000-0000-000000000003';
const UNASSIGNED_SURVEY_ID = 'a1000004-0000-0000-0000-000000000004';

const itemId = (prefix: string, n: number) => `${prefix}-0000-0000-0000-${String(n).padStart(12, '0')}`;

test.describe('Surveyor workflow', () => {
  test('happy path: login, list surveys, open E2E survey, complete checklist, autosave persists, submit', async ({ page }) => {
    await page.goto('/login');
    await expect(page.getByTestId('login-page')).toBeVisible();
    await expect(page.getByTestId('login-user-select')).toHaveValue('surveyor@example.com');
    await page.getByTestId('login-submit').click();
    await expect(page.getByTestId('surveyor-surveys-page')).toBeVisible({ timeout: 10000 });

    await expect(page.getByTestId('surveys-table')).toBeVisible();
    const row = page.getByTestId(`survey-row-${E2E_SURVEY_ID}`);
    await expect(row).toBeVisible();
    await row.click();

    await expect(page.getByTestId('survey-detail-page')).toBeVisible({ timeout: 10000 });
    await expect(page.getByTestId('checklist-section')).toBeVisible();

    const prefix = E2E_SURVEY_ID.slice(0, 8);
    await page.getByTestId(`checklist-item-${itemId(prefix, 1)}-value-Pass`).click();
    await page.getByTestId(`checklist-item-${itemId(prefix, 2)}-value-Pass`).click();

    await expect(page.getByTestId('checklist-save-status')).toContainText('Saved', { timeout: 15000 });
    await page.waitForTimeout(1600);
    await expect(page.getByTestId('submit-survey-btn')).toBeEnabled();

    await page.reload();
    await expect(page.getByTestId('surveyor-surveys-page')).toBeVisible({ timeout: 10000 });
    await page.getByTestId(`survey-row-${E2E_SURVEY_ID}`).click();
    await expect(page.getByTestId('survey-detail-page')).toBeVisible({ timeout: 10000 });
    await expect(page.getByTestId(`checklist-item-${itemId(prefix, 1)}-value-Pass`)).toBeChecked();
    await expect(page.getByTestId(`checklist-item-${itemId(prefix, 2)}-value-Pass`)).toBeChecked();

    const requiredNums = [1, 2, 3, 4, 5, 6, 7, 10, 11, 12];
    for (const n of requiredNums) {
      await page.getByTestId(`checklist-item-${itemId(prefix, n)}-value-Pass`).first().click();
    }
    await page.waitForTimeout(2000);
    await page.getByTestId('submit-survey-btn').click();
    await expect(page.getByTestId('submit-success')).toBeVisible({ timeout: 10000 });
    await expect(page.getByTestId('submit-survey-btn')).not.toBeVisible();
  });

  test('validation: submit with missing required items shows error and highlights', async ({ page }) => {
    await page.goto('/login');
    await page.getByTestId('login-submit').click();
    await expect(page.getByTestId('surveyor-surveys-page')).toBeVisible({ timeout: 10000 });
    await page.getByTestId(`survey-row-${SURVEY_FOR_VALIDATION_ID}`).click();
    await expect(page.getByTestId('survey-detail-page')).toBeVisible({ timeout: 10000 });

    await page.getByTestId('submit-survey-btn').click();
    await expect(page.getByTestId('validation-banner')).toBeVisible({ timeout: 5000 });
    await expect(page.getByTestId('validation-banner')).toContainText('required');
    const missingRow = page.locator('.checklist-row.missing-required').first();
    await expect(missingRow).toBeVisible();
  });

  test('authorization: direct URL to unassigned survey shows forbidden', async ({ page }) => {
    await page.goto('/login');
    await page.getByTestId('login-submit').click();
    await expect(page.getByTestId('surveyor-surveys-page')).toBeVisible({ timeout: 10000 });

    await page.goto(`/surveyor/surveys/${UNASSIGNED_SURVEY_ID}`);
    await expect(page.getByTestId('survey-detail-error')).toBeVisible({ timeout: 10000 });
    await expect(page.getByTestId('validation-banner')).toContainText('do not have access');
  });
});
