<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="HawaiiDBEDT.Web.Profile.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii</title>
	<link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
	<link rel="stylesheet" media="screen" href="/include/controls/radcontrols/css/combobox.css"
		type="text/css" />
	<link rel="stylesheet" media="print" href="/include/css/print.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="/include/js/javascript.js"></script>
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
					<form runat="server">
					    <asp:ScriptManager runat="server" ID="scriptManager"></asp:ScriptManager>
					<h1>
						Renewable Energy Permitting Wizard
					</h1>
					<p>
						Registration information and evaluation data will be used only for purposes directly
						associated with the Renewable Energy Permitting Wizard tool; mainly providing insight
						on user characteristics. Creating an user account is recommended and will allow
						users to save and edits work in progress.</p>
					
					<h2>
						Register</h2>
					<div class="loginContainer">
						<asp:UpdatePanel ID="radPanel" runat="server">
						    <ContentTemplate>
							<asp:Panel ID="pnlExistingUser" runat="server" Visible="false">
								<div class="confirmation">
									<img src="/images/error.png" width="16" height="16" alt="Confirm" />
									<span class="error">We are unable to create your account.</span>
									<p>
										There is already an existing account with that email address. If you have forgotten
										your password, please <a href="forgot_password.aspx">reset your password</a>.
									</p>
								</div>
							</asp:Panel>
							<asp:Panel ID="pnlRegister" runat="server">
								<table class="login" border="0">
									<tr>
										<td colspan="3">
										</td>
									</tr>
									<tr>
										<td colspan="3" class="smText">
											(<span class="required">*</span> Required Fields)
										</td>
									</tr>
									<tr>
										<td colspan="3">
										</td>
									</tr>
									<tr>
										<td width="35%">
											Full Name:
										</td>
										<td colspan="2">
											<asp:TextBox ID="txtName" runat="server" MaxLength="100" Width="220px" />
										</td>
									</tr>
									<tr>
										<td>
											Company/Business:
										</td>
										<td colspan="2">
											<asp:TextBox ID="txtOrganization" runat="server" MaxLength="100" Width="220px" />
										</td>
									</tr>
									<tr>
										<td>
											Title:
										</td>
										<td colspan="2">
											<asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Width="220px" />
										</td>
									</tr>
									<tr>
										<td>
											Telephone Number:
										</td>
										<td colspan="2">
											<asp:TextBox ID="txtTelephone" runat="server" MaxLength="50" Width="220px" />
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
											<asp:CustomValidator ID="CustomValidator1" OnServerValidate="rcbTechnology_Validate"
												ErrorMessage="Please select your your primary technology." CssClass="required" Display="Dynamic" 
												runat="server" EnableClientScript="false" ValidationGroup="Register" />
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
											<asp:CustomValidator ID="CustomValidator2" OnServerValidate="rcbLocation_Validate"
												ErrorMessage="Please select your your primary location." CssClass="required" Display="Dynamic" 
												runat="server" EnableClientScript="false" ValidationGroup="Register" />
										</td>
									</tr>
									<tr>
										<td height="10px">
										</td>
									</tr>
									<tr>
										<td colspan="3">
											<p>
												Please create an account by entering your email address and a new password below:</p>
										</td>
									</tr>
									<tr>
										<td>
											Email Address<span class="required">*</span>:
										</td>
										<td colspan="2">
											<asp:TextBox ID="txtEmailAddressRegistration" runat="server" MaxLength="100" Width="220px"
												ValidationGroup="Register" />
											<asp:RequiredFieldValidator EnableClientScript="false" ErrorMessage="Please enter your email address."
												ID="RequiredFieldValidator1" ControlToValidate="txtEmailAddressRegistration"
												Display="Dynamic" runat="server" ValidationGroup="Register" />
											<asp:RegularExpressionValidator EnableClientScript="false" ErrorMessage="<br />Please enter a valid email address"
												ID="RegularExpressionValidator2" ControlToValidate="txtEmailAddressRegistration"
												Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
												runat="server" ValidationGroup="Register" />
										</td>
									</tr>
									<tr>
										<td>
											Password<span class="required">*</span>:
										</td>
										<td colspan="2">
											<asp:TextBox ID="txtPasswordRegistration" runat="server" MaxLength="15" Width="220px"
												TextMode="Password" ValidationGroup="Register" />
											<asp:RequiredFieldValidator EnableClientScript="false" ErrorMessage="Please enter a password."
												ID="RequiredFieldValidator4" ControlToValidate="txtPasswordRegistration" Display="Dynamic"
												runat="server" ValidationGroup="Register" />
											<asp:RegularExpressionValidator EnableClientScript="false" ErrorMessage="<br />Password must be at least 8 characters and contain a number."
												ID="RegularExpressionValidator1" ControlToValidate="txtPasswordRegistration"
												Display="Dynamic" ValidationExpression="^.*(?=.{8,15})(?=.*\d)(?=.*[A-Za-z\~!@#$%^&\*()\-_+=]).*$"
												runat="server" ValidationGroup="Register" />
											<br />
											<span class="smText">(must be at least 8 characters and contain a number)</span>
										</td>
									</tr>
									<tr>
										<td>
											Password Confirmation<span class="required">*</span>:
										</td>
										<td colspan="2">
											<asp:TextBox ID="txtPasswordRegistration2" runat="server" MaxLength="15" Width="220px"
												TextMode="Password" ValidationGroup="Register" />
											<asp:RequiredFieldValidator EnableClientScript="false" ErrorMessage="Please enter the password confirmation."
												ID="RequiredFieldValidator2" ControlToValidate="txtPasswordRegistration2"
												Display="Dynamic" runat="server" ValidationGroup="Register" />
											<asp:CompareValidator EnableClientScript="false" runat="server" ID="Comp1" ControlToValidate="txtPasswordRegistration2"
												ControlToCompare="txtPasswordRegistration" Display="Dynamic" ErrorMessage="<br />Password and confirm password values do not match."
												ValidationGroup="Register" />
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
										<td width="110px">
											<div id="btnEval" class="evalBtnOutline">
												<div class="evalBtn">
													<asp:LinkButton ID="btnRegister" runat="server" Text="Submit" ValidationGroup="Register"
														OnClick="btnRegister_Click" />
												</div>
											</div>
										</td>
										<td width="350px">
											&nbsp;<a href="/">Cancel</a>
										</td>
									</tr>
									<tr>
										<td>
											&nbsp;
										</td>
									</tr>
								</table>
							</asp:Panel>
							<asp:Panel ID="pnlConfirmation" runat="server" Visible="false">
								<div class="confirmation">
									<img src="/images/confirm.png" width="16" height="16" alt="Confirm" />
									Your account has been created successfully.
									<%= AdditionalConfirmationMessage %>
									<br />
									<br />
									Please note your information and password can be updated on the <a href="/profile/">My
										Profile</a> page.
								</div>
								<a href="/evaluate/">Start the Renewable Energy Permitting Wizard &gt;&gt;</a>
							</asp:Panel>
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
