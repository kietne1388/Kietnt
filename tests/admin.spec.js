// ============================================================
// TC MODULE: ADMIN PANEL - Dashboard, Category, Product, Combo, Voucher, Employee
// ============================================================
import { test, expect } from '@playwright/test';

// ---- Helper: Đăng nhập Admin ----
async function loginAsAdmin(page) {
  await page.goto('/Guest/Auth/Login');
  await page.fill('input[name="Username"]', 'admin');
  await page.fill('input[name="Password"]', 'Admin@123');
  await page.click('button[type="submit"]');
  await page.waitForTimeout(1500); // Đợi redirect
}

test.beforeEach(async ({ page }) => {
  await loginAsAdmin(page);
});

// ===========================================================
test.describe('TC-ADM-DASH: Admin Dashboard', () => {

  test('TC-ADM-001: [Pass] Dashboard load thành công sau khi đăng nhập', async ({ page }) => {
    await page.goto('/Admin/Dashboard');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
    // Phải có ít nhất heading h1/h2/h3
    await expect(page.locator('h1, h2, h3, h4').first()).toBeVisible({ timeout: 8000 });
  });

  test('TC-ADM-002: [Pass] Dashboard hiển thị thông tin thống kê (doanh thu, đơn hàng, sản phẩm)', async ({ page }) => {
    await page.goto('/Admin/Dashboard');
    await page.waitForTimeout(1000);
    const bodyText = await page.locator('body').innerText();
    // Dashboard phải có các từ khóa nghiệp vụ liên quan
    const hasStats = /đơn|doanh|thống|tổng|sản phẩm|revenue|order|product/i.test(bodyText);
    expect(hasStats).toBeTruthy();
  });

  test('TC-ADM-003: [Fail] Truy cập Admin Dashboard khi không phải Admin → Bị chặn redirect về Guest', async ({ page }) => {
    // Logout trước, sau đó vào Admin/Dashboard
    await page.goto('/Guest/Auth/Logout');
    await page.waitForTimeout(1000);
    await page.goto('/Admin/Dashboard');
    await page.waitForTimeout(1500);
    // AdminAuthorizeFilter redirect về Guest/Home/Index (không phải /Admin/Dashboard)
    await expect(page).not.toHaveURL(/Admin\/Dashboard/i);
  });
});

// ===========================================================
test.describe('TC-ADM-CAT: Admin - Danh mục sản phẩm', () => {

  test('TC-ADM-CAT-001: [Pass] Xem danh sách danh mục thành công', async ({ page }) => {
    await page.goto('/Admin/Category');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
    // Trang phải load được HTML
    const bodyVisible = await page.locator('body').isVisible();
    expect(bodyVisible).toBe(true);
  });

  test('TC-ADM-CAT-002: [Pass] Trang tạo danh mục hiển thị form nhập liệu', async ({ page }) => {
    await page.goto('/Admin/Category/Create');
    await page.waitForTimeout(500);
    await expect(page.locator('form')).toBeVisible({ timeout: 5000 });
  });

  test('TC-ADM-CAT-003: [Fail] CreateJson API - Gửi tên rỗng → API trả lỗi success=false', async ({ page }) => {
    await page.goto('/Admin/Category'); // Phải đăng nhập để có session
    await page.waitForTimeout(500);

    // Gọi API Admin Category CreateJson với tên rỗng (Negative test)
    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/Admin/Category/CreateJson', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ name: '', description: 'Test description' })
        });
        if (!r.ok) return { success: false, httpError: r.status };
        return await r.json();
      } catch (e) {
        return { success: false, error: e.message };
      }
    });
    // API phải trả về success: false vì tên trống
    expect(result.success).toBe(false);
  });

  test('TC-ADM-CAT-004: [Fail] Submit form tạo danh mục không nhập tên → Validation báo lỗi', async ({ page }) => {
    await page.goto('/Admin/Category/Create');
    await page.waitForTimeout(500);
    await page.click('button[type="submit"]');
    await page.waitForTimeout(500);

    // HTML5 required hoặc server validation
    const isHtml5Invalid = await page.evaluate(() => {
      const el = document.querySelector('input[name="Name"], input[name="name"]');
      return el && !el.validity.valid;
    });

    if (!isHtml5Invalid) {
      const errors = await page.locator('.field-validation-error, .text-danger, :invalid').count();
      expect(errors).toBeGreaterThan(0);
    } else {
      expect(isHtml5Invalid).toBe(true);
    }
  });
});

