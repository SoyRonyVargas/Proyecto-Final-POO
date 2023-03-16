using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Proyecto_Final.clases
{
    public class Producto
	{
        [Key]    
        public int id { get; set; }
		public string nombre { get; set; }
        public float precio { get; set; }
    }
}

