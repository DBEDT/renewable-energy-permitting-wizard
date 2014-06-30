using System.Diagnostics;

namespace HawaiiDBEDT.Web.evaluate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;
    using Data;
    using HawaiiDBEDT.Domain.Enumerations;
    using utils;
    using Westwind.Web.Utilities;

    using Evaluation = Domain.Evaluation;
    using EvaluationResponse = Domain.EvaluationResponse;
    using Permit = Domain.Permit;
    using PermitSet = Domain.PermitSet;
    using PreEvaluationQuestion = Domain.PreEvaluationQuestion;
    using Question = Domain.Question;
    using QuestionSet = Domain.QuestionSet;
    using HawaiiDBEDT.Data.ViewModels;

    public partial class review : BasePage
    {
        public Evaluation evaluation;
        public int EvaluationID;

        protected string technologyName;
        protected int technologyID;
        protected string locationName;

        protected List<PermitSet> technologyPermitSet;
        protected List<PermitSet> locationPermitSet;
        protected List<PermitSet> capacityPermitSet;
        protected List<PermitSet> federalPermitSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetEvaluation();
            if (!IsPostBack)
            {
                DisplayEvaluation();
            }
        }

        private void DisplayEvaluation()
        {
            lblDate.Text = evaluation.DateModified.ToShortDateString();
            pnlTitle.Visible = (evaluation.Name != "");
            pnlDescription.Visible = (evaluation.Description != "");

            DisplayTechnology();
            DisplayLocation();

            DisplayPreEvalAnswers();
            DisplayQuestionSets();

            DisplayPermits();
            this.chartImage.ImageUrl = "/permitPlan/getImage.aspx";
        }

        private void GetEvaluation()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && int.TryParse(Request.QueryString["id"], out EvaluationID))
            {
                evaluation = Data.Evaluation.GetEvaluation(EvaluationID);

                if (evaluation.UserID == CurrentUser.ID || "1".Equals(Request.QueryString["admin"]))
                {

                    if (!String.IsNullOrEmpty(Request.QueryString["clone"]))
                    {
                        CurrentEvaluation = evaluation;
                        CurrentEvaluation.ID = 0; // Reset id to create new one
                        CurrentEvaluation.StatusID = (int)EvaluationStatusEnum.InProgress;
                        CurrentEvaluation.Active = true;
                        if (CurrentEvaluation.Name != "")
                        {
                            CurrentEvaluation.Name = "Copy of " + CurrentEvaluation.Name;
                        }

                        if (CurrentEvaluation.Name.Length > 100)
                        {
                            CurrentEvaluation.Name = CurrentEvaluation.Name.Substring(0, 99);
                        }
                        SaveCurrentEvaluationComplete();

                        Review = true;
                        Response.Redirect("~/evaluate/", true);
                    }
                    else if (evaluation.StatusID == (int)EvaluationStatusEnum.InProgress)
                    {
                        CurrentEvaluation = evaluation;
                        CurrentEvaluation.QuestionSets =
                            Data.QuestionSet.GetQuestionSetComplete(CurrentEvaluation.TechnologyID,
                                                                    CurrentEvaluation.LocationID).ToList();
                        GetFederalResponses();
                        CurrentEvaluation.NumPages = CurrentEvaluation.QuestionSets.Count;
                        CurrentEvaluation.LastStepFilled = Data.EvaluationResponse.FindLastEvaluationStepFilled(CurrentEvaluation.ID);
                        Review = true;
                        Response.Redirect("~/evaluate/", true);
                    }
                    else
                    {
                        this.lblDate.Text = this.evaluation.DateModified.ToShortDateString();
                        
                        this.PrepareEvaluationForDisplay(evaluation);
                        if (!String.IsNullOrEmpty("admin"))
                        {
                            lbStartOver2.Visible = false;
                        }
                    }

                    return;
                }
            }

            Response.Redirect("~/profile/evaluations.aspx", true);
        }

        private void DisplayPreEvalAnswers()
        {
            List<PreEvaluationQuestion> questions = Data.PreEvaluationQuestion.GetPreEvaluationQuestions();
            PreEvaluationQuestion technologyQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Technology.ToString());
            PreEvaluationQuestion capacityQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Capacity.ToString());
            PreEvaluationQuestion locationQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Location.ToString());

            lblTechnologyQuestion.Text = technologyQuestion.Name;
            lblCapacityQuestion.Text = capacityQuestion.Name;
            lblLocationQuestion.Text = locationQuestion.Name;

            lblTechnologyAnswer.Text = technologyQuestion.LookupOptions.Find(i => i.ID == CurrentEvaluation.Technology.ID).Name;
            lblCapacityAnswer.Text = capacityQuestion.Responses.Find(i => i.ID == CurrentEvaluation.CapacityID).Name;
            lblLocationAnswer.Text = locationQuestion.LookupOptions.Find(i => i.ID == CurrentEvaluation.Location.ID).Name;

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
                return CurrentEvaluation.Answers.Where(i => questionIds.Contains(i.QuestionID));
            }
            return Enumerable.Empty<EvaluationResponse>();
        }

        public String DisplayTechnologyName(object dataItem)
        {
            var questionSet = dataItem as QuestionSet;

            if (questionSet != null && questionSet.Technology != null)
            {
                return questionSet.Technology.Name;
            }

            return string.Empty;
        }

        private void DisplayPermits()
        {
            rptFederalPermits.DataSource = CurrentEvaluation.DistinctPermits.Where(i => i.PermitType.ID == 1).OrderBy(i => i.Name).OrderBy(i => i.StartDuration).ToList();
            rptFederalPermits.DataBind();

            if (rptFederalPermits.Items.Count == 0)
            {
                rptFederalPermits.Visible = false;
            }

            rptStatePermits.DataSource = CurrentEvaluation.DistinctPermits.Where(i => i.PermitType.ID == 2).OrderBy(i => i.Name).OrderBy(i => i.StartDuration).ToList();
            rptStatePermits.DataBind();

            if (rptStatePermits.Items.Count == 0)
            {
                rptStatePermits.Visible = false;
            }

            rptPermits.DataSource = CurrentEvaluation.DistinctPermits.Where(i => i.PermitType.ID >= 3).OrderBy(i => i.Name).OrderBy(i => i.StartDuration).ToList();
            rptPermits.DataBind();

            if (rptPermits.Items.Count == 0)
            {
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
            PermitSetViewModel p = (PermitSetViewModel) dataItem;

            if (string.IsNullOrEmpty(p.PermitUrl))
            {
                return p.PermitSetName;
            }
            else
            {
                return string.Format("<a class='permitLink' href='{0}' target='_blank'>{1}</a>", p.PermitUrl, p.PermitSetName);
            }
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

    }
}