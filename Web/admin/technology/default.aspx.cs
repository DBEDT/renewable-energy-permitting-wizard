namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using HawaiiDBEDT.Domain;
    using utils;

    public partial class TechnologyList : System.Web.UI.Page
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
            var technologies = Data.DataAccess.TechnologyList;
            if (technologies.Count < gridRecords.PageSize)
            {
                gridRecords.AllowPaging = false;
            }
            gridRecords.DataSource = technologies;
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

        protected void gridRecords_sort(object source, DataGridSortCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Technology> responses1 = Data.DataAccess.TechnologyList;
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                responses1 = responses1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = responses1.ToList();
                gridRecords.DataBind();
                SortingInfo = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }

        protected void gridRecords_pageChange(object source, DataGridPageChangedEventArgs e)
        {
            this.gridRecords.CurrentPageIndex = e.NewPageIndex;
            this.Rebind();
        }
	}
}