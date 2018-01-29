using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.BE;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;

namespace Sigesoft.Node.WinClient.UI.UserControls
{
    public partial class ucEspirometria : UserControl
    {
       
        List<RecomendationList> _recomendations = null;
        List<RestrictionList> _restrictions = null;
        List<DiagnosticRepositoryList> _dx = null;
        bool _isChangueValueControl = false;

        ServiceComponentFieldValuesList _espirometria = null;
        List<ServiceComponentFieldValuesList> _listEspirometria = new List<ServiceComponentFieldValuesList>();

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
                return _listEspirometria;
            }
            set
            {
                if (value != _listEspirometria)
                {
                    ClearValueControl();
                    _listEspirometria = value;
                    SearchControlAndFill(value);
                }
            }
        }

        public void ClearValueControl()
        {
            _isChangueValueControl = false;
            txtcvf.Text = string.Empty;
            txtvef1.Text = string.Empty;
            txtdiv.Text = string.Empty;
            txtrp.Text = string.Empty;
        }

        public bool IsChangeValueControl { get { return _isChangueValueControl; } }
     
        #endregion

        public ucEspirometria()
        {
            InitializeComponent();
        }

        private void ucEspirometria_Load(object sender, EventArgs e)
        {
            txtcvf.Name = Constants.TXT_ESP_CVF;
            txtvef1.Name = Constants.TXT_ESP_VEF1;
            txtdiv.Name = Constants.TXT_ESP_CVF_VEF1;
            txtrp.Name = Constants.txt_ESP_DX_AUTO;

            LoadDxAndRecoAndRes();
        }

        private void txtvef1_TextChanged(object sender, EventArgs e)
        {
            Calcualte();
            SaveValueControlForInterfacingESO(txtvef1.Name, txtvef1.Text);
        }

        private void txtcvf_TextChanged(object sender, EventArgs e)
        {
            Calcualte();
            SaveValueControlForInterfacingESO(txtcvf.Name, txtcvf.Text);
        }

        private void txtdiv_TextChanged(object sender, EventArgs e)
        {
            SaveValueControlForInterfacingESO(txtdiv.Name, txtdiv.Text);
        }

        // Alejandro
        private DiagnosticRepositoryList SearchDxSugeridoOfSystem(string dx, string componentFieldsId)
        {
            DiagnosticRepositoryList diagnosticRepository = null;
            // Buscar reco / res asociadas a un dx


            var diagnostic = GetDxByName(dx);

            if (diagnostic != null)
            {
                // Insertar DX sugerido (automático) a la bolsa de DX 
                diagnosticRepository = new DiagnosticRepositoryList();

                diagnosticRepository.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                diagnosticRepository.v_DiseasesId = diagnostic.v_DiseasesId;
                diagnosticRepository.i_AutoManualId = (int)AutoManual.Automático;
                diagnosticRepository.i_PreQualificationId = (int)PreQualification.SinPreCalificar;
                diagnosticRepository.i_FinalQualificationId = (int)FinalQualification.SinCalificar;
                diagnosticRepository.v_ServiceId = "";
                diagnosticRepository.v_ComponentId = Constants.ESPIROMETRIA_ID;
                diagnosticRepository.v_DiseasesName = diagnostic.v_DiseasesName;
                diagnosticRepository.v_AutoManualName = "AUTOMÁTICO";
                diagnosticRepository.v_RestrictionsName = string.Join(", ", diagnostic.Restrictions.Select(p => p.v_RestrictionName));
                diagnosticRepository.v_RecomendationsName = string.Join(", ", diagnostic.Recomendations.Select(p => p.v_RecommendationName));
                diagnosticRepository.v_PreQualificationName = "SIN PRE-CALIFICAR";

                // ID enlace DX automatico para grabar valores dinamicos
                //diagnosticRepository.v_ComponentFieldValuesId = val.v_ComponentFieldValuesId;
                diagnosticRepository.v_ComponentFieldsId = componentFieldsId;
                diagnosticRepository.Recomendations = diagnostic.Recomendations;
                diagnosticRepository.Restrictions = diagnostic.Restrictions;
                diagnosticRepository.i_RecordStatus = (int)RecordStatus.Agregado;
                diagnosticRepository.i_RecordType = (int)RecordType.Temporal;


                //diagnosticRepository.d_ExpirationDateDiagnostic = DateTime.Now.AddMonths(vm);
            }
            else
            {

            }

            return diagnosticRepository;
        }


        // loco revisar
        private void LoadDxAndRecoAndRes()
        {

            _recomendations = new List<RecomendationList>()
            {
                //new RecomendationList { v_MasterRecommendationId = "N009-MR000000502", v_DiseasesId = Constants.OBSTRUCCIÓN_LEVE, v_RecommendationName = "CITA CON NEUMOLOGÍA" },
                //new RecomendationList { v_MasterRecommendationId = "N009-MR000000502", v_DiseasesId = Constants.RESTRICCIÓN_LEVE, v_RecommendationName = "CITA CON NEUMOLOGÍA" },
                //new RecomendationList { v_MasterRecommendationId = "N009-MR000000502", v_DiseasesId = Constants.PATRÓN_MIXTO_LEVE, v_RecommendationName = "CITA CON NEUMOLOGÍA" }             
            };

            _restrictions = new List<RestrictionList>()
            {
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000503", v_DiseasesId = Constants.OBSTRUCCIÓN_LEVE, v_RestrictionName = "NO LABORAR EN ÁREAS DE ACUMULACIÓN DE POLVO Y GASES" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000504", v_DiseasesId = Constants.OBSTRUCCIÓN_MODERADA, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000503", v_DiseasesId = Constants.OBSTRUCCIÓN_MODERADA, v_RestrictionName = "NO LABORAR EN ÁREAS DE ACUMULACIÓN DE POLVO Y GASES"},
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.OBSTRUCCIÓN_MODERADAMENTE_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.OBSTRUCCIÓN_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.OBSTRUCCIÓN_MUY_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR"},

                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000503", v_DiseasesId = Constants.RESTRICCIÓN_LEVE, v_RestrictionName = "NO LABORAR EN ÁREAS DE ACUMULACIÓN DE POLVO Y GASES" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.RESTRICCIÓN_MODERADA, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.RESTRICCIÓN_MODERADAMENTE_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR"},
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.RESTRICCIÓN_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.RESTRICCIÓN_MUY_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR"},

                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.PATRÓN_MIXTO_MUY_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.PATRÓN_MIXTO_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR"  },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.PATRÓN_MIXTO_MODERADAMENTE_GRAVE, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR" },
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000505", v_DiseasesId = Constants.PATRÓN_MIXTO_MODERADO, v_RestrictionName = "INTERCONSULTA CON NEUMOLOGÍA ANTES DE LABORAR"},
                //new RestrictionList { v_MasterRestrictionId = "N009-MR000000503", v_DiseasesId = Constants.PATRÓN_MIXTO_LEVE, v_RestrictionName = "NO LABORAR EN ÁREAS DE ACUMULACIÓN DE POLVO Y GASES"}

            };

            _dx = new List<DiagnosticRepositoryList>()
            {
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.OBSTRUCCIÓN_VARIANTE_FISIOLÓGICA, v_DiseasesName = "Obstrucción Variante Fisiológica" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.OBSTRUCCIÓN_LEVE, v_DiseasesName = "Obstrucción Leve" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.OBSTRUCCIÓN_MODERADA, v_DiseasesName = "Obstrucción Moderada" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.OBSTRUCCIÓN_MODERADAMENTE_GRAVE, v_DiseasesName = "Obstrucción Moderadamente Grave" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.OBSTRUCCIÓN_GRAVE, v_DiseasesName = "Obstrucción Grave" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.OBSTRUCCIÓN_MUY_GRAVE, v_DiseasesName = "Obstrucción Muy Grave" },

                //new DiagnosticRepositoryList { v_DiseasesId = Constants.RESTRICCIÓN_LEVE, v_DiseasesName = "Restricción Leve" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.RESTRICCIÓN_MODERADA, v_DiseasesName = "Restricción Moderada" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.RESTRICCIÓN_MODERADAMENTE_GRAVE, v_DiseasesName = "Restricción Moderadamente Grave" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.RESTRICCIÓN_GRAVE, v_DiseasesName = "Restricción Grave" },             
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.RESTRICCIÓN_MUY_GRAVE, v_DiseasesName = "Restricción Muy Grave" },

                //new DiagnosticRepositoryList { v_DiseasesId = Constants.PATRÓN_MIXTO_MUY_GRAVE, v_DiseasesName = "Patrón Mixto Muy Grave" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.PATRÓN_MIXTO_GRAVE, v_DiseasesName = "Patrón Mixto Grave" },                
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.PATRÓN_MIXTO_MODERADAMENTE_GRAVE, v_DiseasesName = "Patrón Mixto Moderadamente Grave" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.PATRÓN_MIXTO_MODERADO, v_DiseasesName = "Patrón Mixto Moderado" },
                //new DiagnosticRepositoryList { v_DiseasesId = Constants.PATRÓN_MIXTO_LEVE, v_DiseasesName = "Patrón Mixto Leve" }     
            };

        }

        // Alejandro
        private DiagnosticRepositoryList GetDxByName(string dx)
        {
            var sql = (from d in _dx
                       join rec in _recomendations on d.v_DiseasesId equals rec.v_DiseasesId into l_rec
                       join rec in _restrictions on d.v_DiseasesId equals rec.v_DiseasesId into l_res
                       where d.v_DiseasesName == dx
                       select new DiagnosticRepositoryList
                       {
                           v_DiseasesId = d.v_DiseasesId,
                           v_DiseasesName = d.v_DiseasesName,
                           Recomendations = l_rec.ToList(),
                           Restrictions = l_res.ToList()
                       }).FirstOrDefault();

            if (sql != null)
            {
                sql.Recomendations.ForEach(p => { p.v_ComponentId = Constants.ESPIROMETRIA_ID; p.i_RecordStatus = (int)RecordStatus.Agregado; p.i_RecordType = (int)RecordType.Temporal; });
                sql.Restrictions.ForEach(p => { p.v_ComponentId = Constants.ESPIROMETRIA_ID; p.i_RecordStatus = (int)RecordStatus.Agregado; p.i_RecordType = (int)RecordType.Temporal; });
            }

            return sql;
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
                }
            }
        }

        private void SaveValueControlForInterfacingESO(string name, string value)
        {
            #region Capturar Valor del campo

            _listEspirometria.RemoveAll(p => p.v_ComponentFieldId == name);

            _espirometria = new ServiceComponentFieldValuesList();

            _espirometria.v_ComponentFieldId = name;
            _espirometria.v_Value1 = value;

            _listEspirometria.Add(_espirometria);

            DataSource = _listEspirometria;

            #endregion
        }

        private void Calcualte()
        {
            double div = 0;
            var cvf = int.Parse(string.IsNullOrEmpty(txtcvf.Text) ? "0" : txtcvf.Text);
            var vef1 = int.Parse(string.IsNullOrEmpty(txtvef1.Text) ? "0" : txtvef1.Text);

            if (cvf != 0 && vef1 != 0)
            {
                div = (vef1 / cvf * 100);
            }

            txtdiv.Text = div == 0 ? string.Empty : div.ToString();

            #region Logica de formula

            if (cvf >= 80 && div >= 90)
            { txtrp.Text = "Normal"; }
            else
            {
                if (cvf >= 80 && div < 90)
                {
                    if (vef1 >= 100)
                    { txtrp.Text = "Obstrucción Variante Fisiológica"; }
                    else
                    {
                        if (vef1 >= 70 && vef1 < 100)
                        { txtrp.Text = "Obstrucción Leve"; }
                        else
                        {
                            if (vef1 >= 60 && vef1 < 70)
                            { txtrp.Text = "Obstrucción Moderada"; }
                            else
                            {
                                if (vef1 >= 50 && vef1 < 60)
                                { txtrp.Text = "Obstrucción Moderadamente Grave"; }
                                else
                                {
                                    if (vef1 >= 34 && vef1 < 50)
                                    { txtrp.Text = "Obstrucción Grave"; }
                                    else
                                    { txtrp.Text = "Obstrucción Muy Grave"; }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (cvf < 80 && div >= 90)
                    {
                        if (cvf >= 70)
                        { txtrp.Text = "Restricción Leve"; }
                        else
                        {
                            if ((cvf < 70) && (cvf >= 60))
                            { txtrp.Text = "Restricción Moderada"; }
                            else
                            {
                                if ((cvf < 60) && (cvf >= 50))
                                { txtrp.Text = "Restricción Moderadamente Grave"; }
                                else
                                {
                                    if ((cvf < 50) && (cvf >= 34))
                                    { txtrp.Text = "Restricción Grave"; }
                                    else
                                    { txtrp.Text = "Restricción Muy Grave"; }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cvf < 34 || vef1 < 34)
                        { txtrp.Text = "Patrón Mixto Muy Grave"; }
                        else
                        {
                            if ((cvf < 50 && cvf >= 34) || (vef1 < 50 && vef1 >= 34))
                            { txtrp.Text = "Patrón Mixto Grave"; }
                            else
                            {
                                if ((cvf < 60 && cvf >= 50) || (vef1 < 60 && vef1 >= 50))
                                { txtrp.Text = "Patrón Mixto Moderadamente Grave"; }
                                else
                                {
                                    if ((cvf < 70 && cvf >= 60) || (vef1 < 70 && vef1 >= 60))
                                    { txtrp.Text = "Patrón Mixto Moderado"; }
                                    else
                                    { txtrp.Text = "Patrón Mixto Leve"; }
                                }
                            }
                        }
                    }
                }
            }

            if (txtcvf.Text == string.Empty && txtvef1.Text == string.Empty)
                txtrp.Text = string.Empty;

            #endregion

            //SaveValueControlForInterfacingESO(txtvef1.Name, txtvef1.Text);

            #region capturar dx automatico + recomendaciones y restricciones / disparar evento para comunicacion con E.S.O

            // 
            List<DiagnosticRepositoryList> dxList = null;

            var dx = SearchDxSugeridoOfSystem(txtrp.Text, Constants.txt_ESP_DX_AUTO);

            if (dx != null)
            {
                dxList = new List<DiagnosticRepositoryList>();
            }

            if (dx != null)
            {
                dxList.Add(dx);
            }

            List<string> listCf = new List<string>() { Constants.txt_ESP_DX_AUTO };

            // Disparar evento
            OnAfterValueChange(new AudiometriaAfterValueChangeEventArgs { PackageSynchronization = dxList, ListcomponentFieldsId = listCf });

            #endregion
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
