using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{

    public struct IngresosPorFecha
    {
        public String Fecha { get; set; }
        public Decimal cantidadTotal { get; set; }
    }

    public class GraficaDao
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int NumeroDias { get; set; }
        public int NumeroClientes { get; set; }
        public int NumeroProveedores { get; set; }
        public int NumeroProductos { get; set; }
        public List<KeyValuePair<String, int>> ProductosMasVendidos { get; set; }
        public List<KeyValuePair<String, int>> ProductosBajoStock { get; set; }
        public List<IngresosPorFecha> ListaIngresosBrutos { get; set; }
        public int NumeroVentas { get; set; }
        public Decimal TotalIngresos { get; set; }
        public Decimal TotalGanancias { get; set; }
    }
}
