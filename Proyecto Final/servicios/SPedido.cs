using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    // LA CLASE TIENE QUE TENER LOS METODOS DE LA INTERFAZ
    public class SPedido : IService
    {
        
        // CONSTANE PARA REDIRIGIR AL MISMO MENU
        private const int ROUTER_REDIRECT = 1;
        
        // ARREGLO DE OPCIONES DEL MENU
        string[] OPCIONES_MENU = {
            "1) Listar pedidos PENDIENTES",
            "2) Agregar pedido",
            "3) Actualizar pedido",
            "4) Cobrar pedido",
            "5) Salir",
        };

        // OPCIONES DE COBRO
        private List<string> OPCIONES_COBRO = new List<string>() {
            "Efectivo",
            "Tarjeta",
        };

        // FUNCION DE LA INTERFAZ DE MOSTRAR MENU
        public int mostrarMenu()
        {
            
            // RENDERIZAMOS LAS OPCIONES DEL MENU
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(this.OPCIONES_MENU));

            // TRANSFORMAMOS LA OPCION STRING A ENTERO
            int seleccion = this.checkMenu(opt);

            // EJECUTAMOS LA OPCION SELECCIONADA
            return this.handleSeleccion(seleccion);

        }

        // FUNCION QUE TRANSFORMA LA OPCION A ENTERO
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

        // FUNCION QUE CHECA QUE OPCION ELIGIO EL USUARIO
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

        public int mostrarMenuPlatillos()
        {
            
            ConsoleHooks.printRule("[red]Carta:[/]");

            List<Producto> productos = SProducto.obtenerProductos();

            var carta = new Table();

            carta.BorderColor(Color.Yellow1);
            
            carta.Border(TableBorder.Rounded);
            
            // carta.Expand();
            
            carta.AddColumn("[bold]Nombre producto[/]");

            // AGREGAMOS UNA FILA CON LOS DATOS DEL PEDIDO

            foreach( Producto producto in productos )
            {
                carta.AddRow(
                    $"{producto.nombre}"
                );
            }

            AnsiConsole.Write(carta);

            return -1;

        }

        // FUNCION QUE LISTA LAS VENTAS (PEDIDOS CON STATUS 1)
        public int listarVentas()
        {
            
            // CREAMOS UNA LISTA VACIA
            List<Pedido> pedidos_cobrados = new List<Pedido>();

            AnsiConsole.Status().Start("Cargando ventas...", ctx =>
            {
                // NOS TRAEMOS LOS PEDIDOS CON STATUS 1 "TERMINADO"
                pedidos_cobrados = this.obtenerPedidosPorStatus(1);
            });

            ConsoleHooks.printRule("[red]Ventas:[/]");

            // PINTAMOS LA TABLA
            this.renderTable(pedidos_cobrados);

            return -1;

        }

        // FUNCION QUE CAMBIA EL STATUS DEL PEDIDO DE 0 A 1 ( DE PENDIENTE A TERMINADO )
        public int cobrar()
        {

            // LISTAMOS LOS PEDIDOS PENDIENTES CON STATUS 0
            this.listar();

            List<Pedido> pedidos_pendientes = new List<Pedido>();
            List<int> pedidos_ids = new List<int>();

            AnsiConsole.Status().Start("Cargando pedidos...", ctx =>
            {
                // OBTENER LOS PEDIDOS POR STATUS 0 OSEA PENDIENTES
                pedidos_pendientes = this.obtenerPedidosPorStatus(0);
                // OBTENEMOS LOS PEDIDOS POR SUS IDS
                pedidos_ids = pedidos_pendientes.Select( pedido => pedido.id ).ToList();
            });

            // PEDIMOS AL USUARIO QUE NOS INGRESE EL ID DEL PEDIDO
            int orden = ConsoleHooks.askNumero("Selecciona la orden que quieres cobrar: ");

            // SI NO EXISTE
            if( !pedidos_ids.Contains(orden) )
            {
                Menu.showMainLogo();
                ConsoleHooks.printRule("Orden no valida");
                return -1;
            }

            // SI EXISTE EL PEDIDO...

            // MOSTRAMOS QUE ELIGA UNA OPCION DE PAGO
            // TARJETA O EFECTIVO
            string cobro = ConsoleHooks.askOpciones(this.OPCIONES_COBRO , "[red]Selecciona el tipo de pago[/]");

            // LO TRANSFORMAMOS A ENTERO
            int tipo_cobro = Utilidades.getTipoCobro(cobro);

            // ACTUALIZAMOS LA ORDEN CON EL TIPO DE COBRO
            bool response_tipo_cobro = this.actualizarTipoCobro( orden , tipo_cobro );
            
            // ACTUALIZAMOS LA ORDEN CON EL STATUS DE TERMINADO
            bool response = this.actualizarStatusOrden( orden , 1 );

            // SI SE ACTUALIZO BIEN TERMINO
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

        // FUNCION PARA ACTUALIZAR EL STATUS DE LA ORDEN
        // PARAMETRO 1 - ID DE LA ORDEN A ACTUALIZAR
        // PARAMETROS 2 - STATUS A ACTUALIZAR
        private bool actualizarStatusOrden( int id , int status )
        {

            try
            {

                using (RestauranteDataContext dc = new RestauranteDataContext())
                {
                    
                    // TRAEMOS EL PEDIDO POR ID
                    
                    Pedido pedido = dc.Pedidos.Where( pedido => pedido.id == id ).FirstOrDefault()!;

                    // LE ACTUALIZAMOS EL STATUS

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

        // FUNCION PARA ACTUALIZAR EL STATUS DE LA ORDEN
        // PARAMETRO 1 - ID DE LA ORDEN A ACTUALIZAR
        // PARAMETROS 2 - TIPO DE COBRO A ACTUALIZAR
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
        // FUNCION QUE CREA EL PEDIDO
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

                var producto = ConsoleHooks.askOpciones(producto_listado);

                pedido_nuevo.producto = producto;

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
                    
                    // ASIGNAMOS QUE ES PARA COMER AQUI
                    pedido_nuevo.tipo_pedido = 0;
                    
                    // PREGUNTAMOS EL NUMERO DE ESA Y LO ASIGNAMOS
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

                // CALCULAMOS EL IVA 16%
                pedido_nuevo.iva = (float)(importe * 0.16);
                
                // SUMAMOS IVA Y IMPORTE
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

                // AGREGAMOS UNA FILA CON LOS DATOS DEL PEDIDO

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

        // FUNCION QUE SOLO AGREGA UN PEDIDO
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
        // FUNCION QUE DEVUELVE EL TIPO DE PEDIDO
        public static string checkTipoPedido( int tipo )
        {
            if( tipo == 0 )
            {
                return "Para comer aqui";
            }

            return "Para llevar";
        }
        // FUNCION QUE DEVUELVE EL STATUS DE LA ORDEN
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

        // FUNCION QUE RENDERIZA UNA TABLA POR PEDIDOS
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

            // SI RECIBE LA BANDERA AGREGA UNA COLUMNA
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

        // FUNCION PARA LISTAR TODOS LOS PEDIDOS PENDIENTES
        // CON STATUS 0
        public int listar()
        {

            List<Pedido> pedidos_pendientes = new List<Pedido>();

            AnsiConsole.Status().Start("Cargando pedidos...", ctx =>
            {
                pedidos_pendientes = this.obtenerPedidosPorStatus(0);
            });

            ConsoleHooks.printRule("[red]Pedidos pendientes:[/]");

            this.renderTable(pedidos_pendientes , false);            

            return ROUTER_REDIRECT;

        }

        // FUNCION PARA OBTENER LOS PEDIDOS POR EL STATUS QUE SE LE PASE
        private List<Pedido> obtenerPedidosPorStatus( int status = 0 )
        {

            List<Pedido> pedidos = new List<Pedido>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

               pedidos = dc.Pedidos.Where(pedido => pedido.status == status ).ToList();

               return pedidos;

            }

        }
        
        // FUNCION PARA OBTENER EL ULTIMO ID DE TODAS LAS ORDENES
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
