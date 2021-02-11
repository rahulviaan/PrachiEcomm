<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppReportView.aspx.cs"  Inherits="PrachiIndia.Portal.Areas.Report.AppReportView" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> 
    <style type="text/css">
a[title=Word] {

display: none !important;
}

</style> 
      
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"  Width="100%" PageCountMode="Actual"                 
                ShowFindControls="false"  ShowBackButton="false" ShowPageNavigationControls="false"   WaitMessageFont-Names="Verdana" 
                 WaitMessageFont-Size="14pt"  Height="550px"></rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
