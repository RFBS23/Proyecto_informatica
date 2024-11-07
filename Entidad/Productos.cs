using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class Productos
    {
        public int idproducto { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Categoria oCategoria { get; set; }
        public Subcategoria oSubcategoria { get; set; }
        public int stock { get; set; }
        public string modelo {  get; set; }
        public string marca { get; set; }
        public decimal precioventa { get; set; }
        public int descuento { get; set; }
        public string garantia { get; set; }
        public string eficiencia_energetica { get; set; }
        public string ubicacion {  get; set; }
        public bool estado { get; set; }
        public string fecharegistro { get; set; }
    }
}
