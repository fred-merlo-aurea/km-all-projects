using System;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using ecn.wizard.Component;
using ecn.common.classes;
	//	========= Comment these when not using VeriSign ==========
	
using VeriSign.Payments.Common;				
using VeriSign.Payments.Common.Utility;
using VeriSign.Payments.DataObjects;
using VeriSign.Payments.Transactions;

namespace ecn.wizard.Component
{

	public class SaveWizard
	{
		Session WizardSession;

		private int _customerID = -1;
		private int _userID = -1;
		private int _channelID = -1;

		private string _cardHolderName;
		private string _cardType;
		private string _cardNumber;
		private string _cardExpMonth;
		private string _cardExpYear;
		private string _cardVerificationNumber;
		
		string crResponse;

		public string CardHolderName
		{
			get	{return _cardHolderName;}
			set { _cardHolderName = value; }
		}

		public string CardType
		{
			get	{return _cardType;}
			set { _cardType = value; }
		}

		public string CardNumber
		{
			get	{return _cardNumber;}
			set { _cardNumber = value; }
		}

		public string CardExpMonth
		{
			get	{return _cardExpMonth;}
			set { _cardExpMonth = value; }
		}

		public string CardExpYear
		{
			get	{return _cardExpYear;}
			set { _cardExpYear = value; }
		}

		public string CardVerificationNumber
		{
			get	{return _cardVerificationNumber;}
			set { _cardVerificationNumber = value; }
		}

		public SaveWizard(int ChannelID, int CustomerID, int UserID)
		{
			_channelID = ChannelID;
			_customerID = CustomerID;
			_userID = UserID;

			WizardSession = Session.GetCurrentSession();
		}

		public bool Save()
		{
			try
			{
				if (SaveContents() > 0)
				{										
					if (SaveLayout() > 0)
					{
						if (SaveBlast() > 0)
						{
							// Try to charge the credit card
							if (WizardSession.ProcessCC)
							{
								if (ProcessedCreditCard()) 
								{	
									// Means credit card was processed successfully
									// You are here means everything went just fine :)
									// Set current blast's status-code to [pending], since it seems like credit card has been charged successfully
									UpdateCurrentBlastTo("pending");

									// Send an Email receipt to the client
									SendMail();
								}
							}
							return true;
						} 
					}
				}
			}
			catch
			{
				throw;
			}
			return false;
		}


		/// <summary>
		/// Stores Contents created into the DB as a new record.
		/// A new record is created, every time customer creates new message, in the Cotnents Table
		/// </summary>
		/// <returns>
		/// Number:	upon successfull creation of Contents returns ContentID
		/// 0: upon failure from either PerformChecks() or if failed to create Contents
		/// </returns>
		private int SaveContents() 
		{
			int ContentID = WizardSession.ContentID;
			// Get the object saved at step1 and step2
			string csource = WizardSession.ContentSource;
			string ctext = DataFunctions.CleanString(StripTextFromHtml(WizardSession.ContentText));

			// Before saving the contents we need to add code-snippet and footer information
			if (WizardSession.Salutation.Equals("firstname")) 
			{
				csource = "<P align=left>Dear %%FirstName%%,</P>" + csource;
			} 
			else 
			{
				csource = "<P align=left>Dear %%FirstName%% %%LastName%%,</P>" + csource;
			}

			// In any case we need to insert Footer-Information in the end
			csource += "<BR><P align=left>"+
				WizardSession.FooterName+"<BR>"+
				WizardSession.FooterTitle+"<BR>"+
				WizardSession.FooterCompany+"<BR>"+
				WizardSession.FooterPhone+"<BR>";

			if (WizardSession.IsCustomHeader)
			{
				string headerImage = "";
				if (WizardSession.HeaderImage != string.Empty)
				{
					ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();

                    headerImage = "<IMG src='" + System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/Customers/" + _customerID + "/Images/" + WizardSession.HeaderImage + "' border=0>";
				}
				else
					headerImage = "&nbsp;";

				csource = "<TR><TD style='HEIGHT: 100px;border:0px solid #afc2d5;' valign='middle' ><center>" + headerImage + "</center></TD></TR><TR><TD style='PADDING-RIGHT: 30px; PADDING-LEFT: 30px; FONT-SIZE: 12px; PADDING-BOTTOM: 30px; PADDING-TOP: 30px'>" + csource + "</TD></TR>";
			}

			ctext = StripTextFromHtml(csource);

			string sqlquery = "";
			if (ContentID == -1) 
			{
				sqlquery = " INSERT INTO Content ( "+
					" ContentTitle, ContentTypeCode, LockedFlag, UserID, FolderID, "+
					" ContentSource, ContentText, CustomerID, ModifyDate, Sharing "+
					" ) VALUES ( @spcntitle,'html','N',@spuid,0,@spcsrc,@spctxt,@spcid,@spmdt,'N');SELECT @@IDENTITY";
			} 
			else 
			{
				sqlquery = "Update Content SET "+
					" ContentTitle = @spcntitle, ContentTypeCode = 'html', LockedFlag = 'N', UserID = @spuid, FolderID = 0, "+
					" ContentSource = @spcsrc, ContentText = @spctxt, CustomerID = @spcid, ModifyDate = @spmdt, Sharing = 'N' "+
					" WHERE ContentID = "+ ContentID;
			}

			// Create Connection
			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);	
			// Create Command
			SqlCommand com = new SqlCommand(sqlquery, con);

