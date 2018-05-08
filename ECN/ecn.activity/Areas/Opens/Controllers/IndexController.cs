using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ecn.activity.Areas.Opens.Controllers
{
    public class IndexController : Controller
    {
        int EmailID = -2;
        int BlastID = 0;
        int RefBlastID = 0;
        KMPlatform.Entity.User User = null;
        //
        // GET: /Opens/Index/
        [HttpGet]
        public FileContentResult Index(string query)
        {
            string rawURL = Request.ServerVariables["HTTP_URL"].ToString();
            query = rawURL.Substring(rawURL.ToLower().IndexOf("opens/") + 6);
            try
            {
                if (HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())] == null)
                {
                    User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
                    HttpRuntime.Cache.Add(string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString()), User, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    User = (KMPlatform.Entity.User)HttpRuntime.Cache[string.Format("cache_user_by_AccessKey_{0}", ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString())];
                }

                GetQueryParams(query);

                
                if (BlastID > 0 && EmailID >= -1)
                {
                    RefBlastID = GetRefBlastID();
                    if (ConfigurationManager.AppSettings["ValidateB4Tracking"].ToString().ToLower().Equals("true"))
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.EmailGroup.ValidForTracking(RefBlastID, EmailID))
                        {
                            ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertOpen(EmailID, BlastID, Request.UserHostAddress + " | " + Request.UserAgent, User);
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertOpen(EmailID, BlastID, Request.UserHostAddress + " | " + Request.UserAgent, User);
                    }
                    return send1X1Img();
                }
                //WGH: 10/31/2014 - Removing old logging
                //else
                //{
                //    try
                //    {
                //        KM.Common.Entity.ApplicationLog.LogNonCriticalError("EmailID and/or BlastID are empty or are invalid", "Open.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Open.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
            }
            return send1X1Img();
        }


        //
        // HEAD: /Opens/Index/
        [HttpHead]
        public ActionResult Index(string query, bool amHead = true)
        {
            return send1X1Img();
        }

        private void GetQueryParams(string dirtyQuery)
        {
            if (!string.IsNullOrEmpty(dirtyQuery))
            {
                string unencrypted = "";
                string[] matches = null;
                string b = "0";
                string e = "0";
                try
                {


                    KM.Common.Entity.Encryption ecLink = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                    unencrypted = KM.Common.Encryption.Base64Decrypt(System.Web.HttpUtility.UrlDecode(dirtyQuery), ecLink);
                }
                catch{
                    unencrypted = dirtyQuery.StartsWith("?") ? dirtyQuery.Substring(1) : dirtyQuery;
                }

                try{
                    matches = unencrypted.Split('&');
                foreach (string match in matches)
                {
                    string[] kvp = match.Split('=');
                    string name = kvp[0];
                    string value = kvp[1];

                    switch (name)
                {
                        case "b":
                            b = value;
                            break;
                        case "e":
                            e = value;
                            break;
                }
                }


                if (b.IndexOf(",") > 0)
                {
                    BlastID = Convert.ToInt32(b.Substring(0, b.IndexOf(",")));
                }
                else
                {
                    BlastID = Convert.ToInt32(b.ToString());
                }

                EmailID = Convert.ToInt32(e);
            }
                catch { }
            }
        }

        private int GetRefBlastID()
        {
            RefBlastID = BlastID;
            ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(BlastID, false);
            try
            {
                if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                {
                    RefBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, EmailID, blast.CustomerID.Value, blast.BlastType);
                }
            }
            catch (Exception) { }
            return RefBlastID;
        }

        [HttpGet]
        public FileContentResult send1X1Img()
        {

            string clearGIF1X1 = "R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==";

            FileContentResult fsr = new FileContentResult(Convert.FromBase64String(clearGIF1X1), "image/gif");
            return fsr;
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            try
            {
                adminEmailVariables.AppendLine("<BR><BR>BlastID: " + BlastID.ToString());
                adminEmailVariables.AppendLine("<BR>EmailID: " + EmailID.ToString());
                adminEmailVariables.AppendLine("<BR>Page URL: " + HttpContext.Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
                adminEmailVariables.AppendLine("<BR>SPY Info:&nbsp;[" + Request.UserHostAddress + "] / [" + Request.UserAgent + "]");
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }
    }
}
