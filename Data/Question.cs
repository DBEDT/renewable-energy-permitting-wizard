using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class Question
	{
		#region Domain Object Methods
		public Domain.Question GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.Question dto = new Domain.Question(); 
			dto.ID = this.QuestionID;
			dto.Name = this.QuestionText;
			dto.Description = this.Description;
			dto.QuestionTypeID = this.QuestionTypeID;
			dto.QuestionType = DataAccess.QuestionTypeList.Find(i => i.ID == this.QuestionTypeID);
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				dto.Responses = Response.GetResponses(this.QuestionID);
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		#region CRUD
		public static int AddQuestion(Domain.Question dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Question question = new Question
				{
					QuestionText = dto.Name,
					Description = dto.Description,
					QuestionTypeID = dto.QuestionTypeID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.Questions.InsertOnSubmit(question);
				db.SubmitChanges();

				return question.QuestionID;
			}
		} 

		public static void UpdateQuestion(Domain.Question dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Question question = db.Questions.Where(i => i.QuestionID == dto.ID).SingleOrDefault();

				if (question != null)
				{
					question.QuestionText = dto.Name;
					question.Description = dto.Description;
					question.QuestionTypeID = dto.QuestionTypeID;
					question.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}

		}
		public static Domain.Question GetQuestion(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from questions in db.Questions
					    where questions.QuestionID == questionID
					    select questions).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static List<Domain.Question> GetQuestions()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Question> questions = db.Questions.Select(i => i.GetDomainObject(false, false)).ToList();
				return questions.OrderBy(i => i.Name).ToList();
			}
		}

		public static List<Domain.Question> GetQuestions(int questionTypeID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Question> questions = db.Questions.Where(i => i.QuestionTypeID == questionTypeID).Select(i => i.GetDomainObject(false, false)).ToList();
				return questions;
			}
		}

		public static List<Domain.Question> GetQuestionsExceptTypes(int questionTypeID, int questionTypeID2)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Question> questions = db.Questions.Where(i => i.QuestionTypeID != questionTypeID && i.QuestionTypeID != questionTypeID2).Select(i => i.GetDomainObject(false, false)).ToList();
			    questions = questions.OrderBy(q => q.Name).ToList();
				return questions;
			}
		}

		public static bool DeleteQuestion(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				// Delete question from nested response question
				Response.DeleteNestedQuestion(questionID);

				// QuestionSetQuestion
				QuestionSetQuestion.DeleteQuestionSetsForQuestion(questionID);

				// delete from PreEvaluationSubQuestion
				PreEvaluationSubQuestion.DeleteSubQuestionsForQuestion(questionID);

				// Delete any permit set relationship using this permit set
				Response.DeleteResponsesForQuestion(questionID);

				var rowsToDelete = from q in db.Questions
							    where q.QuestionID == questionID
							    select q;

				db.Questions.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();

				return true;
			}
		}
		#endregion
	}
}
