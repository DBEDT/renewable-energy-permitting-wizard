using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;


namespace HawaiiDBEDT.Web.Admin
{
	public partial class TechnologyDetail : System.Web.UI.Page
	{
		protected string ConfirmationText;
		protected int technologyID;
		private Technology technology;

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfirmationText = "";
			GeTechnology();

			if (!IsPostBack)
			{
				HandlePageAction();
				DisplayPermitSets();

			    this.tlbPermitSets.SourceListLabel = "<b>Available Questions</b><br />Only state and county questions are listed below.<br />"
                    + "(Please ensure each question is used only once per location/technology)";
			    this.tlbPermitSets.DestinationListLabel = "<b>Selected Questions</b><br />(Specify order as desired)";
			}
		}

		private void GeTechnology()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				technologyID = int.Parse(Request.QueryString["id"]);
				technology = Data.Technology.GetTechnology(technologyID);
				lblTechnology.Text = technology.Name;
			}
			else
			{
				Response.Redirect("default.aspx", true);
			}
		}

		private void HandlePageAction()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["qid"]))
			{
				if (Request.QueryString["action"] == "delete")
				{
					Data.QuestionSet.DeleteQuestionSet(int.Parse(Request.QueryString["qid"]));
					ConfirmationText = "Question set has been deleted.";
				}
			}
		}

		private void DisplayPermitSets()
		{
			var permitSets = Data.PermitSet.GetPermitSets();
		    this.tlbPermitSets.DestinationListItems = technology.PermitSets;
			foreach (var permitSet in technology.PermitSets)
			{
				permitSets.RemoveAt(permitSets.FindIndex(i => i.ID == permitSet.ID));
			}

		    this.tlbPermitSets.SourceListItems = permitSets;
		}


		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("default.aspx", true);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			// Redirect to current page
			if (UpdateTechnology())
			{
				Response.Redirect("edit.aspx?id=" + technologyID.ToString(), true);
			}
		}

		protected void btnSumit_Click(object sender, EventArgs e)
		{
			if (UpdateTechnology())
			{
				Response.Redirect("default.aspx?action=edit&result=success", true);
			}
		}

		#region Update

		private bool UpdateTechnology()
		{
			Page.Validate();
			if (Page.IsValid)
			{
				var permitSets = new List<PermitSet>();
				foreach (ListItem item in this.tlbPermitSets.DestinationListValues)
				{
					permitSets.Add(new PermitSet(int.Parse(item.Value), item.Text));
				}
				technology.PermitSets = permitSets;

				Data.TechnologyPermitSet.DeletePermitSetsForTechnology(technology.ID);
				Data.TechnologyPermitSet.AddPermitSetsToTechnology(technology);
                Data.Technology.UpdateTechnology(technology);

				return true;
			}
			return false;
		}
		#endregion
	}
}