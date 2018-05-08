using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.engines
{
    public partial class emailProfileManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {

            Response.Redirect(ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/emailProfileManager.aspx" + Request.Url.Query);
        }
    }
}