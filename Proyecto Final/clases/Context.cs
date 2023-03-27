using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace Proyecto_Final.clases
{
    public class RestauranteDataContext : DbContext
    {
        static readonly string connectionString = "Server=localhost;port=5506;User ID=root; Password=12345678; Database=restaurante_poo_test";

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {

                // SI OCUPAS SQLSERVER USA ESTA
                optionsBuilder.UseSqlServer("Server=DESKTOP-KPGRKDT;Database=la_delicia ;Trusted_Connection=SSPI;MultipleActiveResultSets=true;Trust Server Certificate=true");
                
                // SI OCUPAS MYSQL USA ESTA
              

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

