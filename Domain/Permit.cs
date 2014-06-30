using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HawaiiDBEDT.Domain.Enumerations;

namespace HawaiiDBEDT.Domain
{
	public class Permit : BaseClass
	{
		#region Properties
		public string ShortName { get; set; }
		public string ShortName2
		{
			get
			{
				return " ";
			}
		}
		public string URL { get; set; }
		public int? DepartmentID { get; set; }
		public string DepartmentName
		{
			get
			{
				if (DepartmentID.HasValue && Department != null)
				{
					return Department.Name;
				}
				else
				{
					return "";
				}
			}
		}
		public Department Department { get; set; }
		public string AppendixLink { get; set; }
		public PermitType PermitType { get; set; }
		public County County { get; set; }
		public int? CountyID { get; set; }
		public Int16 Duration { get; set; }
		public int StartDuration { get; set; }
		public int EndDuration
		{
			get
			{
				return StartDuration + Duration;
			}
		}

		public int StartDuration1
		{
			get
			{
				if (PermitType.ID == 1)
				{
					return StartDuration;
				}
				else
				{
					return -1;
				}
			}
		}
		public int EndDuration1
		{
			get
			{
				if (PermitType.ID == 1)
				{
					return StartDuration + Duration;
				}
				else
				{
					return -1;
				}
			}
		}
		public int StartDuration2
		{
			get
			{
				if (PermitType.ID == 2)
				{
					return StartDuration;
				}
				else
				{
					return -1;
				}
			}
		}
		public int EndDuration2
		{
			get
			{
				if (PermitType.ID == 2)
				{
					return StartDuration + Duration;
				}
				else
				{
					return -1;
				}
			}
		}

		public int StartDuration3
		{
			get
			{
				if (PermitType.ID >= 3)
				{
					return StartDuration;
				}
				else
				{
					return -1;
				}
			}
		}
		public int EndDuration3
		{
			get
			{
				if (PermitType.ID >= 3)
				{
					return StartDuration + Duration;
				}
				else
				{
					return -1;
				}
			}
		}

		/* List of permit sets this permit belongs */
		public List<PermitSet> PermitSets { get; set; }

		public List<PermitDependency> Dependencies { get; set; }

		//public List<Permit> FinishToStartDependencyPermits
		//{
		//     get
		//     {
		//          return Dependencies.Find(i => i.DependencyType == PermitDependencyType.FinishToStart).Permits;
		//     }
		//}

		//public List<Permit> FinishToFinishDependencyPermits
		//{
		//     get
		//     {
		//          return Dependencies.Find(i => i.DependencyType == PermitDependencyType.FinishToFinish).Permits;
		//     }
		//}

		//public List<Permit> StartToStartDependencyPermits
		//{
		//     get
		//     {
		//          return Dependencies.Find(i => i.DependencyType == PermitDependencyType.StartToStart).Permits;
		//     }
		//}

		#endregion


		#region Constructors
		public Permit() { }

		public Permit(int id)
		{
			ID = id;
		}

		public Permit(int id, string name)
		{
			ID = id;
			Name = name;
			StartDuration = 0;
		}
		#endregion

	}

	// Custom comparer for the Permit class
	public class PermitComparer : IEqualityComparer<Permit>
	{
		// Permits are equal if their names and permit numbers are equal.
		public bool Equals(Permit x, Permit y)
		{  
			//Check whether the compared objects reference the same data.
			if (Object.ReferenceEquals(x, y)) return true;

			//Check whether any of the compared objects is null.
			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return false;

			//Check whether the permits' properties are equal.
			return x.ID == y.ID;
		}

		// If Equals() returns true for a pair of objects 
		// then GetHashCode() must return the same value for these objects.

		public int GetHashCode(Permit permit)
		{
			//Check whether the object is null
			if (Object.ReferenceEquals(permit, null)) return 0;

			//Get hash code for the ID field if it is not null.
			int hashPermitID = permit.ID == 0 ? 0 : permit.ID.GetHashCode();

			//Get hash code for the Name field.
			int hashPermitName = permit.Name.GetHashCode();

			//Calculate the hash code for the permit.
			return hashPermitID ^ hashPermitName;
		}
	}
}
