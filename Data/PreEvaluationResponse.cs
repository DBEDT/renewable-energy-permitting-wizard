using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class PreEvaluationResponse
	{
		#region Domain Object Methods
		public Domain.PreEvaluationResponse GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.PreEvaluationResponse dto = new Domain.PreEvaluationResponse();
			dto.ID = this.PreEvaluationResponseID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.PreEvaluationQuestionID = this.PreEvaluationQuestionID;
			dto.RequiresPermits = this.RequiresPermits;
			dto.IsEndPoint = this.IsEndPoint;
			dto.EndPointMessage = this.EndPointMessage;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				dto.SubQuestions = PreEvaluationSubQuestion.GetPreEvaluationSubQuestions(dto.ID);

				if (dto.RequiresPermits)
				{
					dto.UniquePermitSets = PreEvaluationResponsePermitSet.GetPermitSetsForPreEvaluationResponse(dto.ID);
					dto.PermitSets = PreEvaluationResponsePermitSet.GetPreEvaluationResponsePermitSets(dto.ID);
				}
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		#region CRUD
		public static int AddPreEvaluationResponse(Domain.PreEvaluationResponse dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PreEvaluationResponse response = new PreEvaluationResponse
				{
					Name = dto.Name,
					Description = dto.Description,
					PreEvaluationQuestionID = dto.PreEvaluationQuestionID,
					RequiresPermits = dto.RequiresPermits,
					IsEndPoint = dto.IsEndPoint,
					EndPointMessage = dto.EndPointMessage,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.PreEvaluationResponses.InsertOnSubmit(response);
				db.SubmitChanges();

				PreEvaluationSubQuestion.AddPreEvaluationSubQuestions(response.PreEvaluationResponseID, response.PreEvaluationQuestionID, dto);

				// Add any selected permits
				if (response.RequiresPermits)
				{
					PreEvaluationResponsePermitSet.AddPreEvaluationResponsePermitSets(response.PreEvaluationResponseID, dto.PermitSets);
				}

				return response.PreEvaluationResponseID;
			}
		}

		public static void UpdatePreEvaluationResponse(Domain.PreEvaluationResponse dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PreEvaluationResponse response = db.PreEvaluationResponses.Where(i => i.PreEvaluationResponseID == dto.ID).SingleOrDefault();

				if (response != null)
				{
					response.Name = dto.Name;
					response.Description = dto.Description;
					response.PreEvaluationQuestionID = dto.PreEvaluationQuestionID;
					response.RequiresPermits = dto.RequiresPermits;
					response.IsEndPoint = dto.IsEndPoint;
					response.EndPointMessage = dto.EndPointMessage;
					response.DateModified = DateTime.Now;
					db.SubmitChanges();

					PreEvaluationResponsePermitSet.DeletePreEvaluationResponsePermitSets(response.PreEvaluationResponseID);

					PreEvaluationSubQuestion.DeleteSubQuestionsForResponse(response.PreEvaluationResponseID);

					PreEvaluationSubQuestion.AddPreEvaluationSubQuestions(response.PreEvaluationResponseID, response.PreEvaluationQuestionID, dto);

					// Add any selected permits
					if (response.RequiresPermits)
					{
						PreEvaluationResponsePermitSet.AddPreEvaluationResponsePermitSets(response.PreEvaluationResponseID, dto.PermitSets);
					}
				}
			}
		}

		public static Domain.PreEvaluationResponse GetPreEvaluationResponse(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from responses in db.PreEvaluationResponses
					    where responses.PreEvaluationResponseID == responseID
					    select responses).FirstOrDefault();
			    if (q == null)
			    {
			        return null;
			    }

				return q.GetDomainObject(true, false);
			}
		}

		public static List<Domain.PreEvaluationResponse> GetPreEvaluationResponses(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.PreEvaluationResponse> responses = db.PreEvaluationResponses.Where(i => i.PreEvaluationQuestionID == questionID).Select(i => i.GetDomainObject(false, false)).ToList();
				return responses;
			}
		}

		public static void DeletePreEvaluationResponses(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.PreEvaluationResponse> responses = GetPreEvaluationResponses(questionID);
				foreach (Domain.PreEvaluationResponse response in responses)
				{
					DeletePreEvaluationResponse(response.ID);
				}
			}
		}

		public static void DeletePreEvaluationResponse(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PreEvaluationResponsePermitSet.DeletePreEvaluationResponsePermitSets(responseID);
                PreEvaluationSubQuestion.DeleteSubQuestionsForResponse(responseID);
                db.PreEvaluationResponses.DeleteOnSubmit(db.PreEvaluationResponses.First(x => x.PreEvaluationResponseID == responseID));
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
