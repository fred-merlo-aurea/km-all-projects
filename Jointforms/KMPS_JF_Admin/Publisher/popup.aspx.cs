using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMPS_JF_Setup.Publisher
{
    public partial class popup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s = System.DateTime.Now.ToLongDateString()  + " / " + System.Guid.NewGuid();

            Response.Write(s);
            Session["lbl"] = s;
        }
    }
}
