namespace HawaiiDBEDT.Web.admin.user
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using HawaiiDBEDT.Domain;
    using HawaiiDBEDT.Domain.Enumerations;
    using utils;

    public partial class detail : System.Web.UI.Page
	{
		private int userID;
		private User user;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetUser();

			if (!IsPostBack)
			{
				DisplayUser();
				DisplayLoginHistory();
				DisplayEvaluationHistory();
                this.RebindEvaluationHistory();
                this.RebindLoginHistory();
			}
		}

		private void DisplayUser()
		{
			if (user.ID != 0)
			{
				lblName.Text = user.Name;
				lblTitle.Text = user.Title;
				lblOrganization.Text = user.Organization;
				lblTelephoneNumber.Text = user.TelephoneNumber;
				lblEmail.Text = user.EmailAddress;
				lblTechnology.Text = user.Technology.Name;
				lblLocation.Text = user.Location.Name;
                if (user.LastLoginDate.HasValue)
                {
                    lblLastLogin.Text = string.Format("{0:g}", DateTimeUtils.ToHawaiiTime(user.LastLoginDate.Value));
                }
			}
		}

		private void DisplayEvaluationHistory()
		{
		}

		private void DisplayLoginHistory()
		{
		}

		private void GetUser()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				userID = int.Parse(Request.QueryString["id"]);
				user = Data.User.GetUser(userID);
			}
		}

        protected void RebindEvaluationHistory()
        {
            var evaluations = Data.Evaluation.GetEvaluations(userID);
            var completeEvaluations = evaluations.Where(i => i.StatusID == (int) EvaluationStatusEnum.Complete).ToList();
            if (completeEvaluations.Count < gridEvaluation.PageSize)
            {
                gridEvaluation.AllowPaging = false;
            }

            gridEvaluation.DataSource = completeEvaluations;
            gridEvaluation.DataBind();

            lblNumTotal.Text = evaluations.Count.ToString();
            lblNumComplete.Text = completeEvaluations.Count().ToString();
            lblNumInProgress.Text = evaluations.Where(i => i.StatusID == (int)EvaluationStatusEnum.InProgress).Count().ToString();
        }

		protected void RebindLoginHistory()
		{
            gridLoginHistory.DataSource = Data.UserLoginHistory.GetLoginHistory(userID);
            gridLoginHistory.DataBind();
		}

        public Sorting SortingInfoEvaluationHistory
        {
            get
            {
                if (ViewState["SortingEvaluationHistory"] != null)
                {
                    return (Sorting)ViewState["SortingEvaluationHistory"];
                }
                return null;
            }
            set
            {
                ViewState["SortingEvaluationHistory"] = value;
            }
        }

        protected void gridEvaluation_sort(object source, DataGridSortCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                var evaluations = Data.Evaluation.GetEvaluations(userID);
                IEnumerable<Evaluation> users = evaluations.Where(i => i.StatusID == (int) EvaluationStatusEnum.Complete);
                var sortDirection = SortDirection.Ascending;
                if (SortingInfoEvaluationHistory != null && SortingInfoEvaluationHistory.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfoEvaluationHistory.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                users = users.OrderBy(e.SortExpression, sortDirection);
                gridEvaluation.DataSource = users.ToList();
                gridEvaluation.DataBind();
                SortingInfoEvaluationHistory = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }

        protected void gridEvaluation_pageChange(object source, DataGridPageChangedEventArgs e)
        {
            this.gridEvaluation.CurrentPageIndex = e.NewPageIndex;
            this.RebindEvaluationHistory();
        }

        public Sorting SortingInfoLoginHistory
        {
            get
            {
                if (ViewState["SortingLoginHistory"] != null)
                {
                    return (Sorting)ViewState["SortingLoginHistory"];
                }
                return null;
            }
            set
            {
                ViewState["SortingLoginHistory"] = value;
            }
        }

        protected void gridLoginHistory_sort(object source, DataGridSortCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SortExpression))
            {
                IEnumerable<UserLoginHistory> users = Data.UserLoginHistory.GetLoginHistory(userID);
                var sortDirection = SortDirection.Ascending;
                if (SortingInfoLoginHistory != null && SortingInfoLoginHistory.Column.Equals(e.SortExpression))
                {
                    sortDirection = SortDirection.Ascending.Equals(SortingInfoLoginHistory.Direction)
                                        ? SortDirection.Descending
                                        : SortDirection.Ascending;
                }

                users = users.OrderBy(e.SortExpression, sortDirection);
                gridLoginHistory.DataSource = users.ToList();
                gridLoginHistory.DataBind();
                SortingInfoLoginHistory = new Sorting { Column = e.SortExpression, Direction = sortDirection };
            }
        }

        protected void gridLoginHistory_pageChange(object source, DataGridPageChangedEventArgs e)
        {
            this.gridLoginHistory.CurrentPageIndex = e.NewPageIndex;
            this.RebindLoginHistory();
        }
	}
}