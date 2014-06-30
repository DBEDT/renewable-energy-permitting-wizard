using System.Web.UI;

namespace HawaiiDBEDT.Web.Evaluate
{
    using System;

    using HawaiiDBEDT.Web.Controls;

    public partial class Default : BasePage
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            #if FILLFORM
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterClientScriptBlock(GetType(), "FillFormScript", "<script type='text/javascript' src='/include/js/fillFormScript.js'>", false);
            #endif

            var postbackReference = "{" + Page.ClientScript.GetPostBackEventReference(this, "saveIncompleteStep") + "}";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "incompleteStepScript"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "incompleteStepScript", string.Format(@"<script>function saveIncompleteStep() {0}</script>", postbackReference), false);
            }

            if (Page.IsPostBack)
            {
                if (Request["__EVENTARGUMENT"] == "saveIncompleteStep")
                {
                    this.SaveCurrentStepData();
                }
            } 
        }

	    public void SaveCurrentStepData()
	    {
            var evaluationControl = this.EvaluationControl1;
	        if (evaluationControl != null)
	        {
                evaluationControl.SaveStepData();
	        }

            Response.Redirect("/profile/logout.aspx");
	    }
	}
}