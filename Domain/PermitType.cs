﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain
{
	public class PermitType : BaseClass
	{		 		
		#region Constructors
		public PermitType() {}

		public PermitType(int id, string name)
		{
			ID = id;
			Name = name;
		}
		#endregion
	}
}