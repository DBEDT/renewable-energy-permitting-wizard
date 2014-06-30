using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HawaiiDBEDT.Domain;
using HawaiiDBEDT.Domain.Enumerations;

namespace HawaiiDBEDT.Web.Controls
{
    using utils;
    using Westwind.Web.Utilities;

    public partial class PreEvaluationControl : BaseControl
	{
		protected PreEvaluationQuestion technologyQuestion;
		protected PreEvaluationQuestion capacityQuestion;
		protected PreEvaluationQuestion locationQuestion;
		protected PreEvaluationQuestion federalQuestion;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				GetQuestions();
				DisplayTechnologyQuestion();
				DisplayCapacityQuestion();
				DisplayLocationQuestion();
				LoadPage();
			}
		}

		public void LoadPage()
		{
			CurrentEvaluation.CurrentPage = 0;
			RefreshProgressBar();

			if (HasExistingValues())
			{
				lbStartEval.CssClass = "startEval";
				lbStartEval.Attributes.Remove("disabled");
			}
			else if (CurrentUserID != 0)
			{
				PrepopulateUserValues();
			}
			else
			{
				lbStartEval.Attributes.Remove("disabled");
				lbStartEval.Attributes.Add("disabled", "disabled");
			}
		}

		private void PrepopulateUserValues()
		{
			string technologyIDValue = CurrentUser.TechnologyID.ToString();
			for (int i = 0; i < rptTechnology.Items.Count; i++)
			{
				RadioButton rb = (RadioButton)rptTechnology.Items[i].FindControl("rbTechnology");
				if (rb != null && rb.Attributes["technologyID"] == technologyIDValue)
				{
					rb.Checked = true;
					break;
				}
			}
			string locationIDValue = CurrentUser.LocationID.ToString();
			for (int i = 0; i < rptLocation.Items.Count; i++)
			{
				RadioButton rb = (RadioButton)rptLocation.Items[i].FindControl("rbLocation");

				if (rb != null && rb.Attributes["locationID"] == locationIDValue)
				{
					rb.Checked = true;
					break;
				}
				else
				{

					rb.Checked = false;
				}
			}
		}

		#region Page Display
		private void GetQuestions()
		{
			List<PreEvaluationQuestion> questions = Data.PreEvaluationQuestion.GetPreEvaluationQuestions();
			technologyQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Technology.ToString());
			capacityQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Capacity.ToString());
			locationQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Location.ToString());
			federalQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Federal.ToString());
		}

		private void DisplayTechnologyQuestion()
		{
			lblTechnologyQuestion.Text = technologyQuestion.Name;
			if (technologyQuestion.Description != "")
			{
				ltTechnologyToolTip.Text = HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(technologyQuestion.Description));
			}
			else
			{
				hlTechnologyToolTip.Visible = false;
				hlTechnologyToolTip.ImageUrl = "";
			}
			rptTechnology.DataSource = technologyQuestion.LookupOptions.OrderBy(i => i.Order).ToList();
			rptTechnology.DataBind();

			string technologyIDValue = CurrentEvaluation.TechnologyID.ToString();

			if (technologyIDValue != "0")
			{
				for (int i = 0; i < rptTechnology.Items.Count; i++)
				{
					RadioButton rb = (RadioButton)rptTechnology.Items[i].FindControl("rbTechnology");
					if (rb != null && rb.Attributes["technologyID"] == technologyIDValue)
					{
						rb.Checked = true;
						break;
					}
				}
			}
		}

		private void DisplayCapacityQuestion()
		{
			lblCapacityQuestion.Text = capacityQuestion.Name;
			if (capacityQuestion.Description != "")
			{
				ltCapacityToolTip.Text = HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(capacityQuestion.Description));
			}
			else
			{
				hlCapacityQuestion.Visible = false;
				hlCapacityQuestion.ImageUrl = "";
			}

			rptResponses.DataSource = capacityQuestion.Responses;
			rptResponses.DataBind();

			rptResponses2.DataSource = capacityQuestion.Responses;
			rptResponses2.DataBind();

			if (CurrentEvaluation.CapacityID != 0)
			{
				capacityResponseID.Value = CurrentEvaluation.CapacityID.ToString();
			}
		}

		private void DisplayLocationQuestion()
		{
			lblLocationQuestion.Text = locationQuestion.Name;
			if (locationQuestion.Description != "")
			{
				ltLocationToolTip.Text = HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(locationQuestion.Description));
			}
			else
			{
				hlLocationQuestion.Visible = false;
				hlLocationQuestion.ImageUrl = "";
			}
			rptLocation.DataSource = locationQuestion.LookupOptions.OrderBy(i => i.Order).ToList();
			rptLocation.DataBind();
		}
        
		private bool HasExistingValues()
		{
			//If current eval is not null, re-display current values;
			if (CurrentEvaluation.Active.HasValue && CurrentEvaluation.Active.Value)
			{
				return true;
			}
			return false;
		}

		#endregion

		#region Page Events
		protected void rptTechnology_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (HasExistingValues())
				{
					Technology technology = e.Item.DataItem as Technology;
                    if (CurrentEvaluation != null && CurrentEvaluation.Technology != null && technology != null)
                    {
                        if (CurrentEvaluation.Technology.ID == technology.ID)
                        {
                            RadioButton rb = e.Item.FindControl("rbTechnology") as RadioButton;
                            rb.Checked = true;
                        }
                    }
				}
			}
		}

		protected void rptLocation_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (HasExistingValues())
				{
					Location location = e.Item.DataItem as Location;
					if (CurrentEvaluation != null && CurrentEvaluation.Location != null)
					{
						if (CurrentEvaluation.Location.ID == location.ID)
						{
							RadioButton rb = e.Item.FindControl("rbLocation") as RadioButton;
							rb.Checked = true;
						}
					}
				}
			}
		}

		protected void lbStartEvaluation_OnClick(object sender, EventArgs e)
		{
			Page.Validate();

			if (Page.IsValid)
			{
				UpdateEvaluation();
                CurrentEvaluation.CurrentPage = 1;

                if (CurrentEvaluation.QuestionSets.Count == 0)
                {
                    DisplayResultsPage();
                }
                else
                {
                    DisplayQuestionsPage();
                }
			}
		}

        public void StartEvaluationFromNavigation(int stepIndex)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                UpdateEvaluation();
                CurrentEvaluation.CurrentPage = stepIndex;

                if (CurrentEvaluation.QuestionSets.Count == 0)
                {
                    DisplayResultsPage();
                }
                else
                {
                    DisplayQuestionsPage();
                }
            }
        }


		#endregion

		#region Update

		// Make sure the questions in question sets are unique
		List<QuestionSet> QuestionSets = new List<QuestionSet>();

		private void UpdateEvaluation()
		{
			if (CurrentEvaluationID != 0)
			{
                CurrentEvaluation.LastStepFilled = Data.EvaluationResponse.FindLastEvaluationStepFilled(CurrentEvaluation.ID);
			}
			else
			{
				CurrentEvaluation = new Evaluation();
				CurrentEvaluation.Active = true;
			    CurrentEvaluation.LastStepFilled = 0;
			}

			CurrentEvaluation.Technology = GetSelectedTechnology();
			CurrentEvaluation.TechnologyID = CurrentEvaluation.Technology.ID;
			CurrentEvaluation.Location = GetSelectedLocation();
			CurrentEvaluation.LocationID = CurrentEvaluation.Location.ID;            

			GetCapacityResponses();

			QuestionSets = Data.QuestionSet.GetQuestionSetComplete(CurrentEvaluation.TechnologyID, CurrentEvaluation.LocationID).ToList();
            
			GetFederalResponses();

			List<Question> allQuestions = new List<Question>();

			foreach (QuestionSet questionSet in QuestionSets)
			{
				List<Question> uniqueQuestions = new List<Question>();

				foreach (Question question in questionSet.Questions)
				{
					if (allQuestions.Find(i => i.ID == question.ID) == null)
					{
						allQuestions.Add(question);
						uniqueQuestions.Add(question);
					}
					else
					{
						//found duplicate question
					}
				}
				questionSet.Questions = uniqueQuestions;
			}

			CurrentEvaluation.UniqueQuestions = allQuestions;
			CurrentEvaluation.QuestionSets = QuestionSets;
			CurrentEvaluation.NumPages = CurrentEvaluation.QuestionSets.Count;

			// Add to permit set list

			CurrentEvaluation.PermitSets.AddRange(Data.TechnologyPermitSet.GetPermitSetsInTechnology(CurrentEvaluation.TechnologyID));
			CurrentEvaluation.PermitSets.AddRange(Data.LocationPermitSet.GetPermitSetsInLocation(CurrentEvaluation.LocationID));

			if (CurrentUserID != 0)
			{
				CurrentEvaluation.UserID = CurrentUserID;
			}

			// Save evaluation to DB
			if (IsLoggedIn)
			{
				SaveEvaluationToDB();
			}

			if (CurrentEvaluation.CapacityAnswers.Count > 0 && IsLoggedIn)
			{
				SavePageResponsesToDB(CurrentEvaluation.CapacityAnswers);
			}
		}

        private void GetFederalResponses()
        {
            CurrentEvaluation.FederalID = int.Parse(ConfigurationManager.AppSettings["FederalID"]);
            List<Question> subFederalQuestions = Data.PreEvaluationResponse.GetPreEvaluationResponse(CurrentEvaluation.FederalID).SubQuestions;

            if (subFederalQuestions.Count > 0)
            {
                var questionSet = new QuestionSet();
                questionSet.Name = "Federal";

                foreach (var q in subFederalQuestions)
                {
                    questionSet.Questions.Add(Data.Question.GetQuestion(q.ID));
                }

                QuestionSets.Insert(0, questionSet);
            }
        }

	    private void GetCapacityResponses()
		{
			CurrentEvaluation.CapacityID = int.Parse(capacityResponseID.Value);

			for (int i = 0; i < rptResponses2.Items.Count; i++)
			{
				// Make question/response pair
				int responseID = int.Parse(((HtmlInputHidden)rptResponses2.Items[i].FindControl("currentResponseID")).Value);

				if (CurrentEvaluation.CapacityID.Equals(responseID))
				{
					// Get nested question values
					Repeater rptSubQuestions = rptResponses2.Items[i].FindControl("rptSubQuestions") as Repeater;
					for (int j = 0; j < rptSubQuestions.Items.Count; j++)
					{
						int subQuestionID = int.Parse(((HtmlInputHidden)rptSubQuestions.Items[j].FindControl("subQID")).Value);
						string subResponseValue = ((HtmlInputHidden)rptSubQuestions.Items[j].FindControl("subRID")).Value;

						if (subResponseValue != "" && subResponseValue != "default")
						{
							if (CurrentEvaluation.CapacityAnswers.Find(q => q.QuestionID == subQuestionID) != null)
							{
								CurrentEvaluation.CapacityAnswers.Find(q => q.QuestionID == subQuestionID).ResponseID = int.Parse(subResponseValue);
							}
							else
							{
								CurrentEvaluation.CapacityAnswers.Add(new EvaluationResponse(CurrentEvaluation.CurrentPage, subQuestionID, int.Parse(subResponseValue)));
							}

						}
					}	
				}
			}
		}

		private Technology GetSelectedTechnology()
		{
			for (int i = 0; i < rptTechnology.Items.Count; i++)
			{
				RadioButton rb = (RadioButton)rptTechnology.Items[i].FindControl("rbTechnology");

				if (rb != null && rb.Checked)
				{
					return HawaiiDBEDT.Data.DataAccess.TechnologyList.Find(q => q.ID == int.Parse(rb.Attributes["technologyID"]));
				}
			}
			return new Technology();
		}

		private Location GetSelectedLocation()
		{
			for (int i = 0; i < rptLocation.Items.Count; i++)
			{
				RadioButton rb = (RadioButton)rptLocation.Items[i].FindControl("rbLocation");

				if (rb != null && rb.Checked)
				{
					return HawaiiDBEDT.Data.DataAccess.LocationList.Find(q => q.ID == int.Parse(rb.Attributes["locationID"]));
				}
			}
			return new Location();
		}
		#endregion

		#region Evaluation Control Display
		private void DisplayQuestionsPage()
		{
			HideControl("PreEvalControl1");
			ShowControl("QuestionSetControl1");
			LoadControl("QuestionSetControl", "QuestionSetControl1");
		}

		private void DisplayResultsPage()
		{
			HideControl("PreEvalControl1");
			ShowControl("ResultsControl1");
			CurrentEvaluation.CurrentPage = CurrentEvaluation.NumPages;
			LoadControl("ResultsControl", "ResultsControl1");
		}
		#endregion

		#region Capacities Responses / Subquestion

		protected void rptSubQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
				if (CurrentEvaluation.CapacityAnswers.Find(i => i.QuestionID == question.ID) != null)
				{
					if (((HtmlInputHidden)e.Item.FindControl("subRID")) != null)
					{
						((HtmlInputHidden)e.Item.FindControl("subRID")).Value = CurrentEvaluation.CapacityAnswers.Find(i => i.QuestionID == question.ID).ResponseID.ToString();
					}
				}
			}
		}

		protected void rptResponses_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				PreEvaluationResponse response = e.Item.DataItem as PreEvaluationResponse;
				RadioButton rb = e.Item.FindControl("rbResponse") as RadioButton;
				rb.Attributes.Add("onclick", "SetUniqueRadioButton('" + response.QuestionGroupName + "', this);");

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

				// If existing answer, display selected response
				if (HasExistingValues())
				{
					if (rb.Attributes["QuestionTypeID"].ToString() == ((int)QuestionTypeEnum.Capacity).ToString())
					{
						if (rb.Attributes["responseID"].ToString() == CurrentEvaluation.CapacityID.ToString())
						{
							rb.Checked = true;
						}
					}
					else if (rb.Attributes["QuestionTypeID"].ToString() == ((int)QuestionTypeEnum.Federal).ToString())
					{
						if (rb.Attributes["responseID"].ToString() == CurrentEvaluation.FederalID.ToString())
						{
							rb.Checked = true;
						}
					}
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
					closingTDs.Text = "<td class=\"noStyle\">&nbsp;</td>";
				}
			}
		}

		protected void rptSubResponses_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Response response = e.Item.DataItem as Response;
				RadioButton rb = e.Item.FindControl("rbResponse") as RadioButton;
				rb.Attributes.Add("onclick", "SetUniqueRadioButton('" + response.QuestionGroupName + "', this);");

				// If existing answer, display selected response
				if (CurrentEvaluation.CapacityAnswers.Find(i => i.QuestionID == response.QuestionID) != null)
				{
					if (CurrentEvaluation.CapacityAnswers.Find(i => i.QuestionID == response.QuestionID).ResponseID == response.ID)
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
					closingTDs.Text = "<td class=\"noStyle\">&nbsp;</td>";
				}
			}
		}

		protected string DisplayReponseStyle(object dataItem)
		{
			Response response = dataItem as Response;

			// If existing answer, display selected response
			if (response.ID.Equals(CurrentEvaluation.CapacityID))
			{
				return "display:block";
			}
			return "display:none";
		}

		protected List<Question> DisplaySubQuestions(object dataItem)
		{
			PreEvaluationResponse response = dataItem as PreEvaluationResponse;

			List<Question> questions = new List<Question>();
			List<Question> subQuestions = Data.PreEvaluationSubQuestion.GetPreEvaluationSubQuestions(response.ID);
			foreach (Question question in subQuestions)
			{
				Question q = Data.Question.GetQuestion(question.ID);
				q.ParentResponseID = response.ID;
				questions.Add(q);
			}

			return questions;
		}

		protected List<Response> DisplaySubResponses(object dataItem)
		{
			Question question = dataItem as Question;
			return Data.Response.GetResponses(question.ID);
			//return question.Responses;
		}

		protected List<PermitSet> DisplayRequiredPermitSets(object dataItem)
		{
			PreEvaluationResponse response = dataItem as PreEvaluationResponse;
			return new List<PermitSet>();
			//return Data.PreEvaluationResponsePermitSet.GetPermitSetsForPreEvaluationResponse(response.ID);
		}

		#endregion
	}
}