using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class QuestionSetQuestion
	{
		#region CRUD

		public static void AddQuestionsToQuestionSet(Domain.QuestionSet dto)
		{
			int questionSetID = dto.ID;

			foreach (Domain.Question question in dto.Questions)
			{
				AddQuestionToQuestionSet(questionSetID, question.ID);
			}
		}

		public static void AddQuestionToQuestionSet(int questionSetID, int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				QuestionSetQuestion questionSetQuestion = new QuestionSetQuestion
				{
					QuestionSetID = questionSetID,
					QuestionID = questionID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				// Set quest set order to max order + 1
				byte? maxOrder = db.QuestionSetQuestions.Where(i => i.QuestionSetID == questionSetID).Max(i => (byte?)i.QuestionOrder);
				byte order = ++maxOrder ?? 1;
				questionSetQuestion.QuestionOrder = order;  

				db.QuestionSetQuestions.InsertOnSubmit(questionSetQuestion);
				db.SubmitChanges();
			}
		}

		/* List question sets for a given question */
		public static List<Domain.QuestionSet> GetQuestionSets(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from questionsInSet in db.QuestionSetQuestions
					    join questionSets in db.QuestionSets on questionsInSet.QuestionSetID equals questionSets.ID
					    where questionsInSet.QuestionID == questionID
					    select questionSets);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		/* List questions in a given question set */
		public static List<Domain.Question> GetQuestionsInQuestionSet(int questionSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from questions in db.Questions
					    join questionsInSet in db.QuestionSetQuestions on questions.QuestionID equals questionsInSet.QuestionID
					    join questionSets in db.QuestionSets on questionsInSet.QuestionSetID equals questionSets.ID
					    where questionSets.ID == questionSetID
					    orderby questionsInSet.QuestionOrder
					    select questions);

				return q.Select(i => i.GetDomainObject(true, false)).ToList().OrderBy(i => i.Order).ToList();
			}
		}
		
		/* List questions not yet used in a technology */
		public static List<Domain.Question> GetQuestionsNotUsedInTechnology(int technologyID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{

				var q = from questions2 in db.Questions

					   where !(from questions in db.Questions
							 join questionsInSet in db.QuestionSetQuestions on questions.QuestionID equals questionsInSet.QuestionID
							 join questionSets in db.QuestionSets on questionsInSet.QuestionSetID equals questionSets.ID
							 where questionSets.TechnologyID == technologyID
							 select questions.QuestionID).Contains(questions2.QuestionID)
					   select questions2;

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static void DeleteQuestionSetsForQuestion(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.QuestionSetQuestions
							    where q.QuestionID == questionID
							    select q;

				db.QuestionSetQuestions.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeleteQuestionsForQuestionSet(int questionSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.QuestionSetQuestions
							    where q.QuestionSetID == questionSetID
							    select q;

				db.QuestionSetQuestions.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
