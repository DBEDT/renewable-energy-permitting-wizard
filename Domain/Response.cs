using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class Response : BaseClass
	{
		#region Properties
		public int QuestionID { get; set; }
		public bool RequiresPermits { get; set; }
		public bool IsEndPoint { get; set; }
		public string EndPointMessage { get; set; }
		public int? SubQuestionID { get; set; }
		public string QuestionGroupName
		{
			get
			{
				return "qGroup_" + QuestionID.ToString() + "_";
			}
		}

		public List<PermitSet> UniquePermitSets { get; set; }	  // Contains just the list of permit sets used by the response
		public List<ResponsePermitSet> PermitSets { get; set; }
		public Question SubQuestion { get; set; }
		#endregion

		#region Constructors
		public Response()
		{
			UniquePermitSets = new List<PermitSet>();
			PermitSets = new List<ResponsePermitSet>();
		}
		#endregion
	}
}
