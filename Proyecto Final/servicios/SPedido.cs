using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    public class SPedido : IService
    {
        private const int ROUTER_REDIRECT = 1;
        string[] OPCIONES_MENU = {
            "1) Listar pedidos PENDIENTES",
            "2) Agregar pedido",
            "3) Actualizar pedido",
            "4) Cobrar pedido",
            "5) Salir",
        };

        private List<string> OPCIONES_COBRO = new List<string>() {
            "Efectivo",
            "Tarjeta",
        };

        public int mostrarMenu()
        {
            
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(this.OPCIONES_MENU));

            int seleccion = this.checkMenu(opt);

            return this.handleSeleccion(seleccion);

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

        public int listarVentas()
        {
            
            List<Pedido> pedidos_cobrados = new List<Pedido>();

            AnsiConsole.Status().Start("Cargando ventas...", ctx =>
            {
                pedidos_cobrados = this.obtenerPedidosPorStatus(1);
            });

            ConsoleHooks.printRule("[red]Ventas:[/]");

            this.renderTable(pedidos_cobrados);

            return 0;

        }

        public int cobrar()
        {

            this.listar();

            List<Pedido> pedidos_pendientes = new List<Pedido>();
            List<int> pedidos_ids = new List<int>();

            AnsiConsole.Status().Start("Cargando pedidos...", ctx =>
            {
                pedidos_pendientes = this.obtenerPedidosPorStatus(0);
                pedidos_ids = pedidos_pendientes.Select( pedido => pedido.id ).ToList();
            });

            int orden = ConsoleHooks.askNumero("Selecciona la orden que quieres cobrar: ");

            if( !pedidos_ids.Contains(orden) )
            {
                Menu.showMainLogo();
                ConsoleHooks.printRule("Orden no valida");
                return -1;
            }

            string cobro = ConsoleHooks.askOpciones(this.OPCIONES_COBRO , "[red]Selecciona el tipo de pago[/]");
            int tipo_cobro = Utilidades.getTipoCobro(cobro);

            bool response_tipo_cobro = this.actualizarTipoCobro( orden , tipo_cobro );
            bool response = this.actualizarStatusOrden( orden , 1 );

            if( response && response_tipo_cobro )
            {
                Menu.showMainLogo();
                ConsoleHooks.printRule("Orden cobrada");
                return ROUTER_REDIRECT;
            }

            Menu.showMainLogo();
            
            ConsoleHooks.printRule("No se pudo cobrar la orden");

            return ROUTER_REDIRECT;

        }

        private bool actualizarStatusOrden( int id , int status )
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
        
        private bool actualizarTipoCobro( int id , int tipo )
        {

            try
            {

                using (RestauranteDataContext dc = new RestauranteDataContext())
                {
                    
                    Pedido pedido = dc.Pedidos.Where( pedido => pedido.id == id ).FirstOrDefault()!;

                    pedido.tipo_cobro = tipo;

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

                // CREO EL PEDIDO

                Pedido pedido_nuevo = new Pedido()
                {
                    mesa = 0,
                    tipo_cobro = null
                };

                // ME TRAIGO LOS PRODUCTOS DE LA BASE DE DATOS
                List<string> producto_listado = new List<string>();
                
                AnsiConsole.Status().Start("Cargando productos...", ctx =>
                {
                    producto_listado = SProducto.obtenerProductosListado();
                });
                
                Menu.showMainLogo();
                
                ConsoleHooks.printRule("[red]Selecciona los productos de la orden:[/]");

                // SELECCIONAMOS EL PRODUCTO

                var producto = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .PageSize(3)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(producto_listado));

                pedido_nuevo.producto = producto[0];


                // SELECCIONAMOS EL TIPO DE PEDIDO

                string tipo_pedido = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Selecciona el tipo de pedido:")
                        .PageSize(3)
                        .AddChoices(new[] {
                            "1) Comer aqui", "2) Para llevar",
                        }));

                // SI ES PARA COMER AQUI

                if( tipo_pedido == "1) Comer aqui" ) {

                    pedido_nuevo.tipo_pedido = 0;

                    int mesa = ConsoleHooks.askNumero("Ingresa el numero de mesa: ");

                    pedido_nuevo.mesa = mesa;

                }
                else
                {
                    pedido_nuevo.tipo_pedido = 1;
                }

                // PEDIMOS EL IMPORTE

                float importe = ConsoleHooks.askDecimal("[red]Ingresa el importe del pedido:[/]");
                
                pedido_nuevo.importe = importe;

                pedido_nuevo.iva = (float)(importe * 0.16);
                
                pedido_nuevo.total = (float)(importe + pedido_nuevo.iva);

                // 0 - PENDIENTE
                // 1 - TERMINADO
                
                // DECLARO QUE SEA 0 PORQUE SIGNIFICA 'PENDIENTE'

                pedido_nuevo.status = 0;

                Menu.showMainLogo();

                ConsoleHooks.printRule("[red]Pedido:[/]");

                // AGREGAMOS LA TABLA

                var table_pedido = new Table();

                table_pedido.BorderColor(Color.Yellow1);
                table_pedido.Border(TableBorder.Rounded);
                table_pedido.Expand();

                table_pedido.AddColumn("Numero Orden");
                table_pedido.AddColumn("Tipo Pedido");
                table_pedido.AddColumn("Status");
                table_pedido.AddColumn("Mesa");
                table_pedido.AddColumn("Producto");
                table_pedido.AddColumn("Importe");
                table_pedido.AddColumn("IVA");
                table_pedido.AddColumn("Total");

                int numero_orden = obtenerUltimoIdOrden();

                table_pedido.AddRow(
                    $"{numero_orden}",
                    $"{Utilidades.renderTipoPedido(pedido_nuevo.tipo_pedido)}",
                    $"{Utilidades.renderStatusPedido(pedido_nuevo.status)}",
                    $"{pedido_nuevo.mesa}",
                    $"{pedido_nuevo.producto}",
                    $"{Utilidades.renderDinero(pedido_nuevo.importe)}",
                    $"{Utilidades.renderDinero(pedido_nuevo.iva)}",
                    $"{Utilidades.renderDinero(pedido_nuevo.total)}"
                );
                
                AnsiConsole.Write(table_pedido);

                Console.WriteLine("");

                bool response = this.agregarPedido( pedido_nuevo );

                if( response )
                {

                    ConsoleHooks.printRule("[red]Pedido agregado correctamente[/]");

                    return 1;

                }

                ConsoleHooks.printRule("[yellow]Error: no se pudo agregar el pedido[/]");

            }
            catch( Exception e )
            {
                
                System.Console.WriteLine(e);
                System.Console.WriteLine(e.Message);

                ConsoleHooks.printRule("[yellow]Error: no se pudo agregar el pedido[/]");

                return ROUTER_REDIRECT;

            }

            return ROUTER_REDIRECT;

        }

        private bool agregarPedido( Pedido pedido )
        {

            try
            {
                
                using (RestauranteDataContext dc = new RestauranteDataContext())
                {

                    dc.Pedidos.Add(pedido);

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

        public void renderTable( List<Pedido> pedidos_pendientes , bool isPago = true )
        {
            
            var table = new Table();
            
            table.Expand();

            var table_pedidos = new Table()
                .BorderColor(Color.Yellow1)
                .Border(TableBorder.Rounded)
                .Centered()
                .Expand()
                .AddColumn(new TableColumn("[bold yellow]Numero Pedido[/]"))
                .AddColumn(new TableColumn("[bold yellow]Tipo[/]"))
                .AddColumn(new TableColumn("[bold yellow]Status[/]"))
                .AddColumn(new TableColumn("[bold yellow]Mesa[/]"))
                .AddColumn(new TableColumn("[bold yellow]Producto[/]"))
                .AddColumn(new TableColumn("[bold yellow]Importe[/]"))
                .AddColumn(new TableColumn("[bold yellow]IVA[/]"))
                .AddColumn(new TableColumn("[bold yellow]Total[/]"));

            if( isPago )
            {
                table_pedidos.AddColumn(new TableColumn("[bold yellow]Tipo de pago[/]"));
            }

            foreach (Pedido pedido in pedidos_pendientes)
            {
                if( isPago )
                {
                    table_pedidos.AddRow(
                        new Markup($"[bold]{pedido.id}[/]"),
                        new Markup($"[bold]{Utilidades.renderTipoPedido(pedido.tipo_pedido)}[/]"),
                        new Markup($"[bold]{Utilidades.renderStatusPedido(pedido.status)}[/]"),
                        new Markup($"[bold]{pedido.mesa}[/]"),
                        new Markup($"[bold]{pedido.producto}[/]"),
                        new Markup($"[bold]{Utilidades.renderDinero(pedido.importe)}[/]"),
                        new Markup($"[bold]{Utilidades.renderDinero(pedido.iva)}[/]"),
                        new Markup($"[bold]{Utilidades.renderDinero(pedido.total)}[/]"),
                        new Markup($"[bold]{Utilidades.renderTipoCobro(pedido.tipo_cobro)}[/]")
                    );
                }
                else
                {
                    table_pedidos.AddRow(
                        new Markup($"[bold]{pedido.id}[/]"),
                        new Markup($"[bold]{Utilidades.renderTipoPedido(pedido.tipo_pedido)}[/]"),
                        new Markup($"[bold]{Utilidades.renderStatusPedido(pedido.status)}[/]"),
                        new Markup($"[bold]{pedido.mesa}[/]"),
                        new Markup($"[bold]{pedido.producto}[/]"),
                        new Markup($"[bold]{Utilidades.renderDinero(pedido.importe)}[/]"),
                        new Markup($"[bold]{Utilidades.renderDinero(pedido.iva)}[/]"),
                        new Markup($"[bold]{Utilidades.renderDinero(pedido.total)}[/]")
                    );
                }
            }

            AnsiConsole.Write(table_pedidos);

        }

        public int listar()
        {

            List<Pedido> pedidos_pendientes = new List<Pedido>();

            AnsiConsole.Status().Start("Cargando pedidos...", ctx =>
            {
                pedidos_pendientes = this.obtenerPedidosPorStatus(0);
            });

            ConsoleHooks.printRule("[red]Pedidos pendientes:[/]");

            this.renderTable(pedidos_pendientes , false );            

            return ROUTER_REDIRECT;

        }


        private List<Pedido> obtenerPedidosPorStatus( int status = 0 )
        {

            List<Pedido> pedidos = new List<Pedido>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

               pedidos = dc.Pedidos.Where(pedido => pedido.status == status ).ToList();

               return pedidos;

            }

        }

        private int obtenerUltimoIdOrden()
        {
            try
            {
                using (RestauranteDataContext dc = new RestauranteDataContext())
                {
                    Pedido pedido = dc.Pedidos.OrderBy(x=>x.id).LastOrDefault()!;

                    return pedido.id + 1;
                }
            }
            catch
            {
                return 1;
            }
        }
       
    }
}
