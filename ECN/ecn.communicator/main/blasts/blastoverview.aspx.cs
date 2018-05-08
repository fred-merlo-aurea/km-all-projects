using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ecn.communicator.blastsmanager 
{
	public partial class blastoverview : ECN_Framework.WebPageHelper
    {
       
		protected void Page_Load(object sender, System.EventArgs e) 
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.blastpriv))
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View)
                )
            {
				int requestBlastID = getBlastID();
				if (requestBlastID>0) 
                	loadLogGrid(requestBlastID);				
			}
            else
            {
				Response.Redirect("../default.aspx");			
			}
		}

		private int getBlastID() 
        {
            if (Request.QueryString["BlastID"].ToString() != null)
            {
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            else
                return 0;
		}

		private void loadLogGrid(int BlastID)
        {
            ECN_Framework_Entities.Communicator.Blast blast=
            ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(BlastID, Master.UserSession.CurrentUser, false);
            string BlastLog = blast.BlastLog;

			DataTable dt = new DataTable();
			DataRow dr;
			DataColumn dcEmailID = new DataColumn("EmailID",typeof(string));
			DataColumn dcEmailAddress = new DataColumn("EmailAddress",typeof(string));
			DataColumn dcSuccess = new DataColumn("Success",typeof(string));
			DataColumn dcSendTime = new DataColumn("SendTime",typeof(string));
			dt.Columns.Add(dcEmailID);
			dt.Columns.Add(dcEmailAddress);
			dt.Columns.Add(dcSuccess);
			dt.Columns.Add(dcSendTime);

			Regex cr = new Regex("\n");
			char[] comma={','};
			foreach (string myrows in cr.Split(BlastLog))
            {
				string[] splituprow = myrows.Split(comma);
				if (splituprow[0]!=""){
					dr=dt.NewRow();
					dr["EmailID"]=splituprow[0];
					dr["EmailAddress"]=splituprow[1];
					dr["Success"]=splituprow[2];
					dr["SendTime"]=splituprow[3];
					dt.Rows.Add(dr);
				}
			}
			LogGrid.DataSource=dt.DefaultView;
			LogGrid.DataBind();
		}
      
	}
}
