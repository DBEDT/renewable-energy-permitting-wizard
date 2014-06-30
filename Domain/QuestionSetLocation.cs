using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class QuestionSetLocation : BaseClass
	{
		#region Properties
		public int QuestionSetID { get; set; }
		public int LocationID { get; set; }
		public Location Location { get; set; }
		public int QuestionOrder { get; set; }
		#endregion

		#region Constructors
		public QuestionSetLocation() {}

		public QuestionSetLocation(int locationID)
		{
			LocationID = locationID;
		}

		public QuestionSetLocation(int questionSetID, int locationID)
		{
			QuestionSetID = questionSetID;
			LocationID = locationID;
		}
		#endregion
	}
}
