<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.UserList" %>
<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>
<%@ Register tagPrefix="cc1" namespace="HawaiiDBEDT.Web.admin.controls" assembly="HawaiiDBEDT.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
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
                        Registered Users</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a>
                    </div>
                    <asp:Panel ID="pnlConfirm" runat="server" Visible="false">
                        <div class="confirmation">
                            <img src="/images/confirm.png" width="16" height="16" alt="Confirm" />
                            A user has been sent a new password.
                        </div>
                    </asp:Panel>
                    <table>
                        <tr>
                            <td>
                                Filters:
                                <asp:DropDownList ID="rcbTechnology" runat="server" Title="Filter By Technology"
                                    DataTextField="Name" DataValueField="ID" Width="280px" />
                                <asp:DropDownList ID="rcbLocation" runat="server" Title="Filter By Location" DataTextField="Name"
                                    DataValueField="ID" Width="120px" />
                            </td>
                            <td>
                                <div class="evalBtnOutline floatLeft">
                                    <div class="evalBtn">
                                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnFilter_Click" />
                                    </div>
                                </div>
                                <div class="evalBtnOutline floatLeft">
                                    <div class="resetBtn">
                                        <asp:LinkButton ID="btnReset" runat="server" Text="Reset" OnClick="lbReset_Click" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <cc1:TopPagerGridView ID="gridRecords" runat="server" GridLines="None" AllowPaging="true"
                            AllowSorting="true" AutoGenerateColumns="false" PageSize="20" Skin="Default"
                            OnSorting="HandleSorting" OnPageIndexChanging="HandlePaging" OnDataBound="HandleDataBound"
                            OnItemCommand="HandleItemCommand" CssClass="dataGrid">
                            <HeaderStyle Wrap="false" CssClass="headerRow"/>
                            <RowStyle VerticalAlign="Top" />
                            <AlternatingRowStyle VerticalAlign="Top" CssClass="userDataGridOddRow" />
                            <PagerStyle CssClass="pagerOuterTable"></PagerStyle>
                            <PagerSettings Position="Top"></PagerSettings>
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
                                <asp:BoundField DataField="ID" ReadOnly="true" Visible="false" />
                                <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="true" SortExpression="Name" />
                                <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" ReadOnly="true"
                                    ItemStyle-Wrap="false" SortExpression="EmailAddress"/>
                                <asp:BoundField DataField="Organization" HeaderText="Organization" ReadOnly="true" SortExpression="Organization"/>
                                <asp:TemplateField HeaderText="Technology" SortExpression="Technology.Name">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Technology.Name")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" SortExpression="Location.Name">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Location.Name")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Login*" SortExpression="LastLoginDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateCreatedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("LastLoginDate")).ToString("g")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField HeaderText="Password Reset"  Text="Password Reset" 
                                    CommandName="ResetPassword"></asp:ButtonField>
                                <asp:TemplateField HeaderText="Detail" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a href="detail.aspx?id=<%# Eval("ID") %>">
                                            <img src="/admin/images/detail.gif" alt="View Detail" /></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </cc1:TopPagerGridView>
                        <p>
                            Times are shown in Hawaii Time.</p>
                    </div>
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
