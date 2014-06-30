<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.PermitSetDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii</title>
	<link rel="stylesheet" media="screen" href="/admin/include/css/stylesheet.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
</head>
<body>
    <%@ Register tagprefix="hiewiz" tagname="TransferableListBox" src="/admin/controls/TransferableListBox.ascx" %>
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
						Permit Set Detail</h1>
					<div id="adminNav">
						&#171; <a href="/admin/">Administration Home</a><br />
						&#171; <a href="/admin/permit_set/">Permit Set List</a><br />
					</div>
					<p>
						Please note the left listbox lists all available options and the right listbox lists
						selected options. To manage the permits in the current permit set, select desired
						options to move and use the control buttons the options from one
						listbox to the listbox. To select multiple options, hold down the < Ctrl > key while
						selecting multiple records with your mouse.</p>
					<span class="small">Field marked with <span class="required">*</span> are required.</span>
                    <asp:UpdatePanel runat="server">
                    <ContentTemplate>
					<table border="0" class="editform">
						<tr>
							<td>
								Name<span class="required">*</span>:
							</td>
							<td colspan="2">
								<asp:TextBox ID="txtName" runat="server" Columns="90" MaxLength="100" />
								<asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="required" ErrorMessage="Required"
									ControlToValidate="txtName" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtName" Display="Dynamic" ErrorMessage="Name cannot be longer that 100 characters" 
                                ValidationExpression="[\s\S]{0,100}" CssClass="required" ID="revName"/>
							</td>
						</tr>
						<tr>
							<td>
								Description:
							</td>
							<td colspan="2">
								<asp:TextBox ID="txtDescription" runat="server" Columns="88" Rows="2" TextMode="MultiLine" />
							</td>
						</tr>
						<tr>
							<td>
								Permits:
							</td>
							<td>
                                <hiewiz:TransferableListBox ID="tlbPermits" runat="server" ListHeight="400px"
                                    ListWidth="350px" DataTextField="Name" DataValueField="ID" SourceListLabel="Available Permits"
                                    DestinationListLabel="Selected Permits" SourceListToolTip="Available Permits" DestinationListToolTip="Selected Permits">
                                </hiewiz:TransferableListBox>								    
							</td>
						</tr>
						<tr>
							<td class="buttonRow">
								&nbsp;
							</td>
							<td class="buttonRow" colspan="2">
								<div class="evalBtnOutline floatLeft">
									<div class="evalBtn">
										<asp:LinkButton ID="btnSumit" runat="server" Text="Submit" OnClick="btnSumit_Click" />
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
										<asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
											CausesValidation="false" />
									</div>
								</div>
							</td>
						</tr>
					</table>
                    </ContentTemplate>
                    </asp:UpdatePanel>
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
