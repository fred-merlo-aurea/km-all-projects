using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.contentmanager 
{
	
	public partial class preview : System.Web.UI.Page 
    {
		
		protected void Page_Load(object sender, System.EventArgs e) 
        {
            if (ECN_Framework_BusinessLayer.Communicator.Content.HasPermission(KMPlatform.Enums.Access.View, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            {
				int requestLayoutID = getLayoutID();	
				string requestFormat = getFormat();
				if (requestLayoutID>0)
                {        
            
                    if (requestFormat=="html")
                    {
                        string body = ECN_Framework_BusinessLayer.Communicator.Layout.GetPreview(requestLayoutID, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        //body = DoRSSFeed(body);
                        ECN_Framework_BusinessLayer.Communicator.ContentReplacement.RSSFeed.Replace(ref body, 
                            ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, 
                            false, null); // content is HTML, do NOT search for cached RSS info by BlastID
                        body = RegexUtilities.GetCleanUrlContent(body);
                        LabelPreview.Text = body;
                    }
                    else
                    {
                        LabelPreview.Text = "<form><textarea cols=80 rows=25 READONLY>" + ECN_Framework_BusinessLayer.Communicator.Layout.GetPreview(requestLayoutID, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.TEXT, false, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser)+"</textarea></form>";
					}
				} 
                else 
                {
					LabelPreview.Text="No LayoutID Specified";
				}
			}
            else
            {
				Response.Redirect("../default.aspx");				
			}
		}

        

		private int getLayoutID() {
			int theLayoutID = 0;
            if (Request.QueryString["LayoutID"] != null)
            {
                theLayoutID = Convert.ToInt32(Request.QueryString["LayoutID"].ToString());
            }
			return theLayoutID;
		}

		private string getFormat() {
			string theFormat = "html";
			if (Request.QueryString["Format"] != null)
            {
				theFormat = Request.QueryString["Format"].ToString();
			}
			return theFormat;
		}
	}
}
