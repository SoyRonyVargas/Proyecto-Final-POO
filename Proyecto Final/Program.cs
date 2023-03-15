
using Proyecto_Final.clases;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {

        Menu menu = new Menu();
        while( true)
        {
            
            menu.mostrarMenu();

            Console.WriteLine("Presiona cualquier tecla para continuar...");

            Console.ReadKey();

        }

        /*using (RestauranteDataContext dc = new RestauranteDataContext())
        {

            Console.WriteLine("Productos: ");
            ICollection<Pedido> pedidos = dc.Pedidos.Where(pedido => pedido.status == 4).ToList();

            foreach( Pedido _pedido in pedidos )
            {
                Console.WriteLine($"Status: {_pedido.status}");
                Console.WriteLine($"Cantidad: {_pedido.cantidad}");
                Console.WriteLine($"Cantidad:");
                //List<Producto> _Productos = dc.Productos.Where( p => p.PedidoID == _pedido.PedidoID ).ToList();
                Debugger.Break();
                //_pedido.Pedido_tiene_productos.ToList()[0].Producto.m
            }

            Console.ReadKey();

            var cliente = new Cliente()
            {
                apellido = "Jhordi",
                nombre = "UCAN",
                rfc = "inventado" 
            };

            dc.Clientes.Add(cliente);

            var pedido = new Pedido()
            {
                cantidad = 1,
                mesa = 4,
                status = 4,
                tipo_pedido = 0,
            };
            
            dc.Pedidos.Add(pedido);
         
            dc.SaveChanges();

            var productos = new Pedido_tiene_productos();

            productos.Producto = dc.Productos.FirstOrDefault();

            productos.Pedido = pedido;

            dc.pedido_tiene_productos.Add(productos);

            //pedido.productos.Add(p);

            dc.SaveChanges();

            Console.WriteLine("Se agrego el pedido");

            Console.ReadKey();

        }/*/
    }
}