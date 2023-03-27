using Spectre.Console;

namespace Proyecto_Final.servicios
{
    
    public class SLogin
    {

        public bool run()
        {

            string usuario = AnsiConsole.Ask<string>("[red]Usuario: [/]");

            string pass = AnsiConsole.Prompt(
                new TextPrompt<string>("[red]Contraseña: [/]")
                    .PromptStyle("red")
                    .Secret('*'));
            
            bool auth = false;
            
            AnsiConsole.Status().Start("Autenticando...", ctx =>
            {
                auth = SUsuario.login( usuario , pass );
            });
            
            return auth;

        }

    }

}