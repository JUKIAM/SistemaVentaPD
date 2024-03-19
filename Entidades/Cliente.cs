using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Cliente : Persona
    {
        public String NombreCompleto { get; set; }
        public String Telefono { get; set; }
        public bool Estado { get; set; }
        public String FechaRegistro { get; set; }
    }
}
