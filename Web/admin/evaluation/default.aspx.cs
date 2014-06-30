namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using CustomControls;
    using HawaiiDBEDT.Domain;
    using utils;

    public partial class EvaluationList : System.Web.UI.Page
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
			rcbLocation.DataSource = Data.DataAccess.LocationList;
			rcbLocation.DataBind();

			rcbLocation.Items.Insert(0, new ListItem("< select location >", ""));

			rcbTechnology.DataSource = Data.DataAccess.TechnologyList;
			rcbTechnology.DataBind();

			rcbTechnology.Items.Insert(0, new ListItem("< select technology >", ""));
		}

		protected void Rebind()
		{
		    var evaluations = GetData();
            if (evaluations.Count < gridRecords.PageSize)
            {
                gridRecords.AllowPaging = false;
            }

		    gridRecords.DataSource = evaluations;
            gridRecords.DataBind();
		}

        private List<Evaluation> GetData()
        {
            var evaluations = Data.Evaluation.GetEvaluationsAdmin();
            if (rcbTechnology.SelectedIndex > 0)
            {
                evaluations.RemoveAll(i => i.TechnologyID != int.Parse(rcbTechnology.SelectedValue));
            }
            if (rcbLocation.SelectedIndex > 0)
            {
                evaluations.RemoveAll(i => i.LocationID != int.Parse(rcbLocation.SelectedValue));
            }

            return evaluations;
        } 

		protected void btnFilter_Click(object sender, EventArgs e)
		{
			this.Rebind();
		}

		protected void lbReset_Click(object sender, EventArgs e)
		{
			rcbLocation.SelectedIndex = 0;
			rcbTechnology.SelectedIndex = 0;
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
                IEnumerable<Evaluation> evaluations = GetData();
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                evaluations = evaluations.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = evaluations.ToList();
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