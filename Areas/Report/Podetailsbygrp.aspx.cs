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
    public partial class Podetailsbygrp : System.Web.UI.Page
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
            var Result = context.USP_Get_PO_DETAILS(Request.QueryString["PO"].ToString(), 1);
            rptPOBygrp.LocalReport.EnableExternalImages = true;
            rptPOBygrp.LocalReport.ReportPath = Server.MapPath("Rpt/Podetailsbygrprpt.rdlc");
            ReportDataSource rdc = new ReportDataSource("DS_POdetailsbygrp", Result);
            rptPOBygrp.LocalReport.DataSources.Add(rdc);
            rptPOBygrp.LocalReport.Refresh();
        }
    }
}