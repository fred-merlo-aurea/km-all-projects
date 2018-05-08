using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;


namespace ecn.activityengines
{

    public partial class websubscribe : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //redirect to unsubscribe as this page is old
            KM.Common.Entity.ApplicationLog.LogNonCriticalError("Should no longer be using this page", "websubscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            string redirect = Request.RawUrl.ToString().Replace("websubscribe.aspx", "unsubscribe.aspx");
            Response.Write("<script language='javascript'>window.location.href='" + redirect.Replace("'", "\\'") + "'</script>");
        }
    }
}
