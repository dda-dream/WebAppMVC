using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }


        public DbSet<OrderTable> OrderTable { get; set; }
        public DbSet<OrderLine> OrderLine { get; set; }


        public DbSet<SalesTable> SalesTable { get; set; }
        public DbSet<SalesLine> SalesLine { get; set; }



        public DbSet<MyDailyJournalModel> MyDailyJournal { get; set; }
        public DbSet<LogTableModel> LogTable { get; set; }

        public DbSet<ChatModel> Chat {get; set;}
        
        
        //My test Table//
        public DbSet<TestForMigrationModel> TestForMigration_1 { get; set; }


    }
}
