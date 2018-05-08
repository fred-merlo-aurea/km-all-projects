using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace ecn.activityengines
{
    public partial class WattUnsubscribe : System.Web.UI.Page
    {
        private KMPlatform.Entity.User User; 
        private int CustomerID = 0;
        private int GroupID = 0;
        private int BlastID = 0;
        private int RefBlastID = 0;
        private int EmailID = 0;
        private string EmailAddress = string.Empty;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //redirect to unsubscribe as this page is old
            KM.Common.Entity.ApplicationLog.LogNonCriticalError("Should no longer be using this page", "wattunsubscribe.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));

            //take emailid and blastid and create querystring for unsubscribe
            if (Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
            {
                User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                User = (KMPlatform.Entity.User)Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
            }

            if (Request.Url.Query.ToString().Length > 0)
            {
                GetValuesFromQuerystring(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
            }

            if (BlastID > 0)
            {
                ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(BlastID, User, true);
                if (blast != null) 
                {
                    if (blast.CustomerID != null && blast.CustomerID.Value > 0)
                        CustomerID = blast.CustomerID.Value;
                    if(blast.GroupID != null && blast.GroupID.Value > 0)
                        GroupID = blast.GroupID.Value;
                }
            }

            if (EmailID > 0)
            {
                ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(EmailID, User);
                if (email != null && email.EmailAddress != string.Empty)
                    EmailAddress = email.EmailAddress;
            }
            string redirect = ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/unsubscribe.aspx?e=" + EmailAddress + "&g=" + GroupID.ToString() + "&b=" + BlastID.ToString() + "&c=" + CustomerID.ToString();
            Response.Write("<script language='javascript'>window.location.href='" + redirect.Replace("'", "\\'") + "'</script>");
        }

        private void GetValuesFromQuerystring(string queryString)
        {
            try
            {
                ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(Server.UrlDecode(QSCleanUP(queryString)));
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailID).ParameterValue, out EmailID);
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.BlastID).ParameterValue, out BlastID);
            }
            catch { }
        }

        private string QSCleanUP(string querystring)
        {
            try
            {
                querystring = querystring.Replace("&amp;", "&");
                querystring = querystring.Replace("&lt;", "<");
                querystring = querystring.Replace("&gt;", ">");
            }
            catch (Exception)
            {
            }

            return querystring.Trim();
        }
    }
}
