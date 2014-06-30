<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="pre_evaluation.ascx.cs"
	Inherits="HawaiiDBEDT.Web.Controls.PreEvaluationControl" %>
<%@ Import Namespace="Westwind.Web.Utilities" %>
<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>
<script type="text/javascript">
    $(document).ready(function () {
        $('.navigationButton:enabled').addClass('navigationActive');
		validatePage();
		refreshSubQuestions();
		refreshEndPoints();
	});
</script>
<div id="evaluation">
	<h2 class="preEval">
		1. Technology
	</h2>
	<div>
		<table class="chooseTech responses">
			<tr>
				<td class="noStyle" colspan="2" height="20">
					<b>
						<asp:Label ID="lblTechnologyQuestion" runat="server" /></b> <span class="toolTip">
							<asp:HyperLink ID="hlTechnologyToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
					<span class="toolTipContent" style="display: none">
						<asp:Literal ID="ltTechnologyToolTip" runat="server" /></span>
				</td>
			</tr>
			<asp:Repeater ID="rptTechnology" runat="server" OnItemDataBound="rptTechnology_OnItemDataBound">
				<ItemTemplate>
					<%# (Container.ItemIndex % 2 == 0) ? "<tr>" : "" %>
					<td class="tech<%# Eval("ID") %>">
						<asp:RadioButton ID="rbTechnology" GroupName="technology" runat="server" technologyID='<%# Eval("ID") %>'
							onclick="SetUniqueRadioButton('rptTechnology', this);validatePage();" Text='<%# Eval("Name") %>' />
						<%# (Container.ItemIndex % 2 == 1) ? "</tr>" : "" %>
				</ItemTemplate>
			</asp:Repeater>
		</table>
		<br />
		<h2 class="preEval">
		2. Capacity
		</h2>
		<table class="chooseIsland responses" id="capacityTable">
			<tr>
				<td class="noStyle" colspan="2" height="20">
					<b>
						<asp:Label ID="lblCapacityQuestion" runat="server" /></b> <span class="toolTip">
							<asp:HyperLink ID="hlCapacityQuestion" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
					<span class="toolTipContent" style="display: none">
						<asp:Literal ID="ltCapacityToolTip" runat="server" /></span>
					<input type="hidden" id="capacityResponseID" runat="server" value="default" class="rID" />
					<input id="capacityIsEndPoint" type="hidden" runat="server" value="default" class="rIsEndPoint" />
				</td>
			</tr>
			<tr>
				<asp:Repeater ID="rptResponses" runat="server" OnItemDataBound="rptResponses_ItemDataBound">
					<ItemTemplate>
						<%# (Container.ItemIndex % 2 == 0 && Container.ItemIndex >= 2) ? "</tr><tr>" : ""%>
						<td id="R<%# Eval("ID") %>">
							<asp:RadioButton ID="rbResponse" runat="server" GroupName='<%# Eval("QuestionGroupName") %>'
								responseID='<%# Eval("ID") %>' class="rbCapacityResponse" QuestionTypeID="4" />
							<input type="hidden" class="responseID" value='<%# Eval("ID") %>' runat="server" />
							<input id="Hidden1" type="hidden" class="isEndPoint" value='<%# Eval("IsEndPoint") %>'
								runat="server" />
							<span class="end_message" style="display: none">
								<%# HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(ExplainEndpoint(Convert.ToString(Eval("EndPointMessage")))))%></span>
							<asp:Repeater ID="rptPermitSets" runat="server" DataSource='<%# DisplayRequiredPermitSets(Container.DataItem) %>'>
								<HeaderTemplate>
									<div class="responsePermitSets" style="display: none">
								</HeaderTemplate>
								<ItemTemplate>
									<span class="permitSetID">
										<%# Eval("ID") %></span><span class="permitSetName"><%# Eval("Name") %></span>
								</ItemTemplate>
								<FooterTemplate>
									</div>
								</FooterTemplate>
							</asp:Repeater>
							<%# Eval("Name") %>
							<span class="toolTip">
								<asp:HyperLink ID="hlToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
							<span class="toolTipContent" style="display: none">
								<asp:Literal ID="ltToolTip" runat="server" /></span>
						</td>
					</ItemTemplate>
					<FooterTemplate>
						<asp:Literal ID="closingTDs" runat="server" />
					</FooterTemplate>
				</asp:Repeater>
			</tr>
		</table>
		<asp:Repeater ID="rptResponses2" runat="server">
			<ItemTemplate>
				<div id="subQuestion<%# Eval("ID") %>" class="capacitySubQuestions" style="<%# DisplayReponseStyle(Container.DataItem)%>">
				<input type="hidden" id="currentResponseID" runat="server" value='<%# Eval("ID") %>' />
				<asp:Repeater ID="rptSubQuestions" runat="server" OnItemDataBound="rptSubQuestions_ItemDataBound"
					DataSource='<%# DisplaySubQuestions(Container.DataItem) %>'>
					<ItemTemplate>
						<table class="chooseIsland responses" id="capacitySubQuestions<%# Eval("ID") %>">
							<tr>
								<td class="noStyle" colspan="2" height="20">
									<b>
										<%# Eval("Name") %></b> <span class="toolTip">
											<asp:HyperLink ID="hlToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
									<span class="toolTipContent" style="display: none">
										<asp:Literal ID="ltToolTip" runat="server" /></span>
									<input type="hidden" id="subQID" runat="server" value='<%# Eval("ID") %>' />
									<input type="hidden" id="subRID" runat="server" value="default" class="subResponseID" />
									<input id="Hidden3" type="hidden" runat="server" value="default" class="subResponseIsEndPoint" />
								</td>
							</tr>
							<tr>
								<asp:Repeater ID="Repeater1" runat="server" DataSource='<%# DisplaySubResponses(Container.DataItem) %>'
									OnItemDataBound="rptSubResponses_ItemDataBound">
									<ItemTemplate>
										<%# (Container.ItemIndex % 2 == 0 && Container.ItemIndex >= 2) ? "</tr><tr>" : ""%>
										<td>
											<asp:RadioButton ID="rbResponse" runat="server" GroupName='<%# Eval("QuestionGroupName") %>'
												responseID='<%# Eval("ID") %>' />
											<input id="Hidden2" type="hidden" class="responseID" value='<%# Eval("ID") %>' runat="server" />
											<input id="Hidden1" type="hidden" class="isEndPoint" value='<%# Eval("IsEndPoint") %>'
												runat="server" />
											<span class="end_message" style="display: none">
												<%# HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(ExplainEndpoint(Convert.ToString(Eval("EndPointMessage")))))%></span>
											<asp:Repeater ID="rptPermitSets" runat="server" DataSource='<%# DisplayRequiredPermitSets(Container.DataItem) %>'>
												<HeaderTemplate>
													<div class="responsePermitSets" style="display: none">
												</HeaderTemplate>
												<ItemTemplate>
													<li><a class="permitLink" href="/evaluate/permit_set.aspx?id=<%# Eval("ID") %>" target="_blank">
														<%# Eval("Name") %></a></li>
												</ItemTemplate>
												<FooterTemplate>
													</div>
												</FooterTemplate>
											</asp:Repeater>
											<%# Eval("Name") %><span class="toolTip">
												<asp:HyperLink ID="hlToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
											<span class="toolTipContent" style="display: none">
												<asp:Literal ID="ltToolTip" runat="server" /></span>
										</td>
									</ItemTemplate>
									<FooterTemplate>
										<asp:Literal ID="closingTDs" runat="server" />
									</FooterTemplate>
								</asp:Repeater>
							</tr>
						</table>
					</ItemTemplate>
				</asp:Repeater>
				</div>
			</ItemTemplate>
		</asp:Repeater>
		<table class="chooseTech">
			<tr class="end_point inactive" id="capacityEndPointRow">
				<td class="noStyle">
				</td>
				<td class="noStyle">
					<div class="error">
					</div>
				</td>
			</tr>
		</table>
	</div>
	<h2 class="preEval">
		3. Location
	</h2>
	<div>
		<table class="chooseIsland responses">
			<tr>
				<td class="noStyle" colspan="2">
					<b>
						<asp:Label ID="lblLocationQuestion" runat="server" /></b> <span class="toolTip">
							<asp:HyperLink ID="hlLocationQuestion" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
					<span class="toolTipContent" style="display: none">
						<asp:Literal ID="ltLocationToolTip" runat="server" /></span>
				</td>
			</tr>
			<asp:Repeater ID="rptLocation" runat="server" OnItemDataBound="rptLocation_OnItemDataBound">
				<ItemTemplate>
					<%# (Container.ItemIndex % 2 == 0) ? "<tr>" : "" %>
					<td>
						<asp:RadioButton ID="rbLocation" GroupName="location" runat="server" locationID='<%# Eval("ID") %>'
							onclick="SetUniqueRadioButton('rptLocation', this);validatePage();" Text='<%# Eval("Name") %>' />
						<%# (Container.ItemIndex % 2 == 1) ? "</tr>" : "" %>
				</ItemTemplate>
			</asp:Repeater>
		</table>
		<br />
	</div>
	<div id="btnEval" class="evalBtnOutline floatRight marginRight30">
		<div class="evalBtnInactive">
			<asp:LinkButton ID="lbStartEval" runat="server" Text="Proceed &#187;" OnClientClick="return submitForm()"
				OnClick="lbStartEvaluation_OnClick" ToolTip="Please answer all questions on the page to proceed." />
		</div>
	</div>
	<div id="validateMessage">
		Please answer all questions on the page to proceed.
	</div>
