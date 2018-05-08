using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.common.classes;
using ecn.communicator.Constants;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.listsmanager {
	
	public partial class ReferralProgram : ECN_Framework.WebPageHelper {
		protected System.Web.UI.WebControls.Button OptinHTMLPreview;

		string selectedFields = "";
		protected System.Web.UI.WebControls.TextBox RPFieldSet;
		string optinHTML = "";
		private string jscript = "<!-- Do NOT MODIFY THE SCRIPT BELOW -->"+
			"<!-- REFERRAL PROGRAM WILL NOT WORK PROPERLY IF THE SCRIPT IS MODIFIED -->"+
			"<SCRIPT LANGUAGE='JavaScript' src='" + System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/channels/1/js/referralProgram.js'></ENDSCRIPT>"+
			"<!-- END SCRIPT -->";
		protected System.Web.UI.WebControls.TextBox txtgid;
		protected System.Web.UI.WebControls.TextBox txtcuid;
		protected System.Web.UI.WebControls.TextBox txtchid;

        public int GroupId
        {
            get
            {
                return RequestQueryString(QueryStringKeys.GroupId, default(int));
            }
        }

        public int ReferralProgramId
        {
            get
            {
                return RequestQueryString(QueryStringKeys.ReferralProgramId, default(int));
            }
        }

        public string RequestedAction
        {
            get
            {
                return RequestQueryString(QueryStringKeys.Action, string.Empty);
            }
        }

        public ReferralProgram() {
			Page.Init += new System.EventHandler(Page_Init);
		}
	
		protected void Page_Load(object sender, System.EventArgs e)  {

            Response.Redirect("~/main/lists/default.aspx");

           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icogroups.gif><b>Group Subscribe</b><br />Just copy and paste the code that is in the Text Box in your Newsletter to enable the Subscribe and UnSubscribes from the users who receive your Newsletter.</p><p>Check Boxes agint ";
            Master.HelpTitle = "Groups Manager";	

			RP_SmartFormButton.Enabled = false;		
			DO_SO_SmartFormButton.Enabled = true;

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "grouppriv") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.View))				
            {
				var requestGroupID = GroupId;
				var requestRPID = ReferralProgramId;
				var requestAction = RequestedAction;

				ECN_Framework.Common.SecurityAccess.canI("Groups",requestGroupID.ToString());
				if(requestRPID > 0) {
					if(requestAction.Equals("delete")) {
						string sqldelRP=
							" DELETE FROM ReferralProgram "+
							" WHERE ReferralProgramID = "+requestRPID;
						try {
							DataFunctions.Execute(sqldelRP);
							PopulateDDL();
							LoadEmailFields(requestGroupID, RP_OptinFieldSelection);
						} catch(Exception ex2) {
							PopUp.PopupMsg = "<font color='#FF0000'>Cannot Delete SmartForm.</font> <br> "+ex2.ToString();
							Response.Write("<script>javascript:window.open('../../includes/popup.aspx','Error', 'left=100,top=100,height=300,width=615,resizable=yes,scrollbar=yes,status=no' );</script>");					
						}
					} else {
						if (!(Page.IsPostBack)) {
							RP_panelFCKEditor.Enabled	= true;
							RP_panelFCKEditor.Visible	= true;
							PopulateDDL();
							LoadEmailFields(requestGroupID, RP_OptinFieldSelection);
							LoadSmartFormData(requestRPID);
						}
					}
					LoadSmartFormGrid(requestGroupID);
				} else	if (requestGroupID>0) {
					if (!(Page.IsPostBack)) {
						if(Convert.ToInt32(Master.UserSession.CurrentCustomer.CommunicatorLevel.ToString()) > 1) {
							PopulateDDL();
							LoadEmailFields(requestGroupID, RP_OptinFieldSelection);
							LoadSmartFormGrid(requestGroupID);	
						} else {
							TxtBx_HTMLCode.Enabled		= true;
							TxtBx_HTMLCode.Visible		= true;
							SetHTMLCode(requestGroupID, false, RP_HTMLCode);
						}
					} else {
						LoadSmartFormGrid(requestGroupID);
					}
				} else {
					Response.Redirect("../default.aspx");			
				}
			}
		}

		#region Form Prep
		private void SetHTMLCode(int setGroupID, bool smartForm, CKEditor.NET.CKEditorControl HTMLCode) {
			if(smartForm){
				string sqlQuery =	" SELECT OptinHTML,OptinFields FROM Groups " +
					" WHERE GroupID = "+ setGroupID;
				string DBoptinCde = "", DBoptinFields="";
				DataTable dt = DataFunctions.GetDataTable(sqlQuery);
				foreach ( DataRow dr in dt.Rows ) {
					DBoptinCde		= dr["OptinHTML"].ToString();
					DBoptinFields	= dr["OptinFields"].ToString();												
				}
			
				if(DBoptinCde.ToString().Length == 0 || DBoptinFields.ToString().Length == 0){
					HTMLCode.Text = GetHTMLCode(setGroupID, "DO"); 
				}else{
					optinHTML		= DBoptinCde.ToString();
					selectedFields	= DBoptinFields.ToString();
					HTMLCode.Text = optinHTML.ToString();
				}
			}else{
                string CustomerID = Master.UserSession.CurrentUser.CustomerID.ToString();
				string sqlquery="SELECT GroupID from Groups WHERE GroupID="+setGroupID+" AND CustomerID="+CustomerID;
				string GroupValue=DataFunctions.ExecuteScalar(sqlquery).ToString();
				//--- Still don't know what path to redirect to...
                string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/ReferralProgram.aspx";
				string thecode=
					"<form action="+redirpage+">"+
					"<INPUT id=EmailAddress type=text name=e size=25><br>"+
					"<INPUT id=RadioSub type=radio value=S name=s checked>Subscribe&nbsp;"+
					"<INPUT id=RadioUnSub type=radio value=U name=s>UnSubscribe&nbsp;<br>"+
					"<INPUT id=RadioHTML type=radio value=html name=f checked>HTML&nbsp;"+
					"<INPUT id=RadioText type=radio value=text name=f>Text&nbsp;<br>"+
					"<input type=hidden name=g value='"+GroupValue+"'> <input type=hidden name=c value="+CustomerID+">"+
					"<INPUT id=Submit type=submit value=Submit name=Submit>"+
					"</form>";
				TxtBx_HTMLCode.Text=thecode;
			}
		}

		private void Set_RPHTMLCode(string htmlCode, string selectedFields) {
			if(htmlCode.ToString().Length == 0 || selectedFields.ToString().Length == 0)
				RP_HTMLCode.Text = ""; 
			else{
				this.RP_OptinHTMLSave.Enabled = true;
				StringTokenizer st = new StringTokenizer(selectedFields, '|');
				while(st.HasMoreTokens()){
					string token = st.NextToken();
					for(int i=0; i<RP_OptinFieldSelection.Items.Count; i++){
						if(RP_OptinFieldSelection.Items[i].Text.Equals(token))
							RP_OptinFieldSelection.Items[i].Selected = true;
					}					
				}
				RP_HTMLCode.Text = htmlCode.ToString();
			}
		}

		private void PopulateDDL() 
		{
			string cuID = Request.QueryString["cuID"].ToString();
			string sql = "select LayoutID, LayoutName from Layouts where CustomerID='"+cuID+"'";
			DataTable dt = DataFunctions.GetDataTable(sql);
			int i = 0;
			RefererMsgID.Items.Clear();
			RefereeMsgID.Items.Clear();
			foreach (DataRow dr in dt.Rows) 
			{				
				RefererMsgID.Items.Add(dr["LayoutName"].ToString());
				RefererMsgID.Items.FindByText(dr["LayoutName"].ToString()).Text = dr["LayoutName"].ToString();
				RefererMsgID.Items.FindByText(dr["LayoutName"].ToString()).Value = dr["LayoutID"].ToString();

				RefereeMsgID.Items.Add(dr["LayoutName"].ToString());
				RefereeMsgID.Items.FindByText(dr["LayoutName"].ToString()).Text = dr["LayoutName"].ToString();
				RefereeMsgID.Items.FindByText(dr["LayoutName"].ToString()).Value = dr["LayoutID"].ToString();
				i++;
			}
		}

		private void LoadSmartFormData(int setRPID){
			string sql = "SELECT * FROM ReferralProgram WHERE ReferralProgramID = "+setRPID;
			DataTable dt = DataFunctions.GetDataTable(sql);
			//PopulateDDL();

			foreach(DataRow dr in dt.Rows) {
				programName.Text = dr["ReferralProgramName"].ToString();
				Set_RPHTMLCode(dr["SmartFormHtml"].ToString(), dr["SmartFormFields"].ToString());
                RefereeMsgSubject.Text = dr["Referee_Lead_MsgSubject"].ToString();
				RefererResponseScreen.Text = 	dr["Referer_Response_Screen"].ToString();
//				Response_FromEmail.Text = 	dr["Referer_Response_FromEmail"].ToString();
				RefererFromEmail.Text = 	dr["Referer_Response_FromEmail"].ToString();
				RefererMsgSubject.Text = dr["Referer_Response_MsgSubject"].ToString();
				RefererMsgID.Items.FindByValue(dr["Referer_Response_MsgID"].ToString()).Selected = true;
				RefereeMsgID.Items.FindByValue(dr["Referee_Lead_MsgID"].ToString()).Selected = true;
				SFFieldSet.Text = dr["SmartFormFieldset"].ToString();
				string [] fields = (dr["SmartFormFields"].ToString()).Split(',');
				for (int i=0; i<fields.Length-1; i++)
					RP_OptinFieldSelection.Items.FindByText(fields[i]).Selected = true;
			}
		}

		//! Simply changed the 'SmartForm' to 'ReferralProgram'
		private void LoadSmartFormGrid(int setGroupID){
			string chID = Request.QueryString["chID"].ToString();
			string cuID = Request.QueryString["cuID"].ToString();

			string sqlquery="SELECT ('rpid='+CONVERT(VARCHAR,ReferralProgramID)+'&GroupID="+setGroupID+"&chID="+chID+"&cuID="+cuID+"') as 'ReferralProgramID', ReferralProgramID as 'DelReferralProgramID', ('GroupID="+setGroupID+"&chID="+chID+"&cuID="+cuID+"') as 'RespParams', ReferralProgramName "+
				" FROM ReferralProgram"+
				" WHERE GroupID = "+setGroupID+
				" ORDER BY ReferralProgramName ";

			DataTable dt = DataFunctions.GetDataTable(sqlquery);
			SmartFormGrid.DataSource=dt.DefaultView;
			SmartFormGrid.DataBind();
			GridPager.RecordCount = dt.Rows.Count;
		}

		private void LoadEmailFields(int grpID, ListBox smartFormType) {
			smartFormType.Items.Clear();
			smartFormType.Items.Add(new ListItem("Title", "Title"));
			smartFormType.Items.Add(new ListItem("FirstName", "FirstName"));
			smartFormType.Items.Add(new ListItem("LastName", "LastName"));
			smartFormType.Items.Add(new ListItem("FullName", "FullName"));
			smartFormType.Items.Add(new ListItem("Company", "Company"));
			smartFormType.Items.Add(new ListItem("Occupation", "Occupation"));
			smartFormType.Items.Add(new ListItem("Address", "Address"));
			smartFormType.Items.Add(new ListItem("Address2", "Address2"));
			smartFormType.Items.Add(new ListItem("City", "City"));
			smartFormType.Items.Add(new ListItem("State", "State"));
			smartFormType.Items.Add(new ListItem("Zip", "Zip"));
			smartFormType.Items.Add(new ListItem("Country", "Country"));
			smartFormType.Items.Add(new ListItem("Phone", "Phone"));
			smartFormType.Items.Add(new ListItem("Mobile", "Mobile"));
			smartFormType.Items.Add(new ListItem("Fax", "Fax"));
			smartFormType.Items.Add(new ListItem("Website", "Website"));
			smartFormType.Items.Add(new ListItem("Age", "Age"));
			smartFormType.Items.Add(new ListItem("Income", "Income"));
			smartFormType.Items.Add(new ListItem("Gender", "Gender"));
			smartFormType.Items.Add(new ListItem("User1", "User1"));
			smartFormType.Items.Add(new ListItem("User2", "User2"));
			smartFormType.Items.Add(new ListItem("User3", "User3"));
			smartFormType.Items.Add(new ListItem("User4", "User4"));
			smartFormType.Items.Add(new ListItem("User5", "User5"));
			smartFormType.Items.Add(new ListItem("User6", "User6"));
			smartFormType.Items.Add(new ListItem("Birthdate", "Birthdate"));
			smartFormType.Items.Add(new ListItem("UserEvent1", "UserEvent1"));
			smartFormType.Items.Add(new ListItem("UserEvent1Date", "UserEventDate1"));
			smartFormType.Items.Add(new ListItem("UserEvent2", "UserEvent2"));
			smartFormType.Items.Add(new ListItem("UserEvent2Date", "UserEventDate2"));

			string sqlstmt=
				" SELECT ShortName FROM GroupDatafields "+
				" WHERE GroupID="+ grpID;
			DataTable UDFields = DataFunctions.GetDataTable(sqlstmt);

			foreach(DataRow dr in UDFields.Rows) {
				smartFormType.Items.Add(new ListItem(dr["ShortName"].ToString(), "user_" + dr["ShortName"].ToString()));
			}
		}

		private void ClearRP_Fields(){
			programName.Text = "";
			RP_HTMLCode.Text = "";
			RefereeMsgSubject.Text = "";
			RefererResponseScreen.Text = "";
			RefererFromEmail.Text = "";
			RefererMsgSubject.Text = "";
			RefererMsgID.SelectedIndex = 0;
			SFFieldSet.Text = "";
//			Response_FromEmail.Text = "";
			RefereeMsgID.SelectedIndex = 0;
			RP_OptinFieldSelection.SelectedIndex = 0;
		}
		#endregion

		protected void Page_Init(object sender, EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		#region Web Form Designer generated code
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    

		}
		#endregion

		#region refresh & save Form button clicks
		protected void RefreshHTML_Click(object sender, System.EventArgs e) {
				this.RP_OptinHTMLSave.Enabled = true;
				optinHTML = GetHTMLCode(GroupId, "RP").ToString();
				RP_HTMLCode.Text = optinHTML.ToString();
		}

		protected void SaveOptinHTML_Click(object sender, System.EventArgs e) {
			string connStr=ConfigurationManager.AppSettings["connString"];
			SqlConnection conn	= new SqlConnection(connStr);
			SqlCommand cmd = null;

				string sqlquery=
					" UPDATE ReferralProgram SET "+
					" GroupID=@GroupID, ReferralProgramName=@ReferralProgramName, SmartFormHtml=@SmartFormHtml, "+
					" SmartFormFields=@SmartFormFields, SmartFormFieldset=@SmartFormFieldSet, "+
					" Referer_Response_FromEmail=@Referer_Response_FromEmail, Referer_Response_MsgSubject=@Referer_Response_MsgSubject, "+
					" Referer_Response_MsgID=@Referer_Response_MsgID, Referer_Response_Screen=@Referer_Response_Screen, "+
					" Referee_Lead_MsgSubject=@Referee_Lead_MsgSubject, Referee_Lead_MsgID=@Referee_Lead_MsgID, DateUpdated=@DateUpdated"+
					" WHERE ReferralProgramID = @ReferralProgramID ";
				cmd		= new SqlCommand(sqlquery, conn);

			var optinHTML = GetHTMLCode(GroupId, "RP").ToString();
			cmd.Parameters.AddWithValue("@GroupID", GroupId);
			cmd.Parameters.AddWithValue("@ReferralProgramName", this.programName.Text.ToString());
			cmd.Parameters.AddWithValue("@SmartFormHtml", optinHTML.ToString());
			cmd.Parameters.AddWithValue("@SmartFormFields", selectedFields.ToString());	
			cmd.Parameters.AddWithValue("@SmartFormFieldSet",SFFieldSet.Text);
			cmd.Parameters.AddWithValue("@Referer_Response_MsgSubject", RefererMsgSubject.Text.ToString());
			cmd.Parameters.AddWithValue("@Referer_Response_MsgID", RefererMsgID.SelectedValue);
			cmd.Parameters.AddWithValue("@Referer_Response_Screen", RefererResponseScreen.Text.ToString());	
			cmd.Parameters.AddWithValue("@Referer_Response_FromEmail", RefererFromEmail.Text.ToString());
			cmd.Parameters.AddWithValue("@Referee_Lead_MsgSubject", RefereeMsgSubject.Text.ToString());
			cmd.Parameters.AddWithValue("@Referee_Lead_MsgID", RefereeMsgID.SelectedValue);
			cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now.ToString());	
			cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now.ToString());	
			cmd.Parameters.AddWithValue("@ReferralProgramID",Request.QueryString["rpid"]);

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();
		}

		private string GetHTMLCode(int groupID, string formType){
			string thecode = "";
			 if(formType.Equals("RP")){
				int itemsCnt = RP_OptinFieldSelection.Items.Count;
                string CustomerID = Master.UserSession.CurrentUser.CustomerID.ToString();
				//--- Don't know where to redirect to...
                string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/ReferralProgram.aspx";
				thecode=
					"<form action="+redirpage+">"+ //--- Change it as RPon  as you get more information...
					"<table border=1><tr>";
				if(itemsCnt > 0){
					thecode += "<tr><td>Email Address:</td>";
					for(int i=0; i< itemsCnt; i++)
					{
						if(RP_OptinFieldSelection.Items[i].Selected)
							thecode += "<td>"+RP_OptinFieldSelection.Items[i].Text.ToString()+"</td>";
					}
					thecode += "</tr><tr>";
					for (int j=0; j<Convert.ToInt32(SFFieldSet.Text); j++) 
					{
						thecode += "<td><INPUT id=EmailAddress_"+Convert.ToString(j+1)+" type=text name=EmailAddress_"+Convert.ToString(j+1)+" size=25></td>";
						for(int i=0; i< itemsCnt; i++)
						{
							if(RP_OptinFieldSelection.Items[i].Selected) 
							{
								thecode += "<td><INPUT id="+RP_OptinFieldSelection.Items[i].Text.ToString()+"_"+Convert.ToString(j+1)+
									" type=text name="+RP_OptinFieldSelection.Items[i].Text.ToString()+"_"+Convert.ToString(j+1)+" size=15></td>";
								selectedFields += RP_OptinFieldSelection.Items[i].Text.ToString()+",";
							}
						}		
						thecode += "</tr>";
					}
				}
				thecode += 
					"<tr><td colspan=2 align=center>"+
					"	<INPUT type=hidden value=S name=s>"+
					"	<INPUT type=hidden value=html name=f>"+
					"	<input type=hidden name=g value='"+groupID+"'>"+
					"	<input type=hidden name=c value="+CustomerID+">"+
					"	<input type=hidden name=rpid value='"+ReferralProgramId+"'>"+
					"	<INPUT name=reID type=hidden value=Submit name=Submit>"+
					"  <INPUT name=user_Referred_By type=hidden>"+
					"  <INPUT name=user_Referred_On type=hidden>"+
					"  <INPUT id=Submit type=submit value=Submit name=Submit>"+
					"</td></tr></table></form>";
			}
			return (thecode+jscript);
		}
		#endregion

		#region smartForm selection button Clicks

		protected void DO_SO_SmartFormButton_Click(object sender, System.EventArgs e) {
			Response.Redirect("groupsubscribe.aspx?GroupID="+GroupId+"&chID="+Master.UserSession.CurrentBaseChannel.BaseChannelID+"&cuID="+Master.UserSession.CurrentUser.CustomerID);		
		}
		
		protected void RP_OptinHTMLSaveNew_Click(object sender, EventArgs e) {
			string connStr=ConfigurationManager.AppSettings["connString"];
			SqlConnection conn	= new SqlConnection(connStr);

			string sqlquery=
				" INSERT INTO ReferralProgram ("+
				" GroupID, ReferralProgramName, SmartFormHtml, SmartFormFields, SmartFormFieldset, Referer_Response_FromEmail, "+
				" Referer_Response_MsgSubject, Referer_Response_MsgID, Referer_Response_Screen, Referee_Lead_MsgSubject, "+
				" Referee_Lead_MsgID, DateCreated, DateUpdated) VALUES (@GroupID, @ReferralProgramName, @SmartFormHtml, @SmartFormFields, "+
				" @SmartFormFieldSet, @Referer_Response_FromEmail, @Referer_Response_MsgSubject, @Referer_Response_MsgID," + 
				" @Referer_Response_Screen, @Referee_Lead_MsgSubject, @Referee_Lead_MsgID, @DateCreated, @DateUpdated) ";
			SqlCommand cmd	= new SqlCommand(sqlquery, conn);
		
			var optinHTML = GetHTMLCode(GroupId, "RP").ToString();
			cmd.Parameters.AddWithValue("@GroupID", GroupId);
			cmd.Parameters.AddWithValue("@ReferralProgramName", this.programName.Text.ToString());
			cmd.Parameters.AddWithValue("@SmartFormHtml", optinHTML.ToString());
			cmd.Parameters.AddWithValue("@SmartFormFields", selectedFields.ToString());	
			cmd.Parameters.AddWithValue("@SmartFormFieldSet",SFFieldSet.Text);
			cmd.Parameters.AddWithValue("@Referer_Response_MsgSubject", RefererMsgSubject.Text.ToString());
			cmd.Parameters.AddWithValue("@Referer_Response_MsgID", RefererMsgID.SelectedValue);
			cmd.Parameters.AddWithValue("@Referer_Response_Screen", RefererResponseScreen.Text.ToString());	
			cmd.Parameters.AddWithValue("@Referer_Response_FromEmail", RefererFromEmail.Text.ToString());
			cmd.Parameters.AddWithValue("@Referee_Lead_MsgSubject", RefereeMsgSubject.Text.ToString());
			cmd.Parameters.AddWithValue("@Referee_Lead_MsgID", RefereeMsgID.SelectedValue);
			cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now.ToString());	
			cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now.ToString());	

			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();
			ClearRP_Fields();
			//PopulateDDL();
			LoadSmartFormGrid(GroupId);
		}
		#endregion
	}
}
