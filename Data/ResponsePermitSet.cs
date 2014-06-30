using HawaiiDBEDT.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class ResponsePermitSet
	{
		#region Domain Object Methods
		public Domain.ResponsePermitSet GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.ResponsePermitSet dto = new Domain.ResponsePermitSet();
			dto.ResponseID = this.ResponseID;
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
		public static int AddResponsePermitSets(int responseID, List<Domain.ResponsePermitSet> permitSets)
		{ 
			foreach (Domain.ResponsePermitSet permitSet in permitSets)
			{
				permitSet.ResponseID = responseID;
				AddResponsePermitSet(permitSet);
			}
			return 0;
		}

		public static int AddResponsePermitSet(Domain.ResponsePermitSet dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				ResponsePermitSet response = new ResponsePermitSet
				{
					ResponseID = dto.ResponseID,
					PermitSetID = dto.PermitSetID,
					LocationID = dto.LocationID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.ResponsePermitSets.InsertOnSubmit(response);
				db.SubmitChanges();

				return 0;
			}
		}

		public static Domain.ResponsePermitSet GetResponsePermitSet(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from responses in db.ResponsePermitSets
					    where responses.ResponseID == responseID
					    select responses).FirstOrDefault();

				return q.GetDomainObject(false, true);
			}
		}

		public static List<Domain.ResponsePermitSet> GetResponsePermitSets(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.ResponsePermitSet> responses = db.ResponsePermitSets.Where(i => i.ResponseID == responseID).Select(i => i.GetDomainObject(true, false)).ToList();
				return responses;
			}
		}

		public static List<Domain.Permit> GetPermitsForResponse(int responseID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permits in db.Permits
					    join permitsInSet in db.PermitSetPermits on permits.ID equals permitsInSet.PermitID
					    join responsePermitSets in db.ResponsePermitSets on permitsInSet.PermitSetID equals responsePermitSets.PermitSetID
					    where responsePermitSets.ResponseID == responseID && responsePermitSets.LocationID == locationID
					    select permits).Distinct();

				return q.Select(i => i.GetDomainObject(true, false)).ToList();
			}
		}

		public static List<Domain.PermitSet> GetPermitSetsForResponse(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join responsePermitSets in db.ResponsePermitSets on permitSets.ID equals responsePermitSets.PermitSetID
					    where responsePermitSets.ResponseID == responseID
					    select permitSets).Distinct();

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static List<Domain.PermitSet> GetPermitSetsForResponse(int responseID, int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join responsePermitSets in db.ResponsePermitSets on permitSets.ID equals responsePermitSets.PermitSetID
					    where responsePermitSets.ResponseID == responseID && responsePermitSets.LocationID == locationID
					    select permitSets).Distinct();

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

        public static List<PermitSetViewModel> GetPermitSetsForResponseWithPermitCount(int responseID, int locationID)
        {
            using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
            {
                var q = (from permitSets in db.PermitSets
                    join responsePermitSets in db.ResponsePermitSets on permitSets.ID equals
                        responsePermitSets.PermitSetID
                    join permitSetPermits in db.PermitSetPermits on permitSets.ID equals permitSetPermits.PermitSetID
                    join permits in db.Permits on permitSetPermits.PermitID equals permits.ID
                    where responsePermitSets.ResponseID == responseID && responsePermitSets.LocationID == locationID
                    group new {permitSets, permits} by new {permitSets.ID, permitSets.Name}
                    into g
                    select new PermitSetViewModel
                    {
                        PermitSetId = g.Key.ID,
                        PermitSetName = g.Key.Name,
                        PermitCount = g.Count(),
                        PermitUrl = g.Max(x => x.permits.URL)
                    }
                    ).Distinct().ToList();
                
                return q;
            }
        } 

		public static List<Domain.Location> GetLocationsForResponsePermitSet(int responseID, int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from locations in db.Locations
					    join responsePermitSets in db.ResponsePermitSets on locations.ID equals responsePermitSets.LocationID
					    where responsePermitSets.ResponseID == responseID && responsePermitSets.PermitSetID == permitSetID
					    select locations);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static void DeleteResponsePermitSets(int responseID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.ResponsePermitSets
							    where q.ResponseID == responseID
							    select q;

				db.ResponsePermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeletePermitSetInResponses(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.ResponsePermitSets
							    where q.PermitSetID == permitSetID
							    select q;

				db.ResponsePermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
