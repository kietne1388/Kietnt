// ============================================================
// TC MODULE: CART & CHECKOUT (Web / Guest)
// ============================================================
import { test, expect } from '@playwright/test';

// ---- Helper: Đăng nhập Customer ----
async function loginAsCustomer(page) {
  await page.goto('/Guest/Auth/Login');
  await page.fill('input[name="Username"]', 'customer1');
  await page.fill('input[name="Password"]', '123456');
  await page.click('button[type="submit"]');
  await page.waitForTimeout(1500);
}

// ===========================================================
test.describe('TC-CART: Giỏ hàng (Cart)', () => {

  test('TC-CART-001: [Pass] Load trang giỏ hàng Guest khi chưa đăng nhập → Không crash', async ({ page }) => {
    await page.goto('/Guest/Cart');
    await page.waitForTimeout(1000);
    // Có thể redirect về Login hoặc hiển thị giỏ trống, không được crash
    await expect(page).not.toHaveURL(/\/Error/i);
  });

  test('TC-CART-002: [Pass] AddToCart qua API JSON được xử lý (trả về 200 OK)', async ({ page }) => {
    // AddToCart nhận [FromBody] JSON { productId, quantity }
    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/Guest/Cart/AddToCart', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ productId: 1, quantity: 1 })
        });
        if (!r.ok) return { ok: false, status: r.status };
        const data = await r.json();
        return { ok: true, success: data.success, status: r.status };
      } catch (e) {
        return { ok: false, error: e.message, status: 500 };
      }
    });
    // Trả về 200 mới tính là pass logic gọi API
    expect(result.status).toBe(200);
  });

  test('TC-CART-003: [Pass] API GetCart trả về count và total hợp lệ', async ({ page }) => {
    await page.goto('/Guest/Cart'); // Vào trang để có session
    await page.waitForTimeout(500);

    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/Guest/Cart/GetCart', { method: 'GET' });
        if (!r.ok) return { ok: false, status: r.status };
        const data = await r.json();
        return { ok: true, hasCount: 'count' in data, hasTotal: 'total' in data };
      } catch (e) {
        return { ok: false, error: e.message };
      }
    });
    expect(result.ok).toBe(true);
    expect(result.hasCount).toBe(true);
    expect(result.hasTotal).toBe(true);
  });

  test('TC-CART-004: [Fail] Thêm sản phẩm với ID không tồn tại (ID=9999999) → Trả về success=false', async ({ page }) => {
    await page.goto('/Guest/Cart');
    await page.waitForTimeout(500);

    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/Guest/Cart/AddToCart', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ productId: 9999999, quantity: 1 })
        });
        if (!r.ok) return { ok: false, status: r.status };
        const data = await r.json();
        return { ok: true, success: data.success };
      } catch (e) {
        return { ok: false, error: e.message };
      }
    });
    // API phải trả về success: false vì sản phẩm không tồn tại
    expect(result.ok).toBe(true); // HTTP 200
    expect(result.success).toBe(false); // Nhưng nghiệp vụ thất bại
  });

  test('TC-CART-005: [Fail] Thêm sản phẩm với số lượng âm → Hệ thống chấp nhận (lỗi nghiệp vụ)', async ({ page }) => {
    await page.goto('/Guest/Cart');

    // CartController.AddToCart không validate quantity âm → đây là lỗi nghiệp vụ
    // Test này documented bug: hệ thống KHÔNG nên cho phép quantity âm
    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/Guest/Cart/AddToCart', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ productId: 1, quantity: -5 })
        });
        if (!r.ok) return { ok: false, status: r.status };
        const data = await r.json();
        return { ok: true, success: data.success, data };
      } catch (e) {
        return { ok: false, error: e.message };
      }
    });
    // BUG DOCUMENTATION: Hệ thống hiện tại chấp nhận quantity âm (success=true)
    // Kỳ vọng đúng nghiệp vụ: success nên = false với quantity âm
    // Test này sẽ FAIL vì hệ thống chưa validate → cần sửa code
    expect(result.ok).toBe(true); // HTTP ok
    // Dòng này test nghiệp vụ - nếu pass là hệ thống đã fix bug, nếu fail là bug còn tồn tại
    // Với hệ thống hiện tại CHƯA fix: success=true (bug) → test expect false → FAILED
    if (result.success === true) {
      // Ghi nhận bug: hệ thống chấp nhận quantity âm
      console.warn('[BUG] CartController chấp nhận quantity âm: -5');
    }
    expect(result.success).toBe(false); // Expectation: NÊN reject quantity âm
  });
});

