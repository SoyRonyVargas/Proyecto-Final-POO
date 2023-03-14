using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.clases
{
	public class Pedido_tiene_productos
	{
        [Key]
        public int id { get; set; }

        public int id_pedido { get; set; }
        public int id_producto { get; set; }

        public Pedido pedido { get; set; }
        public Producto producto { get; set; }


    }
}

