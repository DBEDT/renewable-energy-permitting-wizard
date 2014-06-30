<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermitImage.aspx.cs" Inherits="HawaiiDBEDT.Web.permitPlan.PermitImage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <%@ Register tagprefix="hiewiz" tagname="PermitChart" src="/permitPlan/permitChart.ascx" %>    
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager runat="server">
        </asp:ScriptManager>
        <hiewiz:PermitChart ID="permitChart" runat="server" Visible="False"></hiewiz:PermitChart>
    </div>
    </form>
</body>
</html>
