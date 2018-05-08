using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.creator.classes;
using ecn.common.classes;
using ECN_Framework_Common.Functions;

namespace ecn.creator.pages {
	
	
	
	public partial class pagedetail : System.Web.UI.Page {
		
		protected Panel[] Slots = new Panel[9];
		
		protected System.Web.UI.WebControls.Button viewHeaderFooterButton;

		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;

		public static string communicatordb	= ConfigurationManager.AppSettings["communicatordb"];
		public static string accountsdb		= ConfigurationManager.AppSettings["accountsdb"];
		int requestPageID = 0;
		public int hfID = 0;

		public pagedetail() {
			Page.Init += new System.EventHandler(Page_Init);
		}
	
		protected void Page_Load(object sender, System.EventArgs e) {

            Master.SubMenu = "add new page";
            Master.Heading = "Add/Edit Page";

            Master.HelpTitle = "Add/Edit Page";
            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            if (KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
				Slots[0]=Slot1;
				Slots[1]=Slot2;
				Slots[2]=Slot3;
				Slots[3]=Slot4;
				Slots[4]=Slot5;
				Slots[5]=Slot6;
				Slots[6]=Slot7;
				Slots[7]=Slot8;
				Slots[8]=Slot9;

				requestPageID = getPageID();
				if(getMsg() == "pgExists"){
					msglabel.Visible = true;
					msglabel.Text = "Page with the same Identifier already exists. Please use a different page Identifier";
				}

				if (Page.IsPostBack==false) {
					BindDropDowns();
					DisableSlotDropDowns();

					TemplateBrowser.TemplateID="1";

					if (requestPageID>0) {
                        ECN_Framework.Common.SecurityAccess.canI("Pages",requestPageID.ToString());
						LoadFormData(requestPageID);
						SetUpdateInfo(requestPageID);
					}
					setViewHeaderFooterURL();
				}	
				SizeLabel.Text = getPageSize().ToString() + " KB";
			}else{
				Response.Redirect("../default.aspx");				
			}
		}

