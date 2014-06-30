<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.DepartmentDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii</title>
	<link rel="stylesheet" href="/admin/include/css/stylesheet.css" type="text/css" /> 
	<link rel="stylesheet" href="/include/controls/radcontrols/css/grid.css" type="text/css" />
	<link rel="stylesheet" href="/include/controls/radcontrols/css/listbox.css" type="text/css" />
	<link rel="stylesheet" href="/include/controls/radcontrols/css/combobox.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
    <script src="/scripts/tinymce/tinymce.js"></script>
    <script language="javascript" type="text/javascript" src="/scripts/tinymce_setup.js"></script>
</head>
<body>
	<form id="Form1" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server" />
	<div id="pageWrapper">
		<!-- BEGIN GLOBAL -->
		<!-- #include virtual="/admin/include/header.aspx" -->
		<!-- END GLOBAL -->
		<div id="contentWrapper">
			<div id="wideContentWrapper">
				<div class="wideContentWrapper">
					<h1>
						Department Detail</h1>
					<div id="adminNav">
						&#171; <a href="/admin/">Administration Home</a><br />
						&#171; <a href="/admin/department/">Department List</a><br />
					</div>
					<p class="required"><%= ConfirmationText %></p> 
					<p>
						Update department name and contact info below. 
						Click "Submit" to submit your changes and return to the department list page.
					</p>
					<span class="small">Field marked with <span class="required">*</span> are required.</span>
					<table border="0" class="editform">
						<tr>
							<td width="17%">
								Department Name <span class="required">*</span>:
							</td>
							<td>
								<asp:TextBox ID="txtName" runat="server" Columns="90" MaxLength="250" />
								<asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="required" ErrorMessage="Required"
									ControlToValidate="txtName" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtName" Display="Dynamic" ErrorMessage="Department name cannot be longer that 250 characters" 
                                ValidationExpression="[\s\S]{0,250}" CssClass="required" />
							</td>
						</tr>
						<tr>
							<td>
								Contact Information:
							</td>
							<td>
								<asp:TextBox TextMode="multiline" ID="reDescription" runat="server"  
                                Height="250px" MaxLength="2000" />
                               <asp:RegularExpressionValidator runat="server" ControlToValidate="reDescription" Display="Dynamic" ErrorMessage="Contact Information cannot be longer that 2000 characters" 
                                ValidationExpression="[\s\S]{0,2000}" CssClass="required" ID="revDescription"/>
							</td>
						</tr>
						<tr>
							<td class="buttonRow">

							</td>
							<td class="buttonRow">
								<div class="evalBtnOutline floatLeft" id="divSubmit" runat="server">
									<div class="evalBtn">
										<asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
									</div>
								</div>								
								<div class="evalBtnOutline floatLeft">
									<div class="resetBtn">
										<asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
									</div>
								</div>
							</td>
						</tr>
					</table>
				</div>
				<!-- BEGIN FOOTER -->
				<!-- #include virtual="/admin/include/footer.aspx" -->
				<!-- END FOOTER -->
			</div>
		</div>
	</div>
	</form>
</body>
</html>
