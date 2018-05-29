using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmDescargarAdjuntos : Form
    {
        private string Fecha;
        private string Dni;
        public frmDescargarAdjuntos(string fecha, string dni)
        {
            InitializeComponent();
            Fecha = fecha;
            Dni = dni;
        }

        private void frmDescargarAdjuntos_Load(object sender, EventArgs e)
        {
            DirectoryInfo rutaOrigen = null;
            string rutaDestino = null;
            string ArchivosConcatenado = "";
           
                var  rutaOrigenRX = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgRxOrigen").ToString());

                FileInfo[] files = rutaOrigenRX.GetFiles();
                //int Contador = 1;
                foreach (FileInfo file in files)
                {
                   
                    if (file.ToString().Count() > 16)
                    {
                        if (file.ToString().Substring(0, 17) == Dni + "-" + Fecha)
                        {
                            //file.CopyTo(rutaDestino + "-" + Contador + ".jpg");
                            //Contador++;
                            ArchivosConcatenado += file.ToString() + "; ";
                        };
                    }
                }
           
                rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgEKGOrigen").ToString());
              
                files = rutaOrigen.GetFiles();
                //int Contador = 1;
                foreach (FileInfo file in files)
                {
                   
                    if (file.ToString().Count() > 16)
                    {
                        if (file.ToString().Substring(0, 17) == Dni + "-" + Fecha)
                        {
                            //file.CopyTo(rutaDestino + "-" + Contador + ".pdf");
                            //Contador++;
                            ArchivosConcatenado += file.ToString() + "; ";
                        };
                    }
                }
         
                rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgESPIROOrigen").ToString());               
                 files = rutaOrigen.GetFiles();
                //int Contador = 1;
                foreach (FileInfo file in files)
                {
                    if (file.ToString().Count() > 16)
                    {

                        if (file.ToString().Substring(0, 17) == Dni + "-" + Fecha)
                        {
                            //file.CopyTo(rutaDestino + "-" + Contador + ".pdf");
                            //Contador++;
                            ArchivosConcatenado += file.ToString() + "; ";
                        };
                    }
                }
          
                rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgLABOrigen").ToString());

                files = rutaOrigen.GetFiles();
                //int Contador = 1;
                foreach (FileInfo file in files)
                {
                  
                    if (file.ToString().Count() > 16)
                    {
                        if (file.ToString().Substring(0, 17) == Dni + "-" + Fecha)
                        {
                            ArchivosConcatenado += file.ToString() + "; ";
                            //file.CopyTo(rutaDestino + "-" + Contador + ".pdf");
                            //Contador++;
                        };
                    }
                }

                txtArchivosAdjuntos.Text = ArchivosConcatenado;
           
        }

        private void btnDescargarAdjuntos_Click(object sender, EventArgs e)
        {
            DirectoryInfo rutaOrigen = null;
            string mdoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.Directory.CreateDirectory(mdoc + @"\" + Dni);
            string rutaDestino = mdoc + @"\" + Dni + @"\";
            var rutaOrigenRX = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgRxOrigen").ToString());

            FileInfo[] files = rutaOrigenRX.GetFiles();
            int ContadorRx = 1;
            foreach (FileInfo file in files)
            {

                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == Dni + "-" + Fecha)
                    {
                       var  ext = Path.GetExtension(file.ToString());
                       file.CopyTo(rutaDestino + file.Name.Substring(0, 17) + "-" + ContadorRx + ext);
                        ContadorRx++;                       
                    };
                }
            }

            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgEKGOrigen").ToString());

            files = rutaOrigen.GetFiles();
            int ContadorEKG = 1;
            foreach (FileInfo file in files)
            {

                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == Dni + "-" + Fecha)
                    {
                        var ext = Path.GetExtension(file.ToString());
                        file.CopyTo(rutaDestino + file.Name.Substring(0, 17) + "-" + ContadorEKG + ext);
                        ContadorEKG++;                       
                    };
                }
            }

            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgESPIROOrigen").ToString());
            files = rutaOrigen.GetFiles();
            int ContadorESPIRO = 1;
            foreach (FileInfo file in files)
            {
                if (file.ToString().Count() > 16)
                {

                    if (file.ToString().Substring(0, 17) == Dni + "-" + Fecha)
                    {
                        var ext = Path.GetExtension(file.ToString());
                        file.CopyTo(rutaDestino + file.Name.Substring(0, 17) + "-" + ContadorESPIRO + ext);
                        ContadorESPIRO++;
                    };
                }
            }

            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgLABOrigen").ToString());

            files = rutaOrigen.GetFiles();
            int ContadorLAB = 1;
            foreach (FileInfo file in files)
            {

                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == Dni + "-" + Fecha)
                    {
                        var ext = Path.GetExtension(file.ToString());
                        file.CopyTo(rutaDestino + file.Name.Substring(0, 17) + "-" + ContadorLAB + ext);
                        ContadorLAB++;
                    };
                }
            }

            MessageBox.Show("Los archivos se grabaron en  Mis Documentos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

    }
}
