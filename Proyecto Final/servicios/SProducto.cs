using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    public class SProducto : IService
    {
        private const int ROUTER_REDIRECT = 4;

         string[] OPCIONES_ACTUALIZAR_PEDIDO = {
            "1) Nombre del producto",
            "2) Precio del producto",
            "3) Existencias",
        };

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
                case 2:
                    return this.actualizar();
                case 3:
                    return this.eliminar();
                default:
                    return 0;
            }
        }

        public int actualizar()
        {

            Menu.showMainLogo();

            this.mostrarProductos();

            int id = ConsoleHooks.askInt(
                "[red]Ingresa el id del producto a actualizar:[/]",
                "Ingresa un id valido",
                true,
                false
            );

            bool existe = existeProductoPorId(id);

            if( !existe )
            {
                
                ConsoleHooks.printRule("[red]Producto no valido[/]");

                return ROUTER_REDIRECT;

            }

            Producto producto = obtenerProducto(id);

            Menu.showMainLogo();

            ConsoleHooks.printRule("[red]Elige la opcion a actualizar[/]");

            string opcion = ConsoleHooks.askOpciones( OPCIONES_ACTUALIZAR_PEDIDO.ToList() , null );

            Menu.showMainLogo();

            switch( opcion )
            {
                case "1) Nombre del producto":
                    
                    string nombre_producto = ConsoleHooks.askString("[red]Ingresa el nombre del producto:[/]");
                    
                    producto.nombre = nombre_producto;
                    
                break;
                case "2) Precio del producto":
                    
                    double precio = ConsoleHooks.askDouble("[red]Ingresa el precio del producto:[/]");

                    producto.precio = precio;

                break;
                case "3) Existencias":
                    
                    int existencias_iniciales = ConsoleHooks.askInt("[red]Ingresa las existencias del producto:[/]");

                    producto.existencias_restantes = existencias_iniciales;

                break;
            }

            this.mostrarProducto(producto);

            bool confirmed = Menu.handleConfirm("¿Deseas actualizar el producto?");

            if( confirmed )
            {
                
                bool response = false;

                AnsiConsole.Status().Start("Actualizando el producto...", ctx =>
                {
                    response = this.actualizarProducto(producto);
                });

                Menu.showMainLogo();

                if( response )
                {
                    ConsoleHooks.printRule("[red]Producto actualizado correctamente[/]");
                }
                else
                {
                    ConsoleHooks.printRule("[red]No se pudo actualizar el producto[/]");
                }
                
                return ROUTER_REDIRECT;

            }

            return ROUTER_REDIRECT;

        }

        public int menuAgregarProducto()
        {
            
            Menu.showMainLogo();

            string nombre_producto = ConsoleHooks.askString("[red]Ingresa el nombre del producto:[/]");

            double precio = ConsoleHooks.askDouble("[red]Ingresa el precio del producto:[/]");
            
            int existencias_iniciales = ConsoleHooks.askInt("[red]Ingresa las existencias del producto:[/]");

            Producto producto = new Producto()
            {
                nombre = nombre_producto,
                precio = precio,
                existencias_iniciales = existencias_iniciales,
                existencias_restantes = existencias_iniciales
            };

            Menu.showMainLogo();

            this.mostrarProducto(producto);

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
       
        private bool actualizarProducto(Producto producto)
        {
            try
            {
                using(RestauranteDataContext dc = new RestauranteDataContext())
                {
                    dc.Productos.Update(producto);

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
                productos = dc.Productos.Where( producto => producto.existencias_restantes > 0 ).ToList();
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
                table.AddColumn("[yellow bold]Existencias[/]");
                table.AddColumn("[yellow bold]Fecha de creación[/]");

                Menu.showMainLogo();

                ConsoleHooks.printRule("[red]Productos[/]");

                table.BorderColor(Color.Yellow1);

                foreach (Producto producto in productos)
                {
                    table.AddRow(
                        $"[white]{producto.id.ToString()}[/]",
                        $"[white]{producto.nombre}[/]",
                        $"[white]${producto.precio}[/]",
                        $"[white]{producto.existencias_restantes}[/]",
                        $"[white]{producto.CreatedDate.ToString()}[/]"
                   );
                }

                AnsiConsole.Write(table);
                  
                return ROUTER_REDIRECT;

            }

        }

        private void mostrarProducto( Producto producto )
        {

            Table table = new Table().Expand().BorderColor(Color.Grey);

            table.Border(TableBorder.Rounded);

            if( producto.id != 0 )
            {
                table.AddColumn("[yellow bold]ID[/]");
            }
            
            table.AddColumn("[yellow bold]Nombre[/]");
            
            table.AddColumn("[yellow bold]Precio[/]");
            
            table.AddColumn("[yellow bold]Existencias[/]");
            
            if( producto.id != 0 )
            {
                table.AddColumn("[yellow bold]Fecha de creación[/]");
            }

            Menu.showMainLogo();

            ConsoleHooks.printRule("[red]Producto[/]");

            table.BorderColor(Color.Yellow1);

            if( producto.id != 0 )
            {
                table.AddRow(
                    $"[white]{producto.id.ToString()}[/]",
                    $"[white]{producto.nombre}[/]",
                    $"[white]${producto.precio}[/]",
                    $"[white]{producto.existencias_restantes}[/]",
                    $"[white]{producto.CreatedDate.ToString()}[/]"
                );
            }
            else
            {
                table.AddRow(
                    $"[white]{producto.nombre}[/]",
                    $"[white]${producto.precio}[/]",
                    $"[white]{producto.existencias_restantes}[/]"
                );
            }

            AnsiConsole.Write(table);

        }

        public int listar()
        {
            throw new NotImplementedException();
        }

        public int crear()
        {
            throw new NotImplementedException();
        }

        public bool existeProductoPorId( int id )
        {
            try
            {
                using (RestauranteDataContext dc = new RestauranteDataContext())
                {

                    Producto p = dc.Productos.Where( producto => producto.id == id).FirstOrDefault()!;

                    if( p != null ) return true;

                    return false;

                }
            }
            catch
            {
                return false;
            }
        }
        public int eliminar()
        {

            this.mostrarProductos();

            int id = ConsoleHooks.askInt(
                "[red]Ingresa el id del producto a eliminar:[/]" , 
                "Ingresa un id valido",
                true,
                false
            );

            if( !existeProductoPorId(id) )
            {

                ConsoleHooks.printRule("[red]El producto no se encuentra en la lista[/]");

                return ROUTER_REDIRECT;

            }

            Producto producto = new Producto();

            AnsiConsole.Status().Start("cargandp producto...", ctx =>
            {
                producto = obtenerProducto(id);
            });

            Menu.showMainLogo();

            this.mostrarProducto(producto);

            bool eliminar = Menu.handleConfirm("¿Deseas eliminar el producto?" , false);

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
