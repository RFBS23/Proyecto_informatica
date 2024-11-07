using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class Subcategoria
    {
        public int idsubcategorias { get; set; }
        public string nombresubcategoria { get; set; }
        public Categoria oCategoria { get; set; }
        public bool estado { get; set; }
        public string fecharegistro { get; set; }
    }
}
