-- ============================================================
-- CaféLux – SQL Update Script
-- Tạo các bảng mới cho Admin: Nhân Viên, Phiếu Bán Hàng, Nguyên Liệu
-- ============================================================

-- 1. Bảng Nhân Viên
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='NhanVien' AND xtype='U')
BEGIN
    CREATE TABLE NhanVien (
        MaNV        INT IDENTITY(1,1) PRIMARY KEY,
        HoTen       NVARCHAR(100)   NOT NULL,
        ChucVu      NVARCHAR(50),
        Email       NVARCHAR(100),
        SoDienThoai NVARCHAR(20),
        NgaySinh    DATE,
        DiaChi      NVARCHAR(200),
        LuongCoBan  DECIMAL(18,2)   DEFAULT 0,
        NgayVaoLam  DATE            DEFAULT GETDATE(),
        TrangThai   BIT             DEFAULT 1,
        CreatedAt   DATETIME        DEFAULT GETDATE(),
        UpdatedAt   DATETIME        DEFAULT GETDATE()
    );
    PRINT N'✔ Đã tạo bảng NhanVien';
END
ELSE
    PRINT N'⚠ Bảng NhanVien đã tồn tại, bỏ qua.';
GO

-- 2. Bảng Phiếu Bán Hàng
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PhieuBanHang' AND xtype='U')
BEGIN
    CREATE TABLE PhieuBanHang (
        MaPhieu         INT IDENTITY(1,1) PRIMARY KEY,
        MaNV            INT             REFERENCES NhanVien(MaNV) ON DELETE SET NULL,
        MaKhachHang     INT,            -- có thể FK đến bảng Users nếu cần
        TenKhachHang    NVARCHAR(100)   DEFAULT N'Khách lẻ',
        NgayLap         DATETIME        DEFAULT GETDATE(),
        TongTien        DECIMAL(18,2)   DEFAULT 0,
        TrangThai       NVARCHAR(50)    DEFAULT N'Hoàn Thành',
        GhiChu          NVARCHAR(500),
        CreatedAt       DATETIME        DEFAULT GETDATE()
    );
    PRINT N'✔ Đã tạo bảng PhieuBanHang';
END
ELSE
    PRINT N'⚠ Bảng PhieuBanHang đã tồn tại, bỏ qua.';
GO

-- 3. Bảng Chi Tiết Phiếu Bán Hàng
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ChiTietPhieuBan' AND xtype='U')
BEGIN
    CREATE TABLE ChiTietPhieuBan (
        MaCT        INT IDENTITY(1,1) PRIMARY KEY,
        MaPhieu     INT             NOT NULL REFERENCES PhieuBanHang(MaPhieu) ON DELETE CASCADE,
        MaSanPham   INT,
        TenSanPham  NVARCHAR(200),
        SoLuong     INT             DEFAULT 1,
        DonGia      DECIMAL(18,2)   DEFAULT 0,
        ThanhTien   DECIMAL(18,2)   DEFAULT 0
    );
    PRINT N'✔ Đã tạo bảng ChiTietPhieuBan';
END
ELSE
    PRINT N'⚠ Bảng ChiTietPhieuBan đã tồn tại, bỏ qua.';
GO

-- 4. Bảng Nguyên Liệu
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='NguyenLieu' AND xtype='U')
BEGIN
    CREATE TABLE NguyenLieu (
        MaNL            INT IDENTITY(1,1) PRIMARY KEY,
        TenNguyenLieu   NVARCHAR(200)   NOT NULL,
        DonVi           NVARCHAR(50),
        SoLuongTon      DECIMAL(18,2)   DEFAULT 0,
        GiaNhap         DECIMAL(18,2)   DEFAULT 0,
        MoTa            NVARCHAR(500),
        TrangThai       BIT             DEFAULT 1,
        CreatedAt       DATETIME        DEFAULT GETDATE(),
        UpdatedAt       DATETIME        DEFAULT GETDATE()
    );
    PRINT N'✔ Đã tạo bảng NguyenLieu';
END
ELSE
    PRINT N'⚠ Bảng NguyenLieu đã tồn tại, bỏ qua.';
GO

-- ============================================================
-- Dữ liệu mẫu (tùy chọn – xóa nếu không cần)
-- ============================================================

-- Nhân viên mẫu
INSERT INTO NhanVien (HoTen, ChucVu, Email, SoDienThoai, LuongCoBan, NgayVaoLam) VALUES
(N'Nguyễn Văn An',   N'Pha chế',    'an@cafelux.vn',   '0901234561', 6500000, '2024-01-15'),
(N'Trần Thị Bình',   N'Thu ngân',   'binh@cafelux.vn', '0901234562', 5800000, '2024-03-20'),
(N'Lê Hoàng Nam',    N'Quản lý ca', 'nam@cafelux.vn',  '0901234563', 9000000, '2023-06-10');

-- Nguyên liệu mẫu
INSERT INTO NguyenLieu (TenNguyenLieu, DonVi, SoLuongTon, GiaNhap, MoTa) VALUES
(N'Cà phê nguyên hạt',  N'kg',   25.5, 180000, N'Cà phê Arabica rang mộc'),
(N'Sữa tươi',           N'lít',  40.0,  28000, N'Sữa tươi không đường'),
(N'Đường cát trắng',    N'kg',   30.0,  20000, N'Đường tinh luyện'),
(N'Trà đen Ceylon',     N'kg',   10.0, 120000, N'Trà Ceylon nhập khẩu'),
(N'Trân châu đen',      N'kg',   15.0,  45000, N'Trân châu tapioca');
GO

PRINT N'=== Hoàn tất cập nhật cơ sở dữ liệu CaféLux ===';
