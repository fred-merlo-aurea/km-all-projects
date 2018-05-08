using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;

namespace ecn.communicator.engines
{
    public partial class open : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/open.aspx" + Request.Url.Query);
        }
    }
}