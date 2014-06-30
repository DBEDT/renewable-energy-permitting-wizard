<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="permit_set.ascx.cs"
	Inherits="HawaiiDBEDT.Web.Controls.permit_set" %>
	<p class="alignLeft">
		<b>Permits in
			<asp:Label ID="lblPermitSetName" runat="server" /></b></p>
	<ul id="requiredPermit">
		<asp:Repeater ID="rptPermits" runat="server">
			<ItemTemplate>
                <li>
                    <%# DisplayPermit(Container.DataItem) %>
                </li>
			</ItemTemplate>
		</asp:Repeater>
	</ul>


<script language="javascript" type="text/javascript">
	$(document).ready(function () {
		$("ul#requiredPermit").children(':last').attr("class", "lastItem");
	});
</script>