using Microsoft.Identity.Client;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.clases
{
    public class GenerarTkt
    {
        public static int Ticket()
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                
                int id_orden = ConsoleHooks.askNumero("");

                var tabla = new Table().Centered();

            Console.WriteLine("Ingrese su No.de orden");
            Console.ReadLine();
            Pedido pedidobd = dc.Pedidos.Where(Pedido => Pedido.id == id_orden).FirstOrDefault()!;

            tabla.AddColumn("No.Orden");
            tabla.AddColumn("Productos");
            tabla.AddColumn("Costos $");
             //agregue tablas con tres columnas
              
            AnsiConsole.Write(tabla);


                tabla.AddRow("No.Pedido"+pedidobd.id + pedidobd.producto + pedidobd.importe);

                

            }
            return 0;    

        }
            
        
    }
}
