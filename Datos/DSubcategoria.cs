using Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DSubcategoria
    {
        public List<Subcategoria> Listar()
        {
            List<Subcategoria> lista = new List<Subcategoria>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select idsubcategorias, c.idcategoria, c.nombrecategoria, nombresubcategoria, sb.estado, CONVERT(VARCHAR(10), sb.fecharegistro, 120)AS fecharegistro_tallas from subcategorias sb");
                    query.AppendLine("inner join categorias c on c.idcategoria = sb.idcategoria");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Subcategoria()
                            {
                                idsubcategorias = Convert.ToInt32(dr["idsubcategorias"]),
                                nombresubcategoria = dr["nombresubcategoria"].ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                oCategoria = new Categoria() { idcategoria = Convert.ToInt32(dr["idcategoria"]), nombrecategoria = dr["nombrecategoria"].ToString() },
                                fecharegistro = dr["fecharegistro_tallas"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Subcategoria>();
                }
            }
            return lista;
        }

        public int Registrar(Subcategoria obj, out string Mensaje)
        {
            int idsubcategoriagenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("spu_registrar_subcategorias", oconexion);
                    cmd.Parameters.AddWithValue("nombresubcategoria", obj.nombresubcategoria);
                    cmd.Parameters.AddWithValue("idcategoria", obj.oCategoria.idcategoria);
                    cmd.Parameters.AddWithValue("estado", obj.estado);
                    cmd.Parameters.AddWithValue("fecharegistro", Convert.ToDateTime(obj.fecharegistro));
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    idsubcategoriagenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idsubcategoriagenerado = 0;
                Mensaje = ex.Message;
            }
            return idsubcategoriagenerado;
        }

        public bool Editar(Subcategoria obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("spu_editar_subcategoria", oconexion);
                    cmd.Parameters.AddWithValue("idcategoria", obj.oCategoria.idcategoria);
                    cmd.Parameters.AddWithValue("idsubcategorias", obj.idsubcategorias);
                    cmd.Parameters.AddWithValue("estado", obj.estado);
                    cmd.Parameters.AddWithValue("nombresubcategoria", obj.nombresubcategoria);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

        public bool Eliminar(Subcategoria obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("spu_eliminar_subcategoria", oconexion);
                    cmd.Parameters.AddWithValue("idsubcategorias", obj.idsubcategorias);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

        public List<Subcategoria> FiltrosSubCategorias(int idcategoria)
        {
            List<Subcategoria> lista = new List<Subcategoria>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT sb.idsubcategorias, c.idcategoria, c.nombrecategoria, sb.nombresubcategoria, sb.estado, CONVERT(VARCHAR(10), sb.fecharegistro, 120) AS fecharegistro_subcategorias");
                    query.AppendLine("FROM subcategorias sb");
                    query.AppendLine("INNER JOIN categorias c ON c.idcategoria = sb.idcategoria");
                    query.AppendLine("WHERE c.estado = 1 AND sb.estado = 1 AND c.idcategoria = @idcategoria;");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Subcategoria()
                            {
                                idsubcategorias = Convert.ToInt32(dr["idsubcategorias"]),
                                nombresubcategoria = dr["nombresubcategoria"].ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                oCategoria = new Categoria() { idcategoria = Convert.ToInt32(dr["idcategoria"]), nombrecategoria = dr["nombrecategoria"].ToString() },
                                fecharegistro = dr["fecharegistro_subcategorias"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Subcategoria>();
                }
            }
            return lista;
        }

    }
}
