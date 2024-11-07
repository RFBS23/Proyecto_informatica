using ClosedXML.Excel;
using CustomAlertBoxDemo;
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
    public partial class frmListarProductos : Form
    {
        public frmListarProductos()
        {
            InitializeComponent();
        }

        public void Alert(string msg, Form_Alert.enmType type)
        {
            Form_Alert frm = new Form_Alert();
            frm.showAlert(msg, type);
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn columna in tablaproductos.Columns)
            {
                if (columna.Visible == true)
                {
                    listabuscar.Items.Add(new OpcionesComboBox() { Valor = columna.Name, Texto = columna.HeaderText });
                }
                listabuscar.DisplayMember = "Texto";
                listabuscar.ValueMember = "Valor";
                listabuscar.SelectedIndex = 0;
            }

            List<Productos> listaProductos = new NProductos().Listar();
            foreach (Productos item in listaProductos)
            {
                tablaproductos.Rows.Add(new object[] { "", item.idproducto, item.codigo, item.nombre, item.descripcion, item.oCategoria.idcategoria, item.oCategoria.nombrecategoria, item.oSubcategoria.idsubcategorias, item.oSubcategoria.nombresubcategoria, item.stock, item.modelo, item.marca, item.precioventa, item.descuento, item.garantia, item.eficiencia_energetica, item.ubicacion, item.estado == true ? 1 : 0, item.estado == true ? "Activo" : "Inactivo", item.fecharegistro });
            }
        }

        private void tablaproductos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.tablaproductos.Columns[e.ColumnIndex].Name == "stock")
            {
                if (e.Value != null && e.Value != DBNull.Value)
                {
                    int stockValue = Convert.ToInt32(e.Value);

                    if (stockValue >= 20)
                    {
                        e.CellStyle.BackColor = Color.FromArgb(129, 250, 123);
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else if (stockValue <= 19)
                    {
                        e.CellStyle.BackColor = Color.Salmon;
                        e.CellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void btnexportarexcel_Click(object sender, EventArgs e)
        {
            if (tablaproductos.Rows.Count < 1)
            {
                MessageBox.Show("NO HE PODIDO ENCONTRAR DATOS PARA PODER EXPORTARLOS", "Informatica", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataTable dtt = new DataTable();
                foreach (DataGridViewColumn columna in tablaproductos.Columns)
                {
                    if (columna.HeaderText != "" && columna.Visible)
                        dtt.Columns.Add(columna.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in tablaproductos.Rows)
                {
                    if (row.Visible)
                        dtt.Rows.Add(new object[] {
                            row.Cells[0].Value.ToString(),
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[8].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                            row.Cells[10].Value.ToString(),
                            row.Cells[11].Value.ToString(),
                            row.Cells[12].Value.ToString(),
                            row.Cells[13].Value.ToString(),
                            row.Cells[14].Value.ToString(),
                            row.Cells[15].Value.ToString(),
                            row.Cells[16].Value.ToString(),
                            row.Cells[18].Value.ToString(),
                            row.Cells[19].Value.ToString()
                        });
                }
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("ReporteProducto_{0}.xlsx", DateTime.Now.ToString("dd-MM-yyyy"));
                savefile.Filter = "Excel Files | *.xlsx";
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dtt, "Informe de productos en stock");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);
                        MessageBox.Show("REPORTE GENERADO EXITOSAMENTE", "Informatica", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Alert("Reporte Guardado", Form_Alert.enmType.Success);
                    }
                    catch
                    {
                        MessageBox.Show("Error al generar reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.Alert("Error al generar reporte", Form_Alert.enmType.Error);
                    }
                }
            }
        }

        public DataGridViewRow ProductoSeleccionado { get; private set; }
        private void tablaproductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ProductoSeleccionado = tablaproductos.Rows[e.RowIndex];
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnbuscarlista_Click(object sender, EventArgs e)
        {
            String columnaFiltro = ((OpcionesComboBox)listabuscar.SelectedItem).Valor.ToString();
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
            txtbuscar.Select();
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
    }
}
