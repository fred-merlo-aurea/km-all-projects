using System;
using System.Data;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ecn.communicator.engines
{
    public partial class unsubscribeDirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/unsubscribeDirect.aspx" + Request.Url.Query);
        }
    }
}