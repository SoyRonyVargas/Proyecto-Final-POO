using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    public class SEntrada : IService
    {

        private string[] OPCIONES_MENU = {
            "1) Listar entradas",
            "2) Agregar entrada",
            "3) Salir",
        };
        
        private int checkMenu(string opcion)
        {
            switch (opcion)
            {
                case "1) Listar entradas":
                    return 0;
                case "2) Agregar entrada":
                    return 1;
                case "3) Salir":
                    return 2;
            }

            return -1;

        }

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

            ConsoleHooks.printRule("[red]Selecciona los productos de la orden[/]");

            List<string> productos_opciones = SComponente.obtenerComponentesListado();
            List<Componente> componentes = SComponente.obtenerComponentes();

            string opcion = ConsoleHooks.askOpciones(productos_opciones);

            Componente componente_seleccionado = componentes.Where( componente => componente.nombre == opcion ).FirstOrDefault()!;

            int cantidad_entrada = ConsoleHooks.askNumero("Ingresa la cantidad de la entrada");

            Entrada entrada = new Entrada()
            {
                existencias = cantidad_entrada,
                existencias_iniciales = cantidad_entrada,
                id_componente = componente_seleccionado.id
            };

            this.agregarEntrada(entrada);

            Menu.showMainLogo();

            ConsoleHooks.printRule("[red]Entrada creada correctamente[/]");

            return 1;

        }

        private bool agregarEntrada(Entrada entrada)
        {

            try
            {
                using(RestauranteDataContext dc = new RestauranteDataContext())
                {
                    dc.Entradas.Add(entrada);
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

        public List<Entrada> obtenerEntradas()
        {
            
            List<Entrada> entradas = new List<Entrada>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                entradas = dc.Entradas.ToList();
            }

            return entradas;

        }

        public int listar()
        {
            
            List<Entrada> entradas = new List<Entrada>();

            AnsiConsole.Status().Start("Cargando entradas...", ctx =>
            {
                entradas = this.obtenerEntradas();
            });

            var table = new Table().Expand().BorderColor(Color.Yellow1);
                    table.AddColumn("[yellow bold]Producto[/]");
                    table.AddColumn("[yellow bold]Cantidad Entrada[/]");

            Menu.showMainLogo();

            AnsiConsole.Live(table).AutoClear(false)
                    .Start(ctx =>
                    {

                       

                        table.Columns[0].Header("[yellow bold]ID[/]");
                        
                        table.Columns[1].Header("[yellow bold]Nombre Componente[/]");
                        
                        table.Title("Componentes").LeftAligned();
                        
                        table.BorderColor(Color.Yellow1);

                        foreach ( Entrada entrada in entradas )
                        {
                            table.AddRow( 
                                $"[white]1[/]" , 
                                $"[white]{entrada.existencias_iniciales}[/]"
                            );
                        }

                    });

            return -1;

        }
        
    }
}