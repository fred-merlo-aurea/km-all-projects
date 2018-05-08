using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace ecn.communicator.engines
{
    public partial class reportSpam : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e) 
        {
            Response.Redirect(ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/reportSpam.aspx" + Request.Url.Query);

        }
    }
}