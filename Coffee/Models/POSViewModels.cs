using System;

namespace FastFood.Models
{
    // --- POS & ADMIN VIEW MODELS ---
    
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty; // Position mapping
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public decimal LuongCoBan { get; set; }
        public DateTime NgayVaoLam { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; } = true; // IsActive mapping
    }

    public class SalesReceiptModel
    {
        public int Id { get; set; }
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = "Hoàn Thành";
        public string GhiChu { get; set; } = string.Empty;
    }

    public class SalesReceiptDetailModel
    {
        public int Id { get; set; }
        public int MaPhieu { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }

    public class AttendanceModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public decimal HourlyRate { get; set; } = 30000;
        public decimal TotalSalary => ClockOutTime.HasValue ? (decimal)(ClockOutTime.Value - ClockInTime).TotalHours * HourlyRate : 0;
    }
}
