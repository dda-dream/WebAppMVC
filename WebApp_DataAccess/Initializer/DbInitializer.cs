using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp_DataAccess.Data;
using WebAppMVC_Models;
using WebAppMVC_Utility;

namespace WebApp_DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        void IDbInitializer.Initialize()
        {

            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
            }

            if (_roleManager.RoleExistsAsync(WC.AdminRole).GetAwaiter().GetResult() == false)
            {
                _roleManager.CreateAsync(new IdentityRole { Id = WC.AdminRole, Name = WC.AdminRole }).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole { Id = WC.CustomerRole, Name = WC.CustomerRole }).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser { 
                    UserName = "admin@admin.com", 
                    Email = "admin@admin.com", 
                    FullName = "ADMIN",
                    EmailConfirmed = true, 
                    PhoneNumber = "11111111111"},
                    "-Qweqwe123"
                ).GetAwaiter().GetResult();

                var adminUser = _db.Users.Where(u => u.UserName == "admin@admin.com").FirstOrDefault();

                _userManager.AddToRoleAsync(adminUser, WC.AdminRole).GetAwaiter().GetResult();
            }

        }
    }
}
