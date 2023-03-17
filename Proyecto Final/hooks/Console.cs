using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
}
