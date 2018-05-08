using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Services;
using ecn.showcare.webservice.Objects;

namespace ecn.showcare.webservice {
	/// <summary>
	/// WebService to Create ShowCare Accounts Need to set Values for 
	/// CustomerInformation, UserInformation.
	/// </summary>
	[WebService(
		 Namespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/AccountCreation.asmx", 
		 Description="Provides Access to Create ShowCare Accounts in ECN<br>* Use setCustomerContactInformation() to set the values for the Contact Object which is the Contact Information for the new Customer.<br>* Use setupCustomerAccount() to create the Customer Account in ECN.<br>* Use setupUserAccount() to create the User Accounts for the new Customer.")
	]

	public class AccountCreation : System.Web.Services.WebService {
		private Contact _customerContactInfo;
		public Contact customerContactInfo{
			get{ return (this._customerContactInfo); }
			set{this._customerContactInfo = value; }
		}

		private Customer _customerInfo;
		public Customer customerInfo{
			get{ return (this._customerInfo); }
			set{this._customerInfo = value; }
		}

		private bool _isNewCustomerCreated;
		public bool isNewCustomerCreated{
			get{ return (this._isNewCustomerCreated); }
			set{this._isNewCustomerCreated = value; }
		}

		public int baseChannelID{
			get{ return ( Convert.ToInt32(ConfigurationManager.AppSettings["baseChannelID"].ToString()) ); }
		}

