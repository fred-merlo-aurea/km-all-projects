using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.activityengines
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblHeader = Master.FindControl("lblHeader") as Label;
            lblHeader.Text = "";
            Label lblFooter = Master.FindControl("lblFooter") as Label;
            lblFooter.Text = "";
            ECN_Framework_Entities.Accounts.LandingPageAssign LPA = null;
            Page.Title = "Error";
            try
            {
                if (Cache[string.Format("cache_LandingPageAssign_by_LPID_{0}_and_CustomerID{1}", 2, -1)] == null)
                {
                    LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(2, -1, true);
                    Cache.Add(string.Format("cache_LandingPageAssign_by_LPID_{0}_and_CustomerID{1}", 2, -1), LPA, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    LPA = (ECN_Framework_Entities.Accounts.LandingPageAssign)Cache[string.Format("cache_LandingPageAssign_by_LPID_{0}_and_CustomerID{1}", 2, -1)];
                }
                lblHeader.Text = LPA.Header;
                lblFooter.Text = LPA.Footer;
            }
            catch (Exception){  }

            string error = "HardError";
            try
            {
                error = Request.QueryString.ToString().Substring(2, Request.QueryString.ToString().Length - 2);
            }
            catch {}
            if (error == "InvalidLink")
            {
                lblMessage.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.InvalidLink);
            }
            else if (error == "PageNotFound")
            {
                lblMessage.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.PageNotFound);
            }
            else if (error == "Timeout")
            {
                lblMessage.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.Timeout);
            }
            else
            {
                lblMessage.Text = ActivityError.GetErrorMessage(Enums.ErrorMessage.HardError);
            }
        }
    }
}