<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="question_set.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.QuestionSetDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
	<title>Renewable Energy Facility Questions in the State of Hawaii</title>
	<link rel="stylesheet" media="screen" href="/admin/include/stylesheet.css" type="text/css" /> 
	<link rel="stylesheet" href="/include/controls/radcontrols/combobox.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="/admin/include/javascript.js"></script>
</head>
<body>
	<form id="Form1" runat="server">
	<telerik:RadScriptManager ID="scriptManager1" runat="server">
	</telerik:RadScriptManager>
	<div id="pageWrapper">
		<!-- BEGIN GLOBAL -->
		<!-- #include virtual="~/admin/include/header.aspx" -->
		<!-- END GLOBAL -->
		<div id="contentWrapper">
			<div id="wideContentWrapper">
				<div class="wideContentWrapper">
					<h1>
						Question Set Detail</h1>					
					<div id="adminNav">
						&#171; <a href="/admin/">Administration Home</a><br />
						&#171; <a href="/admin/technology/">Technology List</a><br />
					</div>					
					<p>
						Questions in a question set will be displayed on a single page.<br />
						To manage the questions in the question set, select desired questions in the left listbox and use
						the control buttons to move or drag-n-drop the options from one listbox to the listbox. To select
						multiple options, hold down the < Ctrl > key while selecting multiple records with
						your mouse.</p> 
					<span class="small">Field marked with <span class="required">*</span> are required.</span>
					<table border="0" class="editform"> 
						<tr>
							<td>
								Technology:
							</td>
							<td colspan="2">
								<asp:Label ID="lblTechnology" runat="server" />
							</td>
						</tr>
						<tr>
							<td>
								Name<span class="required">*</span>:
							</td>
							<td colspan="2">
								<asp:TextBox ID="txtName" runat="server" Columns="90" MaxLength="250" /> 
								<asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="required" ErrorMessage="Required" ControlToValidate="txtName" />
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
								Questions:
							</td>
							<td>
								<span class="small">Available Questions (Excludes questions<br />used in other question sets for the current technology)</span>
								<telerik:RadListBox ID="lbQuestions" runat="server" Height="400px" Width="350px" AllowTransfer="true"
									TransferMode="Move" TransferToID="lbQuestionsDestination" DataTextField="Name"
									DataValueField="ID" ToolTip="Available Questions" SelectionMode="Multiple"  />
							</td>
							<td>
								<span class="small">Selected Questions<br />(Specify order as desired).</span>
								<telerik:RadListBox runat="server" ID="lbQuestionsDestination" Height="400px" Width="350px"
									DataTextField="Name" DataValueField="ID" ToolTip="Selected Questions" SelectionMode="Multiple" AllowReorder="true"  />
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
										<asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
									</div>
								</div>
							</td>
						</tr>
					</table>
				</div>
				<!-- BEGIN FOOTER -->
				<!-- #include virtual="~/admin/include/footer.aspx" -->
				<!-- END FOOTER -->
			</div>
		</div>
	</div>
	</form>
</body>
</html>
