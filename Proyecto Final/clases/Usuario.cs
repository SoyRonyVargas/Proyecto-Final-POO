using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.clases
{
    public class Usuario
	{
        [Key]    
        public int id { get; set; }
		public string? nombre { get; set; }
		public string? apellidos { get; set; }
		public string? password { get; set; }
		public string correo { get; set; }
    }
}

