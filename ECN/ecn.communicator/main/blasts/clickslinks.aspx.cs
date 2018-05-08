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

namespace ecn.communicator.blastsmanager
{
	public partial class clicks_links : ECN_Framework.WebPageHelper
    {
        int _pagerCurrentPage = 1;
		public int pagerCurrentPage
        {
			set {_pagerCurrentPage = value;}
			get {return _pagerCurrentPage - 1;}
		}
       
		protected void Page_Load(object sender, System.EventArgs e) 
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "Blast Manager";
            Master.HelpTitle = "<p><b>Click-Through</b><br />Lists all recepients who clicked on the URL links in your email Blast<br />Displays the time clicked, the URL link clicked.<br />Click on the email address to view the profile of that email address.";	

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "blastpriv") || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "viewreport") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View))
            {
                if (getBlastID() > 0 || getCampaignItemID() > 0)
                {
                    if (!(Page.IsPostBack)) 
                    {
                        loadClicksGrid(HttpUtility.UrlDecode(getLink().Trim()));
                    }
                }
			}
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();		
			}
		}

        public int getBlastID() 
        {
            try { return Convert.ToInt32(Request.QueryString["BlastID"].ToString()); } catch { return 0; }
        }

        public int getCampaignItemID() 
        {
            try { return Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString()); }
            catch (Exception E) { return 0; }
        }

        private string getLink()
        {
            try { return Request.RawUrl.Trim().ToString().Substring(Request.RawUrl.Trim().IndexOf("link=") + 5); } catch (Exception E) { return ""; }
		}

        public string getUDFName()
        {
            try
            {
                return Request.QueryString["UDFName"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string getUDFData()
        {
            try
            {
                return Request.QueryString["UDFdata"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }


		private void loadClicksGrid(string link) 
        {
            DataSet ds = new DataSet();
            if (getBlastID() > 0)
            {
                ds = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastGroupClicksData(null, getBlastID(),  "", "", "topclickslink", link, 0, 0, getUDFName(), getUDFData());

            }
            else
            {
                ds = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastGroupClicksData(getCampaignItemID(), null, "", "", "topclickslink", link, 0, 0, getUDFName(), getUDFData());
            }
            DataTable dt = ds.Tables[0];

            DataTable newDT = new DataTable();
            newDT.Columns.Add("Click Time");
            newDT.Columns.Add("EmailAddress");
            newDT.Columns.Add("First Name");
            newDT.Columns.Add("Last Name");
            newDT.Columns.Add("Phone #");
            newDT.Columns.Add("Company");

            DataRow newDR;

            foreach (DataRow dr in dt.Rows)
            {
                newDR = newDT.NewRow();
                newDR[0] = dr["ActionDate"].ToString();
                newDR[1] = "<a href='../lists/emaileditor.aspx?" + dr["URL"].ToString() + "'>" + dr["EmailAddress"].ToString() + "</a>";
                newDR[2] = dr["FirstName"].ToString();
                newDR[3] = dr["LastName"].ToString();
                newDR[4] = dr["Phone#"].ToString();
                newDR[5] = dr["Company"].ToString();
                newDT.Rows.Add(newDR);
            }
            ClicksGrid.DataSource = new DataView(newDT);
            ClicksGrid.DataBind();
            ClicksPager.RecordCount = newDT.Rows.Count;

            string alias = "";
            if (getBlastID() > 0)
            {
                alias = getLinkAlias(getBlastID(), link);
            }
            string linkText = "<b>Click-Through By Link / Alias: </b><font color='#FF0000'><i>";
            if (alias.Length > 0)
            {
                LinkLabel.Text = linkText + alias + "</font></i>";
            }
            else
            {
                LinkLabel.Text = linkText + link + "</font></i>";
            }
		}

        protected void ClicksPager_IndexChanged(object sender, EventArgs e) 
        {
            loadClicksGrid(HttpUtility.UrlDecode(getLink()));
        }

        private string getLinkAlias(int BlastID, string link) 
        { 
            ECN_Framework_Entities.Communicator.LinkAlias linkAlias=
            ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByBlastLink(BlastID, link, Master.UserSession.CurrentUser, false);
            if (linkAlias != null)
                return linkAlias.Alias;
            else
                return string.Empty;
		}
         
	}
}
