using FineUI;
using Sigesoft.Common;
using Sigesoft.Server.WebClientAdmin.BE;
using Sigesoft.Server.WebClientAdmin.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Sigesoft.Server.WebClientAdmin.UI.Auditar
{
    public partial class FRM033G : System.Web.UI.Page
    {
        ServiceBL oServiceBL = new ServiceBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadCombo();
                btnClose.OnClientClick = ActiveWindow.GetConfirmHideReference();
            }    
        }

        private void loadCombo()
        {
            OperationResult objOperationResult = new OperationResult();

            if (((ClientSession)Session["objClientSession"]).i_ProfesionId == 30)
            {
                //Llenar combo consultorio 
                int Nodo = 9;
                int RolId = int.Parse(((ClientSession)Session["objClientSession"]).i_RoleId.ToString());


                // Obtener permisos de cada examen de un rol especifico
                var componentProfile = oServiceBL.GetRoleNodeComponentProfileByRoleNodeId(Nodo, RolId);

                var _componentListTemp = oServiceBL.GetAllComponents(ref objOperationResult);

                Session["componentListTemp"] = _componentListTemp;
                var xxx = _componentListTemp.FindAll(p => p.Value4 != -1);

                List<KeyValueDTO> groupComponentList = xxx.GroupBy(x => x.Value4).Select(group => group.First()).ToList();

                groupComponentList.AddRange(_componentListTemp.ToList().FindAll(p => p.Value4 == -1));
                // Remover los componentes que no estan asignados al rol del usuario
                var results = groupComponentList.FindAll(f => componentProfile.Any(t => t.v_ComponentId == f.Value2));


                Utils.LoadDropDownList(ddlConsultorio, "Value1", "Value4", results, DropDownListAction.Select);
                ddlConsultorio.SelectedIndex = 1;

                var CategoriaId = ddlConsultorio.SelectedValue;

                //Obtener los componetes de ese servicio en función de la categoría seleccionada

                var Examenes = oServiceBL.DevolverExamenesPorCategoria(Session["ServiceId"].ToString(), int.Parse(CategoriaId));

                Utils.LoadDropDownList(ddlExamen, "Value1", "Id", Examenes, DropDownListAction.Select);


            }
            else if (((ClientSession)Session["objClientSession"]).i_ProfesionId == 31)
            {
                //Llenar combo consultorio 
                int Nodo = 9;
                int RolId = int.Parse(((ClientSession)Session["objClientSession"]).i_RoleId.ToString());


                // Obtener permisos de cada examen de un rol especifico
                var componentProfile = oServiceBL.GetRoleNodeComponentProfileByRoleNodeId(Nodo, RolId);

                var _componentListTemp = oServiceBL.GetAllComponents(ref objOperationResult);

                Session["componentListTemp"] = _componentListTemp;
                var xxx = _componentListTemp.FindAll(p => p.Value4 != -1);

                List<KeyValueDTO> groupComponentList = xxx.GroupBy(x => x.Value4).Select(group => group.First()).ToList();

                groupComponentList.AddRange(_componentListTemp.ToList().FindAll(p => p.Value4 == -1));

                //ddlConsultorio.Enabled = false;
                Utils.LoadDropDownList(ddlConsultorio, "Value1", "Value4", groupComponentList, DropDownListAction.All);
               
            }
        }

        protected void ddlConsultorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obtener Componentes del consultorio en función de su protoclo
            var CategoriaId = ddlConsultorio.SelectedValue;

            //Obtener los componetes de ese servicio en función de la categoría seleccionada

            var Examenes = oServiceBL.DevolverExamenesPorCategoria(Session["ServiceId"].ToString(), int.Parse(CategoriaId));

            Utils.LoadDropDownList(ddlExamen, "Value1", "Id", Examenes, DropDownListAction.Select);


        }

        protected void grdCie10_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            int index = e.RowIndex;
            var dataKeys = grdCie10.DataKeys[index];
            //Session["Cie10Id"] = dataKeys[3];
            Session["DiseasesId"] = dataKeys[2];
            Session["v_DxFrecuenteId"] = dataKeys[3];            

            //Obtener las Recomendaciones y restricciones

          txtRecomendacion1.Text = oServiceBL.ObtenerRecomendacionesPorDxFrecuente(Session["v_DxFrecuenteId"].ToString(), 1);
          txtRestriccion1.Text = oServiceBL.ObtenerRecomendacionesPorDxFrecuente(Session["v_DxFrecuenteId"].ToString(), 2);

        }

        protected void grdCie10_PageIndexChange(object sender, FineUI.GridPageEventArgs e)
        {
            grdCie10.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnCie10_Click(object sender, EventArgs e)
        {
            List<string> Filters = new List<string>();
            if (!string.IsNullOrEmpty(txtDx.Text)) Filters.Add("v_CIE10Idv_Name.Contains(\"" + txtDx.Text.Trim() + "\")");
            string strFilterExpression = null;
            if (Filters.Count > 0)
            {
                foreach (string item in Filters)
                {
                    strFilterExpression = strFilterExpression + item + " && ";
                }
                strFilterExpression = strFilterExpression.Substring(0, strFilterExpression.Length - 4);
            }

            // Save the Filter expression in the Session
            Session["strFilterExpression"] = strFilterExpression;

            // Refresh the grid
            grdCie10.PageIndex = 0;
            this.BindGrid();
        }

        private void BindGrid()
        {
            string strFilterExpression = Convert.ToString(Session["strFilterExpression"]);
            OperationResult objOperationResult = new OperationResult();

            grdCie10.DataSource = oServiceBL.GetDxFrecuentes(ref objOperationResult, grdCie10.PageIndex, grdCie10.PageSize, "v_CIE10Id", strFilterExpression);
            grdCie10.DataBind();
        }

        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {

            if (ddlExamen.SelectedValue == null || ddlExamen.SelectedValue == "-1")
            {
                Alert.Show("Por favor seleccione un examen.");
                return; 
            }
            OperationResult objOperationResult = new OperationResult();

            //Grabar el Dx en el servicio
            Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList _diagnosticRepository = new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList();
            List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> _ListadiagnosticRepository = new List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList>();

            List<Sigesoft.Node.WinClient.BE.RecomendationList> Recomendaciones = new List<Node.WinClient.BE.RecomendationList>();
            Sigesoft.Node.WinClient.BE.RecomendationList Recomendacion = null;


            List<Sigesoft.Node.WinClient.BE.RestrictionList> Restricciones = new List<Node.WinClient.BE.RestrictionList>();
            Sigesoft.Node.WinClient.BE.RestrictionList Restriccion = null;



            _diagnosticRepository.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
            _diagnosticRepository.v_DiseasesId = Session["DiseasesId"].ToString();
            _diagnosticRepository.i_AutoManualId = 1;
            _diagnosticRepository.i_PreQualificationId = 1;
            _diagnosticRepository.i_FinalQualificationId = (int)FinalQualification.Definitivo;
            _diagnosticRepository.i_DiagnosticTypeId = (int)TipoDx.Enfermedad_Comun;

            _diagnosticRepository.v_ServiceId = Session["ServiceId"].ToString();
            _diagnosticRepository.v_ComponentId = ddlExamen.SelectedValue.ToString();  //_componentId;
            _diagnosticRepository.i_RecordStatus = (int)RecordStatus.Agregado;
            _diagnosticRepository.i_RecordType = (int)RecordType.Temporal;


            var ListaReco = oServiceBL.ObtenerListaRecomendacionesPorDxFrecuente(Session["v_DxFrecuenteId"].ToString(), 1);

            foreach (var item in ListaReco)
            {
                Recomendacion = new Node.WinClient.BE.RecomendationList();

                Recomendacion.v_ServiceId = Session["ServiceId"].ToString();
                Recomendacion.v_ComponentId = ddlExamen.SelectedValue.ToString();
                Recomendacion.v_MasterRecommendationId = item.v_MasterRecommendationRestricctionId;
                Recomendacion.i_RecordType = (int)RecordType.Temporal;
                Recomendacion.i_RecordStatus = (int)RecordStatus.Agregado;
                Recomendaciones.Add(Recomendacion);
            }

            var ListaRestri = oServiceBL.ObtenerListaRecomendacionesPorDxFrecuente(Session["v_DxFrecuenteId"].ToString(), 2);

            foreach (var item1 in ListaRestri)
            {
                Restriccion = new Node.WinClient.BE.RestrictionList();

                Restriccion.v_ServiceId = Session["ServiceId"].ToString();
                Restriccion.v_ComponentId = ddlExamen.SelectedValue.ToString();
                Restriccion.v_MasterRestrictionId = item1.v_MasterRecommendationRestricctionId;
                Restriccion.i_RecordType = (int)RecordType.Temporal;
                Restriccion.i_RecordStatus = (int)RecordStatus.Agregado;
                Restricciones.Add(Restriccion);
            }



            _diagnosticRepository.Restrictions = Restricciones;
            _diagnosticRepository.Recomendations = Recomendaciones;

            _ListadiagnosticRepository.Add(_diagnosticRepository);

            oServiceBL.AddDiagnosticRepository(ref objOperationResult, _ListadiagnosticRepository, null, ((ClientSession)Session["objClientSession"]).GetAsList(), true);

            if (objOperationResult.Success == 1)  // Operación sin error
            {
                // Cerrar página actual y hacer postback en el padre para actualizar
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            else  // Operación con error
            {
                Alert.ShowInTop("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage);
                // Se queda en el formulario.
            }


        }

        protected void WindowCie10_Close(object sender, EventArgs e)
        {

        }
    }
}