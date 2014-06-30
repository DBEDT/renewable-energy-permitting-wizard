namespace HawaiiDBEDT.Web.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using HawaiiDBEDT.Domain;

    public partial class QuestionSetOrder : System.Web.UI.Page
	{
		protected int technologyID = 0;
		protected int locationID = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				DisplayTechnologies();
				DisplayLocations();
                this.Rebind();
			}
		}

		private void HandlePageAction()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				if (Request.QueryString["action"] == "delete")
				{
					Data.QuestionSet.DeleteQuestionSet(int.Parse(Request.QueryString["id"]));
					lblConfirmation.Text = "Question set has been deleted.";
				}
			}
		}

		private void DisplayTechnologies()
		{
			rcbTechnology.DataSource = Data.DataAccess.TechnologyList;
			rcbTechnology.DataBind();
		}

		private void DisplayLocations()
		{
			rcbLocation.DataSource = Data.DataAccess.LocationList;
			rcbLocation.DataBind();
		}

		protected void Rebind()
		{
			technologyID = int.Parse(rcbTechnology.SelectedValue);
			locationID = int.Parse(rcbLocation.SelectedValue);

			List<QuestionSet> questionSets = Data.QuestionSet.GetQuestionSets(technologyID, locationID);

			gridRecords.DataSource = questionSets;
            gridRecords.DataBind();
		}

		private void ReorderQuestionSets(List<QuestionSet> questionSets)
		{
			int newOrder = 1;
			foreach (QuestionSet questionSet in questionSets)
			{
				questionSet.Order = newOrder++;
			}

			Data.QuestionSetLocation.UpdateQuestionSetOrder(questionSets, locationID);
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			this.Rebind();
            lblConfirmation.Text = string.Empty;
		}

        protected void gridRecords_pageChange(object source, DataGridPageChangedEventArgs e)
        {
            this.gridRecords.CurrentPageIndex = e.NewPageIndex;
            this.Rebind();
            lblConfirmation.Text = string.Empty;
        }

        protected void GridRecondsItemCommand(object source, DataGridCommandEventArgs e)
        {
            var rowNumber = e.Item.ItemIndex;
            technologyID = int.Parse(rcbTechnology.SelectedValue);
            locationID = int.Parse(rcbLocation.SelectedValue);
            List<Domain.QuestionSet> questionSets = Data.QuestionSet.GetQuestionSets(technologyID, locationID);
            var itemMoved = false;
            if ("MoveUp".Equals(e.CommandName) && rowNumber > 0)
            {
                var elementToMove = questionSets[rowNumber];
                questionSets[rowNumber] = questionSets[rowNumber - 1];
                questionSets[rowNumber - 1] = elementToMove;
                itemMoved = true;
            }
            else if ("MoveDown".Equals(e.CommandName) && rowNumber < questionSets.Count - 1)
            {
                var elementToMove = questionSets[rowNumber];
                questionSets[rowNumber] = questionSets[rowNumber + 1];
                questionSets[rowNumber + 1] = elementToMove;
                itemMoved = true;
            }

            if (itemMoved)
            {
                ReorderQuestionSets(questionSets);
                this.Rebind();
                lblConfirmation.Text = "Question Set reordered successfully.";
            }
            else
            {
                lblConfirmation.Text = string.Empty;
            }
        }
	}
}