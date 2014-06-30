<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getImage.aspx.cs" Inherits="HawaiiDBEDT.Web.permitPlan.getImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
</head>
<body>
<%@ Register tagprefix="hiewiz" tagname="PermitChart" src="/permitPlan/permitChart.ascx" %>
<hiewiz:PermitChart ID="permitChart" runat="server" Visible="False"></hiewiz:PermitChart>
</body>
