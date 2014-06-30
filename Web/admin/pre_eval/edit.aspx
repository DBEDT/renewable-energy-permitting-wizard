<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.PreEvaluationQuestionDetail" %>
<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii</title>
	<link rel="stylesheet" media="screen" href="/admin/include/css/stylesheet.css" type="text/css" />
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
						Pre-Evaluation Question Detail</h1>
					<div id="adminNav">
						&#171; <a href="/admin/">Administration Home</a><br />
						&#171; <a href="/admin/pre_eval/">Pre-Evaluation Question List</a><br />
					</div>
					<p>
						Please update question text and tool tip below.</p>
					<span class="small">Field marked with <span class="required">*</span> are required.</span>
					<table border="0" class="editform">
						<tr>
							<td>
								Question Type:
							</td>
							<td>
								<asp:Label ID="lblLookupClassName" runat="server" />
							</td>
						</tr>
						<tr>
							<td>
								Question Text<span class="required">*</span>:
							</td>
							<td>
								<asp:TextBox ID="txtName" runat="server" Columns="90" MaxLength="500" />
								<asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="required" ErrorMessage="Required"
									ControlToValidate="txtName" />
							</td>
						</tr>
						<tr>
							<td>
								Tool Tip:<br />
								<span class="small">(5000 character limit)</span>
							</td>
							<td>
								<asp:Label ID="lblError" runat="server" CssClass="required" />
								<asp:TextBox TextMode="multiline" ID="reDescription" runat="server" 
									  Height="150px" />
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
									<div class="resetBtn">
										<asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
											CausesValidation="false" />
									</div>
								</div>
							</td>
						</tr>
					</table>

					
					<asp:Panel ID="pnlResponses" runat="server" Visible="false">
					<h3>
						Responses</h3>
						
					<p class="required"><%= ConfirmationText %></p> 
                    <div class="commandRow">
						<a href="response.aspx?qid=<%= questionID %>">
							<img src="/include/controls/radcontrols/grid/AddRecord.gif" alt="Add New Response" /></a>
						<a href="response.aspx?qid=<%= questionID %>">Add New Response</a>
					</div>
					<asp:DataGrid ID="gridRecords" runat="server" GridLines="None" 
						AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" PageSize="20" 
						Skin="Custom" EnableEmbeddedSkins="false" OnSortCommand="gridRecords_sort" OnPageIndexChanged="gridRecords_pageChange" CssClass="dataGrid">
							<HeaderStyle Wrap="false" CssClass="headerRow"/>
							<ItemStyle VerticalAlign="Top" CssClass="dataGridEvenRow" />
							<AlternatingItemStyle VerticalAlign="Top" CssClass="dataGridOddRow"/>
                            <PagerStyle Mode="NumericPages" HorizontalAlign="Left" Position="TopAndBottom" CssClass="pagerRow" ></PagerStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Name" SortExpression="Name" >
									<ItemTemplate>
										<a href='response.aspx?id=<%# DataBinder.Eval(Container.DataItem, "ID")%>&qid=<%# DataBinder.Eval(Container.DataItem, "PreEvaluationQuestionID") %>'>
											<%# DataBinder.Eval(Container.DataItem, "Name")%></a>
									</ItemTemplate>
								</asp:TemplateColumn>		
                                <asp:TemplateColumn HeaderText="Last Modified" SortExpression="DateModified">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateModifiedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateModified")).ToString("MM/dd/yyyy")%>' />
                                    </ItemTemplate>
                                </asp:TemplateColumn>                                 					    
								<asp:TemplateColumn HeaderText="Edit" >
									<ItemTemplate>
										<a href='response.aspx?id=<%# DataBinder.Eval(Container.DataItem, "ID")%>&qid=<%# DataBinder.Eval(Container.DataItem, "PreEvaluationQuestionID") %>'>
											<img src="/admin/images/edit.gif" alt="Edit" /></a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<a href='edit.aspx?action=delete&id=<%# DataBinder.Eval(Container.DataItem, "PreEvaluationQuestionID") %>&rid=<%# DataBinder.Eval(Container.DataItem, "ID")%>' onclick="return confirm('Are you sure you wish to delete this item? This action cannot be undone.');">
											<img src="/admin/images/delete.gif" alt="Delete" /></a>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
					</asp:DataGrid>
					</asp:Panel>
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
