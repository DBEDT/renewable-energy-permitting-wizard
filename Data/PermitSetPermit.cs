using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class PermitSetPermit
	{
		#region CRUD
		public static void AddPermitsToPermitSet(Domain.Permit dto)
		{
			int permitID = dto.ID;

			foreach (Domain.PermitSet permitSet in dto.PermitSets)
			{
				AddPermitToPermitSet(permitSet.ID, permitID);
			}
		}

		public static void AddPermitsToPermitSet(Domain.PermitSet dto)
		{
			int permitSetID = dto.ID;

			foreach (Domain.Permit permit in dto.Permits)
			{
				AddPermitToPermitSet(permitSetID, permit.ID);
			}
		}

		public static void AddPermitToPermitSet(int permitSetID, int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PermitSetPermit permitSetPermit = new PermitSetPermit
				{				
					PermitSetID = permitSetID,
					PermitID = permitID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.PermitSetPermits.InsertOnSubmit(permitSetPermit);
				db.SubmitChanges();
			}
		}

		/* List permit sets for a given permit */
		public static List<Domain.PermitSet> GetPermitSets(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join permitsInSet in db.PermitSetPermits on permitSets.ID equals permitsInSet.PermitSetID
					    join permits in db.Permits on permitsInSet.PermitID equals permits.ID
					    where permits.ID == permitID
					    select permitSets);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		/* List permits in a given permit set */
		public static List<Domain.Permit> GetPermitsInPermitSet(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permits in db.Permits
					    join permitsInSet in db.PermitSetPermits on permits.ID equals permitsInSet.PermitID
					    join permitSets in db.PermitSets on permitsInSet.PermitSetID equals permitSets.ID
					    where permitSets.ID == permitSetID
					    select permits);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}  

		public static void DeletePermitSetsForPermit(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PermitSetPermits
							    where q.PermitID == permitID
							    select q;

				db.PermitSetPermits.DeleteAllOnSubmit(rowsToDelete);	   
				db.SubmitChanges();
			}
		}

		public static void DeletePermitsForPermitSet(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PermitSetPermits
							    where q.PermitSetID == permitSetID
							    select q;

				db.PermitSetPermits.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
