using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class EvaluationResponse
	{
		#region Domain Object Methods
		public Domain.EvaluationResponse GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.EvaluationResponse dto = new Domain.EvaluationResponse();
			dto.ID = this.EvaluationResponseID;
			dto.ResponseID = this.ResponseID;
			dto.QuestionID = this.QuestionID;
			dto.Page = this.PageNo;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;
		    dto.Note = this.Note;

			if (getChildrenObject)
			{
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		public static List<Domain.EvaluationResponse> GetResponses(int evaluationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				return db.EvaluationResponses.Where(i => i.EvaluationID == evaluationID).Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static void AddEvaluationResponse(int evaluationID, int pageNo, List<Domain.EvaluationResponse> responses)
		{
			DeleteEvaluationResponseByPage(evaluationID, pageNo);

			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				foreach (Domain.EvaluationResponse Response in responses)
				{
					EvaluationResponse eResponse = new EvaluationResponse
					{
						EvaluationID = evaluationID,
						ResponseID = Response.ResponseID,
						QuestionID = Response.QuestionID,
						PageNo = Response.Page,
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now,
                        Note = Response.Note
					};

					db.EvaluationResponses.InsertOnSubmit(eResponse);
					db.SubmitChanges();
				}
			}
		}

		public static void AddEvaluationResponse(int evaluationID, List<Domain.EvaluationResponse> responses)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				foreach (Domain.EvaluationResponse Response in responses)
				{
						EvaluationResponse eResponse = new EvaluationResponse
						{
							EvaluationID = evaluationID,
							ResponseID = Response.ResponseID,
							QuestionID = Response.QuestionID,
							PageNo = Response.Page,
							DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            Note = Response.Note
						};

						db.EvaluationResponses.InsertOnSubmit(eResponse);
						db.SubmitChanges();
					}
			}
		}

		public static void AddUpdateEvaluationResponse(int evaluationID, int questionID, int ResponseID, int pageNo)
		{
			DeleteEvaluationResponse(evaluationID, questionID);

			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				EvaluationResponse eResponse = new EvaluationResponse
				{
					EvaluationID = evaluationID,
					ResponseID = ResponseID,
					QuestionID = questionID,
					PageNo = pageNo,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.EvaluationResponses.InsertOnSubmit(eResponse);
				db.SubmitChanges();
			}
		}

		public static void DeleteEvaluationResponses(int evaluationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.EvaluationResponses
							    where q.EvaluationID == evaluationID
							    select q;

				db.EvaluationResponses.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		private static void DeleteEvaluationResponseByPage(int evaluationID, int pageNo)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.EvaluationResponses
							    where q.EvaluationID == evaluationID && q.PageNo == pageNo
							    select q;

				db.EvaluationResponses.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeleteEvaluationResponse(int evaluationID, int questionID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.EvaluationResponses
							    where q.EvaluationID == evaluationID && q.QuestionID == questionID
							    select q;

				db.EvaluationResponses.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

        public static int FindLastEvaluationStepFilled(int evaluationId)
        {
            using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
            {
                var pageNumbers = (from q in db.EvaluationResponses
                               where q.EvaluationID == evaluationId
                               select q.PageNo);
                return pageNumbers.Count() != 0 ? pageNumbers.Max() : 0;
            }
        }
	}
}
