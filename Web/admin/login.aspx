<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii</title>
	<link rel="stylesheet" media="screen" href="/admin/include/css/stylesheet.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
</head>
<body>    
	<div id="pageWrapper">
		<!-- BEGIN GLOBAL -->
		<!-- #include virtual="/admin/include/header.aspx" -->
		<!-- END GLOBAL -->
		<div id="contentWrapper">
			<div id="wideContentWrapper">
				<div class="wideContentWrapper">
					<h1>
						Administration</h1>
					<form id="Form1" runat="server">
					    <asp:ScriptManager ID="ScriptManager1" runat="server" />
					<asp:Label ID="lblError" runat="server" CssClass="required" />
					<table border="0" class="editform" style="width:500px">
						<tr>
							<td>
								Username :
							</td>
							<td>
								<asp:TextBox ID="txtUserName" runat="server" />
							</td>
						</tr>
						<tr>
							<td>
								Password :
							</td>
							<td>
								<asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
							</td>
						</tr>
						<tr>
							<td class="buttonRow">
								&nbsp;
							</td>
							<td class="buttonRow">
								<asp:CheckBox ID="chkPersistLogin" runat="server" Text="Remember my login information" Visible="false" />
							</td>
						</tr>
						<tr>
							<td class="buttonRow">
								&nbsp;
							</td>
							<td class="buttonRow">
								<asp:ImageButton ImageUrl="/admin/images/submit_btn.png" ID="bnSumit" runat="server" OnClick="btnSumit_Click" />
							</td>
						</tr>
					</table>
					</form>
				</div>
			</div>
		</div>
		<!-- BEGIN FOOTER -->
		<!-- #include virtual="/admin/include/footer.aspx" -->
		<!-- END FOOTER -->
	</div>
</body>
</html>
