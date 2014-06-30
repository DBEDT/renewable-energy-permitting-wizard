using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class LocationPermitSet
	{ 
		#region CRUD
		public static void AddPermitSetsToLocation(Domain.Location dto)
		{
			int locationID = dto.ID;

			foreach (Domain.PermitSet permitSet in dto.PermitSets)
			{
				AddPermitSetToLocation(locationID, permitSet.ID);
			}
		}

		public static void AddPermitSetToLocation(int locationID, int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				LocationPermitSet locationPermitSet = new LocationPermitSet
				{
					LocationID = locationID,
					PermitSetID = permitSetID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.LocationPermitSets.InsertOnSubmit(locationPermitSet);
				db.SubmitChanges();
			}
		}

		/* List permitSet sets for a given permitSet */
		public static List<Domain.Location> GetLocations(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from locations in db.Locations
					    join permitSetsInSet in db.LocationPermitSets on locations.ID equals permitSetsInSet.LocationID
					    join permitSets in db.PermitSets on permitSetsInSet.PermitSetID equals permitSets.ID
					    where permitSets.ID == permitSetID
					    select locations);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		/* List permitSets in a given permitSet set */
		public static List<Domain.PermitSet> GetPermitSetsInLocation(int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join permitSetsInSet in db.LocationPermitSets on permitSets.ID equals permitSetsInSet.PermitSetID
					    join locations in db.Locations on permitSetsInSet.LocationID equals locations.ID
					    where locations.ID == locationID
					    select permitSets);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		/* List permits in a given location */
		public static List<Domain.Permit> GetPermitsInLocation(int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join permitSetsInSet in db.LocationPermitSets on permitSets.ID equals permitSetsInSet.PermitSetID
					    join locations in db.Locations on permitSetsInSet.LocationID equals locations.ID
					    join permitSetPermits in db.PermitSetPermits on permitSets.ID equals permitSetPermits.PermitSetID
					    join permit in db.Permits on permitSetPermits.PermitID equals permit.ID
					    where locations.ID == locationID
					    select permit);

				return q.Select(i => i.GetDomainObject(true, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static void DeletePermitSet(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.LocationPermitSets
							    where q.PermitSetID == permitSetID
							    select q;

				db.LocationPermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeletePermitSetsForLocation(int locationID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.LocationPermitSets
							    where q.LocationID == locationID
							    select q;

				db.LocationPermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
