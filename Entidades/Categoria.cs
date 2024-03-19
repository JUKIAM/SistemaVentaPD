using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public String Descripcion { get; set; }
        public bool Estado { get; set; }
        public String FechaRegistro { get; set; }
    }
}
