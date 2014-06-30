using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HawaiiDBEDT.Domain;

namespace HawaiiDBEDT.Web.Controls
{
	public class BaseControl : System.Web.UI.UserControl
	{
		#region Session Values
		private string revisit = "REVISIT";

		public bool Revisit
		{
			get
			{
				if (Session[revisit] != null)
				{
					return (bool)Session[revisit];
				}
				return false;
			}
			set
			{
				Session[revisit] = value;
			}
		}

		private string review = "REVIEW";

		public bool Review
		{
			get
			{
				if (Session[review] != null)
				{
					return (bool)Session[review];
				}
				return false;
			}
			set
			{
				Session[review] = value;
			}
		}

		public bool IsLoggedIn
		{
			get
			{
				return (HttpContext.Current.User.Identity.Name != "");
			}
		}

		public int CurrentUserID
		{
			get
			{
				if (IsLoggedIn)
				{
					return int.Parse(HttpContext.Current.User.Identity.Name);
				}
				else
				{

					return 0;
				}
			}
		}
		public int CurrentEvaluationID
		{
			get
			{
				return CurrentEvaluation.ID;
			}
		}

		private string currentUser = "CURRENT_USER";

		public User CurrentUser
		{
			get
			{
				if (Session[currentUser] != null)
				{
					return (User)Session[currentUser];
				}
				else
				{
					if (CurrentUserID != 0)
					{
						return Data.User.GetUser(CurrentUserID);
					}
					else
					{
						return new User();
					}
				}
			}
			set
			{
				Session[currentUser] = value;
			}
		}

		private string currentEvaluation = "CURRENT_EVALUATION";

		public Evaluation CurrentEvaluation
		{
			get
			{
				if (Session[currentEvaluation] != null)
				{
					return (Evaluation)Session[currentEvaluation];
				}
				return new Evaluation();
			}
			set
			{
				Session[currentEvaluation] = value;
			}
		}

		#endregion

		#region Save

		public void SaveCurrentEvaluationComplete()
		{
			SaveEvaluationToDB();
			SavePageResponsesToDB(CurrentEvaluation.CapacityAnswers);
			SaveResponsesToDB(CurrentEvaluation.Answers);

		}

		public void SaveEvaluationToDB()
		{
			if (CurrentEvaluationID == 0)
			{
				CurrentEvaluation.ID = Data.Evaluation.AddEvaluation(CurrentEvaluation);
			}
			else
			{
				Data.Evaluation.UpdateEvaluation(CurrentEvaluation);
			}
		}

		public void SavePageResponsesToDB(List<EvaluationResponse> responses)
		{
			if (CurrentEvaluationID != 0)
			{
				Data.EvaluationResponse.AddEvaluationResponse(CurrentEvaluationID, CurrentEvaluation.CurrentPage, responses);
			    CurrentEvaluation.Answers.RemoveAll(i => i.Page == CurrentEvaluation.CurrentPage);
                CurrentEvaluation.Answers.AddRange(responses);
			}
		}

		public void SaveResponsesToDB(List<EvaluationResponse> responses)
		{
			if (CurrentEvaluationID != 0)
			{
				Data.EvaluationResponse.AddEvaluationResponse(CurrentEvaluationID, responses);
			}
		}
		#endregion

		public void RefreshProgressBar()
		{
			LoadControl("ProgressBarControl", "ProgressBar1");
		}

		public void ShowControl(string controlID)
		{
			((EvaluationControl)this.Parent).FindControl(controlID).Visible = true;
		}

		public void HideControl(string controlID)
		{
			((EvaluationControl)this.Parent).FindControl(controlID).Visible = false;
		}

		public void LoadControl(string controlName, string controlID)
		{
			if (controlName == "PreEvalControl")
			{
				((PreEvaluationControl)((EvaluationControl)this.Parent).FindControl(controlID)).LoadPage();
			}
			else if (controlName == "QuestionSetControl")
			{
				((QuestionSetControl)((EvaluationControl)this.Parent).FindControl(controlID)).LoadPage();
			}
			else if (controlName == "ProgressBarControl")
			{
				((ProgressBarControl)((EvaluationControl)this.Parent).FindControl(controlID)).LoadPage();
			}
			else if (controlName == "ResultsControl")
			{
				((ResultsControl)((EvaluationControl)this.Parent).FindControl(controlID)).LoadPage();
			}
		}

		public void LoadPreEvaluationPage()
		{
			HideControl("QuestionSetControl1");
			HideControl("ResultsControl1");
			ShowControl("PreEvalControl1");
			CurrentEvaluation.CurrentPage = 0;
			LoadControl("PreEvalControl", "PreEvalControl1");
		}

		public void LoadQuestionSetPage()
		{
			HideControl("PreEvalControl1");
			HideControl("ResultsControl1");
			ShowControl("QuestionSetControl1");
			LoadControl("QuestionSetControl", "QuestionSetControl1");
		}

		public void LoadResultsPage()
		{
			HideControl("PreEvalControl1");
			HideControl("QuestionSetControl1");
			ShowControl("ResultsControl1");
			CurrentEvaluation.CurrentPage = CurrentEvaluation.NumPages + 1;
			LoadControl("ResultsControl", "ResultsControl1");
		}

	    protected string ExplainEndpoint(string endpointMessage)
	    {
	        if (!string.IsNullOrEmpty(endpointMessage))
	        {
                return endpointMessage;
	        }

            return "You cannot advance to the next step if this answer is selected.";
	    }
	}
}