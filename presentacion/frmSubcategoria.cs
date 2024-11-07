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

namespace presentacion
{
    public partial class frmSubcategoria : Form
    {
        public frmSubcategoria()
        {
            InitializeComponent();
        }

        private void frmSubcategoria_Load(object sender, EventArgs e)
        {
            cbcategoria.Items.Clear();

            DateTime fechaActual = DateTime.Now;
            lblfecha.Text = $"{fechaActual.Year}-{fechaActual.Month}-{fechaActual.Day}";

            List<Categoria> listacat = new NCategorias().ListarCategorias();
            cbcategoria.Items.Add(new OpcionesComboBox() { Valor = 0, Texto = "Elija una categoria" });
            foreach (Categoria item in listacat)
            {
                cbcategoria.Items.Add(new OpcionesComboBox() { Valor = item.idcategoria, Texto = item.nombrecategoria });
            }
            cbcategoria.DisplayMember = "Texto";
            cbcategoria.ValueMember = "Valor";
            cbcategoria.SelectedIndex = 0;

            cbestado.Items.Add(new OpcionesComboBox() { Valor = 1, Texto = "Activo" });
            cbestado.Items.Add(new OpcionesComboBox() { Valor = 0, Texto = "Inactivo" });
            cbestado.DisplayMember = "Texto";
            cbestado.ValueMember = "Valor";
            cbestado.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in tablasubcategorias.Columns)
            {
                if (columna.Visible == true)
                {
                    listabuscar.Items.Add(new OpcionesComboBox() { Valor = columna.Name, Texto = columna.HeaderText });
                }
                listabuscar.DisplayMember = "Texto";
                listabuscar.ValueMember = "Valor";
                listabuscar.SelectedIndex = 0;
            }

            List<Subcategoria> listasubcategoria = new NSubcategoria().Listar();
            foreach (Subcategoria item in listasubcategoria)
            {
                tablasubcategorias.Rows.Add(new object[] { "", item.idsubcategorias, item.oCategoria.idcategoria, item.oCategoria.nombrecategoria, item.nombresubcategoria, item.estado == true ? 1 : 0, item.estado == true ? "Activo" : "Inactivo", item.fecharegistro });
            }
        }

