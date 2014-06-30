using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class PermitSet : BaseClass
	{
		public List<Permit> Permits { get; set; }
		public County County { get; set; }
		public int? CountyID { get; set; }

		#region Constructors
		public PermitSet() {}

		public PermitSet(int id, string name)
		{
			ID = id;
			Name = name;
		}
		#endregion
	}

	// Custom comparer for the Permit class
	public class PermitSetComparer : IEqualityComparer<PermitSet>
	{
		// Permits are equal if their names and permit numbers are equal.
		public bool Equals(PermitSet x, PermitSet y)
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

		public int GetHashCode(PermitSet permitSet)
		{
			//Check whether the object is null
			if (Object.ReferenceEquals(permitSet, null)) return 0;

			//Get hash code for the ID field if it is not null.
			int hashPermitSetID = permitSet.ID == 0 ? 0 : permitSet.ID.GetHashCode();

			//Get hash code for the Name field.
			int hashPermitSetName = permitSet.Name.GetHashCode();

			//Calculate the hash code for the permit.
			return hashPermitSetID ^ hashPermitSetName;
		}
	}
}
