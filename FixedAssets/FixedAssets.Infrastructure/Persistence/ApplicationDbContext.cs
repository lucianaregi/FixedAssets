using Microsoft.EntityFrameworkCore;
using FixedAssets.Domain.Entities;

namespace FixedAssets.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserAsset> UserAssets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para UserAsset
            modelBuilder.Entity<UserAsset>()
                .HasKey(ua => new { ua.UserId, ua.ProductId });

            modelBuilder.Entity<UserAsset>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.Assets)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAsset>()
                .HasOne(ua => ua.Product)
                .WithMany(p => p.UserAssets)
                .HasForeignKey(ua => ua.ProductId);

            // Configuração para Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // Configuração para OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ProductId });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            // Configurações de tipo para propriedades decimal
            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasColumnType("decimal(18,4)"); 

            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18,4)"); 

            modelBuilder.Entity<Product>()
                .Property(p => p.Tax)
                .HasColumnType("decimal(18,4)"); 

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,4)"); 
        }
    }
}
