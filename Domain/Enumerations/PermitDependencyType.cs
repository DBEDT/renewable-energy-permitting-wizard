using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain.Enumerations
{
	public enum PermitDependencyType
	{
		FinishToStart = 1,
		FinishToFinish = 2,
		StartToStart = 3
	}
}