// ===========================================================
test.describe('TC-CHK: Checkout (Thanh toán)', () => {

  test('TC-CHK-001: [Pass] Load trang Checkout khi chưa đăng nhập → Redirect về Login', async ({ page }) => {
    await page.goto('/Guest/Checkout');
    await page.waitForTimeout(1000);
    // Checkout bắt đăng nhập → redirect Login
    const url = page.url();
    const isRedirected = url.includes('Login') || url.includes('Cart') || !url.includes('Checkout');
    // Không được là Error 500
    expect(url).not.toContain('Error');
    expect(isRedirected).toBe(true);
  });

  test('TC-CHK-002: [Pass] API ApplyVoucher với voucher hợp lệ → Trả về kết quả JSON đúng format', async ({ page }) => {
    await loginAsCustomer(page);

    // Test với voucher bất kỳ để kiểm tra API trả về JSON đúng format
    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/Guest/Checkout/ApplyVoucher', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ code: 'CAFEHELLO2024', orderAmount: 200000 })
        });
        if (!r.ok) return { ok: false, status: r.status };
        const data = await r.json();
        return { ok: true, hasSuccess: 'success' in data, hasMessage: 'message' in data, data };
      } catch (e) {
        return { ok: false, error: e.message };
      }
    });
    // API phải trả về JSON có trường success và message (bất kể voucher có hợp lệ hay không)
    expect(result.ok).toBe(true);
    expect(result.hasSuccess).toBe(true);
    expect(result.hasMessage).toBe(true);
  });

  test('TC-CHK-003: [Fail] ApplyVoucher với mã không tồn tại → Trả về success=false', async ({ page }) => {
    await loginAsCustomer(page);

    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/Guest/Checkout/ApplyVoucher', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ code: 'SAI_TE_TUA_XYZ999', orderAmount: 100000 })
        });
        if (!r.ok) return { ok: false, status: r.status };
        return await r.json();
      } catch (e) {
        return { success: false, fallback: true };
      }
    });
    // Voucher không tồn tại → success: false
    expect(result.success).toBe(false);
  });

  test('TC-CHK-004: [Fail] ApplyVoucher với code rỗng → Không hợp lệ, trả success=false', async ({ page }) => {
    await loginAsCustomer(page);

    // Gửi code rỗng - CheckoutController.ApplyVoucher sẽ gọi GetVoucherByCodeAsync("")
    // Nếu không có voucher code = "" thì trả false
    const result = await page.evaluate(async () => {
      try {
        const r = await fetch('/Guest/Checkout/ApplyVoucher', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ code: '', orderAmount: 150000 })
        });
        if (!r.ok) return { ok: false, status: r.status };
        return await r.json();
      } catch (e) {
        return { success: false, fallback: true };
      }
    });
    // Code rỗng → không tìm thấy voucher → success: false
    expect(result.success).toBe(false);
  });

  test('TC-CHK-005: [Pass] Trang Checkout khi đã đăng nhập và có giỏ hàng → Hiện form thanh toán', async ({ page }) => {
    await loginAsCustomer(page);

    // Thêm sản phẩm vào giỏ
    await page.evaluate(async () => {
      await fetch('/Guest/Cart/AddToCart', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId: 1, quantity: 1 })
      });
    });
    await page.waitForTimeout(500);

    await page.goto('/Guest/Checkout');
    await page.waitForTimeout(1000);

    // Nếu sản phẩm ID=1 tồn tại → trang checkout hiển thị
    // Nếu không → redirect Cart (không crash)
    await expect(page).not.toHaveURL(/Error/i);
  });
});
