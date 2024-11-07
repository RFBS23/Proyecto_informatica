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
    public partial class frmInformacion : Form
    {
        public frmInformacion()
        {
            InitializeComponent();
        }

        private void frmInformacion_Load(object sender, EventArgs e)
        {
            Label lblFooter = new Label();
            lblFooter.Text = "© 2024 FabriDev. Todos los derechos reservados.";
            lblFooter.Dock = DockStyle.Bottom;
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            lblFooter.AutoSize = false;
            lblFooter.Height = 30;
            lblFooter.BackColor = Color.LightGray;
            this.Controls.Add(lblFooter);

            LinkLabel linkPortfolio = new LinkLabel();
            linkPortfolio.Text = "Visita mi portafolio";
            linkPortfolio.AutoSize = true;
            linkPortfolio.Location = new Point(10, 10);
            linkPortfolio.LinkClicked += new LinkLabelLinkClickedEventHandler(linkPortfolio_LinkClicked);

            LinkLabel linkWebsite = new LinkLabel();
            linkWebsite.Text = "Visita mi sitio web";
            linkWebsite.AutoSize = true;
            linkWebsite.Location = new Point(10, 40);
            linkWebsite.LinkClicked += new LinkLabelLinkClickedEventHandler(linkWebsite_LinkClicked);

            this.Controls.Add(linkPortfolio);
            this.Controls.Add(linkWebsite);
        }

        private void linkPortfolio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://portafolio-fabridev.vercel.app/",
                UseShellExecute = true
            });
        }

        private void linkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://portafolio-fabridev.vercel.app/",
                UseShellExecute = true
            });
        }
    }
}
