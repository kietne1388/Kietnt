using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // await ClearAllDataAsync(); // DANGEROUS: Wipes database on every start.

            // Only seed if tables are empty
            if (await _context.Categories.AnyAsync()) return;

            await SeedCategoriesAsync();
            await SeedProductsAsync();
            await SeedCombosAsync();
            await SeedUsersAsync();
            await SeedOrdersAsync();
            await SeedVouchersAsync();
        }

        private async Task ClearAllDataAsync()
        {
            // Xóa theo thứ tự phụ thuộc (con trước, cha sau)
            _context.Notifications.RemoveRange(_context.Notifications);
            _context.UserVouchers.RemoveRange(_context.UserVouchers);
            _context.ComboItems.RemoveRange(_context.ComboItems);
            _context.OrderItems.RemoveRange(_context.OrderItems);
            _context.Comments.RemoveRange(_context.Comments);
            _context.Orders.RemoveRange(_context.Orders);
            _context.Combos.RemoveRange(_context.Combos);
            _context.Vouchers.RemoveRange(_context.Vouchers);
            _context.Products.RemoveRange(_context.Products);
            _context.Categories.RemoveRange(_context.Categories);
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
        }

        // ======================== CATEGORIES ========================
        private async Task SeedCategoriesAsync()
        {
            var categories = new List<Category>
            {
                new Category { Name = "Cà phê",       Description = "Espresso, Americano, Cappuccino, Latte và các loại cà phê đặc sắc" },
                new Category { Name = "Trà",           Description = "Trà xanh, trà đen, trà ô long và trà trái cây thơm mát" },
                new Category { Name = "Sinh tố & Đá xay", Description = "Sinh tố trái cây tươi, frappuccino và đá xay mát lạnh" },
                new Category { Name = "Đồ uống đặc biệt", Description = "Sữa tươi, nước ép, yogurt và thức uống lên men" },
                new Category { Name = "Bánh & Snack",  Description = "Croissant, muffin, bánh ngọt và snack ăn kèm" },
            };

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        // ======================== PRODUCTS ========================
        private async Task SeedProductsAsync()
        {
            var cats = await _context.Categories.ToListAsync();
            int coffee    = cats.First(c => c.Name == "Cà phê").Id;
            int tea       = cats.First(c => c.Name == "Trà").Id;
            int smoothie  = cats.First(c => c.Name == "Sinh tố & Đá xay").Id;
            int special   = cats.First(c => c.Name == "Đồ uống đặc biệt").Id;
            int bakery    = cats.First(c => c.Name == "Bánh & Snack").Id;

            var products = new List<Product>
            {
                // ── CÀ PHÊ ──
                new Product { Name = "Espresso",             Description = "Cà phê espresso nguyên chất, đậm đà và thơm nồng từ hạt Arabica rang Ý",               Price = 35000,  ImageUrl = "/images/coffee/espresso.jpg",      CategoryId = coffee },
                new Product { Name = "Americano",            Description = "Espresso pha loãng với nước nóng, thanh đắng dịu nhẹ phù hợp mọi buổi sáng",           Price = 39000,  ImageUrl = "/images/coffee/americano.jpg",     CategoryId = coffee },
                new Product { Name = "Cappuccino",           Description = "1/3 espresso, 1/3 sữa hấp, 1/3 bọt sữa mịn – hương vị cổ điển nước Ý",                 Price = 49000,  ImageUrl = "/images/coffee/cappuccino.jpg",    CategoryId = coffee },
                new Product { Name = "Latte",                Description = "Espresso kết hợp sữa hấp béo ngậy và một lớp bọt sữa mỏng, nhẹ nhàng",                 Price = 52000,  ImageUrl = "/images/coffee/latte.jpg",         CategoryId = coffee },
                new Product { Name = "Cold Brew",            Description = "Cà phê ủ lạnh 24 giờ, vị ngọt tự nhiên, bóng mượt và không chua gắt",                   Price = 55000,  ImageUrl = "/images/coffee/coldbrew.jpg",      CategoryId = coffee },
                new Product { Name = "Cà phê sữa đá",       Description = "Cà phê phin truyền thống Việt Nam với sữa đặc ngọt thơm, đá lạnh tan chậm",             Price = 35000,  ImageUrl = "/images/coffee/ca-phe-sua-da.jpg", CategoryId = coffee },

                // ── TRÀ ──
                new Product { Name = "Trà Ô Long Sữa",      Description = "Trà ô long thượng hạng pha cùng sữa tươi, topping boba dai giòn hấp dẫn",               Price = 55000,  ImageUrl = "/images/coffee/tra-olong.jpg",     CategoryId = tea },
                new Product { Name = "Trà Sữa Matcha",       Description = "Matcha Uji Nhật Bản xay mịn, pha với sữa tươi nguyên chất – béo, thơm, đắng nhẹ",       Price = 59000,  ImageUrl = "/images/coffee/matcha.jpg",        CategoryId = tea },
                new Product { Name = "Trà Chanh Tươi",       Description = "Trà xanh ướp lạnh với chanh tươi vắt tay và đường mía, cực kỳ giải nhiệt",               Price = 35000,  ImageUrl = "/images/coffee/tra-chanh.jpg",     CategoryId = tea },
                new Product { Name = "Hồng Trà Đào",         Description = "Hồng trà ủ lạnh, slice đào tươi thơm và nước syrup đào ngọt dịu",                        Price = 49000,  ImageUrl = "/images/coffee/hong-tra-dao.jpg",  CategoryId = tea },

                // ── SINH TỐ & ĐÁ XAY ──
                new Product { Name = "Sinh Tố Dâu",         Description = "Dâu tươi xay mịn với sữa chua Hy Lạp và mật ong – giàu vitamin C và antioxidant",         Price = 55000,  ImageUrl = "/images/coffee/smoothie-dau.jpg",  CategoryId = smoothie },
                new Product { Name = "Đá Xay Cà Phê",       Description = "Espresso đá xay kết hợp kem tươi đánh bông và topping chocolate drizzle đẳng cấp",         Price = 65000,  ImageUrl = "/images/coffee/frappe-coffee.jpg", CategoryId = smoothie },
                new Product { Name = "Đá Xay Matcha",        Description = "Matcha Nhật Bản xay với đá và sữa, phủ kem tươi và topping matcha powder",                 Price = 65000,  ImageUrl = "/images/coffee/frappe-matcha.jpg", CategoryId = smoothie },
                new Product { Name = "Sinh Tố Bơ",          Description = "Bơ chín xay mịn cùng sữa tươi và condensed milk – bổ dưỡng và cực kỳ béo ngậy",           Price = 55000,  ImageUrl = "/images/coffee/smoothie-bo.jpg",   CategoryId = smoothie },

                // ── ĐỒ UỐNG ĐẶC BIỆT ──
                new Product { Name = "Sữa Tươi Trân Châu",  Description = "Sữa tươi Vinamilk nguyên chất, trân châu đen nấu đường đen mềm dai đặc biệt",              Price = 45000,  ImageUrl = "/images/coffee/milk-boba.jpg",     CategoryId = special },
                new Product { Name = "Nước ép Cam Tươi",     Description = "Cam Valencia vắt tay 100%, không đường không phẩm màu, giàu vitamin C",                    Price = 45000,  ImageUrl = "/images/coffee/orange-juice.jpg",  CategoryId = special },

                // ── BÁNH & SNACK ──
                new Product { Name = "Croissant Bơ",        Description = "Croissant bơ Pháp nướng vàng giòn, lớp vỏ xốp bên ngoài, mềm thơm bên trong",             Price = 35000,  ImageUrl = "/images/coffee/croissant.jpg",     CategoryId = bakery },
                new Product { Name = "Bánh Muffin Việt Quất", Description = "Muffin việt quất tươi nướng theo đơn hàng, ẩm ướt và đầy ắp hạt việt quất",              Price = 39000,  ImageUrl = "/images/coffee/muffin.jpg",        CategoryId = bakery },
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }

        // ======================== COMBOS ========================
        private async Task SeedCombosAsync()
        {
            var products = await _context.Products.ToListAsync();
            Product P(string name) => products.First(p => p.Name == name);

            var combos = new List<Combo>
            {
                new Combo
                {
                    Name = "Combo Sáng Tốt Lành",
                    Description = "1 Cà phê sữa đá + 1 Croissant Bơ – Khởi đầu ngày mới hoàn hảo",
                    OriginalPrice = 70000, ComboPrice = 55000,
                    ComboType = "1 Người", ImageUrl = "/images/coffee/combo-morning.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Làm Việc",
                    Description = "1 Cold Brew + 1 Latte + 2 Bánh Muffin Việt Quất – Năng lượng cho cả buổi làm việc",
                    OriginalPrice = 188000, ComboPrice = 149000,
                    ComboType = "2 Người", ImageUrl = "/images/coffee/combo-work.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Bạn Bè",
                    Description = "2 Trà Ô Long Sữa + 2 Đá Xay Cà Phê + 2 Croissant Bơ – Tụ tập cùng bạn bè",
                    OriginalPrice = 280000, ComboPrice = 219000,
                    ComboType = "2 Người", ImageUrl = "/images/coffee/combo-friends.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Matcha Lover",
                    Description = "1 Trà Sữa Matcha + 1 Đá Xay Matcha – Thiên đường cho người yêu matcha",
                    OriginalPrice = 124000, ComboPrice = 99000,
                    ComboType = "1 Người", ImageUrl = "/images/coffee/combo-matcha.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Gia Đình",
                    Description = "4 Cà phê sữa đá + 2 Sinh Tố Dâu + 4 Bánh Muffin Việt Quất – Cả nhà cùng vui",
                    OriginalPrice = 412000, ComboPrice = 329000,
                    ComboType = "Gia Đình", ImageUrl = "/images/coffee/combo-family.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Trà & Bánh",
                    Description = "1 Hồng Trà Đào + 1 Trà Chanh Tươi + 2 Croissant Bơ – Chiều tà thư giãn",
                    OriginalPrice = 154000, ComboPrice = 119000,
                    ComboType = "2 Người", ImageUrl = "/images/coffee/combo-tea.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Detox Sáng",
                    Description = "1 Sinh Tố Bơ + 1 Nước ép Cam Tươi – Thanh lọc cơ thể mỗi sáng",
                    OriginalPrice = 100000, ComboPrice = 79000,
                    ComboType = "1 Người", ImageUrl = "/images/coffee/combo-detox.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Espresso Set",
                    Description = "1 Espresso + 1 Americano + 2 Croissant Bơ – Thuần cà phê cho dân sành",
                    OriginalPrice = 144000, ComboPrice = 109000,
                    ComboType = "2 Người", ImageUrl = "/images/coffee/combo-espresso.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Buổi Chiều",
                    Description = "1 Cappuccino + 1 Đá Xay Cà Phê + 1 Bánh Muffin Việt Quất – Nghỉ trưa thật chất",
                    OriginalPrice = 153000, ComboPrice = 119000,
                    ComboType = "1 Người", ImageUrl = "/images/coffee/combo-afternoon.jpg", IsActive = true
                },
                new Combo
                {
                    Name = "Combo Nhóm 4",
                    Description = "2 Latte + 2 Trà Sữa Matcha + 4 Bánh Muffin – Tụ họp nhóm bạn",
                    OriginalPrice = 374000, ComboPrice = 289000,
                    ComboType = "3+ Người", ImageUrl = "/images/coffee/combo-group.jpg", IsActive = true
                },
            };

            await _context.Combos.AddRangeAsync(combos);
            await _context.SaveChangesAsync();

            // Lấy lại combos đã lưu (có Id)
            var savedCombos = await _context.Combos.OrderBy(c => c.Id).ToListAsync();
            var comboItems = new List<ComboItem>();

            // Combo 1: Sáng Tốt Lành
            var c1 = savedCombos.First(c => c.Name == "Combo Sáng Tốt Lành");
            comboItems.Add(new ComboItem { ComboId = c1.Id, ProductId = P("Cà phê sữa đá").Id,   Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c1.Id, ProductId = P("Croissant Bơ").Id,     Quantity = 1 });

            // Combo 2: Làm Việc
            var c2 = savedCombos.First(c => c.Name == "Combo Làm Việc");
            comboItems.Add(new ComboItem { ComboId = c2.Id, ProductId = P("Cold Brew").Id,                  Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c2.Id, ProductId = P("Latte").Id,                      Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c2.Id, ProductId = P("Bánh Muffin Việt Quất").Id,     Quantity = 2 });

            // Combo 3: Bạn Bè
            var c3 = savedCombos.First(c => c.Name == "Combo Bạn Bè");
            comboItems.Add(new ComboItem { ComboId = c3.Id, ProductId = P("Trà Ô Long Sữa").Id,   Quantity = 2 });
            comboItems.Add(new ComboItem { ComboId = c3.Id, ProductId = P("Đá Xay Cà Phê").Id,    Quantity = 2 });
            comboItems.Add(new ComboItem { ComboId = c3.Id, ProductId = P("Croissant Bơ").Id,     Quantity = 2 });

            // Combo 4: Matcha Lover
            var c4 = savedCombos.First(c => c.Name == "Combo Matcha Lover");
            comboItems.Add(new ComboItem { ComboId = c4.Id, ProductId = P("Trà Sữa Matcha").Id,   Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c4.Id, ProductId = P("Đá Xay Matcha").Id,    Quantity = 1 });

            // Combo 5: Gia Đình
            var c5 = savedCombos.First(c => c.Name == "Combo Gia Đình");
            comboItems.Add(new ComboItem { ComboId = c5.Id, ProductId = P("Cà phê sữa đá").Id,          Quantity = 4 });
            comboItems.Add(new ComboItem { ComboId = c5.Id, ProductId = P("Sinh Tố Dâu").Id,            Quantity = 2 });
            comboItems.Add(new ComboItem { ComboId = c5.Id, ProductId = P("Bánh Muffin Việt Quất").Id, Quantity = 4 });

            // Combo 6: Trà & Bánh
            var c6 = savedCombos.First(c => c.Name == "Combo Trà & Bánh");
            comboItems.Add(new ComboItem { ComboId = c6.Id, ProductId = P("Hồng Trà Đào").Id,     Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c6.Id, ProductId = P("Trà Chanh Tươi").Id,   Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c6.Id, ProductId = P("Croissant Bơ").Id,     Quantity = 2 });

            // Combo 7: Detox Sáng
            var c7 = savedCombos.First(c => c.Name == "Combo Detox Sáng");
            comboItems.Add(new ComboItem { ComboId = c7.Id, ProductId = P("Sinh Tố Bơ").Id,           Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c7.Id, ProductId = P("Nước ép Cam Tươi").Id,     Quantity = 1 });

            // Combo 8: Espresso Set
            var c8 = savedCombos.First(c => c.Name == "Combo Espresso Set");
            comboItems.Add(new ComboItem { ComboId = c8.Id, ProductId = P("Espresso").Id,    Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c8.Id, ProductId = P("Americano").Id,   Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c8.Id, ProductId = P("Croissant Bơ").Id, Quantity = 2 });

            // Combo 9: Buổi Chiều
            var c9 = savedCombos.First(c => c.Name == "Combo Buổi Chiều");
            comboItems.Add(new ComboItem { ComboId = c9.Id, ProductId = P("Cappuccino").Id,                 Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c9.Id, ProductId = P("Đá Xay Cà Phê").Id,             Quantity = 1 });
            comboItems.Add(new ComboItem { ComboId = c9.Id, ProductId = P("Bánh Muffin Việt Quất").Id,    Quantity = 1 });

            // Combo 10: Nhóm 4
            var c10 = savedCombos.First(c => c.Name == "Combo Nhóm 4");
            comboItems.Add(new ComboItem { ComboId = c10.Id, ProductId = P("Latte").Id,                     Quantity = 2 });
            comboItems.Add(new ComboItem { ComboId = c10.Id, ProductId = P("Trà Sữa Matcha").Id,            Quantity = 2 });
            comboItems.Add(new ComboItem { ComboId = c10.Id, ProductId = P("Bánh Muffin Việt Quất").Id,    Quantity = 4 });

            await _context.ComboItems.AddRangeAsync(comboItems);
            await _context.SaveChangesAsync();
        }

        // ======================== 10 USERS ========================
        private async Task SeedUsersAsync()
        {
            var users = new List<User>
            {
                new User { Username = "admin",    FullName = "Quản Trị Viên",     Email = "admin@cafelux.vn",      PhoneNumber = "0901234567", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),  Role = "Admin" },
                new User { Username = "user1",    FullName = "Nguyễn Văn An",     Email = "an.nguyen@gmail.com",   PhoneNumber = "0912345678", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user2",    FullName = "Trần Thị Bình",     Email = "binh.tran@gmail.com",   PhoneNumber = "0923456789", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user3",    FullName = "Lê Văn Cường",      Email = "cuong.le@gmail.com",    PhoneNumber = "0934567890", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user4",    FullName = "Phạm Thị Dung",     Email = "dung.pham@gmail.com",   PhoneNumber = "0945678901", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user5",    FullName = "Hoàng Văn Em",      Email = "em.hoang@gmail.com",    PhoneNumber = "0956789012", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user6",    FullName = "Vũ Thị Phương",     Email = "phuong.vu@gmail.com",   PhoneNumber = "0967890123", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user7",    FullName = "Đặng Văn Giang",    Email = "giang.dang@gmail.com",  PhoneNumber = "0978901234", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user8",    FullName = "Bùi Thị Hạnh",      Email = "hanh.bui@gmail.com",    PhoneNumber = "0989012345", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user9",    FullName = "Ngô Văn Khoa",      Email = "khoa.ngo@gmail.com",    PhoneNumber = "0990123456", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
                new User { Username = "user10",   FullName = "Đinh Thị Lan",      Email = "lan.dinh@gmail.com",    PhoneNumber = "0911234567", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),   Role = "User" },
            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        // ======================== 10 ORDERS + ORDER ITEMS ========================
        private async Task SeedOrdersAsync()
        {
            var users    = await _context.Users.Where(u => u.Role == "User").ToListAsync();
            var products = await _context.Products.ToListAsync();
            var combos   = await _context.Combos.ToListAsync();
            if (!users.Any() || !products.Any()) return;

            Product P(string name) => products.First(p => p.Name == name);

            var orders = new List<Order>
            {
                new Order { UserId = users[0].Id, OrderCode = "CF-20260301-001", TotalAmount = 110000,  Status = "Completed", Address = "123 Nguyễn Huệ, Q.1, TP.HCM",        CreatedAt = DateTime.Now.AddDays(-14) },
                new Order { UserId = users[0].Id, OrderCode = "CF-20260301-002", TotalAmount = 329000,  Status = "Completed", Address = "456 Lê Lợi, Q.3, TP.HCM",             CreatedAt = DateTime.Now.AddDays(-12) },
                new Order { UserId = users[1].Id, OrderCode = "CF-20260305-003", TotalAmount = 55000,   Status = "Completed", Address = "789 Trần Hưng Đạo, Q.5, TP.HCM",      CreatedAt = DateTime.Now.AddDays(-10) },
                new Order { UserId = users[1].Id, OrderCode = "CF-20260305-004", TotalAmount = 149000,  Status = "Completed", Address = "101 Võ Văn Tần, Q.10, TP.HCM",         CreatedAt = DateTime.Now.AddDays(-8) },
                new Order { UserId = users[2].Id, OrderCode = "CF-20260310-005", TotalAmount = 219000,  Status = "Completed", Address = "55 Hai Bà Trưng, Q.1, TP.HCM",         CreatedAt = DateTime.Now.AddDays(-6) },
                new Order { UserId = users[2].Id, OrderCode = "CF-20260310-006", TotalAmount = 119000,  Status = "Processing",Address = "22 Điện Biên Phủ, Bình Thạnh, TP.HCM", CreatedAt = DateTime.Now.AddDays(-4) },
                new Order { UserId = users[3].Id, OrderCode = "CF-20260315-007", TotalAmount = 99000,   Status = "Processing",Address = "888 Cách Mạng Tháng 8, Q.3, TP.HCM",  CreatedAt = DateTime.Now.AddDays(-3) },
                new Order { UserId = users[3].Id, OrderCode = "CF-20260315-008", TotalAmount = 289000,  Status = "Pending",   Address = "123 Phan Đăng Lưu, Phú Nhuận, TP.HCM",CreatedAt = DateTime.Now.AddDays(-2) },
                new Order { UserId = users[4].Id, OrderCode = "CF-20260318-009", TotalAmount = 109000,  Status = "Pending",   Address = "456 Nguyễn Trãi, Q.5, TP.HCM",        CreatedAt = DateTime.Now.AddDays(-1) },
                new Order { UserId = users[4].Id, OrderCode = "CF-20260318-010", TotalAmount = 79000,   Status = "Cancelled", Address = "789 Lý Tự Trọng, Q.1, TP.HCM",        CreatedAt = DateTime.Now },
            };

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            var savedOrders = await _context.Orders.OrderBy(o => o.Id).ToListAsync();
            var items = new List<OrderItem>();

            // Order 1: Cà phê x2 + Croissant
            items.Add(new OrderItem { OrderId = savedOrders[0].Id, ProductId = P("Cà phê sữa đá").Id, Quantity = 2, UnitPrice = 35000 });
            items.Add(new OrderItem { OrderId = savedOrders[0].Id, ProductId = P("Croissant Bơ").Id,  Quantity = 1, UnitPrice = 35000 });

            // Order 2: Combo Gia Đình
            items.Add(new OrderItem { OrderId = savedOrders[1].Id, ComboId = combos.First(c => c.Name == "Combo Gia Đình").Id,  Quantity = 1, UnitPrice = 329000 });

            // Order 3: Combo Sáng Tốt Lành
            items.Add(new OrderItem { OrderId = savedOrders[2].Id, ComboId = combos.First(c => c.Name == "Combo Sáng Tốt Lành").Id, Quantity = 1, UnitPrice = 55000 });

            // Order 4: Combo Làm Việc
            items.Add(new OrderItem { OrderId = savedOrders[3].Id, ComboId = combos.First(c => c.Name == "Combo Làm Việc").Id, Quantity = 1, UnitPrice = 149000 });

            // Order 5: Combo Bạn Bè
            items.Add(new OrderItem { OrderId = savedOrders[4].Id, ComboId = combos.First(c => c.Name == "Combo Bạn Bè").Id, Quantity = 1, UnitPrice = 219000 });

            // Order 6: Combo Trà & Bánh
            items.Add(new OrderItem { OrderId = savedOrders[5].Id, ComboId = combos.First(c => c.Name == "Combo Trà & Bánh").Id, Quantity = 1, UnitPrice = 119000 });

            // Order 7: Combo Matcha Lover
            items.Add(new OrderItem { OrderId = savedOrders[6].Id, ComboId = combos.First(c => c.Name == "Combo Matcha Lover").Id, Quantity = 1, UnitPrice = 99000 });

            // Order 8: Combo Nhóm 4
            items.Add(new OrderItem { OrderId = savedOrders[7].Id, ComboId = combos.First(c => c.Name == "Combo Nhóm 4").Id, Quantity = 1, UnitPrice = 289000 });

            // Order 9: Combo Espresso Set
            items.Add(new OrderItem { OrderId = savedOrders[8].Id, ComboId = combos.First(c => c.Name == "Combo Espresso Set").Id, Quantity = 1, UnitPrice = 109000 });

            // Order 10: Combo Detox Sáng (đã hủy)
            items.Add(new OrderItem { OrderId = savedOrders[9].Id, ComboId = combos.First(c => c.Name == "Combo Detox Sáng").Id, Quantity = 1, UnitPrice = 79000 });

            await _context.OrderItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        // ======================== 10 VOUCHERS ========================
        private async Task SeedVouchersAsync()
        {
            var vouchers = new List<Voucher>
            {
                new Voucher { Code = "WELCOME10",    DiscountPercent = 10,  DiscountAmount = 0,     Quantity = 1000, ExpiredAt = DateTime.Now.AddMonths(6),  IsActive = true },
                new Voucher { Code = "COFFEE20",     DiscountPercent = 20,  DiscountAmount = 0,     Quantity = 500,  ExpiredAt = DateTime.Now.AddMonths(3),  IsActive = true },
                new Voucher { Code = "GIAM30K",      DiscountPercent = 0,   DiscountAmount = 30000, Quantity = 200,  ExpiredAt = DateTime.Now.AddMonths(2),  IsActive = true },
                new Voucher { Code = "GIAM50K",      DiscountPercent = 0,   DiscountAmount = 50000, Quantity = 100,  ExpiredAt = DateTime.Now.AddMonths(2),  IsActive = true },
                new Voucher { Code = "FREESHIP",     DiscountPercent = 0,   DiscountAmount = 20000, Quantity = 500,  ExpiredAt = DateTime.Now.AddMonths(3),  IsActive = true },
                new Voucher { Code = "VIP15",        DiscountPercent = 15,  DiscountAmount = 0,     Quantity = 50,   ExpiredAt = DateTime.Now.AddMonths(1),  IsActive = true },
                new Voucher { Code = "COMBO25",      DiscountPercent = 25,  DiscountAmount = 0,     Quantity = 100,  ExpiredAt = DateTime.Now.AddMonths(2),  IsActive = true },
                new Voucher { Code = "NEWUSER",      DiscountPercent = 0,   DiscountAmount = 40000, Quantity = 300,  ExpiredAt = DateTime.Now.AddMonths(6),  IsActive = true },
                new Voucher { Code = "WEEKEND",      DiscountPercent = 0,   DiscountAmount = 25000, Quantity = 200,  ExpiredAt = DateTime.Now.AddMonths(1),  IsActive = true },
                new Voucher { Code = "TESTVOUCHER",  DiscountPercent = 0,   DiscountAmount = 10000, Quantity = 9999, ExpiredAt = DateTime.Now.AddMonths(12), IsActive = true },
            };

            await _context.Vouchers.AddRangeAsync(vouchers);
            await _context.SaveChangesAsync();
        }
    }
}
