namespace HawaiiDBEDT.Web.permitPlan
{
    using System.IO;
    using Data;
    using Domain.Pdf;
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class PermitImage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.PrepareEvaluationForDisplay(CurrentEvaluation);

                var localReport = new LocalReport();
                localReport.ReportPath = "permitPlan\\image.rdlc";
                localReport.DataSources.Add(new ReportDataSource("imgDataSet", DisplayImage()));
                localReport.EnableExternalImages = true;
                localReport.EnableHyperlinks = true;

                string whiteSpace = " ";
                var evaluationTitle = string.IsNullOrEmpty(CurrentEvaluation.Name)
                                          ? whiteSpace
                                          : CurrentEvaluation.Name;
                var evaluationDate = CurrentEvaluation.DateModified.ToShortDateString();

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

        public List<ChartImage> DisplayImage()
        {
            return new List<ChartImage> {new ChartImage() {Image = this.permitChart.GetImageData()}};
        } 
    }
}