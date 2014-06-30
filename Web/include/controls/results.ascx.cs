using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Data;
using HawaiiDBEDT.Domain;
using System.Data;
using HawaiiDBEDT.Domain.Enumerations;
using EvaluationResponse = HawaiiDBEDT.Domain.EvaluationResponse;
using Permit = HawaiiDBEDT.Domain.Permit;
using PermitDependency = HawaiiDBEDT.Domain.PermitDependency;
using PermitSet = HawaiiDBEDT.Domain.PermitSet;
using PreEvaluationQuestion = HawaiiDBEDT.Domain.PreEvaluationQuestion;
using QuestionSet = HawaiiDBEDT.Domain.QuestionSet;

namespace HawaiiDBEDT.Web.Controls
{
    using System.Drawing;
    using System.Web.UI.DataVisualization.Charting;
    using log4net;
    using utils;
    using Westwind.Web.Utilities;
    using HawaiiDBEDT.Data.ViewModels;

    public partial class ResultsControl : BaseControl
	{
		protected string technologyName;
		protected int technologyID;
		protected string locationName;

		protected List<PermitSet> technologyPermitSet;
		protected List<PermitSet> locationPermitSet;
		protected List<PermitSet> capacityPermitSet;
		protected List<PermitSet> federalPermitSet;

		protected List<Permit> distinctPermits;
		protected List<Permit> ganntPermits;
        ILog log = LogManager.GetLogger(typeof(ResultsControl));

		public void LoadPage()
		{
			pnlPermitNoResults.Visible = false;
			pnlPermitResults.Visible = true;

			try
			{
				RefreshProgressBar();
				DisplayTechnology();
				DisplayLocation();

				DisplayPreEvalAnswers();
			    DisplayQuestionSets();
				DisplayPermits();
                this.chartImage.ImageUrl = "/permitPlan/getImage.aspx";
				DisplaySavePanel();
			}
			catch (Exception ex)
			{
                log.Error(ex);
				LoadPreEvaluationPage();
			}
		}

		private void DisplaySavePanel()
		{
			if (IsLoggedIn)
			{
				pnlSave.Visible = true;
				txtName.Text = CurrentEvaluation.Name;
				txtDescription.Text = CurrentEvaluation.Description;
			}
			else
			{
				pnlLoginRegister.Visible = true;
			}
		}

