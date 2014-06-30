using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class PreEvaluationQuestion : BaseClass
	{
		public string LookupClassName { get; set; }
		public List<BaseClass> LookupOptions { get; set; }
		public bool AllowCustomResponse { get; set; }
		public int? SubQuestionTypeID { get; set; }

		public List<PreEvaluationResponse> Responses { get; set; }
	}
}
