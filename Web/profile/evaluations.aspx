<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="evaluations.aspx.cs" Inherits="HawaiiDBEDT.Web.Profile.Evaluations" %>

<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii</title>
    <link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
    <link rel="stylesheet" media="print" href="/include/css/print.css" type="text/css" />
    <script language="javascript" type="text/javascript" src="/include/js/javascript.js"></script>
    <script type="text/javascript" >
        showSessionModal = '<% = IsLoggedIn %>';
    </script>
</head>
<body>
    <div id="pageWrapper">
        <!-- BEGIN GLOBAL -->
        <!-- #include virtual="~/include/global.aspx" -->
        <!-- END GLOBAL -->
        <!-- BEGIN HEADER -->
        <!-- #include virtual="~/include/header.aspx" -->
        <!-- END HEADER -->
        <div id="contentWrapper">
            <div>
                <div class="contentWrapperHome">
                    <a name="mainContent" id="mainContent"></a>
                    <form id="Form2" runat="server">
                    <asp:ScriptManager runat="server">
                    </asp:ScriptManager>
                    <h1>
                        Renewable Energy Permitting Wizard
                    </h1>
                    <div class="right">
                        <asp:LinkButton ID="lbStartOver2" runat="server" OnClick="lbStartOver_OnClick" CssClass="startOver"
                            Text="Start a New Session &gt;&gt;" />
                    </div>
                    <h2>
                        My Sessions</h2>
                    <asp:UpdatePanel ID="ajaxPanel" runat="server">
                        <ContentTemplate>
                            <table class="evalList">
                                <tr>
                                    <td>
                                        <h3>
                                            In Progress</h3>
                                        <asp:Panel ID="pnlDeleteConfirmation" runat="server" Visible="false">
                                            <div class="confirmation">
                                                <img src="/images/confirm.png" width="16" height="16" alt="Confirm" />
                                                The evaluation has been deleted successfully.
                                            </div>
                                        </asp:Panel>
                                        <asp:DataGrid ID="gridEvaluation" runat="server" GridLines="None" AllowPaging="True"
                                            AllowSorting="true" AutoGenerateColumns="false" PageSize="10" OnSortCommand="gridEvaluation_sort"
                                            OnPageIndexChanged="gridEvaluation_pageChange" CssClass="dataGrid" OnItemCommand="HandleItemCommand">
                                            <HeaderStyle Wrap="false" CssClass="headerRow" />
                                            <ItemStyle VerticalAlign="Top" CssClass="dataGridEvenRow" />
                                            <AlternatingItemStyle VerticalAlign="Top" CssClass="dataGridOddRow" />
                                            <PagerStyle Mode="NumericPages" HorizontalAlign="Left" Position="TopAndBottom" CssClass="pagerRow">
                                            </PagerStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Modified Date" SortExpression="DateModified">
                                                    <ItemStyle Width="110px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDateModifiedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateModified")).ToString("g")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Created Date" SortExpression="DateCreated">
                                                    <ItemStyle Width="110px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDateCreatedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateCreated")).ToString("g")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Name" HeaderText="Name" ReadOnly="true" ItemStyle-Width="250px"
                                                    SortExpression="Name" />
                                                <asp:BoundColumn DataField="Description" HeaderText="Description" ReadOnly="true"
                                                    SortExpression="Description" />
                                                <asp:TemplateColumn HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <a href="/evaluate/review.aspx?id=<%# Eval("ID") %>">
                                                            <img src="/images/edit.gif" alt="Edit" /></a>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                                            ImageUrl="/images/delete.gif" OnClientClick="return confirm('Are you sure you wish to delete this evaluation? This action cannot be undone.');"/>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <p>
                                            &nbsp;</p>
                                        <h3>
                                            Completed</h3>
                                        <asp:DataGrid ID="gridEvaluationComplete" runat="server" GridLines="None" OnNeedDataSource="gridEvaluationComplete_NeedDataSource"
                                            AllowPaging="True" AllowSorting="true" AutoGenerateColumns="false" PageSize="10"
                                            OnSortCommand="gridEvaluationComplete_sort" OnPageIndexChanged="gridEvaluationComplete_pageChange"
                                            CssClass="dataGrid">
                                            <HeaderStyle Wrap="false" CssClass="headerRow" />
                                            <ItemStyle VerticalAlign="Top" CssClass="dataGridEvenRow" />
                                            <AlternatingItemStyle VerticalAlign="Top" CssClass="dataGridOddRow" />
                                            <PagerStyle Mode="NumericPages" HorizontalAlign="Left" Position="TopAndBottom" CssClass="pagerRow">
                                            </PagerStyle>
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Modified Date" SortExpression="DateModified">
                                                    <ItemStyle Width="110px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDateModifiedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateModified")).ToString("g")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Created Date" SortExpression="DateCreated">
                                                    <ItemStyle Width="110px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDateCreatedAdjusted" runat="server" Text='<%# DateTimeUtils.ToHawaiiTime((DateTime)Eval("DateCreated")).ToString("g")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Name" HeaderText="Name" ReadOnly="true" ItemStyle-Width="250px"
                                                    SortExpression="Name" />
                                                <asp:BoundColumn DataField="Description" HeaderText="Description" ReadOnly="true"
                                                    SortExpression="Description" />
                                                <asp:TemplateColumn HeaderText="Detail" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <a href="/evaluate/review.aspx?id=<%# Eval("ID") %>&detail=1" target="_blank">
                                                            <img src="/images/detail.gif" alt="View" /></a>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Make a Copy*" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <a href="/evaluate/review.aspx?id=<%# Eval("ID") %>&clone=1">
                                                            <img src="/images/page_copy.png" height="16" width="16" alt="Make a copy" /></a>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <p>
                                            * - Any completed session can be copied as a basis for a new evaluation session.</p>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </form>
                    <p class="smText">
                        All times shown are in Hawaii Time.</p>
                </div>
            </div>
            <!-- BEGIN FOOTER -->
            <!-- #include virtual="~/include/footer.aspx" -->
            <!-- END FOOTER -->
        </div>
    </div>
</body>
</html>
