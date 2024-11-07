using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NSubcategoria
    {
        private DSubcategoria objd_subcategoria = new DSubcategoria();

        public List<Subcategoria> Listar()
        {
            return objd_subcategoria.Listar();
        }

        public int Registrar(Subcategoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.nombresubcategoria == "")
            {
                Mensaje += "Es necesario que ingrese una subcategoria que no se repita \n";
            }
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objd_subcategoria.Registrar(obj, out Mensaje);
            }
        }

        public bool Editar(Subcategoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.nombresubcategoria == "")
            {
                Mensaje += "Es necesario que cambie el nombre de la subcategoria \n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objd_subcategoria.Editar(obj, out Mensaje);
            }
        }

        public bool Eliminar(Subcategoria obj, out string Mensaje)
        {
            return objd_subcategoria.Eliminar(obj, out Mensaje);
        }

        public List<Subcategoria> FiltrosSubCategorias(int idcategoria)
        {
            return objd_subcategoria.FiltrosSubCategorias(idcategoria);
        }
    }
}
