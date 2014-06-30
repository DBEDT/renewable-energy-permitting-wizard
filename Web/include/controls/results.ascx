<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="results.ascx.cs" Inherits="HawaiiDBEDT.Web.Controls.ResultsControl" %>
<h3>Customize Your Session</h3>
<div class="loginContainer padLeft10">
	<asp:Panel ID="pnlSave" runat="server" Visible="false" class="saveSummary" CssClass="BeforePanel">
		<p>
			This evaluation session has been automatically saved and can be accessed via the <a href="/profile/evaluations.aspx"
				target="_blank">My Evaluations</a> page.<br />
			If you would like to customize the session, please enter a title and description
			and click 'Save'.</p>
		<table class="login">
			<tr>
				<td width="25%">
					Session Name:
				</td>
				<td colspan="2">
					<asp:TextBox ID="txtName" runat="server" Width="320px" MaxLength="50" />
				</td>
			</tr>
			<tr>
				<td>
					Status:
				</td>
				<td colspan="2">
					<asp:RadioButton ID="rbStatus1" runat="server" GroupName="rbStatus" Text="In Progress"
						Checked="true" /><span class="toolTip"><a href="#" title="In Progress evaluation can be modified and deleted in the future. Choose this option if you wish to return to this evaluation in the future and modify your responses.">
							<img src="/images/i.gif" alt="" /></a></span>
					<asp:RadioButton ID="rbStatus2" runat="server" GroupName="rbStatus" Text="Complete" /><span
						class="toolTip"><a href="#" title="Complete evaluation cannot be modified. Select this option if you wish to save this session as a snapshot of your current permits.">
							<img src="/images/i.gif" alt="" /></a></span>
				</td>
			</tr>
			<tr>
				<td valign="top">
					Description:
				</td>
				<td colspan="2">
					<asp:TextBox ID="txtDescription" runat="server" Width="320px" MaxLength="50" TextMode="MultiLine"
						Rows="3" />
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td width="110px">	
					<div id="btnEval" class="evalBtnOutline">
						<div class="evalBtn">
							<asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
						</div>
					</div>
				</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
		</table>
	</asp:Panel>
	<asp:Panel ID="pnlSaveConfirmation" runat="server" Visible="false" CssClass="BeforePanel">
		<div class="confirmation">
			<img src="/images/confirm.png" width="16" height="16" alt="Confirm" />
			Current evaluation has been saved successfully.
			<p>
				This evaluation can be accessed later on the <a href="/profile/evaluations.aspx"
					target="_blank">My Evaluations</a> page.</p>
		</div>
	</asp:Panel>
	<asp:Panel ID="pnlLoginRegister" runat="server" Visible="false">
		<div class="saveSummary">
			<p>
				Would you like to save this evaluation? Please <a href="/#login">
					login</a> or <a href="/profile/register.aspx">register</a>.</p>
		</div>
	</asp:Panel>
</div>
<h3>
	Your Responses</h3>
 <div class="evalBtnOutline" style="margin-right: 1em">
	<div class="evalBtn">
		<a href="/permitPlan/PermitPlan.aspx?download=1" class="longDownload">Download Evaluation</a>
	</div>
</div>
<div class="evalBtnOutline" style="margin-right: 1em">
	<div class="evalBtn">
		<a href="/permitPlan/PermitPlan.aspx?print=1" target="_blank">Print Evaluation</a>
	</div>
</div>
<div class="evalBtnOutline" style="margin-right: 1em">
    <div class="evalBtn">
        <a href="/permitPlan/PermitImage.aspx?download=1" target="_blank">Download Permit Schedule</a>
    </div>
</div>
<div class="evalBtnOutline">
    <div class="evalBtn">
        <a href="/permitPlan/PermitImage.aspx?print=1" target="_blank">Print Permit Schedule</a>
    </div>
</div>
<br/><br/>
<div class="floatRight paddingRight30">
	<asp:LinkButton ID="lbRevisitAll" runat="server" OnClick="lbRevisitAll_OnClick" Text="Revisit All" />
	<asp:Literal ID="ltDivider" runat="server" Text="|" />
	<asp:LinkButton ID="lbStartOver" runat="server" OnClick="lbStartOver_OnClick" CssClass="startOver"
		Text="Start Over" />
</div>
    <br />
	<h4>
		Pre-Evaluation</h4>
		<br />