		public AccountCreation() {
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
		private void InitializeComponent() {
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if(disposing && components != null) {
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region Set CustomerContact, Customer Information & Create new Customer / Update Customer
		[WebMethod(
			 Description="Provides Access to Set Values for a Customer Contact Information in ECN.<br>- Parameters passed are First & LastName, Address, phone etc.<br>- Returns the Contact Object")
		]
		public Contact setCustomerContactInformation(	string customerContactFirstName, string customerContactLastName, string customerContactAddress, string customerContactCity, string customerContactState, string customerContactCountry, string customerContactZip, string customerContactPhone, string customerContactFax, string customerContactEmailAddress, string customerContactUID) {
			Contact customerContact = new Contact("",
																		customerContactFirstName.ToString().Trim(),
																		customerContactLastName.ToString().Trim(),
																		customerContactUID.ToString().Trim(),
																		customerContactPhone.ToString().Trim(),
																		customerContactFax.ToString().Trim(),
																		customerContactEmailAddress.ToString().Trim(),
																		customerContactAddress.ToString().Trim(),
																		customerContactCity.ToString().Trim(),
																		customerContactState.ToString().Trim(),
																		customerContactCountry.ToString().Trim(),
																		customerContactZip.ToString().Trim()
																	);
			_customerContactInfo = customerContact;
			return customerContact;
		}

		[WebMethod(
			 Description="Provides Access to SetUp Customer Account in ECN.<br>- Parameters passed are Contact Object, CustomerName<br>- Will Create a new Customer Account if it doesn't Exist. Will Update if it exists.<br>- Returns Integer CustomerID value.")
		]
		public int setupCustomerAccount(Contact customerContactInformation, string customerName) {
			string existingCustomerID = "0";
			Customer currentCustomer = null;
			try{
				string sql = "SELECT CustomerID FROM Customer WHERE BaseChannelID = "+baseChannelID+" AND ContactTitle = '"+customerContactInformation.ContactTitle.ToString().Trim()+"'";
				existingCustomerID = DataFunctions.ExecuteScalar(sql).ToString();
			}catch(Exception ex){ ex.ToString();}

			if(Convert.ToInt32(existingCustomerID) > 0){
				currentCustomer = new Customer(Convert.ToInt32(existingCustomerID), baseChannelID);
				_isNewCustomerCreated = false;
			}else{
				currentCustomer = new Customer(-1, baseChannelID);
				_isNewCustomerCreated = true;
			}

			currentCustomer.Name = customerName;
			currentCustomer.GeneralContact = customerContactInformation;
			currentCustomer.BillingContact = customerContactInformation;
			currentCustomer.IsActive = true;
			currentCustomer.WebAddress = "";
			currentCustomer.TechContact = "";
			currentCustomer.TechEmail = "";
			currentCustomer.TechPhone = "";
			currentCustomer.SubscriptionsEmail = "subscriptions@bounce2.com";
			
			currentCustomer.CreatorLevel			= 0;
			currentCustomer.CreatorChannelID	= 0;
						
			currentCustomer.CollectorLevel		= 0;
			currentCustomer.CollectorChannelID= 0;
						
			currentCustomer.CommunicatorLevel		= 3;
			int commChannelID = 0;
			try{
				commChannelID = Convert.ToInt32(DataFunctions.ExecuteScalar("accounts",	"SELECT ChannelID FROM Channel WHERE BaseChannelID = "+baseChannelID+" AND ChannelTypeCode = 'communicator'").ToString());
			}catch(Exception){}
			currentCustomer.CommunicatorChannelID	= commChannelID;

			currentCustomer.AccountLevel = 0;			

			currentCustomer = saveCustomerInformation(currentCustomer);

			_customerInfo = currentCustomer;
			return currentCustomer.ID;
		}

		public Customer saveCustomerInformation(Customer currentCustomer){
			currentCustomer.Save();	
			if(isNewCustomerCreated){
				//Create Assets Path.
				CreateAssetPaths(baseChannelID, currentCustomer.ID, Server);
				//currentCustomer.CreateAssertPaths(Server);

				//Create Default Features for Customer.
				currentCustomer.CreateDefaultFeatures();         
				
				//Activate Required Features.
				ActivateRequiredECNFeatures(currentCustomer.ID);
				
				//Create Default Roles.
				currentCustomer.CreateDefaulRole();          
				
				//Create Customer Licenses.
				CreateCustomerLicenses(currentCustomer.ID);

				//Create Customer Licenses.
				CreateCustomerTemplates(currentCustomer.ID);
				
				//Create Master Supp Group.
				currentCustomer.CreateMasterSupressionGroup();
				
				//Create Master List Group.
				//string masterListGroupID = CreateMasterListGroup(currentCustomer.ID);

				//if(Convert.ToInt32(masterListGroupID) > 0){
					//CreateUDFForMasterListGroup(masterListGroupID);
				//}
			}
            return currentCustomer;
		}
		#endregion

		#region Create / Check User account for Customer
		#region VIEW commented code
		/*[WebMethod(
			 Description="Provides Access to Create User for the Customer in ECN. Returns Integer UserID.")
		]
		public int createUser(string userName, string password, Customer customerID){
			User newUser = null;
			Customer currentCustomer = Customer.GetCustomerByID(customerID.ID);
			//Create New User for Customer
			try{
				newUser = new User(userName, password, currentCustomer);
				if(!(newUser.Exists)){
					newUser.AccountsOptions="000000";
					newUser.Save();
				}else{

				}
			}catch(Exception ex){
				//appLog.writeToLog("Exception in Creating Default User for Customer");
			}
			
			//Set Default UserActions for the newly Created User
			string userActionsAllowed = ConfigurationManager.AppSettings["AllowedUserActions"].ToString();
			try{
				DataFunctions.ExecuteScalar("UPDATE UserActions SET Active = 'N' WHERE ActionID NOT IN ("+userActionsAllowed+") AND UserID = "+newUser.ID);
			}catch(Exception ex){
				//appLog.writeToLog("Exception in Default Action Roles for User : "+ex.ToString());			
			}

			return newUser.ID;
		}

		[WebMethod(
			 Description="Provides Access to get User Account for the Customer in ECN. Returns the Integer UserID.")
		]
		public int getUser(string userName, string password, Customer customerID){
			User newUser = null;
			Customer currentCustomer = Customer.GetCustomerByID(customerID.ID);
			//Create New User for Customer
			try{
				newUser = new User(userName, password, currentCustomer);
			}catch(Exception ex){
				//appLog.writeToLog("Exception in Creating Default User for Customer");
			}
			
			//Set Default UserActions for the newly Created User
			string userActionsAllowed = ConfigurationManager.AppSettings["AllowedUserActions"].ToString();
			try{
				DataFunctions.ExecuteScalar("UPDATE UserActions SET Active = 'N' WHERE ActionID NOT IN ("+userActionsAllowed+") AND UserID = "+newUser.ID);
			}catch(Exception ex){
				//appLog.writeToLog("Exception in Default Action Roles for User : "+ex.ToString());			
			}

			return newUser.ID;
		}*/
		#endregion

		[WebMethod(
			 Description="Provides Access to SetUp User Account in ECN for a Customer.<br>- Parameters passed are UserName, Password, CustomerID<br>- Will Create a new User Account if it doesn't Exist.<br>- Returns Integer UserID value.")
		]
		public int setupUserAccount(string userName, string password, int customerID){
			User newUser = null;
			int userID = 0;
			Customer currentCustomer = new Customer(customerID, baseChannelID);
			try{
				newUser = new User(userName, password, currentCustomer);
				if(!(newUser.Exists)){
					newUser.AccountsOptions="000000";
					newUser.Save();
					userID = newUser.ID;
				}else{
					userID = newUser.getUserID();
				}
				//Set Default UserActions for the newly Created User
				string userActionsAllowed = ConfigurationManager.AppSettings["AllowedUserActions"].ToString();
				try{
					DataFunctions.ExecuteScalar("UPDATE UserActions SET Active = 'N' WHERE ActionID NOT IN ("+userActionsAllowed+") AND UserID = "+userID);
				}catch(Exception){
					//appLog.writeToLog("Exception in Default Action Roles for User : "+ex.ToString());			
				}
			}catch(Exception){

			}
			return userID;
		}

		#endregion

		#region Create Customer AssetPaths
		public static void CreateAssetPaths(int BaseChannelID, int CustomerID, HttpServerUtility server){
			ChannelCheck cc = new ChannelCheck();
			string assetsPath = cc.getAssetsPath("accounts");
			try{
				string dataPath	= assetsPath + "/channelID_"+ BaseChannelID.ToString() +"/customers/" + CustomerID.ToString() + "/data";  
				DirectoryInfo dataDirInfo	= new DirectoryInfo(server.MapPath(dataPath));
				dataDirInfo.Create();

				string imagePath = assetsPath + "/channelID_"+ BaseChannelID.ToString() +"/customers/" + CustomerID.ToString() + "/images";  
				DirectoryInfo imageDirInfo	= new DirectoryInfo(server.MapPath(imagePath));
				imageDirInfo.Create();
			}catch(Exception ex){
				ex.ToString();
			}
		}
		#endregion

		#region Activate Required ECN Features
		public static void ActivateRequiredECNFeatures(int CustomerID){
			string ecnFeatures = ConfigurationManager.AppSettings["ECNFeaturesAllowed"].ToString();
			try{
                DataFunctions.ExecuteScalar("UPDATE CustomerProduct SET Active = 'y' WHERE ProductDetailID IN (" + ecnFeatures + ")");
			}catch(Exception){
				//appLog.writeToLog("Exception in Activating Required ECNFeatures for Customer : "+ex.ToString());					
			}
		}
		#endregion

		#region Create ECNCustomer License & SLA
		public static void CreateCustomerLicenses(int customerID){
			string licenseList = ConfigurationManager.AppSettings["ECNCustomerLicenseCodes"].ToString();
			string communicator_db = ConfigurationManager.AppSettings["communicatordb"];
			try{
				StringTokenizer st = new StringTokenizer(licenseList,',');
				while(st.HasMoreTokens()){
					string currentToken	= st.NextToken().Trim().ToString();
					string licenseQty		= ConfigurationManager.AppSettings["ECNCustomerLicenseQTY_"+currentToken].ToString();
					string expiry			= DateTime.Now.AddYears(1).ToString();
					DataFunctions.ExecuteScalar("INSERT INTO CustomerLicense (CustomerID , QuoteItemID, LicenseTypeCode, LicenseLevel, Quantity, Used, ExpirationDate, AddDate, IsActive) VALUES ("+customerID+",'-1','"+currentToken+"','CUST','"+licenseQty+"','0','"+expiry+"','"+DateTime.Now.ToString()+"','1')");		
				}
			}catch(Exception){
				//appLog.writeToLog("Exception in Creating UDF For MasterList Group for Customer : "+ex.ToString());					
			}
		}
		#endregion

		#region Create ECNCustomer Templates F2F etc.,
		public static void CreateCustomerTemplates(int customerID) {
			string F2FLandingPgHdr	= "<table width=750 border=0 cellspacing=0 cellpadding=0><tr><td><img src=http://emarketing.showlead.com/wizard/images/header.gif></td></tr></table>";
			string F2FLandingPgFtr	= "";
			string F2FIntroEmailHdr	= "<font face=verdana size=2>Hi %%FullName%%,<br>  %%Notes%%<br>  You have been forwarded this email from %%email_friend%%.</font>";
			string sqlquery = "";
			try {
				sqlquery = 
				" INSERT INTO CustomerTemplate ( "+
					" CustomerID, TemplateTypeCode, HeaderSource, FooterSource, ActiveFlag, ModifyDate "+
					" ) VALUES ( "+
					customerID+", 'F2FLandingPgHdr', '"+F2FLandingPgHdr+"', '', 'Y', '"+DateTime.Now.ToString()+"' "+
					") ";
				DataFunctions.Execute(sqlquery);
			}
			catch(Exception) 	{}

			try {
				sqlquery = 
					" INSERT INTO CustomerTemplate ( "+
					" CustomerID, TemplateTypeCode, HeaderSource, FooterSource, ActiveFlag, ModifyDate "+
					" ) VALUES ( "+
					customerID+", 'F2FLandingPgFtr', '"+F2FLandingPgFtr+"', '', 'Y', '"+DateTime.Now.ToString()+"' "+
					") ";
				DataFunctions.Execute(sqlquery);
			}
			catch(Exception) 	{}

			try {
				sqlquery = 
					" INSERT INTO CustomerTemplate ( "+
					" CustomerID, TemplateTypeCode, HeaderSource, FooterSource, ActiveFlag, ModifyDate "+
					" ) VALUES ( "+
					customerID+", 'F2FIntroEmailHdr', '"+F2FIntroEmailHdr+"', '', 'Y', '"+DateTime.Now.ToString()+"' "+
					") ";
				DataFunctions.Execute(sqlquery);
			}
			catch(Exception) 	{}

			
		}
		#endregion

		#region Create MasterListGroup for Customer
		public static string CreateMasterListGroup(int CustomerID)
		{
			string masterListID = "0";
			try{
				string communicator_db = ConfigurationManager.AppSettings["communicatordb"];
				string masterList = ConfigurationManager.AppSettings["MasterGroupName"];
				masterListID = DataFunctions.ExecuteScalar("INSERT INTO " +communicator_db+ ".dbo.Groups (CustomerID , GroupName, OwnerTypeCode,MasterSupression,PublicFolder) values (" + CustomerID + ",'"+masterList+"','customer',0,0);SELECT @@IDENTITY").ToString();		
			}catch(Exception){
				//appLog.writeToLog("Exception in Creating MasterList Group for Customer : "+ex.ToString());					
			}
			return masterListID;
		}
		#endregion
	}
}