		#region ger request variables
		private int getPageID() {
			int thePageID = 0;
			try {
				thePageID = Convert.ToInt32(Request.QueryString["PageID"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return thePageID;
		}
		private string getMsg() {
			string theMsg = "";
			try {
				theMsg = Request.QueryString["msg"].ToString();
			}
			catch(Exception ) {
				theMsg="";
			}
			return theMsg;
		}
		#endregion

		public int getPageSize() {
			int thesize=0;
			try {
                string sqlquery = "SELECT TemplateSource FROM " + communicatordb + ".dbo.Template WHERE TemplateStyleCode='creator' and TemplateID=" + TemplateBrowser.TemplateID + " and BaseChannelID = " + Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
				string TemplateSource=DataFunctions.ExecuteScalar(sqlquery).ToString();
				string body=TemplateFunctions.EmailHTMLBody(
					TemplateSource, "",
					Convert.ToInt32(ContentSlot1.SelectedItem.Value),
					Convert.ToInt32(ContentSlot2.SelectedItem.Value),
					Convert.ToInt32(ContentSlot3.SelectedItem.Value),
					Convert.ToInt32(ContentSlot4.SelectedItem.Value),
					Convert.ToInt32(ContentSlot5.SelectedItem.Value),
					Convert.ToInt32(ContentSlot6.SelectedItem.Value),
					Convert.ToInt32(ContentSlot7.SelectedItem.Value),
					Convert.ToInt32(ContentSlot8.SelectedItem.Value),
					Convert.ToInt32(ContentSlot9.SelectedItem.Value)
					);
				sqlquery=
						" SELECT HeaderCode, FooterCode FROM HeaderFooters "+
						" WHERE HeaderFooterID = "+HeaderFooter.SelectedValue.ToString();
				DataTable dt = DataFunctions.GetDataTable(sqlquery);
				string headerSource	= dt.Rows[0]["HeaderCode"].ToString();
				string footerSource	= dt.Rows[0]["FooterCode"].ToString();

				int headersize	= ((headerSource.Length + footerSource.Length)/ 1024);
				int htmlsize		= (body.Length/1024);
				thesize = (htmlsize+headersize)+1;
			} catch (Exception E) {
				string devnull=E.ToString();
				this.msglabel.Text = "WARNING: Page size cannot be calculated. ";
			}
			return thesize;
		}

		#region Form Prep
		private void BindDropDowns() {
            string CustomerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();

			HeaderFooter.DataSource = DataLists.GetHeaderFooterDR(CustomerID);
			HeaderFooter.DataBind();

			string sqlQuery = "SELECT * FROM Code WHERE CodeType = 'PageType';";
			DataTable codeDT = DataFunctions.GetDataTable(sqlQuery);
			WebControlFunctions.PopulateDropDownList(PageTypeCode, codeDT, "CodeDisplay", "CodeValue");

			FolderID.DataSource = DataLists.GetFoldersDR(CustomerID, "CNT");
			FolderID.DataBind();
			FolderID.Items.Insert(0,"root");
			FolderID.Items.FindByValue("root").Value = "0";

			ContentSlot1.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot1.DataBind();
			ContentSlot2.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot2.DataBind();
			ContentSlot3.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot3.DataBind();
			ContentSlot4.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot4.DataBind();
			ContentSlot5.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot5.DataBind();
			ContentSlot6.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot6.DataBind();
			ContentSlot7.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot7.DataBind();
			ContentSlot8.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot8.DataBind();
			ContentSlot9.DataSource = DataLists.GetContentDR(CustomerID);
			ContentSlot9.DataBind();
		}

		private void setViewHeaderFooterURL() {
			//viewHeaderFooter.NavigateUrl = "javascript:window.open('viewHeaderFooter.aspx?hfID="+HeaderFooter.SelectedItem.Value.ToString()+"')";
			//viewHeaderFooterButton.Attributes.Add ("onclick","window.open('viewHeaderFooter.aspx?hfID="+HeaderFooter.SelectedItem.Value.ToString()+"', ' ', 'width=800,height=600,resizable=yes,scrollbars=yes');");
			hfID = Convert.ToInt32(HeaderFooter.SelectedItem.Value.ToString());
		}

		public void setViewHeaderFooterURL(object sender, EventArgs e) {
			//viewHeaderFooter.NavigateUrl = "viewHeaderFooter.aspx?hfID="+HeaderFooter.SelectedItem.Value.ToString();
			//viewHeaderFooterButton.Attributes.Add ("onclick","window.open('viewHeaderFooter.aspx?hfID="+HeaderFooter.SelectedItem.Value.ToString()+"', ' ', 'width=800,height=600,resizable=yes,scrollbars=yes');");
			hfID = Convert.ToInt32(HeaderFooter.SelectedItem.Value.ToString());
		}

		private void DisableSlotDropDowns() {
			int theSlots=0;
			if (TemplateBrowser.SlotsTotal!=null){
				theSlots=Convert.ToInt32(TemplateBrowser.SlotsTotal);
			}
			for (int i=0;i<9;i++) {
				Slots[i].Visible=true;
				if (i>=theSlots) {
					Slots[i].Visible=false;
				}
			}
		}

		protected override bool OnBubbleEvent(object sender, EventArgs e) {
			int theSlots=Convert.ToInt32(sender.ToString());
			for (int i=0;i<9;i++) {
				Slots[i].Visible=true;
				if (i>=theSlots) {
					Slots[i].Visible=false;
				}
			}
			return true;
		}

		private static void DisableDropDowns(int theSlots) {
			//not implemented
		}

		private void SetUpdateInfo(int setPageID) {
			PageID.Text=setPageID.ToString();
			SaveButton.Text="Update";
			SaveButton.Visible=false;

			UpdateButton.Visible=true;
			
		}

		private bool checkPageExists(string pgIdentifier){
            string customerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();
			bool exists = false;			
			String sqlQuery=
				" SELECT count(*) "+
				" FROM Page "+
				" WHERE customerID="+customerID+" and QueryValue = '"+pgIdentifier+"'";
			string countRows = DataFunctions.ExecuteScalar(sqlQuery).ToString();

			if(Convert.ToInt32(countRows) > 0){
				exists = true;
			}

			return exists;
		}

		#endregion

		#region Data Load
		private void LoadFormData(int setPageID) {
			String sqlQuery=
				" SELECT p.*, c.WebAddress "+
				" FROM Page p join " + accountsdb + ".dbo.Customer c on p.CustomerID = c.CustomerID"+
				" WHERE PageID="+setPageID+" ";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				PageName.Text	= dr["PageName"].ToString();
				QueryValue.Text	= dr["QueryValue"].ToString();
				QueryValue.Enabled = false;
				HeaderFooter.Items.FindByValue(dr["HeaderFooterID"].ToString()).Selected = true;
				PageTypeCode.Items.FindByValue(dr["PageTypeCode"].ToString()).Selected = true;
				FolderID.Items.FindByValue(dr["FolderID"].ToString()).Selected = true;
				TemplateBrowser.TemplateID=dr["TemplateID"].ToString();
				if(dr["DisplayFlag"].ToString() == "Y") {
					DisplayFlag.Checked = true;
				}
				if(dr["HomePageFlag"].ToString() == "Y") {
					HomePageFlag.Checked = true;
				}
				
				string pgProps = dr["PageProperties"].ToString();
				setPageProperties(pgProps);

				int col_count = getValidColumnCount(Convert.ToInt32(TemplateBrowser.TemplateID));
                
				ContentSlot1.Items.FindByValue(dr["ContentSlot1"].ToString()).Selected = true;

				if(col_count > 1) {
					ContentSlot2.Items.FindByValue(dr["ContentSlot2"].ToString()).Selected = true;
				}
				if(col_count > 2) {
					ContentSlot3.Items.FindByValue(dr["ContentSlot3"].ToString()).Selected = true;
				}
				if(col_count > 3) {
					ContentSlot4.Items.FindByValue(dr["ContentSlot4"].ToString()).Selected = true;
				}
				if(col_count > 4) {
					ContentSlot5.Items.FindByValue(dr["ContentSlot5"].ToString()).Selected = true;
				}
				if(col_count > 5) {
					ContentSlot6.Items.FindByValue(dr["ContentSlot6"].ToString()).Selected = true;
				}
				if(col_count > 6) {
					ContentSlot7.Items.FindByValue(dr["ContentSlot7"].ToString()).Selected = true;
				}
				if(col_count > 7) {
					ContentSlot8.Items.FindByValue(dr["ContentSlot8"].ToString()).Selected = true;
				}
				if(col_count > 8) {
					ContentSlot9.Items.FindByValue(dr["ContentSlot9"].ToString()).Selected = true;
				}

				// Following was done for Select Comfort Customer.. they are no longer our Customer.
				/*if(dr["CustomerID"].ToString().Equals("53")) {
					HyperLink1.NavigateUrl = "contentfilters.aspx?ContentID=" + dr["ContentSlot1"].ToString();
					HyperLink2.NavigateUrl = "contentfilters.aspx?ContentID=" + dr["ContentSlot2"].ToString();
					HyperLink3.NavigateUrl = "contentfilters.aspx?ContentID=" + dr["ContentSlot3"].ToString();
					HyperLink4.NavigateUrl = "contentfilters.aspx?ContentID=" + dr["ContentSlot4"].ToString();
					HyperLink5.NavigateUrl = "contentfilters.aspx?ContentID=" + dr["ContentSlot5"].ToString();
					HyperLink6.NavigateUrl = "contentfilters.aspx?ContentID=" + dr["ContentSlot6"].ToString();
				} */
				string website = dr["WebAddress"].ToString();
				URLPanel.Visible = true;
				WebsiteURL.Text = "http://"+website+"/web/pagedetail.aspx?pgID="+dr["QueryValue"].ToString();
			}
		}
		#endregion

		#region Data Handlers

		public void CreatePage(object sender, System.EventArgs e) {
			string pgIdentifier = QueryValue.Text.ToString();
			if(!(checkPageExists(pgIdentifier))){
				char displayFlag = 'N';
				if(DisplayFlag.Checked) {
					displayFlag = 'Y';
				}
				char homePageFlag = 'N';
				if(HomePageFlag.Checked) {
					homePageFlag = 'Y';
				}

				string pageProperties = getPageProperties();
	            string [] content_slot	= getValidColumns(Convert.ToInt32(TemplateBrowser.TemplateID));
				string sqlquery= "";
				try{
					sqlquery = 
						" INSERT INTO Page ( "+
						" CustomerID, HeaderFooterID, QueryValue, PageTypeCode, "+
						" PageName, AddDate, HomePageFlag, DisplayFlag, PageProperties, TemplateID, "+
						" ContentSlot1, ContentSlot2, ContentSlot3, "+
						" ContentSlot4, ContentSlot5, ContentSlot6, "+
						" ContentSlot7, ContentSlot8, ContentSlot9, "+
						" FolderID, UserID, UpdatedDate, PageSize "+
						" ) VALUES ( "+
						Master.UserSession.CurrentCustomer.CustomerID.ToString() +", "+HeaderFooter.SelectedItem.Value+", '"+pgIdentifier+"', '"+PageTypeCode.SelectedItem.Value+"', "+
						" '"+DataFunctions.CleanString(PageName.Text.ToString())+"', '"+System.DateTime.Now+"', '"+homePageFlag+"', '"+displayFlag+"', '"+pageProperties+"', "+TemplateBrowser.TemplateID+", "+
						" "+content_slot[0]+", "+content_slot[1]+", "+content_slot[2]+", "+
						" "+content_slot[3]+", "+content_slot[4]+", "+content_slot[5]+", "+
						" "+content_slot[6]+", "+content_slot[7]+", "+content_slot[8]+", "+
						" "+FolderID.SelectedItem.Value+", "+Master.UserSession.CurrentUser.UserID.ToString()+", '"+System.DateTime.Now+"',"+getPageSize()+" ) SELECT @@IDENTITY;";
					int newPageID = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlquery));
					Response.Redirect("pagedetail.aspx?PageID="+newPageID);
				}catch{
					msglabel.Text = "ERROR: Couldnot Create Page.";
				}
			}else{
				Response.Redirect("pagedetail.aspx?msg=pgExists");
			}
		}

