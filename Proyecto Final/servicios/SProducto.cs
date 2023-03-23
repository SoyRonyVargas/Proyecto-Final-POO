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
                        "3) Actualizar producto",
                        "4) Eliminar producto",
                        "5) Salir",
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
                case "3) Actualizar producto":
                    return 2;
                case "4) Eliminar producto":
                    return 3;
                case "5) Salir":
                    return 4;
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
                default:
                    return 0;
            }
        }

        

        public int menuAgregarProducto()
        {
            
            var nombre_producto = AnsiConsole.Ask<string>("[green]Ingresa el nombre del producto[/]?");
            
            Producto producto = new Producto()
            {
                nombre = nombre_producto,
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

                AnsiConsole.Status().Start("Cargando productos...", ctx =>
                {

                    Thread.Sleep(500);

                    productos = dc.Productos.ToList();

                });
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

                var table = new Table().Expand().BorderColor(Color.Grey);

                table.AddColumn("[yellow bold]ID[/]");
                table.AddColumn("[yellow bold]Nombre[/]");

                Menu.showMainLogo();

                ConsoleHooks.printRule("[red]Productos[/]");

                AnsiConsole.Live(table).AutoClear(false)
                    .Start(ctx =>
                    {

                        table.Columns[0].Header("[yellow bold]ID[/]");

                        table.Columns[1].Header("[yellow bold]Nombre[/]");


                        table.BorderColor(Color.Yellow1);

                        foreach (Producto producto in productos)
                        {
                            table.AddRow(
                                $"[white]{producto.id.ToString()}[/]",
                                $"[white]{producto.nombre}[/]"
                           );
                        }

                    });

                return ROUTER_REDIRECT;

            }

        }

        protected int eleminar()
        {

            var rule = new Rule("[red]Selecciona los productos que vas a eliminar[/] \n").LeftJustified();

            //AnsiConsole.Write(rule);

            //List<Componente> componentes_seleccionados = new List<Componente>();

            //componentes_seleccionados = SComponente.seleccionarComponentes();

            return ROUTER_REDIRECT;

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
            throw new NotImplementedException();
        }
    }
    
}
