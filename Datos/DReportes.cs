﻿using Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DReportes
    {
        public List<ReporteVentas> Ventas(string fechainicio, string fechafin)
        {
            List<ReporteVentas> lista = new List<ReporteVentas>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    SqlCommand cmd = new SqlCommand("spu_reporte_venta", oconexion);
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteVentas()
                            {
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                tipodocumento = dr["tipodocumento"].ToString(),
                                numerodocumento = dr["numerodocumento"].ToString(),
                                montototal = dr["montototal"].ToString(),
                                UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                                documentocliente = dr["documentocliente"].ToString(),
                                nombrecliente = dr["nombrecliente"].ToString(),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                Subcategoria = dr["Subcategoria"].ToString(),
                                cantidad = dr["cantidad"].ToString(),
                                Modelo = dr["Modelo"].ToString(),
                                Marca = dr["Marca"].ToString(),
                                precioventa = dr["precioventa"].ToString(),
                                Descuento = dr["Descuento"].ToString(),
                                Garantia = dr["Garantia"].ToString(),
                                Eficiencia = dr["Eficiencia"].ToString(),
                                subtotal = dr["subtotal"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ReporteVentas>();
                }
            }
            return lista;
        }
    }
}
