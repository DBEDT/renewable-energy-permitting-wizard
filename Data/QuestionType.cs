using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class QuestionType
	{
		#region Domain Object Methods
		public Domain.QuestionType GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.QuestionType dto = new Domain.QuestionType();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		#region CRUD
		public int AddQuestionType(Domain.QuestionType dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				QuestionType QuestionType = new QuestionType
				{
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.QuestionTypes.InsertOnSubmit(QuestionType);
				db.SubmitChanges();

				return 0;
			}
		}

		public static Domain.QuestionType GetQuestionType(int QuestionTypeID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from QuestionTypes in db.QuestionTypes
					    where QuestionTypes.ID == QuestionTypeID
					    select QuestionTypes).FirstOrDefault();

				return q.GetDomainObject(false, true);
			}
		}

		public static List<Domain.QuestionType> GetQuestionTypes()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.QuestionType> QuestionTypes = db.QuestionTypes.Select(i => i.GetDomainObject(false, false)).ToList();
				return QuestionTypes;
			}
		}

		public static void UpdateQuestionType(Domain.QuestionType dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				QuestionType QuestionType = db.QuestionTypes.Where(i => i.ID == dto.ID).SingleOrDefault();

				if (QuestionType != null)
				{
					QuestionType.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}

		}
		#endregion
	}
}
