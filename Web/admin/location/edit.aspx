<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.LocationDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii : Administration
    </title>
    <link rel="stylesheet" href="/admin/include/css/stylesheet.css" type="text/css" />
    <link rel="stylesheet" href="/include/controls/radcontrols/css/grid.css" type="text/css" />
    <link rel="stylesheet" href="/include/controls/radcontrols/css/listbox.css" type="text/css" />
    <link rel="stylesheet" href="/include/controls/radcontrols/css/combobox.css" type="text/css" />
    <script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
</head>
<body>
    <%@ Register tagprefix="hiewiz" tagname="TransferableListBox" src="/admin/controls/TransferableListBox.ascx" %>
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
                        Location Details</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a><br />
                        &#171; <a href="/admin/location/">Location List</a>
                    </div>
                    <p class="required">
                        <%= ConfirmationText %></p>
                    <p>
                        Select permit sets for the location below. Click "Submit" to submit your changes
                        and return to the location list page.
                    </p>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <table border="0" class="editform">
                                <tr>
                                    <td>
                                        Location:
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblLocation" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap="true">
                                        <a href="/admin/permit_set" target="_blank">Permit Sets</a>:
                                    </td>
                                    <td>
                                        <hiewiz:TransferableListBox ID="tlbPermitSets" runat="server" ListHeight="115px"
                                            ListWidth="350px" DataTextField="Name" DataValueField="ID" SourceListLabel="Available Permit Sets"
                                            DestinationListLabel="Selected Permit Sets" SourceListToolTip="Available Permit Sets"
                                            DestinationListToolTip="Selected Permit Sets"></hiewiz:TransferableListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="buttonRow">
                                        &nbsp;
                                    </td>
                                    <td class="buttonRow" colspan="2">
                                        <div class="evalBtnOutline floatLeft">
                                            <div class="evalBtn">
                                                <asp:LinkButton ID="btnSumit" runat="server" Text="Submit" OnClick="btnSumit_Click" />
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
