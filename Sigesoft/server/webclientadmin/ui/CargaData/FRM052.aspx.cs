using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sigesoft.Server.WebClientAdmin.BLL;
using Sigesoft.Server.WebClientAdmin.BE;
using Sigesoft.Common;


namespace Sigesoft.Server.WebClientAdmin.UI.CargaData
{
    public partial class FRM052 : System.Web.UI.Page
    {
        List<Sigesoft.Node.WinClient.BE.PacientList> _TempPacientList;
        Sigesoft.Node.WinClient.BE.PacientList TempPacient;
        DataHierarchyBL _objDataHierarchyBL = new DataHierarchyBL();
        SystemParameterBL _objSystemParameterBL = new SystemParameterBL();
        ProtocolBL oProtocolBL = new ProtocolBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadComboBox();
                btnClose.OnClientClick = ActiveWindow.GetConfirmHideReference();
            }
        }

        private void LoadComboBox()
        {
            OperationResult objOperationResult = new OperationResult();

            var _DocType = _objDataHierarchyBL.GetDataHierarchyForCombo(ref objOperationResult, 106);

            Utils.LoadDropDownList(ddlDocumento, "Description", "Id", _DocType, DropDownListAction.Select);
            Utils.LoadDropDownList(ddlGenero, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 100), DropDownListAction.Select);
            //Utils.LoadDropDownList(ddlProtocoloId, "v_Name", "Id", oProtocolBL.DevolverProtocolosPorEmpresaOnly(Session["EmpresaClienteId"].ToString()), DropDownListAction.Select);
     
        }
        protected void btnSaveRefresh_Click(object sender, EventArgs e)
        {
            _TempPacientList = new List<Sigesoft.Node.WinClient.BE.PacientList>();
            TempPacient = new Sigesoft.Node.WinClient.BE.PacientList();

            TempPacient.i_Correlative = 1;
            TempPacient.v_FirstName = txtNombres.Text;
            TempPacient.v_FirstLastName = txtApellidoPaterno.Text;
            TempPacient.v_SecondLastName = txtApellidoMaterno.Text;
            TempPacient.i_DocTypeId = int.Parse(ddlDocumento.SelectedValue.ToString());
            TempPacient.v_DocTypeName = ddlDocumento.SelectedText;
            TempPacient.v_DocNumber = txtNroDocumento.Text;
            TempPacient.i_SexTypeId = int.Parse(ddlGenero.SelectedValue.ToString());
            TempPacient.v_SexTypeName = ddlGenero.SelectedText;
            TempPacient.d_Birthdate = dpFechaNacimiento.SelectedDate;
            TempPacient.v_CurrentOccupation = txtPuesto.Text;
            TempPacient.v_ProtocoloId = Session["ProtocoloNombre"].ToString();

            _TempPacientList.Add(TempPacient);
            Session["_TempPacientList"] = _TempPacientList;

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference()); 
        }
    }
}