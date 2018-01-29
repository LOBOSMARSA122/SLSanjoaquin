using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class FormPrecioComponente : Form
    {
        public float Precio { get; set; }
        public FormPrecioComponente(string pstrNombreComponente)
        {          
            InitializeComponent();
            lblNombreComponente.Text = pstrNombreComponente;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Precio = float.Parse(txtPrecio.Text.ToString());
            this.DialogResult = DialogResult.OK;
        }
    }
}
