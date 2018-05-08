using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;


namespace ecn.communicator.engines
{
    public partial class linkfrom : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/linkfrom.aspx" + Request.Url.Query);
        }
    }
}