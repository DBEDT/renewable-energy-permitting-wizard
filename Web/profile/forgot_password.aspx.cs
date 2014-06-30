using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using System.Configuration;

namespace HawaiiDBEDT.Web.Profile
{
    using Data;

    public partial class ForgotPassword : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				pnlPasswordRequest.Visible = true;
				pnlConfirm.Visible = false;
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
		    if (Page.IsValid)
		    {
		        ResetPassword(txtEmail.Text);
		        pnlPasswordRequest.Visible = false;
		        pnlConfirm.Visible = true;
		    }
		}

		private void ResetPassword(string userEmailAddress)
		{
			pnlUserNotFound.Visible = false;

            int userId = Data.User.GetUserIDByEmail(userEmailAddress);
			if (userId != 0)
			{
			    var userAccountManager = new UserAccountManager();
                userAccountManager.ResetPasswordFor(userId, userEmailAddress);
			}
			else
			{
				pnlUserNotFound.Visible = true;
			}
		}

		protected void OnResetClick(object sender, EventArgs e)
		{
			Response.Redirect(Request.Url.AbsolutePath.ToString(), true);
		}
	}
}