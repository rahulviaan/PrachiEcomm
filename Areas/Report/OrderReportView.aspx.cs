using Microsoft.Reporting.WebForms;
using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrachiIndia.Portal.Areas.Report
{
    public partial class OrderReportView : System.Web.UI.Page
    {
        string txtsearch = "";
        string fromdate = "";
        string todate = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                RenderReport();
            }
        }

        private void RenderReport()
        {
            
            txtsearch = Request.QueryString["search"];
            fromdate = Request.QueryString["fromdate"];
            todate = Request.QueryString["todate"];
            //if (String.IsNullOrEmpty(txtsearch) && String.IsNullOrEmpty(fromdate) && String.IsNullOrEmpty(todate))
            //{
            //    fromdate = DateTime.UtcNow.ToString();
            //    todate = DateTime.UtcNow.ToString();
            //}
            var context = new dbPrachiIndia_PortalEntities();
            var Result = context.Usp_geteBookOrder((txtsearch ?? "").Trim(), fromdate ?? "", todate ?? "").ToList();
            OrderRpt.Reset();
            OrderRpt.LocalReport.EnableExternalImages = true;
            OrderRpt.LocalReport.ReportPath = Server.MapPath("Rpt/ViewOrderRpt.rdlc");
            ReportDataSource rdc = new ReportDataSource("DS_OrderRpt", Result);
            OrderRpt.LocalReport.DataSources.Add(rdc);
            //ReportParameter[] rp1 = new ReportParameter[] {
            //         new ReportParameter("ReportType", rptType),

            //         };
           // OrderRpt.LocalReport.SetParameters(rp1);
            OrderRpt.LocalReport.Refresh();
            OrderRpt.LocalReport.EnableHyperlinks = true;
        }
    }
}