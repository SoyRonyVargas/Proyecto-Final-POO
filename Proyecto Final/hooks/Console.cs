using Spectre.Console;
namespace Proyecto_Final.hooks
{
    public class ConsoleHooks
    {

        public static void printRule( string msg )
        {
            var rule = new Rule(msg).LeftJustified();

            AnsiConsole.Write(rule);

        }

        public static int askNumero( string msg = "" , string error = "Ingresa un valor valido" )
        {
            return AnsiConsole.Prompt(
                        new TextPrompt<int>(msg)
                            .PromptStyle("red")
                            .ValidationErrorMessage($"[red]{error}[/]")
                            .Validate(age =>
                            {
                            return age switch
                                {
                                    <= 0 => ValidationResult.Error($"[red]{error}[/]"),
                                    _ => ValidationResult.Success(),
                                };
                        }));
        }

        public static string askOpciones( List<string> opciones , string msg = "Selecciona una opcion"  )
        {

            var eleccion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[red]{msg}[/]?")
                    .PageSize(10)
                    .AddChoices(opciones));

            return eleccion;

        }

    }
}
