using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.BE;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmLog : Form
    {
        LogBL _objBL = new LogBL();
        string strFilterExpression;
        private int _intNodeId;

        public frmLog()
        {
            InitializeComponent();
        }

        private void frmAdministracion_Load(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();

            _intNodeId = int.Parse(Common.Utils.GetApplicationConfigValue("NodeId"));  
            // Establecer el filtro inicial para los datos
            strFilterExpression = null;
            //_Util.SetFormActionsInSession("FRM001");
            //btnNew.Enabled = _Util.IsActionEnabled("FRM001_ADD");

            //Llenado de combos
            Utils.LoadDropDownList(ddlEventTypeId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 102,null), DropDownListAction.All);
            Utils.LoadDropDownList(ddlSuccess, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.All);

            dtpDateTimeStar.CustomFormat = "dd/MM/yyyy";
            dptDateTimeEnd.CustomFormat = "dd/MM/yyyy";
            btnFilter_Click(sender, e);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Get the filters from the UI
            List<string> Filters = new List<string>();
            if (ddlEventTypeId.SelectedValue.ToString() != "-1") Filters.Add("i_EventTypeId==" + ddlEventTypeId.SelectedValue);
            if (ddlSuccess.SelectedValue.ToString() != "-1") Filters.Add("i_Success==" + ddlSuccess.SelectedValue);
            Filters.Add("i_NodeId==" + _intNodeId);
      
            if (!string.IsNullOrEmpty(txtUserName.Text)) Filters.Add("v_SystemUserName.Contains(\"" + txtUserName.Text.Trim() + "\")");
            if (!string.IsNullOrEmpty(txtProcessEntity.Text)) Filters.Add("v_ProcessEntity.Contains(\"" + txtProcessEntity.Text.Trim() + "\")");
            if (!string.IsNullOrEmpty(txtElementItem.Text)) Filters.Add("v_ElementItem.Contains(\"" + txtElementItem.Text.Trim() + "\")");
      
            // Create the Filter Expression
            strFilterExpression = null;
            if (Filters.Count > 0)
            {
                foreach (string item in Filters)
                {
                    strFilterExpression = strFilterExpression + item + " && ";
                }
                strFilterExpression = strFilterExpression.Substring(0, strFilterExpression.Length - 4);
            }

            this.BindGrid();

            // Utilitario para obtener los tamaños de las columnas de la grilla
            //Clipboard.SetText(Utils.GetGridColumnsDetail(grdData));
        }

        private void BindGrid()
        {

            var objData = GetData(0, 5000, "d_Date DESC", strFilterExpression);
            
            grdData.DataSource = objData;
            lblRecordCount.Text = string.Format("Se encontraron {0} registros.", objData.Count());
        }

        private List<LogList> GetData(int pintPageIndex, int pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
            DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);
          
            var _objData = _objBL.GetLogsPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, pdatBeginDate, pdatEndDate);
            
            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _objData;
        }

        private void mnuGridVer_Click(object sender, EventArgs e)
        {
            string strLogId = grdData.Selected.Rows[0].Cells[0].Value.ToString();
            frmLogEdicion frm = new frmLogEdicion(strLogId);
            frm.ShowDialog();
        }

        private void grdData_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point point = new System.Drawing.Point(e.X, e.Y);
                Infragistics.Win.UIElement uiElement = ((Infragistics.Win.UltraWinGrid.UltraGridBase)sender).DisplayLayout.UIElement.ElementFromPoint(point);

                if (uiElement == null || uiElement.Parent == null)
                    return;

                Infragistics.Win.UltraWinGrid.UltraGridRow row = (Infragistics.Win.UltraWinGrid.UltraGridRow)uiElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow));

                if (row != null)
                {
                    grdData.Rows[row.Index].Selected = true;
                    contextMenuStrip1.Items["toolStripMenuItem2"].Enabled = true;
                }
                else
                {
                    contextMenuStrip1.Items["toolStripMenuItem2"].Enabled = false;
                }
            } 
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void ddlSuccess_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateandSelect_Click(object sender, EventArgs e)
        {
            string strLogId = grdData.Selected.Rows[0].Cells[0].Value.ToString();
            frmLogEdicion frm = new frmLogEdicion(strLogId);
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the filters from the UI
            List<string> Filters = new List<string>();
            if (ddlEventTypeId.SelectedValue.ToString() != "-1") Filters.Add("i_EventTypeId==" + ddlEventTypeId.SelectedValue);
            if (ddlSuccess.SelectedValue.ToString() != "-1") Filters.Add("i_Success==" + ddlSuccess.SelectedValue);
            Filters.Add("i_NodeId==" + _intNodeId);

            if (!string.IsNullOrEmpty(txtUserName.Text)) Filters.Add("v_SystemUserName.Contains(\"" + txtUserName.Text.Trim() + "\")");
            if (!string.IsNullOrEmpty(txtProcessEntity.Text)) Filters.Add("v_ProcessEntity.Contains(\"" + txtProcessEntity.Text.Trim() + "\")");
            if (!string.IsNullOrEmpty(txtElementItem.Text)) Filters.Add("v_ElementItem.Contains(\"" + txtElementItem.Text.Trim() + "\")");

            // Create the Filter Expression
            strFilterExpression = null;
            if (Filters.Count > 0)
            {
                foreach (string item in Filters)
                {
                    strFilterExpression = strFilterExpression + item + " && ";
                }
                strFilterExpression = strFilterExpression.Substring(0, strFilterExpression.Length - 4);
            }

            this.BindGrid();

            // Utilitario para obtener los tamaños de las columnas de la grilla
            //Clipboard.SetText(Utils.GetGridColumnsDetail(grdData));
        }
        
    }
}
