using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.clases
{
    public class Producto_tiene_componentes
    {
        [Key]
        public int id { get; set; }
        public int id_producto { get; set; }
        public int id_componente { get; set; }
    }
}
