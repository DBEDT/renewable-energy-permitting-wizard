using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class TechnologyPermitSet
	{
		#region CRUD

		public static void AddPermitSetsToTechnology(Domain.Technology dto)
		{
			int technologyID = dto.ID;

			foreach (Domain.PermitSet permitSet in dto.PermitSets)
			{
				AddPermitSetToTechnology(technologyID, permitSet.ID);
			}
		}

		public static void AddPermitSetToTechnology(int technologyID, int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				TechnologyPermitSet technologyPermitSet = new TechnologyPermitSet
				{
					TechnologyID = technologyID,
					PermitSetID = permitSetID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.TechnologyPermitSets.InsertOnSubmit(technologyPermitSet);
				db.SubmitChanges();
			}
		}

		/* List permitSet sets for a given permitSet */
		public static List<Domain.Technology> GetTechnologys(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from technologys in db.Technologies
					    join permitSetsInSet in db.TechnologyPermitSets on technologys.ID equals permitSetsInSet.TechnologyID
					    join permitSets in db.PermitSets on permitSetsInSet.PermitSetID equals permitSets.ID
					    where permitSets.ID == permitSetID
					    select technologys);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		/* List permitSets in a given permitSet set */
		public static List<Domain.PermitSet> GetPermitSetsInTechnology(int technologyID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join permitSetsInSet in db.TechnologyPermitSets on permitSets.ID equals permitSetsInSet.PermitSetID
					    join technologys in db.Technologies on permitSetsInSet.TechnologyID equals technologys.ID
					    where technologys.ID == technologyID
					    select permitSets);

				return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		/* List permits in a given technology */
		public static List<Domain.Permit> GetPermitsInTechnology(int technologyID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    join permitSetsInSet in db.TechnologyPermitSets on permitSets.ID equals permitSetsInSet.PermitSetID
					    join technologys in db.Technologies on permitSetsInSet.TechnologyID equals technologys.ID
					    join permitSetPermits in db.PermitSetPermits on permitSets.ID equals permitSetPermits.PermitSetID
					    join permit in db.Permits on permitSetPermits.PermitID equals permit.ID
					    where technologys.ID == technologyID
					    select permit);

				return q.Select(i => i.GetDomainObject(true, false)).ToList().OrderBy(i => i.Name).ToList();
			}
		}

		public static void DeletePermitSet(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.TechnologyPermitSets
							    where q.PermitSetID == permitSetID
							    select q;

				db.TechnologyPermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		public static void DeletePermitSetsForTechnology(int technologyID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.TechnologyPermitSets
							    where q.TechnologyID == technologyID
							    select q;

				db.TechnologyPermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
