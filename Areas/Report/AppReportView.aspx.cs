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
    public partial class AppReportView : System.Web.UI.Page
    {
        string txtsearch = "";
        string fromdate="" ;
        string todate="";
        string rptType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RenderReport();
            }
        }
        private void RenderReport()
        {
            rptType= Request.QueryString["rptType"];
            txtsearch = Request.QueryString["search"];
            fromdate = Request.QueryString["fromdate"];
            todate = Request.QueryString["todate"];
            //if (String.IsNullOrEmpty(txtsearch) && String.IsNullOrEmpty(fromdate) && String.IsNullOrEmpty(todate))
            //{
            //    fromdate = DateTime.UtcNow.ToString();
            //    todate = DateTime.UtcNow.ToString();
            //}
            var context = new dbPrachiIndia_PortalEntities();
            var Result = context.usp_getSalesReport((txtsearch??"").Trim(), fromdate??"", todate??"", rptType).ToList();
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Rpt/ViewReport.rdlc");
            ReportDataSource rdc = new ReportDataSource("DS_SalesMarketing", Result);
            ReportViewer1.LocalReport.DataSources.Add(rdc);
            ReportParameter[] rp1 = new ReportParameter[] {
                 new ReportParameter("txtSearch", txtsearch),
                  new ReportParameter("toDate", fromdate),
                   new ReportParameter("frmDate", todate),
                     new ReportParameter("ReportType", rptType),
                                      
                     };
            ReportViewer1.LocalReport.SetParameters(rp1);
            ReportViewer1.LocalReport.Refresh();
        }
    }
}