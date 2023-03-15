using Proyecto_Final.clases;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.servicios
{
    public class SProducto
    {
        public int showMenu()
        {
            
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1) Listar productos",
                        "2) Agregar producto",
                        "3) Actualizar producto",
                        "4) Eliminar producto",
                        "5) Salir",
                    }));

            int seleccion = this.checkMenu(opt);

            this.handleSeleccion(seleccion);

            return -1;

        }

        private int checkMenu(string opcion)
        {

            switch (opcion)
            {
                case "1) Listar productos":
                    return 0;
                case "2) Agregar producto":
                    return 1;
                case "3) Actualizar producto":
                    return 2;
                case "4) Eliminar producto":
                    return 3;
                case "5) Salir":
                    return 4;
            }

            return -1;

        }

        private void handleSeleccion(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    this.menuAgregarProducto();
                break;
            }
        }

        public void menuAgregarProducto()
        {
            
            var nombre_producto = AnsiConsole.Ask<string>("[green]Ingresa el nombre del producto[/]?");
            
            var precio_producto = AnsiConsole.Ask<float>("[green]Ingresa el precio del producto[/]?");

        }

        public bool agregarProducto(Producto producto)
        {
            using(RestauranteDataContext dc = new RestauranteDataContext())
            {
                dc.Productos.Add(producto);

                return true;

            }
        }
    }
    
}
