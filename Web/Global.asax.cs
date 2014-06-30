using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Net.Mail;

namespace HawaiiDBEDT.Web
{
    using log4net;

    public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
            ILog log = LogManager.GetLogger(typeof(Global));
            log.Info("App Started");
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{
            if (!Request.Path.ToLower().Contains("/webresource.axd"))
			{
                ILog log = LogManager.GetLogger(typeof(Global));
                log.Error("Unhandled exception", Server.GetLastError());
			}
		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}