			// Declare Parameters
			SqlParameter spcntitle = new SqlParameter("@spcntitle", SqlDbType.VarChar, 255);
			spcntitle.Value = WizardSession.MessageTitle;
			SqlParameter spuid = new SqlParameter("@spuid", SqlDbType.Int);
			spuid.Value =  _userID ;
			SqlParameter spcsrc = new SqlParameter("@spcsrc", SqlDbType.Text);
			spcsrc.Value = csource;
			SqlParameter spctxt = new SqlParameter("@spctxt", SqlDbType.Text);
			spctxt.Value = ctext;
			SqlParameter spcid = new SqlParameter("@spcid", SqlDbType.Int);
			spcid.Value = _customerID;
			SqlParameter spmdt = new SqlParameter("@spmdt", SqlDbType.DateTime);
			spmdt.Value = System.DateTime.Now;

			// Add parameters to command
			com.Parameters.Add(spcntitle);	// content title
			com.Parameters.Add(spuid);		// user id
			com.Parameters.Add(spcsrc); 	// content  source
			com.Parameters.Add(spctxt);		// content text
			com.Parameters.Add(spcid);		// customer id
			com.Parameters.Add(spmdt);		// modify date

			try
			{
				con.Open();
				ContentID = Convert.ToInt32(com.ExecuteScalar());
			}
			catch(Exception err)
			{
				string msg = ConfigurationManager.AppSettings["createerrmsg"];
				msg = msg.Replace("%%errmsg%%", err.Message);
				throw new Exception("Error occured while creating contents.<BR>" + msg);
			} 
			finally 
			{
				con.Close();
			}
			if (WizardSession.ContentID == -1) 
				WizardSession.ContentID = ContentID;

			return WizardSession.ContentID;
		}