		private void DisplayPreEvalAnswers()
		{
			List<PreEvaluationQuestion> questions = Data.PreEvaluationQuestion.GetPreEvaluationQuestions();
			PreEvaluationQuestion technologyQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Technology.ToString());
			PreEvaluationQuestion capacityQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Capacity.ToString());
			PreEvaluationQuestion locationQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Location.ToString());
			PreEvaluationQuestion federalQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Federal.ToString());

			lblTechnologyQuestion.Text = technologyQuestion.Name;
			lblCapacityQuestion.Text = capacityQuestion.Name;
			lblLocationQuestion.Text = locationQuestion.Name;
			lblFederalQuestion.Text = federalQuestion.Name;

			lblTechnologyAnswer.Text = technologyQuestion.LookupOptions.Find(i => i.ID == CurrentEvaluation.Technology.ID).Name;
			lblCapacityAnswer.Text = capacityQuestion.Responses.Find(i => i.ID == CurrentEvaluation.CapacityID).Name;
			lblLocationAnswer.Text = locationQuestion.LookupOptions.Find(i => i.ID == CurrentEvaluation.Location.ID).Name;
			lblFederalAnswer.Text = federalQuestion.Responses.Find(i => i.ID == CurrentEvaluation.FederalID).Name;

			technologyPermitSet = Data.TechnologyPermitSet.GetPermitSetsInTechnology(CurrentEvaluation.Technology.ID);
			rptTechnologyPermitSet.DataSource = technologyPermitSet;
			rptTechnologyPermitSet.DataBind();

			locationPermitSet = Data.LocationPermitSet.GetPermitSetsInLocation(CurrentEvaluation.Location.ID);
			rptLocationPermitSet.DataSource = locationPermitSet;
			rptLocationPermitSet.DataBind();

			// Display Capacity Permits
			capacityPermitSet = Data.PreEvaluationResponsePermitSet.GetPermitSetsForPreEvaluationResponse(CurrentEvaluation.CapacityID, CurrentEvaluation.Location.ID);
			rptCapacityPermitSet.DataSource = capacityPermitSet;
			rptCapacityPermitSet.DataBind();

			rptCapacityQuestions.DataSource = CurrentEvaluation.CapacityAnswers;
			rptCapacityQuestions.DataBind();

			federalPermitSet = Data.PreEvaluationResponsePermitSet.GetPermitSetsForPreEvaluationResponse(CurrentEvaluation.FederalID, CurrentEvaluation.Location.ID);
			rptFederalPermitSet.DataSource = federalPermitSet;
			rptFederalPermitSet.DataBind();
		}

		private void CalculatePermitDependencies()
		{
			// go through distinctPermits		    

			ganntPermits = new List<Permit>();
			foreach (Permit permit in distinctPermits)
			{
				GetPermitDependency2(permit, 0);
			}
		}

		private void UpdateChildrenPermitDependency(Permit permit, int j)
		{
			List<PermitDependency> dependencies = GetChildPermitsNR(permit.ID); 
			if (dependencies.Count > 0)
			{
				foreach (PermitDependency dependency in dependencies)
				{
					// there should only be one permit
					if (dependency.Permits.Count > 0)
					{
						int childPermitID = dependency.Permits[0].ID;
						if (distinctPermits.Find(i => i.ID == childPermitID) != null)
						{
							Permit childPermit = distinctPermits.Find(i => i.ID == childPermitID);

							if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToStart)
							{
								if (permit.EndDuration > childPermit.StartDuration)
								{
									childPermit.StartDuration = permit.EndDuration;
									GetPermitDependency2(childPermit, j);
								}

							}
							else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.StartToStart)
							{
								if (permit.StartDuration > childPermit.StartDuration)
								{
									childPermit.StartDuration = permit.StartDuration;
									GetPermitDependency2(childPermit, j);
								}
							}
							else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToFinish)
							{
								if (permit.EndDuration > childPermit.EndDuration)
								{
									childPermit.StartDuration = permit.EndDuration - childPermit.Duration;
									GetPermitDependency2(childPermit, j);
								}
							}
						}
					}
					else
					{
						return;
					}
				}
			}
			else
			{
				return;
			}
		}

		private void UpdateParentPermitDependency(Permit permit, int j)
		{
			List<PermitDependency> dependencies = GetParentPermitsNR(permit.ID);
			if (dependencies.Count > 0)
			{
				foreach (PermitDependency dependency in dependencies)
				{
					int parentPermitID = dependency.DependentPermitID;
					if (distinctPermits.Find(i => i.ID == parentPermitID) != null)
					{
						Permit parentPermit = distinctPermits.Find(i => i.ID == parentPermitID);
						if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToStart)
						{
							if (parentPermit.EndDuration == permit.StartDuration)
							{
								break;
							}
							else if (parentPermit.EndDuration > permit.StartDuration)
							{
								parentPermit.StartDuration = permit.StartDuration - parentPermit.Duration;
								GetPermitDependency2(parentPermit, j);
							}
						}
						else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.StartToStart)
						{
							if (permit.StartDuration > parentPermit.StartDuration)
							{
								parentPermit.StartDuration = permit.StartDuration;
								GetPermitDependency2(parentPermit, j);
							}
						}
						else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToFinish)
						{
							if (parentPermit.EndDuration > permit.EndDuration)
							{
								parentPermit.StartDuration = permit.EndDuration - parentPermit.Duration;
								GetPermitDependency2(parentPermit, j);
							}
						}
					}
				}
			}
			else
			{
				return;
			}
		}

		private void GetPermitDependency2(Permit permit, int j)
		{
		    if (j > 6)
		    {
                return;
		    }

            j++;
			permit.Dependencies = Data.PermitDependency.GetPermitDependencies(permit.ID);
			if (permit.Dependencies.Count > 0)
			{
				foreach (PermitDependency dependency in permit.Dependencies)
				{
					if (dependency.Permits.Count > 0)
					{
						IEnumerable<Permit> availablePermits = distinctPermits.Intersect(dependency.Permits, new PermitComparer());

						if (availablePermits.ToList().Count > 0)
						{
							if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToStart)
							{
								int maxDelay = availablePermits.Max(i => i.EndDuration);

								permit.StartDuration = maxDelay;
								UpdateParentPermitDependency(permit, j);
								UpdateChildrenPermitDependency(permit, j);
							}
							else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.StartToStart)
							{
								int maxDelay = availablePermits.Max(i => i.StartDuration);
								if (permit.StartDuration < maxDelay)
								{
									permit.StartDuration = maxDelay;
									UpdateParentPermitDependency(permit, j);
									UpdateChildrenPermitDependency(permit, j);
								}
							}
							else if (dependency.DependencyType == Domain.Enumerations.PermitDependencyType.FinishToFinish)
							{
								int maxEndDuration = availablePermits.Max(i => i.EndDuration);
								if (permit.EndDuration < maxEndDuration)
								{
									permit.StartDuration = maxEndDuration - permit.Duration;
									UpdateParentPermitDependency(permit, j);
									UpdateChildrenPermitDependency(permit, j);
								}
							}
						}
					}
					else
					{
						return;
					}
				}
			}
			else
			{
				return;
			}
		}

        private void DisplayQuestionSets()
        {
            rptQuestionSet.DataSource = CurrentEvaluation.QuestionSets;
            rptQuestionSet.DataBind();
        }

        public IEnumerable<EvaluationResponse> DisplayAnswers(object dataItem)
        {
            var questionSet = dataItem as QuestionSet;
            if (questionSet != null)
            {
                var questionIds = questionSet.Questions.Select(i => i.ID).Distinct();
                return CurrentEvaluation.Answers.Where(i => questionIds.Contains(i.QuestionID)).Distinct();
            }
            return Enumerable.Empty<EvaluationResponse>();
        }

        public String DisplayTechnologyName(object dataItem)
        {
            return CurrentEvaluation.Technology.Name;
        }

		private void DisplayPermits()
		{
			List<Permit> permits = new List<Permit>();
			foreach (EvaluationResponse answer in CurrentEvaluation.Answers)
			{
				permits.AddRange(Data.ResponsePermitSet.GetPermitsForResponse(answer.ResponseID, CurrentEvaluation.Location.ID));
			}

			// Add permits for technology	
			permits.AddRange(Data.TechnologyPermitSet.GetPermitsInTechnology(CurrentEvaluation.Technology.ID));

			// Add permits for location 
			permits.AddRange(Data.LocationPermitSet.GetPermitsInLocation(CurrentEvaluation.Location.ID));

			// Add permits for capacity
			permits.AddRange(Data.PreEvaluationResponsePermitSet.GetPermitsForPreEvaluationResponse(CurrentEvaluation.CapacityID, CurrentEvaluation.Location.ID));

			// Add permits for capacity subquestions
			foreach (EvaluationResponse answer in CurrentEvaluation.CapacityAnswers)
			{
				permits.AddRange(Data.ResponsePermitSet.GetPermitsForResponse(answer.ResponseID, CurrentEvaluation.Location.ID));
			}

			distinctPermits = permits.Distinct(new PermitComparer()).ToList();

			CalculatePermitDependencies();

			rptFederalPermits.DataSource = distinctPermits.Where(i => i.PermitType.ID == 1).OrderBy(i => i.Name).OrderBy(i => i.StartDuration).ToList();
			rptFederalPermits.DataBind();

			if (rptFederalPermits.Items.Count == 0)
			{
				rptFederalPermits.Visible = false;
			}

			rptStatePermits.DataSource = distinctPermits.Where(i => i.PermitType.ID == 2).OrderBy(i => i.Name).OrderBy(i => i.StartDuration).ToList();
			rptStatePermits.DataBind();

			if (rptStatePermits.Items.Count == 0)
			{
				rptStatePermits.Visible = false;
			}

			rptPermits.DataSource = distinctPermits.Where(i => i.PermitType.ID >= 3).OrderBy(i => i.Name).OrderBy(i => i.StartDuration).ToList();
			rptPermits.DataBind();

			if (rptPermits.Items.Count == 0)
			{
				pnlPermitNoResults.Visible = true;
				pnlPermitResults.Visible = false;
				rptPermits.Visible = false;
			}
		}
		#region Page Display

		private void DisplayTechnology()
		{
			technologyID = CurrentEvaluation.Technology.ID;
			technologyName = CurrentEvaluation.Technology.Name;

			lblTechnology.Text = CurrentEvaluation.Technology.Name;
			lblTechnology2.Text = CurrentEvaluation.Technology.Name;
		}

		private void DisplayLocation()
		{
			locationName = CurrentEvaluation.Location.Name;
			lblLocation.Text = CurrentEvaluation.Location.Name;
			lblLocation2.Text = CurrentEvaluation.Location.Name;
		}

		#endregion

		#region Page Events
		protected string DisplayQuestion(object dataItem)
		{
			EvaluationResponse answer = dataItem as EvaluationResponse;
			return Data.Question.GetQuestion(answer.QuestionID).Name;
		}

		protected string DisplayResponse(object dataItem)
		{
			EvaluationResponse answer = dataItem as EvaluationResponse;
			return Data.Response.GetResponse(answer.ResponseID).Name;
		}

        protected List<PermitSetViewModel> DisplayPermitSet(object dataItem)
		{
			EvaluationResponse answer = dataItem as EvaluationResponse;
            return Data.ResponsePermitSet.GetPermitSetsForResponseWithPermitCount(answer.ResponseID, CurrentEvaluation.Location.ID);
		}

        protected string DisplayPermitSetUrl(object dataItem)
        {
            PermitSetViewModel p = (PermitSetViewModel)dataItem;

            if (string.IsNullOrEmpty(p.PermitUrl))
            {
                return p.PermitSetName;
            }
            else
            {
                return string.Format("<a class='permitLink' href='{0}' target='_blank'>{1}</a>", p.PermitUrl, p.PermitSetName);
            }
        }

		protected void rptAnswers_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			int pageIndex = 0;
			if (CurrentEvaluation.QuestionSets != null)
			{
				for (int i = 0; i < CurrentEvaluation.QuestionSets.Count; i++)
				{
					if (CurrentEvaluation.QuestionSets[i].Questions.Find(q => q.ID == int.Parse(e.CommandArgument.ToString())) != null)
					{
						pageIndex = i;
						break;
					}
				}

				CurrentEvaluation.CurrentPage = pageIndex + 1;

				Revisit = true;
				LoadQuestionSetPage();
			}
			else
			{
				LoadPreEvaluationPage();
			}
		}

		protected void lbRevisitAll_OnClick(object sender, EventArgs e)
		{
			Revisit = false;
			CurrentEvaluation.CurrentPage = 1;
			LoadQuestionSetPage();
		}

		protected void lbStartOver_OnClick(object sender, EventArgs e)
		{
			CurrentEvaluation = null;
			Response.Redirect("/evaluate/", true);
		}

		protected void rptPermits_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Permit permit = e.Item.DataItem as Permit;
				HyperLink hlToolTip = e.Item.FindControl("hlToolTip") as HyperLink;
				Literal ltToolTip = e.Item.FindControl("ltToolTip") as Literal;

				if (permit.DepartmentID.HasValue && permit.Department.Description != "")
				{
					hlToolTip.Attributes.Add("href", "#");
					ltToolTip.Text = HtmlSanitizer.SanitizeHtml(HtmlEncoder.BetterHtmlDecode(permit.Department.Description));
				}
				else
				{
					hlToolTip.Visible = false;
					hlToolTip.ImageUrl = "";
				}
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			CurrentEvaluation.Name = txtName.Text.Trim();
			CurrentEvaluation.Description = txtDescription.Text.Trim();
			if (rbStatus1.Checked)
			{
				CurrentEvaluation.StatusID = 1;
			}
			else
			{
				CurrentEvaluation.StatusID = 2;
			}

			if (IsLoggedIn)
			{
				SaveEvaluationToDB();
			}

			if (CurrentEvaluation.StatusID == (int)EvaluationStatusEnum.Complete)
			{
				lbRevisitAll.Visible = false;
				ltDivider.Visible = false;
				foreach (RepeaterItem item in rptQuestionSet.Items)
				{
				    var repeater = item.FindControl("rptAnswers") as Repeater;
                    foreach (RepeaterItem ri in repeater.Items)
                    {
                        ri.FindControl("lbRevisit").Visible = false;
                    }
				}
			}

			technologyID = CurrentEvaluation.TechnologyID;
			pnlSave.Visible = false;
			pnlSaveConfirmation.Visible = true;
            if (CurrentEvaluation.StatusID == (int)EvaluationStatusEnum.Complete)
            {
                Response.Redirect(string.Format("~/Evaluate/Review.aspx?id={0}", this.CurrentEvaluation.ID));
            }
		}


		#region Cached Values
		// Retrieve database values only once
		List<PermitDependency> permitDependencies = new List<PermitDependency>();
		public List<PermitDependency> PermitDependencies
		{
			get
			{
				if (permitDependencies.Count == 0)
				{
					permitDependencies = Data.PermitDependency.GetPermitDependenciesAll(PermitList);
				}
				return permitDependencies;
			}
		}

		List<Permit> permitList = new List<Permit>();
		public List<Permit> PermitList
		{
			get
			{
				if (permitList.Count == 0)
				{
					permitList = Data.Permit.GetPermits();
				}
				return permitList;
			}
		}
		public List<PermitDependency> GetPermitDependencies(int permitID)
		{
			return PermitDependencies.Where(i => i.PermitID == permitID).OrderBy(i => i.DependencyType).ToList();
		}

		public List<PermitDependency> GetChildPermitsNR(int permitID)
		{
			return PermitDependencies.Where(i => i.DependentPermitID == permitID).ToList();
		}

		public List<PermitDependency> GetParentPermitsNR(int permitID)
		{
			return PermitDependencies.Where(i => i.PermitID == permitID).ToList();
		}
		#endregion

	}
}