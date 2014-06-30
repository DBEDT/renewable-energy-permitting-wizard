<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgot_password.aspx.cs"
    Inherits="HawaiiDBEDT.Web.Profile.ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii</title>
    <link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
    <link rel="stylesheet" media="screen" href="/include/controls/radcontrols/css/combobox.css"
        type="text/css" />
    <link rel="stylesheet" media="print" href="/include/css/print.css" type="text/css" />
    <script type="text/javascript" src="/include/js/javascript.js"></script>
    <script type="text/javascript">
        showSessionModal = false;
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
                    <h1>
                        Renewable Energy Permitting Wizard
                    </h1>
                    <h2>
                        Password Reset</h2>
                    <div class="loginContainer">
                        <div class="evaluation2">
                            <asp:Panel ID="pnlUserNotFound" runat="server" Visible="false">
                                <div class="confirmation">
                                    <img src="/images/error.png" width="16" height="16" alt="Confirm" />
                                    <span class="error">We could not find your email address in our users list.</span>
                                    <p>
                                        Please try again or <a href="/profile/register.aspx">register</a>.
                                    </p>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlPasswordRequest" runat="server">
                                <table class="login2" width="90%">
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            Please enter your email address below and click submit to receive a randomly generated
                                            password via email.<br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20%">
                                            Email Address <span class="required">*</span>:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtEmail" Columns="30" MaxLength="50" runat="server" />
                                            <asp:RequiredFieldValidator EnableClientScript="false" ErrorMessage="<br />Please enter your email address."
                                                ID="emailValidator" ControlToValidate="txtEmail" Display="Dynamic" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="90px">
                                            <div id="btnEval" class="evalBtnOutline">
                                                <div class="evalBtn">
                                                    <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                                </div>
                                            </div>
                                        </td>
                                        <td width="370px">
                                            <div class="left padTop5" width="350px">
                                                &nbsp;<a href="/">Back to Login</a></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlConfirm" runat="server" Visible="false">
                                <div class="confirmation">
                                    <img src="/images/confirm.png" width="16" height="16" alt="Confirm" />
                                    A new password has been sent to your email address. Please check your inbox.
                                    <br />
                                    <br />
                                    <p class="padLeft10">
                                        <a href="/#login"><strong>Login</strong></a></p>
                                </div>
                            </asp:Panel>
                        </div>
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
