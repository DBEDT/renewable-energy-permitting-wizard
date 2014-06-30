using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class UserLoginHistory
	{
		public Domain.UserLoginHistory GetDomainObject()
		{
			Domain.UserLoginHistory dto = new Domain.UserLoginHistory();
			dto.LoginDate = this.LoginDate;
			return dto;
		}

		#region CRUD


		public static List<Domain.UserLoginHistory> GetLoginHistory(int userID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				return db.UserLoginHistories.Where(i => i.UserID == userID).Select(i => i.GetDomainObject()).ToList();
			}
		}

		public static void AddUserLoginHistory(Domain.UserLoginHistory dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				UserLoginHistory history = new UserLoginHistory
				{
					UserID = dto.UserID,
					LoginDate = dto.LoginDate
				};

				db.UserLoginHistories.InsertOnSubmit(history);
				db.SubmitChanges();
			}
		}

		#endregion
	}
}
