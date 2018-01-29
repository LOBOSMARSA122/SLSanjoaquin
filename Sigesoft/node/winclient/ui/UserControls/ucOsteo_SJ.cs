using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;
using System.IO;

namespace Sigesoft.Node.WinClient.UI.UserControls
{
    public partial class ucOsteo_SJ : UserControl
    {

        bool _isChangueValueControl = false;
        List<ServiceComponentFieldValuesList> _listOfAtencionAdulto1 = new List<ServiceComponentFieldValuesList>();
        ServiceComponentFieldValuesList _UserControlValores = null;

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
        public string PersonId { get; set; }
        public string ServiceId { get; set; }

        public List<ServiceComponentFieldValuesList> DataSource
        {
            get
            {
                
//                SaveValueControlForInterfacingESO("N009-OSJ00000004","1");
//                SaveValueControlForInterfacingESO("N009-OSJ00000005","1");
//                SaveValueControlForInterfacingESO("N009-OSJ00000006","1");

//                SaveValueControlForInterfacingESO( "N009-OSJ00000007","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000008","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000009","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000010","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000011","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000012","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000013","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000014","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000024","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000025","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000026","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000027","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000028","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000029","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000030","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000031","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000032","1");
//               SaveValueControlForInterfacingESO( "N009-OSJ00000033","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000034","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000035","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000036","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000037","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000038","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000039","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000040","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000041","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000042","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000043","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000044","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000045","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000049","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000050","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000051","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000052","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000053","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000054","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000055","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000056","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000057","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000058","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000059","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000060","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000061","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000062","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000063","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000064","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000065","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000066","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000067","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000068","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000069","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000070","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000071","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000072","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000073","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000074","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000075","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000076","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000077","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000078","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000079","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000080","1");
//               SaveValueControlForInterfacingESO( "N009-OSJ00000081","1");
//               SaveValueControlForInterfacingESO( "N009-OSJ00000082","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000083","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000084","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000085","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000086","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000087","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000088","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000089","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000090","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000091","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000092","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000093","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000094","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000095","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000096","1");

//                 SaveValueControlForInterfacingESO( "N009-OSJ00000103","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000104","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000105","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000106","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000107","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000114","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000115","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000116","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000117","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000118","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000119","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000120","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000121","1");

//                SaveValueControlForInterfacingESO( "N009-OSJ00000124","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000125","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000126","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000127","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000128","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000129","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000130","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000131","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000132","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000133","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000134","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000135","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000136","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000137","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000138","1");
//               SaveValueControlForInterfacingESO( "N009-OSJ00000139","1");
//                 SaveValueControlForInterfacingESO( "N009-OSJ00000140","1");
//               SaveValueControlForInterfacingESO( "N009-OSJ00000141","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000142","1");
//               SaveValueControlForInterfacingESO( "N009-OSJ00000143","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000144","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000145","1");
//                SaveValueControlForInterfacingESO( "N009-OSJ00000146","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000147","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000148","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000149","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000150","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000151","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000152","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000153","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000154","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000155","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000156","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000157","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000158","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000159","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000160","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000161","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000162","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000163","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000164","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000165","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000166","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000167","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000168","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000169","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000170","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000171","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000172","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000173","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000174","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000175","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000176","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000177","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000178","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000179","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000180","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000181","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000182","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000183","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000184","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000185","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000186","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000187","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000188","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000189","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000190","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000191","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000192","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000193","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000194","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000195","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000196","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000197","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000198","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000199","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000200","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000201","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000202","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000203","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000204","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000205","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000206","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000207","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000208","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000209","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000210","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000211","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000212","1");
// SaveValueControlForInterfacingESO( "N009-OSJ00000212","1");
//SaveValueControlForInterfacingESO( "N009-OSJ00000212","1");
                return _listOfAtencionAdulto1;
            }
            set
            {
                if (value != _listOfAtencionAdulto1)
                {
                    ClearValueControl();
                    _listOfAtencionAdulto1 = value;
                    SearchControlAndFill(value);
                }
            }
        }

        public void ClearValueControl()
        {
            _isChangueValueControl = false;
        }

        public bool IsChangeValueControl { get { return _isChangueValueControl; } }
        #endregion

        public ucOsteo_SJ()
        {
            InitializeComponent();
        }

