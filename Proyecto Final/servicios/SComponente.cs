using Proyecto_Final.clases;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    public class SComponente
    {
        public int showMenu()
        {

            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1) Listar componentes",
                        "2) Agregar componente",
                        "3) Actualizar componente",
                        "4) Eliminar componente",
                        "5) Salir",
                    }));

            int seleccion = this.checkMenu(opt);

            return this.handleSeleccion(seleccion);

        }

        private int checkMenu(string opcion)
        {

            switch (opcion)
            {
                case "1) Listar componentes":
                    return 0;
                case "2) Agregar componente":
                    return 1;
                case "3) Actualizar componente":
                    return 2;
                case "4) Eliminar componente":
                    return 3;
                case "5) Salir":
                    return 4;
            }

            return -1;

        }

        private int handleSeleccion(int opcion)
        {
            
            switch (opcion)
            {
                case 1:
                    return this.menuAgregarComponente();
            }

            return -1;

        }

        public int menuAgregarComponente()
        {

            var nombre_componente = AnsiConsole.Ask<string>("[green]Ingresa el nombre del componente[/]?");

            return -1;

        }

        public bool agregaComponente(Producto producto)
        {
            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                dc.Productos.Add(producto);

                return true;

            }
        }
    }
}
