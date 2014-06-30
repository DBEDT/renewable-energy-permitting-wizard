using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HawaiiDBEDT.Web.Controls
{
	public partial class GlobalNavControl : BaseControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsLoggedIn)
			{
				hlLogin.Visible = false;
				hlEvaluations.Visible = true;
				hlRegister.Visible = false;
				ltlSeparator1.Visible = hlEvaluations.Visible;
				hlProfile.Visible = hlEvaluations.Visible;
				ltlSeparator2.Visible = hlEvaluations.Visible;
				hlLogout.Visible = hlEvaluations.Visible;
			}
			else
			{
				string CurrentPage = HttpContext.Current.Request.Url.AbsolutePath;

				if (CurrentPage.Contains("/evaluate/"))
				{
					hlLogin.Visible = false;
					hlRegister.Visible = false;
					ltlSeparator0.Visible = false;
					ltlSeparator1.Visible = false;
					ltlSeparator2.Visible = false;

				}
			}
		}
	}
}