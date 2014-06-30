namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI.MobileControls;
    using System.Web.UI.WebControls;
    using CustomControls;
    using Domain;
    using HawaiiDBEDT.Data;
    using utils;

    public partial class UserList : System.Web.UI.Page
	{
        protected FullGridPager fullGridPager;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				PopulateFilters();
                this.Rebind();
			}
            else
            {
                fullGridPager = new FullGridPager(gridRecords, 20, "Page", "of");
                fullGridPager.CreateCustomPager(gridRecords.TopPagerRow);
            }
		}

		private void PopulateFilters()
		{
			rcbTechnology.DataSource = DataAccess.TechnologyList;
			rcbTechnology.DataBind();

			rcbTechnology.Items.Insert(0, new ListItem("< select technology >", ""));

			rcbLocation.DataSource = DataAccess.LocationList;
			rcbLocation.DataBind();

			rcbLocation.Items.Insert(0, new ListItem("< select location >", ""));
		}

		protected void Rebind()
		{
		    var users = GetData();
            if (users.Count < gridRecords.PageSize)
            {
                gridRecords.AllowPaging = false;
            }

		    gridRecords.DataSource = users;
            gridRecords.DataBind();
		    this.pnlConfirm.Visible = false;
		}

        private List<Domain.User> GetData()
        {
            var users = Data.User.GetUsers();

            if (rcbTechnology.SelectedIndex > 0)
            {
                users.RemoveAll(i => i.TechnologyID != int.Parse(rcbTechnology.SelectedValue));
            }
            if (rcbLocation.SelectedIndex > 0)
            {
                users.RemoveAll(i => i.LocationID != int.Parse(rcbLocation.SelectedValue));
            }

            return users;
        } 

		protected void btnFilter_Click(object sender, EventArgs e)
		{
			this.Rebind();
		}

		protected void lbReset_Click(object sender, EventArgs e)
		{
			rcbTechnology.SelectedIndex = 0;
			rcbLocation.SelectedIndex = 0;
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

        protected void HandleItemCommand(object source, DataGridCommandEventArgs e)
        {
            var linkButton = e.CommandSource as LinkButton;
            if (linkButton == null)
            {
                return;
            }

            if (linkButton.CommandName == "ResetPassword")
            {
                TableCell idCell = e.Item.Cells[0];
                TableCell emailCell = e.Item.Cells[2];
                int id = int.Parse(idCell.Text);
                string email = emailCell.Text;
                var userAccountManager = new UserAccountManager();
                userAccountManager.ResetPasswordFor(id, email);
                this.pnlConfirm.Visible = true;
            }
        }

        protected void HandleSorting(object sender, GridViewSortEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Domain.User> users = GetData();
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                users = users.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = users.ToList();
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