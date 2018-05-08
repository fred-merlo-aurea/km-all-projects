using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace wattpaidpub
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblmessage.Text = Request["msg"];
            }
            catch
            {
                lblmessage.Text = "Unspecified Error";
            }
        }
    }
}

