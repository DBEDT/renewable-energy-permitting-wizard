using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	/// <summary>
	///  All classes have these same fields in common
	/// </summary>
	public class BaseClass
	{
		#region Properties
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ToolTip { get; set; }
		public int Order { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		#endregion
	}
}
