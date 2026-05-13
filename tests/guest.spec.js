// ============================================================
// TC MODULE: GUEST SITE (Trang dành cho khách)
// ============================================================
import { test, expect } from '@playwright/test';

// ===========================================================
test.describe('TC-GUEST-HOME: Trang Chủ & Navigation', () => {

  test('TC-GUEST-001: [Pass] Khách load trang chủ không có lỗi 500', async ({ page }) => {
    await page.goto('/');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error/i);
    const bodyVisible = await page.locator('body').isVisible();
    expect(bodyVisible).toBe(true);
  });

  test('TC-GUEST-002: [Pass] Trang Danh mục/Sản phẩm Guest load thành công', async ({ page }) => {
    await page.goto('/Guest/Product');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error/i);
  });

  test('TC-GUEST-003: [Pass] Trang Combo Guest load thành công', async ({ page }) => {
    await page.goto('/Guest/Combo');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error/i);
  });

  test('TC-GUEST-004: [Pass] Trang chủ Guest hiển thị nội dung (có text)', async ({ page }) => {
    await page.goto('/Guest/Home');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error/i);
    const text = await page.locator('body').innerText();
    // Phải có nội dung text hiển thị (không blank)
    expect(text.trim().length).toBeGreaterThan(50);
  });

  test('TC-GUEST-005: [Fail] Truy cập URL Admin khi là Guest → Bị chặn redirect', async ({ page }) => {
    // Không cần đăng nhập, truy cập thẳng Admin area
    await page.goto('/Admin/Dashboard');
    await page.waitForTimeout(1500);
    // Phải bị redirect, không ở Dashboard của Admin
    await expect(page).not.toHaveURL(/Admin\/Dashboard/i);
  });
});

// ===========================================================
test.describe('TC-GUEST-SEARCH: Tìm kiếm sản phẩm', () => {

  test('TC-GUEST-006: [Pass] Tìm kiếm với keyword "cafe" → Trả về kết quả/trang hợp lệ', async ({ page }) => {
    // SearchController dùng tham số "keyword" chứ không phải "query"
    await page.goto('/Guest/Search?keyword=cafe');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error/i);
    // URL giữ nguyên query param
    expect(page.url()).toContain('keyword=cafe');
  });

  test('TC-GUEST-007: [Pass] Tìm kiếm với keyword trống → Không bị lỗi 500, trả trang trống', async ({ page }) => {
    await page.goto('/Guest/Search?keyword=');
    await page.waitForTimeout(1000);
    // Không được crash server
    await expect(page).not.toHaveURL(/\/Error/i);
  });

  test('TC-GUEST-008: [Fail] Tìm kiếm ký tự đặc biệt → Không crash, hiển thị kết quả trống', async ({ page }) => {
    await page.goto('/Guest/Search?keyword=###@@@$$$');
    await page.waitForTimeout(1000);
    // Không được báo lỗi 500
    await expect(page).not.toHaveURL(/\/Error/i);
    const bodyText = await page.locator('body').innerText();
    // Phải có chữ "không tìm thấy" hoặc kết quả 0, KHÔNG được hiển thị y hệt "###@@@$$$" như là dữ liệu sản phẩm hợp lệ
    // Đây là FAIL case: nếu hiển thị "###@@@$$$" như sản phẩm thì hệ thống có lỗi
    const showsSpecialCharsAsProduct = bodyText.includes('###@@@$$$') && !bodyText.includes('không tìm') && !bodyText.includes('Không tìm');
    expect(showsSpecialCharsAsProduct).toBe(false); // Không được hiển thị ký tự đặc biệt như kết quả sản phẩm
  });

  test('TC-GUEST-009: [Fail] Tìm kiếm với query nguy hiểm (SQL injection style) → Không crash server', async ({ page }) => {
    const maliciousQuery = "' OR '1'='1' --";
    await page.goto(`/Guest/Search?keyword=${encodeURIComponent(maliciousQuery)}`);
    await page.waitForTimeout(1000);
    // Server không được crash với input nguy hiểm
    const statusOk = !page.url().includes('/Error') && !page.url().includes('500');
    expect(statusOk).toBe(true);
  });
});

// ===========================================================
test.describe('TC-GUEST-PRODUCT: Chi tiết sản phẩm & Bình luận', () => {

  test('TC-GUEST-010: [Pass] Load trang chi tiết sản phẩm ID=1 thành công', async ({ page }) => {
    await page.goto('/Guest/Product/Detail/1');
    await page.waitForTimeout(1000);
    // Không bị Error 500
    await expect(page).not.toHaveURL(/\/Error/i);
  });

  test('TC-GUEST-011: [Pass] Chi tiết sản phẩm ID=1 hiển thị thông tin sản phẩm hoặc báo 404 gracefully', async ({ page }) => {
    // page.goto sẽ trả về object response
    const response = await page.goto('/Guest/Product/Detail/1');
    await page.waitForTimeout(1000);
    
    // Nếu status là 404 thì coi như web xử lý graceful NotFound, không bị crash 500
    const status = response ? response.status() : 0;
    const isErrorPage = page.url().includes('Error') || status === 404 || status === 500;
    
    // Yêu cầu là KHÔNG bị lỗi 500
    expect(status).not.toBe(500);

    // Nếu trang load OK (200), phải có nội dung sản phẩm
    if (!isErrorPage && status === 200) {
      const text = await page.locator('body').innerText();
      expect(text.trim().length).toBeGreaterThan(10);
    }
  });

  test('TC-GUEST-012: [Fail] Truy cập sản phẩm không tồn tại (ID=9999999) → Không crash 500', async ({ page }) => {
    await page.goto('/Guest/Product/Detail/9999999');
    await page.waitForTimeout(1000);
    // Hệ thống phải xử lý gracefully: 404 hoặc redirect, KHÔNG phải 500
    const url = page.url();
    const isHardError = url.includes('/500') || url.includes('Exception');
    expect(isHardError).toBe(false);
  });

  test('TC-GUEST-013: [Fail] Gửi bình luận rỗng → Validation chặn hoặc báo lỗi', async ({ page }) => {
    await page.goto('/Guest/Product/Detail/1');
    await page.waitForTimeout(1000);

    const isErrorPage = page.url().includes('Error') || page.url().includes('404');
    if (!isErrorPage) {
      const submitBtns = await page.locator('button:has-text("Gửi"), input[type="submit"][value="Gửi"]').count();
      if (submitBtns > 0) {
        await page.locator('button:has-text("Gửi"), input[type="submit"][value="Gửi"]').first().click();
        await page.waitForTimeout(500);

        const isHtml5Invalid = await page.evaluate(() => {
          const el = document.querySelector('textarea, input[name="Content"]');
          return el && !el.validity.valid;
        });

        if (!isHtml5Invalid) {
          const hasErr = await page.locator(':invalid, .text-danger, .field-validation-error').count();
          expect(hasErr).toBeGreaterThan(0);
        } else {
          expect(isHtml5Invalid).toBe(true);
        }
      }
    }
  });
});
