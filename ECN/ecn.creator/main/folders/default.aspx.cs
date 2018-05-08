using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.folders {
	
	
	
	public partial class folderseditor : System.Web.UI.Page {


		protected static string connStr	= ConfigurationManager.AppSettings["connstring"];
		public static string communicatordb	= ConfigurationManager.AppSettings["communicatordb"];

		//ECN_Framework.Common.SecurityCheck sc	= new ECN_Framework.Common.SecurityCheck();
		SqlConnection conn				= new SqlConnection(connStr);
		int selectedFolderID				= 0;
		string customerID					= "0";
		string selectedFolderType	= "";

		string folderID					= "";
		string folderName				= "";
		string folderDescription		= "";
		string folderType				= "";
		string systemFolder			= "";

		string parentID				= "";

		public folderseditor() {
			Page.Init += new System.EventHandler(Page_Init);
		}
	
		protected void Page_Load(object sender, System.EventArgs e) 
        {
            Master.SubMenu = "folders";
            Master.Heading = "Folders Manager";

            Master.HelpTitle = "Folders Manager";
			//if(KMPlatform.BusinessLogic.User.IsAdministrator(Master.UserSession.CurrentUser)){
				selectedFolderType = FolderTypeDR.SelectedItem.Value.ToString();
                customerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();

				if(Page.IsPostBack)
                {
					selectedFolderID = FolderControl.SelectedFolderID;
					FolderError.Visible		= false;

                    hasEdit();
                    
                    if (selectedFolderID > 0)
                    {
						LoadFolderData(selectedFolderID);
						DetailsButton.Enabled	= true;
                        hasDelete();
                    }
				}
                
                else
                {
					LoadFolderControl(selectedFolderType);
				}
		
        }

        protected void hasEdit(string edit = "")
        {
            if (((selectedFolderType == "GRP") && (KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING,
                            KMPlatform.Enums.ServiceFeatures.GroupFolder, KMPlatform.Enums.Access.Edit))
                    || ((selectedFolderType == "CNT") && (KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING,
                                KMPlatform.Enums.ServiceFeatures.ContentFolder, KMPlatform.Enums.Access.Edit)))))
            {
                EditButton.Enabled = true;
                if (edit == "")
                {
                    AddButton.Enabled = true;
                }
            }
        }
        
        protected void hasDelete()
        {
            if  (((selectedFolderType == "GRP") && (KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING,
                    KMPlatform.Enums.ServiceFeatures.GroupFolder, KMPlatform.Enums.Access.Delete))
                    || ((selectedFolderType == "CNT") && (KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING,
                    KMPlatform.Enums.ServiceFeatures.ContentFolder, KMPlatform.Enums.Access.Delete)))));
            {
                DeleteButton.Enabled = true;
            }
        }


		#region Data Load
		private void LoadFolderData(int selectedFolderID) {
			string foldersQuery=
				" SELECT * FROM "+communicatordb+".dbo.Folder "+
				" WHERE FolderID = "+selectedFolderID;
			DataTable dt = DataFunctions.GetDataTable(foldersQuery);

			foreach(DataRow dr in dt.Rows){
				folderID			= dr["FolderID"].ToString();
				folderName			= dr["FolderName"].ToString();
				folderDescription	= dr["FolderDescription"].ToString();
				folderType			= dr["FolderType"].ToString();
				systemFolder		= dr["IsSystem"].ToString();
				parentID			= dr["ParentID"].ToString();
			}
		}

		private void LoadFromData() {
			txtFolderName.Text		= folderName.ToString();
			txtFolderDesc.Text		= folderDescription.ToString();
            hasEdit("edit");
            //EditButton.Enabled	= true;
		}

		private void LoadFolderControl(string selectedFolderType){
			FolderControl.ID = "FolderControl";
			FolderControl.CustomerID			= Convert.ToInt32(customerID);
			FolderControl.FolderType			= selectedFolderType;
			FolderControl.ConnString			= connStr;
			FolderControl.NodesExpanded		= true;
			FolderControl.ChildrenExpanded	= false;
			//FolderControl.SelectedFolderID	= 11;
			FolderControl.LoadFolderTree();

			DetailsButton.Enabled	= false;
            hasEdit();
            hasDelete();
            
            //DeleteButton.Enabled	= false;
		}
		#endregion

		#region Data Handlers

		public void DeleteFolderChecks(string type) {
			if(systemFolder.ToLower().Equals("true")){
				FolderError.Text = "This a System Folder. Delete is not allowed !!";
				FolderError.Visible = true;
			}else{
				if(SubFolderCheck(type) > 0){
					FolderError.Text = "Subfolders exist for this Folder. Delete is not allowed !!";
					FolderError.Visible = true;					
				}else{
					if(type.Equals("GRP")){
						if(GroupsCheck() > 0){
							FolderError.Text = "Groups exist in this Folder. Delete is not allowed !!";
							FolderError.Visible = true;											
						}else{
							DeleteFolder();						
						}
					}else if(type.Equals("CNT")){
						if(ContentsCheck() > 0){
							FolderError.Text = "Contents / Campaigns exist in this Folder. Delete is not allowed !!";
							FolderError.Visible = true;											
						}else{
							DeleteFolder();						
						}
					}else{
						FolderError.Text = "Unrecognized Folder. Delete is not allowed !!";
						FolderError.Visible = true;								
					}
				}
			}
		}

		public int GroupsCheck(){
			string sqlcheck=
				" SELECT COUNT(*) as Count FROM "+communicatordb+".dbo.Folder f, "+communicatordb+".dbo.Groups g "+
				" WHERE f.FolderID = @FolderID"+
				" AND f.CustomerID=@CustomerID "+
				" AND f.folderID = g.folderID ";
			SqlCommand cmd		= new SqlCommand(sqlcheck, conn);
            cmd.Parameters.AddWithValue("@FolderID", selectedFolderID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
			conn.Open();
			int groupsExist=Convert.ToInt32(cmd.ExecuteScalar().ToString());
			conn.Close();

			return groupsExist;
		}

		public int ContentsCheck(){
			string sqlContentcheck=
				" SELECT COUNT(*) as Count FROM "+communicatordb+".dbo.Folder f, "+communicatordb+".dbo.Content c "+
				" WHERE f.FolderID = @FolderID"+
				" AND f.CustomerID=@CustomerID AND c.CustomerID = @CustomerID "+
				" AND (f.folderID = c.folderID)";
			string sqlLayoutcheck=
				" SELECT COUNT(*) as Count FROM "+communicatordb+".dbo.Folder f, Page p "+
				" WHERE f.FolderID = @FolderID"+
				" AND f.CustomerID=@CustomerID AND p.CustomerID = @CustomerID "+
				" AND (f.folderID = p.folderID)";
			SqlCommand cntCmd		= new SqlCommand(sqlContentcheck, conn);
            cntCmd.Parameters.AddWithValue("@FolderID", selectedFolderID);
            cntCmd.Parameters.AddWithValue("@CustomerID", customerID);

			SqlCommand LytCmd		= new SqlCommand(sqlLayoutcheck, conn);
            LytCmd.Parameters.AddWithValue("@FolderID", selectedFolderID);
            LytCmd.Parameters.AddWithValue("@CustomerID", customerID);

			conn.Open();
			int cntExist	=Convert.ToInt32(cntCmd.ExecuteScalar().ToString());
			int LytExist	=Convert.ToInt32(LytCmd.ExecuteScalar().ToString());

			conn.Close();

			return cntExist+LytExist;
		}

		public int SubFolderCheck(string type){
			string sqlCheck = "SELECT COUNT(FolderID) from "+communicatordb+".dbo.Folder where ParentID = @ParentID AND FolderType = @FolderType";
			SqlCommand cmd		= new SqlCommand(sqlCheck, conn);
            cmd.Parameters.AddWithValue("@ParentID", selectedFolderID);
            cmd.Parameters.AddWithValue("@FolderType", type);

			conn.Open();
			int subFoldersExist = Convert.ToInt32(cmd.ExecuteScalar().ToString());
			conn.Close();

			return subFoldersExist;
		}

		public void DeleteFolder(){
			string sqlquery	=	"DELETE FROM "+communicatordb+".dbo.Folder WHERE FolderID = @FolderID ";
			SqlCommand cmd		= new SqlCommand(sqlquery, conn);
            cmd.Parameters.AddWithValue("@FolderID", folderID);
			
			conn.Open();
			cmd.ExecuteNonQuery();
			conn.Close();		

			LoadFolderControl(selectedFolderType);	
		}

		public void CreateFolder() {
			if(systemFolder.ToLower().Equals("true")){
				FolderError.Text = "This a System Folder. Sub Folders cannot be created.";
				FolderError.Visible = true;
			}else{
				string sqlquery	=	" INSERT INTO "+communicatordb+".dbo.Folder ( "+
					" CustomerID, FolderName, FolderType, FolderDescription, IsSystem, ParentID "+
					" ) VALUES ( "+
					" @CustomerID, @FolderName, @FolderType, @FolderDescription, @IsSystem, @ParentID)";
				SqlCommand cmd		= new SqlCommand(sqlquery, conn);

				cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@FolderName", txtFolderName.Text);
                cmd.Parameters.AddWithValue("@FolderType", selectedFolderType);
                cmd.Parameters.AddWithValue("@FolderDescription", txtFolderDesc.Text);
                cmd.Parameters.AddWithValue("@IsSystem", false);
                cmd.Parameters.AddWithValue("@ParentID", selectedFolderID);
			
				conn.Open();
				cmd.ExecuteNonQuery();
				conn.Close();

				LoadFolderControl(selectedFolderType);
			}
		}

		public void UpdateFolder() {
			if(systemFolder.ToLower().Equals("true")){
				FolderError.Text = "This a System Folder. Cannot be Modified";
				FolderError.Visible = true;
			}else{
				string sqlquery=
					" UPDATE "+communicatordb+".dbo.Folder SET "+
					" FolderName= @FolderName, "+
					" FolderDescription= @FolderDescription"+
					" WHERE FolderID= @FolderID";
				SqlCommand cmd		= new SqlCommand(sqlquery, conn);

				string folderName = txtFolderName.Text.ToString();
				string folderDesc = txtFolderDesc.Text.ToString();
				int folderID			=  selectedFolderID;

                cmd.Parameters.AddWithValue("@FolderName", folderName);
                cmd.Parameters.AddWithValue("@FolderDescription", folderDesc);
                cmd.Parameters.AddWithValue("@FolderID", folderID);
				
				conn.Open();
				cmd.ExecuteNonQuery();
				conn.Close();

				LoadFolderControl(selectedFolderType);
			}
		}
		#endregion

		#region Event Handlers
		protected void DetailsButton_Click(object sender, System.EventArgs e) {
			LoadFromData();
		}

		protected void AddButton_Click(object sender, System.EventArgs e) {
			CreateFolder();
		}

		protected void EditButton_Click(object sender, System.EventArgs e) {
			UpdateFolder();
		}

		protected void DeleteButton_Click(object sender, System.EventArgs e) {
			string type = FolderTypeDR.SelectedValue.ToString();
			DeleteFolderChecks(type);
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

		protected void FolderTypeDR_SelectedIndexChanged(object sender, System.EventArgs e) {
			LoadFolderControl(selectedFolderType);
		}

        
	}
}
