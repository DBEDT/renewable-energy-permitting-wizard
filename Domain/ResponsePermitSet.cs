using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class ResponsePermitSet : BaseClass
	{
		#region Properties
		public int ResponseID { get; set; }
		public int PermitSetID { get; set; }
		public PermitSet PermitSet { get; set; }
		public int LocationID { get; set; }
		public Location Location { get; set; }
		#endregion

		#region Constructor
		public ResponsePermitSet(int permitSetID, int locationID)
		{
			this.PermitSetID = permitSetID;
			this.LocationID = locationID;
		}

		public ResponsePermitSet() { }
		#endregion
	}
}
