using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

namespace ecn.wizard.webservice.KM {
	[WebService(
		 Namespace="http://wizard.ecn5.com/webservice/KM/ContentsManager.asmx", 
		 Description="Provides Access to Manage Contents & Messages in ECN.<br>* Use createContent() to create a new Content in your account.")
	] 
	public class ContentsManager : System.Web.Services.WebService {
		public ContentsManager() {
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region Login Setup
		[WebMethod(
			 Description="Provides Access to Create a piece of Content in ECN under your account. This content will be used in a message to create a Message Campaign that is sent out.<br>- Parameters passed are CustomerID UserID ContentTitle & ContentSource.<br>- Returns positive Integer ContentID value created in ECN.<br>- If ContentID is negative (-ve) OR zero (0) a Content was not successfully created.")
		]
		public int createContent(int customerID, int userID, string contentTitle, string contentSource){
			//just incase if they don't wrap it with a table..
			contentSource = "<TABLE BORDER=0 WIDTH=100%><TR><TD>"+contentSource+"</TD></TR></TABLE>";
			int contentID = 0;
			string connString = ConfigurationManager.AppSettings["com"];
			if(contentTitle.Trim().ToString().Length < 1){
				contentTitle = "CONTENT_"+DateTime.Now.ToString("yyyyMMdd-HH:mm:ss");
			}
			string contentText = StripTextFromHtml(contentSource);
			string contentInsertSql = @" INSERT INTO Content ( "+
				" ContentTitle, ContentTypeCode, LockedFlag, UserID, FolderID, "+
				" ContentSource, ContentText, ContentURL, ContentFilePointer, "+
				" CustomerID, ModifyDate, Sharing "+
				" ) VALUES ( "+
				" @ContentTitle, @ContentTypeCode, @LockedFlag,@UserID,@FolderID, "+
				" @ContentSource,@ContentText, @ContentURL, @ContentFilePointer, "+
				" @CustomerID,@ModifyDate, @Sharing);SELECT @@IDENTITY;";
			
			SqlConnection actConn = new SqlConnection(connString);
			SqlCommand contentInsertCmd = new SqlCommand(contentInsertSql, actConn);
			contentInsertCmd.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userID;
			contentInsertCmd.Parameters.Add("@FolderID", SqlDbType.Int, 4).Value = 0;
			contentInsertCmd.Parameters.Add("@LockedFlag", SqlDbType.VarChar, 1).Value = "Y";
			contentInsertCmd.Parameters.Add("@ContentSource", SqlDbType.Text).Value = contentSource;
			contentInsertCmd.Parameters.Add("@ContentText", SqlDbType.Text).Value = contentText;
			contentInsertCmd.Parameters.Add("@ContentTypeCode", SqlDbType.VarChar, 50).Value = "html";
			contentInsertCmd.Parameters.Add("@contentTitle", SqlDbType.VarChar, 255).Value = contentTitle;
			contentInsertCmd.Parameters.Add("@CustomerID", SqlDbType.Int, 4).Value = customerID;
			contentInsertCmd.Parameters.Add("@ContentURL", SqlDbType.VarChar, 255).Value = "";
			contentInsertCmd.Parameters.Add("@ContentFilePointer", SqlDbType.VarChar, 255).Value = "";
			contentInsertCmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DateTime.Now;
			contentInsertCmd.Parameters.Add("@Sharing", SqlDbType.Char, 1).Value = "N";
			try {
				actConn.Open();
				contentID = Convert.ToInt32(contentInsertCmd.ExecuteScalar().ToString());
				actConn.Close();
			} finally {
				contentInsertCmd.Dispose();
				actConn.Close();
			}

			return contentID;
		}
		#endregion

		private string StripTextFromHtml(string html){
			string strOutput = "";
			try{
				Regex regExp = new Regex("<(.|\n)+?>");
				strOutput = regExp.Replace(html, "");

				strOutput = strOutput.Replace("&lt;", "<");
				strOutput = strOutput.Replace("&gt;", ">");
				strOutput = strOutput.Replace("&nbsp;", " ");
			}catch{ }

			return strOutput;
		}
	}
}
