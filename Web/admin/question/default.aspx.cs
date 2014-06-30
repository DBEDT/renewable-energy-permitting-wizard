namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using CustomControls;
    using HawaiiDBEDT.Data;
    using utils;

    public partial class QuestionList : System.Web.UI.Page
	{
		protected string ConfirmationText;
        protected FullGridPager fullGridPager;
		protected void Page_Load(object sender, EventArgs e)
		{
			ConfirmationText = "";

		    if (!IsPostBack)
		    {
		        HandlePageAction();
		        DisplayQuestionTypeList();
		        this.Rebind();
		        SortingInfo = new Sorting {Column = "Name", Direction = SortDirection.Ascending};
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
					if (Data.Question.DeleteQuestion(int.Parse(Request.QueryString["id"])))
					{
						ConfirmationText = "Question has been deleted.";
					}
					else {
						ConfirmationText = "Question cannot be deleted as it's used as a sub-question in one or more responses.";
					}
				}
			}
		}

		private void DisplayQuestionTypeList()
		{
			rcbQuestionType.DataSource = DataAccess.QuestionTypeList;
			rcbQuestionType.DataBind();

			rcbQuestionType.Items.Insert(0, new ListItem("< question type >", ""));
		}

		protected void Rebind()
		{
		    var questions = GetData();
			QuestionIDList = questions.Select(i => i.ID).ToList();
			gridRecords.DataSource = questions;
            gridRecords.DataBind();
		}

        private List<Domain.Question> GetData()
        {
            List<Domain.Question> questions;

            if (rcbQuestionType.SelectedIndex != 0)
            {
                questions = Question.GetQuestions(int.Parse(rcbQuestionType.SelectedValue)).OrderBy(i => i.Name).ToList();
            }
            else
            {
                questions = Question.GetQuestions().OrderBy(i => i.Name).ToList();
            }
            return questions;
        } 

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			this.Rebind();
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			rcbQuestionType.SelectedIndex = 0;
			this.Rebind();
		}

		// Used to let admins traverse through records
		public List<int> QuestionIDList
		{
			get
			{
				if (Session["QuestionIDList"] != null)
				{
					return (List<int>)Session["QuestionIDList"];
				}
				else
				{
					return new List<int>();
				}
			}
			set
			{
				Session["QuestionIDList"] = value;
			}
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

        protected void HandlePageChange(object sender, GridViewPageEventArgs e)
        {
            this.gridRecords.PageIndex = e.NewPageIndex;
            this.Rebind();
        }

        protected void HandleSorting(object sender, GridViewSortEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Domain.Question> questions1 = GetData();
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                questions1 = questions1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = questions1.ToList();
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