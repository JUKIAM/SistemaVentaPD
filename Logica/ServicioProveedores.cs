using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Logica
{
    public class ServicioProveedores : IServicios<Proveedor>
    {
        private RepositorioProveedor repositorioProveedor = new RepositorioProveedor();

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el documento del Proveedor\n";
            }
            else if (obj.Documento.Length < 6 || obj.Documento.Length > 15)
            {
                Mensaje += "El tamaño del documento no está dentro del rango permitido\n";
            }

            if (obj.RazonSocial == "")
            {
                Mensaje += "Es necesaria la razon social del Proveedor\n";
            }
            else if (obj.RazonSocial.Length < 6 || obj.RazonSocial.Length > 40)
            {
                Mensaje += "El tamaño de la razon social no está dentro del rango permitido\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "Es necesario el correo del Proveedor\n";
            }
            else if (!Regex.IsMatch(obj.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Mensaje += "El formato del correo no está permitido\n";
            }

            if (Mensaje != String.Empty)
            {
                return false;
            }
            else
            {
                return repositorioProveedor.Editar(obj, out Mensaje);
            }
        }

        public bool Eliminar(Proveedor obj, out string Mensaje)
        {
            return repositorioProveedor.Eliminar(obj, out Mensaje);
        }

        public List<Proveedor> Listar()
        {
            return repositorioProveedor.Listar();
        }

        public int Registrar(Proveedor obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el documento del Proveedor\n";
            }
            else if (obj.Documento.Length < 6 || obj.Documento.Length > 15)
            {
                Mensaje += "El tamaño del documento no está dentro del rango permitido\n";
            }

            if (obj.RazonSocial == "")
            {
                Mensaje += "Es necesaria la razon social del Proveedor\n";
            }
            else if (obj.RazonSocial.Length < 6 || obj.RazonSocial.Length > 40)
            {
                Mensaje += "El tamaño de la razon social no está dentro del rango permitido\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "Es necesario el correo del Proveedor\n";
            }
            else if (!Regex.IsMatch(obj.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Mensaje += "El formato del correo no está permitido\n";
            }

            if (Mensaje != String.Empty)
            {
                return 0;
            }
            else
            {
                return repositorioProveedor.Registrar(obj, out Mensaje);
            }
        }
    }
}