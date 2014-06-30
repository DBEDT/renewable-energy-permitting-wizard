using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;

using System.Collections;
using HawaiiDBEDT.Domain.Enumerations;

namespace HawaiiDBEDT.Web.Admin
{
    using admin.controls;
    using include.controls;

	public partial class PermitDetail : System.Web.UI.Page
	{
		protected string ErrorText = "";
		private int permitID;
		private Permit permit;
		private List<Permit> availablePermits;

		public List<Permit> AvailablePermits
		{
			get
			{
				if (availablePermits == null || availablePermits.Count == 0)
				{
					availablePermits = HawaiiDBEDT.Data.DataAccess.PermitList;
					if (permitID != 0 && availablePermits.Find(i => i.ID == permitID) != null)
					{ 				// First remove the current permit
						availablePermits.RemoveAt(availablePermits.FindIndex(i => i.ID == permitID));
					}
					return availablePermits;
				}
				else
				{
					return availablePermits;
				}
			}

		}

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorText = "";
            GetPermit();

            if (!IsPostBack)
            {
                availablePermits = HawaiiDBEDT.Data.DataAccess.PermitList;
                DisplayPermitTypeList();
                DisplayPermitSets();
                DisplayDepartments();
                //DisplayDependencyPermits();
                DisplayPermit();
            }

            this.tlbPermitFinishedToStart.ElementsMoved += new EventHandler(TransferableListBoxElementsMoved);
            this.tlbPermitFinishedToFinish.ElementsMoved += new EventHandler(TransferableListBoxElementsMoved);
            this.tlbPermitStartedToStart.ElementsMoved += new EventHandler(TransferableListBoxElementsMoved);
        }

        private void TransferableListBoxElementsMoved(object sender, EventArgs e)
        {
            DisplayDependencyPermits();
            RefreshListBoxes();
        }

		private void GetPermit()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				permitID = int.Parse(Request.QueryString["id"]);
				permit = Data.Permit.GetPermit(permitID);
				int previousID = GetPreviousPermitID(permitID);
				if (previousID != -1)
				{
					hlPrevious.NavigateUrl = String.Format("edit.aspx?id={0}", GetPreviousPermitID(permitID));
				}
				else
				{
					hlPrevious.Visible = false;
				}

