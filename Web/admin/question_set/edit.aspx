<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="HawaiiDBEDT.Web.Admin.QuestionSetDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii</title>
    <link rel="stylesheet" media="screen" href="/admin/include/css/stylesheet.css" type="text/css" />
    <link rel="stylesheet" href="/include/controls/radcontrols/css/combobox.css" type="text/css" />
    <script language="javascript" type="text/javascript" src="/admin/include/js/javascript.js"></script>
    <script language="javascript" type="text/javascript">
        function checkAllLocationFields() {
            for (var i = 0; i < document.forms[0].elements.length; i++) {
                var fldObj = document.forms[0].elements[i];
                if (fldObj.type == 'checkbox' && fldObj.name.indexOf('<%= cbLocation.ClientID %>') >= 0) {
                    fldObj.checked = true;
                }
            }
        }
    </script>
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
                        Question Set Detail</h1>
                    <div id="adminNav">
                        &#171; <a href="/admin/">Administration Home</a><br />
                        &#171; <a href="/admin/question_set/">Question Set List</a><br />
                    </div>
                    <p>
                        Questions in a question set will be displayed on a single page.<br />
                        To manage the questions in the question set, select desired questions in the left
                        listbox and use the control buttons to move the options from one
                        listbox to the listbox. To select multiple options, hold down the < Ctrl > key while
                        selecting multiple records with your mouse.</p>
                    <span class="small">Field marked with <span class="required">*</span> are required.</span>
                    <p class="required">
                        <%= ConfirmationText %></p>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <table border="0" class="editform">
                                <tr>
                                    <td>
                                        Name<span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtName" runat="server" Columns="90" MaxLength="250" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="required" ErrorMessage="Required"
                                            ControlToValidate="txtName" />
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
                                        Technology<span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="rcbTechnology" runat="server" Title="Select Technology" DataTextField="Name"
                                            DataValueField="ID" Skin="Custom" EnableEmbeddedSkins="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Location<span class="required">*</span>:<br />
                                        <a href="#" onclick="checkAllLocationFields(); return false;">Select All</a>
                                    </td>
                                    <td colspan="2">
                                        <span class="required">
                                            <%= ErrorText %></span>
                                        <asp:CheckBoxList ID="cbLocation" runat="server" DataTextField="Name" DataValueField="ID"
                                            RepeatDirection="Horizontal" CssClass="noStyle" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Questions:
                                    </td>
                                    <td>
                                        <hiewiz:TransferableListBox ID="tlbQuestions" runat="server" ListHeight="500px" ListWidth="320px"
                                            DataTextField="Name" DataValueField="ID" SourceListToolTip="Available Questions"
                                            SourceListLabel="Available Questions" DestinationListLabel="Selected Questions"
                                            DestinationListToolTip="Selected Questions" AllowReorder="True"></hiewiz:TransferableListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="buttonRow">
                                        &nbsp;
                                    </td>
                                    <td class="buttonRow" colspan="2">
                                        <div class="evalBtnOutline floatLeft">
                                            <div class="evalBtn">
                                                <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                            </div>
                                        </div>
                                        <div class="evalBtnOutline floatLeft">
                                            <div class="evalBtn">
                                                <asp:LinkButton ID="btnSubmitAnother" runat="server" Text="Submit and Add Another"
                                                    OnClick="btnSubmitAnother_Click" />
                                            </div>
                                        </div>
                                        <div class="evalBtnOutline floatLeft">
                                            <div class="evalBtn">
                                                <asp:LinkButton ID="btnSubmitDuplicate" runat="server" Text="Submit and Duplicate"
                                                    OnClick="btnSubmitDuplicate_Click" />
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
