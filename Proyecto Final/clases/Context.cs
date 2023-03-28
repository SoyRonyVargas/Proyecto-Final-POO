using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.clases
{
    public class RestauranteDataContext : DbContext
    {
        static readonly string connectionString = "Server=localhost;port=5506;User ID=root; Password=12345678; Database=restaurante_poo_test";

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {

                // SI OCUPAS SQLSERVER USA ESTA
                optionsBuilder.UseSqlServer("Server=localhost;Database=la_delicia ;Trusted_Connection=SSPI;MultipleActiveResultSets=true;Trust Server Certificate=true");
                
                // optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                
                // SI OCUPAS MYSQL USA ESTA
              

            }
            catch
            {
                Console.WriteLine("Error al conectar la base de datos");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    id = 1,
                    apellidos = "Gomez",
                    nombre = "Adamaris",
                    correo = "prueba@gmail.com",
                    password = "1234"
                }
            );
            
            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    id = 1,
                    nombre = "Hamburguesa con queso",
                    precio = 99.99
                }
            );
            
            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    id = 2,
                    nombre = "Coca cola 600ml",
                    precio = 19.99
                }
            );
            
            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    id = 3,
                    nombre = "Pepsi 600ml",
                    precio = 15.99
                }
            );
            
            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    id = 4,
                    nombre = "Papas fritas medianas",
                    precio = 49.99
                }
            );
            
            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    id = 5,
                    nombre = "Nuggets de pollo (6 piezas)",
                    precio = 89.99
                }
            );

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

