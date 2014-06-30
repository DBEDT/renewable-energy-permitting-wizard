namespace HawaiiDBEDT.Web.permitPlan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Data;
    using Domain.Enumerations;
    using Domain.Pdf;
    using Microsoft.Reporting.WebForms;
    using System.IO;

    public partial class PermitPlan : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.PrepareEvaluationForDisplay(CurrentEvaluation);
                var responses = new List<ResponseGrid>();
                var evaluationResponses = Data.Pdf.ResponseGrid.GetItems(CurrentEvaluation.ID);
                var capacityResponses = evaluationResponses.Where(i => i.PageNo == 0);
                responses.AddRange(DisplayPreEvalAnswers(capacityResponses));
                responses.AddRange(evaluationResponses.Where(i => i.PageNo > 0));
                CalculatePageBreaks(responses);

                var localReport = new LocalReport();
                localReport.SubreportProcessing += DemoSubreportProcessingEventHandler;
                localReport.ReportPath = "permitPlan\\permitPlan.rdlc";
                localReport.DataSources.Add(new ReportDataSource("permitPlan", responses));
                localReport.DataSources.Add(new ReportDataSource("federalPermits", DisplayFederalPermits()));
                localReport.DataSources.Add(new ReportDataSource("statePermits", DisplayStatePermits()));
                localReport.DataSources.Add(new ReportDataSource("countyPermits", DisplayCountyPermits()));
                localReport.DataSources.Add(new ReportDataSource("imageData", DisplayImage()));
                localReport.EnableExternalImages = true;
                localReport.EnableHyperlinks = true;

                string whiteSpace = " ";
                var reportParameters = new ReportParameterCollection();
                var evaluationTitle = string.IsNullOrEmpty(CurrentEvaluation.Name)
                                          ? whiteSpace
                                          : CurrentEvaluation.Name;
                var evaluationDesc = string.IsNullOrEmpty(CurrentEvaluation.Description)
                                          ? whiteSpace
                                          : CurrentEvaluation.Description;
                var evaluationDate = CurrentEvaluation.DateModified.ToShortDateString();
                reportParameters.Add(new ReportParameter("EvaluationDate", evaluationDate));
                reportParameters.Add(new ReportParameter("EvaluationTitle", evaluationTitle));
                reportParameters.Add(new ReportParameter("EvaluationDescription", evaluationDesc));
                reportParameters.Add(new ReportParameter("TechnologyName", CurrentEvaluation.Technology.Name));
                reportParameters.Add(new ReportParameter("LocationName", CurrentEvaluation.Location.Name));
                localReport.SetParameters(reportParameters);

                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0in</MarginTop>" +
                "  <MarginLeft>0in</MarginLeft>" +
                "  <MarginRight>0in</MarginRight>" +
                "  <MarginBottom>0in</MarginBottom>" +
                "</DeviceInfo>";
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = localReport.Render(
                    "PDF",
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings
                    );

                string titleEncoded = HttpUtility.UrlEncode(evaluationTitle);
                string contentType = string.IsNullOrEmpty(Request.QueryString["print"])
                    ? "application/octet-stream"
                    : "application/pdf";
                Response.ClearHeaders();
                Response.ClearContent();
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", string.Format("inline;filename=session_{0}_{1}.pdf", titleEncoded, evaluationDate));
                Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
                if (!string.IsNullOrEmpty(Request.QueryString["print"]))
                {
                    var pdfPrintHelper = new PdfPrintHelper(Response.OutputStream);
                    pdfPrintHelper.AnnotateForPrinting(new MemoryStream(renderedBytes));
                }
                else
                {
                    Response.BinaryWrite(renderedBytes);
                }

                Response.End();
            }
        }

        private void CalculatePageBreaks(List<ResponseGrid> responses)
        {
            var questionSets = new Dictionary<string, bool>();
            int i = 0;
            foreach (var response in responses)
            {
                if (!questionSets.ContainsKey(response.QuestionSet))
                {
                    questionSets.Add(response.QuestionSet, true);
                    if (questionSets.Keys.Count%2 == 1 && questionSets.Keys.Count > 1)
                    {
                        i++;
                    }
                }

                response.PageBreak = i.ToString();
            }
        }

        protected void DemoSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            var mainSource = ((LocalReport)sender).DataSources["permitPlan"];
            var rowNumber = int.Parse(e.Parameters["RowNumber"].Values.First());
            List<PermitSetInfo> permitSetInfos = ((List<Domain.Pdf.ResponseGrid>) mainSource.Value).Where(x => x.Question == e.Parameters["Question"].Values.First()).First().PermitSetInfo;
            e.DataSources.Add(new ReportDataSource("permitSetInfo", permitSetInfos));
        }

        private IEnumerable<ResponseGrid> DisplayPreEvalAnswers(IEnumerable<ResponseGrid> capacityAnswers)
        {
            var preEvalResponses = new List<ResponseGrid>();
            var questions = Data.PreEvaluationQuestion.GetPreEvaluationQuestions();
            var technologyQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Technology.ToString());
            var capacityQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Capacity.ToString());
            var locationQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Location.ToString());
            var federalQuestion = questions.Find(i => i.LookupClassName == PreEvaluationQuestionType.Federal.ToString());

            var technologyPermitSetInfo = Data.TechnologyPermitSet.GetPermitSetsInTechnology(CurrentEvaluation.Technology.ID).Select(permitSet => new PermitSetInfo { Name = permitSet.Name, Urls = permitSet.Permits != null ? permitSet.Permits.Select(i => i.URL).ToList() : null}).ToList();
            preEvalResponses.Add(new ResponseGrid() { Question = technologyQuestion.Name, Response = technologyQuestion.LookupOptions.Find(i => i.ID == CurrentEvaluation.Technology.ID).Name, PermitSetInfo = technologyPermitSetInfo, QuestionSet = "Pre-Evaluation" });

            var capacityPermitSetInfo = Data.PreEvaluationResponsePermitSet.GetPermitSetsForPreEvaluationResponse(CurrentEvaluation.CapacityID, CurrentEvaluation.Location.ID).Select(permitSet => new PermitSetInfo { Name = permitSet.Name, Urls = permitSet.Permits != null ? permitSet.Permits.Select(i => i.URL).ToList() : null }).ToList();
            preEvalResponses.Add(new ResponseGrid() { Question = capacityQuestion.Name, Response = capacityQuestion.Responses.Find(i => i.ID == CurrentEvaluation.CapacityID).Name, PermitSetInfo = capacityPermitSetInfo, QuestionSet = "Pre-Evaluation" });

            preEvalResponses.AddRange(capacityAnswers);

            var locationPermitSetInfo = Data.LocationPermitSet.GetPermitSetsInLocation(CurrentEvaluation.Location.ID).Select(permitSet => new PermitSetInfo { Name = permitSet.Name, Urls = permitSet.Permits != null ? permitSet.Permits.Select(i => i.URL).ToList() : null }).ToList();
            preEvalResponses.Add(new ResponseGrid() { Question = locationQuestion.Name, Response = locationQuestion.LookupOptions.Find(i => i.ID == CurrentEvaluation.Location.ID).Name, PermitSetInfo = locationPermitSetInfo, QuestionSet = "Pre-Evaluation" });

            return preEvalResponses;
        }

        public List<PermitGrid> DisplayFederalPermits()
        {
            return GetPermitsByType(1);
        }

        public List<PermitGrid> DisplayStatePermits()
        {
            return GetPermitsByType(2);
        }

        public List<PermitGrid> DisplayCountyPermits()
        {
            return GetPermitsByType(3);
        } 

        private List<PermitGrid> GetPermitsByType(int permitType)
        {
            var permits =
                CurrentEvaluation.DistinctPermits.Where(i => i.PermitType.ID == permitType).OrderBy(i => i.Name).OrderBy(i => i.StartDuration);
            var permitGridItems = new List<PermitGrid>();
            foreach (var permit in permits)
            {
                permitGridItems.Add(new PermitGrid
                                        {
                                            Name = permit.Name,
                                            ShortName = permit.ShortName,
                                            StartMonth = permit.StartDuration,
                                            EndMonth = permit.EndDuration,
                                            DurationMonth = permit.Duration,
                                            Department = permit.DepartmentName,
                                            DepartmentDescription = permit.Department.Description,
                                            AppendixName = permit.AppendixLink,
                                            AppendixUrl = permit.URL
                                        });
            }

            return permitGridItems;
        }

        public List<ChartImage> DisplayImage()
        {
            return new List<ChartImage> {new ChartImage() {Image = this.permitChart.GetImageData()}};
        } 
    }
}