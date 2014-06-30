<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="HawaiiDBEDT.Web.admin.user.detail" %>

<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii : Administration
    </title>
    <link rel="stylesheet" href="/admin/include/css/stylesheet.css" type="text/css" />
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
                        User Details</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a><br />
                        &#171; <a href="/admin/user/">User List</a><br />
                    </div>
                    <table border="0" class="editform">
                        <tr>
                            <td class="colHeading" width="30%">
                                Name:
                            </td>
                            <td>
                                <asp:Label ID="lblName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colHeading">
                                Title:
                            </td>
                            <td>
                                <asp:Label ID="lblTitle" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colHeading">
                                Business/Company:
                            </td>
                            <td>
                                <asp:Label ID="lblOrganization" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colHeading">
                                Telephone Number:
                            </td>
                            <td>
                                <asp:Label ID="lblTelephoneNumber" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colHeading">
                                Email Address:
                            </td>
                            <td>
                                <asp:Label ID="lblEmail" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colHeading">
                                Technology :
                            </td>
                            <td>
                                <asp:Label ID="lblTechnology" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colHeading">
                                Location :
                            </td>
                            <td>
                                <asp:Label ID="lblLocation" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colHeading">
                                Last Login :
                            </td>
                            <td>
                                <asp:Label ID="lblLastLogin" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="colHeading">
                                Evaluation Sessions :
                            </td>
                            <td>
                                Total -
                                <asp:Label ID="lblNumTotal" runat="server" /><br />
                                Complete -
                                <asp:Label ID="lblNumComplete" runat="server" /><br />
                                In Progress -
                                <asp:Label ID="lblNumInProgress" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <p>
                    </p>
                    <h3>
                        Evaluation History</h3>
                    <p>
                        Only completed evaluations are shown below. Click on the detail column to view complete
                        results.</p>
                    <asp:DataGrid ID="gridEvaluation" runat="server" GridLines="None" AllowPaging="true"
                        AllowSorting="true" AutoGenerateColumns="false" PageSize="20" Skin="Default"
                        OnSortCommand="gridEvaluation_sort" OnPageIndexChanged="gridEvaluation_pageChange"
                        CssClass="userDataGrid">
                        <HeaderStyle Wrap="false" CssClass="userHeaderRow" />
                        <ItemStyle VerticalAlign="Top" />
                        <AlternatingItemStyle VerticalAlign="Top" CssClass="userDataGridOddRow" />
                        <PagerStyle Mode="NumericPages" HorizontalAlign="Left" Position="TopAndBottom" CssClass="userPagerRow">
                        </PagerStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Last Modified" SortExpression="DateModified">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateCreatedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateModified")).ToString("g")%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Technology" SortExpression="Technology.Name">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Technology.Name")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Location" SortExpression="Location.Name">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Location.Name")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Detail" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="/evaluate/review.aspx?id=<%# Eval("ID") %>&admin=1" target="_blank">
                                        <img src="/admin/images/detail.gif" alt="View Detail" /></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <br />
                    <h3>
                        Login History</h3>
                    <br />
                    <asp:DataGrid ID="gridLoginHistory" runat="server" GridLines="None" AllowPaging="true"
                        AllowSorting="true" AutoGenerateColumns="false" PageSize="5" Skin="Default" OnSortCommand="gridLoginHistory_sort"
                        OnPageIndexChanged="gridLoginHistory_pageChange" CssClass="userDataGrid">
                        <HeaderStyle Wrap="false" CssClass="userHeaderRow" />
                        <ItemStyle VerticalAlign="Top" />
                        <AlternatingItemStyle VerticalAlign="Top" CssClass="userDataGridOddRow" />
                        <PagerStyle Mode="NumericPages" HorizontalAlign="Left" Position="TopAndBottom" CssClass="userPagerRow">
                        </PagerStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Login Date" SortExpression="LoginDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateCreatedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("LoginDate")).ToString("g")%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <p>
                       Times are shown in Hawaii Time.</p>
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
