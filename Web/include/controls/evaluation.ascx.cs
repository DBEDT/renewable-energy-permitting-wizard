using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HawaiiDBEDT.Domain;

namespace HawaiiDBEDT.Web.Controls
{
	public partial class EvaluationControl : BaseControl
	{
	    public String PreEvaluationControlId
	    {
	        get
	        {
                return this.PreEvalControl1.ID;
	        }
	    }

	    public String QuestionSetControlId
	    {
	        get
	        {
                return this.QuestionSetControl1.ID;
	        }
	    }
		
        protected void Page_Load(object sender, EventArgs e)
		{
		}

	    public void SaveStepData()
	    {
	        this.QuestionSetControl1.UpdateQuestionSetResponses();
	    }
	}
}