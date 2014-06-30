<%@ Register TagPrefix="dbwc" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.DynamicControlsPlaceholder" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="response.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.ResponseDetail" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii</title>
	<link rel="stylesheet" media="screen" href="/admin/include/css/stylesheet.css" type="text/css" />	
	<link rel="stylesheet" href="/include/controls/radcontrols/css/listbox.css" type="text/css" />
	<link rel="stylesheet" href="/include/controls/radcontrols/css/combobox.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
	<script language="javascript" type="text/javascript">
		function deleteField(id) {
			document.getElementById("permitSetIDToDelete").value = id;
		}
		function checkAllLocationFields(idPrefix) {
			for (var i = 0; i < document.forms[0].elements.length; i++) {
				var fldObj = document.forms[0].elements[i];
				if (fldObj.type == 'checkbox' && fldObj.name.indexOf(idPrefix) >= 0) {
					fldObj.checked = true;
				}
			}
		}
		function uncheckAllfield(id) {
			document.getElementById(id).checked = false;	
		}
	</script>
    <script src="/scripts/tinymce/tinymce.js"></script>
    <script language="javascript" type="text/javascript" src="/scripts/tinymce_setup.js"></script>
</head>
<body>
	<form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />	    
        <input type="hidden" name="permitSetIDToDelete" id="permitSetIDToDelete" value="" runat="server" />	
	<div id="pageWrapper">
		<!-- BEGIN GLOBAL -->
		<!-- #include virtual="/admin/include/header.aspx" -->
		<!-- END GLOBAL -->
		<div id="contentWrapper">
			<div id="wideContentWrapper">
				<div class="wideContentWrapper">
					<h1>
						Response Detail</h1>					
					<div id="adminNav">
						&#171; <a href="/admin/">Administration Home</a><br />
						&#171; <a href="/admin/question/">Question List</a>
					</div> 
					<span class="small">Field marked with <span class="required">*</span> are required.</span>
					<table border="0" class="editform">
						<tr>
							<td>Question Text:</td>
							<td><asp:Label ID="lblQuestion" runat="server" /></td>
						</tr>
						<tr>
							<td>
								Response Text<span class="required">*</span>:
							</td>
							<td>
								<asp:TextBox ID="txtName" runat="server" Columns="90" MaxLength="250" /> 
								<asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="required" ErrorMessage="Required" ControlToValidate="txtName" />
							</td>
						</tr>
						<tr>
							<td>
								Tooltip:
							</td>
							<td>
								<asp:TextBox TextMode="multiline" ID="reDescription" runat="server" 
                                 Height="150px" />
							</td>
						</tr>
						<tr>
							<td>End Point:</td>
							<td>
								<asp:RadioButtonList ID="rbEndPoint" runat="server" 
									RepeatDirection="Horizontal" CssClass="noStyle" 
									OnSelectedIndexChanged="rbEndPoint_SelectedIndexChanged" AutoPostBack="True">
									<asp:ListItem value="true" Text="Yes" />
									<asp:ListItem value="false" Text="No" Selected="True" />
								</asp:RadioButtonList>
								<span class="small">Select 'Yes" if evaluation cannot continue when this response is selected.</span>
							</td>
						</tr>
						<tr>
							<td>
								End Point Message:
							</td>
							<td>
								<asp:TextBox TextMode="multiline" ID="reEndPoint" runat="server"  Height="150px" />
							</td>
						</tr>
						<tr runat="server" id="trPermits">
							<td>
								Permit Sets:
							</td>
							<td>
								<asp:DropDownList ID="rcbPermitSet" runat="server" Width="300px" Title="Select a permit set" DataTextField="Name" 
								DataValueField="ID" Skin="Custom" EnableEmbeddedSkins="false" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="rcbPermitSet_IndexChanged"/>
								<asp:Panel ID="pnlPermitSet" runat="server" Visible="false">
									<table width="100%">
										<tr>
											<td><b>Permit Set</b></td>
											<td colspan="2"><b>Applicable Location</b></td>
										</tr>
									 <dbwc:DynamicControlsPlaceholder ID="MyDCP" runat="server" ControlsWithoutIDs="Persist">
									</dbwc:DynamicControlsPlaceholder>
									</table>
								</asp:Panel>	
							</td>
						</tr>
 						<tr runat="server" id="trSubQuestion">
							<td>
								Sub-Question:
							</td>
							<td>
								<asp:DropDownList ID="rcbSubQuestion" runat="server" Title="Select a question"
									DataTextField="Name" DataValueField="ID" Width="400px"/>
								<span class="small">Please note you can only nest one question level.</span>
							</td>
						</tr>

						<tr>
							<td class="buttonRow">
								&nbsp;
							</td>
							<td class="buttonRow">
								<div class="evalBtnOutline floatLeft">
									<div class="evalBtn">
										<asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
									</div>
								</div>
								<div class="evalBtnOutline floatLeft">
									<div class="evalBtn">
										<asp:LinkButton ID="btnSubmitAnother" runat="server" Text="Submit and Add Another"
											OnClick="btnSubmitAnother_Click" />
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
