using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class Evaluation
	{
		public Domain.Evaluation GetDomainObject(bool getChildrenObject, bool getParent)
		{
			return GetDomainObject(getChildrenObject, getParent, false);
		}

		public Domain.Evaluation GetDomainObject(bool getChildrenObject, bool getParent, bool getUserInfo)
		{
			Domain.Evaluation dto = new Domain.Evaluation();
			dto.ID = this.EvaluationID;
			dto.UserID = this.UserID;
			dto.Active = true;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.TechnologyID = this.TechnologyID;
			dto.Technology = DataAccess.TechnologyList.Find(i => i.ID == this.TechnologyID);
			dto.LocationID = this.LocationID;
			dto.Location = DataAccess.LocationList.Find(i => i.ID == this.LocationID);
			dto.CapacityID = this.CapacityID;
			dto.FederalID = this.FederalID;

			dto.StatusID = this.StatusID;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				List<Domain.EvaluationResponse> responses = EvaluationResponse.GetResponses(dto.ID);
				dto.CapacityAnswers = responses.Where(i => i.Page == 0).ToList();
				dto.Answers = responses.Where(i => i.Page > 0).ToList();
			}

			if (getParent)
			{
			}

			if (getUserInfo)
			{
				dto.User = User.GetUser(this.UserID);
			}
			return dto;
		}

		#region CRUD
		public static List<Domain.Evaluation> GetEvaluations()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				return db.Evaluations.Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static List<Domain.Evaluation> GetEvaluations(int userID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				return db.Evaluations.Where(i => i.UserID == userID).Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static List<Domain.Evaluation> GetEvaluationsAdmin()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				return db.Evaluations.Where(i => i.StatusID == 2).Select(i => i.GetDomainObject(false, false, true)).ToList();
			}
		}

		public static List<Domain.Evaluation> GetEvaluations(int userID, int statusID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				return db.Evaluations.Where(i => i.UserID == userID && i.StatusID == statusID).Select(i => i.GetDomainObject(false, false)).ToList().OrderByDescending(i => i.DateModified).ToList();
			}
		}

		public static Domain.Evaluation GetEvaluation(int evaluationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from Evaluations in db.Evaluations
					    where Evaluations.EvaluationID == evaluationID
					    select Evaluations).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static int AddEvaluation(Domain.Evaluation dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Evaluation Evaluation = new Evaluation
				{
					Name = dto.Name,
					Description = dto.Description,
					StatusID = dto.StatusID,
					UserID = dto.UserID,
					TechnologyID = dto.TechnologyID,
					LocationID = dto.LocationID,
					CapacityID = dto.CapacityID,
					FederalID = dto.FederalID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.Evaluations.InsertOnSubmit(Evaluation);
				db.SubmitChanges();
				dto.ID = Evaluation.EvaluationID;

				return dto.ID;
			}
		}

		public static void UpdateEvaluation(Domain.Evaluation dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Evaluation foundEvaluation = db.Evaluations.Where(i => i.EvaluationID == dto.ID).SingleOrDefault();

				if (foundEvaluation != null)
				{
					foundEvaluation.Name = dto.Name;
					foundEvaluation.Description = dto.Description;
					foundEvaluation.StatusID = dto.StatusID;
					foundEvaluation.TechnologyID = dto.TechnologyID;
					foundEvaluation.LocationID = dto.LocationID;
					foundEvaluation.CapacityID = dto.CapacityID;
					foundEvaluation.FederalID = dto.FederalID;
					foundEvaluation.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}

		public static void UpdateEvaluationLastModified(int EvaluationID, DateTime LastModified)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Evaluation foundEvaluation = db.Evaluations.Where(i => i.EvaluationID == EvaluationID).SingleOrDefault();

				if (foundEvaluation != null)
				{
					foundEvaluation.DateModified = LastModified;
					db.SubmitChanges();
				}
			}
		}

		public static void DeleteEvaluation(int evaluationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				EvaluationResponse.DeleteEvaluationResponses(evaluationID);
				
				// Delete evaluation

				var rowsToDelete = from q in db.Evaluations
							    where q.EvaluationID == evaluationID
							    select q;

				db.Evaluations.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}

		}

		#endregion
	}
}
