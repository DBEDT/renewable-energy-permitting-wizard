using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class QuestionSet : BaseClass
	{
		#region Properties
		public List<Question> Questions { get; set; }
		public int TechnologyID { get; set; }
		public Technology Technology { get; set; }

		public string LocationNames
		{
			get
			{
				StringBuilder sb = new StringBuilder("");
				foreach (QuestionSetLocation location in Locations)
				{
					sb.Append(location.Location.Name + ", ");
				}

				string display = sb.ToString();
				if (display != "")
				{
					display = display.Substring(0, display.Length - 2);
				}
				return display;
			}
		}

		public List<int> LocationIDs { get; set; }
		public List<QuestionSetLocation> Locations { get; set; }
		public List<QuestionSetLocation> LocationsToDelete { get; set; }
		#endregion
		
		#region Constructors
		public QuestionSet()
		{
			Locations = new List<QuestionSetLocation>();
			LocationsToDelete = new List<QuestionSetLocation>();
			LocationIDs = new List<int>();
			Questions = new List<Question>();
		}
		#endregion
	}
}
