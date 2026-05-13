using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Application.Interfaces;
using ClosedXML.Excel;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(AdminAuthorizeFilter))]
    public class ExcelExportController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IReportService _reportService;

        public ExcelExportController(
            IOrderService orderService,
            IProductService productService,
            IReportService reportService)
        {
            _orderService = orderService;
            _productService = productService;
            _reportService = reportService;
        }

        // Xuất danh sách đơn hàng
        public async Task<IActionResult> ExportOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Đơn Hàng");

            // Header
            ws.Cell(1, 1).Value = "Mã ĐH";
            ws.Cell(1, 2).Value = "Khách Hàng";
            ws.Cell(1, 3).Value = "Tổng Tiền";
            ws.Cell(1, 4).Value = "Trạng Thái";
            ws.Cell(1, 5).Value = "Địa Chỉ";
            ws.Cell(1, 6).Value = "Ngày Đặt";

            // Style header
            var headerRange = ws.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.Red;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (var order in orders)
            {
                ws.Cell(row, 1).Value = order.OrderCode;
                ws.Cell(row, 2).Value = order.UserName;
                ws.Cell(row, 3).Value = order.TotalAmount;
                ws.Cell(row, 4).Value = order.Status;
                ws.Cell(row, 5).Value = order.Address;
                ws.Cell(row, 6).Value = order.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                row++;
            }

            // Format currency column
            ws.Column(3).Style.NumberFormat.Format = "#,##0";
            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"DonHang_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        // Xuất danh sách sản phẩm
        public async Task<IActionResult> ExportProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Sản Phẩm");

            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Tên Sản Phẩm";
            ws.Cell(1, 3).Value = "Giá";
            ws.Cell(1, 4).Value = "Mô Tả";
            ws.Cell(1, 5).Value = "Trạng Thái";

            var headerRange = ws.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.Red;
            headerRange.Style.Font.FontColor = XLColor.White;

            int row = 2;
            foreach (var p in products)
            {
                ws.Cell(row, 1).Value = p.Id;
                ws.Cell(row, 2).Value = p.Name;
                ws.Cell(row, 3).Value = p.Price;
                ws.Cell(row, 4).Value = p.Description;
                ws.Cell(row, 5).Value = p.IsActive ? "Đang bán" : "Ẩn";
                row++;
            }

            ws.Column(3).Style.NumberFormat.Format = "#,##0";
            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"SanPham_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");
        }

        // Xuất báo cáo doanh thu
        public async Task<IActionResult> ExportRevenueReport()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var topProducts = await _reportService.GetTopSellingProductsAsync(20);

            using var workbook = new XLWorkbook();

            // Sheet 1: Tổng quan
            var ws1 = workbook.Worksheets.Add("Tổng Quan");
            ws1.Cell(1, 1).Value = "Báo Cáo Doanh Thu FastFood";
            ws1.Cell(1, 1).Style.Font.Bold = true;
            ws1.Cell(1, 1).Style.Font.FontSize = 16;
            ws1.Range(1, 1, 1, 4).Merge();

            ws1.Cell(3, 1).Value = "Ngày xuất:";
            ws1.Cell(3, 2).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            ws1.Cell(4, 1).Value = "Tổng đơn hàng:";
            ws1.Cell(4, 2).Value = orders.Count();

            ws1.Cell(5, 1).Value = "Tổng doanh thu:";
            ws1.Cell(5, 2).Value = orders.Sum(o => o.TotalAmount);
            ws1.Cell(5, 2).Style.NumberFormat.Format = "#,##0";

            // Sheet 2: Top sản phẩm bán chạy
            var ws2 = workbook.Worksheets.Add("Top Bán Chạy");
            ws2.Cell(1, 1).Value = "Sản Phẩm";
            ws2.Cell(1, 2).Value = "Số Lượng Bán";
            ws2.Cell(1, 3).Value = "Doanh Thu";

            var header2 = ws2.Range(1, 1, 1, 3);
            header2.Style.Font.Bold = true;
            header2.Style.Fill.BackgroundColor = XLColor.Red;
            header2.Style.Font.FontColor = XLColor.White;

            int r = 2;
            foreach (var tp in topProducts)
            {
                ws2.Cell(r, 1).Value = tp.ProductName;
                ws2.Cell(r, 2).Value = tp.TotalSold;
                ws2.Cell(r, 3).Value = tp.Revenue;
                r++;
            }

            ws2.Column(3).Style.NumberFormat.Format = "#,##0";
            ws2.Columns().AdjustToContents();
            ws1.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"BaoCao_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");
        }

        // Xuất báo cáo Word (HTML-based .doc)
        public async Task<IActionResult> ExportWord()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var topProducts = await _reportService.GetTopSellingProductsAsync(20);

            var totalRevenue = orders.Sum(o => o.TotalAmount);
            var totalOrders = orders.Count();

            var html = $@"
