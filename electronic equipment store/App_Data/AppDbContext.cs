using electronic_equipment_store.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace electronic_equipment_store.App_Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Categories> Categories { get; set; } = null!;
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Manufactures> Manufacturers { get; set; } = null!;
        public DbSet<Order_Items> Order_Items { get; set; } = null!;
        public DbSet<Orders> Orders { get; set; } = null!;
        public DbSet<Payments> Payments { get; set; } = null!;
        public DbSet<Products> Products { get; set; } = null!;
        public DbSet<Promotions> Promotions { get; set; } = null!;
        public DbSet<Reviews> Reviews { get; set; } = null!;
        public DbSet<Shipping> Shipping { get; set; } = null!;


        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}
