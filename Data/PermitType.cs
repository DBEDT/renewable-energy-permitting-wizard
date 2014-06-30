using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{							   
	public partial class PermitType
	{  		
		#region Domain Object Methods
		public Domain.PermitType GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.PermitType dto = new Domain.PermitType();
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
		public int AddPermitType(Domain.PermitType dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PermitType PermitType = new PermitType
				{
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.PermitTypes.InsertOnSubmit(PermitType);
				db.SubmitChanges();

				return 0;
			}
		}

		public static Domain.PermitType GetPermitType(int PermitTypeID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from PermitTypes in db.PermitTypes
					    where PermitTypes.ID == PermitTypeID
					    select PermitTypes).FirstOrDefault();

				return q.GetDomainObject(false, true);
			}
		}

		public static List<Domain.PermitType> GetPermitTypes()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.PermitType> PermitTypes = db.PermitTypes.Select(i => i.GetDomainObject(false, false)).ToList();
				return PermitTypes;
			}
		}

		public static void UpdatePermitType(Domain.PermitType dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PermitType PermitType = db.PermitTypes.Where(i => i.ID == dto.ID).SingleOrDefault();

				if (PermitType != null)
				{
					PermitType.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}

		}
		#endregion
	}
}