				int nextID = GetNextPermitID(permitID);
				if (nextID != -1)
				{
					hlNext.NavigateUrl = String.Format("edit.aspx?id={0}", GetNextPermitID(permitID));
				}
				else
				{
					hlNext.Visible = false;
					divSubmitNext.Visible = false;
				}
			}
			else
			{
				permitID = 0;
				permit = new Permit();
				divSubmitNext.Visible = false;
				hlPrevious.Visible = false;
				hlNext.Visible = false;
			}
		}

		private void DisplayPermitTypeList()
		{
			rcbPermitType.DataSource = HawaiiDBEDT.Data.DataAccess.PermitTypeList;
			rcbPermitType.DataBind();
		}

		private void DisplayPermitSets()
		{
            var permitSets = Data.PermitSet.GetPermitSets();
			if (permitID != 0)
			{
                this.tlbPermitSets.DestinationListItems = permit.PermitSets;
                foreach (var permitSet in permit.PermitSets)
                {
                    permitSets.RemoveAt(permitSets.FindIndex(i => i.ID == permitSet.ID));
                }
			}

            tlbPermitSets.SourceListItems = permitSets;
		}

		private void DisplayDepartments()
		{
			rcbDepartment.DataSource = HawaiiDBEDT.Data.DataAccess.DepartmentList;
			rcbDepartment.DataBind();

			rcbDepartment.Items.Insert(0, new ListItem("< please select >", ""));
		}

		private void DisplayPermit()
		{
			if (permitID != 0)
			{
				txtName.Text = permit.Name;
				txtShortName.Text = permit.ShortName;
				txtDescription.Text = permit.Description;
				if (permit.DepartmentID.HasValue)
				{
					rcbDepartment.SelectedValue = permit.DepartmentID.Value.ToString();
				}
				rcbPermitType.SelectedValue = permit.PermitType.ID.ToString();
				txtURL.Text = permit.URL;
				txtAppendixLink.Text = permit.AppendixLink;
				txtDuration.Text = permit.Duration.ToString();

				DisplaySelectedDependencyPermits();
			}
		}

		private void DisplaySelectedDependencyPermits()
		{
			if (permitID != 0)
			{
                var allPermits = new List<Permit>();
				foreach (PermitDependency dependency in permit.Dependencies)
				{
					if (dependency.DependencyType == PermitDependencyType.FinishToStart)
					{
                        this.tlbPermitFinishedToStart.DestinationListItems = dependency.Permits;
					}
					else if (dependency.DependencyType == PermitDependencyType.FinishToFinish)
					{
                        this.tlbPermitFinishedToFinish.DestinationListItems = dependency.Permits;
					}
					else if (dependency.DependencyType == PermitDependencyType.StartToStart)
					{
                        this.tlbPermitStartedToStart.DestinationListItems = dependency.Permits;
					}
                    allPermits.AddRange(dependency.Permits);
				}

                var availablePermitsList = AvailablePermits;
                foreach (var p in allPermits)
                {
                    availablePermitsList.RemoveAt(availablePermits.FindIndex(i => i.ID == p.ID));
                }

                this.tlbPermitFinishedToStart.SourceListItems = availablePermitsList;
                this.tlbPermitFinishedToFinish.SourceListItems = availablePermitsList;
                this.tlbPermitStartedToStart.SourceListItems = availablePermitsList;
			}
		}

		private void DisplayDependencyPermits()
		{
            this.tlbPermitFinishedToStart.SourceListItems = AvailablePermits;
            this.tlbPermitFinishedToStart.Rebind();
            this.tlbPermitFinishedToFinish.SourceListItems = AvailablePermits;
            this.tlbPermitFinishedToFinish.Rebind();
            this.tlbPermitStartedToStart.SourceListItems = AvailablePermits;
            this.tlbPermitStartedToStart.Rebind();
		}

        /*
         * TODO method seems to be unused
		private void DisplayDependency(PermitDependency dependency, List<Permit> permits)
		{
			if (dependency.DependencyType == PermitDependencyType.FinishToStart)
			{
				foreach (Permit permit1 in dependency.Permits)
				{
					lbDependencyDestination1.Items.Add(new RadListBoxItem(permit1.Name, permit1.ID.ToString()));
					permits.RemoveAt(permits.FindIndex(i => i.ID == permit1.ID));
				}
			}
			else if (dependency.DependencyType == PermitDependencyType.FinishToFinish)
			{
				foreach (Permit permit1 in dependency.Permits)
				{
					lbDependencyDestination2.Items.Add(new RadListBoxItem(permit1.Name, permit1.ID.ToString()));
					permits.RemoveAt(permits.FindIndex(i => i.ID == permit1.ID));
				}
			}
			else if (dependency.DependencyType == PermitDependencyType.StartToStart)
			{
				foreach (Permit permit1 in dependency.Permits)
				{
					lbDependencyDestination3.Items.Add(new RadListBoxItem(permit1.Name, permit1.ID.ToString()));
					permits.RemoveAt(permits.FindIndex(i => i.ID == permit1.ID));
				}
			}
		}
        */

		#region Update

		private bool UpdatePermit()
		{
			Page.Validate();
			try
			{
				int Duration = int.Parse(txtDuration.Text);
			}
			catch
			{
				ErrorText = "Please enter a valid duration.";
				return false;
			}

			if (Page.IsValid)
			{
				permit.Name = txtName.Text;
				permit.ShortName = txtShortName.Text;
				permit.Description = txtDescription.Text;
				permit.PermitType = new PermitType(int.Parse(rcbPermitType.SelectedValue), rcbPermitType.SelectedItem.Text);
				if (rcbDepartment.SelectedIndex > 0)
				{
					permit.DepartmentID = int.Parse(rcbDepartment.SelectedValue);
				}
				else
				{
					permit.DepartmentID = null;
				}
				permit.URL = txtURL.Text;
				permit.AppendixLink = txtAppendixLink.Text;
				permit.Duration = Int16.Parse(txtDuration.Text);

				UpdatePermitSets();
				UpdatePermitDependencies();

				if (permitID != 0)
				{
					Data.Permit.UpdatePermit(permit);
				}
				else
				{
					permitID = Data.Permit.AddPermit(permit);
				}
				return true;
			}
			return false;
		}

		private void UpdatePermitSets()
		{
            var permitSets = new List<PermitSet>();
            foreach (ListItem item in this.tlbPermitSets.DestinationListValues)
			{
				permitSets.Add(new PermitSet(int.Parse(item.Value), item.Text));
			}
			permit.PermitSets = permitSets;
		}

		private void UpdatePermitDependencies()
		{
			permit.Dependencies = new List<PermitDependency>();

            var permits = new List<Permit>();
            foreach (ListItem item in this.tlbPermitFinishedToStart.DestinationListValues)
			{
				permits.Add(new Permit(int.Parse(item.Value), item.Text));
			}
			permit.Dependencies.Add(new PermitDependency(PermitDependencyType.FinishToStart, permits));

            var permits2 = new List<Permit>();
            foreach (ListItem item in this.tlbPermitFinishedToFinish.DestinationListValues)
			{
				permits2.Add(new Permit(int.Parse(item.Value), item.Text));
			}
			permit.Dependencies.Add(new PermitDependency(PermitDependencyType.FinishToFinish, permits2));

            var permits3 = new List<Permit>();
            foreach (ListItem item in this.tlbPermitStartedToStart.DestinationListValues)
			{
				permits3.Add(new Permit(int.Parse(item.Value), item.Text));
			}
			permit.Dependencies.Add(new PermitDependency(PermitDependencyType.StartToStart, permits3));

		}
		#endregion

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("default.aspx", true);
		}

		protected void btnSumit_Click(object sender, EventArgs e)
		{
			if (UpdatePermit())
			{
				Response.Redirect("default.aspx?action=edit&result=success", true);
			}
		}

		protected void btnSubmitAnother_Click(object sender, EventArgs e)
		{
			if (UpdatePermit())
			{
				Response.Redirect("edit.aspx?confirm=true", true);
			}
		}

		protected void btnSubmitNext_Click(object sender, EventArgs e)
		{
			if (UpdatePermit())
			{
				int nextPermitID = GetNextPermitID(permitID);
				if (nextPermitID != -1)
				{
					Response.Redirect("edit.aspx?id=" + nextPermitID.ToString(), true);
				}
				else
				{
					Response.Redirect("edit.aspx", true);
				}
			}
		}

		#region Dependency ListBoxes

		/* The three listboxes are depedent on each other. A permit can be used only for one dependency so remove
		 * it from the other two listboxes when selected */
     /*   protected void RadListBox_Dropped(object sender, RadListBoxDroppedEventArgs e)
        {
            string destinationID = e.HtmlElementID;
            if (destinationID == lbDependency1.ClientID ||
                destinationID == lbDependency2.ClientID ||
                destinationID == lbDependency3.ClientID)
            {
                DisplayDependencyPermits();
            }
            RefreshListBoxes();
        }

        protected void RadListBox_Transferred(object sender, RadListBoxTransferredEventArgs e)
        {
            string destinationID = e.DestinationListBox.ClientID;
            if (destinationID == lbDependency1.ClientID ||
                destinationID == lbDependency2.ClientID ||
                destinationID == lbDependency3.ClientID)
            {
                DisplayDependencyPermits();
            }
            RefreshListBoxes();
        }*/

        private void RefreshListBoxes()
        {
            RefreshListBox(tlbPermitFinishedToStart);
            RefreshListBox(tlbPermitFinishedToFinish);
            RefreshListBox(tlbPermitStartedToStart);
        }

        private void RefreshListBox(TransferableListBox listBox)
        {
            foreach (ListItem item in listBox.DestinationListValues)
            {
                int selectedPermitID = int.Parse(item.Value);
                List<int> parentPermitIDs = Data.PermitDependency.GetParentPermitIDs(selectedPermitID);

                RemoveSelectedItems(selectedPermitID, tlbPermitFinishedToStart, parentPermitIDs);
                RemoveSelectedItems(selectedPermitID, tlbPermitFinishedToFinish, parentPermitIDs);
                RemoveSelectedItems(selectedPermitID, tlbPermitStartedToStart, parentPermitIDs);
            }
        }

        private void RemoveSelectedItems(int permitID, TransferableListBox listBox, List<int> parentPermitIDs)
        {
            listBox.SourceListRemoveItem(Convert.ToString(permitID));

            // Need to remove all parent permits of the selected permit
            foreach (int parentPermitID in parentPermitIDs)
            {
                listBox.SourceListRemoveItem(Convert.ToString(parentPermitID));
            }
        }

        #endregion

        // Used to let admins traverse through records
        public List<int> PermitIDList
        {
            get
            {
                if (Session["PermitIDList"] != null)
                {
                    return (List<int>)Session["PermitIDList"];
                }
                else
                {
                    return new List<int>();
                }
            }
            set
            {
                Session["PermitIDList"] = value;
            }
        }

        public int GetNextPermitID(int permitID)
        {
            if (PermitIDList.Count == 0)
            {
                return -1;
            }

            // current submission is last element
            if (permitID == PermitIDList[PermitIDList.Count - 1])
            {
                return -1;
            }

            for (int i = 0; i < PermitIDList.Count - 1; i++)
            {
                if (PermitIDList[i] == permitID)
                {
                    return PermitIDList[i + 1];
                }
            }

            // not found
            return -1;
        }

        public int GetPreviousPermitID(int permitID)
        {
            if (PermitIDList.Count == 0)
            {
                return -1;
            }
            // current submission is first submission
            if (permitID == PermitIDList[0])
            {
                return -1;
            }

            for (int i = 1; i < PermitIDList.Count; i++)
            {
                if (PermitIDList[i] == permitID)
                {
                    return PermitIDList[i - 1];
                }
            }

            // not found
            return -1;
        }
    }
}