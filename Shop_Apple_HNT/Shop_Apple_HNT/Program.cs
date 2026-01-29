using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Repository;

namespace Shop_Apple_HNT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");

            // ================= DB =================
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("ConnectedDb"))
            );

            // ================= IDENTITY =================
            builder.Services
                .AddIdentity<AppUserModel, IdentityRole>(options =>
                {
                    // LOGIN NGAY – KHÔNG CẦN CONFIRM EMAIL
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;

                    // PASSWORD
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;

                    // LOCKOUT
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // USER
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            // ================= MVC =================
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // ================= SESSION =================
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // ================= MIDDLEWARE =================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();          // PHẢI TRƯỚC AUTH
            app.UseAuthentication();   // BẮT BUỘC
            app.UseAuthorization();

            // ================= ROUTING =================
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            // ================= SEED DATA =================
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                SeedData.SeedingData(context);
            }

            app.Run();
        }
    }
}
