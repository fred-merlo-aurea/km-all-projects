using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.front
{
    public partial class spam : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.LOGIN;
            Master.SubMenu = "";
            Master.Heading = "ECN.communicator Anti-Spam Policy";
            Master.HelpContent = "By clicking the 'I accept these terms and conditions' button on the sign-up page, you accept these terms and conditions.";
            Master.HelpTitle = "What is Spam?";
        }

    }
}