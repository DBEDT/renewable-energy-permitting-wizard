using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class Location : BaseClass
	{
		public List<PermitSet> PermitSets { get; set; }
		public int CountyID { get; set; }
		public County County { get; set; }
	}
}
