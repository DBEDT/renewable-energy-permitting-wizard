using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HawaiiDBEDT.Web.admin
{
    using log4net;

    public class ErrorHandlingModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.Error += HandleError;
        }

        private void HandleError(object sender, EventArgs eventArgs)
        {
            ILog log = LogManager.GetLogger(typeof(Global));
            log.Error("Unhandled exception", HttpContext.Current.Server.GetLastError());
        }

        public void Dispose()
        {
        }
    }
}