using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;
using System.Text;

namespace ecn.activityengines.engines
{
    public partial class EmailDirectOpen : System.Web.UI.Page
    {
        int EmailDirectID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Url.Query.ToString().Length > 0)
                {
                    GetValuesFromQuerystring(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
                }
                if(EmailDirectID > 0)
                {
                    ECN_Framework_BusinessLayer.Communicator.EmailDirect.UpdateStatus(EmailDirectID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Opened);
                    send1X1Img();
                }
            }
            catch(Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "EmailDirectOpen.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
            }
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            try
            {
                adminEmailVariables.AppendLine("<BR><BR>EmailDirectID: " + EmailDirectID.ToString());
                adminEmailVariables.AppendLine("<BR>Page URL: " + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
                adminEmailVariables.AppendLine("<BR>SPY Info:&nbsp;[" + Request.UserHostAddress + "] / [" + Request.UserAgent + "]");
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        private void GetValuesFromQuerystring(string queryString)
        {
            try
            {
                ECN_Framework_Common.Objects.QueryString qs = ECN_Framework_Common.Objects.QueryString.GetECNParameters(Server.UrlDecode(QSCleanUP(queryString)));
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == ECN_Framework_Common.Objects.Enums.ParameterTypes.EmailDirectID).ParameterValue, out EmailDirectID);
                
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
    }
}