<table class='techWide tech<%= technologyID %> responsesTable' id="responsesTable">
    <thead>
	    <tr>
		    <th class="colHead" width="40%">
			    <b>Question</b>
		    </th>
		    <th class="colHead" width="10%">
			    <b>Response&nbsp;&nbsp;</b>
		    </th>
		    <th class="colHead" width="30%">
			    <b>Note</b>
		    </th>
		    <th class="colHead" width="15%">
			    <b>Permit Sets</b>
		    </th>
		    <th class="colHead" width="5%">
		    </th>
	    </tr>
    </thead>
    <tbody>
	<tr>
		<td class="left">
			<asp:Label ID="lblTechnologyQuestion" runat="server" />
		</td>
		<td>
			<asp:Label ID="lblTechnologyAnswer" runat="server" />
		</td>
        <td></td>
		<td>
			<asp:Repeater ID="rptTechnologyPermitSet" runat="server">
				<ItemTemplate>
					<a class="permitLink" href="/evaluate/permit_set.aspx?id=<%# Eval("ID") %>" target="_blank">
						<%# Eval("Name") %>&nbsp;
					</a>
				</ItemTemplate>
				<SeparatorTemplate>
					<br />
				</SeparatorTemplate>
			</asp:Repeater>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="left">
			<asp:Label ID="lblCapacityQuestion" runat="server" />
		</td>
		<td>
			<asp:Label ID="lblCapacityAnswer" runat="server" />
		</td>
        <td></td>
		<td>
			<asp:Repeater ID="rptCapacityPermitSet" runat="server">
				<ItemTemplate>
					<a class="permitLink" href="/evaluate/permit_set.aspx?id=<%# Eval("ID") %>" target="_blank">
						<%# Eval("Name") %>&nbsp;
					</a>
				</ItemTemplate>
				<SeparatorTemplate>
					<br />
				</SeparatorTemplate>
			</asp:Repeater>
		</td>
		<td>
		</td>
	</tr>
	<asp:Repeater ID="rptCapacityQuestions" runat="server">
		<ItemTemplate>
			<tr>
				<td class="left">
					<%# DisplayQuestion(Container.DataItem) %>
				</td>
				<td>
					<%# DisplayResponse(Container.DataItem) %>
				</td>
                <td></td>
				<td>
					<asp:Repeater ID="rptPermitSet" runat="server" DataSource='<%# DisplayPermitSet(Container.DataItem) %>'>
						<ItemTemplate>
                            <%# DisplayPermitSetUrl(Container.DataItem) %>
						</ItemTemplate>
						<SeparatorTemplate>
							<br />
						</SeparatorTemplate>
					</asp:Repeater>
				</td>
				<td>
				</td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
	<tr>
		<td class="left">
			<asp:Label ID="lblLocationQuestion" runat="server" />
		</td>
		<td>
			<asp:Label ID="lblLocationAnswer" runat="server" />
		</td>
        <td></td>
		<td>
			<asp:Repeater ID="rptLocationPermitSet" runat="server">
				<ItemTemplate>
					<a class="permitLink" href="/evaluate/permit_set.aspx?id=<%# Eval("ID") %>" target="_blank">
						<%# Eval("Name") %>&nbsp;
					</a>
				</ItemTemplate>
				<SeparatorTemplate>
					<br />
				</SeparatorTemplate>
			</asp:Repeater>
		</td>
		<td>
		</td>
	</tr>
	<tr>
		<td class="left">
			<asp:Label ID="lblFederalQuestion" runat="server" />
		</td>
		<td>
			<asp:Label ID="lblFederalAnswer" runat="server" />
		</td>
        <td></td>
		<td>
			<asp:Repeater ID="rptFederalPermitSet" runat="server">
				<ItemTemplate>
					<a class="permitLink" href="/evaluate/permit_set.aspx?id=<%# Eval("ID") %>" target="_blank">
						<%# Eval("Name") %>&nbsp;
					</a>
				</ItemTemplate>
				<SeparatorTemplate>
					<br />
				</SeparatorTemplate>
			</asp:Repeater>
		</td>
		<td>
		</td>
	</tr>
    </tbody>
    </table>
	<br />
    <asp:Repeater runat="server" ID="rptQuestionSet">
        <ItemTemplate>
            <h4><%# DisplayTechnologyName(Container.DataItem) %> <%# Eval("Name") %></h4>  
            <br />
            <table class='techWide tech<%= technologyID %> responsesTable' id="responsesTable">
				<thead>
				<tr>
		            <th class="colHead" width="40%">
			            <b>Question</b>
		            </th>
		            <th class="colHead" width="10%">
			            <b>Response&nbsp;&nbsp;</b>
		            </th>
		            <th class="colHead" width="30%">
			            <b>Note</b>
		            </th>
		            <th class="colHead" width="15%">
			            <b>Permit Sets</b>
		            </th>
		            <th class="colHead" width="5%">
		            </th>
				</tr>
                </thead>
                <tbody>  
	                <asp:Repeater ID="rptAnswers" runat="server" OnItemCommand="rptAnswers_ItemCommand" DataSource='<%# DisplayAnswers(Container.DataItem) %>'>
		                <ItemTemplate>
			                <tr>
				                <td class="left">
					                <%# DisplayQuestion(Container.DataItem) %>
				                </td>
				                <td>
					                <%# DisplayResponse(Container.DataItem) %>
				                </td>
                                <td><%# Eval("Note") %></td>
				                <td>
					                <asp:Repeater ID="rptPermitSet" runat="server" DataSource='<%# DisplayPermitSet(Container.DataItem) %>'>
						                <ItemTemplate>
                                            <%# DisplayPermitSetUrl(Container.DataItem) %>
						                </ItemTemplate>
						                <SeparatorTemplate>
							                <br />
						                </SeparatorTemplate>
					                </asp:Repeater>
				                </td>
				                <td>
					                <asp:LinkButton ID="lbRevisit" runat="server" ToolTip="Revisit" CommandArgument='<%# Eval("QuestionID") %>'
						                Text="Revisit" />
				                </td>
			                </tr>
		                </ItemTemplate>
	             </asp:Repeater>
        </tbody>
        </table>
    </ItemTemplate>
	<SeparatorTemplate>
		<br />
	</SeparatorTemplate>
