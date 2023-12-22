using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using Final.Models;

namespace Final.Models
{
    public class MyDataContext : DbContext
    {
     
        public DbSet<Account> Accounts { get; set; }
        //public static MyDataContext Create(IMongoDatabase database) =>
        //new(new DbContextOptionsBuilder<MyDataContext>()
        //    .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
        //    .Options);
        public DbSet<Product> Products{ get; set; }             
        public MyDataContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>().ToCollection("accounts"); 
            modelBuilder.Entity<Product>().ToCollection("products");

        }
        public DbSet<Final.Models.Product> Product { get; set; } = default!;

    }
}
