    using Bai2.Data;
using Bai2.Services;
using Bai2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =======================
// ADD SERVICES
// =======================

// MVC
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Dependency Injection (Bài 2 bắt buộc)
builder.Services.AddScoped<IUserService, UserService>(); // Scoped
builder.Services.AddSingleton<ILoggingService, LoggingService>(); // Singleton
builder.Services.AddTransient<IEmailService, EmailService>(); // Transient

var app = builder.Build();

// =======================
// SEED DATA (10 USERS)
// =======================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Đảm bảo database đã được tạo và cập nhật migration mới nhất
    ApplicationDbContextSeedData.Seed(context);
}

// =======================
// MIDDLEWARE
// =======================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// =======================
// ROUTING
// =======================
app.MapControllerRoute(name: "default", pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