</asp:Repeater>		
<h3>
	Your Permits</h3>
<asp:Panel ID="pnlPermitNoResults" runat="server" Visible="false">
	<p>Based on your input, no permits are required for your 
	<asp:Label ID="Label1" runat="server" />
	Renewable Energy Project located in 	
	<asp:Label ID="Label2" runat="server" />.</p>
</asp:Panel>
<asp:Panel ID="pnlPermitResults" runat="server">
<p>
	Based on your input, the following permits will be needed for your 
	<asp:Label ID="lblTechnology2" runat="server" />
	Renewable Energy Project located in 	
	<asp:Label ID="lblLocation2" runat="server" />.<br />
	Please mouse over a department name for contact information. Click on the appendix
	number to download the permit packet.</p>
</asp:Panel>	
<table class='techWide tech<%= technologyID %>'>
	<asp:Repeater ID="rptFederalPermits" runat="server" OnItemDataBound="rptPermits_OnItemDataBound">
		<HeaderTemplate>
			<tr>
				<td class="noStyle" colspan="4">
					<h4>
						Federal Permits</h4>
				</td>
			</tr>
			<tr>
				<td class="colHead">
					Permit Name
				</td>
				<td class="colHead nowrap">
					Start - End<br />
					<span class="small">(Months)</span>
				</td>
				<td class="colHead nowrap">
					Duration<br />
					<span class="small">(Months)</span>
				</td>
				<td class="colHead" colspan="2">
					Department
				</td>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td class="left">
                    <a href='<%# Eval("URL") %>' target="_blank">
						<%# Eval("Name") %></a>
				</td>
				<td>
					<%# Eval("StartDuration") %>
					-
					<%# Eval("EndDuration") %>
				</td>
				<td>
					<%# Eval("Duration") %>
				</td>
				<td colspan="2">
					<%# Eval("Department.Name") %><span class="toolTip">
						<asp:HyperLink ID="hlToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
						<span class="toolTipContent" style="display: none">
							<asp:Literal ID="ltToolTip" runat="server" /></span>
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
		</FooterTemplate>
	</asp:Repeater>
	<asp:Repeater ID="rptStatePermits" runat="server" OnItemDataBound="rptPermits_OnItemDataBound">
		<HeaderTemplate>
			<tr>
				<td class="noStyle" colspan="4">
					<h4>
						State Permits</h4>
				</td>
			</tr>
			<tr>
				<td class="colHead">
					Permit Name
				</td>
				<td class="colHead nowrap">
					Start - End<br />
					<span class="small">(Months)</span>
				</td>
				<td class="colHead nowrap">
					Duration<br />
					<span class="small">(Months)</span>
				</td>
				<td class="colHead" colspan="2">
					Department
				</td>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td class="left">
					<a href='<%# Eval("URL") %>' target="_blank">
						<%# Eval("Name") %></a>
				</td>
				<td>
					<%# Eval("StartDuration") %>
					-
					<%# Eval("EndDuration") %>
				</td>
				<td>
					<%# Eval("Duration") %>
				</td>
				<td colspan="2">
					<%# Eval("Department.Name") %><span class="toolTip">
						<asp:HyperLink ID="hlToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
						<span class="toolTipContent" style="display: none">
							<asp:Literal ID="ltToolTip" runat="server" /></span>
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
		</FooterTemplate>
	</asp:Repeater>
	<asp:Repeater ID="rptPermits" runat="server" OnItemDataBound="rptPermits_OnItemDataBound">
		<HeaderTemplate>
			<tr>
				<td class="noStyle" colspan="4">
					<h4>
						County Permits</h4>
				</td>
			</tr>
			<tr>
				<td class="colHead">
					Permit Name
				</td>
				<td class="colHead nowrap">
					Start - End<br />
					<span class="small">(Months)</span>
				</td>
				<td class="colHead nowrap">
					Duration<br />
					<span class="small">(Months)</span>
				</td>
				<td class="colHead" colspan="2">
					Department
				</td>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td class="left">
					<a href='<%# Eval("URL") %>' target="_blank">
						<%# Eval("Name") %></a>
				</td>
				<td>
					<%# Eval("StartDuration") %>
					-
					<%# Eval("EndDuration") %>
				</td>
				<td>
					<%# Eval("Duration") %>
				</td>
				<td colspan="2">
					<%# Eval("Department.Name") %><span class="toolTip">
						<asp:HyperLink ID="hlToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
						<span class="toolTipContent" style="display: none">
							<asp:Literal ID="ltToolTip" runat="server" /></span>
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
</table>
<h3>
	Permit Schedule</h3>
