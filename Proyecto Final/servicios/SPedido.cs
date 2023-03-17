using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    public class SPedido : IService
    {

        string[] OPCIONES_MENU = {
            "1) Listar pedidos PENDIENTES",
            "2) Agregar pedido",
            "3) Actualizar pedido",
            "4) Cobrar pedido",
            "5) Salir",
        };

        public int mostrarMenu()
        {
            
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(this.OPCIONES_MENU));

            int seleccion = this.checkMenu(opt);

            this.handleSeleccion(seleccion);

            return -1;

        }

        private int checkMenu(string opcion)
        {
            switch (opcion)
            {
                case "1) Listar pedidos PENDIENTES":
                    return 0;
                case "2) Agregar pedido":
                    return 1;
                case "3) Actualizar pedido":
                    return 2;
                case "4) Cobrar pedido":
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
                    return this.listar();
                case 1:
                    return this.crear();
                case 3:
                    return this.cobrar();
                default:
                    return 0;
            }
        }

        private string calcularTotalOrden(List<Producto> productos, List<int> cantidades)
        {

            float total = 0;
            int i = 0;

            foreach(Producto producto in productos )
            {

                int cantidad_producto = cantidades[i];

                float importe = (cantidad_producto * producto.precio);

                total += importe;

                i++;

            }

            return total.ToString("0.00");

        }

        private string calcularImporteConcepto(Producto producto, int cantidad)
        {

            float importe = (cantidad * producto.precio);

            return importe.ToString("0.00");

        }

        public int listarVentas()
        {
            
            List<PedidoFull> pedidos_cobrados = new List<PedidoFull>();

            AnsiConsole.Status().Start("Cargando ventas...", ctx =>
            {
                pedidos_cobrados = this.obtenerPedidosPorStatus(1);
            });

            ConsoleHooks.printRule("[red]Ventas:[/]");

            this.renderTable(pedidos_cobrados);

            return 1;

        }

        public int cobrar()
        {

            this.listar();

            List<PedidoFull> pedidos_pendientes = new List<PedidoFull>();
            List<int> pedidos_ids = new List<int>();

            AnsiConsole.Status().Start("Cargando pedidos...", ctx =>
            {
                pedidos_pendientes = this.obtenerPedidosPorStatus(0);
                pedidos_ids = pedidos_pendientes.Select( pedido => pedido.pedido.id ).ToList();
            });

            int orden = ConsoleHooks.askNumero("Selecciona la orden que quieres cobrar: ");

            if( !pedidos_ids.Contains(orden) )
            {
                Menu.showMainLogo();
                ConsoleHooks.printRule("Orden no valida");
                return -1;
            }

            bool response = actualizarStatusOrden( orden , 1 );

            if( response )
            {
                Menu.showMainLogo();
                ConsoleHooks.printRule("Orden cobrada");
                return -1;   
            }

            Menu.showMainLogo();
            
            ConsoleHooks.printRule("No se pudo cobrar la orden");

            return 6;

        }

        public bool actualizarStatusOrden( int id , int status )
        {

            try
            {

                using (RestauranteDataContext dc = new RestauranteDataContext())
                {
                    
                    Pedido pedido = dc.Pedidos.Where( pedido => pedido.id == id ).FirstOrDefault()!;

                    pedido.status = status;

                    dc.SaveChanges();

                    return true;

                }

            }
            catch
            {
                return false;
            }

        }

        public int crear()
        {
            
            try
            {

                Menu.showMainLogo();

                Pedido pedido_nuevo = new Pedido()
                {
                    mesa = 0
                };

                ConsoleHooks.printRule("[red]Selecciona los productos de la orden[/]");

                List<string> producto_listado = SProducto.obtenerProductosListado();
                List<Producto> productos = SProducto.obtenerProductos();

                // SELECCIONAMOS LOS PRODUCTOS

                var selecciones = AnsiConsole.Prompt(
                           new MultiSelectionPrompt<string>()
                               .Required()
                               .AddChoices(producto_listado));

                List<Producto> productos_seleccionados = new List<Producto>();
                List<int> cantidades = new List<int>();

                // AGREGAMOS A LA LISTA LOS PRODUCTOS SELECCIONADOS
                // Y INGRESAMOS LA CANTIDAD DE CADA PRODUCTO

                foreach (string seleccionado in selecciones)
                {

                    Producto cmp = productos.Where(c => c.nombre == seleccionado ).FirstOrDefault()!;

                    int cantidad = ConsoleHooks.askNumero($"Ingresa la cantidad de {cmp.nombre}:");

                    cantidades.Add(cantidad);

                    productos_seleccionados.Add(cmp);

                }

                // SELECCIONAMOS EL TIPO DE PEDIDO

                var tipo_pedido = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Selecciona el tipo de pedido:")
                        .PageSize(3)
                        .AddChoices(new[] {
                            "1) Comer aqui", "2) Para llevar",
                        }));

                // SI ES PARA COMER AQUI

                if( tipo_pedido == "1) Comer aqui" ) {

                    pedido_nuevo.tipo_pedido = 0;

                    int mesa = AnsiConsole.Prompt(
                        new TextPrompt<int>("[red]Ingresa el numero de mesa:[/]")
                            .PromptStyle("red")
                            .ValidationErrorMessage("[red]Ingresa una mesa valida[/]")
                            .Validate(age =>
                            {
                                return age switch
                                {
                                    <= 0 => ValidationResult.Error("[red]Ingresa una mesa valida[/]"),
                                    _ => ValidationResult.Success(),
                                };
                            }));

                    pedido_nuevo.mesa = mesa;

                }
                else
                {
                    pedido_nuevo.tipo_pedido = 1;
                }

                pedido_nuevo.status = 0;

                Menu.showMainLogo();

                ConsoleHooks.printRule("[red]Pedido:[/]");

                // AGREGAMOS LA TABLA

                var table_pedido = new Table();

                table_pedido.BorderColor(Color.Yellow1);
                table_pedido.Expand();

                table_pedido.AddColumn("Numero Orden");
                table_pedido.AddColumn("Tipo Pedido");
                table_pedido.AddColumn("Status");
                table_pedido.AddColumn("Mesa");

                table_pedido.AddRow(
                    $"1",
                    $"{(pedido_nuevo.tipo_pedido == 0 ? "Comer Aqui" : "Para llevar")}",
                    $"Pendiente",
                    $"9"
                );
                
                AnsiConsole.Write(table_pedido);

                Console.WriteLine("");
                Console.WriteLine("");

                ConsoleHooks.printRule("[red]Productos del pedido:[/]");

                var table = new Table().Expand().RightAligned();

                table.BorderColor(Color.Yellow1);

                AnsiConsole.Live(table)
                    .AutoClear(false)
                    .Overflow(VerticalOverflow.Ellipsis)
                    .Cropping(VerticalOverflowCropping.Top)
                    .Start(ctx =>
                    {

                        table.AddColumn("[bold]Producto[/]");
                        table.AddColumn("[bold]Cantidad[/]");
                        table.AddColumn("[bold]Precio Pieza[/]");
                        table.AddColumn("[bold]Importe[/]");

                        table.Columns[0].Header("[bold]Producto[/]");
                        table.Columns[1].Header("[bold]Cantidad[/]");
                        table.Columns[2].Header("[bold]Precio Pieza[/]");
                        table.Columns[3].Header("[bold]Importe[/]");

                        int i = 0;

                        string total_orden = this.calcularTotalOrden(productos_seleccionados, cantidades);

                        foreach ( Producto producto in productos_seleccionados )
                        {
                            int cantidad_producto = cantidades[i];

                            string importe = calcularImporteConcepto(producto, cantidad_producto );

                            table.AddRow(
                                $"[bold]{producto.nombre}[/]",
                                $"[bold]{cantidad_producto}[/]",
                                $"[bold]${producto.precio}[/]",
                                $"[bold]${importe}[/]"
                            );

                            i++;

                        }

                        table.Columns[0].Footer("");
                        table.Columns[1].Footer("");
                        table.Columns[2].Footer("");
                        table.Columns[3].Footer($"[red bold]Total: ${total_orden}[/]");

                    });

                Console.WriteLine("");
                Console.WriteLine("");

                bool response = this.agregarPedido( pedido_nuevo , productos_seleccionados , cantidades );

                if( response )
                {

                    ConsoleHooks.printRule("[red]Pedido agregado correctamente[/]");

                    return 1;

                }

                ConsoleHooks.printRule("[yellow]Error: no se pudo agregar el pedido[/]");

            }
            catch
            {

            }

            return -1;

        }

        private bool agregarPedido( Pedido pedido , List<Producto> productos , List<int> cantidades )
        {

            try
            {
                
                using (RestauranteDataContext dc = new RestauranteDataContext())
                {

                    dc.Pedidos.Add(pedido);

                    dc.SaveChanges();

                    int i = 0;

                    foreach (Producto producto in productos)
                    {

                        int cantidad = cantidades[i];

                        // Console.WriteLine("cantidad del producto");
                        // Console.WriteLine(cantidad);

                        Pedido_tiene_productos elemento = new Pedido_tiene_productos()
                        {
                            cantidad = cantidad,
                            id_producto = producto.id,
                            id_pedido = pedido.id
                        };

                        dc.pedido_tiene_productos.Add(elemento);

                        i++;

                    }

                    dc.SaveChanges();

                }

                return true;

            }
            catch
            {
                return false;
            }

        }

        public static string checkTipoPedido( int tipo )
        {
            if( tipo == 0 )
            {
                return "Para comer aqui";
            }

            return "Para llevar";
        }

        public static string checkStatus( int status )
        {
            if( status == 0 )
            {
                return "Pendiente";
            }
            
            if( status == 1 )
            {
                return "Terminado/Cobrado";
            }

            return "Cancelado";
        }

        public int eliminar()
        {
            throw new NotImplementedException();
        }

        public void renderTable( List<PedidoFull> pedidos_pendientes )
        {
            
            var table = new Table();
            
            table.Expand();

            table.AddColumn(new TableColumn("[u]Productos[/]"));

            var table_pedidos = new Table()
                .BorderColor(Color.Yellow1)
                .Border(TableBorder.Rounded)
                .Centered()
                .Expand()
                .AddColumn(new TableColumn("[bold u]Numero Pedido[/]"))
                .AddColumn(new TableColumn("[bold u]Tipo[/]"))
                .AddColumn(new TableColumn("[bold u]Mesa[/]"))
                .AddColumn(new TableColumn("[bold u]Status[/]"))
                .AddColumn(new TableColumn("[bold u]Productos[/]"));


            foreach (PedidoFull pedido in pedidos_pendientes)
            {

                var tabla_productos = new Table()
                    .BorderColor(Color.Green)
                    .Border(TableBorder.Rounded)
                    .Expand()
                    .AddColumn(new TableColumn("[u]Producto[/]"))
                    .AddColumn(new TableColumn("[u]Cantidad[/]"));

                int i = 0;

                foreach( Producto producto in pedido.productos )
                {

                    int cantidad = pedido.pedido_tiene_productos[i].cantidad;

                    tabla_productos.AddRow(
                        $"[blue]{producto.nombre}[/]",
                        $"[yellow]{cantidad}[/]"
                    );

                    i++;

                }

                table_pedidos.AddRow(
                    new Markup($"[u]{pedido.pedido.id}[/]"),
                    new Markup($"[u]{checkTipoPedido(pedido.pedido.tipo_pedido)}[/]"),
                    new Markup($"[u]{pedido.pedido.mesa}[/]"),
                    new Markup($"[u]{checkStatus(pedido.pedido.status)}[/]"),
                    tabla_productos
                );
            }

            AnsiConsole.Write(table_pedidos);

        }

        public int listar()
        {

            List<PedidoFull> pedidos_pendientes = new List<PedidoFull>();

            AnsiConsole.Status().Start("Cargando pedidos...", ctx =>
            {
                pedidos_pendientes = this.obtenerPedidosPorStatus(0);
            });

            ConsoleHooks.printRule("[red]Pedidos pendientes:[/]");

            this.renderTable(pedidos_pendientes);            

            return -1;

        }


        private List<PedidoFull> obtenerPedidosPorStatus( int status = 0 )
        {

            List <PedidoFull> pedidos = new List<PedidoFull>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

               List<Pedido> pedidos_pendientes = dc.Pedidos.Where(pedido => pedido.status == status ).ToList();

               foreach( Pedido _pedido in pedidos_pendientes )
               {
                    
                    PedidoFull pedido = new PedidoFull();

                    List<Pedido_tiene_productos> productos_por_pedido = dc.pedido_tiene_productos.Where(pedido => pedido.id_pedido == _pedido.id ).ToList();
                    
                    List<Producto> productos_pedido = new List<Producto>();
                    
                    pedido.pedido_tiene_productos = productos_por_pedido;

                    foreach(Pedido_tiene_productos pedido_tiene_productos in productos_por_pedido)
                    {

                        Producto producto = dc.Productos.Where(p => p.id == pedido_tiene_productos.id_producto).FirstOrDefault()!;

                        productos_pedido.Add(producto);

                    }

                    pedido.productos = productos_pedido;

                    pedido.pedido = _pedido;

                    pedidos.Add(pedido);

               }

               return pedidos;

            }

        }
       
    }
}
