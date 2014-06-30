using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class Technology
	{
		#region Domain Object Methods
		public Domain.Technology GetDomainObject(bool getChildren, bool getParent)
		{
			Domain.Technology dto = new Domain.Technology();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.Description = this.Description;
			dto.Order = this.TableDisplayOrder;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildren)
			{
				dto.PermitSets = TechnologyPermitSet.GetPermitSetsInTechnology(this.ID);
			}
			return dto;
		}

		public Domain.BaseClass GetBaseDomainObject()
		{
			Domain.Location dto = new Domain.Location();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.Order = this.TableDisplayOrder;
			dto.Description = this.Description;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;
			return dto;
		}
		#endregion

		#region CRUD
		public int AddTechnology(Domain.Technology dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Technology technology = new Technology
				{
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.Technologies.InsertOnSubmit(technology);
				db.SubmitChanges();

				return 0;
			}
		}

		public static void UpdateTechnology(Domain.Technology dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Technology technology = db.Technologies.Where(i => i.ID == dto.ID).SingleOrDefault();

				if (technology != null)
				{
					technology.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}

		}

		public static Domain.Technology GetTechnology(int technologyID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from technologies in db.Technologies
					    where technologies.ID == technologyID
					    select technologies).FirstOrDefault();

				return q.GetDomainObject(true, false);
			}
		}

		public static List<Domain.Technology> GetTechnologies()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Technology> technologies = db.Technologies.Select(i => i.GetDomainObject(false, false)).ToList();
				return technologies;
			}
		}

		public static List<Domain.BaseClass> GetTechnologyLookups()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.BaseClass> technologies = db.Technologies.Select(i => i.GetBaseDomainObject()).ToList();
				return technologies;
			}
		}

		#endregion
	}
}
