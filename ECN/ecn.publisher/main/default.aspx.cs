using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ecn.publisher.main 
{

	public partial class _default : System.Web.UI.Page 
{

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string vp = System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"];
                if (!(Convert.ToInt32(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.PublisherLevel) > 0))
                    Response.Redirect(vp + "/main/default.aspx");
            }
            catch
            {
                Response.Redirect("/ecn.accounts/main/default.aspx");
            }
            Response.Redirect("edition/default.aspx", true);

        }
	}
}
