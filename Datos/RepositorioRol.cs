using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;

namespace Datos
{
    public class RepositorioRol : ICrudBaseDatos<Rol>
    {
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
            List<Rol> lista = new List<Rol>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select IdRol,Decripcion from ROL");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Rol()
                            {
                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                Descripcion = dr["Decripcion"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {

                    lista = new List<Rol>();
                }
            }
            return lista;
        }

        public int Registrar(Rol obj, out string Mensaje)
        {
            throw new NotImplementedException();
        }
    }
}