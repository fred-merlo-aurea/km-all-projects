using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using ecn.showcare.wizard.scAccountCreation;
using ecn.showcare.wizard.scPushLeadsList;
using ecn.showcare.wizard.scProcessLogin;

namespace showcareWizardTest
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class WebForm1 : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Create AccountCreation Object
			/*AccountCreation ac = new AccountCreation();
			int newCustomerID = 0;
			int newUserID = 0;

			//Set Contact Information save it as Contact Object
			//Contact newContactInfo = ac.setCustomerContactInformation("ashok","palani","test Add", "Edina", "MN","USA","55435","111-111-1111","222-222-2222","ashanaa@hotmail.com","qrertvtts");
			Contact newContactInfo= new Contact();
			newContactInfo.ContactName = "Ashok" + " " + "Palaniswamy";
			newContactInfo.FirstName = "Ashok";
			newContactInfo.LastName = "Palaniswamy";
			newContactInfo.ContactTitle = "adwtrydwss";	//This is the UNIQUE ID for the Customer. 
			newContactInfo.StreetAddress = "14505 21st ave N #210 ";
			newContactInfo.City = "Plymouth";
			newContactInfo.State = "MN";
			newContactInfo.Country = "USA";
			newContactInfo.Zip = "55447";
			newContactInfo.Phone = "763-111-1111"; 
			newContactInfo.Fax = "763-222-2222";
			newContactInfo.Email = "ashok@showcare.com";

			//Create Customer account passing Contact Object & Customer Name
			newCustomerID = ac.setupCustomerAccount(newContactInfo,"ShowCare Group of COMPANIES- TESTING");

			if(newCustomerID > 0){
				//Check if User Exists passing UserName, Password & the CustomerID
				newUserID = ac.setupUserAccount("ashokuser2@showcare.com", "1111", newCustomerID);
			}

			PushLeadsList psl = new PushLeadsList();
			int groupID = psl.setupGroup(newCustomerID, "NEW SC EVENT");

			int total = 0;
			if(groupID > 0){
				DataSet ds = new DataSet();
				string sql = "select EmailAddress, FirstName, LastName, Voice as 'Phone', '11111' as 'user_BadgeID', "+
" 'Show Care Event1' as 'user_EventName', eg.SubscribeTypeCode as 'SubscriptionType' "+
" from Emails e join Emailgroups eg on e.emailID = eg.emailID "+
" where eg.groupID = 115 ";
				SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["com"].ToString());
				SqlDataAdapter adapter = new SqlDataAdapter(sql,conn);
				conn.Open();
				adapter.Fill(ds,"DataTable");
				conn.Close();

				total = psl.pushLeadsToGroup(newCustomerID, groupID, ds);
				
			}*/

			//Get Access Key for the User & Customer
			string accessKey = ""; 
			ProcessLogin login = new ProcessLogin(); //http://localhost/ecn.showcare.wizard/Web References/scPushLeadsList/
			
			//accessKey = login.setupLogin(newCustomerID,newUserID);
			accessKey = login.setupLogin(1954,2375);

			//get the AutoLoginURL & pass the accessKey as a QueryStrig Param with the URL
			if(accessKey.Length > 0){
				//Response.Redirect(login.getAutoLoginURL()+accessKey+"&groupID="+groupID+"&redirect=campaign",true);
				string redirURL = login.getAutoLoginURL()+Server.UrlEncode(accessKey)+"&groupID=12019&redirect=campaign";
				Response.Redirect(redirURL,true);
			}

			/*Response.Write("Customer Created CustID: "+newCustomerID);
			Response.Write("<br><br>User UserID: "+newUserID);
			Response.Write("<br><br>GroupID: "+groupID);
			Response.Write("<br><br>Total Inserted: "+total);
			Response.Write("<br><br>AcessKey: "+accessKey);*/
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
