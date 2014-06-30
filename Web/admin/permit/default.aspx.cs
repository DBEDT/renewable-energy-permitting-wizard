namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using CustomControls;
    using HawaiiDBEDT.Domain;
    using utils;

    public partial class PermitList : System.Web.UI.Page
	{
		protected string ConfirmationText;
        protected FullGridPager fullGridPager;
	    
		protected void Page_Load(object sender, EventArgs e)
		{
			ConfirmationText = "";

			if (!IsPostBack)
			{
				HandlePageAction();
				DisplayPermitTypeList();
				DisplayPermitSetList();
                this.Rebind();
                SortingInfo = new Sorting { Column = "Name", Direction = SortDirection.Ascending };
            }
            else
            {
                fullGridPager = new FullGridPager(gridRecords, 20, "Page", "of");
                fullGridPager.CreateCustomPager(gridRecords.TopPagerRow);
            }
		}

		private void HandlePageAction()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				if (Request.QueryString["action"] == "delete")
				{
					Data.Permit.DeletePermit(int.Parse(Request.QueryString["id"]));
					ConfirmationText = "Permit has been deleted.";
				}
			}
		}

		private void DisplayPermitTypeList()
		{
			rcbPermitType.DataSource = HawaiiDBEDT.Data.DataAccess.PermitTypeList;
			rcbPermitType.DataBind();

			rcbPermitType.Items.Insert(0, new ListItem("< permit type >", ""));
		}

		private void DisplayPermitSetList()
		{
			rcbPermitSet.DataSource = Data.PermitSet.GetPermitSets().OrderBy(i => i.Name);
			rcbPermitSet.DataBind();

			rcbPermitSet.Items.Insert(0, new ListItem("< permit set >", ""));
		}

		protected void Rebind()
		{
            gridRecords.DataSource = GetData();
            gridRecords.DataBind();
		}

        private List<Permit> GetData()
        {
            List<Permit> permits;

            if (rcbPermitType.SelectedIndex == 0 && rcbPermitSet.SelectedIndex == 0)
            {
                permits = HawaiiDBEDT.Data.DataAccess.PermitList;
            }
            else
            {
                int permitTypeId, permitSetId;
                if (!int.TryParse(rcbPermitType.SelectedValue, out permitTypeId))
                {
                    permitTypeId = 0;
                }

                if (!int.TryParse(rcbPermitSet.SelectedValue, out permitSetId))
                {
                    permitSetId = 0;
                }

                permits = Data.Permit.GetPermits(permitTypeId, permitSetId);
            }

            return permits;
        }

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			this.Rebind();
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			rcbPermitSet.SelectedIndex = 0;
			rcbPermitType.SelectedIndex = 0;
			this.Rebind();
		}

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

	    public Sorting SortingInfo
	    {
	        get
	        {
	            if (ViewState["Sorting"] != null)
	            {
                    return (Sorting) ViewState["Sorting"];
	            }
	            return null;
	        }
            set
            {
                ViewState["Sorting"] = value;
            }
	    }

        protected void HandleSorting(object sender, GridViewSortEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Permit> permits1 = GetData();
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                permits1 = permits1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = permits1.ToList();
                gridRecords.DataBind();
                SortingInfo = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }

        protected void HandlePaging(object sender, GridViewPageEventArgs e)
        {
            this.gridRecords.PageIndex = e.NewPageIndex;
            this.Rebind();
        }

        protected void HandleDataBound(object sender, EventArgs e)
        {
            if (fullGridPager == null)
            {
                fullGridPager = new FullGridPager(gridRecords, 20, "Page", "of");
            }

            fullGridPager.CreateCustomPager(gridRecords.TopPagerRow);
        }
	}
}