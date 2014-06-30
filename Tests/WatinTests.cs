using System.Threading;

namespace Tests
{
    using System.Configuration;
    using NUnit.Framework;
    using System;
    using WatiN.Core;

    [TestFixture, RequiresSTA]
    class WatinTests
    {
        [Test]
        public void FillForm()
        {
            string url = ConfigurationManager.AppSettings["url"];
            string user = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];
            using (var browser = new IE(url))
            {
                if (!browser.Elements.Exists("loginControl_txtEmail"))
                {
                    browser.Link(Find.ById("ctl00_hlLogout")).Click();
                }
                browser.TextField(Find.ById("loginControl_txtEmail")).SetAttributeValue("value", user);
                browser.TextField(Find.ById("loginControl_txtPassword")).SetAttributeValue("value", password);
                browser.Image(Find.ById("loginControl_submitBtn")).Click();

                browser.Link(Find.ById("lbStartOver2")).Click();
                browser.RunScript("checkResponses();");
                browser.Link(Find.ById("EvaluationControl1_PreEvalControl1_lbStartEval")).Click();

                for (int i = 0; i < 7; i++)
                {
                    browser.RunScript("checkResponses();");
                    browser.Link(Find.ById("EvaluationControl1_QuestionSetControl1_lbNext")).Click();
                }

                browser.TextField(Find.ById("EvaluationControl1_ResultsControl1_txtName")).SetAttributeValue("value", string.Format("auto gen {0}", DateTime.Now));
                browser.Link(Find.ById("EvaluationControl1_ResultsControl1_btnSubmit")).Click();

                Thread.Sleep(2000);
            }
        }
    }
}