		public void UpdatePage(object sender, System.EventArgs e) {
            string customerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();
			bool exists = false;
			/*String sqlQuery=
				" SELECT * "+
				" FROM Pages "+
				" WHERE customerID="+customerID+" and QueryValue = '"+QueryValue.Text.ToString()+"'";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				if(!(dr["PageID"].ToString()).Equals(getPageID().ToString())){
					exists = true;
					break;
				}
			}*/

			if(!(exists)){
				char displayFlag = 'N';
				if(DisplayFlag.Checked) {
					displayFlag = 'Y';
				}

				char homePageFlag = 'N';
				if(HomePageFlag.Checked) {
					homePageFlag = 'Y';
				}
				
				string pageProperties = getPageProperties();
                string [] content_slot = getValidColumns(Convert.ToInt32(TemplateBrowser.TemplateID));
				
				string sqlquery=
					" UPDATE Page SET "+
					" PageName='"+DataFunctions.CleanString(PageName.Text.ToString())+"', "+
					" HeaderFooterID = "+HeaderFooter.SelectedItem.Value+", "+
					" PageTypeCode ='"+PageTypeCode.SelectedItem.Value+"', "+
					" HomePageFlag = '"+homePageFlag+"', "+
					" DisplayFlag = '"+displayFlag+"', "+
					" PageProperties = '"+pageProperties+"', "+
					" TemplateID="+TemplateBrowser.TemplateID+", "+
					" ContentSlot1="+content_slot[0]+", "+
					" ContentSlot2="+content_slot[1]+", "+
					" ContentSlot3="+content_slot[2]+", "+
					" ContentSlot4="+content_slot[3]+", "+
					" ContentSlot5="+content_slot[4]+", "+
					" ContentSlot6="+content_slot[5]+", "+
					" ContentSlot7="+content_slot[6]+", "+
					" ContentSlot8="+content_slot[7]+", "+
					" ContentSlot9="+content_slot[8]+", "+
					" FolderID="+FolderID.SelectedItem.Value+", "+
					" UpdatedDate='"+System.DateTime.Now+"', "+
					" PageSize="+getPageSize()+
					" WHERE PageID="+PageID.Text;
				DataFunctions.Execute(sqlquery);
				Response.Redirect("default.aspx");
			}else{
				Response.Redirect("pagedetail.aspx?msg=pgExists");
			}
		}
		#endregion

