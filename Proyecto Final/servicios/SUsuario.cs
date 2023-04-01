using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;

namespace Proyecto_Final.servicios
{
    public class SUsuario : IService
    {
        private const int ROUTER_REDIRECT = 5;
        public int mostrarMenu()
        {
            
            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Selecciona una opcion")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1) Listar usuarios",
                        "2) Agregar usuario",
                        "3) Salir",
                    }));

            int seleccion = this.checkMenu(opt);

            return this.handleSeleccion(seleccion);

        }

        private int checkMenu(string opcion)
        {

            switch (opcion)
            {
                case "1) Listar usuarios":
                    return 0;
                case "2) Agregar usuario":
                    return 1;
                case "3) Salir":
                    return 2;
            }

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
                case 2:
                    return 0;
                default:
                    return 0;
            }
        }

        public int crear()
        {
            
            string nombre = ConsoleHooks.askString(
                "[red]Ingresa el nombre del usuario:[/]"
            );
            
            string apellidos = ConsoleHooks.askString(
                "[red]Ingresa los apellidos del usuario:[/]"
            );
            
            string correo = ConsoleHooks.askString(
                "[red]Ingresa el correo del usuario:[/]"
            );
            
            string pass_1 = ConsoleHooks.askString(
                "[red]Ingresa la contraseña: [/]"
            );
            
            string pass_2 = ConsoleHooks.askString(
                "[red]Ingresa nuevamente la contraseña: [/]"
            );
            
            if( pass_1 != pass_2 )
            {
                
                ConsoleHooks.printRule("[red]Las contraseñas no coinciden[/]");
                
                return ROUTER_REDIRECT;

            }

            bool response = false;

            Usuario usuario = new Usuario() {
                apellidos = apellidos,
                correo = correo,
                nombre = nombre,
                password = pass_1
            };

            AnsiConsole.Status().Start("Guardando usuario...", ctx =>
            {
                response = this.agregarUsuario(usuario);
            });

            if( response )
            {
                ConsoleHooks.printRule("[red]Usuario agregado correctamente[/]");
            }
            else
            {
                ConsoleHooks.printRule("[red]No se pudo agregar el usuario[/]");
            }

            return ROUTER_REDIRECT;

        }

        private bool agregarUsuario( Usuario usuario )
        {
            try
            {
                using(RestauranteDataContext dc = new RestauranteDataContext())
                {
                    dc.Usuarios.Add(usuario);
                    dc.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool login( string correo , string password )
        {
            try
            {
                using(RestauranteDataContext dc = new RestauranteDataContext())
                {
                    Usuario user = dc.Usuarios.Where( usuario => usuario.correo == correo ).FirstOrDefault()!;

                    if (user == null) return false;
                    
                    if( user.password == password ) return true;

                    return false;
                    
                }
            }
            catch
            {
                System.Console.WriteLine("Error bd");
                return false;
            }
        }
        public int eliminar()
        {
            throw new NotImplementedException();
        }

        private List<Usuario> obtenerUsuarios()
        {

            List<Usuario> usuarios = new List<Usuario>();

            using( RestauranteDataContext dc = new RestauranteDataContext())
            {
                usuarios = dc.Usuarios.ToList();
            }

            return usuarios;

        }

        public int listar()
        {

            Table table = new Table().Expand().BorderColor(Color.Grey);

            List<Usuario> usuarios = new List<Usuario>();

            AnsiConsole.Status().Start("Cargando usuarios...", ctx =>{
                usuarios = this.obtenerUsuarios();
            });

            table.Border(TableBorder.Rounded);

            table.AddColumn("[yellow bold]ID[/]");
            table.AddColumn("[yellow bold]Nombre[/]");
            table.AddColumn("[yellow bold]Apellidos[/]");
            table.AddColumn("[yellow bold]Correo Electronico[/]");

            Menu.showMainLogo();

            ConsoleHooks.printRule("[red]Usuarios[/]");

            table.BorderColor(Color.Yellow1);

            foreach (Usuario usuario in usuarios)
            {
                table.AddRow(
                    $"[yellow]{usuario.id.ToString()}[/]",
                    $"[yellow]{usuario.nombre}[/]",
                    $"[yellow]{usuario.apellidos}[/]",
                    $"[yellow]{usuario.correo}[/]"
               );
            }

            AnsiConsole.Write(table);

            return ROUTER_REDIRECT;

        }

    }
}