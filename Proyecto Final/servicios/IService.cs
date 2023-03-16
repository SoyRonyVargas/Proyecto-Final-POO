using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.servicios
{
    public interface IService
    {
        public int listar();
        public int crear();
        public int eliminar();
        public int mostrarMenu();
    }
}