        private void SearchControlAndFill(List<ServiceComponentFieldValuesList> DataSource)
        {
            if (DataSource == null || DataSource.Count == 0) return;
            // Ordenar Lista Datasource
            var DataSourceOrdenado = DataSource.OrderBy(p => p.v_ComponentFieldId).ToList();

            // recorrer la lista que viene de la BD
            foreach (var item in DataSourceOrdenado)
            {
                var matchedFields = this.Controls.Find(item.v_ComponentFieldId, true);

                if (matchedFields.Length > 0)
                {
                    var field = matchedFields[0];

                    if (field is TextBox)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            ((TextBox)field).Text = item.v_Value1;
                        }
                    }

                    else if (field is ComboBox)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            ((ComboBox)field).SelectedValue = item.v_Value1;
                        }
                    }
                }
            }
        }
             
        private void ucOsteo_SJ_Load(object sender, EventArgs e)
        {
             txtTIpoTrabajo.Name =  "N009-OSJ00000001";
        txtHorasTotales.Name =  "N009-OSJ00000002";
        txtHorasDia.Name =  "N009-OSJ00000003";
        ddlFrecuencia.Name =  "N009-OSJ00000004";
        ddlCiclo.Name =  "N009-OSJ00000005";
        ddlMovimientoDeAlcane.Name =  "N009-OSJ00000006";
        ddlElcuelloSeMantiene.Name =  "N009-OSJ00000007";
        ddlGirosDeColumna.Name =  "N009-OSJ00000008";
        ddlMovimientosDeFlexion.Name =  "N009-OSJ00000009";
        ddlCompresionDeNervio.Name =  "N009-OSJ00000010";
        ddlDesviacionesCubitales.Name =  "N009-OSJ00000011";
        ddlRotacionExternaCodo.Name =  "N009-OSJ00000012";
        ddlFlexionExternaCodo.Name =  "N009-OSJ00000013";
        ddlOtros.Name =  "N009-OSJ00000014";
        txtOtrosDescripcion.Name =  "N009-OSJ00000015";
        txtLevanta.Name =  "N009-OSJ00000016";
        txtColoca.Name =  "N009-OSJ00000017";
        txtEmpuja.Name =  "N009-OSJ00000018";
        txtTracciona.Name =  "N009-OSJ00000019";
        txtQueTiempoTiene.Name =  "N009-OSJ00000020";
        txtCuantasHorasDia.Name =  "N009-OSJ00000021";
        txtDesde.Name =  "N009-OSJ00000022";
        txtHasta.Name =  "N009-OSJ00000023";
        ddlEquilibrioInestable.Name =  "N009-OSJ00000024";
        ddlManipulacionDistancia.Name =  "N009-OSJ00000025";
        cboExigeTorsion.Name =  "N009-OSJ00000026";
        cboExistePosiBrusco.Name =  "N009-OSJ00000027";
        ddlCuerpoPosicion.Name =  "N009-OSJ00000028";
        ddlPosturaForzada.Name =  "N009-OSJ00000029";
        ddlDesnivelesSuelo.Name =  "N009-OSJ00000030";
        ddlTemperaturaHumedad.Name =  "N009-OSJ00000031";
        ddlVibraciones.Name =  "N009-OSJ00000032";
        ddlEsfuerzosFisicos.Name =  "N009-OSJ00000033";
        ddlDesnivelesSuelo02.Name =  "N009-OSJ00000034";
        ddlTemperaturaHumedad02.Name =  "N009-OSJ00000035";
        ddlVibraciones02.Name =  "N009-OSJ00000036";
        ddlConoceCuales.Name =  "N009-OSJ00000037";
        ddlLasUsa.Name =  "N009-OSJ00000038";
        ddlHaSidoCapacitado.Name =  "N009-OSJ00000039";
        ddlNuca12Meses.Name =  "N009-OSJ00000040";
        ddlNucaUltMeses.Name =  "N009-OSJ00000041";
        ddlNuca7dias.Name =  "N009-OSJ00000042";
        ddlHombroDer12Meses.Name =  "N009-OSJ00000043";
        ddlHombroDerUltMeses.Name =  "N009-OSJ00000044";
        ddlHombroDer7dias.Name =  "N009-OSJ00000045";
        ddlHombroIzq12Meses.Name =  "N009-OSJ00000049";
        ddlHombroIzqUltMeses.Name =  "N009-OSJ00000050";
        ddlHombroIzq7dias.Name =  "N009-OSJ00000051";
        ddlAmbosHombros12Meses.Name =  "N009-OSJ00000052";
        ddlAmbosHombrosUltMeses.Name =  "N009-OSJ00000053";
        ddlAmbosHombros7dias.Name =  "N009-OSJ00000054";
        ddlCodoDer12Meses.Name =  "N009-OSJ00000055";
        ddlCodoDerUltMeses.Name =  "N009-OSJ00000056";
        ddlCodoDer7dias.Name =  "N009-OSJ00000057";
        ddlCodoIzq12Meses.Name =  "N009-OSJ00000058";
        ddlCodoIzqUltMeses.Name =  "N009-OSJ00000059";
        ddlCodoIzq7dias.Name =  "N009-OSJ00000060";
        ddlAmbosCodos12Meses.Name =  "N009-OSJ00000061";
        ddlAmbosCodosUltMeses.Name =  "N009-OSJ00000062";
        ddlAmbosCodos7dias.Name =  "N009-OSJ00000063";
        ddlManoDer12Meses.Name =  "N009-OSJ00000064";
        ddlManoDerUltMeses.Name =  "N009-OSJ00000065";
        ddlManoDer7dias.Name =  "N009-OSJ00000066";
        ddlManoIzq12Meses.Name =  "N009-OSJ00000067";
        ddlManoIzqUltMeses.Name =  "N009-OSJ00000068";
        ddlManoIzq7dias.Name =  "N009-OSJ00000069";
        ddlAmbasManos12Meses.Name =  "N009-OSJ00000070";
        ddlAmbasManosUltMeses.Name =  "N009-OSJ00000071";
        ddlAmbasManos7dias.Name =  "N009-OSJ00000072";
        ddlColumnaDorsal12Meses.Name =  "N009-OSJ00000073";
        ddlClomunaDorsalUltMeses.Name =  "N009-OSJ00000074";
        ddlColumnaDorsal7dias.Name =  "N009-OSJ00000075";
        ddlColumnaLumbar12Meses.Name =  "N009-OSJ00000076";
        ddlClñoumnaLumbarUltMeses.Name =  "N009-OSJ00000077";
        ddlColumnaLumbar7dias.Name =  "N009-OSJ00000078";
        ddlCaderaDer12Meses.Name =  "N009-OSJ00000079";
        ddlCaderaDerUltMeses.Name =  "N009-OSJ00000080";
        ddlCaderaDer7dias.Name =  "N009-OSJ00000081";
        ddlCaderaIzq12Meses.Name =  "N009-OSJ00000082";
        ddlCaderaIzqUltMeses.Name =  "N009-OSJ00000083";
        ddlCaderaIzq7dias.Name =  "N009-OSJ00000084";
        ddlRodillaDer12Meses.Name =  "N009-OSJ00000085";
        ddlRodillaDerUltMeses.Name =  "N009-OSJ00000086";
        ddlRodillaDer7dias.Name =  "N009-OSJ00000087";
        ddlRodillaIzq12Meses.Name =  "N009-OSJ00000088";
        ddlRodillaIzqUltMeses.Name =  "N009-OSJ00000089";
        ddlRodillaIzq7dias.Name =  "N009-OSJ00000090";
        ddlTobilloDer12Meses.Name =  "N009-OSJ00000091";
        dllTobilloDerUltMeses.Name =  "N009-OSJ00000092";
        ddlTobilloDer7dias.Name =  "N009-OSJ00000093";
        ddlTobilloIzq12Meses.Name =  "N009-OSJ00000094";
        ddlTobilloIzqUltMeses.Name =  "N009-OSJ00000095";
        ddlTobilloIzq7dias.Name =  "N009-OSJ00000096";
        txtTieneAntecedente.Name =  "N009-OSJ00000097";
        txtAPadecido.Name =  "N009-OSJ00000098";
        txtDeLasEnfermedades.Name =  "N009-OSJ00000099";
        txtQueDeportes.Name =  "N009-OSJ00000100";
        txtHobbyOAlguna.Name =  "N009-OSJ00000101";
        txtAnamnesis.Name =  "N009-OSJ00000102";
        txtSchoberReposo.Name = "N009-OSJ00000122";
        txtSchoberExtension.Name = "N009-OSJ00000123";
        txtCervicalApofisis.Name =  "N009-OSJ00000108";
        txtCervicalContract.Name =  "N009-OSJ00000109";
        txtDorsalApofisis.Name =  "N009-OSJ00000110";
        txtDorsalContract.Name =  "N009-OSJ00000111";
        txtLumbarApofisis.Name =  "N009-OSJ00000112";
        txtLumbarContract.Name =  "N009-OSJ00000113";

        ddlCervicalAP.Name = "N009-OSJ00000103";
        ddlDorsalAP.Name = "N009-OSJ00000104";
        ddlLumbarAP.Name = "N009-OSJ00000105";
        ddlDOrsalLAT.Name = "N009-OSJ00000106";
        ddlLumbarLAT.Name = "N009-OSJ00000107";
        ddlLasegeDer.Name =  "N009-OSJ00000114";
        ddlLasegueIzq.Name =  "N009-OSJ00000115";
        ddlBragardDer.Name =  "N009-OSJ00000116";
        ddlBragardIzq.Name =  "N009-OSJ00000117";
        ddlPhalenDer.Name =  "N009-OSJ00000118";
        ddlPhalenIzq.Name =  "N009-OSJ00000119";
        ddlTinelDer.Name =  "N009-OSJ00000120";
        ddlTinelIzq.Name =  "N009-OSJ00000121";
      
        ddlRodillaDerVaro.Name =  "N009-OSJ00000124";
        ddlRodillaIzqVaro.Name =  "N009-OSJ00000125";
        ddlPieDerVaro.Name =  "N009-OSJ00000126";
        ddlPieIzqVaro.Name =  "N009-OSJ00000127";
        ddlCodoDerVaro.Name =  "N009-OSJ00000128";
        ddlCOdoIzqVaro.Name =  "N009-OSJ00000129";
        ddlHombroDerAdd.Name =  "N009-OSJ00000130";
        ddlHombroIzqAdd.Name =  "N009-OSJ00000131";
        ddlCodoDerAdd.Name =  "N009-OSJ00000132";
        ddlCodoIzqAdd.Name =  "N009-OSJ00000133";
        ddlMuñecaDerAdd.Name =  "N009-OSJ00000134";
        ddlMuñecaIzqAdd.Name =  "N009-OSJ00000135";
        ddlCaderaDerAdd.Name =  "N009-OSJ00000136";
        ddlCaderaIzqAdd.Name =  "N009-OSJ00000137";
        ddlRodillaDerAdd.Name =  "N009-OSJ00000138";
        ddlRodillaIzqAdd.Name =  "N009-OSJ00000139";
        ddlTobilloDerAdd.Name =  "N009-OSJ00000140";
        ddlTobilloIzqAdd.Name =  "N009-OSJ00000141";
        ddlHombroDerFlex.Name =  "N009-OSJ00000142";
        ddlHombroIzqFlex.Name =  "N009-OSJ00000143";
        ddlCodoDerFlex.Name =  "N009-OSJ00000144";
        ddlCodoIzqFlex.Name =  "N009-OSJ00000145";
        ddlMuñecaDerFlex.Name =  "N009-OSJ00000146";
        ddlMuñecaIzqFlex.Name =  "N009-OSJ00000147";
        ddlCaderaDerFLex.Name =  "N009-OSJ00000148";
        ddlCaderaIzqFLex.Name =  "N009-OSJ00000149";
        ddlRodillaDerFLex.Name =  "N009-OSJ00000150";
        ddlRodillaIzqFlex.Name =  "N009-OSJ00000151";
        ddlTobilloDerFlex.Name =  "N009-OSJ00000152";
        ddlTobilloIzqFlex.Name =  "N009-OSJ00000153";
        ddlHombroDerExt.Name =  "N009-OSJ00000154";
        ddlHombroIzqExt.Name =  "N009-OSJ00000155";
        ddlCodoDerExt.Name =  "N009-OSJ00000156";
        ddlCodoIzqExt.Name =  "N009-OSJ00000157";
        ddlMuñecaDerExt.Name =  "N009-OSJ00000158";
        ddlMuñecaIzqExt.Name =  "N009-OSJ00000159";
        ddlCaderaDerExt.Name =  "N009-OSJ00000160";
        ddlCaderaIzqExt.Name =  "N009-OSJ00000161";
        ddlRodillaDerExt.Name =  "N009-OSJ00000162";
        ddlRodillaIzqExt.Name =  "N009-OSJ00000163";
        ddlTobilloDerExt.Name =  "N009-OSJ00000164";
        ddlTobilloIzqExt.Name =  "N009-OSJ00000165";
        ddlHombroDerRotExt.Name =  "N009-OSJ00000166";
        ddlHombroIzqRotExt.Name =  "N009-OSJ00000167";
        ddlCodoDerRotExt.Name =  "N009-OSJ00000168";
        ddlCodoIzqRotExt.Name =  "N009-OSJ00000169";
        ddlMuñecaDerRotExt.Name =  "N009-OSJ00000170";
        ddlMuñecaIzqRotExt.Name =  "N009-OSJ00000171";
        ddlCaderaDerRotExt.Name =  "N009-OSJ00000172";
        ddlCaderaIzqRotExt.Name =  "N009-OSJ00000173";
        ddlRodillaDerRotExt.Name =  "N009-OSJ00000174";
        ddlRodillaIzqRotExt.Name =  "N009-OSJ00000175";
        ddlTobilloDerRotExt.Name =  "N009-OSJ00000176";
        ddlTobilloIzqRotExt.Name =  "N009-OSJ00000177";
        ddlHombroDerRotInt.Name =  "N009-OSJ00000178";
        ddlHombroIzqRotInt.Name =  "N009-OSJ00000179";
        ddlCodoDerRotInt.Name =  "N009-OSJ00000180";
        ddlCodoIzqRotInt.Name =  "N009-OSJ00000181";
        ddlMuñecaDerRotInt.Name =  "N009-OSJ00000182";
        ddlMuñecaIzqRotInt.Name =  "N009-OSJ00000183";
        ddlCaderaDerRotInt.Name =  "N009-OSJ00000184";
        ddlCaderaIzqRotInt.Name =  "N009-OSJ00000185";
        ddlRodillaDerRotInt.Name =  "N009-OSJ00000186";
        ddlRodillaIzqRotInt.Name =  "N009-OSJ00000187";
        ddlTobilloDerRotInt.Name =  "N009-OSJ00000188";
        ddlTobilloIzqRotInt.Name =  "N009-OSJ00000189";
        ddlHombroDerIrrad.Name =  "N009-OSJ00000190";
        ddlHombroIzqIrrad.Name =  "N009-OSJ00000191";
        ddlCodoDerIrrad.Name =  "N009-OSJ00000192";
        ddlCodoIzqIrrad.Name =  "N009-OSJ00000193";
        ddlMuñecaDerIrrad.Name =  "N009-OSJ00000194";
        ddlMuñecaIzqIrrad.Name =  "N009-OSJ00000195";
        ddlCaderaDerIrrad.Name =  "N009-OSJ00000196";
        ddlCaderaIzqIrrad.Name =  "N009-OSJ00000197";
        ddlRodillaDerIrrad.Name =  "N009-OSJ00000198";
        ddlRodillaIzqIrrad.Name =  "N009-OSJ00000199";
        ddlTobilloDerIrrad.Name =  "N009-OSJ00000200";
        ddlTobilloIzqIrrad.Name =  "N009-OSJ00000201";
        ddlHombroDerMasaMusc.Name =  "N009-OSJ00000202";
        ddlHombroIzqMasaMusc.Name =  "N009-OSJ00000203";
        ddlCodoDerMasaMusc.Name =  "N009-OSJ00000204";
        ddlCodoIzqMasaMusc.Name =  "N009-OSJ00000205";
        ddlMuñecaDerMasaMusc.Name =  "N009-OSJ00000206";
        ddlMuñecaIzqMasaMusc.Name =  "N009-OSJ00000207";
        ddlCaderaDerMasaMusc.Name =  "N009-OSJ00000208";
        ddlCaderaIzqMasaMusc.Name =  "N009-OSJ00000209";
        ddlRodillaDerMasaMusc.Name =  "N009-OSJ00000210";
        ddlRodillaIzqMasaMusc.Name =  "N009-OSJ00000211";
        ddlTobilloDerMasaMusc.Name =  "N009-OSJ00000212";
        ddlTobilloIzqMasaMusc.Name =  "N009-OSJ00000212";
       
        ddlAptitud.Name = "N009-OSJ00000212";
        txtObservacion.Name = "N009-OSJ00000212";
            #region Llenado de combos
        SystemParameterBL _objSystemParameterBL = new SystemParameterBL();
            OperationResult objOperationResult = new OperationResult();
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 247) = _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 247);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 227) = _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 227);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111) =  _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 183) = _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 183);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 229) = _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 229);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203) = _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 243)= _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 243);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248) = _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 124) = _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 124);
            //var _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 244) = _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 244);


            Utils.LoadDropDownList(ddlFrecuencia, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCiclo, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 227), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlMovimientoDeAlcane, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlElcuelloSeMantiene, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlGirosDeColumna, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMovimientosDeFlexion, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCompresionDeNervio, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlDesviacionesCubitales, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRotacionExternaCodo, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlFlexionExternaCodo, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlOtros, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlEquilibrioInestable, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlManipulacionDistancia, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cboExigeTorsion, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cboExistePosiBrusco, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCuerpoPosicion, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlPosturaForzada, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlDesnivelesSuelo, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTemperaturaHumedad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlVibraciones, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlEsfuerzosFisicos, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlDesnivelesSuelo02, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTemperaturaHumedad02, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlVibraciones02, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlConoceCuales, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlLasUsa, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHaSidoCapacitado, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlNuca12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlNucaUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlNuca7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlHombroDer12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroDerUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroDer7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzq12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzqUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzq7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbosHombros12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbosHombrosUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbosHombros7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDer12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlCodoDerUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDer7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzq12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzqUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzq7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbosCodos12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbosCodosUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbosCodos7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlManoDer12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlManoDerUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlManoDer7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlManoIzq12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlManoIzqUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlManoIzq7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbasManos12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbasManosUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAmbasManos7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlColumnaDorsal12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlClomunaDorsalUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlColumnaDorsal7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlColumnaLumbar12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlClñoumnaLumbarUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlColumnaLumbar7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDer12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDerUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDer7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzq12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzqUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzq7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaDer12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlRodillaDerUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaDer7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzq12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzq7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloDer12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(dllTobilloDerUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloDer7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzq12Meses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzqUltMeses, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlTobilloIzq7dias, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCervicalAP, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 183), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlDorsalAP, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 183), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlLumbarAP, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 183), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlDOrsalLAT, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 229), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlLumbarLAT, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 229), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlLasegeDer, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlLasegueIzq, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlBragardDer, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlBragardIzq, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlPhalenDer, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlPhalenIzq, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTinelDer, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTinelIzq, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaDerVaro, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqVaro, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlPieDerVaro, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlPieIzqVaro, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDerVaro, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCOdoIzqVaro, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlHombroDerAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzqAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDerAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzqAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaDerAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaIzqAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDerAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzqAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaDerAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlTobilloDerAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzqAdd, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroDerFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzqFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDerFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzqFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaDerFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaIzqFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDerFLex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzqFLex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlRodillaDerFLex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloDerFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzqFlex, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroDerExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzqExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDerExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzqExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaDerExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaIzqExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlCaderaDerExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzqExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaDerExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloDerExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzqExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroDerRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzqRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDerRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzqRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlMuñecaDerRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaIzqRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDerRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzqRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaDerRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloDerRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzqRotExt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroDerRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzqRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlCodoDerRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzqRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaDerRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaIzqRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDerRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzqRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaDerRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloDerRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzqRotInt, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlHombroDerIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzqIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDerIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzqIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaDerIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaIzqIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDerIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzqIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaDerIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlTobilloDerIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzqIrrad, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroDerMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlHombroIzqMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoDerMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCodoIzqMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaDerMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMuñecaIzqMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaDerMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCaderaIzqMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlRodillaDerMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlRodillaIzqMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloDerMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlTobilloIzqMasaMusc, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 248), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlAptitud, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 124), DropDownListAction.Select);
       

            #endregion
            ddlFrecuencia.SelectedValue = "1";
          
            ddlFrecuencia.SelectedValue="2";// = "N009-OSJ00000004";
            ddlCiclo.SelectedValue="2";// = "N009-OSJ00000005";
            ddlMovimientoDeAlcane.SelectedValue="0";// = "N009-OSJ00000006";
            ddlElcuelloSeMantiene.SelectedValue="0";// = "N009-OSJ00000007";
            ddlGirosDeColumna.SelectedValue="0";// = "N009-OSJ00000008";
            ddlMovimientosDeFlexion.SelectedValue="0";// = "N009-OSJ00000009";
            ddlCompresionDeNervio.SelectedValue="0";// = "N009-OSJ00000010";
            ddlDesviacionesCubitales.SelectedValue="0";// = "N009-OSJ00000011";
            ddlRotacionExternaCodo.SelectedValue="0";// = "N009-OSJ00000012";
            ddlFlexionExternaCodo.SelectedValue="0";// = "N009-OSJ00000013";
            ddlOtros.SelectedValue="0";// = "N009-OSJ00000014";
          
            ddlEquilibrioInestable.SelectedValue="0";// = "N009-OSJ00000024";
            ddlManipulacionDistancia.SelectedValue="0";// = "N009-OSJ00000025";
            cboExigeTorsion.SelectedValue="0";// = "N009-OSJ00000026";
            cboExistePosiBrusco.SelectedValue="0";// = "N009-OSJ00000027";
            ddlCuerpoPosicion.SelectedValue="0";// = "N009-OSJ00000028";
            ddlPosturaForzada.SelectedValue="0";// = "N009-OSJ00000029";
            ddlDesnivelesSuelo.SelectedValue="0";// = "N009-OSJ00000030";
            ddlTemperaturaHumedad.SelectedValue="0";// = "N009-OSJ00000031";
            ddlVibraciones.SelectedValue="0";// = "N009-OSJ00000032";
            ddlEsfuerzosFisicos.SelectedValue="0";// = "N009-OSJ00000033";
            ddlDesnivelesSuelo02.SelectedValue="0";// = "N009-OSJ00000034";
            ddlTemperaturaHumedad02.SelectedValue="0";// = "N009-OSJ00000035";
            ddlVibraciones02.SelectedValue="0";// = "N009-OSJ00000036";
            ddlConoceCuales.SelectedValue="1";// = "N009-OSJ00000037";
            ddlLasUsa.SelectedValue="1";// = "N009-OSJ00000038";
            ddlHaSidoCapacitado.SelectedValue="1";// = "N009-OSJ00000039";
            ddlNuca12Meses.SelectedValue="0";// = "N009-OSJ00000040";
            ddlNucaUltMeses.SelectedValue="0";// = "N009-OSJ00000041";
            ddlNuca7dias.SelectedValue="0";// = "N009-OSJ00000042";
            ddlHombroDer12Meses.SelectedValue="0";// = "N009-OSJ00000043";
            ddlHombroDerUltMeses.SelectedValue="0";// = "N009-OSJ00000044";
            ddlHombroDer7dias.SelectedValue="0";// = "N009-OSJ00000045";
            ddlHombroIzq12Meses.SelectedValue="0";// = "N009-OSJ00000049";
            ddlHombroIzqUltMeses.SelectedValue="0";// = "N009-OSJ00000050";
            ddlHombroIzq7dias.SelectedValue="0";// = "N009-OSJ00000051";
            ddlAmbosHombros12Meses.SelectedValue="0";// = "N009-OSJ00000052";
            ddlAmbosHombrosUltMeses.SelectedValue="0";// = "N009-OSJ00000053";
            ddlAmbosHombros7dias.SelectedValue="0";// = "N009-OSJ00000054";
            ddlCodoDer12Meses.SelectedValue="0";// = "N009-OSJ00000055";
            ddlCodoDerUltMeses.SelectedValue="0";// = "N009-OSJ00000056";
            ddlCodoDer7dias.SelectedValue="0";// = "N009-OSJ00000057";
            ddlCodoIzq12Meses.SelectedValue="0";// = "N009-OSJ00000058";
            ddlCodoIzqUltMeses.SelectedValue="0";// = "N009-OSJ00000059";
            ddlCodoIzq7dias.SelectedValue="0";// = "N009-OSJ00000060";
            ddlAmbosCodos12Meses.SelectedValue="0";// = "N009-OSJ00000061";
            ddlAmbosCodosUltMeses.SelectedValue="0";// = "N009-OSJ00000062";
            ddlAmbosCodos7dias.SelectedValue="0";// = "N009-OSJ00000063";
            ddlManoDer12Meses.SelectedValue="0";// = "N009-OSJ00000064";
            ddlManoDerUltMeses.SelectedValue="0";// = "N009-OSJ00000065";
            ddlManoDer7dias.SelectedValue="0";// = "N009-OSJ00000066";
            ddlManoIzq12Meses.SelectedValue="0";// = "N009-OSJ00000067";
            ddlManoIzqUltMeses.SelectedValue="0";// = "N009-OSJ00000068";
            ddlManoIzq7dias.SelectedValue="0";// = "N009-OSJ00000069";
            ddlAmbasManos12Meses.SelectedValue="0";// = "N009-OSJ00000070";
            ddlAmbasManosUltMeses.SelectedValue="0";// = "N009-OSJ00000071";
            ddlAmbasManos7dias.SelectedValue="0";// = "N009-OSJ00000072";
            ddlColumnaDorsal12Meses.SelectedValue="0";// = "N009-OSJ00000073";
            ddlClomunaDorsalUltMeses.SelectedValue="0";// = "N009-OSJ00000074";
            ddlColumnaDorsal7dias.SelectedValue="0";// = "N009-OSJ00000075";
            ddlColumnaLumbar12Meses.SelectedValue="0";// = "N009-OSJ00000076";
            ddlClñoumnaLumbarUltMeses.SelectedValue="0";// = "N009-OSJ00000077";
            ddlColumnaLumbar7dias.SelectedValue="0";// = "N009-OSJ00000078";
            ddlCaderaDer12Meses.SelectedValue="0";// = "N009-OSJ00000079";
            ddlCaderaDerUltMeses.SelectedValue="0";// = "N009-OSJ00000080";
            ddlCaderaDer7dias.SelectedValue="0";// = "N009-OSJ00000081";
            ddlCaderaIzq12Meses.SelectedValue="0";// = "N009-OSJ00000082";
            ddlCaderaIzqUltMeses.SelectedValue="0";// = "N009-OSJ00000083";
            ddlCaderaIzq7dias.SelectedValue="0";// = "N009-OSJ00000084";
            ddlRodillaDer12Meses.SelectedValue="0";// = "N009-OSJ00000085";
            ddlRodillaDerUltMeses.SelectedValue="0";// = "N009-OSJ00000086";
            ddlRodillaDer7dias.SelectedValue="0";// = "N009-OSJ00000087";
            ddlRodillaIzq12Meses.SelectedValue="0";// = "N009-OSJ00000088";
            ddlRodillaIzqUltMeses.SelectedValue="0";// = "N009-OSJ00000089";
            ddlRodillaIzq7dias.SelectedValue="0";// = "N009-OSJ00000090";
            ddlTobilloDer12Meses.SelectedValue="0";// = "N009-OSJ00000091";
            dllTobilloDerUltMeses.SelectedValue="0";// = "N009-OSJ00000092";
            ddlTobilloDer7dias.SelectedValue="0";// = "N009-OSJ00000093";
            ddlTobilloIzq12Meses.SelectedValue="0";// = "N009-OSJ00000094";
            ddlTobilloIzqUltMeses.SelectedValue="0";// = "N009-OSJ00000095";
            ddlTobilloIzq7dias.SelectedValue="0";// = "N009-OSJ00000096";
          

            ddlCervicalAP.SelectedValue="3";// = "N009-OSJ00000103";
            ddlDorsalAP.SelectedValue="3";// = "N009-OSJ00000104";
            ddlLumbarAP.SelectedValue="3";// = "N009-OSJ00000105";
            ddlDOrsalLAT.SelectedValue="1";// = "N009-OSJ00000106";
            ddlLumbarLAT.SelectedValue="1";// = "N009-OSJ00000107";
            ddlLasegeDer.SelectedValue="2";// = "N009-OSJ00000114";
            ddlLasegueIzq.SelectedValue="2";// = "N009-OSJ00000115";
            ddlBragardDer.SelectedValue="2";// = "N009-OSJ00000116";
            ddlBragardIzq.SelectedValue="2";// = "N009-OSJ00000117";
            ddlPhalenDer.SelectedValue="2";// = "N009-OSJ00000118";
            ddlPhalenIzq.SelectedValue="2";// = "N009-OSJ00000119";
            ddlTinelDer.SelectedValue="2";// = "N009-OSJ00000120";
            ddlTinelIzq.SelectedValue="2";// = "N009-OSJ00000121";

            ddlRodillaDerVaro.SelectedValue="3";// = "N009-OSJ00000124";
            ddlRodillaIzqVaro.SelectedValue="3";// = "N009-OSJ00000125";
            ddlPieDerVaro.SelectedValue="3";// = "N009-OSJ00000126";
            ddlPieIzqVaro.SelectedValue="3";// = "N009-OSJ00000127";
            ddlCodoDerVaro.SelectedValue="3";// = "N009-OSJ00000128";
            ddlCOdoIzqVaro.SelectedValue="3";// = "N009-OSJ00000129";
            ddlHombroDerAdd.SelectedValue="0";// = "N009-OSJ00000130";
            ddlHombroIzqAdd.SelectedValue="0";// = "N009-OSJ00000131";
            ddlCodoDerAdd.SelectedValue="0";// = "N009-OSJ00000132";
            ddlCodoIzqAdd.SelectedValue="0";// = "N009-OSJ00000133";
            ddlMuñecaDerAdd.SelectedValue="0";// = "N009-OSJ00000134";
            ddlMuñecaIzqAdd.SelectedValue="0";// = "N009-OSJ00000135";
            ddlCaderaDerAdd.SelectedValue="0";// = "N009-OSJ00000136";
            ddlCaderaIzqAdd.SelectedValue="0";// = "N009-OSJ00000137";
            ddlRodillaDerAdd.SelectedValue="0";// = "N009-OSJ00000138";
            ddlRodillaIzqAdd.SelectedValue="0";// = "N009-OSJ00000139";
            ddlTobilloDerAdd.SelectedValue="0";// = "N009-OSJ00000140";
            ddlTobilloIzqAdd.SelectedValue="0";// = "N009-OSJ00000141";
            ddlHombroDerFlex.SelectedValue="0";// = "N009-OSJ00000142";
            ddlHombroIzqFlex.SelectedValue="0";// = "N009-OSJ00000143";
            ddlCodoDerFlex.SelectedValue="0";// = "N009-OSJ00000144";
            ddlCodoIzqFlex.SelectedValue="0";// = "N009-OSJ00000145";
            ddlMuñecaDerFlex.SelectedValue="0";// = "N009-OSJ00000146";
            ddlMuñecaIzqFlex.SelectedValue="0";// = "N009-OSJ00000147";
            ddlCaderaDerFLex.SelectedValue="0";// = "N009-OSJ00000148";
            ddlCaderaIzqFLex.SelectedValue="0";// = "N009-OSJ00000149";
            ddlRodillaDerFLex.SelectedValue="0";// = "N009-OSJ00000150";
            ddlRodillaIzqFlex.SelectedValue="0";// = "N009-OSJ00000151";
            ddlTobilloDerFlex.SelectedValue="0";// = "N009-OSJ00000152";
            ddlTobilloIzqFlex.SelectedValue="0";// = "N009-OSJ00000153";
            ddlHombroDerExt.SelectedValue="0";// = "N009-OSJ00000154";
            ddlHombroIzqExt.SelectedValue="0";// = "N009-OSJ00000155";
            ddlCodoDerExt.SelectedValue="0";// = "N009-OSJ00000156";
            ddlCodoIzqExt.SelectedValue="0";// = "N009-OSJ00000157";
            ddlMuñecaDerExt.SelectedValue="0";// = "N009-OSJ00000158";
            ddlMuñecaIzqExt.SelectedValue="0";// = "N009-OSJ00000159";
            ddlCaderaDerExt.SelectedValue="0";// = "N009-OSJ00000160";
            ddlCaderaIzqExt.SelectedValue="0";// = "N009-OSJ00000161";
            ddlRodillaDerExt.SelectedValue="0";// = "N009-OSJ00000162";
            ddlRodillaIzqExt.SelectedValue="0";// = "N009-OSJ00000163";
            ddlTobilloDerExt.SelectedValue="0";// = "N009-OSJ00000164";
            ddlTobilloIzqExt.SelectedValue="0";// = "N009-OSJ00000165";
            ddlHombroDerRotExt.SelectedValue="0";// = "N009-OSJ00000166";
            ddlHombroIzqRotExt.SelectedValue="0";// = "N009-OSJ00000167";
            ddlCodoDerRotExt.SelectedValue="0";// = "N009-OSJ00000168";
            ddlCodoIzqRotExt.SelectedValue="0";// = "N009-OSJ00000169";
            ddlMuñecaDerRotExt.SelectedValue="0";// = "N009-OSJ00000170";
            ddlMuñecaIzqRotExt.SelectedValue="0";// = "N009-OSJ00000171";
            ddlCaderaDerRotExt.SelectedValue="0";// = "N009-OSJ00000172";
            ddlCaderaIzqRotExt.SelectedValue="0";// = "N009-OSJ00000173";
            ddlRodillaDerRotExt.SelectedValue="0";// = "N009-OSJ00000174";
            ddlRodillaIzqRotExt.SelectedValue="0";// = "N009-OSJ00000175";
            ddlTobilloDerRotExt.SelectedValue="0";// = "N009-OSJ00000176";
            ddlTobilloIzqRotExt.SelectedValue="0";// = "N009-OSJ00000177";
            ddlHombroDerRotInt.SelectedValue="0";// = "N009-OSJ00000178";
            ddlHombroIzqRotInt.SelectedValue="0";// = "N009-OSJ00000179";
            ddlCodoDerRotInt.SelectedValue="0";// = "N009-OSJ00000180";
            ddlCodoIzqRotInt.SelectedValue="0";// = "N009-OSJ00000181";
            ddlMuñecaDerRotInt.SelectedValue="0";// = "N009-OSJ00000182";
            ddlMuñecaIzqRotInt.SelectedValue="0";// = "N009-OSJ00000183";
            ddlCaderaDerRotInt.SelectedValue="0";// = "N009-OSJ00000184";
            ddlCaderaIzqRotInt.SelectedValue="0";// = "N009-OSJ00000185";
            ddlRodillaDerRotInt.SelectedValue="0";// = "N009-OSJ00000186";
            ddlRodillaIzqRotInt.SelectedValue="0";// = "N009-OSJ00000187";
            ddlTobilloDerRotInt.SelectedValue="0";// = "N009-OSJ00000188";
            ddlTobilloIzqRotInt.SelectedValue="0";// = "N009-OSJ00000189";
            ddlHombroDerIrrad.SelectedValue="0";// = "N009-OSJ00000190";
            ddlHombroIzqIrrad.SelectedValue="0";// = "N009-OSJ00000191";
            ddlCodoDerIrrad.SelectedValue="0";// = "N009-OSJ00000192";
            ddlCodoIzqIrrad.SelectedValue="0";// = "N009-OSJ00000193";
            ddlMuñecaDerIrrad.SelectedValue="0";// = "N009-OSJ00000194";
            ddlMuñecaIzqIrrad.SelectedValue="0";// = "N009-OSJ00000195";
            ddlCaderaDerIrrad.SelectedValue="0";// = "N009-OSJ00000196";
            ddlCaderaIzqIrrad.SelectedValue="0";// = "N009-OSJ00000197";
            ddlRodillaDerIrrad.SelectedValue="0";// = "N009-OSJ00000198";
            ddlRodillaIzqIrrad.SelectedValue="0";// = "N009-OSJ00000199";
            ddlTobilloDerIrrad.SelectedValue="0";// = "N009-OSJ00000200";
            ddlTobilloIzqIrrad.SelectedValue="0";// = "N009-OSJ00000201";
            ddlHombroDerMasaMusc.SelectedValue="0";// = "N009-OSJ00000202";
            ddlHombroIzqMasaMusc.SelectedValue="0";// = "N009-OSJ00000203";
            ddlCodoDerMasaMusc.SelectedValue="0";// = "N009-OSJ00000204";
            ddlCodoIzqMasaMusc.SelectedValue="0";// = "N009-OSJ00000205";
            ddlMuñecaDerMasaMusc.SelectedValue="0";// = "N009-OSJ00000206";
            ddlMuñecaIzqMasaMusc.SelectedValue="0";// = "N009-OSJ00000207";
            ddlCaderaDerMasaMusc.SelectedValue="0";// = "N009-OSJ00000208";
            ddlCaderaIzqMasaMusc.SelectedValue="0";// = "N009-OSJ00000209";
            ddlRodillaDerMasaMusc.SelectedValue="0";// = "N009-OSJ00000210";
            ddlRodillaIzqMasaMusc.SelectedValue="0";// = "N009-OSJ00000211";
            ddlTobilloDerMasaMusc.SelectedValue="0";// = "N009-OSJ00000212";
            ddlTobilloIzqMasaMusc.SelectedValue="0";// = "N009-OSJ00000212";
            ddlAptitud.SelectedValue="2";// = "N009-OSJ00000212";
           
            SearchControlAndSetEvents(this);
        }

        private void SearchControlAndSetEvents(Control ctrlContainer)
        {
            foreach (Control ctrl in ctrlContainer.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.Name.Contains("N009-OSJ"))
                    {
                        ctrl.Leave += new EventHandler(lbl_Leave);
                    }
                }
                if (ctrl is ComboBox)
                {
                    if (ctrl.Name.Contains("N009-OSJ"))
                    {
                        var obj = (ComboBox)ctrl;
                        obj.SelectedValueChanged += new EventHandler(ucd_TextChanged);
                     
                        //ctrl.Leave += new EventHandler(ucd_Leave);
                    }
                }
                if (ctrl.HasChildren)
                    SearchControlAndSetEvents(ctrl);

            }
        }

        private void ucd_TextChanged(object sender, System.EventArgs e)
        {
            // Capturar el control invocador
            ComboBox senderCtrl = (ComboBox)sender;

            SaveValueControlForInterfacingESO(senderCtrl.Name, senderCtrl.SelectedValue.ToString());

            _isChangueValueControl = true;
         
        }

        private void lbl_Leave(object sender, System.EventArgs e)
        {
            TextBox senderCtrl = (TextBox)sender;

            SaveValueControlForInterfacingESO(senderCtrl.Name, senderCtrl.Text.ToString());

            _isChangueValueControl = true;
        }

        private void ucd_Leave(object sender, System.EventArgs e)
        {
            TextBox senderCtrl = (TextBox)sender;

            SaveValueControlForInterfacingESO(senderCtrl.Name, senderCtrl.Text.ToString());

            _isChangueValueControl = true;
        }      

        private void SaveValueControlForInterfacingESO(string name, string value)
        {
            #region Capturar Valor del campo

            _listOfAtencionAdulto1.RemoveAll(p => p.v_ComponentFieldId == name);

            _UserControlValores = new ServiceComponentFieldValuesList();

            _UserControlValores.v_ComponentFieldId = name;
            _UserControlValores.v_Value1 = value;
            _UserControlValores.v_ComponentId = Constants.OSTEO_SJ_ID;

            _listOfAtencionAdulto1.Add(_UserControlValores);

            DataSource = _listOfAtencionAdulto1;

            #endregion
        }

    }
}
