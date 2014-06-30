using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class PreEvaluationSubQuestion
	{
		#region CRUD

		public static void AddPreEvaluationSubQuestions(int preEvalResponseID, int prevEvalQuestionID, Domain.PreEvaluationResponse dto)
		{
			int questionSetID = dto.ID;
			int questionOrder = 1;

			foreach (Domain.Question question in dto.SubQuestions)
			{
				AddPreEvaluationSubQuestion(preEvalResponseID, prevEvalQuestionID, question.ID, questionOrder);
				questionOrder++;
			}
		}

		public static void AddPreEvaluationSubQuestion(int preEvalResponseID, int prevEvalQuestionID, int questionID, int order)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PreEvaluationSubQuestion item = new PreEvaluationSubQuestion
				{
					PreEvaluationResponseID = preEvalResponseID,
					PreEvaluationQuestionID = prevEvalQuestionID,
					QuestionID = questionID,
					QuestionOrder = order,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.PreEvaluationSubQuestions.InsertOnSubmit(item);
				db.SubmitChanges();
			}
		}

		/* List question sets for a given question */
		public static List<Domain.Question> GetPreEvaluationSubQuestions(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from preEvalSubQ in db.PreEvaluationSubQuestions
					    join questions in db.Questions on preEvalSubQ.QuestionID equals questions.QuestionID
					    where preEvalSubQ.PreEvaluationResponseID == responseID
					    orderby preEvalSubQ.QuestionOrder
					    select questions);

				return q.Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static void DeleteSubQuestionsForPreEvaluationQuestion(int preEvalQuestionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PreEvaluationSubQuestions
							    where q.PreEvaluationQuestionID == preEvalQuestionID
							    select q;

				db.PreEvaluationSubQuestions.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeleteSubQuestionsForResponse(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PreEvaluationSubQuestions
							    where q.PreEvaluationResponseID == responseID
							    select q;

				db.PreEvaluationSubQuestions.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeleteSubQuestionsForQuestion(int subQuestionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PreEvaluationSubQuestions
							    where q.QuestionID == subQuestionID
							    select q;

				db.PreEvaluationSubQuestions.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		#endregion
	}
}
