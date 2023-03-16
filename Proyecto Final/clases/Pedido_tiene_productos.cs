using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Proyecto_Final.clases
{
	public class Pedido_tiene_productos
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int cantidad { get; set; }
        public int id_producto { get; set; }
        public int id_pedido { get; set; }
    }
}

