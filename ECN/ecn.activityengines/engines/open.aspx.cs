using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace ecn.activityengines
{

    public partial class open : System.Web.UI.Page
    {
        int EmailID = 0;
        int BlastID = 0;
        int RefBlastID = 0;
        KMPlatform.Entity.User User = null;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
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
                if (BlastID > 0 && EmailID > 0)
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
                    send1X1Img();
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

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            try
            {
                adminEmailVariables.AppendLine("<BR><BR>BlastID: " + BlastID.ToString());
                adminEmailVariables.AppendLine("<BR>EmailID: " + EmailID.ToString());
                adminEmailVariables.AppendLine("<BR>Page URL: " + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
                adminEmailVariables.AppendLine("<BR>SPY Info:&nbsp;[" + Request.UserHostAddress + "] / [" + Request.UserAgent + "]");
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
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

        #region Draw 1x1 px image
        public void send1X1Img()
        {
            //if no height, width, src then output "error"
            Bitmap imgOutput = new Bitmap(1, 1, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(imgOutput); //create a new graphic object from the above bmp
            g.Clear(Color.White);
            //g.Clear(Color.Transparent); //blank the image
            //set the contenttype
            Response.ContentType = "image/gif";
            //send the resized image to the viewer
            imgOutput.Save(Response.OutputStream, ImageFormat.Gif); //output to the user
            //tidy up
            g.Dispose();
            imgOutput.Dispose();
        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
        }
        #endregion
    }
}
