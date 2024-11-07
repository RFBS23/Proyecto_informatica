using Datos;
using Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NProductos
    {
        private DProductos objd_producto = new DProductos();

        public List<Productos> Listar()
        {
            return objd_producto.Listar();
        }

        public int Registrar(Productos obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.codigo == "")
            {
                Mensaje += "Es necesario que ingrese el codigo del producto \n";
            }
            if (obj.nombre == "")
            {
                Mensaje += "• Ingrese el nombre del producto \n";
            }
            if (obj.descripcion == "")
            {
                Mensaje += "• Ingrese una descripcion \n";
            }
            if (obj.stock <= 0)
            {
                Mensaje += "• El precio del producto debe ser mayor o igual a 0 \n";
            }
            if (obj.modelo == "")
            {
                Mensaje += "• Ingrese el modelo del producto \n";
            }
            if (obj.marca == "")
            {
                Mensaje += "• La marca esta vacio \n";
            }
            if (obj.eficiencia_energetica == "")
            {
                Mensaje += "• Ingrese la eficiencia energetica \n";
            }
            if (obj.precioventa <= 0)
            {
                Mensaje += "• El precio del producto debe ser mayor a 0 \n";
            }
            if (obj.ubicacion == "")
            {
                Mensaje += "• Debe Seleccionar la Ubicacion del productos \n";
            }
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objd_producto.Registrar(obj, out Mensaje);
            }
        }

        public bool Editar(Productos obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.codigo == "")
            {
                Mensaje += "Es necesario que ingrese el codigo del producto \n";
            }
            if (obj.nombre == "")
            {
                Mensaje += "• Ingrese el nombre del producto \n";
            }
            if (obj.descripcion == "")
            {
                Mensaje += "• Ingrese una descripcion \n";
            }
            if (obj.stock <= 0)
            {
                Mensaje += "• El precio del producto debe ser mayor o igual a 0 \n";
            }
            if (obj.modelo == "")
            {
                Mensaje += "• Ingrese el modelo del producto \n";
            }
            if (obj.marca == "")
            {
                Mensaje += "• La marca esta vacio \n";
            }
            if (obj.eficiencia_energetica == "")
            {
                Mensaje += "• Ingrese la eficiencia energetica \n";
            }
            if (obj.precioventa <= 0)
            {
                Mensaje += "• El precio del producto debe ser mayor a 0 \n";
            }
            if (obj.ubicacion == "")
            {
                Mensaje += "• Debe Seleccionar la Ubicacion del productos \n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objd_producto.Editar(obj, out Mensaje);
            }
        }

        public bool Eliminar(Productos obj, out string Mensaje)
        {
            return objd_producto.Eliminar(obj, out Mensaje);
        }

        public int CantidadProductos()
        {
            return objd_producto.CantidadProductos();
        }

    }
}
