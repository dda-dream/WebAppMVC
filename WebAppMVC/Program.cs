using Braintree;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Initializer;
using WebApp_DataAccess.Repository;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Utility;
using WebApp_Utility.BrainTree;
using WebAppMVC.Hubs;
using WebAppMVC.Middleware;
using WebAppMVC.Views.Services;
using WebAppMVC_Utility;

namespace WebAppMVC
{
    public class Program
    {
        static Func<int, int, string> ppp;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug(); // для отладки


            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen( System.Net.IPAddress.Any, 5055, listenOptions =>
                {
                    listenOptions.UseHttps();
                });
                
                
                options.Listen( System.Net.IPAddress.Any, 5050, listenOptions =>
                {
                    listenOptions.UseHttps();
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


            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

  
            //https://sandbox.braintreegateway.com/
            builder.Services.Configure<BrainTreeSettings>(builder.Configuration.GetSection("BrainTree"));
            builder.Services.AddSingleton<IBrainTreeGate, BrainTreeGate>();


            //FACEBOOK
            builder.Services.AddAuthentication().AddFacebook(o =>
                { 
                    o.AppId = "2263897900778199";
                    o.AppSecret = "b43805b8a2d103e715110cf5def93018";
                });

                       
            builder.Services.AddScoped<IMyDailyJournalRepository, MyDailyJournalRepository>();
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<ChatHistoryService>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            //builder.Services.AddScoped<IChatRepository, ChatRepository1>();


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                .CreateLogger();

            builder.Host.UseSerilog(); // подключаем Serilog как источник логов

            builder.Services.AddSwaggerGen();

            System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            var app = builder.Build();
            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/hello", async context =>
                {
                    await context.Response.WriteAsync("Hello, HTTPS!");
                });
            });
            */
            
            app.UseMiddleware<AnomalyLoggingMiddleware>();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                    if (exceptionHandlerPathFeature?.Error != null)
                    {
                        logger.LogError(exceptionHandlerPathFeature.Error,
                            "NEOBRABOTANNOE ISKLUCHENIE: {Path}", exceptionHandlerPathFeature.Path);
                    }

                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("!!!ERROR!!! 500.");
                });
            });

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize(); 
            }

           if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

           /*
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".7z"] = "application/x-7z-compressed";
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    ContentTypeProvider = provider
                });
            */
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>() { { ".7z", "application/x-7z-compressed" } } )
                });



            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();

            app.MapSwagger();
            app.UseSwagger();
            app.UseSwaggerUI();
            

            /*
            app.MapControllerRoute(
                name: "default0",
                pattern: "{controller=Home}/{action=Index}/{id}",
                defaults: new { controller = "Home", action = "Index" });
            */

            app.MapControllerRoute(
                name: "default1",
                pattern: "{controller=Home}/{action=Index}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default2",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            } );

            app.MapHub<ChatHub>("/chathub");

            app.MapGet("/test", 
                async c => 
                { 
                    await c.Response.WriteAsync("XXX"); 
                } );

            app.Run();
        }
    }
}
 