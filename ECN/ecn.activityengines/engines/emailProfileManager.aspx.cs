using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Entities;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.activityengines {
	
	
	
	public partial class emailProfileManager : System.Web.UI.Page {

		protected string  EmailAdd="",  Action="", panelToShow="";
        protected int EmailID = -1,GroupID=-1, CustomerID = -1;
        private KMPlatform.Entity.User user;
		protected void Page_Load(object sender, System.EventArgs e) {
			MessageLabel.Visible = false;
			EmailID			= getEmailID();
			EmailAdd		= getEmailAddress();
			GroupID		= getGroupID();
			Action			= getAction();
			panelToShow = getPanelToShow();
            CustomerID = getCustomerID();
            user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
            ECN_Framework_Entities.Communicator.Email emailProfile = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(Convert.ToInt32(EmailID));
			
			string emailProfileFullName = emailProfile.FirstName +" "+emailProfile.LastName;
			EmailProfileNameLabel.Text = emailProfileFullName;
		
			LoadManagerPanel();
			EmailProfile_Control_Panel.Controls.Clear();

			if(panelToShow.Equals("emailprofile")){
				Control emailProfileControl = LoadControl("../includes/emailProfile_Base.ascx");
				EmailProfile_Control_Panel.Controls.Add(emailProfileControl);
				EmailProfile_Control_Panel.Visible=true;
			}else if(panelToShow.Equals("UDF")){
				Control emailProfileControl = LoadControl("../includes/emailProfile_UDF.ascx");
				EmailProfile_Control_Panel.Controls.Add(emailProfileControl);
				EmailProfile_Control_Panel.Visible=true;
			}else if(panelToShow.Equals("UDFHist")){
				Control emailProfileControl = LoadControl("../includes/emailProfile_UDFHistory.ascx");
				EmailProfile_Control_Panel.Controls.Add(emailProfileControl);
				EmailProfile_Control_Panel.Visible=true;
			}else if(panelToShow.Equals("survey")){
				Control emailProfileControl = LoadControl("../includes/emailProfile_Survey.ascx");
				EmailProfile_Control_Panel.Controls.Add(emailProfileControl);
				EmailProfile_Control_Panel.Visible=true;
			}else if(panelToShow.Equals("emailActivity")){
				Control emailProfileControl = LoadControl("../includes/emailProfile_EmailActivity.ascx");
				EmailProfile_Control_Panel.Controls.Add(emailProfileControl);
				EmailProfile_Control_Panel.Visible=true;
			}
		}

		private void LoadManagerPanel(){
            ECN_Framework_Entities.Communicator.Group thisGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(GroupID);
			
			string managerMenu = "<table border=0 width=100% cellpadding=4>";
			//Email Profile:
			managerMenu += "<tr><td width=50></td><td bgcolor=#eeeeee><a href='emailProfileManager.aspx?panel=emailprofile&action="+Action+"&ead="+EmailAdd+"&eid="+EmailID+"&gid="+GroupID+"&cid=" + getCustomerID()+ "'><font face='verdana' size=2><b>Customer Profile:</b></font></a>&nbsp;<font face='verdana' size=1>[Name, Address, Phone, etc.,]</font></td>";

			//Stand alone UDF's:
			if(hasUDFinGroup(thisGroup)){
				managerMenu += "<tr><td width=50></td><td class=tableHeader bgcolor=#eeeeee><a href='emailProfileManager.aspx?panel=UDF&action="+Action+"&ead="+EmailAdd+"&eid="+EmailID+"&gid="+GroupID+"&cid=" + getCustomerID()+ "'><font face='verdana' size=2>Additional Profile Information:</font></a></td>";
			}

			//UDF HistoryItems:
			if(hasUDFHistoryinGroup(thisGroup)){
				managerMenu += "<tr><td width=50></td><td  class=tableHeader bgcolor=#eeeeee>Transaction History:";
				managerMenu += "<table width=100%>";
                List<ECN_Framework_Entities.Communicator.DataFieldSets> listDFS = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupID(GroupID);

				foreach ( ECN_Framework_Entities.Communicator.DataFieldSets dfs in listDFS) {
                    managerMenu += "<TR><TD width=10></TD><TD><a href='emailProfileManager.aspx?panel=UDFHist&action=" + Action + "&ead=" + EmailAdd + "&eid=" + EmailID + "&gid=" + GroupID + "&dfsID=" + dfs.DataFieldSetID.ToString() + "&cid=" + getCustomerID() + "'>" + dfs.Name.ToString() + "</a></TD></tr>";
				}
				managerMenu += "</table>";
			}

			//Survey UDF:
			if(hasSurveyinGroup(thisGroup)){
				string collectordb=ConfigurationManager.AppSettings["collectordb"];
				managerMenu += "<tr><td width=50></td><td  class=tableHeader bgcolor=#eeeeee>Survey Responses:";
				managerMenu += "<table width=100%>";

                List<ECN_Framework_Entities.Collector.Survey> listSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetByGroupID(GroupID, thisGroup.CustomerID);

				foreach ( ECN_Framework_Entities.Collector.Survey s in listSurvey) {
					if(hasAnswersInSurvey(s.SurveyID, EmailID)){
                        managerMenu += "<TR><TD width=10></TD><TD><a href='emailProfileManager.aspx?panel=survey&action=" + Action + "&ead=" + EmailAdd + "&eid=" + EmailID + "&gid=" + GroupID + "&surveyID=" + s.SurveyID.ToString() + "&cid=" + getCustomerID() + "'>" + s.SurveyTitle.ToString() + "</a></TD></tr>";
					}
				}
				managerMenu += "</table>";
			}
			
			//Email Activity:
			managerMenu += "<tr><td width=50></td><td class=tableHeader bgcolor=#eeeeee>Email the Customer has viewed:";
			managerMenu += "<table width=100%>";
            managerMenu += "<TR><TD width=10></TD><TD><a href='emailProfileManager.aspx?panel=emailActivity&eactivity=opens&action=" + Action + "&ead=" + EmailAdd + "&eid=" + EmailID + "&gid=" + GroupID + "&cid=" + getCustomerID() + "'>Emails the customer opened</a></td></tr>";
            managerMenu += "<TR><TD width=10></TD><TD><a href='emailProfileManager.aspx?panel=emailActivity&eactivity=clicks&action=" + Action + "&ead=" + EmailAdd + "&eid=" + EmailID + "&gid=" + GroupID + "&cid=" + getCustomerID() + "'>Web links the customer clicked on</a></td></tr>";
			managerMenu += "</table>";

			managerMenu += "</td></tr></table>";
			EmailProfileManager_Menu_Label.Text = managerMenu;
		}

		#region getEmailID, getEmailAddress, getGroupID, getAction, getPanelToShow
		private int getEmailID() {
			int theEmailID = -1;
			try {
				theEmailID	= Convert.ToInt32(Request.QueryString["eID"].ToString());
			}catch{
				MessageLabel.Visible = true;
				MessageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
			}
			return theEmailID;
		}

		private string getEmailAddress() {
			string theEmailAddress = "";
			try {
				theEmailAddress	= Request.QueryString["eAD"].ToString();
			}catch{
				MessageLabel.Visible = true;
				MessageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
			}
			return theEmailAddress;
		}

		private int getGroupID() {
			int theGroupID = -1;
			try {
				theGroupID	= Convert.ToInt32(Request.QueryString["gID"].ToString());
			}catch {
				MessageLabel.Visible = true;
				MessageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
			}
			return theGroupID;
		}

        private int getCustomerID()
        {
            int theCustomerID = -1;
			try {
				theCustomerID	= Convert.ToInt32(Request.QueryString["cid"].ToString());
			}catch {
				MessageLabel.Visible = true;
				MessageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
			}
			return theCustomerID;
        }
		private string getAction() {
			string theAction = "";
			try {
				theAction	= Request.QueryString["action"].ToString();
			}catch{
				theAction = "view";
			}
			return theAction;
		}
		private string getPanelToShow() {
			string thePanel = "";
			try {
				thePanel	= Request.QueryString["panel"].ToString();
			}catch {
			}
			return thePanel;
		}

		#endregion

		#region Other Checks 
		private bool hasUDFinGroup(ECN_Framework_Entities.Communicator.Group thisGroup){
			bool result = false;
			try{
                List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(thisGroup.GroupID);
                if (listUDFs.Count > 0)
                    result = true;
                else
                    result = false;
				
			}catch{
				//doesn't have UDF's		
			}
			return result;
		}

        private bool hasUDFHistoryinGroup(ECN_Framework_Entities.Communicator.Group thisGroup)
        {
			bool result = false;
			try{
                List<ECN_Framework_Entities.Communicator.DataFieldSets> listDFS = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupID(thisGroup.GroupID);
                if (listDFS.Count > 0)
                    result = true;
                else
                    result = false;
				
			}catch{
				//doesn't have UDF's		
			}
			return result;
		}

        private bool hasSurveyinGroup(ECN_Framework_Entities.Communicator.Group thisGroup)
        {
			bool result = false;
			try{

                result = ECN_Framework_BusinessLayer.Collector.Survey.HasSurvey(thisGroup.GroupID);
			}catch{
				//doesn't have UDF's		
			}
			return result;
		}

		private bool hasAnswersInSurvey(int surveyID, int emailID){
			bool result = false;
			try{
                result = ECN_Framework_BusinessLayer.Collector.Survey.HasResponses(emailID, surveyID);
				
			}catch{
				//doesn't have UDF's		
			}
			
			return result;
		}

		#endregion


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
