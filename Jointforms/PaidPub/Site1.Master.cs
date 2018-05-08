using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace PaidPub.MasterPage
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude("validation", ResolveUrl("~/scripts/Validation.js"));

        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Page.Session["UserID"]) == 0)
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
        }

    }
}
