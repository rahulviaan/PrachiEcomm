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
    public partial class Podetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RenderReport();

            }

        }

        private void RenderReport()
        {
            var context = new dbPrachiIndia_PortalEntities();
            var Result = context.USP_Get_PO_DETAILS(Request.QueryString["PONUMBER"].ToString(),0);
            rptPODetails.LocalReport.EnableExternalImages = true;
            rptPODetails.LocalReport.ReportPath = Server.MapPath("Rpt/POdetailsrpt.rdlc");
            ReportDataSource rdc = new ReportDataSource("DS_rptPOdetails", Result);
            rptPODetails.LocalReport.DataSources.Add(rdc);
            rptPODetails.LocalReport.Refresh();
        }
    }
}