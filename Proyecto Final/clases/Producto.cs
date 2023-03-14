using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.clases
{
	public class Producto
	{
		[Key]
		public int id { get; set; }

        public int id_pedido { get; set; }

		public string nombre { get; set; }

        [ForeignKey("id_pedido")]
        public Pedido pedido { get; set; }

    }
}

