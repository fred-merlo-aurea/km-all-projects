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
using System.Web.Security;

namespace ecn.communicator {
	
	public partial class homepage : System.Web.UI.Page {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            string[] cookies = Response.Cookies.AllKeys;
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] + "/main/default.aspx");
        }

	}
}
