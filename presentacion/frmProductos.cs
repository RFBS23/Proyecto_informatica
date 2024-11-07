using CustomAlertBoxDemo;
using DocumentFormat.OpenXml.Spreadsheet;
using Entidad;
using Negocio;
using presentacion.Modales;
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
    public partial class frmProductos : Form
    {
        public frmProductos()
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
            DateTime fechaActual = DateTime.Now;
            lblfecha.Text = $"{fechaActual.Year}-{fechaActual.Month}-{fechaActual.Day}";

            txtstock.Text = "0";
            txtdescuento.Text = "0";
            txtprecioventa.Text = "0.00";

            List<Categoria> listacat = new NCategorias().ListarCategorias();
            cbcategoria.Items.Add(new OpcionesComboBox() { Valor = 0, Texto = "Elija una categoria" });
            foreach (Categoria item in listacat)
            {
                cbcategoria.Items.Add(new OpcionesComboBox() { Valor = item.idcategoria, Texto = item.nombrecategoria });
            }
            cbcategoria.DisplayMember = "Texto";
            cbcategoria.ValueMember = "Valor";
            cbcategoria.SelectedIndex = 0;

            cbsubcategoria.Items.Clear();
            cbsubcategoria.Items.Add(new OpcionesComboBox() { Valor = 0, Texto = "Elija una subcategoria" });
            cbsubcategoria.DisplayMember = "Texto";
            cbsubcategoria.ValueMember = "Valor";
            cbsubcategoria.SelectedIndex = 0;

            List<Productos> listaProductos = new NProductos().Listar();
            foreach (Productos item in listaProductos)
            {
                tablaproductos.Rows.Add(new object[] { "", item.idproducto, item.codigo, item.nombre, item.descripcion, item.oCategoria.idcategoria, item.oCategoria.nombrecategoria, item.oSubcategoria.idsubcategorias, item.oSubcategoria.nombresubcategoria, item.stock, item.modelo, item.marca, item.precioventa, item.descuento, item.garantia, item.eficiencia_energetica, item.ubicacion, item.estado == true ? 1 : 0, item.estado == true ? "Activo" : "Inactivo", item.fecharegistro });
            }

        }

        private void CargarSubcategorias(int idCategoria)
        {
            cbsubcategoria.Items.Clear();
            cbsubcategoria.Items.Add(new OpcionesComboBox() { Valor = 0, Texto = "Elija una subcategoria" });

            if (idCategoria == 0) return;
            List<Subcategoria> listaSubcategorias = new NSubcategoria().FiltrosSubCategorias(idCategoria);

            foreach (Subcategoria item in listaSubcategorias)
            {
                cbsubcategoria.Items.Add(new OpcionesComboBox() { Valor = item.idsubcategorias, Texto = item.nombresubcategoria });
            }

            cbsubcategoria.DisplayMember = "Texto";
            cbsubcategoria.ValueMember = "Valor";
            cbsubcategoria.SelectedIndex = 0;
        }

        private void btnbuscarlista_Click(object sender, EventArgs e)
        {
            string filtro = txtcodigo.Text.Trim().ToUpper();

            if (tablaproductos.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in tablaproductos.Rows)
                {
                    if (row.Cells["codigo"].Value != null && row.Cells["codigo"].Value.ToString().Trim().ToUpper().Contains(filtro))
                    {
                        txtindice.Text = row.Index.ToString();
                        txtid.Text = row.Cells["idproducto"].Value.ToString();
                        txtcodigo.Text = row.Cells["codigo"].Value.ToString();
                        txtnombreproducto.Text = row.Cells["nombre"].Value.ToString();
                        txtdescripcion.Text = row.Cells["descripcion"].Value.ToString();

                        foreach (OpcionesComboBox ocb in cbcategoria.Items)
                        {
                            if (Convert.ToInt32(ocb.Valor) == Convert.ToInt32(row.Cells["idcategoria"].Value))
                            {
                                cbcategoria.SelectedIndex = cbcategoria.Items.IndexOf(ocb);
                                break;
                            }
                        }

                        foreach (OpcionesComboBox otb in cbsubcategoria.Items)
                        {
                            if (Convert.ToInt32(otb.Valor) == Convert.ToInt32(row.Cells["idsubcategorias"].Value))
                            {
                                cbsubcategoria.SelectedIndex = cbsubcategoria.Items.IndexOf(otb);
                                break;
                            }
                        }

                        txtstock.Text = row.Cells["stock"].Value.ToString();
                        txtmodelo.Text = row.Cells["modelo"].Value.ToString();
                        txtmarca.Text = row.Cells["marca"].Value.ToString();
                        txtprecioventa.Text = row.Cells["precioventa"].Value.ToString();
                        txtdescuento.Text = row.Cells["descuento"].Value.ToString();
                        txtgarantia.Text = row.Cells["garantia"].Value.ToString();
                        txteficiencia.Text = row.Cells["eficiencia_energetica"].Value.ToString();
                        txtubicacion.Text = row.Cells["ubicacion"].Value.ToString();

                        btnAgregar.Text = "    Editar";
                        break;
                    }
                }
            }
        }

        private void cbcategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbcategoria.SelectedIndex > 0)
            {
                int idCategoria = Convert.ToInt32(((OpcionesComboBox)cbcategoria.SelectedItem).Valor);
                CargarSubcategorias(idCategoria);
            }
        }

        private void Limpiar()
        {
            txtcodigo.Clear();
            txtnombreproducto.Clear();
            txtdescripcion.Clear();
            cbcategoria.SelectedIndex = 0;
            cbsubcategoria.SelectedIndex = 0;
            txtstock.Clear();
            txtmodelo.Clear();
            txtmarca.Clear();
            txtprecioventa.Clear();
            txtdescuento.Clear();
            txtgarantia.Clear();
            txteficiencia.Clear();
            txtubicacion.Clear();

            txtcodigo.Select();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precioventa = 0;
            string mensaje = string.Empty;

            Productos objproductos = new Productos()
            {
                idproducto = Convert.ToInt32(txtid.Text),
                codigo = txtcodigo.Text,
                nombre = txtnombreproducto.Text,
                descripcion = txtdescripcion.Text,
                oCategoria = new Categoria() { idcategoria = Convert.ToInt32(((OpcionesComboBox)cbcategoria.SelectedItem).Valor) },
                oSubcategoria = new Subcategoria() { idsubcategorias = Convert.ToInt32(((OpcionesComboBox)cbsubcategoria.SelectedItem).Valor) },
                stock = Convert.ToInt32(txtstock.Text),
                modelo = txtmodelo.Text,
                marca = txtmarca.Text,
                precioventa = Convert.ToDecimal(txtprecioventa.Text),
                descuento = Convert.ToInt32(txtdescuento.Text),
                garantia = txtgarantia.Text,
                eficiencia_energetica = txteficiencia.Text,
                ubicacion = txtubicacion.Text,
                fecharegistro = lblfecha.Text
            };

            if (btnAgregar.Text == "    Agregar")
            {
                int idproductogenerado = new NProductos().Registrar(objproductos, out mensaje);

                if (idproductogenerado != 0)
                {
                    tablaproductos.Rows.Add(new object[] {
                        "", idproductogenerado, txtcodigo.Text, txtnombreproducto.Text, txtdescripcion.Text,
                        ((OpcionesComboBox)cbcategoria.SelectedItem).Valor.ToString(),
                        ((OpcionesComboBox)cbcategoria.SelectedItem).Texto.ToString(),
                        ((OpcionesComboBox)cbsubcategoria.SelectedItem).Valor.ToString(),
                        ((OpcionesComboBox)cbsubcategoria.SelectedItem).Texto.ToString(),
                        txtstock.Text,
                        txtmodelo.Text,
                        txtmarca.Text,
                        Convert.ToDecimal(txtprecioventa.Text).ToString("0.00"),
                        txtdescuento.Text,                        
                        txtgarantia.Text,
                        txteficiencia.Text,
                        txtubicacion.Text,
                        lblfecha.Text,
                    });

                    Limpiar();
                    this.Alert("Producto Registrado", Form_Alert.enmType.Success);
                }
                else
                {
                    MessageBox.Show(mensaje);
                    this.Alert("No se pudo registrar", Form_Alert.enmType.Error);
                }
            }
            else if (btnAgregar.Text == "    Editar")
            {
                bool resultado = new NProductos().Editar(objproductos, out mensaje);
                if (resultado)
                {
                    DataGridViewRow row = tablaproductos.Rows[Convert.ToInt32(txtindice.Text)];
                    row.Cells["idproducto"].Value = txtid.Text;
                    row.Cells["codigo"].Value = txtcodigo.Text;
                    row.Cells["nombre"].Value = txtnombreproducto.Text;
                    row.Cells["descripcion"].Value = txtdescripcion.Text;
                    row.Cells["idcategoria"].Value = ((OpcionesComboBox)cbcategoria.SelectedItem).Valor.ToString();
                    row.Cells["nombrecategoria"].Value = ((OpcionesComboBox)cbcategoria.SelectedItem).Texto.ToString();
                    row.Cells["idsubcategorias"].Value = ((OpcionesComboBox)cbsubcategoria.SelectedItem).Valor.ToString();
                    row.Cells["nombresubcategoria"].Value = ((OpcionesComboBox)cbsubcategoria.SelectedItem).Texto.ToString();
                    row.Cells["stock"].Value = txtstock.Text;
                    row.Cells["modelo"].Value = txtmodelo.Text;
                    row.Cells["marca"].Value = txtmarca.Text;
                    row.Cells["precioventa"].Value = Convert.ToDecimal(txtprecioventa.Text).ToString("0.00");
                    row.Cells["descuento"].Value = txtdescuento.Text;
                    row.Cells["garantia"].Value = txtgarantia.Text;
                    row.Cells["eficiencia_energetica"].Value = txteficiencia.Text;
                    row.Cells["ubicacion"].Value = txtubicacion.Text;

                    Limpiar();
                    this.Alert("Producto Editado", Form_Alert.enmType.Success);
                }
                else
                {
                    MessageBox.Show(mensaje);
                    this.Alert("No se pudo editar", Form_Alert.enmType.Error);
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
                if (MessageBox.Show("¿ESTA SEGURO DE ELIMINAR A ESTE PRODUCTO?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Productos objproductos = new Productos()
                    {
                        idproducto = Convert.ToInt32(txtid.Text)
                    };
                    bool respuesta = new NProductos().Eliminar(objproductos, out mensaje);
                    if (respuesta)
                    {
                        frmProductos formProductos = (frmProductos)Application.OpenForms["frmProductos"];
                        if (formProductos != null)
                        {
                            formProductos.tablaproductos.Rows.RemoveAt(Convert.ToInt32(txtindice.Text));
                        }
                        else
                        {
                            MessageBox.Show("No se pudo acceder a la tabla de productos.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                Limpiar();
            }
        }

        private void txtstock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 45) || (e.KeyChar == 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Ingresa Solo Numeros", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void txtprecioventa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 45) || (e.KeyChar == 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Ingresa Solo Numeros", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void txtdescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 45) || (e.KeyChar == 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Ingresa Solo Numeros", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
    }
}
