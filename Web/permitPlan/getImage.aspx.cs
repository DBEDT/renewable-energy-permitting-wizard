namespace HawaiiDBEDT.Web.permitPlan
{
    using System;
    using System.Web;

    public partial class getImage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentEvaluation.DistinctPermits == null)
            {
                this.PrepareEvaluationForDisplay(CurrentEvaluation);
            }

            byte[] image = permitChart.GetImageData();
            if (!string.IsNullOrEmpty(Request.QueryString["download"]))
            {
                string whiteSpace = " ";
                var evaluationTitle = string.IsNullOrEmpty(CurrentEvaluation.Name)
                                          ? whiteSpace
                                          : CurrentEvaluation.Name;
                string titleEncoded = HttpUtility.UrlEncode(evaluationTitle);
                var evaluationDate = CurrentEvaluation.DateModified.ToShortDateString();
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=permitPlan_{0}_{1}.png", titleEncoded, evaluationDate));
            }

            Response.ContentType = "image/png";
            Response.OutputStream.Write(image, 0, image.Length - 1);
            Response.End();
        }
    }
}