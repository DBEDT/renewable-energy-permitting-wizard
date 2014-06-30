using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;


namespace HawaiiDBEDT.Web.Admin
{
    public partial class LocationDetail : System.Web.UI.Page
    {
        protected string ConfirmationText;
        protected int locationID;
        private Location location;

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmationText = "";
            GeLocation();

            if (!IsPostBack)
            {
                HandlePageAction();
                DisplayPermitSets();
            }
        }

        private void GeLocation()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                locationID = int.Parse(Request.QueryString["id"]);
                location = Data.Location.GetLocation(locationID);
                lblLocation.Text = location.Name;
            }
            else
            {
                Response.Redirect("default.aspx", true);
            }
        }

        private void HandlePageAction()
        {
        }

        private void DisplayPermitSets()
        {
            this.tlbPermitSets.DestinationListItems = location.PermitSets;
            this.tlbPermitSets.SourceListItems = Data.PermitSet.GetPermitSets();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx", true);
        }

        protected void btnSumit_Click(object sender, EventArgs e)
        {
            if (UpdateLocation())
            {
                Response.Redirect("default.aspx?action=edit&result=success", true);
            }
        }

        #region Update

        private bool UpdateLocation()
        {
            Page.Validate();
            if (Page.IsValid)
            {
                var permitSets = new List<PermitSet>();
                foreach (ListItem item in this.tlbPermitSets.DestinationListValues)
                {
                    permitSets.Add(new PermitSet(int.Parse(item.Value), item.Text));
                }

                location.PermitSets = permitSets;

                Data.LocationPermitSet.DeletePermitSetsForLocation(location.ID);
                Data.LocationPermitSet.AddPermitSetsToLocation(location);

                return true;
            }
            return false;
        }
        #endregion
    }
}