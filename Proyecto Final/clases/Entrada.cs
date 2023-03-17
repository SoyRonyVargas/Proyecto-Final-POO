namespace Proyecto_Final.clases
{
    public class Entrada : BaseEntity
    {
        public int id { get; set; }
        public int id_componente { get; set; }
        public int existencias_iniciales { get; set; } 
        public int existencias { get; set; } 
        public DateTime CreatedDate { get; set; }
    }
}

