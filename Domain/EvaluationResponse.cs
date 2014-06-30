using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class EvaluationResponse : BaseClass
	{
		#region Properties
		public int Page { get; set; }
		public int QuestionID { get; set; }
		public int ResponseID { get; set; }
		public int EvaluationID { get; set; }
        public string Note { get; set; }
		#endregion

		#region Constructors
		public EvaluationResponse(int page, int questionID, int responseID)
		{
			Page = page;
			QuestionID = questionID;
			ResponseID = responseID;
		}

		public EvaluationResponse() {
			Page = 0;
		}
		#endregion

	}
}
