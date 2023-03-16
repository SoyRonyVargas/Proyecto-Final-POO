using Proyecto_Final.clases;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private int handleSeleccion(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    return this.menuAgregarProducto();
                break;
                default:
                    return 0;
            }
        }

        public int menuAgregarProducto()
        {
            
            var nombre_producto = AnsiConsole.Ask<string>("[green]Ingresa el nombre del producto[/]?");
            
            var precio_producto = AnsiConsole.Ask<float>("[green]Ingresa el precio del producto[/]?");

            Producto producto = new Producto()
            {
                nombre = nombre_producto,
                precio = precio_producto
            };
            List<Componente> componentes = new List<Componente>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

                AnsiConsole.Status().Start("Cargando componentes...", ctx =>
                {
                    Thread.Sleep(500);

                    componentes = dc.Componentes.ToList();

                });

            }

            List<string> _componentes = componentes.Select(c => c.nombre).ToList();

            var fruits = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Selecciona los componentes del producto")
                .NotRequired() // Not required to have a favorite fruit
                .PageSize(10)
                .MoreChoicesText("[grey](Muevete con las flechas)[/]")
                .InstructionsText(
                    "[grey](Presiona [blue]<espacio>[/] para seleccionar un componente, " +
                    "[green]<Enter>[/] para aceptar)[/]")
                .AddChoices(_componentes));

            List<Componente> componentes_seleccionados = new List<Componente>();

            foreach ( string select in fruits )
            {
                
                Componente cmp = componentes.Where(c => c.nombre == select).FirstOrDefault()!;

                componentes_seleccionados.Add(cmp);

            }

            Menu.showMainLogo();

            var table = new Table().Expand().BorderColor(Color.Grey).LeftAligned();

            var rule = new Rule("[bold yellow]Se creara el producto con los siguientes datos:[/] \n").LeftJustified();

            AnsiConsole.Write(rule);

            table.AddColumn("[yellow bold]Nombre[/]");

            table.AddColumn("[yellow bold]Precio[/]");
            
            table.Columns[0].Header("[yellow bold]Nombre[/]");

            table.Columns[1].Header("[yellow bold]Precio[/]");

            table.BorderColor(Color.Yellow1);

            table.AddRow($"[white]{producto.nombre.ToString()}[/]", $"[white]${producto.precio.ToString("0.00")}[/]");

            AnsiConsole.Write(table);

            bool opcion = Menu.handleConfirm("¿Deseas guardar el producto?");

            if( opcion )
            {
                
                AnsiConsole.Status().Start("Guardando producto...", ctx =>
                {
                    this.agregarProducto(producto , componentes_seleccionados);
                });

                Menu.showMainLogo();

                var rule_final = new Rule("[red]Producto agregado correctamente[/] \n").LeftJustified();

                AnsiConsole.Write(rule_final);

                return -1;

            }

            return 0;

        }

        private bool agregarProducto(Producto producto , List<Componente> componentes)
        {
            using(RestauranteDataContext dc = new RestauranteDataContext())
            {
                dc.Productos.Add(producto);

                dc.SaveChanges();

                foreach( Componente c in componentes )
                {
                    
                    Producto_tiene_componentes elemento = new Producto_tiene_componentes()
                    {
                        id_componente = c.id,
                        id_producto = producto.id
                    };

                    dc.producto_tiene_componentes.Add(elemento);

                }

                dc.SaveChanges();

                return true;

            }
        }
    }
    
}
