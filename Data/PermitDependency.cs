using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class PermitDependency
	{
		private const int MAX_DEPENDENCY_ID = 3;

		#region Domain Object Methods
		public Domain.PermitDependency GetDomainObject(bool getChildrenObject, bool getParent, List<Domain.Permit> Permits)
		{
			Domain.PermitDependency dto = new Domain.PermitDependency();
			dto.PermitID = this.PermitID;
			dto.DependentPermitID = this.DependentPermitID; // parent
			dto.DependencyType = (Domain.Enumerations.PermitDependencyType)this.PermitDependencyTypeID;	 
			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				dto.Permit = DataAccess.PermitList.Find(i => i.ID == this.PermitID);
				dto.Permits.Add(dto.Permit);
			}
			else
			{
				dto.Permit = Permits.Find(i => i.ID == this.PermitID);
				dto.Permits.Add(dto.Permit);
			}

			if (getParent)
			{
			}
			return dto;
		}

		public Domain.PermitDependency GetDomainObject(bool getChildrenObject, bool getParent)
		{
			return GetDomainObject(getChildrenObject, getParent, new List<Domain.Permit>());
		}
		#endregion

		#region CRUD 	

		public static void AddPermitDependencies(Domain.Permit dto)
		{
			int permitID = dto.ID;
			int dependencyTypeID;

			foreach (Domain.PermitDependency dependency in dto.Dependencies)
			{
				dependencyTypeID = (int)dependency.DependencyType;
				foreach (Domain.Permit permit in dependency.Permits)
				{
					AddPermitDependency(permitID, permit.ID, dependencyTypeID);
				}
			}
		}
	
		public static int AddPermitDependency(int permitID, int dependentPermitID, int dependencyTypeID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PermitDependency dependency = new PermitDependency
				{
					PermitID = permitID,
					DependentPermitID = dependentPermitID,
					PermitDependencyTypeID = dependencyTypeID,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};

				db.PermitDependencies.InsertOnSubmit(dependency);
				db.SubmitChanges();

				return 0;
			}
		}

		public static List<Domain.PermitDependency> GetPermitDependenciesAll(List<Domain.Permit> PermitList)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.PermitDependency> dependencies = db.PermitDependencies.Select(i => i.GetDomainObject(false, false, PermitList)).ToList();
				return dependencies;
			}
		}
		
		public static List<Domain.PermitDependency> GetPermitDependencies(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				// Need to group the dependencies by type
				List<Domain.PermitDependency> dependencies = new List<Domain.PermitDependency>();

				for (int i = 1; i <= MAX_DEPENDENCY_ID; i++)
				{
					dependencies.Add(GetPermitDependency(permitID, i));
				}				
				
				return dependencies;
			}			   
		}
					    
		// Returns flattened list of permits depended on by the specified permit

		public static List<int> GetParentPermitIDs(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<int> permitIDs = db.PermitDependencies.Where(i => i.PermitID == permitID).Select(i => i.DependentPermitID).ToList();
				return permitIDs;
			}
		}

		public static List<int> GetChildPermitIDs(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<int> permitIDs = db.PermitDependencies.Where(i => i.DependentPermitID == permitID).Select(i => i.PermitID).ToList();
				for (int i = 0; i < permitIDs.Count; i++)
				{
					permitIDs.AddRange(GetChildPermitIDs(permitIDs[i]));
				}
				return permitIDs;
			}
		}

		public static Domain.PermitDependency GetPermitDependency (int permitID, int dependencyTypeID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				// Need to group the dependencies by type
				Domain.PermitDependency dependency = new Domain.PermitDependency(dependencyTypeID);
				dependency.Permits = new List<Domain.Permit>();

				var q = (from permits in db.Permits
					    join dependencies in db.PermitDependencies on permits.ID equals dependencies.DependentPermitID
					    where dependencies.PermitID == permitID &&
					    dependencies.PermitDependencyTypeID == dependencyTypeID
					    orderby permits.Name
					    select new Domain.Permit() { ID = permits.ID, Name = permits.Name });

				dependency.Permits = q.ToList();
				return dependency;
			}
		}

		public static void UpdatePermitDependency(Domain.PermitDependency dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				PermitDependency dependency = db.PermitDependencies.Where(i => i.PermitID == dto.ID).SingleOrDefault();

				if (dependency != null)
				{
					dependency.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		} 

		public static void DeletePermitDependencies(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PermitDependencies
							    where q.PermitID == permitID
							    select q;

				db.PermitDependencies.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}

		// Used to delete permits
		public static void DeletePermitInDependencies(int permitID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var rowsToDelete = from q in db.PermitDependencies
							    where q.DependentPermitID == permitID
							    select q;

				db.PermitDependencies.DeleteAllOnSubmit(rowsToDelete);
				db.SubmitChanges();
			}
		}
		#endregion
	}
}