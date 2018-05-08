using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using ecn.showcare.webservice.Objects;

namespace ecn.showcare.webservice.KM {
	/// <summary>
	/// Webservice to push leads List 
	/// </summary>
	[WebService(
		 Namespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/PushLeadsList.asmx", 
		 Description="Provides Access to Push Leads list to ECN.<br>* Use setupGroup() to setup the Lists Group in ECN. <br>* Use pushLeadsToGroup() to Push Email lists in to the group that's Created.")
	] 
	public class PushLeadsList : System.Web.Services.WebService {
		public PushLeadsList() {
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		public string commDB{
			get{ return (ConfigurationManager.AppSettings["communicatordb"].ToString()); }
		}

		public string commConnString{
			get{ return (ConfigurationManager.AppSettings["com"].ToString()); }
		}

		private SortedList _UDFHash;
		public SortedList UDFHash{
			get{ return (this._UDFHash); }
			set{this._UDFHash = value; }
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

		#region Group setup
		[WebMethod(
			 Description="Provides Access to SetUp Groups (LeadsList) in ECN for Customer.<br>- Parameters passed are CustomerID & ListName.<br>- If the List does not exist it will Create a new Group for the List. <br>- Returns Integer GroupID value.")
		]
		public int setupGroup(int customerID, string listName){
			string sql = "SELECT GroupID FROM "+commDB+".dbo.Groups Where GroupName = '"+listName.Trim()+"' AND CustomerID = "+customerID;
			int groupID = 0;
			try{
				groupID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sql).ToString());
			}catch(Exception ex){ ex.ToString();}
			
			if(!(groupID > 0)){
				try{
					groupID = Convert.ToInt32(DataFunctions.ExecuteScalar("INSERT INTO " +commDB+ ".dbo.Groups (CustomerID , GroupName, OwnerTypeCode,MasterSupression,PublicFolder) values ("+customerID+",'"+listName+"','customer',0,0);SELECT @@IDENTITY").ToString());		
					
					string groupUDFs = ConfigurationManager.AppSettings["GroupUDFs"].ToString();
					try{
						StringTokenizer st = new StringTokenizer(groupUDFs,',');
						while(st.HasMoreTokens()){
							string currentToken = st.NextToken().Trim().ToString();
							DataFunctions.ExecuteScalar("INSERT INTO " +commDB+ ".dbo.GroupDataFields (GroupID , ShortName, LongName) VALUES ("+groupID+",'"+currentToken+"','"+currentToken+"')");		
						}
					}catch(Exception){ }
				}catch(Exception){ }
			}

			return groupID;
		}
		#endregion

		#region GroupList Populate
		[WebMethod(
			 Description="Provides Access to Push Email Profiles in to Groups (LeadsList) in ECN for Customer. <br>- Parameters passed are CustomerID, GroupID, LeadsDataSet<br>- If an Email Profile does not exist it will Create a new Email Profile in the Group. If it exists it will Update the Profile with the current Data.<br>- Returns Integer total number of records processed (# of records Inserted + Updated) GroupID value.")
		]
		public int pushLeadsToGroup(int customerID, int GroupID, DataSet leadsListDS){
			Groups currentGroup = new Groups(GroupID);
			_UDFHash = currentGroup.UDFHash;
			DataTable leadsListDT = leadsListDS.Tables[0];
			DataRow[] leadsListDTRow = leadsListDT.Select();
			int totalRecords = leadsListDTRow.Length;
			int totalRecordsProcessed = 0;
			if(leadsListDTRow.Length > 0){
				totalRecordsProcessed = InsertProfilesToECNGroup(leadsListDTRow, currentGroup, customerID.ToString());
			}

			return totalRecordsProcessed;
		}
		#endregion

