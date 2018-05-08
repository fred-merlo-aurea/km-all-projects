using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.activityengines
{
    public partial class Status : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool monitor = false;
            lblResult.Text = "FAIL";
            try
            {       
                ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(Server.UrlDecode(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1)));
                bool.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.Monitor).ParameterValue, out monitor);
                if(monitor)
                    if (ECN_Framework_BusinessLayer.Accounts.Code.Exists() && ECN_Framework_BusinessLayer.Accounts.Code.Exists() && ECN_Framework_BusinessLayer.Accounts.Code.Exists())
                        lblResult.Text = "SUCCESS";
            }
            catch { }
        }
    }
}