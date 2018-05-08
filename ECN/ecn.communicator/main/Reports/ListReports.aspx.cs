using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.Reports
{
    public partial class Default : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "group reports";
            Master.Heading = "Reports > Group Reports";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.FlashReporting))
            {
                pnlFlashReport.Visible = true;
            }
            else
            {
                pnlFlashReport.Visible = false;
            }
        }
    }
}