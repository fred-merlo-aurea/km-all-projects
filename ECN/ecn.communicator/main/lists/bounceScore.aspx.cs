using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace ecn.communicator.listsmanager 
{
	public partial class bounceScore : ECN_Framework.WebPageHelper
    {

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
            Master.SubMenu = "bounce Score";
            Master.Heading = "Bounce Score Manager";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

			if(!(Page.IsPostBack))
            {
                LoadDRs(Master.UserSession.CurrentUser.CustomerID);
			}
		}

		private void LoadDRs(int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.Group> groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser);

            GroupID.DataSource = groupList;
            GroupID.DataValueField = "GroupName";
            GroupID.DataTextField = "GroupID";
			GroupID.DataBind();
			GroupID.Items.Insert(0,new ListItem("--------------------------", ""));
			GroupID.Items.Insert(0,new ListItem("** ALL THE GROUPS **", "*"));
			GroupID.Items.Insert(0,new ListItem("-- select a Group --", ""));		
			for(int i=-1; i<11; i++){
				BounceScoreDR.Items.Insert((i+1), i.ToString());
			}

			for(int i=15; i<55; i = i+5){
				BounceScoreDR.Items.Add(i.ToString());
			}
			for(int i=60; i<105; i = i+10){
				BounceScoreDR.Items.Add(i.ToString());
			}
		}
        
		protected void ShowCountResultsBtn_Click(object sender, System.EventArgs e) 
        {
			string bounceCondition	= BounceScoreConditionDR.SelectedValue.ToString();
			string bounceScore		= BounceScoreDR.SelectedValue.ToString();
			string groupID				= GroupID.SelectedValue.ToString();
            DataTable emailGroupList = new DataTable();
			if(groupID.Equals("*"))
            {
                emailGroupList = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByBounceScore(Master.UserSession.CurrentUser.CustomerID, null, Convert.ToInt32(bounceScore), bounceCondition);
			}
            else if(!(groupID.Length == 0))
            {
                emailGroupList = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByBounceScore(Master.UserSession.CurrentUser.CustomerID, Convert.ToInt32(groupID), Convert.ToInt32(bounceScore), bounceCondition);
            }
            BounceScoreGrid.DataSource = emailGroupList;

            BounceScoreGrid.DataBind();

            BounceScorePager.RecordCount = emailGroupList.Rows.Count;		
		}
         
	}
}
