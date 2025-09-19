using Microsoft.EntityFrameworkCore;
using WebAppMVC.Models;

namespace WebAppMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<CategoryModel> Category { get; set; }
         public DbSet<ProductModel> Product { get; set; }



        public DbSet<MyDailyJournalModel> MyDailyJournal { get; set; }
        public DbSet<TestForMigrationModel> TestForMigration_1 { get; set; }
    }
}
