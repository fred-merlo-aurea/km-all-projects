using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.blasts
{
    public partial class Reports : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("/ecn.communicator/main/Reports/BlastReports.aspx");
        }
    }
}