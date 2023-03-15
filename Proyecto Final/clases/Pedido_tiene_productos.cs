using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.clases
{
	public class Pedido_tiene_productos
	{
        public int id { get; set; }
        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }
    }
}

