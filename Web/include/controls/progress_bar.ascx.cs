using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;

namespace HawaiiDBEDT.Web.Controls
{
    public partial class ProgressBarControl : BaseControl
    {
        public string ActiveImagePath = "/images/eval_square_active.png";
        public string InactiveImagePath = "/images/eval_square_inactive.png";

        public int? CurrentDimensionIndex
        {
            get
            {
                return CurrentEvaluation.PageIndex;
            }
        }

        public int NumDimensions
        {
            get
            {
                return CurrentEvaluation.NumPages;
            }
        }

        public void LoadPage()
        {
            if (!CurrentEvaluation.Active.Value || (CurrentDimensionIndex < 0 && CurrentEvaluation.LastStepFilled == 0))
            {
                pnlProgressBar.CssClass = "evalProgress1";
                ClearProgressDots();
            }
            else if (NumDimensions == 0)
            {
                pnlProgressBar.CssClass = "evalProgress3";
                DisplayProgressDots();
            }
            else if (CurrentDimensionIndex >= NumDimensions)
            {
                pnlProgressBar.CssClass = "evalProgress3";
                DisplayProgressDots();
            }
            else
            {
                pnlProgressBar.CssClass = "evalProgress2";
                DisplayProgressDots();
            }
        }

        private void ClearProgressDots()
        {
            rptDimensionProgress.DataSource = new List<QuestionSet>();
            rptDimensionProgress.DataBind();
            rptDimensionProgress.Visible = false;
        }

        private void DisplayProgressDots()
        {
            List<QuestionSet> Dimensions = new List<QuestionSet>();
            for (int i = 0; i < NumDimensions; i++)
            {
                Dimensions.Add(new QuestionSet() { ID = i, Name = i.ToString() });
            }
            rptDimensionProgress.DataSource = Dimensions;
            rptDimensionProgress.DataBind();
            rptDimensionProgress.Visible = true;
        }

        protected string GetImagePath(object DimensionIDObject)
        {
            int DimensionID = int.Parse(DimensionIDObject.ToString());
            if (DimensionID <= CurrentDimensionIndex)
            {
                return ActiveImagePath;
            }
            else
            {
                return InactiveImagePath;
            }
        }

        protected string GetStepTitle(int stepIndex)
        {
            var questionSet = CurrentEvaluation.QuestionSets[stepIndex];
            if (questionSet != null)
            {
                return string.Format("{0} {1} ", CurrentEvaluation.Technology.Name, questionSet.Name);
            }

            return string.Empty;
        }

        protected bool EnableNavigationToTheStep(int stepId)
        {
            return stepId < CurrentEvaluation.LastStepFilled;
        }

        protected void LnkBtnPreEvaluationClick(object sender, EventArgs e)
        {
            var evaluationControl = this.Parent as EvaluationControl;
            if (evaluationControl != null)
            {
                var questionSetControl = evaluationControl.FindControl(evaluationControl.QuestionSetControlId) as QuestionSetControl;
                if (questionSetControl != null)
                {
                    questionSetControl.LoadPreEvaluationPage();
                }
            }
        }

        protected void HandleImgButtonClick(object source, RepeaterCommandEventArgs e)
        {
            int stepIndex;
            var evaluationControl = this.Parent as EvaluationControl;
            if (evaluationControl != null && Int32.TryParse(e.CommandArgument.ToString(), out stepIndex))
            {
                if (CurrentEvaluation.CurrentPage == 0)
                {
                    this.LoadPreEvaluationPage();
                    var preEvaluationControl = evaluationControl.FindControl(evaluationControl.PreEvaluationControlId) as PreEvaluationControl;
                    if (preEvaluationControl != null)
                    {
                        preEvaluationControl.StartEvaluationFromNavigation(++stepIndex);
                    }
                }
                else
                {
                    stepIndex++;
                    CurrentEvaluation.CurrentPage = stepIndex;
                    LoadQuestionSetPage();
                    var questionSetControl =
                        evaluationControl.FindControl(evaluationControl.QuestionSetControlId) as QuestionSetControl;
                    if (questionSetControl != null)
                    {
                        questionSetControl.NavigateToStep(false);
                    }
                }
            }
        }
    }
}