using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario : Persona
    {
        public String NombreCompleto { get; set; }
        public String Clave { get; set; }
        public Rol oRol { get; set; }
        public bool Estado { get; set; }
        public String FechaRegistro { get; set; }
    }
}
