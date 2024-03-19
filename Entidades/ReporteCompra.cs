using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ReporteCompra
    {
        public String FechaRegistro { get; set; }
        public String TipoDocumento { get; set; }
        public String NumeroDocumento { get; set; }
        public String MontoTotal { get; set; }
        public String UsuarioComprador { get; set; }
        public String DocumentoProveedor { get; set; }
        public String RazonSocial { get; set; }
        public String CodigoProducto { get; set; }
        public String NombreProducto { get; set; }
        public String Categoria { get; set; }
        public String PrecioCompra { get; set; }
        public String PrecioVenta { get; set; }
        public String Cantidad { get; set; }
        public String SubTotal { get; set; }
    }
}
