using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logica
{
    public class ServicioRoles : IServicios<Rol>
    {
        private RepositorioRol repositorioRol = new RepositorioRol();

        public bool Editar(Rol obj, out string Mensaje)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(Rol obj, out string Mensaje)
        {
            throw new NotImplementedException();
        }

        public List<Rol> Listar()
        {
            return repositorioRol.Listar();
        }

        public int Registrar(Rol obj, out string Mensaje)
        {
            throw new NotImplementedException();
        }
    }
}