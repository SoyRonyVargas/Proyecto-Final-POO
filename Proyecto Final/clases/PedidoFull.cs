﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.clases
{
    public class PedidoFull
    {
        public Pedido pedido { get; set; }
        public List<Producto> productos { get; set; }
        public List<Pedido_tiene_productos> pedido_tiene_productos { get; set; }
    }
}