<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="global_nav.ascx.cs"
	Inherits="HawaiiDBEDT.Web.Controls.GlobalNavControl" %>
	<li>
		<asp:Literal ID="ltlSeparator0" runat="server" Visible="true" Text="|" /></li>
	<li>
		<asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/default.aspx#login"
			Text="Login" /><asp:HyperLink ID="hlEvaluations" runat="server" NavigateUrl="~/profile/evaluations.aspx"
				Text="My Evaluations" Visible="false" /></li>
	<li>
		<asp:Literal ID="ltlSeparator1" runat="server" Visible="true" Text="|" /></li>
	<li>
		<asp:HyperLink ID="hlRegister" runat="server" NavigateUrl="~/profile/register.aspx"
			Text="Register" /><asp:HyperLink ID="hlProfile" runat="server" NavigateUrl="~/profile/"
				Text="My Profile" Visible="false" /></li>
	<li>
		<asp:Literal ID="ltlSeparator2" runat="server" Visible="false" Text="|" />
	</li>
	<li>
		<asp:HyperLink ID="hlLogout" runat="server" NavigateUrl="~/profile/logout.aspx" Text="Logout"
			Visible="false" /></li>

