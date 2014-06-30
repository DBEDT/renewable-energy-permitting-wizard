using System;
using System.Web.Security;

namespace HawaiiDBEDT.Web.Profile
{
	public partial class Logout : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			LogoutUser();
		    if (string.IsNullOrEmpty(Request.QueryString["showMsg"]))
		    {
                FormsAuthentication.RedirectToLoginPage();
		    }
		}

		private void LogoutUser()
		{
			FormsAuthentication.SignOut();
			CurrentUser = null;
			
			Session.RemoveAll();
			Session.Clear();
			Session.Abandon();
		}
	}
}