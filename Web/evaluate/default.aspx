<%@ Register TagPrefix="HawaiiDBEDT" TagName="Evaluation" Src="~/include/controls/evaluation.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HawaiiDBEDT.Web.Evaluate.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Hawaii Renewable Energy - Evaluate</title>
	<script type="text/javascript" src="/include/js/javascript.js"></script>
	<script type="text/javascript" src="/include/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/include/js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="/include/js/jquery.qtip.pack.js"></script>
    <script type="text/javascript" src="/include/js/downloadPlugin.js"></script>
	<link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
	<link rel="stylesheet" media="screen" href="/include/css/jquery-ui.css" type="text/css" />
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
						Renewable Energy Permitting Wizard</h1>
					<HawaiiDBEDT:Evaluation ID="EvaluationControl1" runat="server" />
				</div>
			</div>
			<!-- BEGIN FOOTER -->
			<!-- #include virtual="~/include/footer.aspx" -->
			<!-- END FOOTER -->
		</div>
	</div>
	<script type="text/javascript">
		$(function () {
			$(".inactive").button("disable");
		});
		
		// Create the tooltips only on document load
		$(document).ready(function () {
			$('.toolTip > a').each(function () {
				$(this).qtip({
					content: {
						title: {
							text: true,
							button: 'Close'
						},
						text: $(this).parent().next('.toolTipContent') //
					},
					position: {
						viewport: $(window)
					},
					hide: {
						fixed: true, // Make it fixed so it can be hovered over
						delay: 200,
						when: { event: 'click' }
					}
				});
			});
		});
	</script>
	</form>
</body>
</html>
