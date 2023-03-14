
using Proyecto_Final.clases;

class Program
{
    static void Main(string[] args)
    {
        using (RestauranteDataContext dc = new RestauranteDataContext())
        {

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

            Producto p = new Producto()
            {
                nombre = "test"
            };

            Producto p2 = new Producto()
            {
                nombre = "test 2"
            };

            dc.Add(p);

            dc.Add(p2);

            dc.Pedidos.Add(pedido);

            //dc.Add()

            Pedido_tiene_productos productos = new Pedido_tiene_productos();

            productos.id_producto = 1;

            productos.id_pedido = 1;

            //productos.producto

            //dc..Add(productos);

            dc.SaveChanges();

            Console.WriteLine("Se agrego el pedido");

            Console.ReadKey();

        }
    }
}