﻿using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.clases
{
    public class Producto : BaseEntity
	{
        [Key]    
        public int id { get; set; }
		public string nombre { get; set; }
		public double precio { get; set; }
    }
}

