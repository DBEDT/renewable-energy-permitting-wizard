using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class PreEvaluationResponsePermitSet : ResponsePermitSet
	{
		public int PreEvaluationResponseID { get; set; }

		#region Constructor
		public PreEvaluationResponsePermitSet(int permitSetID, int locationID)
		{
			this.PermitSetID = permitSetID;
			this.LocationID = locationID;
		}

		public PreEvaluationResponsePermitSet() { }
		#endregion
	}
}
