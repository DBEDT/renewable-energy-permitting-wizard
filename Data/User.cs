using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data
{
	public partial class User
	{
		public Domain.User GetDomainObject(bool getChildrenObject, bool getParent)
		{
			Domain.User dto = new Domain.User();
			dto.ID = this.UserID;
			dto.Name = this.Name;
		    dto.Password = this.Password;
			dto.EmailAddress = this.EmailAddress;
			dto.Organization = this.Organization;
			dto.Title = this.Title;
			dto.TelephoneNumber = this.TelephoneNumber;
			dto.TechnologyID = this.TechnologyID;
			dto.Technology = DataAccess.TechnologyList.Find(i => i.ID == this.TechnologyID);

			dto.LocationID = this.LocationID;
			dto.Location = DataAccess.LocationList.Find(i => i.ID == this.LocationID);

			dto.LastLoginDate = this.DateLastLogin;

			dto.DateCreated = this.DateCreated;
			dto.DateModified = this.DateModified;

			if (getChildrenObject)
			{
				// Get login history
				dto.LoginHistory = UserLoginHistory.GetLoginHistory(this.UserID);
			}

			if (getParent)
			{
			}
			return dto;
		}

		#region CRUD

		public static List<Domain.User> GetUsers()
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				return db.Users.Select(i => i.GetDomainObject(false, false)).ToList();
			}
		}

		public static Domain.User GetUser(int userID)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				var q = (from Users in db.Users
					    where Users.UserID == userID
					    select Users).FirstOrDefault();

				return q.GetDomainObject(true, true);
			}
		}

		public static int GetUserIDByEmail(string emailAddress)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				User foundUser = db.Users.Where(i => i.EmailAddress == emailAddress).SingleOrDefault();

				if (foundUser != null)
				{
					return foundUser.UserID;
				}
				return 0;
			}
		}

		public static int AddUser(Domain.User dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				DateTime CurrentTime = DateTime.Now;

				User User = new User
				{
					Name = dto.Name,
					EmailAddress = dto.EmailAddress,
					Password = dto.Password,
					Title = dto.Title,
					Organization = dto.Organization,
					TelephoneNumber = dto.TelephoneNumber,
					TechnologyID = dto.TechnologyID,
					LocationID = dto.LocationID,
					DateCreated = CurrentTime,
					DateModified = CurrentTime
				};

				db.Users.InsertOnSubmit(User);
				db.SubmitChanges();
				dto.ID = User.UserID;

				UpdateUserLastLogin(dto.ID, CurrentTime);
				UserLoginHistory.AddUserLoginHistory(new Domain.UserLoginHistory(dto.ID, CurrentTime));
				return dto.ID;
			}
		}

		public static void UpdateUser(Domain.User dto)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				User foundUser = db.Users.Where(i => i.UserID == dto.ID).SingleOrDefault();

				if (foundUser != null)
				{
					foundUser.Name = dto.Name;
					foundUser.EmailAddress = dto.EmailAddress;
					foundUser.Title = dto.Title;
					foundUser.Organization = dto.Organization;
					foundUser.TelephoneNumber = dto.TelephoneNumber;
					foundUser.TechnologyID = dto.TechnologyID;
					foundUser.LocationID = dto.LocationID;
					if (dto.Password != "")
					{
						foundUser.Password = dto.Password;
					}
					foundUser.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}

		public static void UpdateUserLastLogin(int userID, DateTime lastLoginDate)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				User foundUser = db.Users.Where(i => i.UserID == userID).SingleOrDefault();

				if (foundUser != null)
				{
					foundUser.DateLastLogin = lastLoginDate;
					db.SubmitChanges();
				}
			}
		}

		public static void UpdateUserPassword(int userID, string password)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				User foundUser = db.Users.Where(i => i.UserID == userID).SingleOrDefault();

				if (foundUser != null)
				{
					foundUser.Password = password;
					foundUser.DateModified = DateTime.Now;
					db.SubmitChanges();
				}
			}
		}

		public static List<Domain.User> GetUsers(bool getChildrenObject, bool getParentObject)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				List<Domain.User> Users = db.Users.OrderBy(i => i.Name).Select(i => i.GetDomainObject(getChildrenObject, getParentObject)).ToList();
				return Users;
			}
		}
		#endregion

		#region Login
		public static int LoginUser(string emailAddress, string password)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				User foundUser = db.Users.Where(i => i.EmailAddress == emailAddress && i.Password == password).SingleOrDefault();

				if (foundUser != null)
				{
					int userID = foundUser.UserID;
					DateTime loginDateTime = DateTime.Now;

					UpdateUserLastLogin(userID, loginDateTime);
				    UserLoginHistory.AddUserLoginHistory(new Domain.UserLoginHistory() { UserID = userID, LoginDate = loginDateTime });
					return userID;
				}
				return 0;
			}

		}

		public static bool IsExistingUser(string emailAddress)
		{
			using (DBEDT_EvaluationDataContext db = new DBEDT_EvaluationDataContext())
			{
				User foundUser = db.Users.Where(i => i.EmailAddress == emailAddress).SingleOrDefault();

				if (foundUser != null)
				{
					return true;
				}
				return false;
			}
		}
		#endregion
	}
}
