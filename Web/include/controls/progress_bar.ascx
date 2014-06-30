<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="progress_bar.ascx.cs" Inherits="HawaiiDBEDT.Web.Controls.ProgressBarControl" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Panel ID="pnlProgressBar" CssClass="evalProgress2" runat="server">
    <asp:Panel ID="pnlPreEval" runat="server" CssClass="progressPreEval">
        <asp:LinkButton runat="server" ID="lnkBtnPreEval"  OnClick="LnkBtnPreEvaluationClick" Width="130px" Height="37px"/> 
        <span class="hidden">Pre-Evaluation</span>
    </asp:Panel>
    <asp:Panel ID="pnlDimensionProgress" runat="server" CssClass="progressDimensions">
        <span class="hidden">Evaluation</span>
        <asp:Repeater ID="rptDimensionProgress" runat="server" OnItemCommand="HandleImgButtonClick">
            <HeaderTemplate>
                <table class="progressDots">
                    <tr>
            </HeaderTemplate>
            <ItemTemplate>
                <td>
                    <asp:LinkButton runat="server" CommandName="Foo" CommandArgument='<%#DataBinder.Eval(Container, "ItemIndex", "")%>' Enabled='<%# EnableNavigationToTheStep(int.Parse(DataBinder.Eval(Container, "ItemIndex", ""))) %>'>
                        <asp:Image runat="server" ID="imgBtnDimensionDot" Width="9" Height="9" ImageUrl='<%# GetImagePath(Eval("ID")) %>' CssClass="navigationButton" data-tip='<%# GetStepTitle(int.Parse(DataBinder.Eval(Container, "ItemIndex", ""))) %>' />
                    </asp:LinkButton>
                </td>
            </ItemTemplate>
            <SeparatorTemplate>
                <td>
                    &nbsp;
                </td>
            </SeparatorTemplate>
            <FooterTemplate>
                </tr> </table>
            </FooterTemplate>
        </asp:Repeater>
        <script type="text/javascript">
            // Create the tooltips only on document load
            $(document).ready(function () {
                $('.navigationButton').each(function () {
                    var step = $(this).data('tip');
                    $(this).qtip(
                        {
                            content: '<p style="margin-bottom: 0px; padding-bottom: 0px;">' + step + '</p>'
                        });
                });
            });
                    </script>
    </asp:Panel>
    <asp:Panel ID="Panel1" CssClass="progressResults" runat="server">
        <span class="hidden">Evaluation Results</span>
    </asp:Panel>
</asp:Panel>