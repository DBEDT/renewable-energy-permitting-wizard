namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using CustomControls;
    using Domain;
    using HawaiiDBEDT.Data;
    using utils;

    public partial class PermitSetList : System.Web.UI.Page
	{
		protected string ConfirmationText;
        protected FullGridPager fullGridPager;

		protected void Page_Load(object sender, EventArgs e)
		{ 
			ConfirmationText = "";

			if (!IsPostBack)
			{
				HandlePageAction();
				DisplayPermitList();
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
					Data.PermitSet.DeletePermitSet(int.Parse(Request.QueryString["id"]));
					ConfirmationText = "Permit set has been deleted.";
				}
			}
		}

		private void DisplayPermitList()
		{ 
			rcbPermit.DataSource = Data.Permit.GetPermits().OrderBy(i => i.Name);
			rcbPermit.DataBind();

			rcbPermit.Items.Insert(0, new ListItem("< select >", ""));
		}

		protected void Rebind()
		{
		    gridRecords.DataSource = GetData();
            gridRecords.DataBind();
		}

        private List<Domain.PermitSet> GetData()
        {
            List<Domain.PermitSet> permitSets;

            if (rcbPermit.SelectedIndex > 0)
            {
                permitSets = PermitSetPermit.GetPermitSets(rcbPermit.SelectedIndex);
            }
            else
            {
                permitSets = DataAccess.PermitSetList;
            }

            return permitSets;
        } 

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			this.Rebind();
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			rcbPermit.SelectedIndex = 0;
			this.Rebind();
		}

        public Sorting SortingInfo
        {
            get
            {
                if (ViewState["Sorting"] != null)
                {
                    return (Sorting)ViewState["Sorting"];
                }
                return null;
            }
            set
            {
                ViewState["Sorting"] = value;
            }
        }

        protected void HandlePaging(object sender, GridViewPageEventArgs e)
        {
            this.gridRecords.PageIndex = e.NewPageIndex;
            this.Rebind();
        }

        protected void HandleSorting(object sender, GridViewSortEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Domain.PermitSet> permitSets1 = GetData();
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                permitSets1 = permitSets1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = permitSets1.ToList();
                gridRecords.DataBind();
                SortingInfo = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
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