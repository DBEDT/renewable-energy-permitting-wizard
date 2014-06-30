<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HawaiiDBEDT.Web.Profile.ProfileDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii</title>
    <link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
    <link rel="stylesheet" media="screen" href="/include/controls/radcontrols/css/combobox.css"
        type="text/css" />
    <link rel="stylesheet" media="print" href="/include/css/print.css" type="text/css" />
    <script language="javascript" type="text/javascript" src="/include/js/javascript.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#<%=txtPasswordUpdate.ClientID %>').val('');
        });

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
                    <form id="Form1" runat="server">
                        <asp:ScriptManager runat="server"></asp:ScriptManager>
                    <h1>
                        Renewable Energy Permitting Wizard
                    </h1>
                    <h2>
                        My Profile</h2>
                    <div class="loginContainer">
                        <asp:UpdatePanel ID="radPanel" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlExistingUser" runat="server" Visible="false">
                                
                                <div class="confirmation">
                                    <img src="/images/error.png" width="16" height="16" alt="Confirm" />
                                    <span class="error">We are unable to update your information.</span>
                                    <p>
                                        There is already an existing account with that email address. If you have forgotten
                                        your password, please <a href="forgot_password.aspx">reset your password</a>.
                                    </p>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlConfirmation" runat="server" Visible="false">
                                <div class="confirmation">
                                    <img src="/images/confirm.png" width="16" height="16" alt="Confirm" />
                                    Your account has been updated successfully.
                                </div>
                            </asp:Panel>
                            <table class="login">
                                <tr>
                                    <td colspan="3" class="smText">
                                        (<span class="required">*</span> Required Fields)
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="35%">
                                        Full Name:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtName" runat="server" MaxLength="100" Width="220px" />
                                        <asp:RegularExpressionValidator runat="server" ID="revName" ValidationExpression="^[^/:={}<>\[\]\\]{0,100}$"
                                            ErrorMessage="Please enter a valid full name." ControlToValidate="txtName" Display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Company/Business:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOrganization" runat="server" MaxLength="100" Width="220px" />
                                        <asp:RegularExpressionValidator runat="server" ID="revOrganization" ValidationExpression="^[^/:={}<>\[\]\\]{0,100}$"
                                            ErrorMessage="Please enter a valid company/business." ControlToValidate="txtOrganization"
                                            Display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Title:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Width="220px" />
                                        <asp:RegularExpressionValidator runat="server" ID="revTitle" ValidationExpression="^[^/:={}<>\[\]\\]{0,100}$"
                                            ErrorMessage="Please enter a valid title." ControlToValidate="txtTitle" Display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Telephone Number:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTelephone" runat="server" MaxLength="50" Width="220px" />
                                        <asp:RegularExpressionValidator runat="server" ID="revTelephone" ValidationExpression="^[a-zA-Z0-9\- ()\+]{0,50}$"
                                            ErrorMessage="Please enter a valid telephone number." ControlToValidate="txtTelephone"
                                            Display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Primary Technology<span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="rcbTechnology" runat="server" Skin="Custom" EnableEmbeddedSkins="false"
                                            ExpandAnimation-Type="InQuad" MarkFirstMatch="true" DataTextField="Name" DataValueField="ID"
                                            EnableViewState="true" Width="220px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Primary Location<span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="rcbLocation" runat="server" Skin="Custom" EnableEmbeddedSkins="false"
                                            ExpandAnimation-Type="InQuad" MarkFirstMatch="true" EnableViewState="true" DataTextField="Name"
                                            DataValueField="ID" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email Address<span class="required">*</span>:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" Width="220px" />
                                        <asp:RequiredFieldValidator ErrorMessage="Please enter your email address."
                                            ID="RequiredFieldValidator1" ControlToValidate="txtEmail" Display="Dynamic" runat="server" />
                                        <asp:RegularExpressionValidator runat="server" ID="revEmail" ValidationExpression="^(?:[a-zA-Z0-9_'^&amp;/+-])+(?:\.(?:[a-zA-Z0-9_'^&amp;/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)$"
                                            ErrorMessage="Please enter a valid email address." ControlToValidate="txtEmail"
                                            Display="Dynamic" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <p>
                                            If you would like to update your password, please enter the old password and a new password below:</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Old Password:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOldPassword" runat="server" MaxLength="15" Width="220px" TextMode="Password"
                                            ValidationGroup="Register" />
                                        <asp:RegularExpressionValidator EnableClientScript="false" ErrorMessage="<br />Password must be at least 8 characters and contain a number."
                                            ID="RegularExpressionValidator1" ControlToValidate="txtOldPassword" Display="Dynamic"
                                            ValidationExpression="^.*(?=.{8,15})(?=.*\d)(?=.*[A-Za-z\~!@#$%^&\*()\-_+=]).*$"
                                            runat="server" />
                                        <asp:CustomValidator runat="server" ID="custValOldPassword" ControlToValidate="txtOldPassword" EnableClientScript="False"
                                        Display="Dynamic" ErrorMessage="<br />Please enter the old password." OnServerValidate="CustValOldPasswordValidate" ValidateEmptyText="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Password:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtPasswordUpdate" runat="server" MaxLength="15" Width="220px" TextMode="Password"
                                            ValidationGroup="Register" />
                                        <asp:RegularExpressionValidator EnableClientScript="false" ErrorMessage="<br />Password must be at least 8 characters and contain a number."
                                            ID="revPasswordUpdate" ControlToValidate="txtPasswordUpdate" Display="Dynamic"
                                            ValidationExpression="^.*(?=.{8,15})(?=.*\d)(?=.*[A-Za-z\~!@#$%^&\*()\-_+=]).*$"
                                            runat="server" />
                                        <br />
                                        <span class="smText">(must be at least 8 characters and contain a number)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Password Confirmation:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtPasswordUpdate2" runat="server" MaxLength="15" Width="220px"
                                            TextMode="Password" ValidationGroup="Register" />
                                        <asp:CustomValidator runat="server" ID="custValConfirmPassword" Display="Dynamic" OnServerValidate="CustValConfirmPasswordValidate"
                                        ErrorMessage="<br />Please enter a confirm password." ControlToValidate="txtPasswordUpdate2" ValidateEmptyText="True" EnableClientScript="False" />
                                        <asp:CompareValidator EnableClientScript="false" runat="server" ID="Comp1" ControlToValidate="txtPasswordUpdate2"
                                            ControlToCompare="txtPasswordUpdate" Display="Dynamic" ErrorMessage="<br />Password and confirm password values do not match." />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="10px">
                                    </td>
                                </tr>
                                <tr valign="middle">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td width="110px">
                                        <div id="btnEval" class="evalBtnOutline">
                                            <div class="evalBtn">
                                                <asp:LinkButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Submit" CausesValidation="True" />
                                            </div>
                                        </div>
                                    </td>
                                    <td class="left padTop5" width="350px">
                                        &nbsp;<a href="/">Cancel</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    </form>
                </div>
            </div>
            <!-- BEGIN FOOTER -->
            <!-- #include virtual="~/include/footer.aspx" -->
            <!-- END FOOTER -->
        </div>
    </div>
</body>
</html>
