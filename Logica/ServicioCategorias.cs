using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logica
{
    public class ServicioCategorias : IServicios<Categoria>
    {
        private RepositorioCategoria repositorioCategoria = new RepositorioCategoria();

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario la descripcion de la Categoria\n";
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
                return repositorioCategoria.Editar(obj, out Mensaje);
            }
        }

        public bool Eliminar(Categoria obj, out string Mensaje)
        {
            return repositorioCategoria.Eliminar(obj, out Mensaje);
        }

        public List<Categoria> Listar()
        {
            return repositorioCategoria.Listar();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario la descripcion de la Categoria\n";
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
                return repositorioCategoria.Registrar(obj, out Mensaje);
            }
        }
    }
}