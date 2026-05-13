// @ts-check
import { defineConfig, devices } from '@playwright/test';

/**
 * CaféLux Website - Playwright Test Configuration
 * Website: https://localhost:7129 (MVC) | API: https://localhost:7001
 */
export default defineConfig({
  testDir: './tests',
  fullyParallel: false,          // Chạy tuần tự để tránh conflict session
  forbidOnly: !!process.env.CI,
  retries: 0,                    // Thử lại 1 lần nếu fail
  workers: 4,                    // 1 worker để đảm bảo thứ tự test
  timeout: 5000,                // 30 giây timeout mỗi test
  reporter: [
    ['html', { open: 'never' }], 
    ['list'],
    ['json', { outputFile: 'test-results.json' }]
  ],

  use: {
    baseURL: 'https://localhost:7129',
    ignoreHTTPSErrors: true,     // Bỏ qua lỗi SSL localhost
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    trace: 'on-first-retry',
    actionTimeout: 10000,
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
});

