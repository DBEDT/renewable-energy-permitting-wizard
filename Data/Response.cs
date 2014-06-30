using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class Response
	{
		#region Domain Object Methods
		public Domain.Response GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.Response dto = new Domain.Response();
			dto.ID = this.ResponseID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.QuestionID = this.QuestionID;
			dto.RequiresPermits = this.RequiresPermits;
			dto.IsEndPoint = this.IsEndPoint;
			dto.EndPointMessage = this.EndPointMessage;
			dto.SubQuestionID = this.SubQuestionID;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				if (dto.SubQuestionID.HasValue)
				{
					dto.SubQuestion = Question.GetQuestion(dto.SubQuestionID.Value);
				}
				if (dto.RequiresPermits)
				{
					dto.UniquePermitSets = ResponsePermitSet.GetPermitSetsForResponse(dto.ID); 
				}
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		#region CRUD
		public static int AddResponse(Domain.Response dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Response response = new Response
				{
					Name = dto.Name,
					Description = dto.Description,
					QuestionID = dto.QuestionID,
					RequiresPermits = dto.RequiresPermits,
					IsEndPoint = dto.IsEndPoint,
					EndPointMessage = dto.EndPointMessage,
					SubQuestionID = dto.SubQuestionID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.Responses.InsertOnSubmit(response);
				db.SubmitChanges();

				// Add any selected permits
				if (response.RequiresPermits)
				{
					ResponsePermitSet.AddResponsePermitSets(response.ResponseID, dto.PermitSets);
				}

				return response.ResponseID;
			}
		} 

		public static void UpdateResponse(Domain.Response dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Response response = db.Responses.Where(i => i.ResponseID == dto.ID).SingleOrDefault();

				if (response != null)
				{
					response.Name = dto.Name;
					response.Description = dto.Description;
					response.QuestionID = dto.QuestionID;
					response.RequiresPermits = dto.RequiresPermits;
					response.IsEndPoint = dto.IsEndPoint;
					response.EndPointMessage = dto.EndPointMessage;
					response.SubQuestionID = dto.SubQuestionID;
					response.DateModified = DateTime.Now;
					db.SubmitChanges();

					ResponsePermitSet.DeleteResponsePermitSets(response.ResponseID);
					 
					// Add any selected permits
					if (response.RequiresPermits)
					{
						ResponsePermitSet.AddResponsePermitSets(response.ResponseID, dto.PermitSets);
					}
				}
			}
		}

		public static Domain.Response GetResponse(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from responses in db.Responses
					    where responses.ResponseID == responseID
					    select responses).FirstOrDefault();

				return q.GetDomainObject(true, false);
			}
		}

		public static List<Domain.Response> GetResponses(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Response> responses = db.Responses.Where(i => i.QuestionID == questionID).Select(i => i.GetDomainObject(false, false)).ToList();
				return responses;
			}
		}

		public static void DeleteResponsesForQuestion(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Response> responses = GetResponses(questionID);
				foreach (Domain.Response response in responses)
				{
					DeleteResponse(response.ID);
				}
			}
		}

		public static bool IsQuestionNestedInResponse(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				if (db.Responses.Where(i => i.SubQuestionID == questionID).Count() > 0)
				{
					return true;
				}
				return false;
			}
		}

		public static void DeleteNestedQuestion(int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Response> responses = db.Responses.Where(i => i.SubQuestionID == questionID).ToList();

				foreach (Response response in responses)
				{
					response.SubQuestionID = null;
					response.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}

		public static void DeleteResponse(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				ResponsePermitSet.DeleteResponsePermitSets(responseID);

				var rowsToDelete = from q in db.Responses
							    where q.ResponseID == responseID
							    select q;

				db.Responses.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
