<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.TechnologyList" %>

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
                        Technologies</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a>
                    </div>
                    <p class="required">
                        <%= ConfirmationText %></p>
                    <p>
                        Click on the technology name or the edit icon to view permit sets and question sets
                        for the technology.
                    </p>
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
                            <asp:HyperLinkColumn DataNavigateUrlFormatString="edit.aspx?id={0}" DataNavigateUrlField="ID"
                                DataTextField="Name" HeaderText="Name" SortExpression="Name" />
                            <asp:TemplateColumn HeaderText="Last Modified" SortExpression="DateModified">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateModifiedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateModified")).ToString("MM/dd/yyyy")%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href='edit.aspx?id=<%# DataBinder.Eval(Container.DataItem, "ID")%>'>
                                        <img src="/admin/images/edit.gif" alt="Edit" /></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
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
