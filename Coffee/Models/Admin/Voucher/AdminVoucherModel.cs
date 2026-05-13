using FastFood.Models.Voucher;
using FastFood.Models.Common;

namespace FastFood.Models.Admin.Voucher
{
    public class AdminVoucherModel
    {
        public List<VoucherModel> Vouchers { get; set; } = new();
        public PaginationModel Pagination { get; set; } = new();
    }
}
