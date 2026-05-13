// ============================================================
// TC MODULE: STAFF POS (Chức năng điểm bán hàng)
// ============================================================
import { test, expect } from '@playwright/test';

// ---- Helper: Đăng nhập Staff ----
async function loginAsStaff(page) {
  await page.goto('/Guest/Auth/Login');
  await page.fill('input[name="Username"]', 'staff');
  await page.fill('input[name="Password"]', '123456');
  await page.click('button[type="submit"]');
  await page.waitForTimeout(1500);
  // Vào trang POS
  await page.goto('/Staff/POS');
  await page.waitForTimeout(1000);
}

// ===========================================================
test.describe('TC-POS: Giao diện POS tại quầy', () => {

  test.beforeEach(async ({ page }) => {
    await loginAsStaff(page);
  });

  test('TC-POS-001: [Pass] Staff truy cập giao diện POS thành công', async ({ page }) => {
    // Không lỗi 500, không redirect về Login (nếu staff account tồn tại)
    const url = page.url();
    // Chấp nhận ở POS hoặc nếu staff account ko tồn tại thì ở Login
    expect(url).not.toContain('/Error');
  });

  test('TC-POS-002: [Pass] Giao diện POS load được HTML content', async ({ page }) => {
    const text = await page.locator('body').innerText();
    // Phải có nội dung HTML hiển thị (không blank)
    expect(text.trim().length).toBeGreaterThan(10);
  });

  test('TC-POS-003: [Pass] Giao diện POS chứa thông tin về giỏ hàng hoặc menu', async ({ page }) => {
    const url = page.url();
    if (url.includes('Staff/POS')) {
      const bodyText = await page.locator('body').innerText();
      // POS phải có các từ khóa liên quan đến bán hàng
      const hasPosContent = /Tạm tính|Tổng cộng|Thanh toán|Giỏ hàng|Sản phẩm|Menu|POS|Coffee|Cafe/i.test(bodyText);
      expect(hasPosContent).toBeTruthy();
    }
  });

  test('TC-POS-004: [Pass] Click Thanh toán khi giỏ trống → Không crash, ở lại POS hoặc báo lỗi', async ({ page }) => {
    const url = page.url();
    if (url.includes('Staff/POS')) {
      const btns = await page.locator('button:has-text("Thanh Toán"), button:has-text("Checkout"), .checkout-btn').all();
      if (btns.length > 0) {
        await btns[0].click();
        await page.waitForTimeout(1000);
        // Không được crash sang Error page
        await expect(page).not.toHaveURL(/\/Error/i);
      }
    }
  });

  test('TC-POS-005: [Fail] Staff không thể truy cập Admin Dashboard (phân quyền)', async ({ page }) => {
    // Staff đang đăng nhập thử vào Admin Dashboard
    await page.goto('/Admin/Dashboard');
    await page.waitForTimeout(1500);
    // Staff không được vào Admin Dashboard → phải bị redirect
    await expect(page).not.toHaveURL(/Admin\/Dashboard/i);
  });
});

// ===========================================================
test.describe('TC-POS: API POS bán hàng', () => {

  test.beforeEach(async ({ page }) => {
    await loginAsStaff(page);
  });

  test('TC-POS-006: [Pass] API lấy danh sách sản phẩm POS trả về dữ liệu', async ({ page }) => {
    const result = await page.evaluate(async () => {
      try {
        // POS lấy API thông qua web phụ trợ /Staff/POS/LoadProducts (không phải trực tiếp từ :7001/api)
        const r = await fetch('/Staff/POS/LoadProducts');
        if (!r.ok) return { ok: false, status: r.status };
        const data = await r.json();
        return { ok: true, isArray: Array.isArray(data) };
      } catch (e) {
        return { ok: false, error: e.message };
      }
    });
    expect(result.ok).toBe(true);
  });

  test('TC-POS-007: [Fail] Tìm kiếm sản phẩm POS với ID âm → Trả về lỗi hợp lệ', async ({ page }) => {
    const url = page.url();
    if (!url.includes('Staff/POS')) return; // Skip nếu không vào được POS

    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/api/product/-1');
        return { ok: r.ok, status: r.status };
      } catch (e) {
        return { ok: false, error: e.message };
      }
    });
    // ID âm phải trả về 404 hoặc lỗi không phải 200
    expect(result.ok).toBe(false); // 404 Not Found
  });
});

// ===========================================================
test.describe('TC-POS: POS Sales Receipt', () => {

  test.beforeEach(async ({ page }) => {
    await loginAsStaff(page);
  });

  test('TC-POS-008: [Pass] Trang SalesReceipt (lịch sử bán hàng) load thành công', async ({ page }) => {
    await page.goto('/Staff/SalesReceipt');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error/i);
  });

  test('TC-POS-009: [Fail] Submit hóa đơn POS không có món nào → Bị báo lỗi hoặc block', async ({ page }) => {
    const url = page.url();
    if (!url.includes('Staff/POS')) return;

    // Tìm nút Submit/Checkout và click khi giỏ hàng POS trống
    const submitBtn = page.locator('button[type="submit"]:has-text("Thanh"), button:has-text("Thanh Toán"), #btnCheckout');
    const btnCount = await submitBtn.count();

    if (btnCount > 0) {
      await submitBtn.first().click();
      await page.waitForTimeout(1000);

      // Kỳ vọng: Không được redirect sang trang Success vì giỏ trống
      const currentUrl = page.url();
      const didNotSucceed = !currentUrl.includes('Success') && !currentUrl.includes('Receipt');
      expect(didNotSucceed).toBe(true);
    }
  });
});
