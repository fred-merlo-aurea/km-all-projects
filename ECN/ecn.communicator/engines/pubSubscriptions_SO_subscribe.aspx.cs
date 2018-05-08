using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace ecn.communicator.engines 
{
	public partial class pubSubscriptions_SO_subscribe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/pubSubscriptions_SO_subscribe.aspx" + Request.Url.Query);
        }
	}
}