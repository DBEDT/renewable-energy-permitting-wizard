using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;
using System.Web.UI.HtmlControls;
using System.Text;

namespace HawaiiDBEDT.Web.Controls
{
    using Data.ViewModels;
    using log4net;
    using utils;
    using Westwind.Web.Utilities;

    public partial class QuestionSetControl : EvaluationControl
	{
		private QuestionSet questionSet;
		private static int questionNo = 1;

		protected string technologyName;
		protected int technologyID;
		protected string questionSetName;

		protected string existingPermitSetArray = "";
		protected string permitSetArray = "";

		protected int questionID;
        ILog log = LogManager.GetLogger(typeof(QuestionSetControl));

		private bool? existingValue2;
		public bool existingValue
		{
			get
			{
				if (!existingValue2.HasValue)
				{
					existingValue2 = (hfExistingValue.Value == "true");
				}
				return existingValue2.Value;
			}
			set
			{
				existingValue2 = value;
			}
		}

		public void LoadPage()
		{
            try
            {
                questionSet = CurrentEvaluation.QuestionSets[CurrentEvaluation.PageIndex];

                if (!DisplayExistingValue())
                {
                    lbNext.Attributes.Remove("disabled");
                    lbNext.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    divNext.Attributes.Remove("class");
                    divNext.Attributes.Add("class", "evalBtn");
                    lbNext.Attributes.Remove("disabled");
                }

                questionNo = 1;
                RefreshProgressBar();
                DisplayTechnology();
                DisplayQuestions();
                DisplayExistingPermitSets();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                LoadPreEvaluationPage();
            }
		}

		private void DisplayExistingPermitSets()
		{
			List<PermitSet> existingPermitSets = new List<PermitSet>();
			existingPermitSets.AddRange(Data.TechnologyPermitSet.GetPermitSetsInTechnology(CurrentEvaluation.Technology.ID));
			existingPermitSets.AddRange(Data.LocationPermitSet.GetPermitSetsInLocation(CurrentEvaluation.Location.ID));
			existingPermitSets.AddRange(Data.PreEvaluationResponsePermitSet.GetPermitSetsForPreEvaluationResponse(CurrentEvaluation.CapacityID, CurrentEvaluation.Location.ID)); 
			existingPermitSets.AddRange(Data.PreEvaluationResponsePermitSet.GetPermitSetsForPreEvaluationResponse(CurrentEvaluation.FederalID, CurrentEvaluation.Location.ID));

			foreach (EvaluationResponse answer in CurrentEvaluation.CapacityAnswers)
			{
				existingPermitSets.AddRange(Data.ResponsePermitSet.GetPermitSetsForResponse(answer.ResponseID, CurrentEvaluation.Location.ID));
			}

			List<PermitSet> currentPermitSets = new List<PermitSet>();
			foreach (EvaluationResponse answer in CurrentEvaluation.Answers)
			{
				if (existingValue && answer.Page == CurrentEvaluation.CurrentPage)
				{
					currentPermitSets.AddRange(Data.ResponsePermitSet.GetPermitSetsForResponse(answer.ResponseID, CurrentEvaluation.Location.ID));
				}
				else
				{ 
					existingPermitSets.AddRange(Data.ResponsePermitSet.GetPermitSetsForResponse(answer.ResponseID, CurrentEvaluation.Location.ID));
				}
			}

			List<PermitSet> distinctExistingPermitSets = existingPermitSets.Distinct(new PermitSetComparer()).ToList();
			StringBuilder sbIDs = new StringBuilder("");

			foreach (PermitSet permitSet in distinctExistingPermitSets)
			{
				sbIDs.Append(permitSet.ID.ToString() + ": \"" + permitSet.Name + "\", ");
			}

			if (distinctExistingPermitSets.Count > 0)
			{
				existingPermitSetArray = sbIDs.ToString().Substring(0, sbIDs.Length - 2);
			}

			// If revisiting, we have to determine which permit sets were selected based on other pages vs current page
			if (currentPermitSets.Count > 0)
			{
				List<PermitSet> distinctCurrentPermitSets = currentPermitSets.Distinct(new PermitSetComparer()).ToList();

				StringBuilder sb2 = new StringBuilder("");
				foreach (PermitSet permitSet in distinctCurrentPermitSets)
				{
					if (existingPermitSets.Find(i => i.ID == permitSet.ID) == null)
					{
						sb2.Append(permitSet.ID.ToString() + ": \"" + permitSet.Name + "\", ");
					}
				}
				if (sb2.ToString() != "")
				{
					permitSetArray = sb2.ToString().Substring(0, sb2.Length - 2);
				}
			}
		}

