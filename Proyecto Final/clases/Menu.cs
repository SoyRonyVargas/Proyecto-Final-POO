using Proyecto_Final.servicios;
using System.Diagnostics;
using Spectre.Console;
// using Spectre.Console.ImageSharp;
namespace Proyecto_Final.clases
{
    public class Menu
    {
        private SProducto SProducto = new SProducto();
        private SPedido SPedido = new SPedido();

        public static void setCargando()
        {
            AnsiConsole.Status().Start("Cargando...", ctx =>
            {
                Thread.Sleep(500);
            });
        }
        public static void showMainLogo( bool clear = true )
        {
            if( clear )
            {
                Console.Clear();
            }

            AnsiConsole.Write(
                new FigletText("La Delicia")
               .LeftJustified()
               .Color(Color.Red));
       }
        
        public static bool handleConfirm( string msg = "" )
        {
            var seleccion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(msg)
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Confirmar", "Cancelar"
                    }));

            return seleccion == "Confirmar";

        }

        public void mostrarMenu( int? valor = null )
        {
            
            Console.Clear();

            Menu.showMainLogo();

            var rule = new Rule("[red]Menu[/]\n").LeftJustified();

            Console.WriteLine("");

            AnsiConsole.Write(rule);

            // Console.WriteLine("Valor de retorno");
            // Console.WriteLine(valor);
            // Console.ReadKey();

            int opcion = 0;

            if( valor == null || valor == 0 )
            {
                
                var opt = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Selecciona una opcion")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "1) Menu",
                            "2) Pedidos", 
                            "3) Cobrar pedido", 
                            "4) Ventas",
                            "5) Productos",
                            "6) Salir",
                        }));

               opcion = this.checkMainMenu(opt);
            }
            else
            {
                opcion = (int)valor;
            }

            int response = this.handleSeleccion(opcion);

            // Console.WriteLine("Valor");
            // Console.WriteLine(valor);

            Console.WriteLine("Presiona cualquier tecla para continuar...");
            
            Console.ReadKey();

            if ( response != -1 )
            {
                this.mostrarMenu(response);
            }

        }

        private int checkMainMenu( string opcion )
        {
            
            switch( opcion )
            {
                case "1) Menu":
                    return 0;
                case "2) Pedidos":
                    return 1;
                case "3) Cobrar pedido":
                    return 2;
                case "4) Ventas":
                    return 3;
                case "5) Productos":
                    return 4;
                case "6) Salir":
                    return -1;
            }

            return -1;

        }

        private int handleSeleccion(int opcion )
        {
            switch(opcion)
            {
                case 1:
                    return SPedido.mostrarMenu();
                case 2:
                    return SPedido.cobrar();
                case 3:
                    return SPedido.listarVentas();
                case 4:
                    return SProducto.mostrarMenu();
                default: return -1;
            }
        }
    }
    
}
