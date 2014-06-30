using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;

using HawaiiDBEDT.Domain.Enumerations;

namespace HawaiiDBEDT.Web.Admin
{
	public partial class QuestionSetDetail : System.Web.UI.Page
	{
		protected string ConfirmationText;
		protected string ErrorText;
		private int technologyID;
		private int questionSetID;
		private Domain.QuestionSet questionSet;
		private int QuestionSetNameLengthLimit = 50;

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfirmationText = "";
			GetQuestionSet();
			if (!IsPostBack)
			{
				DisplayTechnologies();
				DisplayLocations();
				DisplayQuestionSet();
				DisplayQuestions();
			}
		}

		private void DisplayTechnologies()
		{
			rcbTechnology.DataSource = Data.DataAccess.TechnologyList;
			rcbTechnology.DataBind();
		}

		private void DisplayLocations()
		{
			cbLocation.DataSource = Data.DataAccess.LocationList;
			cbLocation.DataBind();
		}

		private void GetQuestionSet()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				questionSetID = int.Parse(Request.QueryString["id"]); 
				questionSet = Data.QuestionSet.GetQuestionSet(questionSetID);
				technologyID = questionSet.TechnologyID;

				if (!IsPostBack)
				{
					if (!String.IsNullOrEmpty(Request.QueryString["duplicate"]))
					{
						ConfirmationText = "The question set has been successfully duplicated. Please update this new question set as necessary.";
					}
				}
			}
			else
			{
				questionSetID = 0;
				questionSet = new QuestionSet(); 
			}
		}

		private void DisplayQuestions()
		{
			// Exclude federal and pre-eval
			var questions = Data.Question.GetQuestionsExceptTypes((int)QuestionTypeEnum.Federal, (int)QuestionTypeEnum.Capacity); 

			if (questionSetID != 0 && !IsPostBack)
			{
			    this.tlbQuestions.DestinationListItems = questionSet.Questions;
			}

			// Need to delete questions used in other question sets for the same technology;

            this.tlbQuestions.SourceListItems = questions;
		}

		private void DisplayQuestionSet()
		{
			if (questionSetID != 0)
			{
				txtName.Text = questionSet.Name;
				txtDescription.Text = questionSet.Description;

				rcbTechnology.Items.FindByValue(questionSet.TechnologyID.ToString()).Selected = true;

				foreach (QuestionSetLocation location in questionSet.Locations)
				{
					cbLocation.Items.FindByValue(location.LocationID.ToString()).Selected = true;
				}
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			if (UpdateQuestionSet())
			{
				Response.Redirect("default.aspx?id=" + technologyID.ToString() + "&action=edit&result=success", true);
			}
		}

		private bool UpdateQuestionSet()
		{
			Page.Validate();

			if (Page.IsValid)
			{
				questionSet.Name = txtName.Text;
				questionSet.Description = txtDescription.Text;
				questionSet.TechnologyID = int.Parse(rcbTechnology.SelectedValue);

				UpdateLocations();

				if (questionSet.Locations.Count == 0)
				{
					ErrorText = "Please select at least one location.";
					return false;
				}

				UpdateQuestions();

				if (questionSetID != 0)
				{
					Data.QuestionSet.UpdateQuestionSet(questionSet);
				}
				else
				{
					Data.QuestionSet.AddQuestionSet(questionSet);
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		private void UpdateLocations()
		{
			List<QuestionSetLocation> locations = new List<QuestionSetLocation>();
			foreach (ListItem item in cbLocation.Items)
			{
				if (item.Selected)
				{
					locations.Add(new QuestionSetLocation(int.Parse(item.Value)));
				}
			}

			// check for existing locations which needs to be deleted
			foreach (QuestionSetLocation location in questionSet.Locations)
			{
				if (locations.Find(i => i.LocationID == location.LocationID) == null)
				{
					questionSet.LocationsToDelete.Add(new QuestionSetLocation(questionSet.ID, location.LocationID));
				}
			}

			questionSet.Locations = locations;
		}

		private void UpdateQuestions()
		{
			var questions = new List<Question>();
			foreach (ListItem item in this.tlbQuestions.DestinationListValues)
			{
				questions.Add(new Question(int.Parse(item.Value), item.Text));
			}
			questionSet.Questions = questions;
		}


		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("default.aspx", true);
		}

		protected void btnSubmitAnother_Click(object sender, EventArgs e)
		{
			if (UpdateQuestionSet())
			{
				Response.Redirect("edit.aspx&confirm=true", true);
			}
		}

		protected void btnSubmitDuplicate_Click(object sender, EventArgs e)
		{
			if (UpdateQuestionSet())
			{
				// Create a new question set
				questionSet.ID = 0;
				questionSet.Name = "Copy of " + questionSet.Name;
				if (questionSet.Name.Length > QuestionSetNameLengthLimit)
				{
					questionSet.Name = questionSet.Name.Substring(0, QuestionSetNameLengthLimit);
				}

				int newQuestionSetID = Data.QuestionSet.AddQuestionSet(questionSet);
				Response.Redirect("edit.aspx?id=" + newQuestionSetID.ToString() + "&duplicate=true", true);
			}
		}
	}
}