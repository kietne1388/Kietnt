using Microsoft.AspNetCore.Mvc;
using FastFood.Application.Interfaces;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/voucher")]
    public class VoucherApiController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherApiController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null)
        {
            var vouchers = await _voucherService.GetAllVouchersAsync(searchTerm);
            return Ok(vouchers);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var vouchers = await _voucherService.GetActiveVouchersAsync();
            return Ok(vouchers);
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var voucher = await _voucherService.GetVoucherByCodeAsync(code);

            if (voucher == null)
                return NotFound(new { message = "Voucher not found" });

            return Ok(voucher);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserVouchers(int userId)
        {
            var vouchers = await _voucherService.GetUserVouchersAsync(userId);
            return Ok(vouchers);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVoucherRequest request)
        {
            var voucher = await _voucherService.CreateVoucherAsync(
                request.Code,
                request.DiscountAmount,
                request.DiscountPercent,
                request.ExpiredAt,
                request.Quantity
            );

            return Ok(voucher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _voucherService.DeleteVoucherAsync(id);

            if (!success)
                return NotFound(new { message = "Voucher not found" });

            return Ok(new { message = "Voucher deleted successfully" });
        }

        [HttpPost("assign-welcome/{userId}")]
        public async Task<IActionResult> AssignWelcomeVouchers(int userId)
        {
            await _voucherService.AssignWelcomeVouchersAsync(userId);
            return Ok(new { message = "Welcome vouchers assigned successfully (5 discount + 3 freeship)" });
        }

        [HttpPost("assign-daily/{userId}")]
        public async Task<IActionResult> AssignDailyVouchers(int userId)
        {
            await _voucherService.AssignDailyLoginVouchersAsync(userId);
            return Ok(new { message = "Daily login vouchers assigned successfully (1 discount + 1 freeship)" });
        }

        [HttpPost("apply/{code}")]
        public async Task<IActionResult> Apply(string code)
        {
            var success = await _voucherService.ApplyVoucherAsync(code);

            if (!success)
                return BadRequest(new { message = "Cannot apply this voucher" });

            return Ok(new { message = "Voucher applied successfully" });
        }

        [HttpPost("calculate-discount")]
        public async Task<IActionResult> CalculateDiscount([FromBody] CalculateDiscountRequest request)
        {
            var discount = await _voucherService.CalculateDiscountAsync(request.Code, request.OrderAmount);
            return Ok(new { discount });
        }
    }

    public record CreateVoucherRequest(string Code, decimal DiscountAmount, int? DiscountPercent, DateTime ExpiredAt, int Quantity);
    public record CalculateDiscountRequest(string Code, decimal OrderAmount);
}