<html xmlns:o='urn:schemas-microsoft-com:office:office'
      xmlns:w='urn:schemas-microsoft-com:office:word'
      xmlns='http://www.w3.org/TR/REC-html40'>
<head><meta charset='utf-8'/>
<style>
    body {{ font-family: 'Segoe UI', Arial, sans-serif; padding: 20px; }}
    h1 {{ color: #DC2626; text-align: center; border-bottom: 3px solid #DC2626; padding-bottom: 10px; }}
    h2 {{ color: #1F2937; margin-top: 30px; }}
    table {{ width: 100%; border-collapse: collapse; margin-top: 10px; }}
    th {{ background-color: #DC2626; color: white; padding: 10px; text-align: left; font-weight: bold; }}
    td {{ padding: 8px 10px; border-bottom: 1px solid #E5E7EB; }}
    tr:nth-child(even) {{ background-color: #F9FAFB; }}
    .summary-box {{ background: #FEF2F2; border: 2px solid #DC2626; border-radius: 8px; padding: 15px; margin: 15px 0; }}
    .summary-row {{ display: flex; justify-content: space-between; margin: 5px 0; }}
    .label {{ font-weight: bold; color: #374151; }}
    .value {{ color: #DC2626; font-weight: bold; }}
    .footer {{ margin-top: 30px; text-align: center; color: #9CA3AF; font-size: 12px; border-top: 1px solid #E5E7EB; padding-top: 10px; }}
</style>
</head>
<body>
    <h1>📋 BÁO CÁO ĐƠN HÀNG - FASTFOOD</h1>

    <div class='summary-box'>
        <p><span class='label'>📅 Ngày xuất báo cáo:</span> <span class='value'>{DateTime.Now:dd/MM/yyyy HH:mm}</span></p>
        <p><span class='label'>📦 Tổng số đơn hàng:</span> <span class='value'>{totalOrders}</span></p>
        <p><span class='label'>💰 Tổng doanh thu:</span> <span class='value'>{totalRevenue:N0}đ</span></p>
    </div>

    <h2>📋 Danh Sách Đơn Hàng</h2>
    <table>
        <tr>
            <th>Mã ĐH</th>
            <th>Khách Hàng</th>
            <th>Tổng Tiền</th>
            <th>Trạng Thái</th>
            <th>Địa Chỉ</th>
            <th>Ngày Đặt</th>
        </tr>";

            foreach (var order in orders)
            {
                html += $@"
        <tr>
            <td>{order.OrderCode}</td>
            <td>{order.UserName}</td>
            <td style='text-align:right; color:#DC2626; font-weight:bold;'>{order.TotalAmount:N0}đ</td>
            <td>{order.Status}</td>
            <td>{order.Address}</td>
            <td>{order.CreatedAt:dd/MM/yyyy HH:mm}</td>
        </tr>";
            }

            html += @"
    </table>

    <h2>🏆 Top Sản Phẩm Bán Chạy</h2>
    <table>
        <tr>
            <th>Sản Phẩm</th>
            <th>Số Lượng Bán</th>
            <th>Doanh Thu</th>
        </tr>";

            foreach (var tp in topProducts)
            {
                html += $@"
        <tr>
            <td>{tp.ProductName}</td>
            <td style='text-align:center;'>{tp.TotalSold}</td>
            <td style='text-align:right; color:#DC2626; font-weight:bold;'>{tp.Revenue:N0}đ</td>
        </tr>";
            }

            html += $@"
    </table>

    <div class='footer'>
        <p>Báo cáo được tạo tự động bởi hệ thống FastFood — {DateTime.Now:dd/MM/yyyy HH:mm}</p>
    </div>
</body>
</html>";

            var bytes = System.Text.Encoding.UTF8.GetBytes(html);
            var fileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmm}.doc";
            return File(bytes, "application/msword", fileName);
        }
    }
}
