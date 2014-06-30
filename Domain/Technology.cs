using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class Technology : BaseClass
	{
		public List<PermitSet> PermitSets { get; set; }

		public List<QuestionSet> QuestionSets { get; set; }
	}
}
