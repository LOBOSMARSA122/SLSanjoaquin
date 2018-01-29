using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;

namespace Sigesoft.Node.WinClient.UI.UserControls
{
    public partial class ucRadiografiaOIT : UserControl
    {
        bool _isChangueValueControl = false;

        ServiceComponentFieldValuesList _radiografiaOIT = null;
        List<ServiceComponentFieldValuesList> _listRadiografiaOIT = new List<ServiceComponentFieldValuesList>();


        public ucRadiografiaOIT()
        {
            InitializeComponent();
        }

        #region "------------- Public Events -------------"

        /// <summary>
        /// Se desencadena cada vez que se cambia un valor del examen de Audiometria.
        /// </summary>
        public event EventHandler<AudiometriaAfterValueChangeEventArgs> AfterValueChange;
        protected void OnAfterValueChange(AudiometriaAfterValueChangeEventArgs e)
        {
            if (AfterValueChange != null)
                AfterValueChange(this, e);
        }

        #endregion

        #region "--------------- Properties --------------------"

        public List<ServiceComponentFieldValuesList> DataSource
        {
            get
            {
                return _listRadiografiaOIT;
            }
            set
            {
                if (value != _listRadiografiaOIT)
                {
                    ClearValueControl();
                    _listRadiografiaOIT = value;
                    //SearchControlAndFill(value);
                }
            }
        }

        public void ClearValueControl()
        {
            _isChangueValueControl = false;

        }

        public bool IsChangeValueControl { get { return _isChangueValueControl; } }

        #endregion

        public int CaliH = 145, PareH = 169, PleuH = 305, HallaH = 112, Comen = 77;

        private void ucRadiografiaOIT_Load(object sender, EventArgs e)
        {
            /* Tamaños completos de GroupBoxes
             * gboxCalidad 815x145 - 815x60
             * gboxParenquima 815x169 - 815x60
             * gboxPleural 815x305 - 815x60
             * gboxHallazgo 815x112 - 815x60
             * gboxComentarios 815x77 - 815x77
             */

            groupBoxCalidad.Width = 815; groupBoxCalidad.Height = 60;
            groupBoxCalidad.Location = new Point(8, 87);

            groupBoxParenquima.Width = 815; groupBoxParenquima.Height = 60;
            //groupBoxParenquima.Location = new Point(8, 238);
            groupBoxParenquima.Location = new Point(8, 87 + 6 + 60);

            groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
            //groupBoxPleural.Location = new Point(8, 413);
            groupBoxPleural.Location = new Point(8, 87 + 6 + 60 + 6 + 60);

            groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
            //groupBoxHallazgos.Location = new Point(8, 724);
            groupBoxHallazgos.Location = new Point(8, 87 + 6 + 60 + 6 + 60 + 6 + 60);

            groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
            //groupBoxComentarios.Location = new Point(8, 842);
            groupBoxComentarios.Location = new Point(8, 87 + 6 + 60 + 6 + 60 + 6 + 60 + 6 + 60);
        }

        private void cb1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb1.SelectedIndex < 1)
            {
                CaliH = 60;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = 60;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + 60);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + 60 + 6 + 60);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + 60 + 6 + 60 + 6 + 60);
            }
            else
            {
                CaliH = 145;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = 60;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + 60);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + 60 + 6 + 60);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + 60 + 6 + 60 + 6 + 60);
            }
        }

        private void cb2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb2.SelectedIndex > 0)
            {
                PareH = 60;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + 60);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + 60 + 6 + 60);
            }
            else
            {
                PareH = 169;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + 60);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + 60 + 6 + 60);
            }

        }

        private void cb3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb3.SelectedIndex > 0)
            {
                PleuH = 60;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = PleuH;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH + 6 + 60);
            }
            else
            {
                PleuH = 305;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = PleuH;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH + 6 + 60);
            }
        }

        private void cb4_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb4.SelectedIndex > 0)
            {
                HallaH = 60;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = PleuH;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = HallaH;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH + 6 + HallaH);
            }
            else
            {
                HallaH = 112;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = PleuH;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = HallaH;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH + 6 + HallaH);
            }
        }
    }
}
