using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class QuestionType : BaseClass
	{
		#region Constructors
		public QuestionType() { }

		public QuestionType(int id, string name)
		{
			ID = id;
			Name = name;
		}
		#endregion
	}
}
