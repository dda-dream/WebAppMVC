using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Initializer;
using WebApp_DataAccess.Repository;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Utility.BrainTree;
using WebAppMVC.Hubs;
using WebAppMVC.Middleware;
using WebAppMVC.Views.Services;
using WebAppMVC_Utility;

namespace WebAppMVC
{

    public partial class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug(); // для отладки


            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen( System.Net.IPAddress.Any, 5055, listenOptions =>
                {
                    listenOptions.UseHttps();
                    //listenOptions.UseConnectionLogging();
                    //listenOptions.UseConnectionHandler<MyConnectionHandler>();
                });
            });

            

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        npgsqlOptions =>
                        {
                            npgsqlOptions.EnableRetryOnFailure();        // полезно для облачных БД
                            // npgsqlOptions.CommandTimeout(60);         // если нужны долгие запросы
                        }
                    )
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

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>()
                            .AddScoped<IProductRepository, ProductRepository>()
                            .AddScoped<IOrderTableRepository, OrderTableRepository>()
                            .AddScoped<IOrderLineRepository, OrderLineRepository>()
                            .AddScoped<IApplicationUserRepository, ApplicationUserRepository>()
                            .AddScoped<ISalesTableRepository, SalesTableRepository>()
                            .AddScoped<ISalesLineRepository, SalesLineRepository>();


            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

  
            // https ://sandbox.braintreegateway.com/
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


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)
                /*
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                */
                //TODO: подключить логгирование через PostgreSQL
                .CreateLogger();

            builder.Host.UseSerilog(); 



            builder.Services.AddSwaggerGen();
            builder.Services.AddProblemDetails();



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
            app.Use(async (context, next) =>
            {
                Log.Logger.Information("-----------------");

                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                var remoteIp = context.Connection.RemoteIpAddress;

                string s = $" => CLIENT IP: {remoteIp}";
                logger.LogInformation(s);
                Console.WriteLine(s);
                
                await next();
            });

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
                    await context.Response.WriteAsync("!!!ERROR!!! 500. v UseExceptionHandler Prorgam.cs");
                });
            });

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize(); 
            }

            app.UseDeveloperExceptionPage();
            //app.UseExceptionHandler("/error");

            
            app.UseStaticFiles( 
               new StaticFileOptions
               {
                   ServeUnknownFileTypes = true,
                   ContentTypeProvider = new FileExtensionContentTypeProvider() { Mappings = { { ".7z", "application/x-7z-compressed" } } }
               });


            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapSwagger();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { controller = "Home", action = "Index" });
            

            app.MapHub<ChatHub>("/chathub");

            app.MapGet("/_debug/routes/details", (IEnumerable<EndpointDataSource> endpointSources) =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("Registered Routes:");
                sb.AppendLine("==================");

                foreach (var endpoint in endpointSources.SelectMany(x => x.Endpoints))
                {
                    if (endpoint is RouteEndpoint routeEndpoint)
                    {
                        sb.AppendLine($"DisplayName: {routeEndpoint.DisplayName}");
                        sb.AppendLine($"Pattern: {routeEndpoint.RoutePattern.RawText}");
                        sb.AppendLine($"Order: {routeEndpoint.Order}");

                        var httpMethods = routeEndpoint.Metadata
                            .OfType<HttpMethodMetadata>()
                            .FirstOrDefault()?.HttpMethods;

                        if (httpMethods != null)
                        {
                            sb.AppendLine($"Methods: {string.Join(", ", httpMethods)}");
                        }

                        sb.AppendLine("---");
                    }
                }

                return Results.Text(sb.ToString(), "text/plain");
            });


            app.Run();
        }
    }
}
 