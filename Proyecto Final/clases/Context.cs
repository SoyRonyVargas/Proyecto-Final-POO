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
        public DbSet<Producto_tiene_componentes> producto_tiene_componentes { get; set; }
        public DbSet<Entrada> Entradas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
            catch
            {
                Console.WriteLine("Error al conectar la base de datos");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

    }
}

