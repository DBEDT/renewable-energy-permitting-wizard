using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace HawaiiDBEDT.Web.Admin
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void btnSumit_Click(object sender, EventArgs e)
		{
			if (FormsAuthentication.Authenticate(txtUserName.Text, txtPassword.Text))
			{
				FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, chkPersistLogin.Checked);
			}
			else {
				lblError.Text = "Your username and/or password are incorrect. Please try again.";
			}	  
		}
	}
}