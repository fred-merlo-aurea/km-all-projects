using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMPS_JF_Setup.Publisher
{
    public partial class popuptest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.btnReload, this.GetType(), "reloaditems", "function reloadpage(){hs.getExpander().close() ;" + Page.ClientScript.GetPostBackEventReference(this.btnReload, null) + "}", true);
            btnReload.Attributes.Add("style", "visibility:hidden");

        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            //lbl.Text = Session["lbl"].ToString();
        }

    }
}
