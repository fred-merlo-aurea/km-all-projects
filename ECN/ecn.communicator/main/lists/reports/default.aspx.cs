using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace ecn.communicator.main.lists.reports
{
    public partial class _default : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        //   Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
        //    Master.SubMenu = "reports";
        //    Master.Heading = "";
        //    Master.HelpContent = "";
        //    Master.HelpTitle = "";	

        //    if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures. "Flash Reporting"))
        //    {
        //        pnlFlashReport.Visible = true;
        //    }
        //    else
        //    {
        //        pnlFlashReport.Visible = false;
        //    }
            Response.Redirect("/ecn.communicator/main/Reports/ListReports.aspx");
        }
    }
}
