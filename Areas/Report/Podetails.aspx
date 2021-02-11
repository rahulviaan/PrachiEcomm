<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Podetails.aspx.cs" Inherits="PrachiIndia.Portal.Areas.Report.Podetails" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <style>
        .A0d31085cc52b4c8e97f5b9c719287fde140 {
            width:100%!important;
        }
        #P33a6db3626124552b7cc467a816ac8e9_1_oReportDiv, #P33a6db3626124552b7cc467a816ac8e9_1_oReportDiv TABLE {
               width:100%!important;
        }
         table {
        width:100% !important;
        float: left;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1"  runat="server"></asp:ScriptManager>
    <div>
        <rsweb:ReportViewer ID="rptPODetails" runat="server" Font-Names="Verdana" Font-Size="8pt"  Width="100%" PageCountMode="Actual"                 
                ShowFindControls="false"  ShowBackButton="false" KeepSessionAlive="false" AsyncRendering="false"  ShowPageNavigationControls="false"   WaitMessageFont-Names="Verdana" 
                 WaitMessageFont-Size="14pt"  Height="550px"></rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
