using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }
        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Таблицы в lowercase
                entity.SetTableName(entity.GetTableName().ToLower());

                // Колонки в lowercase
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower());
                }

                // Первичные ключи
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToLower());
                }

                // Индексы
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToLower());
                }
            }
        }
        */


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
