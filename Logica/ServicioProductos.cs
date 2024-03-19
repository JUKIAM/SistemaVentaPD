using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logica
{
    public class ServicioProductos : IServicios<Producto>
    {
        private RepositorioProducto repositorioProducto = new RepositorioProducto();

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el código del Producto\n";
            }
            else if (obj.Codigo.Length < 6 || obj.Codigo.Length > 15)
            {
                Mensaje += "El tamaño del código no está dentro del rango permitido\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre del Producto\n";
            }
            else if (obj.Nombre.Length < 6 || obj.Nombre.Length > 15)
            {
                Mensaje += "El tamaño del nombre no está dentro del rango permitido\n";
            }

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario la descripcion del Producto\n";
            }
            else if (obj.Descripcion.Length < 6 || obj.Descripcion.Length > 40)
            {
                Mensaje += "El tamaño de la descripción no está dentro del rango permitido\n";
            }

            if (Mensaje != String.Empty)
            {
                return false;
            }
            else
            {
                return repositorioProducto.Editar(obj, out Mensaje);
            }
        }

        public bool Eliminar(Producto obj, out string Mensaje)
        {
            return repositorioProducto.Eliminar(obj, out Mensaje);
        }

        public List<Producto> Listar()
        {
            return repositorioProducto.Listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el código del Producto\n";
            }
            else if (obj.Codigo.Length < 6 || obj.Codigo.Length > 15)
            {
                Mensaje += "El tamaño del código no está dentro del rango permitido\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el nombre del Producto\n";
            }
            else if (obj.Nombre.Length < 6 || obj.Nombre.Length > 15)
            {
                Mensaje += "El tamaño del nombre no está dentro del rango permitido\n";
            }

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario la descripcion del Producto\n";
            }
            else if (obj.Descripcion.Length < 6 || obj.Descripcion.Length > 40)
            {
                Mensaje += "El tamaño de la descripción no está dentro del rango permitido\n";
            }

            if (Mensaje != String.Empty)
            {
                return 0;
            }
            else
            {
                return repositorioProducto.Registrar(obj, out Mensaje);
            }
        }
    }
}