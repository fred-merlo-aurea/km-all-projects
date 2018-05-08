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
using System.IO;
using System.Configuration;

namespace ecn.communicator.main.blasts
{
    public partial class DCReports : ECN_Framework.WebPageHelper
    {
        /*
        private int getBlastID()
        {
            if (Request.QueryString["BlastID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            else
                return 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "Dynamic Content Report";
            Master.HelpContent = "";
            Master.HelpTitle = "Dynamic Content Report";	

            if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.blastpriv))
            {
                int requestBlastID = getBlastID();
                if (requestBlastID > 0)
                {                
                    if (!Page.IsPostBack)
                        loaddcGrid(requestBlastID);
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        private void loaddcGrid(int BlastID)
        {           
            DataTable dt  = DataFunctions.GetDataTable("select contentID,contentTitle, count(emailID) as sends from BlastDynamicContents bdc left outer join content c on (c.contentID = bdc.slot1 or c.contentID = bdc.slot2 or c.contentID = bdc.slot3 or c.contentID = bdc.slot4 or c.contentID = bdc.slot5 or c.contentID = bdc.slot6 or c.contentID = bdc.slot7 or c.contentID = bdc.slot8 or c.contentID = bdc.slot9) where bdc.blastID = " + BlastID + " group by contentID,contentTitle");

            dcGrid.DataSource = dt;
            dcGrid.DataBind();
        }
         */
    }
}
