<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="login.ascx.cs" Inherits="HawaiiDBEDT.Web.Controls.LoginControl" %>
<h2 runat="server" id="loginTitle">Login</h2>
<div class="loginContainer">
	<asp:Panel ID="plIncorrectPassword" runat="server" Visible="false">
		<div class="confirmation">
			<img src="/images/error.png" width="16" height="16" alt="Confirm" />
			<span class="error">You did not enter the correct email address and/or password.</span>
			<p>
				If you are a new user and have not registered, please <a href="/profile/register.aspx">
					register</a>.
				<br />
				If you have forgotten your password, please <a href="/profile/forgot_password.aspx">request
					a new password</a>.</p>
		</div>
	</asp:Panel>
	<asp:Panel ID="pnlLogin" runat="server" DefaultButton="submitBtn">
		<table class="login2">
			<tr>
				<td colspan="3">
		<p class="bold">Log in to save a session in progress, update a session in progress, or save a completed session.</p>
				</td>
			</tr>
			<tr>
				<td width="24%">
					Email Address:
				</td>
				<td colspan="2">
					<asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="250px" /><asp:RequiredFieldValidator
						EnableClientScript="false" ErrorMessage=" Required." ID="emailValidator" ControlToValidate="txtEmail"
						Display="Dynamic" runat="server" ValidationGroup="VGLogin" />
				</td>
			</tr>
			<tr>
				<td>
					Password:
				</td>
				<td colspan="2">
					<DEBEDTUtils:PasswordTextBox ID="txtPassword" TextMode="Password" MaxLength="50"
						runat="server" Width="250px" />
					<asp:RequiredFieldValidator EnableClientScript="false" ErrorMessage="Required." ID="passwordValidator"
						ControlToValidate="txtPassword" Display="Dynamic" runat="server" ValidationGroup="VGLogin" />
				</td>
			</tr>
			<tr>
				<td>
					Starting Page:
				</td>
				<td colspan="2">
					<asp:DropDownList ID="ddlStartingPage" runat="server">
						<asp:ListItem Text="My Evaluations" Value="MyEvaluations" />
						<asp:ListItem Text="My Profile" Value="MyProfile" />
						<asp:ListItem Text="New Evaluation" Value="NewEvaluation" />
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td height="10px">
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td colspan="2">
					<asp:CheckBox ID="cbRemember" runat="server" Text="Remember Login Information" />
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
    				<asp:ImageButton runat="server" ID="submitBtn" ImageUrl="/images/submit_btn.png" OnClick="btnLogin_OnClick"/>
				</td>
				<td width="500px">
					&nbsp;<a href="/profile/forgot_password.aspx">Forgot Your Password?</a>
				</td>
			</tr>
			<tr>
				<td height="10px">
				</td>
			</tr>
			<tr>
				<td colspan="3">
					<p>
						<b>New user?</b> <a href="/profile/register.aspx">Register Now</a> to save your sessions
						and evaluation history.</p>
					<p>
						Not interested in creating an account right now? <a href="/evaluate/">Start the Renewable Energy Permitting Wizard</a>. You can register/login when the evaluation is complete.</p>
				
				</td>
			</tr>
		</table>
	</asp:Panel>
	<asp:Panel ID="pnlLoggedIn" runat="server" Visible="false">
		<p class="padLeft10">
			<asp:LinkButton ID="lbStartNewSession" runat="server" OnClick="lbStartSession_OnClick" CssClass="startOver"
							Text="Start a New Session &gt;&gt;" />
		</p>
	</asp:Panel>
</div>
