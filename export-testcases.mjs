// export-testcases.mjs
// Chạy: node export-testcases.mjs
// Yêu cầu: playwright đã chạy xong và có file test-results.json

import ExcelJS from 'exceljs';
import { readFileSync, existsSync } from 'fs';
import { resolve, dirname } from 'path';
import { fileURLToPath } from 'url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const JSON_PATH  = resolve(__dirname, 'test-results.json');
const XLSX_PATH  = resolve(__dirname, 'CafeLux_TestCases_Report.xlsx');

// ── Màu sắc ──────────────────────────────────────────────
const COLOR = {
  headerBg   : '1F4973',   // xanh đậm
  passedBg   : 'C6EFCE',   // xanh lá nhạt
  failedBg   : 'FFC7CE',   // đỏ nhạt
  skippedBg  : 'FFEB9C',   // vàng nhạt
  rowAlt     : 'F2F2F2',   // xám nhạt
  white      : 'FFFFFF',
  passedFont : '006100',
  failedFont : '9C0006',
  skippedFont: '9C6500',
};

// ── Helper ────────────────────────────────────────────────
function headerStyle(bgHex) {
  return {
    font      : { bold: true, color: { argb: 'FFFFFFFF' }, size: 11 },
    fill      : { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF' + bgHex } },
    alignment : { horizontal: 'center', vertical: 'middle', wrapText: true },
    border    : {
      top:    { style: 'thin', color: { argb: 'FF000000' } },
      left:   { style: 'thin', color: { argb: 'FF000000' } },
      bottom: { style: 'thin', color: { argb: 'FF000000' } },
      right:  { style: 'thin', color: { argb: 'FF000000' } },
    },
  };
}

function cellStyle(bgHex, fontHex = '000000', bold = false) {
  return {
    font      : { bold, color: { argb: 'FF' + fontHex }, size: 10 },
    fill      : { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF' + bgHex } },
    alignment : { vertical: 'middle', wrapText: true },
    border    : {
      top:    { style: 'thin', color: { argb: 'FFCCCCCC' } },
      left:   { style: 'thin', color: { argb: 'FFCCCCCC' } },
      bottom: { style: 'thin', color: { argb: 'FFCCCCCC' } },
      right:  { style: 'thin', color: { argb: 'FFCCCCCC' } },
    },
  };
}

function statusStyle(status) {
  if (status === 'passed')  return cellStyle(COLOR.passedBg,  COLOR.passedFont,  true);
  if (status === 'failed')  return cellStyle(COLOR.failedBg,  COLOR.failedFont,  true);
  return                           cellStyle(COLOR.skippedBg, COLOR.skippedFont, true);
}

function statusLabel(status) {
  if (status === 'passed')  return '✅ PASS';
  if (status === 'failed')  return '❌ FAIL';
  return '⚠️ SKIP';
}

function msToSec(ms) {
  return ms ? (ms / 1000).toFixed(2) + 's' : '-';
}

function extractError(result) {
  if (!result || !result.errors || result.errors.length === 0) return '';
  const err = result.errors[0];
  const msg = err.message || '';
  // Keep it short for Excel
  return msg.replace(/\x1b\[[0-9;]*m/g, '').substring(0, 300);
}

function moduleFromFile(file) {
  if (!file) return 'Unknown';
  if (file.includes('auth.spec'))         return 'Authentication';
  if (file.includes('guest.spec'))        return 'Guest / Sản phẩm';
  if (file.includes('cart-checkout'))     return 'Giỏ hàng & Thanh toán';
  if (file.includes('admin.spec'))        return 'Admin Panel';
  if (file.includes('pos-auth.spec'))     return 'Staff POS & Phân quyền';
  return 'Khác';
}

// ── Main ─────────────────────────────────────────────────
async function main() {
  if (!existsSync(JSON_PATH)) {
    console.error('❌ Không tìm thấy test-results.json!');
    console.error('   Hãy chạy:  npx playwright test --reporter=json 2>nul');
    process.exit(1);
  }

  const raw  = JSON.parse(readFileSync(JSON_PATH, 'utf-8'));
  const workbook = new ExcelJS.Workbook();
  workbook.creator = 'CaféLux QA';
  workbook.created = new Date();

  // ── SHEET 1: TỔNG KẾT ────────────────────────────────
  const wsSummary = workbook.addWorksheet('📊 Tổng Kết', {
    views: [{ showGridLines: false }],
    properties: { tabColor: { argb: 'FF1F4973' } }
  });

  // Flatten all results
  const allTests = [];
  function walkSuites(suites, file) {
    for (const suite of suites || []) {
      const f = suite.file || file;
      for (const spec of suite.specs || []) {
        for (const test of spec.tests || []) {
          const result = test.results?.[0] || {};
          allTests.push({
            module   : moduleFromFile(f),
            suiteName: suite.title || '',
            testTitle: spec.title || '',
            status   : result.status || 'skipped',
            duration : result.duration || 0,
            error    : extractError(result),
            file     : f,
          });
        }
      }
      if (suite.suites) walkSuites(suite.suites, f);
    }
  }
  walkSuites(raw.suites, '');

  const total   = allTests.length;
  const passed  = allTests.filter(t => t.status === 'passed').length;
  const failed  = allTests.filter(t => t.status === 'failed').length;
  const skipped = total - passed - failed;
  const passRate = total > 0 ? ((passed / total) * 100).toFixed(1) + '%' : '0%';

  // Summary header
  wsSummary.columns = [
    { key: 'a', width: 30 },
    { key: 'b', width: 20 },
  ];

  const addSummaryRow = (label, value, bgHex = COLOR.white, fontHex = '000000', bold = false) => {
    const row = wsSummary.addRow([label, value]);
    row.height = 22;
    ['a','b'].forEach(k => {
      row.getCell(k === 'a' ? 1 : 2).style = cellStyle(bgHex, fontHex, bold);
    });
  };

  wsSummary.addRow([]);
  const titleRow = wsSummary.addRow(['  🍵 CaféLux Website – Báo Cáo Test Cases', '']);
  titleRow.height = 36;
  titleRow.getCell(1).style = {
    font     : { bold: true, size: 16, color: { argb: 'FFFFFFFF' } },
    fill     : { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF1F4973' } },
    alignment: { horizontal: 'center', vertical: 'middle' },
  };
  wsSummary.mergeCells(titleRow.number, 1, titleRow.number, 2);
  wsSummary.addRow([]);

  addSummaryRow('📅 Ngày chạy test', new Date().toLocaleString('vi-VN'), COLOR.rowAlt, '000000', true);
  addSummaryRow('🌐 Website', 'https://localhost:7129', COLOR.white);
  addSummaryRow('🧪 Tổng số test cases', total, COLOR.white, '000000', true);
  addSummaryRow('✅ Passed', passed, COLOR.passedBg, COLOR.passedFont, true);
  addSummaryRow('❌ Failed', failed, COLOR.failedBg, COLOR.failedFont, true);
  addSummaryRow('⚠️ Skipped/Other', skipped, COLOR.skippedBg, COLOR.skippedFont, true);
  addSummaryRow('📈 Tỷ lệ Pass', passRate, COLOR.white, '000000', true);

  wsSummary.addRow([]);
  addSummaryRow('PHÂN BỔ THEO MODULE', '', COLOR.headerBg, 'FFFFFF', true);

  const modules = {};
  for (const t of allTests) {
    if (!modules[t.module]) modules[t.module] = { pass: 0, fail: 0, skip: 0 };
    if (t.status === 'passed')       modules[t.module].pass++;
    else if (t.status === 'failed')  modules[t.module].fail++;
    else                             modules[t.module].skip++;
  }

  const modHeaderRow = wsSummary.addRow(['Module', 'Pass', 'Fail', 'Skip']);
  modHeaderRow.height = 22;
  [1,2,3,4].forEach(c => modHeaderRow.getCell(c).style = headerStyle(COLOR.headerBg));

  let altMod = false;
  for (const [mod, counts] of Object.entries(modules)) {
    const r = wsSummary.addRow([mod, counts.pass, counts.fail, counts.skip]);
    r.height = 20;
    const bg = altMod ? COLOR.rowAlt : COLOR.white;
    [1,2,3,4].forEach(c => r.getCell(c).style = cellStyle(bg));
    altMod = !altMod;
  }

  // ── SHEET 2: CHI TIẾT TẤT CẢ TEST ──────────────────
  const wsAll = workbook.addWorksheet('📋 Chi Tiết Test', {
    views: [{ showGridLines: false, state: 'frozen', ySplit: 1 }],
    properties: { tabColor: { argb: 'FF2E75B6' } }
  });

  wsAll.columns = [
    { header: 'STT',           key: 'no',       width: 6  },
    { header: 'Module',        key: 'module',   width: 22 },
    { header: 'Nhóm Test',     key: 'suite',    width: 30 },
    { header: 'Tên Test Case', key: 'title',    width: 50 },
    { header: 'Trạng Thái',   key: 'status',   width: 14 },
    { header: 'Thời gian',    key: 'duration',  width: 12 },
    { header: 'Lỗi (nếu có)', key: 'error',    width: 55 },
  ];

  // Style header row
  wsAll.getRow(1).height = 28;
  wsAll.getRow(1).eachCell(cell => { cell.style = headerStyle(COLOR.headerBg); });

  let rowNo = 0;
  let altRow = false;
  for (const t of allTests) {
    rowNo++;
    const bg = altRow ? COLOR.rowAlt : COLOR.white;
    const row = wsAll.addRow({
      no      : rowNo,
      module  : t.module,
      suite   : t.suiteName,
      title   : t.testTitle,
      status  : statusLabel(t.status),
      duration: msToSec(t.duration),
      error   : t.error,
    });
    row.height = 20;
    row.eachCell(cell => { cell.style = cellStyle(bg); });
    // Override status cell
    row.getCell(5).style = statusStyle(t.status);
    altRow = !altRow;
  }

  // ── SHEET 3: FAILED TESTS ─────────────────────────
  const wsFailed = workbook.addWorksheet('❌ Failed Tests', {
    views: [{ showGridLines: false, state: 'frozen', ySplit: 1 }],
    properties: { tabColor: { argb: 'FFC00000' } }
  });

  wsFailed.columns = [
    { header: 'STT',           key: 'no',     width: 6  },
    { header: 'Module',        key: 'module', width: 22 },
    { header: 'Nhóm Test',     key: 'suite',  width: 30 },
    { header: 'Tên Test Case', key: 'title',  width: 50 },
    { header: 'Nội dung lỗi', key: 'error',  width: 70 },
    { header: 'Thời gian',    key: 'dur',    width: 12 },
  ];

  wsFailed.getRow(1).height = 28;
  wsFailed.getRow(1).eachCell(cell => { cell.style = headerStyle('C00000'); });

  const failedTests = allTests.filter(t => t.status === 'failed');
  if (failedTests.length === 0) {
    const r = wsFailed.addRow({ no: '', module: '🎉 Tất cả test cases đều PASS!', suite: '', title: '', error: '', dur: '' });
    r.getCell(2).style = cellStyle(COLOR.passedBg, COLOR.passedFont, true);
  } else {
    let fn = 0;
    for (const t of failedTests) {
      fn++;
      const row = wsFailed.addRow({
        no    : fn,
        module: t.module,
        suite : t.suiteName,
        title : t.testTitle,
        error : t.error,
        dur   : msToSec(t.duration),
      });
      row.height = 30;
      row.eachCell(cell => { cell.style = cellStyle(fn % 2 === 0 ? COLOR.rowAlt : COLOR.white); });
    }
  }

  // ── SHEET 4: PASSED TESTS ─────────────────────────
  const wsPassed = workbook.addWorksheet('✅ Passed Tests', {
    views: [{ showGridLines: false, state: 'frozen', ySplit: 1 }],
    properties: { tabColor: { argb: 'FF00B050' } }
  });

  wsPassed.columns = [
    { header: 'STT',           key: 'no',     width: 6  },
    { header: 'Module',        key: 'module', width: 22 },
    { header: 'Nhóm Test',     key: 'suite',  width: 30 },
    { header: 'Tên Test Case', key: 'title',  width: 50 },
    { header: 'Thời gian',    key: 'dur',    width: 12 },
  ];

  wsPassed.getRow(1).height = 28;
  wsPassed.getRow(1).eachCell(cell => { cell.style = headerStyle('007030'); });

  let pn = 0;
  for (const t of allTests.filter(t => t.status === 'passed')) {
    pn++;
    const row = wsPassed.addRow({
      no    : pn,
      module: t.module,
      suite : t.suiteName,
      title : t.testTitle,
      dur   : msToSec(t.duration),
    });
    row.height = 20;
    row.eachCell(cell => { cell.style = cellStyle(pn % 2 === 0 ? COLOR.passedBg : COLOR.white, COLOR.passedFont); });
  }

  // ── Lưu file ─────────────────────────────────────
  await workbook.xlsx.writeFile(XLSX_PATH);
  console.log('');
  console.log('═══════════════════════════════════════════════');
  console.log('✅ Xuất Excel thành công!');
  console.log('📁 File: ' + XLSX_PATH);
  console.log('───────────────────────────────────────────────');
  console.log(`🧪 Tổng: ${total} | ✅ Pass: ${passed} | ❌ Fail: ${failed} | ⚠️ Skip: ${skipped}`);
  console.log(`📈 Tỷ lệ Pass: ${passRate}`);
  console.log('═══════════════════════════════════════════════');
}

main().catch(err => {
  console.error('❌ Lỗi:', err.message);
  process.exit(1);
});
