using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HawaiiDBEDT.Domain.Enumerations;

namespace HawaiiDBEDT.Data
{
	public partial class PreEvaluationQuestion
	{	 		
		#region Domain Object Methods
		public Domain.PreEvaluationQuestion GetDomainObject(bool getChildrenObject)
		{
			Domain.PreEvaluationQuestion dto = new Domain.PreEvaluationQuestion();
			dto.ID = this.PreEvaluationQuestionID;
			dto.Name = this.QuestionText;
			dto.LookupClassName = this.LookupClassName;
			dto.AllowCustomResponse = this.AllowCustomResponse;
			dto.SubQuestionTypeID = this.SubQuestionTypeID;
			dto.Description = this.Description;
			dto.Order = this.QuestionOrder;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				switch ((PreEvaluationQuestionType)Enum.Parse(typeof(PreEvaluationQuestionType), dto.LookupClassName))
				{
					case PreEvaluationQuestionType.Technology:
						dto.LookupOptions = Technology.GetTechnologyLookups();
						break;
					case PreEvaluationQuestionType.Location:
						dto.LookupOptions = Location.GetLocationLookups();
						break;
					case PreEvaluationQuestionType.Capacity:
						dto.Responses = PreEvaluationResponse.GetPreEvaluationResponses(this.PreEvaluationQuestionID);
						break;
					case PreEvaluationQuestionType.Federal:
						dto.Responses = PreEvaluationResponse.GetPreEvaluationResponses(this.PreEvaluationQuestionID);
						break;
					default:
						break; 
				}
			}
			return dto;
		}
		#endregion

		#region CRUD		
		public static Domain.PreEvaluationQuestion GetPreEvaluationQuestion(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from questions in db.PreEvaluationQuestions
					    where questions.PreEvaluationQuestionID == questionID
					    select questions).FirstOrDefault();

				return q.GetDomainObject(true);
			}
		}

		public static void UpdatePreEvaluationQuestion(Domain.PreEvaluationQuestion dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PreEvaluationQuestion question = db.PreEvaluationQuestions.Where(i => i.PreEvaluationQuestionID == dto.ID).SingleOrDefault();

				if (question != null)
				{
					question.QuestionText = dto.Name;
					question.Description = dto.Description;
					question.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}

		public static List<Domain.PreEvaluationQuestion> GetPreEvaluationQuestions()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.PreEvaluationQuestion> questions = db.PreEvaluationQuestions.OrderBy(i => i.QuestionText).Select(i => i.GetDomainObject(true)).ToList();
				return questions;
			}
		}
		#endregion
	}
}
