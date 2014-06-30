using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class PreEvaluationResponse : Response
	{
		public int PreEvaluationQuestionID { get; set; }
		public List<Question> SubQuestions { get; set; }
		public new string QuestionGroupName
		{
			get
			{
				return "qGroup_" + PreEvaluationQuestionID.ToString() + "_";
			}
		}

		public new List<PreEvaluationResponsePermitSet> PermitSets { get; set; }

		#region Constructors
		public PreEvaluationResponse()
		{
			SubQuestions = new List<Question>();
			PermitSets = new List<PreEvaluationResponsePermitSet>();
		}
		#endregion
	}
}
