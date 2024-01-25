using GoodHamburger.Data.Map;
using GoodHamburger.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data
{
    public class GoodHamburgerDbContext : DbContext
    {
        public GoodHamburgerDbContext(DbContextOptions<GoodHamburgerDbContext> options) : base(options) 
        {
        }

        public DbSet<SandwichModel> Sandwiches { get; set; }
        public DbSet<ExtraModel> Extras { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderExtraModel> OrderExtras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SandwichMap());
            modelBuilder.ApplyConfiguration(new ExtraMap());
            modelBuilder.ApplyConfiguration(new OrderMap());



            base.OnModelCreating(modelBuilder);
        }
    }
}
