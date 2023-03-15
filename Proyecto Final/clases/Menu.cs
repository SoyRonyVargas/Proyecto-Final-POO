using Proyecto_Final.servicios;
using Spectre.Console;

namespace Proyecto_Final.clases
{
    public class Menu
    {
        private SProducto SProducto = new SProducto();
        private SComponente SComponente = new SComponente();

        public void mostrarMenu()
        {
            
            Console.Clear();

            AnsiConsole.Write(
                 new FigletText("La Delicia")
                .LeftJustified()
                .Color(Color.Red));

            var rule = new Rule("[red]Menu[/] \n").LeftJustified();

            Console.WriteLine("");

            AnsiConsole.Write(rule);

            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    //.MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(new[] {
                        "1) Menu",
                        "2) Tomar orden", 
                        "3) Cobrar orden", 
                        "4) Ventas",
                        "5) Productos",
                        "6) Componentes", 
                        "7) Clientes", 
                        "8) Proveedores",
                        "9) Salir",
                    }));

            int opcion = this.checkMainMenu(opt);

            int response = this.handleSeleccion(opcion);

            if( response == 0 )
            {

            }

        }

        private int checkMainMenu( string opcion )
        {
            
            switch( opcion )
            {
                case "1) Menu":
                    return 0;
                case "2) Tomar orden":
                    return 1;
                case "3) Cobrar orden":
                    return 2;
                case "4) Ventas":
                    return 3;
                case "5) Productos":
                    return 4;
                case "6) Componentes":
                    return 5;
            }

            return -1;

        }

        private int handleSeleccion(int opcion )
        {
            switch(opcion)
            {
                case 4:
                   return SProducto.showMenu();
                case 5:
                   return SComponente.showMenu();
                default: return -1;
            }
        }
    }
    
}
