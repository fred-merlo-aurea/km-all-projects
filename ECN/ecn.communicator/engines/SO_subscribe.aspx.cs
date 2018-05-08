using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.communicator.engines
{
    public partial class SO_subscribe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/SO_subscribe.aspx" + Request.Url.Query);

        }
    }
}