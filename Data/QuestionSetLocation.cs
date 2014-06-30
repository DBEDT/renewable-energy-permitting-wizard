using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class QuestionSetLocation
	{
		#region Domain Object Methods
		public Domain.QuestionSetLocation GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.QuestionSetLocation dto = new Domain.QuestionSetLocation();
			dto.ID = this.ID;
			dto.Order = this.QuestionSetOrder;
			dto.LocationID = this.LocationID;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				dto.Location = Data.DataAccess.LocationList.Where(i => i.ID == this.LocationID).FirstOrDefault();
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		#region CRUD
		public static void AddQuestionSetLocations(Domain.QuestionSet questionSet)
		{
			foreach (Domain.QuestionSetLocation location in questionSet.Locations)
			{
				AddQuestionSetLocation(location, questionSet.ID);
			}
		}

		public static int AddQuestionSetLocation(Domain.QuestionSetLocation dto, int questionSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				QuestionSetLocation location = new QuestionSetLocation
				{
					QuestionSetID = questionSetID,
					LocationID = dto.LocationID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				// Set quest set order to max order + 1
				byte? maxOrder = db.QuestionSetLocations.Where(i => i.LocationID == dto.LocationID).Max(i => (byte?)i.QuestionSetOrder);
				byte order = ++maxOrder ?? 1;
				location.QuestionSetOrder = order;

				db.QuestionSetLocations.InsertOnSubmit(location);
				db.SubmitChanges();

				dto.ID = location.ID;

				return location.ID;
			}
		}

		public static void UpdateQuestionSetLocations(Domain.QuestionSet questionSet)
		{
			foreach (Domain.QuestionSetLocation location in questionSet.Locations)
			{
				if (!QuestionSetLocationExists(questionSet.ID, location.LocationID))
				{
					AddQuestionSetLocation(location, questionSet.ID);
				}
			}

			foreach (Domain.QuestionSetLocation location in questionSet.LocationsToDelete)
			{
				DeleteQuestionSetLocation(questionSet.ID, location.LocationID);
			}
		}

		private static bool QuestionSetLocationExists(int questionSetID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				QuestionSetLocation location = db.QuestionSetLocations.Where(i => i.QuestionSetID == questionSetID && i.LocationID == locationID).SingleOrDefault();

				if (location != null)
				{
					return true;
				}
				return false;
			}
		}

		public static List<Domain.QuestionSetLocation> GetQuestionSetLocations(int questionSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.QuestionSetLocation> locations = db.QuestionSetLocations.Where(i => i.QuestionSetID == questionSetID).Select(i => i.GetDomainObject(true, false)).ToList();
				return locations;
			}
		}

		public static void UpdateQuestionSetOrder(List<Domain.QuestionSet> questionSets, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				foreach (Domain.QuestionSet q in questionSets)
				{
					QuestionSetLocation location = db.QuestionSetLocations.Where(i => i.QuestionSetID == q.ID && i.LocationID == locationID).SingleOrDefault();

					if (location != null)
					{
						location.QuestionSetOrder = q.Order;
						location.DateModified = DateTime.Now;
						db.SubmitChanges();
					}
				}
			}
		}

		public static List<Domain.QuestionSetLocation> GetQuestionSetLocationsDetails(int questionSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.QuestionSetLocation> questionSets = db.QuestionSetLocations.Where(i => i.QuestionSetID == questionSetID).Select(i => i.GetDomainObject(true, false)).ToList();
				return questionSets;
			}
		}

		public static void DeleteQuestionSetLocation(int questionSetID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.QuestionSetLocations
							    where q.QuestionSetID == questionSetID && q.LocationID == locationID
							    select q;

				db.QuestionSetLocations.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeleteQuestionSetLocationForQuestionSet(int questionSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.QuestionSetLocations
							    where q.QuestionSetID == questionSetID
							    select q;

				db.QuestionSetLocations.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeleteQuestionSetLocationForLocation(int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.QuestionSetLocations
							    where q.LocationID == locationID
							    select q;

				db.QuestionSetLocations.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
