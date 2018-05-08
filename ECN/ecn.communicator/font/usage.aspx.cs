using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.front
{
    public partial class usage : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Prohibited Content and Commerce Statement";
            Master.HelpContent = "<p>TeckMan reserves the right to prohibit the use of ECN.communicator by any company or site in its sole discretion. As always, sending unsolicited commercial email is FORBIDDEN. See our <a href='terms.aspx'>Terms & Conditions</a> and our Spam Policy for details. This may change from time to time. Any questions about whether your site can use ECN.communicator can be emailed to <a href='mailto:privacy@teckman.com.'>privacy@teckman.com.</a></p>";
            Master.HelpTitle = "All Rights Reserved";
        }

    }
}