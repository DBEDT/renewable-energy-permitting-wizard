using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class Question : BaseClass
	{
		#region Properties
		public List<Response> Responses { get; set; }
		public Response SelectedResponse { get; set; }
		public int? ResponseID { get; set; }
		public int ParentResponseID { get; set; }
		public int QuestionTypeID { get; set; }
		public QuestionType QuestionType { get; set; }
		#endregion

		#region Constructors
		public Question()
		{ 
			Responses = new List<Response>();
			ParentResponseID = 0;
		}

		public Question(int id, string description) : this()
		{
			ID = id;
			Description = description;
		}
		#endregion
	}
}
