using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.content
{
    public partial class ruleEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.SubMenu = "rss feeds";
            Master.Heading = "Content/Messages > Dynamic Tags > Rules List > Edit Rule";
            Master.HelpContent = "";
            Master.HelpTitle = "Manage RSS Feeds";
        }                

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(ruleEditor1.SaveRule()>0)
                Response.Redirect("rulelist.aspx");
        }
    }
}