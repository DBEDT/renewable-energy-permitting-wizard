namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using CustomControls;
    using HawaiiDBEDT.Domain;
    using utils;

    public partial class QuestionSetList : System.Web.UI.Page
	{
		protected string ConfirmationText;
		protected int technologyID = 0;
		protected int locationID = 0;
        protected FullGridPager fullGridPager;

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfirmationText = "";

			if (!IsPostBack)
			{
				HandlePageAction();
				DisplayQuestionList();
				DisplayTechnologies();
				DisplayLocations();
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
					Data.QuestionSet.DeleteQuestionSet(int.Parse(Request.QueryString["id"]));
					ConfirmationText = "Question set has been deleted.";
				}
			}
		}

		private void DisplayTechnologies()
		{
			rcbTechnology.DataSource = Data.DataAccess.TechnologyList;
			rcbTechnology.DataBind();

			rcbTechnology.Items.Insert(0, new ListItem("< select >", ""));
		}

		private void DisplayLocations()
		{
			rcbLocation.DataSource = Data.DataAccess.LocationList;
			rcbLocation.DataBind();

			rcbLocation.Items.Insert(0, new ListItem("< select >", ""));
		}


		private void DisplayQuestionList()
		{
			rcbQuestion.DataSource = Data.Question.GetQuestions();
			rcbQuestion.DataBind();

			rcbQuestion.Items.Insert(0, new ListItem("< select >", ""));
		}

		protected void Rebind()
		{
		    var questionSets = GetData();
			gridRecords.DataSource = questionSets;
            gridRecords.DataBind();
		}

        private List<QuestionSet> GetData()
        {
            List<QuestionSet> questionSets;

            if (rcbQuestion.SelectedIndex > 0)
            {
                questionSets = Data.QuestionSetQuestion.GetQuestionSets(int.Parse(rcbQuestion.SelectedValue));
            }
            else
            {
                questionSets = Data.QuestionSet.GetQuestionSets();
            }

            if (rcbTechnology.SelectedIndex != 0)
            {
                questionSets.RemoveAll(i => i.TechnologyID != int.Parse(rcbTechnology.SelectedValue));
            }

            if (rcbLocation.SelectedIndex != 0)
            {
                questionSets.RemoveAll(i => !i.LocationIDs.Contains(int.Parse(rcbLocation.SelectedValue)));
            }

            return questionSets;
        } 

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			this.Rebind();
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			rcbQuestion.SelectedIndex = 0;
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

        protected void HandleSorting(object sender, GridViewSortEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<QuestionSet> questionSets1 = GetData();
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                questionSets1 = questionSets1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = questionSets1.ToList();
                gridRecords.DataBind();
                SortingInfo = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }
	}
}