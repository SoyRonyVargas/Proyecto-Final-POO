using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    public class SProducto : IService
    {
        private const int ROUTER_REDIRECT = 4;
        public int mostrarMenu()
        {
            
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1) Listar productos",
                        "2) Agregar producto",
                        "3) Eliminar producto",
                        "4) Salir",
                    }));

            int seleccion = this.checkMenu(opt);

            return this.handleSeleccion(seleccion);

        }

        private int checkMenu(string opcion)
        {

            switch (opcion)
            {
                case "1) Listar productos":
                    return 0;
                case "2) Agregar producto":
                    return 1;
                case "3) Eliminar producto":
                    return 3;
                case "4) Salir":
                    return -1;
            }

            return -1;

        }

        private int handleSeleccion(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    return this.mostrarProductos();
                case 1:
                    return this.menuAgregarProducto();
                case 3:
                    return this.eliminar();
                default:
                    return 0;
            }
        }

        

        public int menuAgregarProducto()
        {
            
            var nombre_producto = AnsiConsole.Ask<string>("[green]Ingresa el nombre del producto:[/]");
            
            float precio = AnsiConsole.Ask<float>("[green]Ingresa el precio del producto:[/]");
            
            Producto producto = new Producto()
            {
                nombre = nombre_producto,
                precio = precio
            };

            Menu.showMainLogo();

            bool opcion = Menu.handleConfirm("¿Deseas guardar el producto?");

            if( opcion )
            {
                
                bool response = false;

                AnsiConsole.Status().Start("Guardando producto...", ctx =>
                {
                    response = this.agregarProducto(producto);
                });

                Menu.showMainLogo();

                if( response )
                {
                    ConsoleHooks.printRule("[red]Producto agregado correctamente[/]");
                }
                else
                {
                    ConsoleHooks.printRule("[red]No se pudo agregar el producto[/]");
                }
                
                return ROUTER_REDIRECT;

            }

            return ROUTER_REDIRECT;

        }

        private bool agregarProducto(Producto producto)
        {
            try
            {
                using(RestauranteDataContext dc = new RestauranteDataContext())
                {
                    dc.Productos.Add(producto);

                    dc.SaveChanges();

                    return true;

                }
            }
            catch
            {
                return false;
            }
        }

        public static Producto obtenerProducto( int id )
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                Producto producto = dc.Productos.Where( p => p.id == id ).FirstOrDefault()!;   
                return producto;
            }
        }
        public static List<Producto> obtenerProductos()
        {

            List<Producto> productos = new List<Producto>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                productos = dc.Productos.ToList();
                
                AnsiConsole.Status().Start("Cargando productos...", ctx =>
                {

                    Thread.Sleep(500);

                    productos = dc.Productos.ToList();

                });
            }

            return productos;

        }

        public static List<string> obtenerProductosListado()
        {

            List<Producto> productos = new List<Producto>();
            List<string> productos_listado = new List<string>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                productos = dc.Productos.ToList();
            }

            productos_listado = productos.Select(producto => producto.nombre).ToList();

            return productos_listado;

        }

        private int mostrarProductos()
        {

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

                List<Producto> productos = new List<Producto>();

                AnsiConsole.Status().Start("Cargando productos...", ctx =>
                {
                    
                    productos = dc.Productos.ToList();

                    Thread.Sleep(500);

                });

                Table table = new Table().Expand().BorderColor(Color.Grey);

                table.Border(TableBorder.Rounded);

                table.AddColumn("[yellow bold]ID[/]");
                table.AddColumn("[yellow bold]Nombre[/]");
                table.AddColumn("[yellow bold]Precio[/]");

                Menu.showMainLogo();

                ConsoleHooks.printRule("[red]Productos[/]");

                table.BorderColor(Color.Yellow1);

                foreach (Producto producto in productos)
                {
                    table.AddRow(
                        $"[white]{producto.id.ToString()}[/]",
                        $"[white]{producto.nombre}[/]",
                        $"[white]${producto.precio}[/]"
                   );
                }

                AnsiConsole.Write(table);
                  
                return ROUTER_REDIRECT;

            }

        }

        public int listar()
        {
            throw new NotImplementedException();
        }

        public int crear()
        {
            throw new NotImplementedException();
        }

        public int eliminar()
        {

            this.mostrarProductos();

            int id = ConsoleHooks.askNumero("[red]Ingresa el id del producto a eliminar:[/]");

            bool eliminar = Menu.handleConfirm("¿Deseas eliminar el producto?");

            if (eliminar)
            {

                bool response = false;

                AnsiConsole.Status().Start("eliminando producto...", ctx =>
                {
                    response = this.eliminarProducto(id);
                });

                Menu.showMainLogo();

                if (response)
                {
                    ConsoleHooks.printRule("[red]Producto eliminado correctamente[/]");
                }
                else
                {
                    ConsoleHooks.printRule("[red]No se pudo eliminar el producto[/]");
                }

                return ROUTER_REDIRECT;
            }
            
            return ROUTER_REDIRECT;

        }

        private bool eliminarProducto(int idProducto)
        {
            try
            {
                using (RestauranteDataContext dc = new RestauranteDataContext())
                {

                    Producto p = dc.Productos.Where( producto => producto.id == idProducto).FirstOrDefault()!;

                    dc.Productos.Remove(p);

                    dc.SaveChanges();

                    return true;

                }
            }
            catch
            {
                return false;
            }
        }

    }
    
}
