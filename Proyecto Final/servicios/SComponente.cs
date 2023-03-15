using Proyecto_Final.clases;
using Spectre.Console;
using System.ComponentModel;

namespace Proyecto_Final.servicios
{
    public class SComponente
    {
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

            return -1;

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

            var rule = new Rule("[red]Componente agregado correctamente[/] \n").LeftJustified();

            AnsiConsole.Write(rule);

            return -1;

        }

        public int mostrarComponentes()
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                List<Componente> componentes = dc.Componentes.ToList();

                var table = new Table().LeftAligned();

                AnsiConsole.Live(table)
                    .Start(ctx =>
                    {
                        table.AddColumn("ID");
                        ctx.Refresh();
                        Thread.Sleep(1000);

                        table.AddColumn("Nombre Componente");
                        ctx.Refresh();
                        Thread.Sleep(1000);

                        foreach ( Componente componente in componentes )
                        {
                                table.AddRow(componente.id.ToString() , componente.nombre);
                        }

            });

                return -1;
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
    }
}
