using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class County
	{  		
		#region Domain Object Methods
		public Domain.County GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.County dto = new Domain.County();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
			}

			if (getParent)
			{
			}
			return dto;
		}
		#endregion

		#region CRUD
		public int AddCounty(Domain.County dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				County county = new County
				{
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.Counties.InsertOnSubmit(county);
				db.SubmitChanges();

				return 0;
			}
		}

		public static Domain.County GetCounty(int countyID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from counties in db.Counties
					    where counties.ID == countyID
					    select counties).FirstOrDefault();

				return q.GetDomainObject(false, true);
			}
		}

		public static List<Domain.County> GetCounties()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.County> counties = db.Counties.Select(i => i.GetDomainObject(false, false)).ToList();
				return counties;
			}
		}

		public static void UpdateCounty(Domain.County dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				County county = db.Counties.Where(i => i.ID == dto.ID).SingleOrDefault();

				if (county != null)
				{
					county.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}

		}
		#endregion
	}
}
