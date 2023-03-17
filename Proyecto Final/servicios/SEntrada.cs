using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    public class SEntrada : IService
    {

        private string[] OPCIONES_MENU = {
            "1) Listar entradas",
            "2) Agregar entrada",
            "3) Salir",
        };
        
        public int mostrarMenu()
        {
            
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(this.OPCIONES_MENU));

            int seleccion = 3;//this.checkMenu(opt);

            this.handleSeleccion(seleccion);

            return -1;

        }

        private int handleSeleccion(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    return this.listar();
                case 1:
                    return this.crear();
                default:
                    return 0;
            }
        }

        public int crear()
        {
            throw new NotImplementedException();
        }

        public int eliminar()
        {
            throw new NotImplementedException();
        }

        public int listar()
        {
            throw new NotImplementedException();
        }

        
    }
}