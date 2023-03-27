using Spectre.Console;

namespace Proyecto_Final.clases
{
    public class GenerarTkt
    {
        public static void Ticket()
        {
            var tabla = new Table().Centered();

            tabla.AddColumn("No.Orden");
            tabla.AddColumn("Productos");
            tabla.AddColumn("Costos $");

            AnsiConsole.Write(tabla);
                

        }

    }
}
