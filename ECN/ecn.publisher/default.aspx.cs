using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace ecn.publisher
{
	public partial class Index : System.Web.UI.Page
    {
		protected void Page_Load(object sender, System.EventArgs e)
        {
			Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["Publisher_VirtualPath"]+"/main/default.aspx");
		}
	}
}
