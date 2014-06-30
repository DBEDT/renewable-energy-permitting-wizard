<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HawaiiDBEDT.Web.admin._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii</title>
	<link rel="stylesheet" media="screen" href="/admin/include/css/stylesheet.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
</head>
<body>
	<form runat="server">
	   <asp:ScriptManager ID="ScriptManager1" runat="server" />
	<div id="pageWrapper">
		<!-- BEGIN GLOBAL -->
		<!-- #include virtual="/admin/include/header.aspx" -->
		<!-- END GLOBAL -->
		<div id="contentWrapper">
			<div id="wideContentWrapper">
				<div class="wideContentWrapper">
					<h1>
						Administration</h1>
					<div class="floatRight">
						<asp:LinkButton runat="server" ID="lbLogout" Text="Logout" OnClick="lbLogout_Click" /></div>
					<ul class="listItem">
						<li><a href="permit/">Permits</a> </li>
						<li><a href="permit_set/">Permit Sets</a></li>
						<li><a href="pre_eval/">Pre-Evaluation Questions</a></li>
						<li><a href="question/">Questions</a></li>
						<li><a href="question_set/">Question Sets</a>  (<a href="/admin/question_set/reorder.aspx">Reorder</a>)</li>
						<li><a href="technology/">Technology Permit Sets</a></li>
						<li><a href="location/">Location Permit Sets</a></li>
						<li><a href="department/">Departments</a></li>
					</ul>
					<br />
					<ul class="listItem">
						<li><a href="user/">Users</a></li>
						<li><a href="evaluation/">Completed Evaluations</a></li></ul>
					<p>
						&nbsp;</p>
				</div>
			</div>
		</div>
		<!-- BEGIN FOOTER -->
		<!-- #include virtual="/admin/include/footer.aspx" -->
		<!-- END FOOTER -->
	</div>
	</form>
</body>
</html>
