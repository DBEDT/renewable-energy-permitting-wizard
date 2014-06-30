namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Domain;
    using utils;

    public partial class LocationList : System.Web.UI.Page
	{
		protected string ConfirmationText;

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfirmationText = "";

			if (!IsPostBack)
			{
                this.Rebind();
                SortingInfo = new Sorting { Column = "Name", Direction = SortDirection.Ascending };
			}
		}

        protected void Rebind()
        {
            var locations = Data.DataAccess.LocationList;
            if (locations.Count < gridRecords.PageSize)
            {
                gridRecords.AllowPaging = false;
            }
            gridRecords.DataSource = locations;
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

        protected void HandleSorting(object sender, GridViewSortEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Location> locations1 = Data.DataAccess.LocationList;
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                locations1 = locations1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = locations1.ToList();
                gridRecords.DataBind();
                SortingInfo = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }

        protected void HandlePaging(object sender, GridViewPageEventArgs e)
        {
            this.gridRecords.PageIndex = e.NewPageIndex;
            this.Rebind();
        }
	}
}