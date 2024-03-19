using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Logica
{
    public class ServicioClientes : IServicios<Cliente>
    {
        private RepositorioCliente repositorioCliente = new RepositorioCliente();

        public bool Editar(Cliente obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el documento del Cliente\n";
            }
            else if (obj.Documento.Length < 6 || obj.Documento.Length > 15)
            {
                Mensaje += "El tamaño del documento no está dentro del rango permitido\n";
            }

            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario el nombre del Cliente\n";
            }
            else if (obj.NombreCompleto.Length < 6 || obj.NombreCompleto.Length > 40)
            {
                Mensaje += "El tamaño del nombre no está dentro del rango permitido\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "Es necesario el correo del Cliente\n";
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
                return repositorioCliente.Editar(obj, out Mensaje);
            }
        }

        public bool Eliminar(Cliente obj, out string Mensaje)
        {
            return repositorioCliente.Eliminar(obj, out Mensaje);
        }

        public List<Cliente> Listar()
        {
            return repositorioCliente.Listar();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el documento del Cliente\n";
            }
            else if (obj.Documento.Length < 6 || obj.Documento.Length > 15)
            {
                Mensaje += "El tamaño del documento no está dentro del rango permitido\n";
            }

            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario el nombre del Cliente\n";
            }
            else if (obj.NombreCompleto.Length < 6 || obj.NombreCompleto.Length > 40)
            {
                Mensaje += "El tamaño del nombre no está dentro del rango permitido\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "Es necesario el correo del Cliente\n";
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
                return repositorioCliente.Registrar(obj, out Mensaje);
            }
        }
    }
}