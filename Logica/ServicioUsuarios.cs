using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logica
{
    public class ServicioUsuarios : IServicios<Usuario>
    {

        private RepositorioUsuario repositorioUsuario = new RepositorioUsuario();

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el documento del usuario\n";
            }
            else if (obj.Documento.Length < 6 || obj.Documento.Length > 15)
            {
                Mensaje += "El tamaño del documento no está dentro del rango permitido\n";
            }

            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario el nombre del usuario\n";
            }
            else if (obj.NombreCompleto.Length < 6 || obj.NombreCompleto.Length > 40)
            {
                Mensaje += "El tamaño del nombre no está dentro del rango permitido\n";
            }

            if (obj.Clave == "")
            {
                Mensaje += "Es necesario la clave del usuario\n";
            }
            else if (obj.Clave.Length < 5 || obj.Clave.Length > 15)
            {
                Mensaje += "El tamaño de la clave no está dentro del rango permitido\n";
            }

            if (Mensaje != String.Empty)
            {
                return false;
            }
            else
            {
                return repositorioUsuario.Editar(obj, out Mensaje);
            }
        }

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            return repositorioUsuario.Eliminar(obj, out Mensaje);
        }

        public List<Usuario> Listar()
        {
            return repositorioUsuario.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el documento del usuario\n";
            }
            else if (obj.Documento.Length < 6 || obj.Documento.Length > 15)
            {
                Mensaje += "El tamaño del documento no está dentro del rango permitido\n";
            }

            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario el nombre del usuario\n";
            }
            else if (obj.NombreCompleto.Length < 6 || obj.NombreCompleto.Length > 40)
            {
                Mensaje += "El tamaño del nombre no está dentro del rango permitido\n";
            }

            if (obj.Clave == "")
            {
                Mensaje += "Es necesario la clave del usuario\n";
            }
            else if (obj.Clave.Length < 5 || obj.Clave.Length > 15)
            {
                Mensaje += "El tamaño de la clave no está dentro del rango permitido\n";
            }

            if (Mensaje != String.Empty)
            {
                return 0;
            }
            else
            {
                return repositorioUsuario.Registrar(obj, out Mensaje);
            }
        }
    }
}