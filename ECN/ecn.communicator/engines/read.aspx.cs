using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;

namespace ecn.communicator.engines
{
    public partial class read : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/read.aspx" + Request.Url.Query);
        }
    }
}