<p>
	Estimated permit sequence and timeline for your 
	<asp:Label ID="lblTechnology" runat="server" />
	Renewable Energy Project located in 
	<asp:Label ID="lblLocation" runat="server" />.</p>
    <asp:Image runat="server" ID="chartImage"/>
<p>
	NOTE: Projects may not require all permits identified in the above results or may
	require additional permits not identified in the results above. The Guide and Wizard
	are not meant to be a substitute for hiring a professional permitting consultant.
	DBEDT strongly recommends you procure your own consultant familiar with these permits
	and approvals to assist you through the permitting process.</p>
<p>
	DBEDT recommends contacting the relevant permitting agencies as a first step to
	beginning all permitting planning and processes. For additional information please
	contact (808) 587-9009.
</p>
<div>
    <br />
    <div class="evalBtnOutline" style="margin-right: 1em">
        <div class="evalBtn">
            <a href="/permitPlan/PermitPlan.aspx" class="longDownload">Download Evaluation</a>
        </div>
    </div>
    <div class="evalBtnOutline" style="margin-right: 1em">
        <div class="evalBtn">
            <a href="/permitPlan/PermitPlan.aspx?print=1" target="_blank">Print Evaluation</a>
        </div>
    </div>
    <div class="evalBtnOutline" style="margin-right: 1em">
        <div class="evalBtn">
            <a href="/permitPlan/PermitImage.aspx?download=1" target="_blank">Download Permit Schedule</a>
        </div>
    </div>
    <div class="evalBtnOutline">
        <div class="evalBtn">
            <a href="/permitPlan/PermitImage.aspx" target="_blank">Print Permit Schedule</a>
        </div>
    </div>
<br style="clear: both"/>
	<asp:LinkButton ID="lbStartOver2" runat="server" OnClick="lbStartOver_OnClick" CssClass="startOver"
		Text="Start Over" />
</div>
<div id="downloadMsg" style="display: none">
        <p align="center">
            <img src="/images/spinner.gif" alt="spinning wheel"/>
        </p>
    </div>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $('#downloadMsg').dialog({
            autoOpen: false,
            width: 400,
            height: 200,
            maxHeight: 200,
            modal: true,
            open: function (event, ui) {
                $('body').css('overflow', 'hidden');
            },
            close: function (event, ui) {
                $('body').css('overflow', 'visible');
            }
        });
        
        $('.responsesTable').dataTable({ "bPaginate": false, "bFilter": false, "bInfo": false });
        HIC.trHighlighter("I don't know", "Review the tooltip or guidance packet for any \"I don't know\" answers highlighted in red.");
        $(".longDownload").click(function () {
            $("#downloadMsg").dialog("open");
            $.fileDownload($(this).prop('href'), {
                successCallback: function (url) {
                    $("#downloadMsg").dialog('close');
                },
                failCallback: function (responseHtml, url) {
                    $("#downloadMsg").dialog('close');
                }
            });
            return false; //this is critical to stop the click event which will trigger a normal file download!
        });
        $('.toolTip > a').click(function () { return false; });
    });
</script>