		private void DisplayTechnology()
		{
			technologyID = CurrentEvaluation.Technology.ID;
			technologyName = CurrentEvaluation.Technology.Name;
		}

		private void DisplayQuestions()
		{
			questionSetName = questionSet.Name;
			rptRecords.DataSource = questionSet.Questions.OrderBy(i => i.Order).ToList();
			rptRecords.DataBind();
		}

        private void DisplayQuestionsPage()
        {
            HideControl("PreEvalControl1");
            ShowControl("QuestionSetControl1");
            LoadControl("QuestionSetControl", "QuestionSetControl1");
        }

		protected List<Response> DisplayResponses(object dataItem)
		{
			Question question = dataItem as Question;
			return question.Responses;
		}

		protected List<Response> DisplaySubResponses(object dataItem)
		{
			Question question = dataItem as Question;
			return Data.Response.GetResponses(question.ID);
		}

		protected List<PermitSetViewModel> DisplayRequiredPermitSets(object dataItem)
		{
			Response response = dataItem as Response;
		    return Data.ResponsePermitSet.GetPermitSetsForResponseWithPermitCount(response.ID, CurrentEvaluation.Location.ID);
			//return Data.ResponsePermitSet.GetPermitSetsForResponse(response.ID, CurrentEvaluation.Location.ID);
		}

		protected string DisplaySubQuestionClass(object dataItem)
		{
			Question question = dataItem as Question;
			// If existing answer, display selected response
			if (CurrentEvaluation.Answers.Find(i => i.QuestionID == question.ID && i.Page == CurrentEvaluation.CurrentPage) != null)
			{
				return "qActiveNestedRow";
			}
			return "qNestedRow";
		}

		protected List<Question> DisplaySubQuestions(object dataItem)
		{
			List<Question> questions = new List<Question>();
			Question question = dataItem as Question;
			foreach (Response response in question.Responses)
			{
				if (response.SubQuestionID.HasValue)
				{
					// Do not display sub question if it already appears
					if (CurrentEvaluation.UniqueQuestions.Find(i => i.ID == response.SubQuestionID) == null)
					{
						questions.Add(Data.Question.GetQuestion(response.SubQuestionID.Value));
					}
				}
			}

			return questions;
		}

		protected void rptRecords_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Question question = e.Item.DataItem as Question;
				HyperLink hlToolTip = e.Item.FindControl("hlToolTip") as HyperLink;
				Literal ltToolTip = e.Item.FindControl("ltToolTip") as Literal;

				if (question.Description != "")
				{
					ltToolTip.Text = HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(question.Description));
				}
				else
				{
					hlToolTip.Visible = false;
					hlToolTip.ImageUrl = "";
				}

