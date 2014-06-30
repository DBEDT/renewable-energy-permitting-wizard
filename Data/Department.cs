using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class Department
	{  		
		
		#region Domain Object Methods
		public Domain.Department GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.Department dto = new Domain.Department(); 
			dto.ID = this.ID;
			dto.Name = this.Name;
			dto.Description = this.Description;
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
		public static int AddDepartment(Domain.Department dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Department department = new Department
				{
					Name = dto.Name,
					Description = dto.Description,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.Departments.InsertOnSubmit(department);
				db.SubmitChanges();

				return department.ID;
			}
		}

		public static void UpdateDepartment(Domain.Department dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				Department department = db.Departments.Where(i => i.ID == dto.ID).SingleOrDefault();

				if (department != null)
				{
					department.Name = dto.Name;
					department.Description = dto.Description;
					department.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}

		}
		public static Domain.Department GetDepartment(int departmentID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from departments in db.Departments
					    where departments.ID == departmentID
					    select departments).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static List<Domain.Department> GetDepartments()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.Department> departments = db.Departments.OrderBy(i => i.Name).Select(i => i.GetDomainObject(false, false)).ToList();
				return departments;
			}
		}

		public static bool DeleteDepartment(int departmentID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				// Delete department if it is not used as a nested department
				if (Permit.PermitExistsForDepartment(departmentID))
				{
					return false;
				}

				var rowsToDelete = from q in db.Departments
							    where q.ID == departmentID
							    select q;

				db.Departments.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();

				return true;
			}
		}
		#endregion
	}
}
