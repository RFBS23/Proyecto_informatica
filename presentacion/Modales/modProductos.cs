using DocumentFormat.OpenXml.Wordprocessing;
using Entidad;
using Negocio;
using presentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion.Modales
{
    public partial class modProductos : Form
    {
        public Productos _Productos { get; set; }
        public modProductos()
        {
            InitializeComponent();
        }

        private void modProductos_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn columna in tablaproductos.Columns)
            {

                if (columna.Visible == true)
                {
                    listabuscar.Items.Add(new OpcionesComboBox() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            listabuscar.DisplayMember = "Texto";
            listabuscar.ValueMember = "Valor";
            listabuscar.SelectedIndex = 0;

            List<Productos> listaProductos = new NProductos().Listar();
            foreach (Productos item in listaProductos)
            {
                tablaproductos.Rows.Add(new object[] { "", item.idproducto, item.codigo, item.nombre, item.descripcion, item.oCategoria.idcategoria, item.oCategoria.nombrecategoria, item.oSubcategoria.idsubcategorias, item.oSubcategoria.nombresubcategoria, item.stock, item.modelo, item.marca, item.precioventa, item.descuento, item.garantia, item.eficiencia_energetica, item.ubicacion, item.estado == true ? 1 : 0, item.estado == true ? "Activo" : "Inactivo", item.fecharegistro });
            }
        }

        private void tablaproductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if (iRow >= 0 && iColum > 0)
            {
                _Productos = new Productos()
                {
                    idproducto = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["idproducto"].Value.ToString()),
                    codigo = tablaproductos.Rows[iRow].Cells["codigo"].Value.ToString(),
                    nombre = tablaproductos.Rows[iRow].Cells["nombre"].Value.ToString(),
                    descripcion = tablaproductos.Rows[iRow].Cells["descripcion"].Value.ToString(),
                    oCategoria = new Categoria()
                    {
                        idcategoria = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["idcategoria"].Value.ToString()),
                        nombrecategoria = tablaproductos.Rows[iRow].Cells["nombrecategoria"].Value.ToString()
                    },
                    oSubcategoria = new Subcategoria()
                    {
                        idsubcategorias = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["idsubcategorias"].Value.ToString()),
                        nombresubcategoria = tablaproductos.Rows[iRow].Cells["nombresubcategoria"].Value.ToString()
                    },
                    stock =  Convert.ToInt32(tablaproductos.Rows[iRow].Cells["stock"].Value.ToString()),
                    modelo = tablaproductos.Rows[iRow].Cells["modelo"].Value.ToString(),
                    marca = tablaproductos.Rows[iRow].Cells["marca"].Value.ToString(),
                    precioventa = Convert.ToDecimal(tablaproductos.Rows[iRow].Cells["precioventa"].Value.ToString()),
                    descuento = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["descuento"].Value.ToString()),
                    garantia = tablaproductos.Rows[iRow].Cells["garantia"].Value.ToString(),
                    eficiencia_energetica = tablaproductos.Rows[iRow].Cells["eficiencia_energetica"].Value.ToString(),
                    ubicacion = tablaproductos.Rows[iRow].Cells["ubicacion"].Value.ToString()
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnbuscarlista_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionesComboBox)listabuscar.SelectedItem).Valor.ToString();

            if (tablaproductos.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in tablaproductos.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbuscar.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbuscar.Text = "";
            foreach (DataGridViewRow row in tablaproductos.Rows)
            {
                row.Visible = true;
            }
        }

        private void tablaproductos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.check1.Width;
                var h = Properties.Resources.check1.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.check1, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void tablaproductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tablaproductos.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                int iRow = e.RowIndex;
                if (iRow >= 0)
                {
                    _Productos = new Productos()
                    {
                        idproducto = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["idproducto"].Value.ToString()),
                        codigo = tablaproductos.Rows[iRow].Cells["codigo"].Value.ToString(),
                        nombre = tablaproductos.Rows[iRow].Cells["nombre"].Value.ToString(),
                        descripcion = tablaproductos.Rows[iRow].Cells["descripcion"].Value.ToString(),
                        oCategoria = new Categoria()
                        {
                            idcategoria = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["idcategoria"].Value.ToString()),
                            nombrecategoria = tablaproductos.Rows[iRow].Cells["nombrecategoria"].Value.ToString()
                        },
                        oSubcategoria = new Subcategoria()
                        {
                            idsubcategorias = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["idsubcategorias"].Value.ToString()),
                            nombresubcategoria = tablaproductos.Rows[iRow].Cells["nombresubcategoria"].Value.ToString()
                        },
                        stock = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["stock"].Value.ToString()),
                        modelo = tablaproductos.Rows[iRow].Cells["modelo"].Value.ToString(),
                        marca = tablaproductos.Rows[iRow].Cells["marca"].Value.ToString(),
                        precioventa = Convert.ToDecimal(tablaproductos.Rows[iRow].Cells["precioventa"].Value.ToString()),
                        descuento = Convert.ToInt32(tablaproductos.Rows[iRow].Cells["descuento"].Value.ToString()),
                        garantia = tablaproductos.Rows[iRow].Cells["garantia"].Value.ToString(),
                        eficiencia_energetica = tablaproductos.Rows[iRow].Cells["eficiencia_energetica"].Value.ToString(),
                        ubicacion = tablaproductos.Rows[iRow].Cells["ubicacion"].Value.ToString()
                    };
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
