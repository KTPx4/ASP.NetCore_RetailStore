using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Final.Models
{
    public class MyDataContext : DbContext
    {
     
        public DbSet<Account> Accounts { get; set; }
        public static MyDataContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<MyDataContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

        public MyDataContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>().ToCollection("accounts");

        }

    }
}
