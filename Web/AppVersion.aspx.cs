using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HawaiiDBEDT.Web
{
    using System.Reflection;

    public partial class AppVersion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}