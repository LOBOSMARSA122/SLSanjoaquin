using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sigesoft.Node.WinClient.UI.Reports
{
    public partial class frmConsentimientoHojaRuta : Form
    {
        ServiceBL _serviceBL = new ServiceBL();
        CalendarBL _calendarBL = new CalendarBL();
        private string _serviceId;
        private string _calendarId;
       

        public frmConsentimientoHojaRuta(string serviceId, string calendarId)
        {
            InitializeComponent();
            _serviceId = serviceId;
            _calendarId = calendarId;
        }

        private void frmConsentimientoHojaRuta_Load(object sender, EventArgs e)
        {
           
            using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
            {
                crConsentimientoHojaRuta rp = null;
                rp = new Reports.crConsentimientoHojaRuta();

                //Hoja de Ruta
                OperationResult objOperationResult = new OperationResult();
                // Cabecera
                var headerRoadMap = _calendarBL.GetHeaderRoadMap(_calendarId);
                // Detalle
                var detailRoadMap = _serviceBL.GetServiceComponentsByCategoryExceptLab(ref objOperationResult, _serviceId);

                DataSet ds = new DataSet();

                DataTable dtHeader = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(headerRoadMap);
                DataTable dtDetail = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(detailRoadMap);

                dtHeader.TableName = "dtHeaderRoadMap";
                dtDetail.TableName = "dtDetailRoadMap";

                ds.Tables.Add(dtHeader);
                ds.Tables.Add(dtDetail);
                rp.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                rp.Subreports["crRoadMap.rpt"].SetDataSource(ds);
                rp.DetailSection1.SectionFormat.EnableSuppress = false;

                ////Consentimiento

                var aptitudeCertificate = new PacientBL().GetReportConsentimiento(_serviceId);
                DataSet ds1 = new DataSet();

                DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(aptitudeCertificate);

                dt.TableName = "dtConsentimiento";

                ds1.Tables.Add(dt);
                rp.Subreports["crConsentimiento.rpt"].SetDataSource(ds1);
                rp.Section3.SectionFormat.EnableSuppress = false;

                crystalReportViewer1.EnableDrillDown = false;
                var Path = Application.StartupPath;
                rp.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path + @"\Temp\Reporte.pdf");
                crystalReportViewer1.ReportSource = rp;
                crystalReportViewer1.Show();
            }
        }
    }
}
