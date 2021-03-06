namespace HawaiiDBEDT.Data
{
	using System.Configuration;
	using System;

	partial class DBEDT_EvaluationDataContext
	{
		public static ConnectionStringSettings ConnStringSettings = ConfigurationManager.ConnectionStrings["ConnectionString"];

		partial void OnCreated()
		{
			if (ConnStringSettings == null)
			{
				throw new ApplicationException("Connection String is invalid or not found");
			}
			this.Connection.ConnectionString = ConnStringSettings.ConnectionString;
		}
	}
}
