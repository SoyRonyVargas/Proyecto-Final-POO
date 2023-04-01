using Proyecto_Final.servicios;
using Proyecto_Final.clases;
using Proyecto_Final.hooks;

class Program
{
    static void Main(string[] args)
    {
        
        SLogin usuario = new SLogin();
        Menu menu = new Menu();
        
        bool auth = true;

        while( auth )
        {
            
            Menu.showMainLogo();

            ConsoleHooks.printRule("[red]Autenticacion correcta[/]");
            
            Thread.Sleep(1000);

            ConsoleHooks.printRule("[red bold]Iniciar Sesión[/]");

            bool response = usuario.run();

            if( response )
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

                Menu.showMainLogo();

                ConsoleHooks.printRule("[red]Autenticacion correcta[/]");
                
                Thread.Sleep(700);

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
                Menu.showMainLogo();
                ConsoleHooks.printRule("[red]Usuario invalido[/]");
            }
        }

    }
}