		#region Insert data in to ECN Profile.
		public int InsertProfilesToECNGroup(DataRow[] currentDataSetRow, Groups currentGroup, string currentCustomerID){
			int insertCount = 0, updateCount = 0, count=0;

			try{

				foreach(DataRow currentRow in currentDataSetRow){
					count ++;

					string currentEmailAddress = currentRow["EmailAddress"].ToString().Trim(); 
					string subscriptionType = "S";
					try{
						if(currentRow["SubscriptionType"].ToString().Trim().Equals("Y")){ subscriptionType = "U"; }
					}catch(Exception){ subscriptionType = "S";	}

					string fullName = "";
					try{
						fullName = currentRow["FirstName"].ToString().Trim()+" "+currentRow["LastName"].ToString().Trim();
					}catch(Exception){}

					string voice = "";
					try{
						if(currentRow["Phone"].ToString().Trim().Length > 0){
							voice = currentRow["Phone"].ToString().Trim();
						}
					}catch(Exception){}

					Emails currentEmail = currentGroup.WhatEmailForCustomer(currentEmailAddress);

					int EmailID;
					SqlDateTime bd		= new SqlDateTime();
					SqlDateTime uedt1	= new SqlDateTime();
					SqlDateTime uedt2	= new SqlDateTime();
					SqlDateTime sqldatenull = SqlDateTime.Null;	

					if (null == currentEmail) {
						try{
							// No other Email Exists with the current EmailAddress go ahead & Insert
							SqlConnection db_connection = new SqlConnection(commConnString);
							SqlCommand InsertCommand = new SqlCommand(null,db_connection);
							InsertCommand.CommandText =
								"INSERT INTO Emails "+ 
								"(EmailAddress,CustomerID,Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,City,State,Zip,Country,Voice,Mobile,Fax,"+							"Website,Age,Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,"+
								"DateAdded, DateUpdated)" +
								" VALUES " +
								"(@emailAddress,@customer_id,@title,@first_name,@last_name,@full_name,@company,@occupation,@address,@address2,@city,@state,@zip,"+							"@country,@voice,@mobile,@fax,@website,@age,@income,@gender,@user1,@user2,@user3,@user4,@user5,@user6,@birthdate,"+
								"@user_event1,@user_event1_date,@user_event2,@user_event2_date,@notes,@DateAdded,@DateUpdated) SELECT @@IDENTITY";
					
							InsertCommand.Parameters.Add ("@emailAddress", SqlDbType.VarChar,250).Value = currentEmailAddress;  
							InsertCommand.Parameters.Add ("@customer_id", SqlDbType.Int,4).Value = currentCustomerID;
							InsertCommand.Parameters.Add ("@title",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@first_name",SqlDbType.VarChar,50).Value = currentRow["FirstName"].ToString().Trim(); 
							InsertCommand.Parameters.Add ("@last_name",SqlDbType.VarChar,50).Value = currentRow["LastName"].ToString().Trim(); 
							InsertCommand.Parameters.Add ("@full_name",SqlDbType.VarChar,50).Value = fullName;
							InsertCommand.Parameters.Add ("@company",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@occupation",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@address",SqlDbType.VarChar,255).Value = "";
							InsertCommand.Parameters.Add ("@address2",SqlDbType.VarChar,255).Value = "";
							InsertCommand.Parameters.Add ("@city",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@state",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@zip",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@country",SqlDbType.VarChar,50).Value ="USA";
							InsertCommand.Parameters.Add ("@voice",SqlDbType.VarChar,50).Value = voice;
							InsertCommand.Parameters.Add ("@mobile",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@fax",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@website",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@age",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@income",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@gender",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@user1",SqlDbType.VarChar,255).Value = "";
							InsertCommand.Parameters.Add ("@user2",SqlDbType.VarChar,255).Value = "";
							InsertCommand.Parameters.Add ("@user3",SqlDbType.VarChar,255).Value = "";
							InsertCommand.Parameters.Add ("@user4",SqlDbType.VarChar,255).Value = "";
							InsertCommand.Parameters.Add ("@user5",SqlDbType.VarChar,255).Value = "";
							InsertCommand.Parameters.Add ("@user6",SqlDbType.VarChar,255).Value = "";
							try{
								bd=DateTime.Parse("");
							}catch(Exception){ bd = sqldatenull;}
							try{
								uedt1=DateTime.Parse("");
							}catch(Exception){ uedt1 = sqldatenull; }
							try{
								uedt2=DateTime.Parse("");
							}catch(Exception){ uedt2 = sqldatenull; }
							InsertCommand.Parameters.Add ("@birthdate",SqlDbType.DateTime).Value = bd;
							InsertCommand.Parameters.Add ("@user_event1",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@user_event1_date",SqlDbType.DateTime).Value = uedt1;
							InsertCommand.Parameters.Add ("@user_event2",SqlDbType.VarChar,50).Value = "";
							InsertCommand.Parameters.Add ("@user_event2_date",SqlDbType.DateTime).Value =uedt2; 
							InsertCommand.Parameters.Add ("@notes",SqlDbType.Text).Value = "Subscription through ShowCare WebService. DateAdded: "+DateTime.Now.ToString(); 
							InsertCommand.Parameters.Add ("@DateAdded",SqlDbType.DateTime).Value =DateTime.Now.ToString(); 
							InsertCommand.Parameters.Add ("@DateUpdated",SqlDbType.DateTime).Value =DateTime.Now.ToString(); 

							//InsertCommand.CommandTimeout = 0;
							db_connection.Open();
							EmailID = Convert.ToInt32(InsertCommand.ExecuteScalar());
							db_connection.Close();
							currentEmail= new Emails(EmailID);

							currentGroup.AttachEmail(currentEmail, "html", subscriptionType);
							insertCount++;
						}catch(Exception ex){
							ex.ToString();
						}
					}else{
						//This is an update !!!
						try{
							SqlConnection db_connection = new SqlConnection(commConnString);
							SqlCommand UpdateCommand = new SqlCommand(null,db_connection);
							UpdateCommand.CommandText = "UPDATE Emails SET " + 
								" EmailAddress=@emailAddress, Title = @title, FirstName = @first_name, LastName = @last_name, FullName = @full_name, Company = @company,"+
								" Occupation = @occupation, Address = @address, Address2 = @address2, City = @city, State = @state, Zip = @zip, Country = @country, "+
								" Voice = @voice, Mobile = @mobile, Fax = @fax, Website = @website, Age = @age, Income = @income, Gender = @gender, "+
								" User1 = @user1, User2 = @user2, User3 = @user3, User4 = @user4, User5 = @user5, User6 = @user6, Birthdate = @birthdate, "+
								" UserEvent1 = @user_event1, UserEvent1Date = @user_event1_date, " +
								" UserEvent2 = @user_event2, UserEvent2Date = @user_event2_date " +
								" WHERE EmailID = @email_id;";
							UpdateCommand.Parameters.Add ("@email_id", SqlDbType.VarChar,250).Value = currentEmail.ID(); 
							UpdateCommand.Parameters.Add ("@emailAddress", SqlDbType.VarChar,250).Value = currentEmailAddress;  
							UpdateCommand.Parameters.Add ("@customer_id", SqlDbType.Int,4).Value = currentCustomerID;
							UpdateCommand.Parameters.Add ("@title",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@first_name",SqlDbType.VarChar,50).Value = currentRow["FirstName"].ToString().Trim(); 
							UpdateCommand.Parameters.Add ("@last_name",SqlDbType.VarChar,50).Value = currentRow["LastName"].ToString().Trim(); 
							UpdateCommand.Parameters.Add ("@full_name",SqlDbType.VarChar,50).Value = currentRow["FirstName"].ToString().Trim()+" "+currentRow["LastName"].ToString().Trim();
							UpdateCommand.Parameters.Add ("@company",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@occupation",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@address",SqlDbType.VarChar,255).Value = "";
							UpdateCommand.Parameters.Add ("@address2",SqlDbType.VarChar,255).Value = "";
							UpdateCommand.Parameters.Add ("@city",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@state",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@zip",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@country",SqlDbType.VarChar,50).Value ="USA";
							UpdateCommand.Parameters.Add ("@voice",SqlDbType.VarChar,50).Value = voice;
							UpdateCommand.Parameters.Add ("@mobile",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@fax",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@website",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@age",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@income",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@gender",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@user1",SqlDbType.VarChar,255).Value = "";
							UpdateCommand.Parameters.Add ("@user2",SqlDbType.VarChar,255).Value = "";
							UpdateCommand.Parameters.Add ("@user3",SqlDbType.VarChar,255).Value = "";
							UpdateCommand.Parameters.Add ("@user4",SqlDbType.VarChar,255).Value = "";
							UpdateCommand.Parameters.Add ("@user5",SqlDbType.VarChar,255).Value = "";
							UpdateCommand.Parameters.Add ("@user6",SqlDbType.VarChar,255).Value = "";
							try{
								bd=DateTime.Parse("");
							}catch(Exception){ bd = sqldatenull;}
							try{
								uedt1=DateTime.Parse("");
							}catch(Exception){ uedt1 = sqldatenull; }
							try{
								uedt2=DateTime.Parse("");
							}catch(Exception){ uedt2 = sqldatenull; }
							UpdateCommand.Parameters.Add ("@birthdate",SqlDbType.DateTime).Value = bd;
							UpdateCommand.Parameters.Add ("@user_event1",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@user_event1_date",SqlDbType.DateTime).Value = uedt1;
							UpdateCommand.Parameters.Add ("@user_event2",SqlDbType.VarChar,50).Value = "";
							UpdateCommand.Parameters.Add ("@user_event2_date",SqlDbType.DateTime).Value =uedt2; 
							UpdateCommand.Parameters.Add ("@notes",SqlDbType.Text).Value = "Subscription through NEBookStore Conversion Job. DateUpdated: "+DateTime.Now.ToString(); 
							UpdateCommand.Parameters.Add ("@DateAdded",SqlDbType.DateTime).Value =DateTime.Now.ToString(); 
							UpdateCommand.Parameters.Add ("@DateUpdated",SqlDbType.DateTime).Value =DateTime.Now.ToString(); 
							db_connection.Open();
							UpdateCommand.ExecuteNonQuery();
							db_connection.Close();

							currentGroup.AttachEmail(currentEmail, "html", subscriptionType);
							updateCount++;
						}catch(Exception){	}
					}

					// The following part is for the UDF's Insert & Update.. 
					ArrayList _keyArrayList = new ArrayList();
					ArrayList _UDFData = new ArrayList();

					if(UDFHash.Count > 0){
						IDictionaryEnumerator UDFHashEnumerator = UDFHash.GetEnumerator();
						while (UDFHashEnumerator.MoveNext()){
							string UDFData = "";
							string _value	= "user_"+UDFHashEnumerator.Value.ToString();
							string _key		= UDFHashEnumerator.Key.ToString();
							UDFData = currentRow[_value].ToString().Trim();
							_keyArrayList.Add(_key);
							_UDFData.Add(UDFData);
						}

						for(int i=0; i < _UDFData.Count;i++){
							currentGroup.AttachUDFToEmail(currentEmail,_keyArrayList[i].ToString(), _UDFData[i].ToString());	
						}
					}
					//----- End UDF Insert & Update.. 
				}
			}catch(Exception){

			}

			return (insertCount+updateCount);
		}
		#endregion
	}
}
