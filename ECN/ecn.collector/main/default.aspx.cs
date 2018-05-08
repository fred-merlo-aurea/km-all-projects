using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ecn.collector.main
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["Collector_VirtualPath"] + "/main/survey/default.aspx");
        }
    }
}
