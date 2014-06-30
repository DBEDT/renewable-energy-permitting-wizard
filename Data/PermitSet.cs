using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class PermitSet
	{
		#region Domain Object Methods
		public Domain.PermitSet GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.PermitSet dto = new Domain.PermitSet();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;
			if (getChildrenObject)
			{
				dto.Permits = PermitSetPermit.GetPermitsInPermitSet(this.ID);
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		#region CRUD
		public static int AddPermitSet(Domain.PermitSet dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PermitSet permitSet = new PermitSet
				{
					Name = dto.Name,
					Description = dto.Description,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.PermitSets.InsertOnSubmit(permitSet);
				db.SubmitChanges();

				dto.ID = permitSet.ID;

				// Add permit dependencies.
				PermitSetPermit.AddPermitsToPermitSet(dto);
				return permitSet.ID;
			}
		}

		public static void UpdatePermitSet(Domain.PermitSet dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PermitSet permitSet = db.PermitSets.Where(i => i.ID == dto.ID).SingleOrDefault();

				if (permitSet != null)
				{
					permitSet.Name = dto.Name;
					permitSet.Description = dto.Description;
					permitSet.DateModified = DateTime.Now;
					db.SubmitChanges();

					// Add permit dependencies. 
					PermitSetPermit.DeletePermitsForPermitSet(dto.ID);
					PermitSetPermit.AddPermitsToPermitSet(dto);
				}
			}
		}

		public static Domain.PermitSet GetPermitSet(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permitSets in db.PermitSets
					    where permitSets.ID == permitSetID
					    select permitSets).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static List<Domain.PermitSet> GetPermitSets()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.PermitSet> permitSets = db.PermitSets.OrderBy(i => i.Name).Select(i => i.GetDomainObject(false, false)).ToList();
				return permitSets;
			}
		}
  
		public static void DeletePermitSet(int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				// Delete any permit set relationship using this permit set
				PermitSetPermit.DeletePermitsForPermitSet(permitSetID);
				ResponsePermitSet.DeletePermitSetInResponses(permitSetID);
				TechnologyPermitSet.DeletePermitSet(permitSetID);
				LocationPermitSet.DeletePermitSet(permitSetID);

				var rowsToDelete = from q in db.PermitSets
							    where q.ID == permitSetID
							    select q;

				db.PermitSets.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
