<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reorder.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.QuestionSetOrder"
	MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii : Administration
	</title>
	<link rel="stylesheet" href="/admin/include/css/stylesheet.css" type="text/css" />
	<link rel="stylesheet" href="/include/controls/radcontrols/css/grid.css" type="text/css" />
	<link rel="stylesheet" href="/include/controls/radcontrols/css/combobox.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
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
						Reorder Question Sets</h1>
					<div id="adminNav">
						&#171; <a href="/admin/">Back to Administration Home</a><br />
						&#171; <a href="/admin/question_set/">Question Set List</a><br />
					</div>
					<p class="required">
					    <asp:Label runat="server" ID="lblConfirmation" Text=""></asp:Label>
					</p>
					<p>
						A question set provides grouping of questions to be displayed on a single page.
						To re-order the question sets, select a row to desired position.
						Click on the question set name or the edit icon to to view details.</p>
					<p>
						Use the filter below to narrow the list of Question Sets. Sort the list by clicking
						on a column title. Click on the question set name or the edit icon to view or edit
						question set details.</p>
					<p>
						Please filter by desired Technology <u>and</u> Location and drag-n-drop the question
						sets in desired order.</p>
					<table width="100%">
						<tr>
							<td>
								Filter by Technology:
								<asp:DropDownList ID="rcbTechnology" runat="server" Title="Select Technology"
									DataTextField="Name" DataValueField="ID" Skin="Custom" EnableEmbeddedSkins="false"
									Width="200px" />
								<br />
								<br />
								Filter by Location:
								<asp:DropDownList ID="rcbLocation" runat="server" Title="Select Location" DataTextField="Name"
									DataValueField="ID" Skin="Custom" EnableEmbeddedSkins="false" />
							</td>
							<td valign="top" width="30%">
								<div class="evalBtnOutline floatLeft">
									<div class="evalBtn">
										<asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
									</div>
								</div>
								<a href="default.aspx">Back to Question Sets</a>
							</td>
						</tr>
					</table>
					<br />
					<asp:DataGrid ID="gridRecords" runat="server" GridLines="None" 
						AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" PageSize="20"
						Skin="Custom" EnableEmbeddedSkins="false" OnPageIndexChanged="gridRecords_pageChange"
                        CssClass="dataGrid" OnItemCommand="GridRecondsItemCommand">
                        <HeaderStyle Wrap="false" CssClass="headerRow" />
                        <ItemStyle VerticalAlign="Top" CssClass="dataGridEvenRow" />
                        <AlternatingItemStyle VerticalAlign="Top" CssClass="dataGridOddRow" />
                        <PagerStyle Mode="NumericPages" HorizontalAlign="Left" Position="TopAndBottom" CssClass="pagerRow">
                        </PagerStyle>
						<Columns>
							<asp:HyperLinkColumn DataNavigateUrlFormatString="edit.aspx?id={0}" DataNavigateUrlField="ID"
								DataTextField="Name" HeaderText="Name" />									    
							<asp:BoundColumn DataField="DateModified" HeaderText="Last Modified" 
								DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" />
                            <asp:ButtonColumn Text="Move Up" CommandName="MoveUp"  />
                            <asp:ButtonColumn Text="Move Down" CommandName="MoveDown"  />
						</Columns>
					</asp:DataGrid>
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
