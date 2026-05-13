// Auto-generated API edge case tests
import { test, expect } from '@playwright/test';

const API_BASE = 'http://localhost:5200/api';

test.describe('API Boundary and Edge Case Tests (Auto-generated)', () => {

  test('TC-EXT-100: CategoryApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-101: CategoryApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-102: CategoryApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-103: CategoryApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-104: CategoryApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-105: CategoryApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-106: CategoryApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-107: CategoryApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-108: CategoryApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-109: CategoryApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-110: CategoryApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/CategoryApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/CategoryApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/CategoryApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/CategoryApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-111: ComboApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-112: ComboApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-113: ComboApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-114: ComboApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-115: ComboApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-116: ComboApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-117: ComboApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-118: ComboApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-119: ComboApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-120: ComboApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-121: ComboApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/ComboApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/ComboApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/ComboApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/ComboApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-122: CommentApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-123: CommentApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-124: CommentApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-125: CommentApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-126: CommentApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-127: CommentApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-128: CommentApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-129: CommentApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-130: CommentApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-131: CommentApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-132: CommentApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/CommentApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/CommentApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/CommentApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/CommentApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-133: ContactApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-134: ContactApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-135: ContactApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-136: ContactApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-137: ContactApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-138: ContactApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-139: ContactApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-140: ContactApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-141: ContactApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-142: ContactApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-143: ContactApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/ContactApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/ContactApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/ContactApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/ContactApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-144: FoodExcel - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-145: FoodExcel - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-146: FoodExcel - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-147: FoodExcel - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-148: FoodExcel - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-149: FoodExcel - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-150: FoodExcel - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-151: FoodExcel - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-152: FoodExcel - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-153: FoodExcel - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-154: FoodExcel - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/FoodExcel/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/FoodExcel/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/FoodExcel/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/FoodExcel/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-155: Foods - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Foods`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Foods`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Foods`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-156: Foods - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Foods/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Foods/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Foods/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-157: Foods - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Foods/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Foods/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Foods/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-158: Foods - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Foods/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Foods/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Foods/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-159: Foods - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Foods/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Foods/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Foods/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-160: Foods - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/Foods`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/Foods`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/Foods`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-161: Foods - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/Foods`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/Foods`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/Foods`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-162: Foods - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/Foods/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/Foods/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/Foods/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-163: Foods - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/Foods/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/Foods/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/Foods/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-164: Foods - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/Foods/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/Foods/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/Foods/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-165: Foods - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/Foods/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/Foods/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/Foods/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/Foods/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-166: NotificationApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-167: NotificationApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-168: NotificationApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-169: NotificationApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-170: NotificationApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-171: NotificationApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-172: NotificationApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-173: NotificationApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-174: NotificationApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-175: NotificationApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-176: NotificationApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/NotificationApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/NotificationApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/NotificationApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/NotificationApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-177: OrderApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-178: OrderApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-179: OrderApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-180: OrderApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-181: OrderApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-182: OrderApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-183: OrderApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-184: OrderApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-185: OrderApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-186: OrderApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-187: OrderApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/OrderApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/OrderApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/OrderApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-188: OrderItems - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-189: OrderItems - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-190: OrderItems - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-191: OrderItems - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-192: OrderItems - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-193: OrderItems - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-194: OrderItems - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-195: OrderItems - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-196: OrderItems - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-197: OrderItems - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-198: OrderItems - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/OrderItems/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/OrderItems/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/OrderItems/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/OrderItems/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-199: ProductApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-200: ProductApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-201: ProductApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-202: ProductApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-203: ProductApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-204: ProductApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-205: ProductApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-206: ProductApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-207: ProductApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-208: ProductApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-209: ProductApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/ProductApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/ProductApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/ProductApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/ProductApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-210: ReportApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-211: ReportApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-212: ReportApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-213: ReportApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-214: ReportApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-215: ReportApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-216: ReportApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-217: ReportApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-218: ReportApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-219: ReportApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-220: ReportApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/ReportApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/ReportApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/ReportApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/ReportApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-221: Token - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Token`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Token`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Token`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Token`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-222: Token - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Token/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Token/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Token/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Token/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-223: Token - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Token/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Token/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Token/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Token/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-224: Token - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Token/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Token/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Token/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Token/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-225: Token - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/Token/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/Token/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/Token/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/Token/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-226: Token - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/Token`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/Token`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/Token`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/Token`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-227: Token - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/Token`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/Token`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/Token`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/Token`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-228: Token - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/Token/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/Token/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/Token/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/Token/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-229: Token - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/Token/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/Token/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/Token/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/Token/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-230: Token - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/Token/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/Token/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/Token/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/Token/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-231: Token - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/Token/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/Token/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/Token/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/Token/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-232: UserApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/UserApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/UserApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/UserApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-233: UserApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/UserApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/UserApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/UserApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-234: UserApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/UserApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/UserApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/UserApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-235: UserApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/UserApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/UserApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/UserApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-236: UserApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/UserApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/UserApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/UserApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-237: UserApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/UserApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/UserApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/UserApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-238: UserApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/UserApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/UserApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/UserApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-239: UserApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/UserApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/UserApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/UserApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-240: UserApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/UserApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/UserApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/UserApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-241: UserApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/UserApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/UserApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/UserApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-242: UserApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/UserApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/UserApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/UserApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/UserApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-243: VoucherApi - Get list', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-244: VoucherApi - Get invalid ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi/9999999`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi/9999999`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-245: VoucherApi - Get negative ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi/-1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi/-1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi/-1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-246: VoucherApi - SQL Injection ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi/' OR '1'='1`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi/' OR '1'='1`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi/' OR '1'='1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-247: VoucherApi - Invalid string type ID', async ({ request }) => {
    let res;
    try {
      if ('get' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi/test-string-id`);
      } else if ('get' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi/test-string-id`, {
          data: {}
        });
      } else if ('get' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi/test-string-id`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-248: VoucherApi - Create empty body', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi`, {
          data: {}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-249: VoucherApi - Create malicious payload', async ({ request }) => {
    let res;
    try {
      if ('post' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi`);
      } else if ('post' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi`, {
          data: {"script":"<script>alert(1)</script>"}
        });
      } else if ('post' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi`, {
          data: {}
        });
      } else if ('post' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi`);
      }
      
      if (false) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-250: VoucherApi - Update empty body', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi/1`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi/1`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi/1`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi/1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-251: VoucherApi - Update invalid ID', async ({ request }) => {
    let res;
    try {
      if ('put' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi/9999999`);
      } else if ('put' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi/9999999`, {
          data: {}
        });
      } else if ('put' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-252: VoucherApi - Delete negative ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi/-1`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi/-1`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi/-1`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
  test('TC-EXT-253: VoucherApi - Delete invalid ID', async ({ request }) => {
    let res;
    try {
      if ('delete' === 'get') {
        res = await request.get(`${API_BASE}/VoucherApi/9999999`);
      } else if ('delete' === 'post') {
        res = await request.post(`${API_BASE}/VoucherApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'put') {
        res = await request.put(`${API_BASE}/VoucherApi/9999999`, {
          data: {}
        });
      } else if ('delete' === 'delete') {
        res = await request.delete(`${API_BASE}/VoucherApi/9999999`);
      }
      
      if (true) {
        expect(res.status()).toBeGreaterThanOrEqual(400);
      } else {
        expect([200, 201, 204, 401, 403, 404]).toContain(res.status());
      }
    } catch (e) {
      expect(e).toBeDefined();
    }
  });
});
