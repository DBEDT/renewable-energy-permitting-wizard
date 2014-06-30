namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;
    using HawaiiDBEDT.Domain;
    using utils;

    public partial class QuestionDetail : System.Web.UI.Page
	{
		protected int questionID;
		private Question question;
		protected string ConfirmationText;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetQuestion();
			ConfirmationText = "";

			if (!IsPostBack)
			{
				HandlePageAction();
				DisplayQuestionTypeList();
				DisplayQuestion();
                this.Rebind();
                SortingInfo = new Sorting { Column = "Name", Direction = SortDirection.Ascending };
			}
		}

		private void HandlePageAction()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["rid"]))
			{
				if (Request.QueryString["action"] == "delete")
				{
					Data.Response.DeleteResponse(int.Parse(Request.QueryString["rid"]));
					ConfirmationText = "Response has been deleted.";
				}
			}
		}

		private void DisplayQuestionTypeList()
		{
			rcbQuestionType.DataSource = HawaiiDBEDT.Data.DataAccess.QuestionTypeList;
			rcbQuestionType.DataBind();
		}

		private void GetQuestion()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				questionID = int.Parse(Request.QueryString["id"]);
				question = Data.Question.GetQuestion(questionID);
				int previousID = GetPreviousQuestionID(questionID);
				if (previousID != -1)
				{
					hlPrevious.NavigateUrl = String.Format("edit.aspx?id={0}", GetPreviousQuestionID(questionID));
				}
				else
				{
					hlPrevious.Visible = false;
				}

				int nextID = GetNextQuestionID(questionID);
				if (nextID != -1)
				{
					hlNext.NavigateUrl = String.Format("edit.aspx?id={0}", GetNextQuestionID(questionID));
				}
				else
				{
					hlNext.Visible = false;
					divSubmitNext.Visible = false;
				}
			}
			else
			{
				questionID = 0;
				question = new Question();
				divSubmit.Visible = false;
				divSubmitNext.Visible = false;
				hlPrevious.Visible = false;
				hlNext.Visible = false;
			}
		}

		private void DisplayQuestion()
		{
			if (questionID != 0)
			{
				txtName.Text = question.Name;
				reDescription.Text = HtmlEncoder.BetterHtmlDecode(question.Description);
				rcbQuestionType.SelectedValue = question.QuestionType.ID.ToString();
				pnlResponses.Visible = true;
			}
			else
			{
				btnSubmit.Text = "Submit and Add Responses";
				divSubmitAnother.Visible = false;
			}
		}

		protected void Rebind()
		{
		    var responses = GetData();
			gridRecords.DataSource = responses;
            gridRecords.DataBind();
		}

        private List<Response> GetData()
        {
            List<Response> responses;

            if (questionID != 0)
            {
                responses = Data.Response.GetResponses(questionID);
            }
            else
            {
                responses = new List<Domain.Response>();
            }

            if (responses.Count < gridRecords.PageSize)
            {
                gridRecords.AllowPaging = false;
            }

            return responses;
        } 

		#region Update

		private bool UpdateQuestion()
		{
			Page.Validate();
			if (Page.IsValid)
			{
				question.Name = txtName.Text;
				question.Description = reDescription.Text;
				question.QuestionTypeID = int.Parse(rcbQuestionType.SelectedValue);

				if (questionID != 0)
				{
					Data.Question.UpdateQuestion(question);
				}
				else
				{
					questionID = Data.Question.AddQuestion(question);
				}

				return true;
			}
			return false;
		}
		#endregion

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("default.aspx", true);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			// Redirect to current page
			if (UpdateQuestion())
			{
				Response.Redirect("edit.aspx?id=" + questionID.ToString(), true);
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			if (UpdateQuestion())
			{
				Response.Redirect("default.aspx?action=edit&result=success", true);
			}
		}

		protected void btnSubmitAnother_Click(object sender, EventArgs e)
		{
			if (UpdateQuestion())
			{
				Response.Redirect("edit.aspx?confirm=true", true);
			}
		}

		protected void btnSubmitNext_Click(object sender, EventArgs e)
		{
			if (UpdateQuestion())
			{
				int nextQuestionID = GetNextQuestionID(questionID);
				if (nextQuestionID != -1)
				{
					Response.Redirect("edit.aspx?id=" + nextQuestionID.ToString(), true);
				}
				else
				{
					Response.Redirect("edit.aspx", true);
				}
			}
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

		public int GetNextQuestionID(int questionID)
		{
			if (QuestionIDList.Count == 0)
			{
				return -1;
			}

			// current submission is last element
			if (questionID == QuestionIDList[QuestionIDList.Count - 1])
			{
				return -1;
			}

			for (int i = 0; i < QuestionIDList.Count - 1; i++)
			{
				if (QuestionIDList[i] == questionID)
				{
					return QuestionIDList[i + 1];
				}
			}

			// not found
			return -1;
		}

		public int GetPreviousQuestionID(int questionID)
		{
			if (QuestionIDList.Count == 0)
			{
				return -1;
			}
			// current submission is first submission
			if (questionID == QuestionIDList[0])
			{
				return -1;
			}

			for (int i = 1; i < QuestionIDList.Count; i++)
			{
				if (QuestionIDList[i] == questionID)
				{
					return QuestionIDList[i - 1];
				}
			}

			// not found
			return -1;
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
                IEnumerable<Response> responses1 = GetData();
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