using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Data.ViewModels;
using HawaiiDBEDT.Domain;

namespace HawaiiDBEDT.Web.Controls
{
	public partial class permit_set : BaseControl
	{

		protected int permitSetID;
		protected string permitSetName;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (!String.IsNullOrEmpty(Request.QueryString["id"]))
				{
					permitSetID = int.Parse(Request.QueryString["id"]);
					PermitSet permitSet = Data.PermitSet.GetPermitSet(permitSetID);
					permitSetName = permitSet.Name;
					lblPermitSetName.Text = permitSetName;
					rptPermits.DataSource = permitSet.Permits; //Data.PermitSetPermit.GetPermitsInPermitSet(permitSetID);
					rptPermits.DataBind();
				}
			}
		}

	    protected string DisplayPermit(object dataItem)
	    {
	        Permit p = (Permit) dataItem;

	        if (p.URL.Equals(""))
	        {
	            return p.Name;
	        }
	        else
	        {
                return string.Format("<a href='{0}' target='_blank'>{1}</a>", p.URL, p.Name);
	        }
	    }
	}
}