using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;
using Telerik.Web.UI;

namespace HawaiiDBEDT.Web.Admin
{
	public partial class QuestionSetDetail : System.Web.UI.Page
	{
		private int technologyID;
		private int questionSetID;
		private Domain.QuestionSet questionSet;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetQuestionSet();
			if (!IsPostBack)
			{
				DisplayQuestionSet();
				DisplayQuestions();
			}
		}

		private void GetQuestionSet()
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				questionSetID = int.Parse(Request.QueryString["id"]); 
				questionSet = Data.QuestionSet.GetQuestionSet(questionSetID);
				technologyID = questionSet.TechnologyID;
			}
			else
			{
				questionSetID = 0;
				questionSet = new QuestionSet(); 
				technologyID = int.Parse(Request.QueryString["tid"]);
			}
		}

		private void DisplayQuestions()
		{
			List<Question> questions = Data.QuestionSetQuestion.GetQuestionsNotUsedInTechnology(technologyID); 

			if (questionSetID != 0 && !IsPostBack)
			{
				foreach (Question question in questionSet.Questions)
				{
					try
					{
						lbQuestionsDestination.Items.Add(new RadListBoxItem(question.Name, question.ID.ToString()));
						//questions.RemoveAt(questions.FindIndex(i => i.ID == question.ID));
					}
					catch { }
				}
			}

			// Need to delete questions used in other question sets for the same technology;

			lbQuestions.DataSource = questions;
			lbQuestions.DataBind();
		}

		private void DisplayQuestionSet()
		{
			if (questionSetID != 0)
			{
				lblTechnology.Text = questionSet.Technology.Name;
				txtName.Text = questionSet.Name;
				txtDescription.Text = questionSet.Description;
			}
			else
			{
				lblTechnology.Text = HawaiiDBEDT.Data.DataAccess.TechnologyList.Find(i => i.ID == technologyID).Name;
			}
		}

		protected void btnSumit_Click(object sender, EventArgs e)
		{
			if (UpdateQuestionSet())
			{
				Response.Redirect("edit.aspx?id=" + technologyID.ToString() + "&action=edit&result=success", true);
			}
		}

		private bool UpdateQuestionSet()
		{
			Page.Validate();

			if (Page.IsValid)
			{
				questionSet.Name = txtName.Text;
				questionSet.Description = txtDescription.Text;
				questionSet.TechnologyID = technologyID;

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

		private void UpdateQuestions()
		{
			List<Question> questions = new List<Question>();
			foreach (RadListBoxItem item in lbQuestionsDestination.Items)
			{
				questions.Add(new Question(int.Parse(item.Value), item.Text));
			}
			questionSet.Questions = questions;
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("edit.aspx?id=" + technologyID, true);
		}

		protected void btnSubmitAnother_Click(object sender, EventArgs e)
		{
			if (UpdateQuestionSet())
			{
				Response.Redirect("question_set.aspx?tid=" + technologyID + "&confirm=true", true);
			}
		}
	}
}