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

using System.Data.OleDb;		// for reading Excel file
using System.IO;					// for reading other type of delimited files.
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;

using ecn.common.classes;
using ecn.communicator.classes;
using ecn.communicator.classes.ImportData;
using System.Configuration;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for Importer1.
	/// </summary>
	public partial class Importer1 : ecn.wizard.MasterPage
	{

		protected System.Web.UI.HtmlControls.HtmlSelect tableColumnHeadersSelectbox; //columnNames of Emails table	

		string file			= "";
		string ftc			= "";
		string stc			= "";
		string gid				= "";
		string fileType		= "";
		string sheetName	= "";
		string lineStart		= "";	
        
		ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
		ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
		ArrayList columnHeadings = new ArrayList();

		protected void Page_Load(object sender, System.EventArgs e)	{
			getDatafromQueryString(); //get the values from QueryString
			errlabel.Text = "";
			msglabel.Text = "";	
			btnImport.Visible = true;
			if(Page.IsPostBack){
				return;
			}				
			BuildEmailImportForm(GetDataTableByFileType(file, sheetName, 5));             
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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnImport.Click += new System.Web.UI.ImageClickEventHandler(this.btnImport_Click);

		}
		#endregion

		//Import data & Load data to the Database
		// Gah. I don't have time to rewrite this again. I'm going to use the code as is, remove the dependency on group values
		// and call any "import" events manually.
		protected void ImportData(object sender, System.EventArgs e) {			
			
			int skipedCount = 0;
			int totalRecs = 0;
			int totalChanged = 0;
			
			DataTable dt = new DataTable();
			SqlDateTime sqldatenull = SqlDateTime.Null;			

			// Just load one line of data to get the column count.
			dt = GetDataTableByFileType(file, sheetName, 1);
			int countColumns = dt.Columns.Count;
			
			ImportMapper mapper = new ImportMapper();
			
			int duplicationColumnCount = 0;
			StringBuilder duplicatedColumns = new StringBuilder();
			
			int mappercount = 0;

			for(int i=0; i<countColumns; i++){
				string selectedColumnName = Request.Params.Get("ColumnHeaderSelect"+i);
				if( selectedColumnName == "Ignore")
				{
					continue;
				}
				
				if (!mapper.AddMapping(mappercount, selectedColumnName)) 
				{
					duplicationColumnCount ++;
					duplicatedColumns.Append(string.Format("{0}{1}", duplicatedColumns.Length>0?"/":"", selectedColumnName));
				}
				mappercount++;

//				if( selectedColumnName != "Ignore")
//				{
//					if (!mapper.AddMapping(mappercount, selectedColumnName)) 
//					{
//						duplicationColumnCount ++;
//						duplicatedColumns.Append(string.Format("{0}{1}", duplicatedColumns.Length>0?"/":"", selectedColumnName));
//					}
//					mappercount++;
//				}
			}

			if (duplicationColumnCount>0) {
				printerr(string.Format("Error: <br>Selected duplicated columns : {0}.", duplicatedColumns.ToString()));	
				Reset(mapper);
				return;
			}
			
			if(mapper.MappingCount == 0){	
				printerr("Error:<br>Email address, first name and last name are required to import data.");	
				Reset(mapper);
				return;
			}

			if (!mapper.HasEmailAddress) {
				printerr("Error: <br>Email address, first name and last name are required to import data.");	
				Reset(mapper);
				return;
			}

			bool bFirstNameexists = false;
			bool bLastNameexists = false;

			for(int i=0; i<mapper.MappingCount; i++)
			{
				if (mapper.GetColumnName(i).ToString().ToLower() == "firstname")
				{	
					bFirstNameexists = true;
				}

				if (mapper.GetColumnName(i).ToString().ToLower() == "lastname")
				{
					bLastNameexists = true;
				}
			}

			if ((!bFirstNameexists) || (!bLastNameexists)) 
			{
				printerr("Error:<br><br>“Email address, first name and last name are required to import data.");	
				Reset(mapper);
				return;
			}

			// After all the requirement is satisfied, all the data are loaded.
			dt = GetDataTableByFileType(file, sheetName, 0);

			//Add user_type column with default value NEWLIST-MMDDYYYY-HHMM
			
			int customer_id = Convert.ToInt32(CustomerID);
			Groups group_to_link = new Groups(gid);
			bool groupHasUDFs = group_to_link.HasGroupDataFields();
            
			string connection_string = DataFunctions.GetConnectionString();
			SqlConnection db_connection = new SqlConnection(connection_string);
			db_connection.Open();
			
			ArrayList errorReports = new ArrayList();
			int rowCount = 0;
			DateTime startDateTime = DateTime.Now;
			string userType = "NEWLIST-" + DateTime.Now.ToString("MMddyyyy-HHmm");
			lblMessage.Text = userType+"<br>";
			foreach(DataRow row in dt.Rows) {
				ImportSqlBuilder builder = new ImportSqlBuilder();
				builder.AddParameter("GroupID", gid);
				builder.AddParameter("CustomerID", customer_id);
				builder.AddParameter("SubscribeTypeCode",stc);
				builder.AddParameter("FormatTypeCode", ftc);
				for(int i=0; i< countColumns; i++) {
					if (mapper.IsIgnored(i)) {
						continue;
					}				
					// We can do something nice here since we know the sqldbtype of the column
					builder.AddParameter(mapper.GetColumnName(i), row[i]);
					//I think this where we can add the user_Type <-- NEWLIST-MMDDYYY-HHMM for each row!
					builder.AddParameter("user_Type",userType);
				}
				SqlCommand cmd = builder.GetSqlCommand(GroupDataFields, ColumnManager);
				if (cmd == null) {
					skipedCount++;
					errorReports.Add(new ImportErrorReport(rowCount, "N/A", "Email Address is blank."));
					continue;
				}
				cmd.Connection = db_connection;
				try {
					bool isInsert = Convert.ToBoolean(cmd.ExecuteScalar());
					if (isInsert) {
						totalRecs ++;
					} else {
						totalChanged ++;
					}
				
				} 
				catch (Exception importException) {					
					errorReports.Add(new ImportErrorReport(rowCount, Convert.ToString(cmd.Parameters[0].Value),importException.Message)); 
					skipedCount ++;
				}
				finally {
					rowCount ++;
				}
			}

			db_connection.Close();           
			
			print("<br><br>Successfully Inserted "+totalRecs  +" rows <br>");
			print("Successfully Changed "+totalChanged  +" rows <br>");
			print("Skipped "+ skipedCount +" rows because of errors <br>");
			TimeSpan duration = DateTime.Now - startDateTime;
			print("Importing " + (totalRecs+totalChanged+skipedCount) + " rows takes " + duration.Hours + ":" + duration.Minutes +":"+ duration.Seconds);

			if (skipedCount > 0) {				
				dgdErrorReport.Caption = "Import Data Error Report";
				dgdErrorReport.Visible = true;
				dgdErrorReport.DataSource = errorReports;
				dgdErrorReport.DataBind();
			}

			CreateFilter(CustomerID.ToString(), sc.UserID(), gid, userType, userType+"=''"+userType+"''", DateTime.Now.ToString(), userType);

			Redirecter();
		} 

		private void Redirecter() {
			// Once user has uploaded the list they are supposed to be redirected directly to the first-step of wizard [choose-template]
			Response.Redirect("Wizard.aspx?gid=" + gid);
		}
		
		private void CreateFilter (string cid, string uid, string gid, string fn, string wc, string cd, string cv) {
			string insertQuery = "INSERT INTO Filters (CustomerID, UserID, GroupID, FilterName, WhereClause, CreateDate) "+
				"VALUES ("+cid+","+uid+","+gid+",'"+fn+"','"+wc+"','"+cd+"'); SELECT @@IDENTITY";
			lblMessage.Text += "<br>FILTERS: "+insertQuery+"<BR>";
			try {
				string fid = DataFunctions.ExecuteScalar(insertQuery).ToString();
				insertQuery = "INSERT INTO FiltersDetails (FilterID, CompareType, FieldName, Comparator, CompareValue) "+
					"VALUES ("+fid+",'AND','Type','equals','"+cv+"')";
				DataFunctions.Execute(insertQuery);
				lblMessage.Text += "<BR>"+insertQuery;
			} catch (Exception err) {
				lblMessage.Text = "Error: "+err.Message;
			}
		}

		private void Reset(ImportMapper mapper) {
			BuildEmailImportForm(GetDataTableByFileType(file, sheetName, 5), mapper);  
		}

		#region Method to build HTML table for import data
		private void BuildEmailImportForm(DataTable dt) {
			BuildEmailImportForm(dt, new ImportMapper());
		}

		private void BuildEmailImportForm(DataTable dt, ImportMapper mapper) {
			if (dt == null) {
				return;
			}			

			HtmlTableRow tableRows = null;
			HtmlTableCell headerColumn = null;	// <td> to hold the header which is the emails table dropdown columns
			HtmlTableCell dataColumn = null;		// <td> to hold the data from the file
			int countColumns = dt.Columns.Count;						

			for(int i = 0; i<countColumns; i++){
				//build the HTML select control which has the EmailTable's column headings
				tableColumnHeadersSelectbox = buildColumnHeaderDropdowns("ColumnHeaderSelect"+i);
				tableColumnHeadersSelectbox.SelectedIndex = tableColumnHeadersSelectbox.Items.IndexOf(tableColumnHeadersSelectbox.Items.FindByText(mapper.GetColumnName(i)));
				tableRows = new HtmlTableRow();		//create a <TR> ie the ROW 
				headerColumn = new HtmlTableCell();	//create a <TD> ie headerColumn
				headerColumn.Controls.Add(tableColumnHeadersSelectbox);	//add the <select/> to the TD
				headerColumn.VAlign = "middle";
				headerColumn.Align = "middle";
				headerColumn.Height = "32px";
				headerColumn.Style.Add("background-color","#D6DFFF");
				tableRows.Cells.Add(headerColumn);	//add the <td> to the <tr>

				//now add the data from the file to the next TD
				dataColumn	= new HtmlTableCell();
				dataColumn.Style.Add("font-family","Verdana, Arial, Helvetica, sans-serif");
				dataColumn.Style.Add("font-size","10px");
				dataColumn.Style.Add("background-color","#D6DFFF");
				Label tableDataColumnLabel = new Label();
				string textData = "";
				int lineStartCheck = 0;
				foreach(DataRow dr in dt.Rows){
					textData += dr[i].ToString()+", ";
					lineStartCheck++;
				}

				tableDataColumnLabel.Text = 
					string.Format("<font color='black'>{0}</font>",
					StringFunctions.Left(textData, textData.Length>100?100:textData.Length)+((textData.Length>100)?"<font color=orange><b>&nbsp;&nbsp;&nbsp;more...</b></font>":" "));
				dataColumn.Controls.Add(tableDataColumnLabel);
				tableRows.Cells.Add(	dataColumn);

				dataCollectionTable.Rows.Add(tableRows);	//add the <TR> to the table
			}
		}

		private HtmlSelect buildColumnHeaderDropdowns(string selectBoxName){
			int j = 0;

			HtmlSelect selectbox = new HtmlSelect();
			selectbox.ID = selectBoxName;
			selectbox.Style.Add("font-family","Verdana, Arial, Helvetica, sans-serif");
			selectbox.Style.Add("font-size","11px");
			selectbox.Style.Add("background-color","#FCF8E9");

			for(int i=0; i< ColumnManager.ColumnCount; i++) 
			{
				if (ColumnManager.GetColumnNameByIndex(i).ToLower().IndexOf("ignore") >= 0 ||
					ColumnManager.GetColumnNameByIndex(i).ToLower().IndexOf("emailaddress") >= 0 ||
					ColumnManager.GetColumnNameByIndex(i).ToLower().IndexOf("firstname") >= 0 ||
					ColumnManager.GetColumnNameByIndex(i).ToLower().IndexOf("lastname") >= 0 ||
					ColumnManager.GetColumnNameByIndex(i).ToLower().IndexOf("company") >= 0 ||
					ColumnManager.GetColumnNameByIndex(i).ToLower().IndexOf("voice") >= 0)
				{
					
					if (ColumnManager.GetColumnNameByIndex(i).ToLower().IndexOf("voice") >= 0)
					{
						selectbox.Items.Insert(j, new ListItem("Phone",ColumnManager.GetColumnNameByIndex(i)));
					}
					else
					{
						selectbox.Items.Insert(j, new ListItem(ColumnManager.GetColumnNameByIndex(i),ColumnManager.GetColumnNameByIndex(i)));
					}
					j++;
				}
			}	
			
		
			return selectbox;
		}

		#endregion

		#region Import data helper methods/properties	
		private DataTable GetDataTableByFileType(string fileName, string excelSheetName, int maxRecordsToRetrieve) {
			int startLine = (Convert.ToInt32(lineStart)-1 >= 0 )?(Convert.ToInt32(lineStart)-1):0;
			//cc.getAssetsPath("accounts")
			//string physicalDataPath = "e:/http/ecn5"+cc.getAssetsPath("accounts")+"/"+"channelID_"+ ChannelID +"/customers/"+CustomerID+"/data";
			string physicalDataPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + sc.CustomerID() + "/data");

			try 
			{			
				return FileImporter.GetDataTableByFileType(physicalDataPath, fileType, fileName, excelSheetName, startLine, maxRecordsToRetrieve, "");
			}
			catch(ArgumentException) {
				printerr("Error:<br><br>Unknow file type.<br> Please contact System Administrator by sending an <a href='mailto:support@teckman.com'>email</a> along with the following Stack Trace.<br><br>");
			}
			catch(Exception ex){
				switch (fileType) {
					case "X": 
						printerr("Error:<br><br>Error occured while fetching data from the Excel File.<br> Please contact System Administrator by sending an <a href='mailto:support@teckman.com'>email</a> along with the following Stack Trace.<br><br>"+ex.Message.ToString());
						break;
					default:
						printerr("Error:<br<br>Error occured while fetching data from the CSV File.<br> Please contact System Administrator by sending an <a href='mailto:support@teckman.com'>email</a> along with the following Stack Trace.<br><br>"+ex.Message.ToString());
						break;
				}
			}			
			return null;
		}
		private ArrayList _groupDataFields = null;
		protected ArrayList GroupDataFields {
			get {
				if (_groupDataFields == null) {					
					_groupDataFields = GroupDataField.GetGroupDataFieldsByGroupID(Convert.ToInt32(gid));
				}
				return (this._groupDataFields);
			}			
		}

		private EmailTableColumnManager _columnManager = null;
		public EmailTableColumnManager ColumnManager {
			get {
				if (_columnManager == null) {
					_columnManager = new EmailTableColumnManager();			
					_columnManager.AddGroupDataFields(GroupDataFields);
				}
				return (this._columnManager);
			}			
		}
		private string getPhysicalPath() {

            return Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + CustomerID + "/data");
		}

		private string trimQuotes(string strToTrim){
			string cleanString = "";
			if(strToTrim.Length > 0){
				if(strToTrim.Equals("''")){
					cleanString = "";
				}else if(strToTrim.Equals("\"\"")){
					cleanString = "";
				}else {
					cleanString = StringFunctions.TrimQuotes(strToTrim);
				}
			}
			return cleanString;
		}
		#endregion

		#region Other private methods
		private void getDatafromQueryString(){
			file = Request.QueryString["fn"];			//file name
			ftc	 = "html";											//format type code, always 'html'
			stc = "S";												//subscribe type code, always 'S'
			gid = Request.QueryString["gid"];		//Group ID, for now it's always 2302
			fileType = Request.QueryString["ft"];	//file type, either 'X' or 'C'
			sheetName = Request.QueryString["sheet"]; // "Sheet1";						//sheet name, always 'Sheet1'
			lineStart = "0";										//line start, always 0
		}

		private void print(string text){
			msglabel.Text += text;
			msglabel.Visible = true;
		}
		private void printerr(string text){
			errlabel.Text = text;
			errlabel.Visible = true;
			//btnImport.Visible = false;
		}
		#endregion


		private void btnImport_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			ImportData(sender, e);
		}

	}
}
