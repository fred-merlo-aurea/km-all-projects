using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace ecn.communicator.blastsmanager
{
    public partial class logs_main : ECN_Framework.WebPageHelper
    {   
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View) 
                )
            {
                if (getBlastID() > 0)
                {
                    loadLogGrid(getBlastID());
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        private int getBlastID()
        {
            int theBlastID = 0;
            try
            {
                theBlastID = Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theBlastID;
        }

        private void loadLogGrid(int blastID)
        {
            List<ECN_Framework_Entities.Activity.BlastActivitySends> bas = ECN_Framework_BusinessLayer.Activity.BlastActivitySends.GetByBlastID(blastID);
            var result = (from src in bas
                         select new
                         {
                             EmailID=src.EmailID,
                             SendTime=src.SendTime,
                             EmailAddress= src.EmailAddress,
                             Success="Y"
                         }).ToList();
            LogGrid.DataSource = result;
            LogGrid.DataBind();
            EmailsPager.RecordCount = result.Count;
        }
    }
}
