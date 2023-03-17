using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;
using System.ComponentModel;

namespace Proyecto_Final.servicios
{
    public class SComponente
    {
        private const int ROUTER_REDIRECT = 5;
        public int showMenu()
        {

            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1) Listar componentes",
                        "2) Agregar componente",
                        "3) Actualizar componente",
                        "4) Eliminar componente",
                        "5) Salir",
                    }));

            int seleccion = this.checkMenu(opt);

            return this.handleSeleccion(seleccion);

        }

        private int checkMenu(string opcion)
        {

            switch (opcion)
            {
                case "1) Listar componentes":
                    return 0;
                case "2) Agregar componente":
                    return 1;
                case "3) Actualizar componente":
                    return 2;
                case "4) Eliminar componente":
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
                    return this.mostrarComponentes();
                case 1:
                    return this.menuAgregarComponente();
            }

            return 0;

        }

         public static Componente obtenerComponente( int id )
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                Componente componente = dc.Componentes.Where( p => p.id == id ).FirstOrDefault()!;   
                return componente;
            }
        }

        public static List<Componente> obtenerComponentes()
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                List<Componente> componentes = dc.Componentes.ToList();
                return componentes;
            }
        }
        
        public static List<string> obtenerComponentesListado()
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                List<Componente> componentes = dc.Componentes.ToList();
                List<string> c = componentes.Select( x => x.nombre ).ToList();
                return c;
            }
        }

        public int menuAgregarComponente()
        {

            string nombre_componente = AnsiConsole.Ask<string>("[green]Ingresa el nombre del componente[/]?");

            Componente componente = new Componente() { 
                nombre = nombre_componente
            };

            AnsiConsole.Status().Start("Guardando componente...", ctx =>
            {

                this.agregaComponente(componente);

            });

            Console.Clear();

            Menu.showMainLogo();

            var rule = new Rule("[red]Componente agregado correctamente[/] \n").LeftJustified();

            AnsiConsole.Write(rule);

            return ROUTER_REDIRECT;

        }

        public int mostrarComponentes()
        {

            Console.Clear();

            List<Componente> componentes = new List<Componente>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

                AnsiConsole.Status().Start("Cargando componentes...", ctx =>
                {
                    
                    Thread.Sleep(500);

                    componentes = dc.Componentes.ToList();

                });

                var table = new Table().Expand().BorderColor(Color.Grey);
                    table.AddColumn("[yellow bold]ID[/]");
                    table.AddColumn("[yellow bold]Nombre Componente[/]");

                Menu.showMainLogo();
                
                ConsoleHooks.printRule("[red]Componentes[/]");

                AnsiConsole.Live(table).AutoClear(false)
                    .Start(ctx =>
                    {

                       

                        table.Columns[0].Header("[yellow bold]ID[/]");
                        
                        table.Columns[1].Header("[yellow bold]Nombre Componente[/]");
                        
                        table.BorderColor(Color.Yellow1);

                        foreach ( Componente componente in componentes )
                        {
                              table.AddRow( $"[white]{componente.id.ToString()}[/]" , $"[white]{componente.nombre}[/]");
                        }

                    });

                return ROUTER_REDIRECT;

            }
        }

        public bool agregaComponente( Componente componente )
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                
                dc.Componentes.Add(componente);

                dc.SaveChanges();

                return true;

            }
        }

        

        

        public void eliminarcomp ()
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

                AnsiConsole.Status().Start("Cargando componentes hk...", ctx =>
                {
                    Thread.Sleep(500);

                    List <Componente>componentes_seleccionados = new List<Componente>();

                    List <Componente>componentes = dc.Componentes.ToList();

                    List<string> _componentes = componentes.Select(c => c.nombre).ToList();

                    var fruits = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title("Selecciona los componentes del producto")
                        .NotRequired() // Not required to have a favorite fruit
                        .PageSize(10)
                        .MoreChoicesText("[grey](Muevete con las flechas)[/]")
                        .InstructionsText(
                            "[grey](Presiona [blue]<espacio>[/] para seleccionar un componente, " +
                            "[green]<Enter>[/] para aceptar)[/]")
                        .AddChoices(_componentes));

                    foreach (string select in fruits)
                    {

                        Componente cmp = componentes.Where(c => c.nombre == select).FirstOrDefault()!;

                        componentes_seleccionados.Add(cmp);

                        dc.Componentes.Remove(cmp);
                    }

                    
                });

            }
        }
    }
}
