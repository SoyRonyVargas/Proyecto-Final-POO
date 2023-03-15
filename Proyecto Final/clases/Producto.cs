using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.clases
{
    public class Producto
	{
        
        public int id { get; set; }
		public string nombre { get; set; }
        public virtual List<Producto> componentes { get; set;} 

        public float precio { get; set; }
    }

}

