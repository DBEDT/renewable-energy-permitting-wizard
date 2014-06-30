<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.PermitDetail"
    MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii</title>
    <link rel="stylesheet" href="/admin/include/css/stylesheet.css" type="text/css" />
    <link rel="stylesheet" href="/include/controls/radcontrols/css/listbox.css" type="text/css" />
    <link rel="stylesheet" href="/include/controls/radcontrols/css/combobox.css" type="text/css" />
    <script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
</head>
<body>
    <%@ register tagprefix="hiewiz" tagname="TransferableListBox" src="/admin/controls/TransferableListBox.ascx" %>
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
                        Permit Detail</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a><br />
                        &#171; <a href="/admin/permit/">Permit List</a><br />
                    </div>
                    <span class="small">Field marked with <span class="required">*</span> are required.</span>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <table border="0" class="editform">
                                <tr>
                                    <td width="17%">
                                        Name <span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtName" runat="server" Columns="90" MaxLength="250" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="required" ErrorMessage="Required"
                                            ControlToValidate="txtName" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Short Name <span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtShortName" runat="server" Columns="90" MaxLength="100" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" CssClass="required"
                                            ErrorMessage="Required" ControlToValidate="txtShortName" />
                                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^.{1,100}$" ControlToValidate="txtShortName" Display="Dynamic" ErrorMessage="Maximum 100 characters allowed." ></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Description:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDescription" runat="server" Columns="88" Rows="2" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Type <span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="rcbPermitType" runat="server" Title="Select Permit Type" DataTextField="Name"
                                            DataValueField="ID" Skin="Custom" EnableEmbeddedSkins="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        URL:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtURL" runat="server" Columns="90" MaxLength="1000" />
                                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtURL" 
                                        ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?" Display="Dynamic" ErrorMessage="Valid URL is required."></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Appendix/Section:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAppendixLink" runat="server" Columns="90" MaxLength="100" />
                                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtAppendixLink" 
                                        ValidationExpression="^[a-zA-Z0-9\' &#\$\*_\-@!().,]+$" Display="Dynamic" ErrorMessage="Valid URL is required."></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="/admin/department/" target="_blank">Department</a>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="rcbDepartment" runat="server" Title="Select Department" DataTextField="Name"
                                            DataValueField="ID" Skin="Custom" EnableEmbeddedSkins="false" Width="450px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Duration<span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDuration" runat="server" Columns="5" MaxLength="5" />
                                        <span class="small">(in months)</span> <span class="required">
                                            <%= ErrorText %></span>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="required"
                                            ErrorMessage="Required" ControlToValidate="txtDuration" />
                                        <asp:RangeValidator runat="server" ControlToValidate="txtDuration" MinimumValue="0"
                                            MaximumValue="32000" ErrorMessage="Duration must be between 0 and 32000" Type="Integer" Display="Dynamic"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="/admin/permit_set" target="_blank">Permit Sets</a><br />
                                        <span class="small">< Ctrl > + click to select multiple permits sets. Use the button
                                            controls to move the permit sets.</span>
                                    </td>
                                    <td>
                                        <hiewiz:TransferableListBox ID="tlbPermitSets" runat="server" ListHeight="115px"
                                            ListWidth="320px" DataTextField="Name" DataValueField="ID" SourceListLabel="Available Permit Sets"
                                            DestinationListLabel="Selected Permit Sets" SourceListToolTip="Available Permit Sets"
                                            DestinationListToolTip="Selected Permit Sets"></hiewiz:TransferableListBox>
                                    </td>
                                </tr>
                            </table>
                            <h3>
                                Dependencies</h3>
                            <table width="900" border="0" class="editform">
                                <tr>
                                    <td width="17%">
                                        Finish to Start:<br />
                                        <span class="small">This permit cannot start until the selected permits are finished.</span>
                                    </td>
                                    <td>
                                        <hiewiz:TransferableListBox ID="tlbPermitFinishedToStart" runat="server" ListHeight="115px"
                                            ListWidth="320px" DataTextField="Name" DataValueField="ID" SourceListLabel="Available Permits"
                                            DestinationListLabel="Selected Permits" SourceListToolTip="Available Permits"
                                            DestinationListToolTip="Selected Permits"></hiewiz:TransferableListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Finish to Finish:<br />
                                        <span class="small">This permit cannot finish until the selected permits are finished.</span>
                                    </td>
                                    <td>
                                        <hiewiz:TransferableListBox ID="tlbPermitFinishedToFinish" runat="server" ListHeight="115px"
                                            ListWidth="320px" DataTextField="Name" DataValueField="ID" SourceListLabel="Available Permits"
                                            DestinationListLabel="Selected Permits" SourceListToolTip="Available Permits"
                                            DestinationListToolTip="Selected Permits"></hiewiz:TransferableListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Start to Start:<br />
                                        <span class="small">This permit must start with the selected permits.</span>
                                    </td>
                                    <td>
                                        <hiewiz:TransferableListBox ID="tlbPermitStartedToStart" runat="server" ListHeight="115px"
                                            ListWidth="320px" DataTextField="Name" DataValueField="ID" SourceListLabel="Available Permits"
                                            DestinationListLabel="Selected Permits" SourceListToolTip="Available Permits"
                                            DestinationListToolTip="Selected Permits"></hiewiz:TransferableListBox>
                                    </td>
                                </tr>
                            </table>
                            <%--</asp:UpdatePanel>--%>
                            <table width="900" border="0" class="editform">
                                <tr>
                                    <td class="buttonRow" width="14%" valign="top">
                                        <asp:HyperLink ID="hlPrevious" runat="server" Text="&#171; Previous" />
                                    </td>
                                    <td class="buttonRow">
                                        <div class="evalBtnOutline floatLeft">
                                            <div class="evalBtn">
                                                <asp:LinkButton ID="btnSumit" runat="server" Text="Submit" OnClick="btnSumit_Click" />
                                            </div>
                                        </div>
                                        <div class="evalBtnOutline floatLeft">
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
