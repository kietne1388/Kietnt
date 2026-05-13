using FastFood.Application.Interfaces;
using FastFood.Application.DTOs;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/foods")]
    [Authorize]
    public class FoodExcelController : ControllerBase
    {
        private readonly IProductService _productService;

        public FoodExcelController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Export danh sách Foods hiện tại ra file Excel (.xlsx)
        /// </summary>
        [HttpGet("export")]
        public async Task<IActionResult> ExportExcel()
        {
            var products = await _productService.GetAllProductsAsync();
            
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Foods");

            // Header
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Price";
            worksheet.Cell(1, 4).Value = "Description";
            worksheet.Cell(1, 5).Value = "ImageUrl";
            worksheet.Cell(1, 6).Value = "CategoryId";
            worksheet.Cell(1, 7).Value = "CategoryName";

            // Style header
            var headerRange = worksheet.Range("A1:G1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.Orange;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data
            int row = 2;
            foreach (var p in products)
            {
                worksheet.Cell(row, 1).Value = p.Id;
                worksheet.Cell(row, 2).Value = p.Name;
                worksheet.Cell(row, 3).Value = p.Price;
                worksheet.Cell(row, 4).Value = p.Description;
                worksheet.Cell(row, 5).Value = p.ImageUrl;
                worksheet.Cell(row, 6).Value = p.CategoryId;
                worksheet.Cell(row, 7).Value = p.CategoryName;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Foods_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            );
        }

        /// <summary>
        /// Import Foods từ file Excel. Cột: Name, Price, Description, ImageUrl, CategoryId
        /// </summary>
        [HttpPost("import")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Vui lòng chọn file Excel" });

            var errors = new List<string>();
            int successCount = 0;

            try
            {
                using var stream = file.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed()?.RowsUsed().Skip(1); // Bỏ header

                if (rows == null)
                    return BadRequest(new { message = "File Excel trống" });

                foreach (var row in rows)
                {
                    try
                    {
                        var name = row.Cell(1).GetString().Trim();
                        var priceText = row.Cell(2).GetString().Trim();
                        var description = row.Cell(3).GetString().Trim();
                        var imageUrl = row.Cell(4).GetString().Trim();
                        var categoryIdText = row.Cell(5).GetString().Trim();

                        if (string.IsNullOrEmpty(name))
                        {
                            errors.Add($"Dòng {row.RowNumber()}: Name không được để trống");
                            continue;
                        }

                        if (!decimal.TryParse(priceText, out var price) || price <= 0)
                        {
                            errors.Add($"Dòng {row.RowNumber()}: Price không hợp lệ");
                            continue;
                        }

                        if (!int.TryParse(categoryIdText, out var categoryId))
                        {
                            errors.Add($"Dòng {row.RowNumber()}: CategoryId không hợp lệ");
                            continue;
                        }

                        await _productService.CreateProductAsync(name, description, price, imageUrl, categoryId);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Dòng {row.RowNumber()}: {ex.Message}");
                    }
                }

                return Ok(new
                {
                    message = $"Import thành công {successCount} sản phẩm",
                    successCount,
                    errorCount = errors.Count,
                    errors
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi đọc file Excel: " + ex.Message });
            }
        }
    }
}
