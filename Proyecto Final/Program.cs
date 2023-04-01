
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

        // bool response = usuario.run();

        if( true )
        {

            ConsoleHooks.printRule("[red]Autenticacion correcta[/]");
            
            Thread.Sleep(1000);

            while( true )
            {
                try
                {
                    bool continuar = menu.mostrarMenu(null);
                    if( !continuar )
                    {
                        Menu.showMainLogo();
                        ConsoleHooks.printRule("Fin del programa");
                        break;
                    }
                }
                catch
                {
                    
                }
            }

        }
        else
        {
            ConsoleHooks.printRule("[red]Usuario invalido[/]");
        }

    }
}