		private string getPageProperties(){
			string bgcolor			= pg_bg_color.Text.ToString().Length == 0?"#FFFFFF":pg_bg_color.Text.ToString();
			string text				= pg_txt_color.Text.ToString().Length == 0?"#000000":pg_txt_color.Text.ToString();
			string link				= pg_link_color.Text.ToString().Length == 0?"#0000FF":pg_link_color.Text.ToString();
			string vlink				= pg_vlink_color.Text.ToString().Length == 0?"#FF0000":pg_vlink_color.Text.ToString();
			string alink				= pg_alink_color.Text.ToString().Length == 0?"#123456":pg_alink_color.Text.ToString();
			string marginwidth	= pg_margin_width.Text.ToString().Length == 0?"0":pg_margin_width.Text.ToString();
			string marginheight	= pg_margin_height.Text.ToString().Length == 0?"0":pg_margin_height.Text.ToString();
			string leftmargin		= pg_left_margin.Text.ToString().Length == 0?"0":pg_left_margin.Text.ToString();
			string toptmargin		= pg_top_margin.Text.ToString().Length == 0?"0":pg_top_margin.Text.ToString();
			string otherprops		= pg_other_props.Text.ToString().Length == 0?"":pg_other_props.Text.ToString();

			string pgProps			= "bgcolor="+bgcolor+" text="+text+" link="+link+" vlink="+vlink+" alink="+alink+" marginwidth="+marginwidth+" marginheight="+marginheight+" leftmargin="+leftmargin+" topmargin="+toptmargin+" "+otherprops;

			return pgProps;
		}

