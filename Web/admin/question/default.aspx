<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.QuestionList" %>
<%@ Register tagPrefix="cc1" namespace="HawaiiDBEDT.Web.admin.controls" assembly="HawaiiDBEDT.Web" %>
<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link href="/admin/include/css/fullgridpager.css" rel="stylesheet" type="text/css" />
    <title>Renewable Energy Facility Permits in the State of Hawaii : Administration
    </title>
    <link rel="stylesheet" href="/admin/include/css/stylesheet.css" type="text/css" />
    <link href="/admin/include/css/fullgridpager.css" rel="stylesheet" type="text/css" />
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
                        Questions</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a>
                    </div>
                    <p class="required">
                        <%= ConfirmationText %></p>
                    <p>
                        Use the filters below to narrow the list of questions by technology. Sort the list
                        by clicking on a column title. Click on the question name or the edit icon to view
                        or edit question details.
                    </p>
                    <table>
                        <tr>
                            <td>
                                Filter by Question Type:
                                <asp:DropDownList ID="rcbQuestionType" runat="server" Title="Filter By Question Type"
                                    DataTextField="Name" DataValueField="ID" Width="150px" Skin="Custom" EnableEmbeddedSkins="false" />
                            </td>
                            <td>
                                <div class="evalBtnOutline floatLeft">
                                    <div class="evalBtn">
                                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                    </div>
                                </div>
                                <div class="evalBtnOutline floatLeft">
                                    <div class="resetBtn">
                                        <asp:LinkButton ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div class="commandRow">
                        <a href="edit.aspx">
                            <img src="/include/controls/radcontrols/grid/AddRecord.gif" alt="Add New Question" /></a>
                        <a href="edit.aspx">Add New Question</a>
                    </div>
                    <cc1:TopPagerGridView ID="gridRecords" runat="server" GridLines="None" AllowPaging="true"
                        AllowSorting="true" AutoGenerateColumns="false" PageSize="20" Skin="Custom" EnableEmbeddedSkins="false"
                        OnSorting="HandleSorting" OnPageIndexChanging="HandlePageChange" 
                        CssClass="dataGrid" OnDataBound="HandleDataBound">
                        <HeaderStyle Wrap="false" CssClass="headerRow" />
                        <PagerSettings Position="Top"></PagerSettings>
                        <RowStyle VerticalAlign="Top" CssClass="dataGridEvenRow" />
                        <AlternatingRowStyle VerticalAlign="Top" CssClass="dataGridOddRow" />
                        <PagerStyle CssClass="pagerOuterTable"></PagerStyle>
                         <PagerTemplate>
                            <table ID="pagerOuterTable" class="pagerOuterTable" runat="server">
                                <tr>
                                    <td>
                                        <table ID="pagerInnerTable" runat="server">
                                            <tr>
                                                <td class="pageFirstLast">
                                                    <img src="/admin/images/firstpage.gif" align="absmiddle" />&nbsp;<asp:LinkButton ID="lnkFirstPage" CssClass="pagerLink" runat="server" CommandName="Page" CommandArgument="First">First</asp:LinkButton>
                                                </td>
                                                <td class="pagePrevNextNumber">
                                                    <img src="/admin/images/prevpage.gif" align="absmiddle"/>&nbsp;<asp:LinkButton ID="imgPrevPage" CssClass="pagerLink" runat="server"  CommandName="Page" CommandArgument="Prev">Previous</asp:LinkButton>
                                                </td>
                                    
                                                <td class="pagePrevNextNumber">
                                                    <asp:LinkButton ID="imgNextPage" CssClass="pagerLink" runat="server" CommandName="Page" CommandArgument="Next">Next</asp:LinkButton>&nbsp;<img src="/admin/images/nextpage.gif" align="absmiddle"/>
                                                </td>
                                                <td class="pageFirstLast">
                                                    <asp:LinkButton ID="lnkLastPage" CssClass="pagerLink" CommandName="Page" CommandArgument="Last" runat="server">Last</asp:LinkButton>&nbsp;<img src="/admin/images/lastpage.gif" align="absmiddle" />
                                                </td>       
                                            </tr>
                                        </table>                                                       
                                    </td>
                                </tr>
                            </table>            
                        </PagerTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Question" SortExpression="Name">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" Text='<%#Eval("Name") %>' NavigateUrl='<%#string.Format("edit.aspx?id={0}", Eval("ID")) %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" SortExpression="QuestionType.Name">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "QuestionType.Name")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Modified" SortExpression="DateModified">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateModifiedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateModified")).ToString("MM/dd/yyyy")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href='edit.aspx?id=<%# DataBinder.Eval(Container.DataItem, "ID")%>'>
                                        <img src="/admin/images/edit.gif" alt="Edit" /></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href='default.aspx?action=delete&id=<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                        onclick="return confirm('Are you sure you wish to delete this item? This action cannot be undone.\n\nIf you select \'OK\', please note all responses for the question and user input for this question will also be deleted. The question will also be removed if it was specified as a nested question.');">
                                        <img src="/admin/images/delete.gif" alt="Delete" /></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </cc1:TopPagerGridView>
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
