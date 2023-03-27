
using Proyecto_Final.servicios;
using Proyecto_Final.clases;
using Proyecto_Final.hooks;

class Program
{
    static void Main(string[] args)
    {
        
        SLogin usuario = new SLogin();
        Menu menu = new Menu();
        
        Menu.showMainLogo();

        bool response = usuario.run();

        if( response )
        {
            
            ConsoleHooks.printRule("[red]Autenticacion correcta[/]");
            
            Thread.Sleep(1000);

            while( true )
            {
                bool continuar = menu.mostrarMenu(null);
                if( !continuar )
                {
                    Menu.showMainLogo();
                    ConsoleHooks.printRule("Fin del programa");
                    break;
                }
            }

        }
        else
        {
            ConsoleHooks.printRule("[red]Usuario invalido[/]");
        }

    }
}