		/// <summary>
		/// Creates a New Message record
		/// </summary>
		/// <param name="cntid">ContentID associated with Selcted Template and Custoemr</param>
		/// <returns>
		/// Number:	If could create the record sccessfully, returns LayoutID
		/// 0:	If failed to create
		/// </returns>
		private int SaveLayout() 
		{
			int layoutID = WizardSession.LayoutID;

			string displayaddr = GetAddress(_customerID.ToString());					

			string sqlquery = "";
			if (layoutID == -1) 
			{
				sqlquery = 
					" INSERT INTO Layouts ( "+
					" LayoutName, CustomerID, UserID, FolderID, ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9,"+
					" TemplateID, ModifyDate, DisplayAddress "+
					" ) VALUES ( @splname,@spcid,@spuid,0,@spcntid,0,0,0,0,0,0,0,0,@sptid,@spmdt,@spdadr);SELECT @@IDENTITY";
			} 
			else 
			{
				sqlquery = "UPDATE Layouts SET "+
					" LayoutName =  @splname, CustomerID = @spcid, UserID = @spuid, FolderID = 0, ContentSlot1 = @spcntid, "+
					" ContentSlot2 = 0, ContentSlot3 = 0, ContentSlot4 = 0, ContentSlot5 = 0, ContentSlot6 = 0, ContentSlot7 = 0, "+
					" ContentSlot8 = 0, ContentSlot9 = 0, TemplateID = @sptid, ModifyDate = @spmdt, DisplayAddress = @spdadr "+
					" WHERE LayoutID = " + layoutID;
			}

			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
			SqlCommand com = new SqlCommand();
			com.Connection = con;
			com.CommandText = sqlquery;

			// Declare Parameters
			SqlParameter splname = new SqlParameter("@splname", SqlDbType.VarChar, 50);
			splname.Value = WizardSession.MessageTitle;
			SqlParameter spuid = new SqlParameter("@spuid", SqlDbType.Int);
			spuid.Value = _userID;//step1.UserID;
			SqlParameter spcid = new SqlParameter("@spcid", SqlDbType.Int);
			spcid.Value = _customerID;//step1.CustomerID;
			SqlParameter spcntid = new SqlParameter("@spcntid", SqlDbType.Int);
			spcntid.Value = WizardSession.ContentID;
			SqlParameter spmdt = new SqlParameter("@spmdt", SqlDbType.DateTime);
			spmdt.Value = System.DateTime.Now;
			SqlParameter spadr = new SqlParameter("@spdadr", SqlDbType.VarChar, 255);
			spadr.Value = displayaddr;
			SqlParameter sptid = new SqlParameter("@sptid", SqlDbType.Int);
			sptid.Value = WizardSession.TemplateID;

			com.Parameters.Add(splname);
			com.Parameters.Add(spcid);
			com.Parameters.Add(spuid);
			com.Parameters.Add(spcntid);
			com.Parameters.Add(sptid);
			com.Parameters.Add(spmdt);
			com.Parameters.Add(spadr);

			try
			{
				con.Open();
				layoutID = Convert.ToInt32(com.ExecuteScalar());
			}
			catch (Exception err)
			{
				string msg = ConfigurationManager.AppSettings["createerrmsg"];
				msg = msg.Replace("%%errmsg%%", err.Message);
				throw new Exception("Error occured while creating message.<BR>" + msg);
			} 
			finally 
			{
				con.Close();
			}
			if (WizardSession.LayoutID == -1) 
				WizardSession.LayoutID = layoutID;

			return WizardSession.LayoutID;
		}