        private void Limpiar()
        {
            txtindice.Text = "-1";
            txtid.Text = "0";

            txtnombresubcategoria.Text = "";
            cbcategoria.SelectedIndex = 0;
            cbestado.SelectedIndex = 0;

            txtnombresubcategoria.Select();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Subcategoria objsubcategoria = new Subcategoria()
            {
                idsubcategorias = Convert.ToInt32(txtid.Text),
                nombresubcategoria = txtnombresubcategoria.Text,
                oCategoria = new Categoria() { idcategoria = Convert.ToInt32(((OpcionesComboBox)cbcategoria.SelectedItem).Valor) },
                estado = Convert.ToInt32(((OpcionesComboBox)cbestado.SelectedItem).Valor) == 1 ? true : false,
                fecharegistro = lblfecha.Text
            };
            if(objsubcategoria.idsubcategorias == 0 | btnAgregar.Text == "    Agregar")
            {
                int idsubcategoriagenerado = new NSubcategoria().Registrar(objsubcategoria, out mensaje);

                if (idsubcategoriagenerado != 0)
                {
                    tablasubcategorias.Rows.Add(new object[] { "", idsubcategoriagenerado,
                        ((OpcionesComboBox)cbcategoria.SelectedItem).Valor.ToString(),
                        ((OpcionesComboBox)cbcategoria.SelectedItem).Texto.ToString(),
                        txtnombresubcategoria.Text,
                        ((OpcionesComboBox)cbestado.SelectedItem).Valor.ToString(),
                        ((OpcionesComboBox)cbestado.SelectedItem).Texto.ToString(),
                        lblfecha.Text });
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
            else if (btnAgregar.Text == "    Editar")
            {
                bool resultado = new NSubcategoria().Editar(objsubcategoria, out mensaje);
                if (resultado)
                {
                    DataGridViewRow row = tablasubcategorias.Rows[Convert.ToInt32(txtindice.Text)];
                    row.Cells["idsubcategorias"].Value = txtid.Text;
                    row.Cells["idcategoria"].Value = ((OpcionesComboBox)cbcategoria.SelectedItem).Valor.ToString();
                    row.Cells["nombrecategoria"].Value = ((OpcionesComboBox)cbcategoria.SelectedItem).Texto.ToString();
                    row.Cells["nombresubcategoria"].Value = txtnombresubcategoria.Text;
                    row.Cells["valorestado"].Value = ((OpcionesComboBox)cbestado.SelectedItem).Valor.ToString();
                    row.Cells["estado"].Value = ((OpcionesComboBox)cbestado.SelectedItem).Texto.ToString();
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
                btnAgregar.Text = "    Agregar";
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtid.Text) != 0)
            {
                if (MessageBox.Show("¿Desea eliminar la subcategoria?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string mensaje = string.Empty;
                    Subcategoria objtallas = new Subcategoria()
                    {
                        idsubcategorias = Convert.ToInt32(txtid.Text)
                    };

                    bool respuesta = new NSubcategoria().Eliminar(objtallas, out mensaje);

                    if (respuesta)
                    {
                        tablasubcategorias.Rows.RemoveAt(Convert.ToInt32(txtindice.Text));
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
            }
            Limpiar();
        }

        private void txtnombresubcategoria_TextChanged(object sender, EventArgs e)
        {
            int selectionStart = txtnombresubcategoria.SelectionStart;
            int selectionLength = txtnombresubcategoria.SelectionLength;
            txtnombresubcategoria.Text = txtnombresubcategoria.Text.ToUpper();
            txtnombresubcategoria.SelectionStart = selectionStart;
            txtnombresubcategoria.SelectionLength = selectionLength;
        }

        private void txtbuscar_TextChanged(object sender, EventArgs e)
        {
            int selectionStart = txtbuscar.SelectionStart;
            int selectionLength = txtbuscar.SelectionLength;
            txtbuscar.Text = txtbuscar.Text.ToUpper();
            txtbuscar.SelectionStart = selectionStart;
            txtbuscar.SelectionLength = selectionLength;
        }

        private void btnbuscarlista_Click(object sender, EventArgs e)
        {
            String columnaFiltro = ((OpcionesComboBox)listabuscar.SelectedItem).Valor.ToString();
            if (tablasubcategorias.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in tablasubcategorias.Rows)
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
            foreach (DataGridViewRow row in tablasubcategorias.Rows)
            {
                row.Visible = true;
            }
            txtbuscar.Select();
        }

        private void tablasubcategorias_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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

        private void tablasubcategorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tablasubcategorias.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    txtindice.Text = indice.ToString();
                    txtid.Text = tablasubcategorias.Rows[indice].Cells["idsubcategorias"].Value.ToString();
                    foreach (OpcionesComboBox ocb in cbcategoria.Items)
                    {
                        if (Convert.ToInt32(ocb.Valor) == Convert.ToInt32(tablasubcategorias.Rows[indice].Cells["idcategoria"].Value))
                        {
                            int indice_combo = cbcategoria.Items.IndexOf(ocb);
                            cbcategoria.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                    txtnombresubcategoria.Text = tablasubcategorias.Rows[indice].Cells["nombresubcategoria"].Value.ToString();
                    if (int.TryParse(tablasubcategorias.Rows[indice].Cells["valorestado"].Value?.ToString(), out int estadoValor))
                    {
                        foreach (OpcionesComboBox oc in cbestado.Items)
                        {
                            if (Convert.ToInt32(oc.Valor) == estadoValor)
                            {
                                int indice_combo = cbestado.Items.IndexOf(oc);
                                cbestado.SelectedIndex = indice_combo;
                                break;
                            }
                        }
                    }
                    else
                    {
                        cbestado.SelectedIndex = 0;
                    }
                    btnAgregar.Text = "    Editar";
                }
            }
        }
    }
}
