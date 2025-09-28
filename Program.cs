using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Data;

namespace WebAppMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /*
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5050);
                options.Listen(System.Net.IPAddress.Any, 5055, listenOptions =>
                        {
                            listenOptions.UseHttps(); // HTTPS
                        });
            });
            */

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole(); // вывод логов в консоль
            /*
            builder.WebHost.ConfigureKestrel(options =>
{
                options.ListenAnyIP(5055, listenOptions =>
                {
                    listenOptions.UseConnectionHandler<LoggingConnectionHandler>();
                });
                
                options.ListenAnyIP(5050, listenOptions =>
                {
                    listenOptions.UseConnectionHandler<LoggingConnectionHandler>();
                });
                
                
            });
            */
            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))  
            );
            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            //app.MapConnections("/tcp", c => c.UseConnectionHandler<LoggingConnectionHandler>());

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }

}
