﻿using System.Diagnostics;
using Proyecto_Final.clases;
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

        private static string concatStringInstruccion(string str)
        {

            string concat = str + " [yellow bold](Presiona ESC para salir)[/]";

            return concat;

        }

        public static string askString( string? msg = null , string error = "Ingresa un valor valido" )
        {
            
            if( msg != null )
            {
                msg = concatStringInstruccion(msg);
                ConsoleHooks.printRule(msg);
            }

            string result = "";
            
            ConsoleKeyInfo lastKey;

            // read keys until Enter is pressed
            while ((lastKey = Console.ReadKey()).Key != ConsoleKey.Enter)
            {
                // if Escape is pressed, exit
                if (lastKey.Key == ConsoleKey.Escape)
                {
                    throw new Exception();
                }

                // otherwise, add the key entered to the input
                
                if( lastKey.Key != ConsoleKey.Backspace )
                {
                    result += lastKey.KeyChar.ToString();
                }
                
                if( lastKey.Key == ConsoleKey.Backspace )
                {
                    Console.Write(" \b");
                    result = result.Remove(result.Length - 1 , 1 );
                }

            }

            System.Console.WriteLine(result);

            return result;

        }
        
        public static int askInt( string? msg = null , string error = "Ingresa un valor valido" , bool concat = true , bool clear = true )
        {
            
            if( msg != null )
            {
                if( concat )
                {
                    msg = concatStringInstruccion(msg);
                }
                ConsoleHooks.printRule(msg);
                ValidationResult.Success();
            }

            string numbersEntered = "";
            
            ConsoleKeyInfo lastKey;

            // read keys until Enter is pressed
            while ((lastKey = Console.ReadKey()).Key != ConsoleKey.Enter)
            {
                // if Escape is pressed, exit
                if (lastKey.Key == ConsoleKey.Escape)
                {
                    throw new Exception();
                }

                // otherwise, add the key entered to the input

                if( lastKey.Key != ConsoleKey.Backspace )
                {
                    numbersEntered += lastKey.KeyChar.ToString();
                }
                
                if( lastKey.Key == ConsoleKey.Backspace )
                {
                    Console.Write(" \b");
                    numbersEntered = numbersEntered.Remove(numbersEntered.Length - 1 , 1 );
                }

            }

            while( true )
            {
                try
                {
                    
                    int result = int.Parse(numbersEntered);
                    
                    if( result < 0 )
                    {
                        throw new Exception("pene");
                    }
                    
                    System.Console.WriteLine(result);

                    return result;

                }
                catch
                {
                    
                    if( clear )
                    {
                        Menu.showMainLogo();
                    }
                    
                    printRule("[red]Ingresa un valor valido[/]");
                    
                    return askInt( msg , error , false , clear );

                }
            }

        }

        public static double askDouble( string? msg = null , string error = "Ingresa un valor valido" , bool concat = true )
        {
            
            if( msg != null )
            {
                if( concat )
                {
                    msg = concatStringInstruccion(msg);
                }
                ConsoleHooks.printRule(msg);
                ValidationResult.Success();
            }

            string numbersEntered = "";
            
            ConsoleKeyInfo lastKey;

            // read keys until Enter is pressed
            while ((lastKey = Console.ReadKey()).Key != ConsoleKey.Enter)
            {
                // if Escape is pressed, exit
                if (lastKey.Key == ConsoleKey.Escape)
                {
                    throw new Exception();
                }

                if( lastKey.Key != ConsoleKey.Backspace )
                {
                    numbersEntered += lastKey.KeyChar.ToString();
                }
                
                if( lastKey.Key == ConsoleKey.Backspace )
                {
                    Console.Write(" \b");
                    numbersEntered = numbersEntered.Remove(numbersEntered.Length - 1 , 1 );
                }
            }

            while( true )
            {
                try
                {
                    
                    double result = double.Parse(numbersEntered);
                    
                    if( result < 0 )
                    {
                        throw new Exception();
                    }
                    
                    System.Console.WriteLine(result);

                    return result;

                }
                catch
                {
                    Menu.showMainLogo();
                    printRule("[red]Ingresa un valor valido[/]");
                    return askDouble( msg , error , false );
                }
            }

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
