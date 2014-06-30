<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.QuestionDetail" %>

<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii</title>
    <link rel="stylesheet" href="/admin/include/css/stylesheet.css" type="text/css" />
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
                        Question Detail</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a><br />
                        &#171; <a href="/admin/question/">Question List</a><br />
                    </div>
                    <p class="required">
                        <%= ConfirmationText %></p>
                    <p>
                        Update question text below. Click "Save" to submit your changes and return to this
                        current page. Click "Submit" to submit your changes and return to the question list
                        page.
                    </p>
                    <span class="small">Field marked with <span class="required">*</span> are required.</span>
                    <table border="0" class="editform">
                        <tr>
                            <td width="17%">
                                Question Text <span class="required">*</span>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" Columns="90" MaxLength="500" />
                                <asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="required" ErrorMessage="Required"
                                    ControlToValidate="txtName" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtName" Display="Dynamic" ErrorMessage="Question cannot be longer that 500 characters" 
                                ValidationExpression="[\s\S]{0,500}" CssClass="required" ID="revName"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Type <span class="required">*</span>:
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="rcbQuestionType" runat="server" Title="Select Question Type"
                                    DataTextField="Name" DataValueField="ID" Skin="Custom" EnableEmbeddedSkins="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tooltip:
                            </td>
                            <td>
                                <asp:TextBox TextMode="multiline" ID="reDescription" runat="server" Height="150px" MaxLength="5000" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="reDescription" Display="Dynamic" ErrorMessage="Tooltip cannot be longer that 5000 characters" 
                                ValidationExpression="[\s\S]{0,5000}" CssClass="required" ID="revDescription"/>
                            </td>
                        </tr>
                    </table>
                    <table width="900" border="0" class="editform">
                        <tr>
                            <td class="buttonRow" width="17%" valign="top">
                                <asp:HyperLink ID="hlPrevious" runat="server" Text="&#171; Previous" />
                            </td>
                            <td class="buttonRow">
                                <div class="evalBtnOutline floatLeft">
                                    <div class="evalBtn">
                                        <asp:LinkButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                                <div class="evalBtnOutline floatLeft" id="divSubmit" runat="server">
                                    <div class="evalBtn">
                                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                    </div>
                                </div>
                                <div class="evalBtnOutline floatLeft" id="divSubmitAnother" runat="server">
                                    <div class="evalBtn">
                                        <asp:LinkButton ID="btnSubmitAnother" runat="server" Text="Submit and Add Another"
                                            OnClick="btnSubmitAnother_Click" />
                                    </div>
                                </div>
                                <div class="evalBtnOutline floatLeft" id="divSubmitNext" runat="server">
                                    <div class="evalBtn">
                                        <asp:LinkButton ID="LinkButton1" runat="server" Text="Submit and Next" OnClick="btnSubmitNext_Click" />
                                    </div>
                                </div>
                                <div class="evalBtnOutline floatLeft">
                                    <div class="resetBtn">
                                        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                            CausesValidation="false" />
                                    </div>
                                </div>
                            </td>
                            <td class="buttonRow" align="right" valign="top">
                                <asp:HyperLink ID="hlNext" runat="server" Text="Next &#187;" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlResponses" runat="server" Visible="false">
                        <h3>
                            Responses</h3>
                        <div class="commandRow">
                            <a href="response.aspx?qid=<%= questionID %>">
                                <img src="/include/controls/radcontrols/grid/AddRecord.gif" alt="Add New Response" /></a>
                            <a href="response.aspx?qid=<%= questionID %>">Add New Response</a>
                        </div>
                        <asp:DataGrid ID="gridRecords" runat="server" GridLines="None" AllowPaging="true"
                            AllowSorting="true" AutoGenerateColumns="false" PageSize="20" Skin="Custom" EnableEmbeddedSkins="false"
                            OnSortCommand="gridRecords_sort" OnPageIndexChanged="gridRecords_pageChange"
                            CssClass="dataGrid">
                            <HeaderStyle Wrap="false" CssClass="headerRow" />
                            <ItemStyle VerticalAlign="Top" CssClass="dataGridEvenRow" />
                            <AlternatingItemStyle VerticalAlign="Top" CssClass="dataGridOddRow" />
                            <PagerStyle Mode="NumericPages" HorizontalAlign="Left" Position="TopAndBottom" CssClass="pagerRow">
                            </PagerStyle>
                            <Columns>
                                <asp:TemplateColumn HeaderText="Name" SortExpression="Name">
                                    <ItemTemplate>
                                        <a href='response.aspx?id=<%# DataBinder.Eval(Container.DataItem, "ID")%>&qid=<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'>
                                            <%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Last Modified" SortExpression="DateModified">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateModifiedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateModified")).ToString("MM/dd/yyyy")%>' />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Edit">
                                    <ItemTemplate>
                                        <a href='response.aspx?id=<%# DataBinder.Eval(Container.DataItem, "ID")%>&qid=<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'>
                                            <img src="/admin/images/edit.gif" alt="Edit" /></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a href='edit.aspx?action=delete&id=<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>&rid=<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                            onclick="return confirm('Are you sure you wish to delete this item? This action cannot be undone.');">
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
