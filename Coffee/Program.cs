using FastFood.Filters;
using FastFood.Application.Interfaces;
using FastFood.Application.Services;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Repositories;
using FastFood.Infrastructure.Context;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================== 1. DATABASE CONTEXT ==================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FastFood.Infrastructure")));

// ... (Phần trên giữ nguyên) ...

// ================== 2. REPOSITORIES ==================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IComboRepository, ComboRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserVoucherRepository, UserVoucherRepository>();
builder.Services.AddScoped<GenericRepository<Notification>>();
// concrete registration for OrderRepository if specifically needed by ReportService (but usually IOrderRepository should be enough if changed)
// builder.Services.AddScoped<OrderRepository>(); 
// ================== 3. SERVICES ==================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>(); // QUAN TRỌNG: Giữ lại cái này
builder.Services.AddScoped<IComboService, ComboService>();

// ✅ Voucher, Report & Email Services
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// ... (Phần dưới giữ nguyên) ...

// ================== 4. HTTP CLIENT & SESSION ==================
// Cấu hình để gọi sang API nếu cần
builder.Services.AddHttpClient("FastFoodAPI", client =>
{
    var apiUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7001";
    client.BaseAddress = new Uri(apiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
});

// Cấu hình Session cho Giỏ hàng
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ================== 5. FILTERS & MVC ==================
builder.Services.AddScoped<AuthFilter>();
builder.Services.AddScoped<AdminAuthorizeFilter>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ================== 6. PIPELINE ==================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Guest/Home/Error");
    app.UseHsts();
}

// Chỉ dùng HTTPS redirect khi deploy production, không dùng khi chạy local qua ngrok
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Bắt buộc đặt trước Authorization
app.UseAuthorization();

// Route mặc định
app.MapGet("/", context =>
{
    context.Response.Redirect("/Guest/Home/Index");
    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Guest" });

app.Run();