</div>
<script type="text/javascript">
	function ToggleStopMessage(action) {
		if (action == 'show') {
			deactivateNextButton();
			$("#validateMessage").hide();
			$("#stopMessage").html('<p>Your project cannot be classified as a renewable project.</p>');
		}
		else {
			//$("#btnEval").show();
			$("#validateMessage").show();
			$("#stopMessage").html('&nbsp;');
		}
		return false;
	}

	function submitForm() {
		if ($("#<%= lbStartEval.ClientID %>").parent().attr("class") == "evalBtn") {
			return true;
		}
		return false;
	}

	function activateNextButton() {
		$("#<%= lbStartEval.ClientID %>").attr('title', 'Start Evaluation').removeAttr("disabled");
		$("#<%= lbStartEval.ClientID %>").parent('div').attr('class', 'evalBtn');
		$("#validateMessage").html('&nbsp;');
	}

	function deactivateNextButton() {
		$("#<%= lbStartEval.ClientID %>").parent('div').attr('class', 'evalBtnInactive');
		$("#<%= lbStartEval.ClientID %>").attr("disabled", "disabled").attr('title', 'Please answer all questions on the page to proceed.');
		$("#validateMessage").html('Please answer all questions on the page to proceed.');
	}

	function validatePage() {
		if (technologySelected() && locationSelected() && capacitySelected()) {
		    activateNextButton();
		    $('.navigationButton.navigationActive').attr('disabled', '');
		}
		else {
		    deactivateNextButton();
		    $('.navigationButton').attr('disabled', 'disabled');
		}
		return false;
	}

	function technologySelected() {

		var radioButtons = document.getElementsByTagName('input');
		for (var i = 0; i < radioButtons.length; ++i) {
			if (radioButtons[i].type == 'radio' && radioButtons[i].name.indexOf('rptTechnology') >= 0) {
				if (radioButtons[i].checked) {
					return true;
				}
			}
		}
		return false;
	}

	function locationSelected() {

		var radioButtons = document.getElementsByTagName('input');
		for (var i = 0; i < radioButtons.length; ++i) {
			if (radioButtons[i].type == 'radio' && radioButtons[i].name.indexOf('rptLocation') >= 0) {
				if (radioButtons[i].checked) {

					return true;
				}
			}
		}
		return false;
	}
	
	function SetUniqueRadioButton(nameregex, current) {
		re = new RegExp(nameregex);
		for (i = 0; i < document.forms[0].elements.length; i++) {
			elm = document.forms[0].elements[i]
			if (elm.type == 'radio') {
				if (re.test(elm.name)) {
					elm.checked = false;
				}
			}
		}
		current.checked = true;
	}

	$(function () {
		$(".rbCapacityResponse").find(':radio').live('click', function (event) {
			var $item = $(this).parent('span');
			var responseID = $item.parent('td').find('.responseID').val();
			$('#<%= capacityResponseID.ClientID %>').val(responseID);

			// Is end point?
			if ($item.parent('td').find('.isEndPoint').val() == 'True') {
				$('#capacityEndPointRow').find('.error').html('<div>' + $item.parent('td').find('.end_message').html() + '</div>');
				$('#capacityEndPointRow').attr('class', 'end_point');
				$('#capacityEndPointRow').attr('style', '');

				$('#<%= capacityIsEndPoint.ClientID %>').val('True');
			}
			else {
				$('#capacityEndPointRow').attr('class', 'end_point inactive');
				$('#<%= capacityIsEndPoint.ClientID %>').val('False');
			}

			var subQuestionDivID = 'subQuestion' + responseID;

			$('.capacitySubQuestions').each(function (i) {
				if ($(this).attr('id') != subQuestionDivID) {
					$(this).attr('style', 'display:none');
					$(this).find('.subResponseID').val('default');
					// reset the value
					$(this).find(':radio').removeAttr('checked');
				}
			});

			$('#' + subQuestionDivID).attr('style', 'display:block');

			validatePage();
		});
	});

	$(function () {
		$(".capacitySubQuestions").find(':radio').live('click', function (event) {
			var $item = $(this).parent('span');
			var responseID = $item.parent('td').find('.responseID').val();

			$item.parents('table').find('.subResponseID').val(responseID);

			// Is end point?
			if ($item.parent('td').find('.isEndPoint').val() == 'True') {
				$('#capacityEndPointRow').find('.error').html('<div>' + $item.parent('td').find('.end_message').html() + '</div>');
				$('#capacityEndPointRow').attr('class', 'end_point');
				$('#capacityEndPointRow').attr('style', '');

				$item.parent('table').find('.subResponseIsEndPoint').val('True');
			}
			else {
				$('#capacityEndPointRow').attr('class', 'end_point inactive');
				$item.parent('table').find('.subResponseIsEndPoint').val('False');
			}

			validatePage();
		});
	});

	function capacitySelected() {

		var allAnswered = true;
		var response_value = $('#<%= capacityResponseID.ClientID %>').val();
		var response_end_point = $('#<%= capacityIsEndPoint.ClientID %>').val();

		if (response_value == 'default' || response_value == '' || response_end_point == 'True') {
			allAnswered = false;
			return false;
		}
		else {
			var subQuestionDivID = 'subQuestion' + response_value;

			$('#' + subQuestionDivID).find('.subResponseID').each(function (i) {

				if ($(this).val() == '' || $(this).val() == 'default' || $(this).find('.subResponseIsEndPoint').val() == 'True') {
					//alert('361');
					allAnswered = false;
					return false;
				}
			});
		}
		
		return allAnswered;
	}

	function refreshEndPoints() {

	}

	function refreshSubQuestions() {

		var responseID = $('#<%= capacityResponseID.ClientID %>').val();
		if (responseID == 'default' || responseID == '') {
			responseID = '<%= CurrentEvaluation.CapacityID %>';
		}

		var subQuestionDivID = 'subQuestion' + responseID;
		$('#' + subQuestionDivID).attr('style', 'display:block');
	}
</script>
