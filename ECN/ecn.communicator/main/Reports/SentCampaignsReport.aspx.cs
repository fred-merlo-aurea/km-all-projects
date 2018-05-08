using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.Reports
{
    public partial class SentCampaignsReport : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "sent campaign reports";
            Master.Heading = "Reports > Sent Campaign Reports";
            Master.HelpContent = "";
            Master.HelpTitle = "";
        }
    }
}