<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="review.aspx.cs" Inherits="HawaiiDBEDT.Web.evaluate.review" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	
	<title>Renewable Energy Facility Permits in the State of Hawaii</title>
	<link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
	<link rel="stylesheet" media="screen" href="/include/controls/radcontrols/css/combobox.css"
		type="text/css" />
	<link rel="stylesheet" media="print" href="/include/css/print.css" type="text/css" />
	<script type="text/javascript" src="/include/js/javascript.js"></script>
    <script type="text/javascript" src="/include/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="/include/js/jquery-ui.min.js"></script>
	<script type="text/javascript" src="/include/js/jquery.qtip.pack.js"></script>
    <script type="text/javascript" src="/include/js/downloadPlugin.js"></script>
    <script type="text/javascript" >
        showSessionModal = '<% = IsLoggedIn %>';
    </script>
	<link rel="stylesheet" media="screen" href="/include/css/jquery.qtip.css" type="text/css" />
    <link rel="stylesheet" media="screen" href="/include/css/jquery-ui.css" type="text/css" />
    <style>
        .ui-dialog {
            background-color: transparent;
            border-color: transparent;
        }
        .ui-dialog-titlebar {
            display: none;
        }
    </style>
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
					<div class="right">
						<asp:LinkButton ID="lbStartOver2" runat="server" OnClick="lbStartOver_OnClick" CssClass="startOver"
							Text="Start a New Session &gt;&gt;" />
					</div>
					<h3>
						Session Details</h3>
					<p>
						<span class="bold">Evaluation Date: </span>
						<asp:Label ID="lblDate" runat="server" /></p>
					<asp:Panel ID="pnlTitle" Visible="false" runat="server">
						<p>
							<span class="bold">Title: </span>
							<%= evaluation.Name %></p>
					</asp:Panel>
					<asp:Panel ID="pnlDescription" Visible="false" runat="server">
						<p>
							<span class="bold">Description: </span>
							<%= evaluation.Description%></p>
					</asp:Panel>

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
                    <div><div class="BeforePanel">&nbsp;</div></div>
					<h3>
						Your Responses</h3>
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
						<th class="colHead" width="35%">
							<b>Note</b>
						</th>
						<th class="colHead" width="15%">
							<b>Permit Sets</b>
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
										<%# Eval("Name") %>
									</a>
								</ItemTemplate>
								<SeparatorTemplate>
									<br />
								</SeparatorTemplate>
							</asp:Repeater>
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
										<%# Eval("Name") %>
									</a>
								</ItemTemplate>
								<SeparatorTemplate>
									<br />
								</SeparatorTemplate>
							</asp:Repeater>
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
										<%# Eval("Name") %>
									</a>
								</ItemTemplate>
								<SeparatorTemplate>
									<br />
								</SeparatorTemplate>
							</asp:Repeater>
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
						            <th class="colHead" width="35%">
							            <b>Note</b>
						            </th>
						            <th class="colHead" width="15%">
							            <b>Permit Sets</b>
						            </th>
					            </tr>
                             </thead>
                             <tbody>                                                     
                            <asp:Repeater ID="rptAnswers" runat="server" DataSource='<%# DisplayAnswers(Container.DataItem) %>'>
						        <ItemTemplate>
							        <tr>
								        <td class="left">
									       <%# DisplayQuestion(Container.DataItem) %>
								        </td>
								        <td>
									        <%# DisplayResponse(Container.DataItem) %>
								        </td>
                                        <td>
                                            <%# Eval("Note") %>
                                        </td>
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
					<p>
						Based on your input, the following permits will be needed for your
						<asp:Label ID="lblTechnology2" runat="server" />
						Renewable Energy Project located in
						<asp:Label ID="lblLocation2" runat="server" />.<br />
						Please mouse over a department name for contact information. Click on the appendix
						number to download the permit packet.</p>
					
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
						contact (808) 587-9009 or leave feedback using the <a href="http://forms.hawaiicleanenergyinitiative.org/view.php?id=23" title="Renewable Energy Permitting Wizard Feedback">feedback survey</a>.
					</p>
                        <div class="evalBtnOutline" style="margin-right: 1em">
                            <div class="evalBtn">
                                <a href="/permitPlan/PermitPlan.aspx">Download Evaluation</a>
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
                    <script type="text/javascript">
                        // Create the tooltips only on document load
                        $(document).ready(function () {
                            $('.toolTip > a').each(function () {
                                $(this).qtip({
                                    content: {
                                        title: {
                                            text: true,
                                            button: 'Close'
                                        },
                                        text: $(this).parent().next('.toolTipContent') //
                                    },
                                    position: {
                                        viewport: $(window)
                                    },
                                    hide: {
                                        fixed: true, // Make it fixed so it can be hovered over
                                        delay: 200,
                                        when: { event: 'click' }
                                    }
                                });
                            });
                            $('.toolTip > a').click(function() { return false; });
                        });
                    </script>
					</form>
				</div>
			</div>
			<!-- BEGIN FOOTER -->
			<!-- #include virtual="~/include/footer.aspx" -->
			<!-- END FOOTER -->
		</div>
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
                close: function(event, ui) {
                    $('body').css('overflow','visible');
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
        });
    </script>
</body>
</html>
