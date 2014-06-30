using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;


namespace HawaiiDBEDT.Web.Admin
{
	public partial class PermitSetDetail : System.Web.UI.Page
	{
		private int permitSetID;
		private PermitSet permitSet;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetPermitSet();
			if (!IsPostBack)
			{
				DisplayPermits();
				DisplayPermitSet();
			}
		}  

		private void GetPermitSet()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				permitSetID = int.Parse(Request.QueryString["id"]);

				permitSet = Data.PermitSet.GetPermitSet(permitSetID);
			}
			else
			{
				permitSetID = 0;
				permitSet = new PermitSet();
			}
		}  

		private void DisplayPermits()
		{
			List<Permit> permits = Data.Permit.GetPermits();

			if (permitSetID != 0)
			{
			    this.tlbPermits.DestinationListItems = permitSet.Permits;
				foreach (Permit permit in permitSet.Permits)
				{
					permits.RemoveAt(permits.FindIndex(i => i.ID == permit.ID));
				}
			}

            this.tlbPermits.SourceListItems = permits;
		}

		private void DisplayPermitSet()
		{
			if (permitSetID != 0)
			{
				txtName.Text = permitSet.Name;
				txtDescription.Text = permitSet.Description;
			}
		}

		protected void btnSumit_Click(object sender, EventArgs e)
		{
			if (UpdatePermitSet())
			{
				Response.Redirect("default.aspx?action=edit&result=success", true);
			}
		}

		private bool UpdatePermitSet()
		{
			Page.Validate();

			if (Page.IsValid)
			{
				permitSet.Name = txtName.Text;
				permitSet.Description = txtDescription.Text;
				UpdatePermits();

				if (permitSetID != 0)
				{
					Data.PermitSet.UpdatePermitSet(permitSet);
				}
				else
				{
					Data.PermitSet.AddPermitSet(permitSet);
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		private void UpdatePermits()
		{
			var permits = new List<Permit>();
			foreach (ListItem item in this.tlbPermits.DestinationListValues)
			{
				permits.Add(new Permit(int.Parse(item.Value), item.Text));
			}
			permitSet.Permits = permits;
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("default.aspx", true);
		}

		protected void btnSubmitAnother_Click(object sender, EventArgs e)
		{
			if (UpdatePermitSet())
			{
				Response.Redirect("edit.aspx?confirm=true", true);
			}
		}
	}
}