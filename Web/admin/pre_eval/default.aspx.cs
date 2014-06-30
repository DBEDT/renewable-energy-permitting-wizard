namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using HawaiiDBEDT.Domain;
    using utils;

    public partial class PreEvaluationQuestionList : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
                this.Rebind();
                SortingInfo = new Sorting { Column = "LookupClassName", Direction = SortDirection.Ascending };
			}
		}

        private void Rebind()
        {
            var questions = Data.DataAccess.PreEvaluationQuestionList;
            if (questions.Count < gridRecords.PageSize)
            {
                gridRecords.AllowPaging = false;
            }
            gridRecords.DataSource = questions;
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
                IEnumerable<PreEvaluationQuestion> preEvaluationQuestions1 = Data.DataAccess.PreEvaluationQuestionList;
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                preEvaluationQuestions1 = preEvaluationQuestions1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = preEvaluationQuestions1.ToList();
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