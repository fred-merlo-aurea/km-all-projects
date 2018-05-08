using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace ecn.activityengines 
{
    public partial class read : System.Web.UI.Page 
    {
        int EmailID = 0;
        int BlastID = 0;
        int RefBlastID = 0;
        KMPlatform.Entity.User User = null;
        protected void Page_Load( object sender, EventArgs e ) 
        {

            System.Threading.Thread.Sleep(7000);
            try
            {
                if (Response.IsClientConnected)
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
                        if (ECN_Framework_BusinessLayer.Communicator.EmailGroup.ValidForTracking(RefBlastID, EmailID))
                        {
                            ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertRead(EmailID, BlastID, Request.UserHostAddress + " | " + Request.UserAgent, User);
                        }
                        send1X1Img();
                    }
                    else
                    {
                        try
                        {
                            KM.Common.Entity.ApplicationLog.LogNonCriticalError("EmailID and/or BlastID are empty or are invalid", "Read.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                        }
                        catch (Exception)
                        {
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Read.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
            }
        }

        private int GetRefBlastID()
        {
            RefBlastID = BlastID;
            ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID(BlastID, User, false);
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
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<BR><BR>Blast ID: " + BlastID);
                adminEmailVariables.AppendLine("<BR>Email ID: " + EmailID);
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        #region Draw 1x1 px image
        public void send1X1Img() {
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
    }
}
