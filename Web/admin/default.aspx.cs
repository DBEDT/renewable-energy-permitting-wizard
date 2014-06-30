using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace HawaiiDBEDT.Web.admin
{
	public partial class _default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void lbLogout_Click(object sender, EventArgs e)
		{
			FormsAuthentication.SignOut();
			Response.Redirect("login.aspx", true);
		}
	}
}