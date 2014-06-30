namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using HawaiiDBEDT.Domain;

    using Westwind.Web.Utilities;

    using utils;

    public partial class PreEvaluationQuestionDetail : System.Web.UI.Page
	{
		protected int questionID;
		private PreEvaluationQuestion question;
		private int MAX_TOOL_TIP_LENGTH = 5000;
		protected string ConfirmationText;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetQuestion();
			if (!IsPostBack)
			{
				HandlePageAction();
				DisplayQuestion();
                this.Rebind();
			}
		}
		private void HandlePageAction()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["rid"]))
			{
				if (Request.QueryString["action"] == "delete")
				{
					Data.PreEvaluationResponse.DeletePreEvaluationResponse(int.Parse(Request.QueryString["rid"]));
					ConfirmationText = "Response has been deleted.";
				}
			}
		}

		private void GetQuestion()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				questionID = int.Parse(Request.QueryString["id"]);
				question = Data.PreEvaluationQuestion.GetPreEvaluationQuestion(questionID);
			}
			else
			{
				Response.Redirect("default.aspx", true);
			}
		}

		private void DisplayQuestion()
		{
			lblLookupClassName.Text = question.LookupClassName;
			txtName.Text = HttpUtility.HtmlEncode(question.Name);
            reDescription.Text = HtmlEncoder.BetterHtmlDecode(question.Description);

			pnlResponses.Visible = question.AllowCustomResponse;
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("default.aspx", true);
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			if (UpdatePreEvaluationQuestion())
			{
				Response.Redirect("default.aspx?action=edit&result=success", true);
			}
		}

		protected void Rebind()
		{
		    var responses = GetData();
            if (responses.Count < gridRecords.PageSize)
            {
                gridRecords.AllowPaging = false;
            }
			gridRecords.DataSource = responses;
            gridRecords.DataBind();
		}

        private List<PreEvaluationResponse> GetData()
        {
            List<PreEvaluationResponse> responses;

            if (questionID != 0)
            {
                responses = Data.PreEvaluationResponse.GetPreEvaluationResponses(questionID);
            }
            else
            {
                responses = new List<Domain.PreEvaluationResponse>();
            }

            return responses;
        } 
		#region Update

		private bool UpdatePreEvaluationQuestion()
		{
			int toolTipLength = reDescription.Text.Length;

			lblError.Text = "";
			Page.Validate();
			if (toolTipLength > MAX_TOOL_TIP_LENGTH)
			{
				lblError.Text = "There are " + toolTipLength.ToString() + " characters. Please limit to " + MAX_TOOL_TIP_LENGTH + " characters.";
				return false;
			}

			if (Page.IsValid)
			{
				question.Name = txtName.Text;
				question.Description = reDescription.Text.Trim();
				Data.PreEvaluationQuestion.UpdatePreEvaluationQuestion(question);
				return true;
			}
			return false;
		}

		#endregion 

        
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
                IEnumerable<PreEvaluationResponse> preEvaluationResponses1 = GetData();
                var sortDirection = SortDirection.Ascending;
                if (SortingInfo != null && SortingInfo.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfo.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                preEvaluationResponses1 = preEvaluationResponses1.OrderBy(e.SortExpression, sortDirection);
                gridRecords.DataSource = preEvaluationResponses1.ToList();
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