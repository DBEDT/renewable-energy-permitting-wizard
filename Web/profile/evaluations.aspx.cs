namespace HawaiiDBEDT.Web.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using HawaiiDBEDT.Domain;
    using HawaiiDBEDT.Domain.Enumerations;
    using utils;

    public partial class Evaluations : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			pnlDeleteConfirmation.Visible = false;

			if (!IsPostBack)
			{
                CheckForInProgressEvaluation();
                BindInProgress();
			    BindCompleted();
			}
		}

        private void BindCompleted()
        {
            gridEvaluationComplete.DataSource = Data.Evaluation.GetEvaluations(CurrentUserID, (int)EvaluationStatusEnum.Complete);
            gridEvaluationComplete.DataBind();
        }

        private void BindInProgress()
        {
            gridEvaluation.DataSource = Data.Evaluation.GetEvaluations(CurrentUserID, (int)EvaluationStatusEnum.InProgress);
            gridEvaluation.DataBind();
        }

		private void CheckForInProgressEvaluation()
		{
			if (CurrentEvaluationID != 0)
			{
				if (CurrentEvaluation.UserID == 0)
				{
					CurrentEvaluation.UserID = CurrentUserID;
					SaveCurrentEvaluationComplete();
				}
			}
		}

		protected void lbStartOver_OnClick(object sender, EventArgs e)
		{
			CurrentEvaluation = null;
			Response.Redirect("/evaluate/", true);
		}

        public Sorting SortingInfoEvalInProgress
        {
            get
            {
                if (ViewState["SortingEvalInProgress"] != null)
                {
                    return (Sorting)ViewState["SortingEvalInProgress"];
                }
                return null;
            }
            set
            {
                ViewState["SortingEvalInProgress"] = value;
            }
        }

        public Sorting SortingInfoEvalComplete
        {
            get
            {
                if (ViewState["SortingEvalComplete"] != null)
                {
                    return (Sorting)ViewState["SortingEvalComplete"];
                }
                return null;
            }
            set
            {
                ViewState["SortingEvalComplete"] = value;
            }
        }

        protected void gridEvaluation_sort(object source, DataGridSortCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Evaluation> permits1 = Data.Evaluation.GetEvaluations(CurrentUserID, (int)EvaluationStatusEnum.InProgress);
                var sortDirection = SortDirection.Ascending;
                if (SortingInfoEvalInProgress != null && SortingInfoEvalInProgress.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfoEvalInProgress.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                permits1 = permits1.OrderBy(e.SortExpression, sortDirection);
                gridEvaluation.DataSource = permits1.ToList();
                gridEvaluation.DataBind();
                SortingInfoEvalInProgress = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }

        protected void gridEvaluation_pageChange(object source, DataGridPageChangedEventArgs e)
        {
            this.gridEvaluation.CurrentPageIndex = e.NewPageIndex;
            this.BindInProgress();
        }

        protected void gridEvaluationComplete_sort(object source, DataGridSortCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<Evaluation> permits1 = Data.Evaluation.GetEvaluations(CurrentUserID, (int)EvaluationStatusEnum.Complete);
                var sortDirection = SortDirection.Ascending;
                if (SortingInfoEvalComplete != null && SortingInfoEvalComplete.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfoEvalComplete.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                permits1 = permits1.OrderBy(e.SortExpression, sortDirection);
                gridEvaluationComplete.DataSource = permits1.ToList();
                gridEvaluationComplete.DataBind();
                SortingInfoEvalComplete = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }

        protected void gridEvaluationComplete_pageChange(object source, DataGridPageChangedEventArgs e)
        {
            this.gridEvaluationComplete.CurrentPageIndex = e.NewPageIndex;
            this.BindCompleted();
        }

        protected void HandleItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int evaluationId = Convert.ToInt32(e.CommandArgument);
                var evaluation = Data.Evaluation.GetEvaluation(evaluationId);
                if (evaluation.UserID == CurrentUser.ID)
                {
                    Data.Evaluation.DeleteEvaluation(evaluationId);
                    BindInProgress();
                }
            }
        }
	}
}