using Microsoft.EntityFrameworkCore;
using System;

namespace Proyecto_Final.clases
{
    public class RestauranteDataContext : DbContext
    {
        static readonly string connectionString = "Server=localhost;port=5506;User ID=root; Password=12345678; Database=restaurante_poo_test";

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Componente> Componentes { get; set; }

        public DbSet<Pedido_tiene_productos> pedido_tiene_productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Pedido_tiene_productos)
                .WithOne();
        }
    }
}

