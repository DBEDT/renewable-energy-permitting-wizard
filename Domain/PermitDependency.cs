using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HawaiiDBEDT.Domain.Enumerations;

namespace HawaiiDBEDT.Domain
{
	public class PermitDependency : BaseClass
	{
		#region Properties
		public PermitDependencyType DependencyType { get; set; }
		public List<Permit> Permits { get; set; }
		public Permit Permit { get; set; }
		public int PermitID { get; set; }  // child
		public int DependentPermitID { get; set; } // parent
		#endregion


		#region Constructors
		public PermitDependency() {
			Permits = new List<Permit>();
		}

		public PermitDependency(int dependencyTypeID)
		{
			DependencyType = (PermitDependencyType)dependencyTypeID;
			Permits = new List<Permit>();
		}

		public PermitDependency(int dependencyTypeID, int permitID)
		{
			DependencyType = (PermitDependencyType)dependencyTypeID;
			Permit = new Permit(permitID);
			Permits = new List<Permit>();
			Permits.Add(Permit);
		}

		public PermitDependency(PermitDependencyType dependencyType, List<Permit> permits)
		{
			DependencyType = dependencyType;
			Permits = permits;
		}
		#endregion
	}
}