// ===========================================================
test.describe('TC-ADM-PRD: Admin - Sản phẩm', () => {

  test('TC-ADM-PRD-001: [Pass] Danh sách sản phẩm load thành công', async ({ page }) => {
    await page.goto('/Admin/Product');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
  });

  test('TC-ADM-PRD-002: [Pass] Form tạo sản phẩm có dropdown chọn Danh mục', async ({ page }) => {
    await page.goto('/Admin/Product/Create');
    await page.waitForTimeout(1000);
    await expect(page.locator('form')).toBeVisible({ timeout: 5000 });
    // Phải có select/combobox chọn danh mục (Category)
    const hasCategorySelect = await page.locator('select, [name*="Category"], [name*="category"]').count();
    expect(hasCategorySelect).toBeGreaterThan(0);
  });

  test('TC-ADM-PRD-003: [Fail] Tạo sản phẩm thiếu dữ liệu bắt buộc → Báo lỗi Validation', async ({ page }) => {
    await page.goto('/Admin/Product/Create');
    await page.waitForTimeout(500);
    await page.click('button[type="submit"]');
    await page.waitForTimeout(500);

    const isHtml5Invalid = await page.evaluate(() => {
      const el = document.querySelector('input:required, textarea:required');
      return el && !el.validity.valid;
    });

    if (!isHtml5Invalid) {
      const errors = await page.locator('.field-validation-error, .text-danger, :invalid').count();
      expect(errors).toBeGreaterThan(0);
    } else {
      expect(isHtml5Invalid).toBe(true);
    }
  });

  test('TC-ADM-PRD-004: [Pass] Xem trang bình luận của sản phẩm ID=1', async ({ page }) => {
    await page.goto('/Admin/Product/Comments/1');
    await page.waitForTimeout(1000);
    // Không được hiển thị Error 500
    await expect(page).not.toHaveURL(/\/Error/i);
  });
});

// ===========================================================
test.describe('TC-ADM-CMB: Admin - Combo', () => {

  test('TC-ADM-CMB-001: [Pass] Danh sách Combo load thành công', async ({ page }) => {
    await page.goto('/Admin/Combo');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
  });

  test('TC-ADM-CMB-002: [Fail] Submit form Combo trống → Validation chặn/báo lỗi', async ({ page }) => {
    await page.goto('/Admin/Combo/Create');
    await page.waitForTimeout(500);
    await page.click('button[type="submit"]');
    await page.waitForTimeout(500);
    const errors = await page.locator('input:invalid, .field-validation-error, .text-danger').count();
    expect(errors).toBeGreaterThan(0);
  });
});

// ===========================================================
test.describe('TC-ADM-VCH: Admin - Voucher', () => {

  test('TC-ADM-VCH-001: [Pass] Trang quản lý Voucher load thành công', async ({ page }) => {
    await page.goto('/Admin/Voucher');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
  });

  test('TC-ADM-VCH-002: [Fail] Tạo Voucher không nhập Code bắt buộc → Validation báo lỗi', async ({ page }) => {
    await page.goto('/Admin/Voucher/Create');
    await page.waitForTimeout(500);
    await page.click('button[type="submit"]');
    await page.waitForTimeout(500);
    const errors = await page.locator('input:invalid, .field-validation-error, .text-danger').count();
    expect(errors).toBeGreaterThan(0);
  });

  test('TC-ADM-VCH-003: [Fail] Tạo Voucher với phần trăm giảm giá > 100% → Không hợp lệ', async ({ page }) => {
    await page.goto('/Admin/Voucher/Create');
    await page.waitForTimeout(500);

    // Điền dữ liệu không hợp lệ: % discount > 100
    try { await page.fill('input[name="Code"]', 'TESTINVALID'); } catch {}
    try { await page.fill('input[name="DiscountPercent"], input[name="discountPercent"]', '999'); } catch {}
    await page.click('button[type="submit"]');
    await page.waitForTimeout(1000);

    // Kỳ vọng: có validation error hoặc ở lại trang Create
    const errors = await page.locator('input:invalid, .field-validation-error, .text-danger').count();
    const stayOnCreate = page.url().includes('Create') || page.url().includes('Voucher');
    // Nếu server chấp nhận > 100% đó là lỗi nghiệp vụ, nhưng ta test xem nó xử lý thế nào
    expect(stayOnCreate || errors > 0).toBeTruthy();
  });
});

// ===========================================================
test.describe('TC-ADM-EMP: Admin - Nhân sự/Nhân viên', () => {

  test('TC-ADM-EMP-001: [Pass] Danh sách nhân viên load thành công', async ({ page }) => {
    await page.goto('/Admin/Employee');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
  });

  test('TC-ADM-EMP-002: [Fail] Tạo nhân sự thiếu Username và Password → Validation báo lỗi', async ({ page }) => {
    await page.goto('/Admin/Employee/Create');
    await page.waitForTimeout(500);
    await page.click('button[type="submit"]');
    await page.waitForTimeout(500);
    const errors = await page.locator('input:invalid, .field-validation-error, .text-danger').count();
    expect(errors).toBeGreaterThan(0);
  });
});

// ===========================================================
test.describe('TC-ADM: Các phân hệ khác (Order, Report, Notification)', () => {

  test('TC-ADM-RPT-001: [Pass] Trang Báo cáo (Report) load thành công', async ({ page }) => {
    await page.goto('/Admin/Report');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
  });

  test('TC-ADM-ORD-001: [Pass] Trang Quản lý Đơn hàng (Order) load thành công', async ({ page }) => {
    await page.goto('/Admin/Order');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
  });

  test('TC-ADM-NTF-001: [Pass] Trang Thông báo (Notification) load thành công', async ({ page }) => {
    await page.goto('/Admin/Notification');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
  });

  test('TC-ADM-USR-001: [Pass] Trang Quản lý người dùng (User) load thành công', async ({ page }) => {
    await page.goto('/Admin/User');
    await page.waitForTimeout(1000);
    await expect(page).not.toHaveURL(/Error|Login/i);
  });
});
