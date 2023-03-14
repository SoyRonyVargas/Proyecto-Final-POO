
using Proyecto_Final.clases;

class Program
{
    static void Main(string[] args)
    {
        using (RestauranteDataContext dc = new RestauranteDataContext())
        {

            List<Pedido> pedidos = dc.Pedidos.Where(pedido => pedido.status == 4).ToList();

            foreach( Pedido _pedido in pedidos )
            {
                Console.WriteLine($"Status: {_pedido.status}");
                Console.WriteLine($"Cantidad: {_pedido.cantidad}");
                Console.WriteLine($"Cantidad:");
                foreach( Producto producto in _pedido.productos )
                {
                    Console.WriteLine($"Producto: {producto.nombre}");
                }
            }

            Console.ReadKey();

            Cliente cliente = new Cliente()
            {
                apellido = "Jhordi",
                nombre = "UCAN",
                rfc = "inventado" 
            };

            dc.Clientes.Add(cliente);

            Pedido pedido = new Pedido()
            {
                cantidad = 1,
                mesa = 4,
                status = 4,
                tipo_pedido = 0,
            };


            Pedido_tiene_productos productos = new Pedido_tiene_productos();

            productos.id_producto = 1;

            productos.id_pedido = 1;

            dc.Pedidos.Add(pedido);
         
            dc.SaveChanges();

            Producto p = new Producto()
            {
                nombre = "Papas fritas",
                id_pedido = pedido.id
            };

            dc.Add(p);

            dc.SaveChanges();

            Console.WriteLine("Se agrego el pedido");

            Console.ReadKey();

        }
    }
}