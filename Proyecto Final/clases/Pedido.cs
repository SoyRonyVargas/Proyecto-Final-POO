using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.clases
{
	public class Pedido
	{
        [Key]
        public int id { get; set; }
        public int cantidad { get; set; }
        public int tipo_pedido { get; set; }
        public int mesa { get; set; }
        public int status { get; set; }

        public ICollection<Producto> productos { get; set; }
	}
}

