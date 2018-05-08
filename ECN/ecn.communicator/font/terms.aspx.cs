using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.front
{
    public partial class terms : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Site Owner Agreement and Software License Terms";
            Master.HelpContent = "<p>Spam is unsolicited email also known as junk mail or UCE (Unsolicited Commercial Email.) By sending email to only to those who have requested to receive it, you are following accepted permission-based email guidelines. </p>";
            Master.HelpTitle = "Agreement to Terms";
        }

    }
}