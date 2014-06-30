using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class User : BaseClass
	{
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		public string Organization { get; set; }
		public string Title { get; set; }
		public string TelephoneNumber { get; set; }

		public int LocationID { get; set; }
		public Location Location { get; set; }

		public int TechnologyID { get; set; }
		public Technology Technology { get; set; }

		public List<Evaluation> Evaluations { get; set; }
		public List<UserLoginHistory> LoginHistory { get; set; }

		public DateTime? LastLoginDate { get; set; }
		public string LastLoginDateDisplay
		{
			get
			{
				if (LastLoginDate.HasValue)
				{
					return String.Format("{0:g}", LastLoginDate.Value);
				}
				return "";
			}
		}

		public string ContactInfo
		{
			get
			{
				if (Name != "")
				{
					return Name + " (" + EmailAddress + ")";
				}
				return EmailAddress;
			}
		}

		public User()
		{

		}

	}
}
