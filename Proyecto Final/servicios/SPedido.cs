using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Proyecto_Final.servicios
{
    public class SPedido : IService
    {

        string[] OPCIONES_MENU = {
            "1) Listar pedidos",
            "2) Agregar pedido",
            "3) Actualizar pedido",
            "4) Finalizar pedido",
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
                case "1) Listar pedidos":
                    return 0;
                case "2) Agregar pedido":
                    return 1;
                case "3) Actualizar pedido":
                    return 2;
                case "4) Finalizar pedido":
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
                default:
                    return 0;
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

                var selecciones = AnsiConsole.Prompt(
                           new MultiSelectionPrompt<string>()
                               .Required()
                               .AddChoices(producto_listado));

                List<Producto> productos_seleccionados = new List<Producto>();

                foreach (string seleccionado in selecciones)
                {

                    Producto cmp = productos.Where(c => c.nombre == seleccionado ).FirstOrDefault()!;

                    productos_seleccionados.Add(cmp);

                }

                var tipo_pedido = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Selecciona el tipo de pedido:")
                        .PageSize(3)
                        .AddChoices(new[] {
                            "1) Comer aqui", "2) Para llevar",
                        }));

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

                var table_pedido = new Table();

                table_pedido.BorderColor(Color.Yellow1);
                table_pedido.Expand();

                // Add some columns
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

                        void Update(int delay, Action action)
                        {
                            action();
                            ctx.Refresh();
                            Thread.Sleep(delay);
                        }

                        table.AddColumn("[bold]Producto[/]");
                        table.AddColumn("[bold]Cantidad[/]");
                        table.AddColumn("[bold]Importe[/]");

                        table.Columns[0].Header("[bold]Producto[/]");
                        table.Columns[1].Header("[bold]Cantidad[/]");
                        table.Columns[2].Header("[bold]Importe[/]");

                        foreach ( Producto producto in productos_seleccionados )
                        {
                            table.AddRow(
                                $"[bold]{producto.nombre}[/]",
                                "[bold]3[/]",
                                "[bold]$99.99[/]"
                            );
                        }

                        Update(70, () => table.Columns[0].Footer(""));
                        Update(70, () => table.Columns[1].Footer(""));
                        Update(400, () => table.Columns[2].Footer("[red bold]Total: $10,318,030,576[/]"));

                    });

                Console.WriteLine("");
                Console.WriteLine("");

                bool response = this.agregarPedido( pedido_nuevo , productos_seleccionados );

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

        private bool agregarPedido( Pedido pedido , List<Producto> productos)
        {

            try
            {
                
                using (RestauranteDataContext dc = new RestauranteDataContext())
                {

                    dc.Pedidos.Add(pedido);

                    dc.SaveChanges();

                    foreach (Producto producto in productos)
                    {

                        Pedido_tiene_productos elemento = new Pedido_tiene_productos()
                        {
                            cantidad = 1,
                            id_producto = producto.id
                        };

                        dc.pedido_tiene_productos.Add(elemento);

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

        public int eliminar()
        {
            throw new NotImplementedException();
        }

        public int listar()
        {

            ConsoleHooks.printRule("[red]Pedidos pendientes:[/]");

            var table = new Table();
            
            table.Expand();

            // Add some columns
            table.AddColumn(new TableColumn("[u]Productos[/]"));
            // table.AddColumn("Numero Pedido");
            // table.AddColumn("Tipo ");
            // table.AddColumn("Mesa");
            // table.AddColumn("Status");


            var tabla_productos = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Green)
                .Expand()
                .AddColumn(new TableColumn("[u]Foo[/]"))
                .AddColumn(new TableColumn("[u]Bar[/]"))
                .AddColumn(new TableColumn("[u]Baz[/]"))
                .AddRow("Hello", "[red]World![/]", "")
                .AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            var table_pedidos = new Table()
                .Centered()
                .Expand()
                .BorderColor(Color.Yellow1)
                .Border(TableBorder.DoubleEdge)
                .AddColumn(new TableColumn(new Panel("[bold u]Numero Pedido[/]").BorderColor(Color.Red)))
                .AddColumn(new TableColumn(new Panel("[bold u]Tipo[/]").BorderColor(Color.Green)))
                .AddColumn(new TableColumn(new Panel("[bold u]Mesa[/]").BorderColor(Color.Blue)))
                .AddColumn(new TableColumn(new Panel("[bold u]Status[/]").BorderColor(Color.Blue)))
                .AddColumn(new TableColumn(new Panel("[bold u]Productos[/]").BorderColor(Color.Blue)));


            List<PedidoFull> pedidos_pendientes = this.obtenerPedidosPendientes();

            foreach (PedidoFull pedido in pedidos_pendientes)
            {
                table_pedidos.AddRow(
                    new Markup($"[u]{pedido.pedido.id}[/]"),
                    new Markup($"[u]Para comer aqui[/]"),
                    new Markup($"[u]5[/]"),
                    new Markup($"[u]Pendiente[/]"),
                    tabla_productos
                );
            }

            // Render the table to the console
            AnsiConsole.Write(table_pedidos);

            return -1;
        }


        private List<PedidoFull> obtenerPedidosPendientes()
        {

            List <PedidoFull> pedidos = new List<PedidoFull>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

               List<Pedido> pedidos_pendientes = dc.Pedidos.Where(pedido => pedido.status == 0).ToList();

               foreach( Pedido _pedido in pedidos_pendientes )
               {
                    
                    PedidoFull pedido = new PedidoFull();

                    List<Pedido_tiene_productos> productos_por_pedido = dc.pedido_tiene_productos.Where(pedido => pedido.id_pedido == _pedido.id ).ToList();
                    List<Producto> productos_pedido = new List<Producto>();
                    
                    foreach(Pedido_tiene_productos pedido_tiene_productos in productos_por_pedido)
                    {

                        Producto producto = dc.Productos.Where(p => p.id == pedido_tiene_productos.id_producto).FirstOrDefault()!;

                        productos_pedido.Add(producto);

                    }

                    pedido.productos = productos_pedido;

                    pedido.pedido = _pedido;

                    pedidos.Add(pedido);

               }

            }

            return pedidos;

        }
       
    }
}
