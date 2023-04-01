using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.clases
{
    public class Pedido : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int tipo_pedido { get; set; }
        public int mesa { get; set; }
        public int status { get; set; }
        public string producto { get; set; }
        public float importe { get; set; }
        public float iva { get; set; }
        public float total { get; set; }
        public int? tipo_cobro { get; set; }
    }
}

