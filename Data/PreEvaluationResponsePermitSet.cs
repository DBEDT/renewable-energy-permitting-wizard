using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class PreEvaluationResponsePermitSet
	{
		#region Domain Object Methods
		public Domain.PreEvaluationResponsePermitSet GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.PreEvaluationResponsePermitSet dto = new Domain.PreEvaluationResponsePermitSet();
			dto.PreEvaluationResponseID = this.PreEvaluationResponseID;
			dto.PermitSetID = this.PermitSetID;
			dto.LocationID = this.LocationID;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				dto.PermitSet = PermitSet.GetDomainObject(false, false);
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		#region CRUD
		public static int AddPreEvaluationResponsePermitSets(int responseID, List<Domain.PreEvaluationResponsePermitSet> permitSets)
		{
			foreach (Domain.PreEvaluationResponsePermitSet permitSet in permitSets)
			{
				permitSet.PreEvaluationResponseID = responseID;
				AddPreEvaluationResponsePermitSet(permitSet);
			}
			return 0;
		}

		public static int AddPreEvaluationResponsePermitSet(Domain.PreEvaluationResponsePermitSet dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PreEvaluationResponsePermitSet response = new PreEvaluationResponsePermitSet
				{
					PreEvaluationResponseID = dto.PreEvaluationResponseID,
					PermitSetID = dto.PermitSetID,
					LocationID = dto.LocationID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.PreEvaluationResponsePermitSets.InsertOnSubmit(response);
				db.SubmitChanges();

				return 0;
			}
		}

		public static Domain.PreEvaluationResponsePermitSet GetPreEvaluationResponsePermitSet(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from responses in db.PreEvaluationResponsePermitSets
					    where responses.PreEvaluationResponseID == responseID
					    select responses).FirstOrDefault();

				return q.GetDomainObject(false, true);
			}
		}

		public static List<Domain.PreEvaluationResponsePermitSet> GetPreEvaluationResponsePermitSets(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.PreEvaluationResponsePermitSet> responses = db.PreEvaluationResponsePermitSets.Where(i => i.PreEvaluationResponseID == responseID).Select(i => i.GetDomainObject(true, false)).ToList();
				return responses;
			}
		}

		public static List<Domain.Permit> GetPermitsForPreEvaluationResponse(int responseID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permits in db.Permits
					    join permitsInSet in db.PermitSetPermits on permits.ID equals permitsInSet.PermitID
					    join responsePermitSets in db.PreEvaluationResponsePermitSets on permitsInSet.PermitSetID equals responsePermitSets.PermitSetID
					    where responsePermitSets.PreEvaluationResponseID == responseID && responsePermitSets.LocationID == locationID
					    select permits).Distinct();

				return q.Select(i => i.GetDomainObject(true, false)).ToList();
			}
		}

		public static List<Domain.PermitSet> GetPermitSetsForPreEvaluationResponse(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join responsePermitSets in db.PreEvaluationResponsePermitSets on permitSets.ID equals responsePermitSets.PermitSetID
					    where responsePermitSets.PreEvaluationResponseID == responseID
					    select permitSets).Distinct();

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static List<Domain.PermitSet> GetPermitSetsForPreEvaluationResponse(int responseID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join responsePermitSets in db.PreEvaluationResponsePermitSets on permitSets.ID equals responsePermitSets.PermitSetID
					    where responsePermitSets.PreEvaluationResponseID == responseID && responsePermitSets.LocationID == locationID
					    select permitSets).Distinct();

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static List<Domain.Location> GetLocationsForPreEvaluationResponsePermitSet(int responseID, int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from locations in db.Locations
					    join responsePermitSets in db.PreEvaluationResponsePermitSets on locations.ID equals responsePermitSets.LocationID
					    where responsePermitSets.PreEvaluationResponseID == responseID && responsePermitSets.PermitSetID == permitSetID
					    select locations);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static void DeletePreEvaluationResponsePermitSets(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PreEvaluationResponsePermitSets
							    where q.PreEvaluationResponseID == responseID
							    select q;

				db.PreEvaluationResponsePermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeletePermitSetInPreEvaluationResponses(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PreEvaluationResponsePermitSets
							    where q.PermitSetID == permitSetID
							    select q;

				db.PreEvaluationResponsePermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
