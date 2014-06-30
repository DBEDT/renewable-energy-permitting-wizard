namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using CustomControls;
    using HawaiiDBEDT.Domain;
    using utils;

    public partial class DepartmentList : System.Web.UI.Page
	{
		protected string ConfirmationText;
        protected FullGridPager fullGridPager;

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfirmationText = "";

			if (!IsPostBack)
			{
				HandlePageAction();
                this.Rebind();
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
					if (Data.Department.DeleteDepartment(int.Parse(Request.QueryString["id"])))
					{
						ConfirmationText = "Department has been deleted.";
					}
					else
					{
						ConfirmationText = "Department cannot be deleted as it's referenced by a permit.";
					}
				}
			}
		}

        protected void Rebind()
        {
            var departments = Data.DataAccess.DepartmentList;
            if (departments.Count < gridRecords.PageSize)
            {
                gridRecords.AllowPaging = false;
            }
            gridRecords.DataSource = departments;
            gridRecords.DataBind();
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

        protected void HandleDataBound(object sender, EventArgs e)
        {
            if (fullGridPager == null)
            {
                fullGridPager = new FullGridPager(gridRecords, 20, "Page", "of");
            }

            fullGridPager.CreateCustomPager(gridRecords.TopPagerRow);
        }

        protected void HandlePaging(object sender, GridViewPageEventArgs e)
        {
            this.gridRecords.PageIndex = e.NewPageIndex;
            this.Rebind();
        }

        protected void HandleSort(object sender, GridViewSortEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Department> departments1 = Data.DataAccess.DepartmentList;
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                departments1 = departments1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = departments1.ToList();
                gridRecords.DataBind();
                SortingInfo = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }
	}
}