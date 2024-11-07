using Entidad;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            txtusuario.Select();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btningresar_Click(object sender, EventArgs e)
        {
            Entidad.Usuarios usuariologin = new NUsuarios().IniciarSesion().Where(u => u.nombreusuario == txtusuario.Text && u.clave == txtpassword.Text).FirstOrDefault();
            if (usuariologin != null)
            {
                if (usuariologin.oNivelAcceso != null)
                {
                    Dashboard dashboardAdmin = new Dashboard(usuariologin);
                    dashboardAdmin.Show();
                }
                else
                {
                    MessageBox.Show("Error en los datos de acceso.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Error al Iniciar Sesión", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtpassword_KeyPress(object sender, KeyPressEventArgs e)
        { 
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
