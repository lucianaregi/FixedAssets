using FixedAssets.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FixedAssets.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets representam as tabelas do banco de dados
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais (opcionais)
            // Aqui você pode aplicar configurações para as entidades
            //modelBuilder.ApplyConfiguration(new OrderConfiguration());  // Configuração da entidade Order
            // Adicione outras configurações, se necessário
        }
    }
}
