﻿using Proyecto_Final.clases;
using Proyecto_Final.hooks;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.servicios
{
    public class SProducto : IService
    {

        public int mostrarMenu()
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
                case 0:
                    return this.mostrarProductos();
                case 1:
                    return this.menuAgregarProducto();
                case 3:
                    //return this.eleminar();
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

            List<Componente> componentes_seleccionados = new List<Componente>();
            List<string> _componentes = new List<string>();
            List<Componente> componentes = new List<Componente>();

            AnsiConsole.Status().Start("Cargando componentes", ctx =>
            {
                    Thread.Sleep(250);
                    using (RestauranteDataContext dc = new RestauranteDataContext())
                    {

                        componentes = dc.Componentes.ToList();

                        _componentes = componentes.Select(c => c.nombre).ToList();

                        Console.WriteLine("terminado");

                    }
            });

            var fruits = AnsiConsole.Prompt(
                           new MultiSelectionPrompt<string>()
                               .Title("Selecciona los componentes del producto")
                               .NotRequired()
                               .PageSize(10)
                               .MoreChoicesText("[grey](Muevete con las flechas)[/]")
                               .InstructionsText("[grey](Muevete con las flechas)[/]")
                               .AddChoices(_componentes));

            foreach (string seleccionado in fruits)
            {

                Componente cmp = componentes.Where(c => c.nombre == seleccionado).FirstOrDefault()!;

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

        public static List<Producto> obtenerProductos()
        {

            List<Producto> productos = new List<Producto>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                productos = dc.Productos.ToList();
                
                AnsiConsole.Status().Start("Cargando productos...", ctx =>
                {

                    Thread.Sleep(500);

                    productos = dc.Productos.ToList();

                });
            }

            return productos;

        }

        public static List<string> obtenerProductosListado()
        {

            List<Producto> productos = new List<Producto>();
            List<string> productos_listado = new List<string>();

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {
                productos = dc.Productos.ToList();

                AnsiConsole.Status().Start("Cargando productos...", ctx =>
                {

                    Thread.Sleep(500);

                    productos = dc.Productos.ToList();

                });
            }

            productos_listado = productos.Select(producto => producto.nombre).ToList();

            return productos_listado;

        }

        private int mostrarProductos()
        {

            using (RestauranteDataContext dc = new RestauranteDataContext())
            {

                List<Producto> productos = new List<Producto>();

                AnsiConsole.Status().Start("Cargando productos...", ctx =>
                {
                    
                    productos = dc.Productos.ToList();

                    Thread.Sleep(500);

                });

                var table = new Table().Expand().BorderColor(Color.Grey);

                table.AddColumn("[yellow bold]ID[/]");
                table.AddColumn("[yellow bold]Nombre[/]");
                table.AddColumn("[yellow bold]Precio[/]");

                Menu.showMainLogo();

                ConsoleHooks.printRule("[red]Productos[/]");

                AnsiConsole.Live(table).AutoClear(false)
                    .Start(ctx =>
                    {

                        table.Columns[0].Header("[yellow bold]ID[/]");

                        table.Columns[1].Header("[yellow bold]Nombre[/]");

                        table.Columns[1].Header("[yellow bold]Precio[/]");

                        table.BorderColor(Color.Yellow1);

                        foreach (Producto producto in productos)
                        {
                            table.AddRow(
                                $"[white]{producto.id.ToString()}[/]",
                                $"[white]{producto.nombre}[/]",
                                $"[white]${producto.precio.ToString("0.00")}[/]"
                           );
                        }

                    });

                return 5;

            }

            return -1;

        }

        protected int eleminar()
        {

            var rule = new Rule("[red]Selecciona los productos que vas a eliminar[/] \n").LeftJustified();

            //AnsiConsole.Write(rule);

            //List<Componente> componentes_seleccionados = new List<Componente>();

            //componentes_seleccionados = SComponente.seleccionarComponentes();

            return 6;
        }

        public int listar()
        {
            throw new NotImplementedException();
        }

        public int crear()
        {
            throw new NotImplementedException();
        }

        public int eliminar()
        {
            throw new NotImplementedException();
        }
    }
    
}
