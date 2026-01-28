using Microsoft.EntityFrameworkCore;
using Bai2.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm DbContext với SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// QUAN TRỌNG: Thêm code tạo database tự động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Kiểm tra và tạo database
        var dbName = context.Database.GetDbConnection().Database;
        Console.WriteLine($"Đang kết nối đến database: {dbName}");
        
        // Tạo database nếu chưa tồn tại
        var created = context.Database.EnsureCreated();
        
        if (created)
        {
            Console.WriteLine($"✅ Đã tạo database '{dbName}' thành công!");
        }
        else
        {
            Console.WriteLine($"✅ Database '{dbName}' đã tồn tại!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Lỗi khi tạo database: {ex.Message}");
    }
}

app.Run();