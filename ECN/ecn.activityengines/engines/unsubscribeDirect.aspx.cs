using System;
using System.Data;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;

namespace ecn.activityengines 
{
	
	public partial class unsubscribeDirect : System.Web.UI.Page 
    {
		protected void Page_Load(object sender, System.EventArgs e) 
        {
            try
            {
                int emailID = getEmailID();
                int groupID = getGroupID();
                int blastID = getBlastID();
                string redir = getRedirectURL();

                if (emailID == 0 || groupID == 0 || blastID == 0)
                {
                    try
                    {
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("EmailID, GroupID or BlastID are empty or are invalid", "unsubscribeDirect.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));                        
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    UpdateSubscribeStatus(emailID, groupID, blastID);

                    if (redir.ToLower().StartsWith("http://"))
                    {
                        Response.Redirect(redir, true);
                    }
                    else
                    {
                        drawPixelImg();
                    }
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "unsubscribeDirect.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
            }
		}

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();

            try
            {
                adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + getBlastID());
                adminEmailVariables.AppendLine("<br><b>Group ID:</b>&nbsp;" + getGroupID().ToString());
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + getEmailID());
                adminEmailVariables.AppendLine("<br><b>Redirect URL:</b>&nbsp;" + getRedirectURL());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

		#region getters Blast, Group & EmailID
		private int getBlastID() 
        {
			int theBlastID = 0;
			try 
            {
				theBlastID = Convert.ToInt32(Request.QueryString["b"].ToString());
			} 
            catch(Exception E) 
            {
				string devnull=E.ToString();
			}
			return theBlastID;
		}

		private int getGroupID() 
        {
			int theGroupID = 0;
			try 
            {
				theGroupID = Convert.ToInt32(Request.QueryString["g"].ToString());
			} 
            catch(Exception E) 
            {
				string devnull=E.ToString();
			}
			return theGroupID;
		}

		private int getEmailID() 
        {
			int theEmailID = 0;
			try 
            {
				theEmailID = Convert.ToInt32(Request.QueryString["e"].ToString());
			} catch(Exception E) 
            {
				string devnull=E.ToString();
			}
			return theEmailID;
		}

		private string getRedirectURL()
        {
			string theURL = "";
			try 
            {
				theURL = Request.QueryString["redir"].ToString();
			} catch(Exception E) 
            {
				string devnull=E.ToString();
			}
			return theURL;			
		}
		#endregion

		#region UpdateS
		private void UpdateSubscribeStatus(int theEmailID, int theGroupID, int theBlastID)  
        {
            ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertOptOutFeedback(theGroupID, theGroupID.ToString(), theEmailID, "");
		}
		#endregion

		#region Draw 1X1 Pixel Img 
		public void drawPixelImg()
        {
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
		#endregion

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