		private int SaveBlast () 
		{
			int BlastID =  WizardSession.BlastID;
			string sql = "";
			int FilterID = 0;
			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
				
			if (WizardSession.SendSingle)
			{
				SqlCommand cmd	= new SqlCommand("sp_createFilterforSingleEmails",con);
				cmd.CommandTimeout = 0;
				cmd.CommandType	= CommandType.StoredProcedure;		
	
				cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.VarChar));
				cmd.Parameters["@CustomerID"].Value = _customerID;	
				cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar));
				cmd.Parameters["@UserID"].Value = _userID;	
				cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar));
				cmd.Parameters["@FirstName"].Value = WizardSession.FName;	
				cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar));
				cmd.Parameters["@LastName"].Value = WizardSession.LName;	
				cmd.Parameters.Add(new SqlParameter("@Emailaddress", SqlDbType.VarChar));
				cmd.Parameters["@Emailaddress"].Value = WizardSession.Email;	

				try
				{
					con.Open();
					FilterID = Convert.ToInt32(cmd.ExecuteScalar());
					con.Close();
				}
				catch
				{
				}
			}

			if (BlastID == -1) 
			{
				sql = "INSERT INTO Blasts "+
					"(CustomerID, UserID, EmailSubject, EmailFrom, EmailFromName, ReplyTo, CodeID, LayoutID, GroupID, RefBlastID, "+
					"BlastFrequency, TestBlast, SendTime, StatusCode, SpinLock, AttemptTotal, SendTotal, SuccessTotal, FilterID, BlastType) VALUES "+
					"(@cid, @uid, @es, @ef, @efn, @rt, @ci, @lid, @gid, @rbid, @bf, @tb, @st, @sc, @sl, @at, @sndt, @suct, @fid, @bt); SELECT @@IDENTITY;";
			} 
			else 
			{
				sql = "UPDATE Blasts SET "+
					" CustomerID = @cid, UserID = @uid, EmailSubject = @es, EmailFrom = @ef, EmailFromName = @efn, "+
					" ReplyTo = @rt, CodeID = @ci, LayoutID = @lid, GroupID = @gid, RefBlastID = @rbid, "+
					" BlastFrequency = @bf, TestBlast = @tb, SendTime = @st, StatusCode = @sc, SpinLock = @sl "+
					" WHERE BlastID = " + BlastID;
			}

			SqlCommand com = new SqlCommand(sql, con);

			SqlParameter spcid = new SqlParameter("@cid", _customerID);
			SqlParameter spuid = new SqlParameter("@uid", _userID);
			SqlParameter spes = new SqlParameter("@es", WizardSession.EmailSubject);
			SqlParameter spef = new SqlParameter("@ef", WizardSession.EmailAddress);
			SqlParameter spefn = new SqlParameter("@efn", WizardSession.Name);
			SqlParameter sprt = new SqlParameter("@rt", WizardSession.EmailAddress);
			SqlParameter spci = new SqlParameter("@ci", "0");
			SqlParameter splid = new SqlParameter("@lid", WizardSession.LayoutID);
			SqlParameter spgid = new SqlParameter("@gid", WizardSession.GroupID);
			SqlParameter sprbid = new SqlParameter("@rbid", "-1");
			SqlParameter spbf = new SqlParameter("@bf", "ONETIME");
			SqlParameter sptb = new SqlParameter("@tb", "n");
			SqlParameter spst = new SqlParameter("@st", DateTime.Now.ToString());
			SqlParameter spsc;

			if (WizardSession.ProcessCC)
				spsc = new SqlParameter("@sc", "PendingAuth");
			else
				spsc = new SqlParameter("@sc", "pending");
				

			SqlParameter spsl = new SqlParameter("@sl", "n");
			SqlParameter spat = new SqlParameter("@at", "0");
			SqlParameter spsndt = new SqlParameter("@sndt", "0");
			SqlParameter spsuct = new SqlParameter("@suct", "0");
			SqlParameter spfid = new SqlParameter("@fid", FilterID);
			SqlParameter spbt = new SqlParameter("@bt", "html");
			
			com.Parameters.Add(spcid);
			com.Parameters.Add(spuid);
			com.Parameters.Add(spes);
			com.Parameters.Add(spef);
			com.Parameters.Add(spefn);
			com.Parameters.Add(sprt);
			com.Parameters.Add(spci);
			com.Parameters.Add(splid);
			com.Parameters.Add(spgid);
			com.Parameters.Add(sprbid);
			com.Parameters.Add(spbf);
			com.Parameters.Add(sptb);
			com.Parameters.Add(spst);
			com.Parameters.Add(spsc);
			com.Parameters.Add(spsl);
			com.Parameters.Add(spat);
			com.Parameters.Add(spsndt);
			com.Parameters.Add(spsuct);
			com.Parameters.Add(spfid);
			com.Parameters.Add(spbt);

			try 
			{
				con.Open();
				BlastID = Convert.ToInt32(com.ExecuteScalar());
			} 
			catch (Exception err) 
			{
				string msg = ConfigurationManager.AppSettings["createerrmsg"];
				msg = msg.Replace("%%errmsg%%", err.Message);
				throw new Exception("Error occured while creating Blasts.<BR>" + msg);
			} 
			finally 
			{
				con.Close();
			}

			if (WizardSession.BlastID == -1) 
				WizardSession.BlastID = BlastID;
			 
			
			return WizardSession.BlastID;

		}


		/// <summary>
		/// Gets the default address associated with Custoemr form DB
		/// </summary>
		/// <param name="cid">An array of all Selected Customer</param>
		/// <returns>an Array of addresses associated with each customer</returns>
		private string GetAddress(string cid) 
		{
			string displayaddr = "";

			string selectQuery = "SELECT Address, City, State, Zip FROM "+ConfigurationManager.AppSettings["accountsdb"]+".dbo.Customer WHERE CustomerID=" + _customerID;
			DataRow dr = (DataFunctions.GetDataTable(selectQuery)).Rows[0];
			displayaddr = DataFunctions.CleanString((dr["Address"].ToString()+", "+dr["City"].ToString()+", "+dr["State"].ToString()+" - "+dr["Zip"].ToString()));

			return displayaddr;
		}

		private void UpdateCurrentBlastTo(string statusCode) 
		{
			string sql =	"UPDATE Blasts "+
				"SET StatusCode = @spsc "+
				"WHERE BlastID = @spbid";
			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
			SqlCommand com = new SqlCommand(sql, con);

			SqlParameter spsc = new SqlParameter("@spsc", statusCode);
			SqlParameter spbid = new SqlParameter("@spbid", WizardSession.BlastID);

			com.Parameters.Add(spsc);
			com.Parameters.Add(spbid);

			try 
			{
				con.Open();
				com.ExecuteNonQuery();
			} 
			catch (Exception err) 
			{
				string msg = ConfigurationManager.AppSettings["createerrmsg"];
				msg = msg.Replace("%%errmsg%%", err.Message);
				throw new Exception("Error occured while Updating Blasts.<BR>" + msg);
			} 
			finally 
			{
				con.Close();
			}
		}

		/*					==============================
		 *					VeriSign payment processing functions
		 *					==============================
		 */
		private string[] GetCustomerAddress () 
		{
			// Address array {0-->Address, 1-->City, 2-->State, 3-->Zip, 4-->Country}
			string []addressComponents = new string[8];
			
			string sql = "SELECT Address, City, State, Zip, Country, CustomerName, FirstName, LastName FROM "+ConfigurationManager.AppSettings["accountsdb"]+".dbo.Customer "+
				" WHERE CustomerID="+_customerID.ToString();;

			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["act"]);
			SqlCommand com = new SqlCommand(sql, con);
			SqlDataReader sdr = null;
			try 
			{
				con.Open();
				sdr = com.ExecuteReader();
			} 
			catch (Exception err) 
			{

				string msg = ConfigurationManager.AppSettings["createerrmsg"];
				msg = msg.Replace("%%errmsg%%", err.Message);
				throw new Exception("Error occured while fetching Customer Address.<BR>" + msg);

			}

			if (sdr.Read()) 
			{
				addressComponents[0] = sdr.GetString(0);	// ecn5_accounts.dbo.Customers.Address
				addressComponents[1] = sdr.GetString(1);	// ecn5_accounts.dbo.Customers.City
				addressComponents[2] = sdr.GetString(2);	// ecn5_accounts.dbo.Customers.State
				addressComponents[3] = sdr.GetString(3);	// ecn5_accounts.dbo.Customers.Zip
				addressComponents[4] = sdr.GetString(4);	// ecn5_accounts.dbo.Customers.Country
				addressComponents[5] = sdr.GetString(5);	// ecn5_accounts.dbo.Customers.CustomerName
				addressComponents[6] = sdr.GetString(6);	// ecn5_accounts.dbo.Customers.FirstName
				addressComponents[7] = sdr.GetString(7);	// ecn5_accounts.dbo.Customers.LastName
			} 
			else 
			{
				throw new Exception("Error occured while fetching Customer Address.<BR>No Rows Returned for Customer...!!!");
			}
			con.Close();
			return addressComponents;
		}

		private bool ProcessedCreditCard () 
		{
			try 
			{
				// Get user data
				string []names = new string[2];
				names[0] = CardHolderName.Substring(0, CardHolderName.IndexOf(" ", 0));
				names[1] = CardHolderName.Substring(CardHolderName.IndexOf(" ", 0)+1);
				// Create User Data Object
				UserInfo ui = new UserInfo(ConfigurationManager.AppSettings["vsUser"], 
					ConfigurationManager.AppSettings["vsVendor"], 
					ConfigurationManager.AppSettings["vsPartner"], 
					ConfigurationManager.AppSettings["vsPassword"]);

				// Create PFPro Connection Data Object
				PFProConnectionData con = new PFProConnectionData();

				// Create Invoice
				Invoice inv = new Invoice();
				Currency amt = new Currency(WizardSession.Amount);
				inv.Amt = amt;
				inv.InvNum = WizardSession.BlastID.ToString();
				
				//sunil - 07/05/2006 - added channel name for Verisign Reports

				CustomerInfo cust = new CustomerInfo();

				cust.CompanyName = DataFunctions.ExecuteScalar("accounts", "SELECT BaseChannelName FROM BaseChannel where basechannelID = " + _channelID).ToString();
				inv.CustomerInfo = cust;


				// Get Customer's address info from ecn5 database
				string []arr = GetCustomerAddress();

				// Set Billing Address
				BillTo bill = new BillTo();
				bill.FirstName = arr[6];//names[0];
				bill.LastName = arr[7];//names[1];
				//bill.Email = ((Step2)Session["step2"]).EmailAddress;
				bill.Street = arr[0];
				bill.City = arr[1];
				bill.State = arr[2];
				bill.Zip = arr[3];
				bill.BillToCountry = arr[4];
				inv.BillTo = bill;

				// Shipping is being utilized to store wizard-from fields like from-name and from-email
				ShipTo ship = new ShipTo();
				ship.ShipToFirstName = names[0];//;
				ship.ShipToLastName = names[1];//
				inv.ShipTo = ship;

				inv.Comment1 = arr[5];	// CustomerName from ecn5_accounts.dbo.Customer;
				inv.Comment2 = WizardSession.FooterName + " # " + WizardSession.EmailAddress;
	
				// Create Payment Device - Credit Card Data Object
				CreditCard cc = new CreditCard(CardNumber, CardExpMonth.ToString()+CardExpYear.ToString());
				cc.Cvv2 = CardVerificationNumber;

				// Create Card Tender Data Object
				CardTender ct = new CardTender(cc);

				// Create new Sales Transaction
				SaleTransaction st = new SaleTransaction(ui, con, inv, ct, PFProUtility.RequestId);

				// Submit the Sales Transaction
				Response resp = st.SubmitTransaction();

				// Set Commit Response
				CommitResponse cr = null;

				if (resp != null) 
				{
					// Get Transaction Response parameters
					TransactionResponse tr = resp.TransactionResponse;

					// Create Client Information Data Object
					ClientInfo ci = new ClientInfo();
					// Set the Client Duration
					ci.ClientDuration = st.ClientInfo.ClientDuration;
					// Set Client Info Object to Transaction Object
					st.ClientInfo = ci;

					// Submit Commit Transaction
					cr = st.SubmitCommitTransaction();
				
					// If Transaction-Response and Commit Transaction Response, both return zero means transaction was complete and successfull
					if ((Convert.ToInt32(tr.Result) == 0) && (Convert.ToInt32(cr.Result) == 0)) 
					{
						crResponse = cr.RespMsg;
						return true;
					}
					else
					{
						throw new Exception("");
					
					}

					/* Comment this in PROD release 
					if (tr != null) 
					{
						lblDebugMsg.Text = "=====Transaction Response===== " + 
							"Result: " + tr.Result + "<BR>" +
							"PnRef: " + tr.Pnref + "<BR>" + 
							"Resp Msg: "+ tr.RespMsg + "<BR>" +
							"Auth Code: " + tr.AuthCode + "<BR>" +
							"AVS Addr: " + tr.AVSAddr + "<BR>" +
							"AVS Zip: " + tr.AVSZip + "<BR>" +
							"I AVS: " + tr.IAVS + "<BR>";
					}

					// Display Commit Transaction Response Parameters
					Response.Write("=====Commit Transaction Response=====");
					if (cr != null) 
					{
						lblDebugMsg.Text = lblDebugMsg.Text + " Commit Result: " + cr.Result +  "<BR>" +
							"Commit Resp Msg: " + cr.RespMsg + "<BR>";
					}

					// Display Response
					lblDebugMsg.Text = lblDebugMsg.Text + "=====Display Response======" + "<BR>" + PFProUtility.GetStatus(resp);
					 Comment above lines in PROD release */
				}
			} 
			catch (Exception err) 
			{
				string msg = ConfigurationManager.AppSettings["failuremessage"];
				msg = msg.Replace("%%psiresponse%%", err.Message);

				throw new Exception("Error occured while Processing Credit card.<BR>" + msg);

			}
			return false;
		}
		/// <summary>
		/// Generates an Email Body to attach to email
		/// </summary>
		/// <returns>Email Body String</returns>
		private string GetMailReceiptBody() 
		{
			StringBuilder sb = new StringBuilder();
			// set card number to be sent via email

			string cn = CardNumber.Substring(CardNumber.Length-4);
			cn = cn.PadLeft(CardNumber.Length-4,'*');

			sb.Append("=============================================================================\n\n");
			sb.Append(ConfigurationManager.AppSettings["receiptCCmessage"]+"\n\n");
			sb.Append("=============================================================================\n\n");
			sb.Append("----------- Message Information -----------------\n\n");
			sb.Append("From:\t\t\t"+ConfigurationManager.AppSettings["fromemail"]+"\n\n");
			sb.Append("Sent:\t\t\t"+DateTime.Now+"\n\n");
			sb.Append("To:\t\t\t"+WizardSession.FooterName+"\n\n");
			sb.Append("Subject:\t\t"+WizardSession.EmailSubject+"\n\n");
			sb.Append("Date:\t\t\t"+DateTime.Now.ToString("MM/dd/yyyy")+"\n");
			sb.Append("-------------------------------------------------\n\n\n");
			sb.Append("----------- Billing Information -----------------\n\n");
			sb.Append("Name:\t\t\t"+CardHolderName+"\n\n");
			sb.Append("Card Type:\t\t"+CardType+"\n\n");
			sb.Append("Card Number:\t\t"+cn+"\n\n");
			sb.Append("Expiration Date:\t"+CardExpMonth+"/"+CardExpYear+"\n\n");
			sb.Append("Transaction Result:\t"+crResponse+"\n\n");
			sb.Append("Amount Charged:\t\t"+ WizardSession.Amount +"\n");
			sb.Append("-------------------------------------------------");

			return sb.ToString();
		}


		/// <summary>
		/// Sends email via SMTP server
		/// </summary>
        private void SendMail()
        {
            string body = GetMailReceiptBody();
            MailMessage mm = new MailMessage();
            mm.Body = body;
            mm.IsBodyHtml = false;
            mm.From = new MailAddress(ConfigurationManager.AppSettings["fromemail"]);					// change the from email as soon as you know the replace ment
            mm.Subject = "Receipt for emarketing wizard";						// chnage the mail subject as soon as you know the replacement
            mm.To.Add(WizardSession.EmailAddress);
            mm.Bcc.Add(ConfigurationManager.AppSettings["fromemail"]);

            SmtpClient sc = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            sc.Send(mm);
        }

		private string StripTextFromHtml(string html)
		{
			Regex regExp = new Regex("<(.|\n)+?>");
			string strOutput = "";
			//Replace all HTML tag matches with the empty string
			strOutput = regExp.Replace(html, "");
				  
			//Replace all < and > with &lt; and &gt; and &nbsp; with " " space
			strOutput = strOutput.Replace("&lt;", "<");
			strOutput = strOutput.Replace("&gt;", ">");
			strOutput = strOutput.Replace("&nbsp;", " ");
				  
			//ContentText.Text=StringFunctions.Replace(ContentText.Text,"&nbsp;"," ");
			return strOutput;
		}

	}
}