				// If existing answer, display selected response
				if (CurrentEvaluation.Answers.Find(i => i.QuestionID == question.ID) != null)
				{
					if (((HtmlInputHidden)e.Item.FindControl("rID")) != null)
					{
					    var response = CurrentEvaluation.Answers.Find(i => i.QuestionID == question.ID);
					    ((HtmlInputHidden) e.Item.FindControl("noteText")).Value = response.Note;
                        ((HtmlInputHidden)e.Item.FindControl("rID")).Value = response.ResponseID.ToString();
					}
                    else if (((HtmlInputHidden)e.Item.FindControl("subRID")) != null)
                    {
                        var response = CurrentEvaluation.Answers.Find(i => i.QuestionID == question.ID);
                        ((HtmlInputHidden)e.Item.FindControl("noteText")).Value = response.Note;
                        ((HtmlInputHidden)e.Item.FindControl("subRID")).Value = response.ResponseID.ToString();
                    }
				}
			}
		}

		protected void rptResponses_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Response response = e.Item.DataItem as Response;
				RadioButton rb = e.Item.FindControl("rbResponse") as RadioButton;
				rb.Attributes.Add("onclick", "SetUniqueRadioButton('" + response.QuestionGroupName + "', this);validatePage();");

				// If existing answer, display selected response
				if (CurrentEvaluation.Answers.Find(i => i.QuestionID == response.QuestionID) != null)
				{
					if (CurrentEvaluation.Answers.Find(i => i.QuestionID == response.QuestionID).ResponseID == response.ID)
					{
						rb.Checked = true;
					}
				}

				HyperLink hlToolTip = e.Item.FindControl("hlToolTip") as HyperLink;
				Literal ltToolTip = e.Item.FindControl("ltToolTip") as Literal;

				if (response.Description != "")
				{
					ltToolTip.Text = HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(response.Description));
				}
				else
				{
					hlToolTip.Visible = false;
					hlToolTip.ImageUrl = "";
				}
			}
			else if (e.Item.ItemType == ListItemType.Footer)
			{
				Repeater rptResponses = (Repeater)sender;
				int responseCount = rptResponses.Items.Count;

				// Responses are displayed in two columns
				// If odd number of responses, display an extra column
				if (responseCount > 2 && (responseCount % 2 == 1))
				{
					Literal closingTDs = e.Item.FindControl("closingTDs") as Literal;
					closingTDs.Text = "<td class=\"right1\">&nbsp;</td><td class=\"right2\">&nbsp;</td>";
				}
			}
		}

		public string DisplayQuestionNum()
		{
			return questionNo++.ToString();
		}

		public string DisplayNestedQuestionNum()
		{
			return questionNo.ToString() + "a";
		}

		private bool DisplayExistingValue()
		{
			existingValue = (CurrentEvaluation.Answers.Find(i => i.QuestionID == questionSet.Questions[0].ID) != null);
			hfExistingValue.Value = existingValue.ToString();
			return existingValue;
		}

		protected string DisplayResponseSelected(object dataItem)
		{
			if (existingValue)
			{
				Domain.Question question = dataItem as Domain.Question;
				if (question.ResponseID.HasValue)
				{
					return "answeredYes";
				}
			}
			return "answer";
		}

		protected void lbBack_OnClick(object sender, EventArgs e)
		{
			if (CurrentEvaluation.CurrentPage > 1)
			{
				CurrentEvaluation.CurrentPage--;
				LoadPage();
			}
			else
			{
				CurrentEvaluation.CurrentPage--;
				LoadPreEvaluationPage();
			}
		}

		protected void lbNext_OnClick(object sender, EventArgs e)
		{
            CurrentEvaluation.CurrentPage = CurrentEvaluation.CurrentPage + 1;
            NavigateToStep(true);
		}

        public void NavigateToStep(bool shouldSave)
        {
            if (shouldSave)
            {
                UpdateQuestionSetResponses();
            }

            if (CurrentEvaluation.LastStepFilled < CurrentEvaluation.CurrentPage)
            {
                CurrentEvaluation.LastStepFilled = CurrentEvaluation.CurrentPage;
            }

            if (Revisit)
            {
                Revisit = false;
                LoadResultsPage();
            }
            else if (CurrentEvaluation.CurrentPage <= CurrentEvaluation.NumPages)
            {
                LoadPage();
            }
            else
            {
                LoadResultsPage();
            }
        }

		public void UpdateQuestionSetResponses()
		{
			List<EvaluationResponse> currentPageResponses = new List<EvaluationResponse>();            

			for (int i = 0; i < rptRecords.Items.Count; i++)
			{
				// Make question/response pair
				int questionID = int.Parse(((HtmlInputHidden)rptRecords.Items[i].FindControl("qID")).Value);
				string responseValue = ((HtmlInputHidden)rptRecords.Items[i].FindControl("rID")).Value;
                string noteText = ((HtmlInputHidden)rptRecords.Items[i].FindControl("noteText")).Value;

				if (responseValue != "" && responseValue != "default")
				{
				    var response = new EvaluationResponse(CurrentEvaluation.CurrentPage, questionID, int.Parse(responseValue));
				    response.Note = noteText;
					currentPageResponses.Add(response);
				}

				// Get nested question values
				Repeater rptSubQuestions = rptRecords.Items[i].FindControl("rptSubQuestions") as Repeater;
				for (int j = 0; j < rptSubQuestions.Items.Count; j++)
				{
					int subQuestionID = int.Parse(((HtmlInputHidden)rptSubQuestions.Items[j].FindControl("subQID")).Value);
					string subResponseValue = ((HtmlInputHidden)rptSubQuestions.Items[j].FindControl("subRID")).Value;
                    string subNoteText = ((HtmlInputHidden)rptSubQuestions.Items[j].FindControl("noteText")).Value;

					if (subResponseValue != "" && subResponseValue != "default")
					{
					    var response = new EvaluationResponse(CurrentEvaluation.CurrentPage, subQuestionID, int.Parse(subResponseValue));
					    response.Note = subNoteText;
						currentPageResponses.Add(response);
					} 					
				}	
			}

			if (IsLoggedIn)
			{                               
				SavePageResponsesToDB(currentPageResponses);
			}
		}

        protected string DisplayPermitSetUrl(object dataItem)
        {
            var p = (PermitSetViewModel)dataItem;
            if (string.IsNullOrEmpty(p.PermitUrl))
            {
                return p.PermitSetName;
            }
            else
            {
                return string.Format("<a class='permitLink' href='{0}' target='_blank'>{1}</a>", p.PermitUrl, p.PermitSetName);
            }
        }
	}
}