		private int getValidColumnCount(int template_id) {
			String sqlQuery=
				" SELECT * "+
				" FROM " + communicatordb + ".dbo.Template "+
				" WHERE TemplateID="+template_id+" AND TemplateStyleCode='creator' ";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			return (int) Convert.ToInt32(dt.Rows[0]["SlotsTotal"]);
		}

		private string [] getValidColumns(int template_id)  {
			int valid_column_count = getValidColumnCount(template_id);

			string [] content_slot = { "''", "''", "''", "''", "''", "''", "''", "''", "''" };

			if( valid_column_count >= 1 )
				content_slot[0] = ContentSlot1.SelectedItem.Value;
			if( valid_column_count >= 2 )
				content_slot[1] = ContentSlot2.SelectedItem.Value;
			if( valid_column_count >= 3 )
				content_slot[2] = ContentSlot3.SelectedItem.Value;
			if( valid_column_count >= 4 )
				content_slot[3] = ContentSlot4.SelectedItem.Value;
			if( valid_column_count >= 5 )
				content_slot[4] = ContentSlot5.SelectedItem.Value;
			if( valid_column_count >= 6 )
				content_slot[5] = ContentSlot6.SelectedItem.Value;
			if( valid_column_count >= 7 )
				content_slot[6] = ContentSlot7.SelectedItem.Value;
			if( valid_column_count >= 8 )
				content_slot[7] = ContentSlot8.SelectedItem.Value;
			if( valid_column_count >= 9 )
				content_slot[8] = ContentSlot9.SelectedItem.Value;
			return content_slot;
		}

		private void setPageProperties(string pgProps){
			StringTokenizer st = new StringTokenizer(pgProps, ' ');
			string[] token=new string[10]; string[] textVal = new string[10];
			int i = 0;
			while(st.HasMoreTokens()){
				token[i] = st.NextToken();
				if(i<9){
					int tok_startPos	= token[i].IndexOf("=");
					int tok_endPos		= token[i].Length;
					textVal[i] = token[i].Substring(tok_startPos+1);
				}else{
					textVal[i] = token[i];
				}
				i++;
			}
			pg_bg_color.Text			= textVal[0];
			pg_txt_color.Text			= textVal[1];
			pg_link_color.Text		= textVal[2];
			pg_vlink_color.Text		= textVal[3];
			pg_alink_color.Text		= textVal[4];
			pg_margin_width.Text	= textVal[5];
			pg_margin_height.Text	= textVal[6];
			pg_left_margin.Text		= textVal[7];
			pg_top_margin.Text		= textVal[8];
			//pg_other_props.Text		= StringFunctions.Replace(textVal[9],"\"","'");
		}

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


	}
}
