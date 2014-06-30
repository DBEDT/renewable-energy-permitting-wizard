<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="default.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.LocationList" %>
<%@ Register tagPrefix="cc1" namespace="HawaiiDBEDT.Web.admin.controls" assembly="HawaiiDBEDT.Web" %>
<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>
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
                        Locations</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a><br />
                    </div>
                    <p class="required">
                        <%= ConfirmationText %></p>
                    <p>
                        Click on the location name or the edit icon to view permit sets for the location.
                    </p>
                    <cc1:TopPagerGridView ID="gridRecords" runat="server" GridLines="None" AllowPaging="true"
                        AllowSorting="true" AutoGenerateColumns="false" PageSize="20" Skin="Custom" EnableEmbeddedSkins="false"
                        OnSorting="HandleSorting" OnPageIndexChanging="HandlePaging"
                        CssClass="dataGrid">
                        <HeaderStyle Wrap="false" CssClass="headerRow" HorizontalAlign="Left" />
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
                            <asp:TemplateField HeaderText="Location" SortExpression="Name">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" Text='<%#Eval("Name") %>' NavigateUrl='<%#string.Format("edit.aspx?id={0}", Eval("ID")) %>'></asp:HyperLink>
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
