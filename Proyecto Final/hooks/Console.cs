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

        public static void wait()
        {
            Console.WriteLine("Presiona enter para continuar...");
            Console.ReadKey();
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
        
        public static float askDecimal( string msg = "" , string error = "Ingresa un valor valido" )
        {
            return AnsiConsole.Prompt(
                        new TextPrompt<float>(msg)
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

        public static string askOpciones( List<string> opciones , string? msg = "Selecciona una opcion"  )
        {
            //arreglo de opciones
            if( msg == null )
            {
                return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .AddChoices(opciones));
            }
            
            var eleccion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[red]{msg}[/]")
                    .PageSize(10)
                    .AddChoices(opciones));

            return eleccion;

        }
        
        public static List<string> askMultiOpciones( List<string> opciones , string? msg = "Selecciona una opcion"  )
        {
            //arreglo de opciones
            if( msg == null )
            {
                return AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .PageSize(10)
                    .InstructionsText(
                        "[grey](Muevete con las flechas del teclado)[/]"
                    )
                    .AddChoices(opciones));
            }
            
            var eleccion = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title($"[red]{msg}[/]")
                    .PageSize(10)
                    .InstructionsText(
                        "[grey](Muevete con las flechas del teclado)[/]"
                    )
                    .AddChoices(opciones));

            return eleccion;

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

    }
}
