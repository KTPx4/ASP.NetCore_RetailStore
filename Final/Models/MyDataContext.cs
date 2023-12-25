//using Microsoft.EntityFrameworkCore;
//using MongoDB.Driver;
//using MongoDB.EntityFrameworkCore.Extensions;
//using Final.Models;

//namespace Final.Models
//{
//    public class MyDataContext : DbContext
//    {
//        public DbSet<Product> Products { get; set; }
//        public MyDataContext(DbContextOptions options)
//            : base(options)
//        {
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);
//            modelBuilder.Entity<Account>().ToCollection("accounts");
//            modelBuilder.Entity<Product>().ToCollection("products");

//        }
//        public DbSet<Account> Accounts { get; set; }


//        public DbSet<Final.Models.Product> Product { get; set; } = default!;

//    }
//}


using Microsoft.EntityFrameworkCore;
using Final.Models;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Final.Models
{
    public class MyDataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public MyDataContext(DbContextOptions<MyDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().ToCollection("accounts");
            modelBuilder.Entity<Product>().ToCollection("products");
            modelBuilder.Entity<Order>().ToCollection("orders");
            modelBuilder.Entity<OrderDetail>().ToCollection("orderdetails");
        }
    }
}
