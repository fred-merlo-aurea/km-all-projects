using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.front
{
    public partial class privacy : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "Privacy Policy";
            Master.HelpContent = "<p>If you received a mailing from us, (a) your email address is either listed with us as someone who has expressly shared this address for the purpose of receiving information in the future (&quot;opt-in&quot;), or (b) you have registered or purchased or otherwise have an existing relationship with us. We respect your time and attention by controlling the frequency of our mailings. </p>";
            Master.HelpTitle = "Why did you receive an email from us?";
        }
    }
}