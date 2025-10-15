using Braintree;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Utility;
using WebApp_Utility.BrainTree;
using WebAppMVC_Utility;

namespace WebAppMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5055, listenOptions =>
                {
                    listenOptions.UseConnectionHandler<MyConnectionHandler>();
                });
                
                options.ListenAnyIP(5050, listenOptions =>
                {
                    listenOptions.UseConnectionHandler<MyConnectionHandler>();
                });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))  
            );

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddDefaultTokenProviders()
                            .AddDefaultUI()
                            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });

            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderTableRepository, OrderTableRepository>();
            builder.Services.AddScoped<IOrderLineRepository, OrderLineRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<ISalesTableRepository, SalesTableRepository>();
            builder.Services.AddScoped<ISalesLineRepository, SalesLineRepository>();


            builder.Services.Configure<BrainTreeSettings>(builder.Configuration.GetSection("BrainTree"));
            builder.Services.AddSingleton<IBrainTreeGate, BrainTreeGate>();

            builder.Services.AddAuthentication().AddFacebook(o =>
                { 
                    o.AppId = "2263897900778199";
                    o.AppSecret = "b43805b8a2d103e715110cf5def93018";
                });




                       
            builder.Services.AddScoped<IMyDailyJournalRepository, MyDailyJournalRepository>();





            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
 