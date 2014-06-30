<%@ Register TagPrefix="HawaiiDBEDT" TagName="PermitSetControl" Src="~/include/controls/permit_set.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="permit_set.aspx.cs" Inherits="HawaiiDBEDT.Web.evaluate.permit_set" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Hawaii Renewable Energy - Evaluate</title>
	<script type="text/javascript" src="/include/js/javascript.js"></script>
	<script type="text/javascript" src="/include/js/jquery-ui-1.8.6.custom.min.js"></script>
	<script type="text/javascript" src="/include/js/jquery.qtip.pack.js"></script>
	<link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
	<link rel="stylesheet" media="screen" href="/include/css/jquery-ui-1.8.6.custom.css"
		type="text/css" />
	<link rel="stylesheet" media="screen" href="/include/css/jquery.qtip.css" type="text/css" />
	<link rel="stylesheet" media="print" href="/include/css/print.css" type="text/css" />
    <script type="text/javascript" >
        showSessionModal = '<% = IsLoggedIn %>';
    </script>
</head>
<body class="evalTool">
	<form id="form1" runat="server">
	<asp:ScriptManager ID="ScriptManager" runat="server" />
	<div id="pageWrapper">
		<!-- BEGIN NAV -->
		<!-- #include virtual="~/include/global.aspx" -->
		<!-- END NAV -->
		<!-- BEGIN HEADER -->
		<!-- #include virtual="~/include/header.aspx" -->
		<!-- END HEADER -->
		<div id="contentWrapper">
			<div id="wideContentWrapper">
				<div class="wideContentWrapper">
					<h1>
						Permit Sets</h1>
					<div id="permitWrapper">
						<div class="permits">
							<HawaiiDBEDT:PermitSetControl ID="PermitSetControl1" runat="server" />
						</div>
					</div>
				</div>
			</div>
			<!-- BEGIN FOOTER -->
			<!-- #include virtual="~/include/footer.aspx" -->
			<!-- END FOOTER -->
		</div>
	</div>
	</form>
</body>
</html>
