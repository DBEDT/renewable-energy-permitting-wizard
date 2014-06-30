using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HawaiiDBEDT.Web
{
    using HawaiiDBEDT.Data;

    using Evaluation = HawaiiDBEDT.Domain.Evaluation;
    using EvaluationResponse = HawaiiDBEDT.Domain.EvaluationResponse;
    using PreEvaluationResponse = Domain.PreEvaluationResponse;
    using Question = HawaiiDBEDT.Domain.Question;
    using QuestionSet = HawaiiDBEDT.Domain.QuestionSet;
    using User = HawaiiDBEDT.Domain.User;

    public class BasePage : System.Web.UI.Page
	{
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

        protected void PrepareEvaluationForDisplay(Evaluation evaluation)
        {
            CurrentEvaluation = evaluation;
            CurrentEvaluation.QuestionSets =
                Data.QuestionSet.GetQuestionSetComplete(CurrentEvaluation.TechnologyID,
                                                        CurrentEvaluation.LocationID).ToList();
            GetFederalResponses();

            List<Question> allQuestions = new List<Question>();
            List<QuestionSet> QuestionSets = CurrentEvaluation.QuestionSets;
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
            CurrentEvaluation.DistinctPermits = (new ApplicationManager()).GetDistinctPermitsForEvaluation(evaluation);
        }

        protected void GetFederalResponses()
        {
            PreEvaluationResponse federalResponse = Data.PreEvaluationResponse.GetPreEvaluationResponse(CurrentEvaluation.FederalID);
            if (federalResponse == null)
            {
                return;
            }

            if (federalResponse.SubQuestions.Count > 0)
            {
                QuestionSet questionSet = new QuestionSet();
                questionSet.Name = "Federal";

                foreach (Question q in federalResponse.SubQuestions)
                {
                    questionSet.Questions.Add(Data.Question.GetQuestion(q.ID));
                }

                CurrentEvaluation.QuestionSets.Insert(0, questionSet);
            }
        }

		#region Save
		public void SaveCurrentEvaluationComplete()
		{
			SaveEvaluationToDB();
			SavePageResponsesToDB(CurrentEvaluation.CapacityAnswers);
			SaveResponsesToDB(CurrentEvaluation.Answers);

		}

		public void SaveEvaluationToDB()
		{
			if (CurrentEvaluation.ID == 0)
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
	}
}