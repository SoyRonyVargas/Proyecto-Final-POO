using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.clases
{
	public class Pedido
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PedidoID { get; set; }
        public int cantidad { get; set; }
        public int tipo_pedido { get; set; }
        public int mesa { get; set; }
        public int status { get; set; }
        public List<Pedido_tiene_productos> Pedido_tiene_productos { get; set; }
	}
}

