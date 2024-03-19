using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class CL_Negocio
    {
        private CD_Negocio objcd_empresa = new CD_Negocio();

        public Negocio ObtenerDatos()
        {
            return objcd_empresa.ObtenerDatos();
        }

        public bool GuardarDatos(Negocio obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre\n";
            }

            if (obj.RUC == "")
            {
                Mensaje += "Es necesario el número de ruc\n";
            }

            if (obj.Direccion == "")
            {
                Mensaje += "Es necesario la dirección\n";
            }

            if (Mensaje != String.Empty)
            {
                return false;
            }
            else
            {
                return objcd_empresa.GuardarDatos(obj, out Mensaje);
            }
        }

        public byte[] ObtenerLogo(out bool obtenido)
        {
            return objcd_empresa.ObtenerLogo(out obtenido);
        }

        public bool ActualizarLogo(byte[] imagen,out String mensaje)
        {
            return objcd_empresa.ActualizarLogo(imagen, out mensaje);
        }

    }
}
