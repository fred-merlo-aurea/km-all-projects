using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.common.classes;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.activityengines.includes {

    public partial class emailProfile_EmailActivity : EmailProfileBaseControl {

        private string _emailID = string.Empty;
        DataTable campaignActivityDT = null;

        protected override Label lblResultMessage
        {
            get
            {
                return this.messageLabel;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            _emailID = GetFromQueryString("eID", "emailID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
            loadCampaignActivityByAction("");
		}

        #region Load Campaign Activity Grid by Action (OPEN / SEND / CLICK etc.,)
        private void loadCampaignActivityByAction( string activityType ) {
            campaignActivityGrid.Dispose();
            if(activityType.Length == 0) {
                activityType = "send";
            }
            loadCampaignActivityDT(activityType);

            if(campaignActivityDT.Rows.Count > 0) {
                campaignActivityGrid.Visible = true;
                messageLabel.Visible = false;
                campaignActivityGrid.DataSource = campaignActivityDT.DefaultView;
                campaignActivityGrid.DataBind();
            } else {
                campaignActivityGrid.Visible = false;
                messageLabel.Visible = true;
                messageLabel.Text = "<div style=\"PADDING: 12px\"><img src=\"http://images.ecn5.com/images/small-alertIcon.gif\" />&nbsp;<sup>No "+activityType+" activity available at this time</sup></div>";
            }

            resetStyles();

            if(activityType.Equals("send")) {
                campaignActivityGrid.Columns[2].Visible = false;
                btnEmailsSent.CssClass = "selected";
            } else if(activityType.Equals("open")) {
                campaignActivityGrid.Columns[2].Visible = false;
                btnEmailsOpened.CssClass = "selected";
            } else if(activityType.Equals("click")) {
                campaignActivityGrid.Columns[2].Visible = true;
                btnEmailsClicked.CssClass = "selected";
            } else if(activityType.Equals("bounce")) {
                campaignActivityGrid.Columns[2].Visible = true;
                btnEmailsBunces.CssClass = "selected";
            }
        }
        #endregion

        #region Load the Campaign Activity Data Table
        private void loadCampaignActivityDT( string activityType ) {
            if(activityType == "send") { activityType = "send','testsend"; } else { }
            campaignActivityDT = new DataTable();
            string campaignActivitySQL = " SELECT COUNT(ActionTypeCode) AS 'ActionCount', ActionTypeCode, eal.BlastID, b.EmailSubject AS 'EmailSubject' , b.SendTime AS 'BlastSendTime'," +
                        " (CASE WHEN ActionTypeCode = 'click' THEN ActionValue ELSE '' END) as 'ActionValue' " +
                        " FROM Blast b " +
                        " JOIN EmailActivityLog eal ON b.blastID = eal.blastID " +
                        " JOIN Emails e ON eal.emailID = e.emailID " +
                        " WHERE b.StatusCode <> 'Deleted' and " +
                        " eal.EMailID=" + _emailID + " AND ActionTypeCode IN ('" + activityType + "')" +
                        " GROUP BY eal.EmailID, eal.BlastID, ActionTypeCode, " +
                        " (CASE WHEN ActionTypeCode = 'click' THEN ActionValue ELSE '' END), " +
                        " b.EmailSubject, b.sendTime " +
                        " ORDER BY eal.BlastID DESC, ActionTypeCode";

            campaignActivityDT = DataFunctions.GetDataTable(campaignActivitySQL);
        }
        #endregion

        #region reset Styles on all the LinkButtons
        protected void resetStyles() {
            btnEmailsSent.CssClass = "";
            btnEmailsOpened.CssClass = "";
            btnEmailsClicked.CssClass = "";
            btnEmailsBunces.CssClass = "";
        }
        #endregion

        #region Link Button Click Events
        protected void btnEmailsSent_Click( object sender, EventArgs e ) {
            loadCampaignActivityByAction("send");
        }

        protected void btnEmailsOpened_Click( object sender, EventArgs e ) {
            loadCampaignActivityByAction("open");
        }

        protected void btnEmailsClicked_Click( object sender, EventArgs e ) {
            loadCampaignActivityByAction("click");
        }

        protected void btnEmailsBounced_Click( object sender, EventArgs e ) {
            loadCampaignActivityByAction("bounce");
        }
        #endregion


        /*private void LoadEmailActivityClicks(string eID){
			string emailClickssActivity_sql =	"  SELECT "+
			" (SELECT COUNT(ActionTypeCode) FROM EmailActivityLog WHERE blastID = eal.blastID AND emailID = eal.EMailID AND ActionTypeCode='click' AND "+
			"	ActionValue = eal.ActionValue) AS clicks, "+
			" eal.BlastID, eal.ActionValue, b.EmailSubject, eal.ActionDate "+
			" FROM Blasts b JOIN EmailActivityLog eal ON b.blastID = eal.blastID "+
			" WHERE "+
			" eal.EMailID="+eID+" AND ActionTypeCode='click' "+
			" GROUP BY eal.EMailID, eal.BlastID, eal.ActionValue, b.EmailSubject, eal.actiondate ORDER BY eal.actiondate ";

			DataTable dt = DataFunctions.GetDataTable(emailClickssActivity_sql);
			if(dt.Rows.Count > 0){
				//ClickssEmailActivityGrid.DataSource=dt.DefaultView;
				//ClickssEmailActivityGrid.DataBind();
				DataRow newDR;
				DataTable newDT = new DataTable();
				newDT.Columns.Add(new DataColumn("EmailSubject"));
				newDT.Columns.Add(new DataColumn("ActionValue"));
				newDT.Columns.Add(new DataColumn("ActionDate"));
				newDT.Columns.Add(new DataColumn("Clicks"));
				foreach ( DataRow dr in dt.Rows ) {
					string clickCount	= dr["clicks"].ToString();
					string fullLink		= dr["ActionValue"].ToString();
					string smallLink		= fullLink;
					try{
						smallLink = fullLink.Substring(0,50)+"...";
					}catch{
						//ignore
					}
					string linkORalias	= "";
				
					newDR			= newDT.NewRow();
					newDR[0]		= dr["EmailSubject"].ToString();
					string alias		= getLinkAlias(Convert.ToInt32(dr["BlastID"].ToString()), fullLink);
					if(alias.Length > 0){
						linkORalias = alias;
					}else {
						linkORalias = smallLink;
					}
					newDR[1]		= "<a href='"+fullLink.ToString()+"' target='_blank'>"+linkORalias.ToString()+"</a>";
					newDR[2]		= dr["ActionDate"].ToString();
					newDR[3]		= dr["clicks"].ToString();

					newDT.Rows.Add(newDR);
				}
				ClickssEmailActivityGrid.DataSource =new DataView(newDT);
				ClickssEmailActivityGrid.DataBind();

			}else{
				ClickssEmailActivityGrid.Visible = false;
			}
		}
         */

		private string getLinkAlias(int BlastID, String Link) {
			string sqlquery=	" SELECT Alias FROM " +
				" Blast b, Layout l, Content c, linkAlias la "+
				" WHERE b.StatusCode <> 'Deleted' and l.IsDeleted = 0 and c.IsDeleted = 0 and la.IsDeleted = 0 and "+ 
				" b.blastID = "+BlastID+" AND b.layoutID = l.layoutID AND "+ 
				" (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR "+
				" l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR "+
				" l.ContentSlot9 = c.contentID) AND "+
				" la.ContentID = c.ContentID AND la.Link = '"+Link+"'";
			string alias = "";
			try{
				alias = DataFunctions.ExecuteScalar(sqlquery).ToString();
			}catch(Exception ){
				alias = "";
			}

			return alias;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
