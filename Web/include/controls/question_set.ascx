<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="question_set.ascx.cs"
	Inherits="HawaiiDBEDT.Web.Controls.QuestionSetControl" %>
<%@ Import Namespace="HawaiiDBEDT.Domain" %>
<%@ Import Namespace="Westwind.Web.Utilities" %>
<%@ Import Namespace="HawaiiDBEDT.Web.utils" %>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $(".tech").find("input.rID").each(function () {
            if ($(this).val() == 'default') {
                $(this).val('');
            }
        });

        $('.navigationButton:enabled').addClass('navigationActive');
        refreshEndPoints();
        refreshPermitSets();
        $('#permitSetList').jScrollPane();
        validatePage();

        $('.noteModal').unbind().click(showNoteModal);
        $('.noteModal').css('cursor', 'pointer');
    });

    function showNoteModal() {
        var questionId = $(this).attr('questionId');
        $('#noteDialog').dialog({
            title: 'Add a note',
            modal: true,
            width: "350px",
            open: function () {
                $('#questionId', this).val(questionId);
                var noteText = '';
                if ($('.qId[value="' + questionId + '"]').length > 0) {
                    noteText = $('.qId[value="' + questionId + '"]').siblings('.noteText').val();
                } else if ($('.subQID[value="' + questionId + '"]').length > 0) {
                    noteText = $('.subQID[value="' + questionId + '"]').siblings('.noteText').val();
                }
                $('#noteContent', this).val(noteText);
            },
            buttons: [
              {
                  text: 'Save',
                  click: function () {
                      var questionId = $('#questionId', this).val();
                      var noteText = $('#noteContent', this).val();
                      if ($('.qId[value="' + questionId + '"]').length > 0) {
                          $('.qId[value="' + questionId + '"]').siblings('.noteText').val(noteText);
                      } else if ($('.subQID[value="' + questionId + '"]').length > 0) {
                          $('.subQID[value="' + questionId + '"]').siblings('.noteText').val(noteText);
                      }
                      $(this).dialog("close");
                  }
              },
            {
                text: 'Cancel',
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
        });
    }

	$(function () {
		var offset = $("#floating-box").offset();
		var topPadding = 15;
		$(window).scroll(function () {
			if ($(window).scrollTop() > offset.top) {
				$("#floating-box").stop().animate({ marginTop: $(window).scrollTop() - offset.top + topPadding });
			}
			else {
				$("#floating-box").stop().animate({ marginTop: 0 });
			}
		});
	});

</script>
<div id="noteDialog" style="display: none; height: auto" class="noteDialog" >
    <div style="height: 100px">
    <textarea id="noteContent" rows="4" cols="48"></textarea>
    <input type="hidden" id="questionId"/>
    </div>
</div>
<br />
<h2>
	<span class="tech<%= technologyID %>">
		<%= technologyName %></span>
	<%= questionSetName %></h2>
<div id="evaluation">
	<div id="floating-box" class="permitSets" style="display: none">
			<p>Required <strong><span class="tech<%= technologyID %>">
				<%= technologyName %></span></strong> Permit Sets</p>
				<div id="permitSetList">
				<ul id="requiredPermit">
				</ul>
				</div>
	</div>
	<div>
	<div class="eval">
		<table class="tech tech<%= technologyID %>">
			<asp:Repeater ID="rptRecords" runat="server" OnItemDataBound="rptRecords_ItemDataBound">
				<ItemTemplate>
					<tr class="qRow" id="Q<%# Eval("ID") %>">
						<td width="5%" class="left">
							<%# (Container.ItemIndex + 1) %>
						</td>
						<td width="35%">
							<%# Eval("Name") %>
							<span class="toolTip">
								<asp:HyperLink ID="hlToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
							<span class="toolTipContent" style="display: none">
								<asp:Literal ID="ltToolTip" runat="server" /></span>
							<input type="hidden" id="qID" runat="server" value='<%# Eval("ID") %>' class="qId" />
							<input type="hidden" id="rID" runat="server" value="default" class="rID" />
                            <input type="hidden" id="noteText" runat="server" value='' class="noteText" />
							<input type="hidden" runat="server" value="default" class="rIsEndPoint" />
						</td>
						<td class="response" width="20%">
							<table class="responses">
								<tr>
									<asp:Repeater ID="rptResponses" runat="server" DataSource='<%# DisplayResponses(Container.DataItem) %>'
										OnItemDataBound="rptResponses_ItemDataBound">
										<ItemTemplate>
											<%# (Container.ItemIndex % 2 == 0 && Container.ItemIndex >= 2) ? "</tr><tr>" : ""%>
											<td class="<%# (Container.ItemIndex % 2 == 0) ? "left" : "right"%>1" id="R<%# Eval("ID") %>">
												<asp:RadioButton ID="rbResponse" runat="server" GroupName='<%# Eval("QuestionGroupName") %>'
													responseID='<%# Eval("ID") %>' />
												<input id="Hidden2" type="hidden" class="subQID" value='<%# Eval("SubQuestionID") %>'
													runat="server" />
												<input id="Hidden1" type="hidden" class="isEndPoint" value='<%# Eval("IsEndPoint") %>'
													runat="server" />
												<span class="end_message" style="display: none">
													<%# HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(ExplainEndpoint(Convert.ToString(Eval("EndPointMessage")))))%></span>
											</td>
											<td class="<%# (Container.ItemIndex % 2 == 0) ? "left" : "right"%>2">
												<%# Eval("Name") %>
												<span class="toolTip">
													<asp:HyperLink ID="hlToolTip" runat="server" ><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
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
						</td>
                        <td width="35%" class="psContainer">
                            <asp:Repeater runat="server" DataSource='<%# ((Question)Container.DataItem).Responses %>'>
                                <ItemTemplate>
                                    <div id="R<%# ((Response)Container.DataItem).ID %>" style="display: none">
                            <asp:Repeater ID="rptPermitSets" runat="server" DataSource='<%# DisplayRequiredPermitSets(Container.DataItem) %>'>
								<HeaderTemplate>
                                        <ul>
								</HeaderTemplate>
								<ItemTemplate>
									<li><%# DisplayPermitSetUrl(Container.DataItem) %></li>
								</ItemTemplate>
								<FooterTemplate>
									</ul>
								</FooterTemplate>
							</asp:Repeater>
                                        </div>
                                    </ItemTemplate>
                            </asp:Repeater>
                        </td>
                        <td width="5%" style="align-content: center" align="center">
                            <div class="noteModal" questionId='<%# Eval("ID") %>'><img src="/images/pencil.gif" alt="" /></div>
                        </td>
					</tr>
					<tr class="end_point inactive">
						<td colspan="2" class="noStyle"></td>
						<td colspan="3" class="noStyle">
							<div class="error"></div>
						</td>
					</tr>
					<asp:Repeater ID="rptSubQuestions" runat="server" DataSource='<%# DisplaySubQuestions(Container.DataItem) %>'
						OnItemDataBound="rptRecords_ItemDataBound">
						<ItemTemplate>
							<tr class="<%# DisplaySubQuestionClass(Container.DataItem)%>" id="subQ<%# Eval("ID") %>">
								<td class="left">
									<%# ((RepeaterItem)Container.Parent.Parent).ItemIndex + 1 %>a
								</td>
								<td>
									<%# Eval("Name") %>
									<span class="toolTip">
										<asp:HyperLink ID="hlToolTip" runat="server"><img src="/images/i.gif" alt="" /></asp:HyperLink></span>
									<span class="toolTipContent" style="display: none">
										<asp:Literal ID="ltToolTip" runat="server" /></span>
									<input type="hidden" id="subQID" runat="server" value='<%# Eval("ID") %>' class="subQID"/>
									<input type="hidden" id="subRID" runat="server" value="default" class="rID" />
                                    <input type="hidden" id="noteText" runat="server" value='' class="noteText" />
									<input id="Hidden3" type="hidden" runat="server" value="default" class="rIsEndPoint" />
								</td>
								<td class="response">
									<table class="responses">
										<tr>
											<asp:Repeater ID="Repeater1" runat="server" DataSource='<%# DisplaySubResponses(Container.DataItem) %>'
												OnItemDataBound="rptResponses_ItemDataBound">
												<ItemTemplate>
													<%# (Container.ItemIndex % 2 == 0 && Container.ItemIndex >= 2) ? "</tr><tr>" : ""%>
													<td class="<%# (Container.ItemIndex % 2 == 0) ? "left" : "right"%>1" id="R<%# Eval("ID") %>">
														<asp:RadioButton ID="rbResponse" runat="server" GroupName='<%# Eval("QuestionGroupName") %>'
															responseID='<%# Eval("ID") %>' />
														<input id="Hidden1" type="hidden" class="isEndPoint" value='<%# Eval("IsEndPoint") %>'
															runat="server" />
														<span class="end_message" style="display: none">
															<%# HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(ExplainEndpoint(Convert.ToString(Eval("EndPointMessage")))))%></span>
													</td>
													<td class="<%# (Container.ItemIndex % 2 == 0) ? "left" : "right"%>2">
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
								</td>
                                <td>
                                   <%-- <asp:Repeater ID="rptPermitSets" runat="server" DataSource='<%# DisplayRequiredPermitSets(Container.DataItem) %>'>
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
									</asp:Repeater>--%>
                                </td>
                                <td>
                                    <div class="noteModal" questionId='<%# Eval("ID") %>'><img src="/images/pencil.gif" alt="" /></div>
                                </td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
					<tr class="end_point inactive">
						<td colspan="2" class="noStyle"></td>
						<td colspan="3" class="noStyle" align="right">
							<span class="error floatRight"></span>
						</td>
					</tr>
				</ItemTemplate>
			</asp:Repeater>
		</table>
		<div id="evaluateFooter">
			<div id="btnBack" class="evalBtnOutline floatLeft">
				<div class="evalBtn">
					<asp:LinkButton ID="lbBack" runat="server" Text="&#171; Back" OnClick="lbBack_OnClick" />
				</div>
			</div>
			<div id="btnNext" class="evalBtnOutline floatRight">
				<div class="evalBtnInactive" id="divNext" runat="server">
					<asp:LinkButton ID="lbNext" runat="server" Text="Next &#187;" OnClientClick="return submitForm()"
						OnClick="lbNext_OnClick" ToolTip="Please answer all questions on the page to proceed." />
				</div>
			</div>
			<div id="validateMessage">
					Please answer all questions on the page to proceed.
			</div>
		</div>
		
<% if (CurrentUserID != 0)
    { %>
	<div id="saveMessage" class="smText padTop5">
		<p>
			Your answers are being saved as you advance. You may exit at any time and return
			to this evaluation.</p>
	</div>
	<% } %>
	</div>
</div>	
</div>
<input type="hidden" id="hfExistingValue" runat="server" value="0" />
<script language="javascript" type="text/javascript">
	 var myExistingPermitSets = {<%= existingPermitSetArray %>};
	 var myPermitSets = {<%= permitSetArray %>};

	function submitForm() {
		if ($("#<%= lbNext.ClientID %>").attr("disabled") == "disabled") {
			return false;
		}
	    deactivateNextButton();
		return true;
	}
	$(function () {
		$(":radio").live('click', function (event) {
			var $item = $(this);
			if ($item.parents('.qRow').attr('id') != undefined) {
				$item.parents('.qRow').find('.rID').val($item.parent().attr('responseID'));

				// Is end point?
				if ($item.parents('td').find('.isEndPoint').val() == 'True') {
					//$item.parents('.qRow').next('.end_point').css('display', 'inline');
					$item.parents('.qRow').next('.end_point').find('.error').html('<div>' + $item.parents('td').find('.end_message').html() + '</div>');
					$item.parents('.qRow').next('.end_point').attr('class', 'end_point');
					$item.parents('.qRow').next('.end_point').attr('style', '');
					$item.parents('.qRow').find('.rIsEndPoint').val('True');
				}
				else {
					$item.parents('.qRow').next('.end_point').attr('class', 'end_point inactive');
					$item.parents('.qRow').find('.rIsEndPoint').val('False');
				}

				// Hide any nested question for other responses

			    var subquestion = $item.parents('.qRow').nextAll('tr[id^="subQ"]').first();
				if (subquestion != undefined && subquestion.hasClass('qActiveNestedRow')) {
				    subquestion.attr('class', 'qNestedRow');
				    subquestion.attr('style', '');
				    subquestion.find('.rID').val('default');
				    subquestion.find('input[type="radio"]').each(function() {
				        $(this).attr('checked', '');
				    });
				}
			    
				// Show any nested question for this response
				var subQID = "#subQ" + $item.parents('td').find('.subQID').val();
				if (subQID != "#subQ") {
				    $item.parents('.qRow').siblings(subQID).show().attr('class', 'qActiveNestedRow');
				} 
			}
			else { // nested response
				$item.parents('.qActiveNestedRow').find('.rID').val($item.parent().attr('responseID'));
			}
			validatePage();
		});
	});

	function activateNextButton() {
		$("#<%= lbNext.ClientID %>").attr('title', 'Start Evaluation').removeAttr("disabled");
		$("#<%= lbNext.ClientID %>").parent('div').attr('class', 'evalBtn');
		$("#validateMessage").html('&nbsp;');
	}

	function deactivateNextButton() {
		$("#<%= lbNext.ClientID %>").parent('div').attr('class', 'evalBtnInactive');
		$("#<%= lbNext.ClientID %>").attr("disabled", "disabled").attr('title', 'Please answer all questions on the page to proceed.');
		//$("#validateMessage").html('Please answer all questions on the page to proceed.');
	}

	function displayRequiredPermits() {
		myPermitSets = {};

		$('.qRow').each(function () {
		    var selectedResponseID = "#R" + $(this).find('.rID').val();
		    if (selectedResponseID != undefined && selectedResponseID != '#R') {
		        $(this).find(".psContainer > div").css('display', 'none');
		        $(this).find(".psContainer").find(selectedResponseID).show();
			}
		});
		$('.qActiveNestedRow').each(function () {
			var selectedResponseID = "#R" + $(this).find('.rID').val();

			if (selectedResponseID != undefined && selectedResponseID != '#R') {	
				$(this).find(selectedResponseID).find('.responsePermitSets').find('.permitSetID').each(function() {
					if (myPermitSets[$(this).html()] == undefined && myExistingPermitSets[$(this).html()] == undefined) {
						myPermitSets[$(this).html()] = $(this).next('.permitSetName').html();
					}
				});
			}
		});
															    
		refreshPermitSets();
	}	  

	function refreshPermitSets() {
		$("ul#requiredPermit").html('');
		var permitCount = 0;
		$.each(myExistingPermitSets, function(key, val) {
			$("ul#requiredPermit").append("<li><a class=\"permitLink\" href=\"/evaluate/permit_set.aspx?id=" + key + "\" target=\"_blank\">" + val + "</a></li>");
			permitCount++;
		});		  
		$.each(myPermitSets, function(key, val) {
			$("ul#requiredPermit").append("<li><a class=\"permitLink\" href=\"/evaluate/permit_set.aspx?id=" + key + "\" target=\"_blank\">" + val + "</a></li>");
			permitCount++;
		});
		
		$("ul#requiredPermit").children(':last').attr("class", "lastItem");
		
		$('#permitSetList').jScrollPane();
	}

	function validatePage() {

		displayRequiredPermits();
		if (questionsSelected()) {
			activateNextButton();
		    $('.navigationButton.navigationActive').attr('disabled', '');
		}
		else {
			deactivateNextButton();
		    $('.navigationButton').attr('disabled', 'disabled');
		}
		return false;
	}

	function refreshEndPoints() {
		$('.qRow').each(function () {
			if ($(this).find('.rIsEndPoint').val() == 'True') {
				var response_value = $(this).find('.rID').val();
				var selectedResponseID = "#R" + response_value;
				$(this).next('.end_point').find('.error').html('<p>' + $(this).find(selectedResponseID).find('.end_message').html() + '</p>');
				$(this).next('.end_point').attr('class', 'end_point');
				$(this).next('.end_point').attr('style', '');
			}
		});
		$('.qActiveNestedRow').each(function () { 
			if ($(this).find('.rIsEndPoint').val() == 'True') {
				var response_value = $(this).find('.rID').val();
				var selectedResponseID = "#R" + response_value;
				$(this).next('.end_point').find('.error').html('<p>' + $(this).find(selectedResponseID).find('.end_message').html() + '</p>');
				$(this).next('.end_point').attr('class', 'end_point');
				$(this).next('.end_point').attr('style', '');
			}
		});
	}

	function questionsSelected() {

		var allAnswered = true;
		var index = 0;
		$('.qRow').each(function () {
			var response_value = $(this).find('.rID').val();
			if (response_value == '' || response_value == 'default') {
				allAnswered = false;
				$("#validateMessage").html('Please answer all questions to proceed.');
				return false;
			}
			else if ($(this).find('.rIsEndPoint').val() == 'True') {
				//$("#validateMessage").html('rID not selected ' + index + ' for ' + response_value + '. end point is ' + $(this).find('.rIsEndPoint').val());
				$("#validateMessage").html('Base on your response for question #' + (index + 1) + ', the evaluation cannot continue.');
				allAnswered = false;
				return false;
			}
			index++;
		});
		$('.qActiveNestedRow').each(function () {
			var response_value = $(this).find('.rID').val();
			if (response_value == '' || response_value == 'default' || $(this).find('.rIsEndPoint').val() == 'True') {
				allAnswered = false;
				return false;
			}
		});

		return allAnswered;
	}

	function SetUniqueRadioButton(nameregex, current) {
		re = new RegExp(nameregex);
		for (i = 0; i < document.forms[0].elements.length; i++) {
		    elm = document.forms[0].elements[i];
			if (elm.type == 'radio') {
				if (re.test(elm.name)) {
					elm.checked = false;
				}
			}
		}
		current.checked = true;
	}
</script>
