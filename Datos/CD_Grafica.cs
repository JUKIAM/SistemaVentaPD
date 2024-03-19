using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

namespace Datos
{

    public class CD_Grafica
    {
        public struct IngresosPorFecha
        {
            public String Fecha { get; set; }
            public Decimal cantidadTotal { get; set; }
        }

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


        public void ObtenerNumeroElementos()
        {


            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                oconexion.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = oconexion;

                    //Obtener el numero total de clientes
                    cmd.CommandText = "SELECT COUNT(IdCliente) FROM CLIENTE";
                    NumeroClientes = (int)cmd.ExecuteScalar();


                    //Obtener el numero total de proveedores
                    cmd.CommandText = "SELECT COUNT(IdProveedor) FROM PROVEEDOR";
                    NumeroProveedores = (int)cmd.ExecuteScalar();

                    //Obtener el numero total de productos
                    cmd.CommandText = "SELECT COUNT(IdProducto) FROM PRODUCTO";
                    NumeroProductos = (int)cmd.ExecuteScalar();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT COUNT(IdVenta) FROM VENTA");
                    query.AppendLine("WHERE FechaRegistro BETWEEN @desdeFecha AND @hastaFecha");

                    SqlCommand cmd2 = new SqlCommand(query.ToString(), oconexion);
                    cmd2.Parameters.AddWithValue("@desdeFecha", fechaInicio);
                    cmd2.Parameters.AddWithValue("@hastaFecha", fechaFin);
                    cmd2.CommandType = CommandType.Text;
                    NumeroVentas = (int)cmd2.ExecuteScalar();
                }
            }
        }

        public void ObtenerAnalisisVentas()
        {
            ListaIngresosBrutos = new List<IngresosPorFecha>();
            TotalGanancias = 0;
            TotalIngresos = 0;


            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                oconexion.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = oconexion;
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT FechaRegistro, SUM(MontoTotal)");
                    query.AppendLine("FROM VENTA");
                    query.AppendLine("WHERE FechaRegistro BETWEEN @desdeFecha AND @hastaFecha");
                    query.AppendLine("GROUP BY FechaRegistro");

                    SqlCommand cmd2 = new SqlCommand(query.ToString(), oconexion);
                    cmd2.Parameters.AddWithValue("@desdeFecha", fechaInicio);
                    cmd2.Parameters.AddWithValue("@hastaFecha", fechaFin);
                    cmd2.CommandType = CommandType.Text;

                    var reader = cmd2.ExecuteReader();
                    var resultadoTabla = new List<KeyValuePair<DateTime, decimal>>();
                    while (reader.Read())
                    {
                        resultadoTabla.Add(
                            new KeyValuePair<DateTime, decimal>((DateTime)reader[0], (decimal)reader[1])
                            );
                        TotalIngresos += (decimal)(reader[1]);
                    }
                    TotalGanancias = TotalIngresos * (decimal)0.2; //20%
                    reader.Close();

                    //Agrupar por días
                    if (NumeroDias <= 30)
                    {
                        foreach (var item in resultadoTabla)
                        {
                            ListaIngresosBrutos.Add(new IngresosPorFecha()
                            {
                                Fecha = item.Key.ToString("dd MMM"),
                                cantidadTotal = item.Value
                            });
                        }
                    }

                    //Agrupar por semanas
                    else if (NumeroDias <= 92)
                    {
                        ListaIngresosBrutos = (from listaVenta in resultadoTabla
                                               group listaVenta by CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                                   listaVenta.Key, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                                   into venta
                                               select new IngresosPorFecha
                                               {
                                                   Fecha = "Semana " + venta.Key.ToString(),
                                                   cantidadTotal = venta.Sum(cantidad => cantidad.Value)
                                               }).ToList();
                    }

                    //Agrupar por meses
                    else if (NumeroDias <= (365 * 2))
                    {
                        bool esAño = NumeroDias <= 365;
                        ListaIngresosBrutos = (from listaVenta in resultadoTabla
                                               group listaVenta by listaVenta.Key.ToString("MMM yyyy")
                                                   into venta
                                               select new IngresosPorFecha
                                               {
                                                   Fecha = esAño ? venta.Key.Substring(0, venta.Key.IndexOf(" ")) : venta.Key,
                                                   cantidadTotal = venta.Sum(cantidad => cantidad.Value)
                                               }).ToList();
                    }

                    //Agrupar por años
                    else
                    {
                        ListaIngresosBrutos = (from listaVenta in resultadoTabla
                                               group listaVenta by listaVenta.Key.ToString("yyyy")
                                                   into venta
                                               select new IngresosPorFecha
                                               {
                                                   Fecha = venta.Key,
                                                   cantidadTotal = venta.Sum(cantidad => cantidad.Value)
                                               }).ToList();
                    }
                }
            }
        }

        public void ObtenerAnalisisProductos()
        {
            ProductosMasVendidos = new List<KeyValuePair<string, int>>();
            ProductosBajoStock = new List<KeyValuePair<string, int>>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                oconexion.Open();

                StringBuilder query = new StringBuilder();
                query.AppendLine("SELECT TOP 5 P.Nombre, SUM(DETALLE_VENTA.Cantidad) AS Q");
                query.AppendLine("FROM DETALLE_VENTA");
                query.AppendLine("INNER JOIN PRODUCTO P ON P.IdProducto = DETALLE_VENTA.IdProducto");
                query.AppendLine("INNER JOIN VENTA V ON V.IdVenta = DETALLE_VENTA.IdVenta");
                query.AppendLine("WHERE V.FechaRegistro BETWEEN @desdeFecha AND @hastaFecha");
                query.AppendLine("GROUP BY P.Nombre");
                query.AppendLine("ORDER BY Q DESC");

                SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                cmd.Parameters.AddWithValue("@desdeFecha", fechaInicio);
                cmd.Parameters.AddWithValue("@hastaFecha", fechaFin);
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ProductosMasVendidos.Add(
                        new KeyValuePair<string, int>(dr[0].ToString(), (int)dr[1]));
                    }
                }

                StringBuilder query1 = new StringBuilder();
                query1.AppendLine("SELECT Nombre, Stock");
                query1.AppendLine("FROM PRODUCTO");
                query1.AppendLine("WHERE Stock <= 7 AND Estado = 1");

                SqlCommand cmd1 = new SqlCommand(query1.ToString(), oconexion);
                cmd1.CommandType = CommandType.Text;

                using (SqlDataReader dr = cmd1.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ProductosBajoStock.Add(
                        new KeyValuePair<string, int>(dr[0].ToString(), (int)dr[1]));
                    }
                }
            }
        }

        public bool CargarDatos(DateTime fechaInicio, DateTime fechaFin)
        {
            
            fechaFin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day, fechaFin.Hour, fechaFin.Minute, 59);
            if (fechaInicio != this.fechaInicio || fechaFin != this.fechaFin)
            {
                this.fechaInicio = fechaInicio;
                this.fechaFin = fechaFin;
                this.NumeroDias = (fechaFin - fechaInicio).Days;

                ObtenerNumeroElementos();
                ObtenerAnalisisProductos();
                ObtenerAnalisisVentas();

                Console.WriteLine("Datos actualizados: {0} - {1}", fechaInicio.ToString(), fechaFin.ToString());
                return true;
            }
            else
            {
                Console.WriteLine("Datos no actualizados, es la misma consulta: {0} - {1}", fechaInicio.ToString(), fechaFin.ToString());
                return false;
            }
        }
    }
}
