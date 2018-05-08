using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Configuration;

namespace ecn.activityengines 
{	
	
	public partial class conversion : System.Web.UI.Page
    {
        private KMPlatform.Entity.User User;

		protected void Page_Load(object sender, System.EventArgs e) 
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
			int blastID=getBlastID();
            int emailID = getEmailID();
            if (blastID > 0 && emailID > 0)
            {
                ECN_Framework_Entities.Communicator.SmartFormTracking sft = new ECN_Framework_Entities.Communicator.SmartFormTracking();
                sft.BlastID = blastID;
                sft.ReferringUrl = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();
                sft.ActivityDate = DateTime.Now;
                ECN_Framework_BusinessLayer.Communicator.SmartFormTracking.Insert(sft);

                int refBlastID = GetRefBlastID(blastID, emailID);
                decimal total = getRevenueTotal();
                if (ECN_Framework_BusinessLayer.Communicator.EmailGroup.ValidForTracking(refBlastID, emailID))
                {
                    try
                    {
                        TrackData(blastID, emailID);
                        if (total > Convert.ToDecimal(0.0))
                        {
                            PostRevenueData(blastID, emailID, total);
                        }
                    }
                    catch (TimeoutException te)
                    {
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError(te, "conversion.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                    }
                    catch (Exception ex)
                    {
                        KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "conversion.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                    }
                }

                sendBlank();
            }
		}

		#region Get Request Variables

		private int getBlastID() {
			int theBlastID = 0;
			try {
				theBlastID = Convert.ToInt32(Request.QueryString["b"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theBlastID;
		}

		private int getEmailID() {
			int theEmailID = 0;
			try {
				theEmailID = Convert.ToInt32(Request.QueryString["e"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theEmailID;
		}

		private string getLink() {
			int indexOfl = 0;
			String theLink = "";
			try {
				theLink	= Request.Url.ToString();
				indexOfl	= theLink.IndexOf("oLink=");
				int length	= theLink.Length;
				theLink	= theLink.Substring(indexOfl+6, ((theLink.Length -1) - indexOfl)-5 );
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theLink;
		}

		private decimal getRevenueTotal() 
        {
            decimal thetotal = Convert.ToDecimal("0.0");
			try 
            {
				thetotal = Convert.ToDecimal(Request.QueryString["total"].ToString());
			}
			catch(Exception E) 
            {
				string devnull=E.ToString();
			}
			return thetotal;
		}
		#endregion

		public void sendBlank(){
			//if no height, width, src then output "error"
			Bitmap imgOutput = new Bitmap(1, 1, PixelFormat.Format24bppRgb);
			Graphics g = Graphics.FromImage(imgOutput); //create a new graphic object from the above bmp
			g.Clear(Color.White);
			//g.Clear(Color.Transparent); //blank the image
			//set the contenttype
			Response.ContentType="image/gif";
			//send the resized image to the viewer
			imgOutput.Save(Response.OutputStream, ImageFormat.Gif); //output to the user
			//tidy up
			g.Dispose();
			imgOutput.Dispose();
		}

        private int GetRefBlastID(int blastID, int emailID)
        {
            int refBlastID = blastID;
            ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(blastID, false);
            try
            {
                if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                {
                    refBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(blastID, emailID, blast.CustomerID.Value, blast.BlastType);
                }
            }
            catch (Exception) { }
            return refBlastID;
        }

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();

            try
            {
                adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + getBlastID());
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + getEmailID());
                adminEmailVariables.AppendLine("<br><b>Revenue Total:</b>&nbsp;" + getRevenueTotal());
                adminEmailVariables.AppendLine("<br><b>The Link:</b>&nbsp;" + getLink());
                adminEmailVariables.AppendLine("<br><b>Page URL:</b>&nbsp;" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + Request.RawUrl.ToString());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

		private void TrackData(int theBlastID, int theEmailID) 
        {
            string spyinfo = string.Empty;
            if (Request.UserHostAddress != null)
            {
                spyinfo = Request.UserHostAddress;
            }
            if (Request.UserAgent != null)
            {
                if (spyinfo.Length > 0)
                {
                    spyinfo += " | ";
                }
                spyinfo += Request.UserAgent;
            }
            ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertConversion(theEmailID, theBlastID, getLink().Replace("'", "''").Replace("'", "''"), spyinfo, User);
		}

		private void PostRevenueData(int theBlastID, int theEmailID, decimal total) 
        {
            string spyinfo = string.Empty;
            if (Request.UserHostAddress != null)
            {
                spyinfo = Request.UserHostAddress;
            }
            if (Request.UserAgent != null)
            {
                if (spyinfo.Length > 0)
                {
                    spyinfo += " | ";
                }
                spyinfo += Request.UserAgent;
            }
            if(ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.GetConversionRevenueCount(theEmailID, theBlastID) <= 0)
                ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertConversionRevenue(theEmailID, theBlastID, total.ToString(), User);

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    
		}
		#endregion
	}
}
