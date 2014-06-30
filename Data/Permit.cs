using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class Permit
	{
		#region Domain Object Methods
		public Domain.Permit GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.Permit dto = new Domain.Permit();
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.ShortName = this.ShortName;
			dto.PermitType = DataAccess.PermitTypeList.Find(i => i.ID == this.PermitTypeID);
			dto.Description = this.Description;
			dto.URL = this.URL;
			dto.DepartmentID = this.DepartmentID; 
			if (dto.DepartmentID.HasValue)
			{
				dto.Department = DataAccess.DepartmentList.Find(i => i.ID == dto.DepartmentID.Value);
			}
			dto.AppendixLink = this.AppendixLink;
			dto.Duration = this.Duration;
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				dto.Dependencies = PermitDependency.GetPermitDependencies(this.ID);
			}

			if (getParent)
			{
				dto.PermitSets = PermitSetPermit.GetPermitSets(this.ID);
			}
			return dto;
		}
		#endregion

		#region CRUD
		public static Domain.Permit GetPermit(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from permits in db.Permits
					    where permits.ID == permitID
					    select permits).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static int AddPermit(Domain.Permit dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Permit permit = new Permit
				{
					Name = dto.Name,  
					ShortName = dto.ShortName,
					Description = dto.Description,
					PermitTypeID = dto.PermitType.ID,
					URL = dto.URL,
					DepartmentID = dto.DepartmentID,
					AppendixLink = dto.AppendixLink,
					Duration = dto.Duration,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.Permits.InsertOnSubmit(permit);
				db.SubmitChanges();
				dto.ID = permit.ID;

				// Add permit dependencies
				PermitDependency.AddPermitDependencies(dto);

				// Add to selected permit sets
				PermitSetPermit.AddPermitsToPermitSet(dto);

				return permit.ID;
			}
		}

		public static void UpdatePermit(Domain.Permit dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Permit permit = db.Permits.Where(i => i.ID == dto.ID).SingleOrDefault();

				if (permit != null)
				{
					permit.Name = dto.Name;
					permit.ShortName = dto.ShortName;
					permit.Description = dto.Description;
					permit.PermitTypeID = dto.PermitType.ID;
					permit.URL = dto.URL;
					permit.DepartmentID = dto.DepartmentID;
					permit.AppendixLink = dto.AppendixLink;
					permit.Duration = dto.Duration;
					permit.DateModified = DateTime.Now;
					db.SubmitChanges();

					// Add permit dependencies after clearing them first
					PermitDependency.DeletePermitDependencies(dto.ID);
					PermitDependency.AddPermitDependencies(dto);

					// Add to selected permit sets after clearing them first
					PermitSetPermit.DeletePermitSetsForPermit(dto.ID);
					PermitSetPermit.AddPermitsToPermitSet(dto);	 
				}
			} 
		}

		public static List<Domain.Permit> GetPermits(bool getChildrenObject, bool getParentObject)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Permit> permits = db.Permits.OrderBy(i => i.Name).Select(i => i.GetDomainObject(getChildrenObject, getParentObject)).ToList();
				return permits;
			}
		}

		public static List<Domain.Permit> GetPermits(int permitTypeID, int permitSetID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{				
				if (permitSetID != 0 && permitTypeID == 0)
				{
					return PermitSetPermit.GetPermitsInPermitSet(permitSetID);
				}
				else if (permitSetID == 0 && permitTypeID != 0)
				{
					return GetPermits(permitTypeID);
				}
				else {  
					var q = (from permits in db.Permits
						    join permitsInSet in db.PermitSetPermits on permits.ID equals permitsInSet.PermitID
						    join permitSets in db.PermitSets on permitsInSet.PermitSetID equals permitSets.ID
						    where permitSets.ID == permitSetID && permits.PermitTypeID == permitTypeID
						    select permits);

					return q.Select(i => i.GetDomainObject(false, false)).ToList().OrderBy(i => i.Name).ToList();
				}
			}
		}

		public static List<Domain.Permit> GetPermits()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Permit> permits = db.Permits.OrderBy(i => i.Name).Select(i => i.GetDomainObject(false, false)).ToList();
				return permits;
			}
		}

		public static List<Domain.Permit> GetPermits(int permitTypeID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Permit> permits = db.Permits.Where(i => i.PermitTypeID == permitTypeID).OrderBy(i => i.Name).Select(i => i.GetDomainObject(false, false)).ToList();
				return permits;
			}
		}

		public static bool PermitExistsForDepartment(int departmentID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				return db.Permits.Where(i => i.DepartmentID == departmentID).Count() > 0;	 
			}
		}

		public static void DeletePermit(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				// Delete permit set relationships using this permit
				PermitSetPermit.DeletePermitSetsForPermit(permitID);
				PermitDependency.DeletePermitDependencies(permitID);
				PermitDependency.DeletePermitInDependencies(permitID);

				var rowsToDelete = from q in db.Permits
							    where q.ID == permitID
							    select q;

				db.Permits.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}
