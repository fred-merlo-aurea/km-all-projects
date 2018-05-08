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

//	========= Comment these when not using VeriSign ==========
using PayPal.Payments.Common;				
using PayPal.Payments.Common.Utility;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;

//	====== End Of [Comment these when not using VeriSign] =======
using System.Configuration;
using System.Web.Configuration; 
using ecn.common.classes;
using ecn.communicator.classes;
using ecn.showcare.wizard.main;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Web.Mail;
using System.Text.RegularExpressions;

namespace ecn.showcare.wizard.main
{
	/// <summary>
	/// Summary description for CreditCardSend.
	/// </summary>
	public class CreditCardSend : System.Web.UI.Page {
		protected System.Web.UI.HtmlControls.HtmlInputText Name;
		protected System.Web.UI.WebControls.DropDownList cardType;
		protected System.Web.UI.HtmlControls.HtmlInputText cardNumber;
		protected System.Web.UI.HtmlControls.HtmlSelect year;
		protected System.Web.UI.HtmlControls.HtmlSelect month;
		protected System.Web.UI.HtmlControls.HtmlInputText cvNumber;
		protected System.Web.UI.HtmlControls.HtmlInputImage submit;
		protected System.Web.UI.HtmlControls.HtmlGenericControl smsg;
		protected System.Web.UI.WebControls.Label lblAmount;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divMsg;

		protected string accountsdb = ConfigurationManager.AppSettings["accountsdb"];

		protected System.Web.UI.WebControls.RequiredFieldValidator rfvCardName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvCardType;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvCardNumber;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvCardYear;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvCardMonth;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvCVNumber;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.ImageButton btnSubmit;
		protected Steps steps = new Steps();
		protected string psierrmsg = "";
		protected string coreerrmsg = "";
		protected Step4 step4 = null;
		protected System.Web.UI.WebControls.TextBox txtCouponcode;
		protected System.Web.UI.WebControls.RadioButtonList rdCoupon;
		protected System.Web.UI.WebControls.PlaceHolder plCreditcard;
		protected System.Web.UI.WebControls.PlaceHolder plCoupon;
		string respMsg = "";
	
		private void Page_Load(object sender, System.EventArgs e) {
			// Set Last visited step to current
            //if (IsPostBack)
            //{
            //    if (ViewState["msgtype"] != null) 
            //    {
            //        if (ViewState["msgtype"].ToString().Equals("c"))
            //        {
            //            lblAmount.Text = "$30.00"; 
            //            ShowMessage("c");
            //        }
            //    }
            //}

            if (!IsPostBack && (steps.Current > steps.LastVisited)) {
				steps.nextStep();

			}

			if (!IsPostBack)
			{
				plCreditcard.Visible = true;
                plCoupon.Visible = true;
                lblAmount.Text = ConfigurationManager.AppSettings["defaultamount"]; 
			}

			try {
				step4 = (Step4) Session["step4"];
				if (step4.ContentID == "") {
					step4 = new Step4();
				}
			} catch (Exception) {
				step4 = new Step4();
			}

			// Display charhes at the top label control
			
			smsg.Visible= false; 
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
//		this.cardType.SelectedIndexChanged += new System.EventHandler(this.cardType_SelectedIndexChanged);
//		this.btnSubmit.Click += new System.Web.UI.ImageClickEventHandler(this.btnSubmit_Click);

		private void InitializeComponent() 
		{    
			//this.rdCoupon.SelectedIndexChanged += new System.EventHandler(this.rdCoupon_SelectedIndexChanged);
			this.cardType.SelectedIndexChanged += new System.EventHandler(this.cardType_SelectedIndexChanged);
			this.btnSubmit.Click += new System.Web.UI.ImageClickEventHandler(this.btnSubmit_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		
		# region special helper methods
		/// <summary>
		/// Performs a check to see if all the information required to create a Cotnent or Layout is present or not.
		/// If any of the information requied to create Content or Layout is not avaibale, 
		/// then this method simply displays an error message, until the required information is avaiable.
		/// </summary>
		/// <returns>
		/// True:	If all the required information is present
		/// False: If any of the piece of information is not found.
		/// </returns>
		private bool PerformChecks() {
			// Errors for unknown reasons. Mainly because there is no session contents available to successfully process datastore.
			if (Session["step1"] == null) {
				psierrmsg = "Could not find information about selected Template.";
				return false;
			}
			if (Session["step2"] == null) {
				psierrmsg = "Could not find information regarding Message.";
				return false;
			}
			return true;
		}
		
		# endregion special helper methods

		# region Contents Creation

		/// <summary>
		/// By passes all html tags and returns pure text
		/// </summary>
		/// <param name="html">Html embeded string</param>
		/// <returns>Plain Text string</returns>
		private string StripTextFromHtml(string html){
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

		/// <summary>
		/// Stores Contents created into the DB as a new record.
		/// A new record is created, every time customer creates new message, in the Cotnents Table
		/// </summary>
		/// <returns>
		/// Number:	upon successfull creation of Contents returns ContentID
		/// 0: upon failure from either PerformChecks() or if failed to create Contents
		/// </returns>
		private int SaveContents() {
			if (PerformChecks()) {
				// Get the object saved at step1 and step2
				Step1 step1 = (Step1)Session["step1"];
				Step2 step2 = (Step2)Session["step2"];

				string csource = step2.ContentSource;
				string ctext = step2.ContentText;
				int cntid = 0;

				// Before saving the contents we need to add code-snippet and footer information
				if (step2.Salutation.Equals("firstname")) {
					csource = "<P align=left>Dear %%FirstName%%,</P>" + csource;
				} else {
					csource = "<P align=left>Dear %%FirstName%% %%LastName%%,</P>" + csource;
				}
				// In any case we need to insert Footer-Information in the end
				csource += "<BR><P align=left>"+
					step2.FooterName+"<BR>"+
					step2.FooterTitle+"<BR>"+
					step2.FooterCompany+"<BR>"+
					step2.FooterPhone+"<BR>";
	//			csource = DataFunctions.CleanString(csource);

				if (step1.IsCustomHeader)
				{
					string headerImage = "";
					if (step2.HeaderImage != string.Empty)
					{
						ChannelCheck cc = new ChannelCheck();

						headerImage = "<IMG src='http://www.ecn5.com/" + cc.getAssetsPath("accounts")+"/"+"channelID_"+ ConfigurationManager.AppSettings["ChannelID"] +"/customers/"+ step1.CustomerID +"/images/" + step2.HeaderImage + "' border=0>";
					}
					else
						headerImage = "&nbsp;";

					csource = "<TR><TD style='HEIGHT: 100px;border:2px solid #afc2d5;' valign='middle' ><center>" + headerImage + "</center></TD></TR><TR><TD style='PADDING-RIGHT: 30px; PADDING-LEFT: 30px; FONT-SIZE: 12px; PADDING-BOTTOM: 30px; PADDING-TOP: 30px'>" + csource + "</TD></TR>";
				}

				ctext = StripTextFromHtml(csource);

				string sqlquery = "";
				if (step4.ContentID.Equals("")) {
					sqlquery = " INSERT INTO Content ( "+
						" ContentTitle, ContentTypeCode, LockedFlag, UserID, FolderID, "+
						" ContentSource, ContentText, CustomerID, ModifyDate, Sharing "+
						" ) VALUES ( @spcntitle,'html','N',@spuid,0,@spcsrc,@spctxt,@spcid,@spmdt,'N');SELECT @@IDENTITY";
				} else {
					sqlquery = "Update Content SET "+
						" ContentTitle = @spcntitle, ContentTypeCode = 'html', LockedFlag = 'N', UserID = @spuid, FolderID = 0, "+
						" ContentSource = @spcsrc, ContentText = @spctxt, CustomerID = @spcid, ModifyDate = @spmdt, Sharing = 'N' "+
						" WHERE ContentID = "+step4.ContentID;
				}

				SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);	// Create Connection
				SqlCommand com = new SqlCommand(sqlquery, con);																		// Create Command

				// Declare Parameters
				SqlParameter spcntitle = new SqlParameter("@spcntitle", SqlDbType.VarChar, 255);
				spcntitle.Value = step2.MessageName;
				SqlParameter spuid = new SqlParameter("@spuid", SqlDbType.Int);
				spuid.Value = step1.UserID;
				SqlParameter spcsrc = new SqlParameter("@spcsrc", SqlDbType.Text);
				spcsrc.Value = csource;
				SqlParameter spctxt = new SqlParameter("@spctxt", SqlDbType.Text);
				spctxt.Value = ctext;
				SqlParameter spcid = new SqlParameter("@spcid", SqlDbType.Int);
				spcid.Value = step1.CustomerID;
				SqlParameter spmdt = new SqlParameter("@spmdt", SqlDbType.DateTime);
				spmdt.Value = System.DateTime.Now;

				// Add parameters to command
				com.Parameters.Add(spcntitle);		// content title
				com.Parameters.Add(spuid);						// user id
				com.Parameters.Add(spcsrc);								// content  source
				com.Parameters.Add(spctxt);									// content text
				com.Parameters.Add(spcid);				// customer id
				com.Parameters.Add(spmdt);		// modify date

				try{
					con.Open();
					cntid = Convert.ToInt32(com.ExecuteScalar());
				}catch(Exception err){
					//					lblMsg.Text = "ERROR: Error Occured when creating Content<br>"+sqlquery;
					psierrmsg = "Error occured while creating contents!<BR>";
					coreerrmsg = err.Message+"<BR>";
					return 0;
				} finally {
					con.Close();
				}
				if (step4.ContentID == "") 
					return cntid;											// Make sure it's integer
				else
					return Convert.ToInt32(step4.ContentID);
			} else
				return 0;
		}

		# endregion Contents Creation

		# region Layouts Creation

		/// <summary>
		/// Gets the default address associated with Custoemr form DB
		/// </summary>
		/// <param name="cid">An array of all Selected Customer</param>
		/// <returns>an Array of addresses associated with each customer</returns>
		private string GetAddress(string cid) {
			string displayaddr = "";

			string selectQuery = "SELECT Address, City, State, Zip FROM "+accountsdb+".dbo.Customer WHERE CustomerID="+cid;
			DataRow dr = (DataFunctions.GetDataTable(selectQuery)).Rows[0];
			displayaddr = DataFunctions.CleanString((dr["Address"].ToString()+", "+dr["City"].ToString()+", "+dr["State"].ToString()+" - "+dr["Zip"].ToString()));

			return displayaddr;
		}


		/// <summary>
		/// Creates a New Message record
		/// </summary>
		/// <param name="cntid">ContentID associated with Selcted Template and Custoemr</param>
		/// <returns>
		/// Number:	If could create the record sccessfully, returns LayoutID
		/// 0:	If failed to create
		/// </returns>
		private int SaveMessage(string cntid) {
			// Get Step1 and Step2 objects stored during those steps
			Step1 step1 = (Step1)Session["step1"];
			Step2 step2 = (Step2)Session["step2"];

			//string lname = DataFunctions.CleanString(step2.MessageName);

			string displayaddr = GetAddress(step1.CustomerID);						// --> Don't think I need it
			int layoutID = 0;

			string sqlquery = "";
			if (step4.LayoutID.Equals("")) {
				sqlquery = 
					" INSERT INTO Layouts ( "+
					" LayoutName, CustomerID, UserID, FolderID, ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9,"+
					" TemplateID, ModifyDate, DisplayAddress "+
					" ) VALUES ( @splname,@spcid,@spuid,0,@spcntid,0,0,0,0,0,0,0,0,@sptid,@spmdt,@spdadr);SELECT @@IDENTITY";
			} else {
				sqlquery = "UPDATE Layouts SET "+
					" LayoutName =  @splname, CustomerID = @spcid, UserID = @spuid, FolderID = 0, ContentSlot1 = @spcntid, "+
					" ContentSlot2 = 0, ContentSlot3 = 0, ContentSlot4 = 0, ContentSlot5 = 0, ContentSlot6 = 0, ContentSlot7 = 0, "+
					" ContentSlot8 = 0, ContentSlot9 = 0, TemplateID = @sptid, ModifyDate = @spmdt, DisplayAddress = @spdadr "+
					" WHERE LayoutID = " + step4.LayoutID;
			}

			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
			SqlCommand com = new SqlCommand();
			com.Connection = con;
			com.CommandText = sqlquery;

			// Declare Parameters
			SqlParameter splname = new SqlParameter("@splname", SqlDbType.VarChar, 50);
			splname.Value = step2.MessageName;
			SqlParameter spuid = new SqlParameter("@spuid", SqlDbType.Int);
			spuid.Value = step1.UserID;
			SqlParameter spcid = new SqlParameter("@spcid", SqlDbType.Int);
			spcid.Value = step1.CustomerID;
			SqlParameter spcntid = new SqlParameter("@spcntid", SqlDbType.Int);
			spcntid.Value = cntid;
			SqlParameter spmdt = new SqlParameter("@spmdt", SqlDbType.DateTime);
			spmdt.Value = System.DateTime.Now;
			SqlParameter spadr = new SqlParameter("@spdadr", SqlDbType.VarChar, 255);
			spadr.Value = displayaddr;
			SqlParameter sptid = new SqlParameter("@sptid", SqlDbType.Int);
			sptid.Value = step1.TemplateID;

			com.Parameters.Add(splname);
			com.Parameters.Add(spcid);
			com.Parameters.Add(spuid);
			com.Parameters.Add(spcntid);
			com.Parameters.Add(sptid);
			com.Parameters.Add(spmdt);
			com.Parameters.Add(spadr);

			try{
				con.Open();
				layoutID = Convert.ToInt32(com.ExecuteScalar());
			}catch (Exception err){
				//					lblMsg.Text = "ERROR: Error Occured when creating Message<br>"+sqlquery;
				psierrmsg = "Error occured while creating message!<BR>";
				coreerrmsg = err.Message+"<BR>";
				return 0;
			} finally {
				con.Close();
			}
			if (step4.LayoutID == "")
				return layoutID;
			else 
				return Convert.ToInt32(step4.LayoutID);
		}


		# endregion Layouts Creation

		# region Setup Blast

		private int BlastSetup (string layoutid) {
			Step1 step1 = (Step1) Session["step1"];
			Step2 step2 = (Step2) Session["step2"];

			Blasts blast = new Blasts();
			int bid = 0;

			// Store new blast record into Blasts table
			string sql = "";

			if (step4.BlastID.Equals("")) {
				sql = "INSERT INTO Blasts "+
					"(CustomerID, UserID, EmailSubject, EmailFrom, EmailFromName, ReplyTo, CodeID, LayoutID, GroupID, RefBlastID, "+
					"BlastFrequency, TestBlast, SendTime, StatusCode, SpinLock, AttemptTotal, SendTotal, SuccessTotal, FilterID, BlastType) VALUES "+
					"(@cid, @uid, @es, @ef, @efn, @rt, @ci, @lid, @gid, @rbid, @bf, @tb, @st, @sc, @sl, @at, @sndt, @suct, @fid, @bt); SELECT @@IDENTITY;";
			} else {
				sql = "UPDATE Blasts SET "+
					" CustomerID = @cid, UserID = @uid, EmailSubject = @es, EmailFrom = @ef, EmailFromName = @efn, "+
					" ReplyTo = @rt, CodeID = @ci, LayoutID = @lid, GroupID = @gid, RefBlastID = @rbid, "+
					" BlastFrequency = @bf, TestBlast = @tb, SendTime = @st, StatusCode = @sc, SpinLock = @sl "+
					" WHERE BlastID = " + step4.BlastID;
			}

			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
			SqlCommand com = new SqlCommand(sql, con);

			SqlParameter spcid = new SqlParameter("@cid", step1.CustomerID);
			SqlParameter spuid = new SqlParameter("@uid", step1.UserID);
			SqlParameter spes = new SqlParameter("@es", step2.EmailSubject);
			SqlParameter spef = new SqlParameter("@ef", step2.EmailAddress);
			SqlParameter spefn = new SqlParameter("@efn", step2.HeaderFromName);
			SqlParameter sprt = new SqlParameter("@rt", step2.EmailAddress);
			SqlParameter spci = new SqlParameter("@ci", "0");
			SqlParameter splid = new SqlParameter("@lid", layoutid);
			SqlParameter spgid = new SqlParameter("@gid", step1.GroupID);
			SqlParameter sprbid = new SqlParameter("@rbid", "-1");
			SqlParameter spbf = new SqlParameter("@bf", "ONETIME");
			SqlParameter sptb = new SqlParameter("@tb", "n");
			SqlParameter spst = new SqlParameter("@st", DateTime.Now.ToString());
			SqlParameter spsc = new SqlParameter("@sc", "PendingAuth");
			SqlParameter spsl = new SqlParameter("@sl", "n");
			SqlParameter spat = new SqlParameter("@at", "0");
			SqlParameter spsndt = new SqlParameter("@sndt", "0");
			SqlParameter spsuct = new SqlParameter("@suct", "0");
			SqlParameter spfid = new SqlParameter("@fid", "0");
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

			try {
				con.Open();
				bid = Convert.ToInt32(com.ExecuteScalar());
			} catch (Exception err) {
				psierrmsg = "Error ocured while setting up blast!<BR>";
				coreerrmsg = err.Message+"<BR>";
			} finally {
				con.Close();
			}


			if (step4.BlastID == "") {
				// Store new grouping information itno BlastGrouping table
				string insertQuery = "INSERT INTO BlastGrouping (BlastIDs, UserID, DateAdded, EmailSubject) "+
					"VALUES (@spbids,@spui,@spda,@spesub)";
				con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
				com = new SqlCommand(insertQuery, con);

				SqlParameter spbids = new SqlParameter("@spbids", bid);
				SqlParameter spui = new SqlParameter("@spui", step1.UserID);
				SqlParameter spda = new SqlParameter("@spda", System.DateTime.Now);
				SqlParameter spesub = new SqlParameter("@spesub", step2.EmailSubject);

				com.Parameters.Add(spbids);
				com.Parameters.Add(spui);
				com.Parameters.Add(spda);
				com.Parameters.Add(spesub);

				try {
					con.Open();
					com.ExecuteNonQuery();
					return bid;
				} catch (Exception err) {	
					psierrmsg = "Error occured while seting up blast!<BR>";
					coreerrmsg = err.Message+"<BR>";
				}
				finally {
					con.Close();
				}
				return 0;
			} else {
				return Convert.ToInt32(step4.BlastID);
			}
		}


		private int UpdateCurrentBlastTo(string statusCode, int bid) {
			string sql =	"UPDATE Blasts "+
				"SET StatusCode = @spsc "+
				"WHERE BlastID = @spbid";
			SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["com"]);
			SqlCommand com = new SqlCommand(sql, con);

			SqlParameter spsc = new SqlParameter("@spsc", statusCode);
			SqlParameter spbid = new SqlParameter("@spbid", bid);

			com.Parameters.Add(spsc);
			com.Parameters.Add(spbid);

			try {
				con.Open();
				return com.ExecuteNonQuery();
			} catch (Exception err) {
				psierrmsg = "Error occured while updating blast status!<BR>";
				coreerrmsg = err.Message+"<BR>";
			} finally {
				con.Close();
			}
			return 0;
		}


		# endregion Setup Blast

		# region Credit Card Processing

/*											==============================
 *												VeriSign payment processing functions
 *											==============================
 */

		private bool ProcessedCreditCard () 
		{
			try 
			{
				string pfRequestID = PayflowUtility.RequestId;

				// Create User Data Object
				UserInfo pfUserInfo = new UserInfo(ConfigurationManager.AppSettings["vsUser"], 
					ConfigurationManager.AppSettings["vsVendor"], 
					ConfigurationManager.AppSettings["vsPartner"], 
					ConfigurationManager.AppSettings["vsPassword"]); 


				PayflowConnectionData pfConnection = new PayflowConnectionData(); 
				
				// Create Invoice
				//sunil - 07/05/2006 - added channel name for Verisign Reports
				CustomerInfo pfcustomerinfo = new CustomerInfo();
				pfcustomerinfo.CustId = 
					pfcustomerinfo.CustCode = DataFunctions.ExecuteScalar("accounts", "SELECT BaseChannelName FROM BaseChannel where basechannelID = " + ConfigurationManager.AppSettings["ChannelID"].ToString()).ToString();

				Invoice pfinvoice = new Invoice();
				//pfinvoice.Amt = new Currency(Convert.ToDecimal(ConfigurationManager.AppSettings["defaultamount"]));
                pfinvoice.Amt = new Currency(Convert.ToDecimal(lblAmount.Text));  
				pfinvoice.InvNum = step4.BlastID.ToString();
				pfinvoice.CustomerInfo = pfcustomerinfo;

				// Create Payment Device - Credit Card Data Object
				CreditCard cc = new CreditCard(cardNumber.Value, month.Value+year.Value);
				cc.Cvv2 = cvNumber.Value;

				// Create Card Tender Data Object
				CardTender Tender = new CardTender(cc);

				// Create new Transaction
				SaleTransaction Trans = new SaleTransaction(pfUserInfo, pfConnection, pfinvoice, Tender, pfRequestID);

				// Submit the Sales Transaction
				Trans.SubmitTransaction();

				// If Transaction-Response returns zero means transaction was complete and successfull
				if (Trans.Response.TransactionResponse.Result == 0) 
					return true;
					/*
				else
				{
					
					Response.Write("=====Transaction Response===== " + 
						"Result: " + Trans.Response.TransactionResponse.Result + "<BR>" +
						"PnRef: " + Trans.Response.TransactionResponse.Pnref + "<BR>" + 
						"Resp Msg: "+ Trans.Response.TransactionResponse.RespMsg + "<BR>" +
						"Auth Code: " + Trans.Response.TransactionResponse.AuthCode + "<BR>" +
						"AVS Addr: " + Trans.Response.TransactionResponse.AVSAddr + "<BR>" +
						"AVS Zip: " + Trans.Response.TransactionResponse.AVSZip + "<BR>" +
						"I AVS: " + Trans.Response.TransactionResponse.IAVS);

					// Display Response
					//Response.Write("=====Display Response======" + "<BR>" + PFProUtility.GetStatus(resp));
					Response.Write("=====Display Response======" + "<BR>" + PayflowUtility.GetStatus(Trans.Response));
				}
				*/
					
			}
			
			catch (Exception) {}
			return false;
		}

		# endregion Credit Card Processing

		# region Send Mail

		/// <summary>
		/// Shows appropriate message for credit-card transaction
		/// </summary>
		/// <param name="type">Type of message to show
		/// f:	Failure Message
		/// s:	Success Message
		/// </param>
		private void ShowMessage(string type) {
			Label lbl = new Label();
			if (type == "s") {
				Step2 step2 = (Step2) Session["step2"];
				string msg = ConfigurationManager.AppSettings["successmessage"];
				msg = msg.Replace("%%emailaddress%%", step2.EmailAddress);
				Session["msg"] = msg;
				//lbl.Text = msg;
				//lbl.ForeColor = Color.Black;
			} else if (type == "f"){
				string msg = ConfigurationManager.AppSettings["failuremessage"];
				msg = msg.Replace("%%psiresponse%%", psierrmsg+coreerrmsg);
				lbl.Text = msg;
				lbl.ForeColor = Color.Red;
				divMsg.Controls.Add(lbl);
			} else if (type == "d"){
				string msg = ConfigurationManager.AppSettings["createerrmsg"];
				msg = msg.Replace("%%errmsg%%", coreerrmsg+psierrmsg);
				lbl.Text = msg;
				lbl.ForeColor = Color.Red;
				divMsg.Controls.Add(lbl);
			} else if (type == "c"){
				string msg = "Invalid Coupon code";
				lbl.Text = msg;
				lbl.ForeColor = Color.Red;
				divMsg.Controls.Add(lbl);
			}
		}


		/// <summary>
		/// Generates an Email Body to attach to email
		/// </summary>
		/// <returns>Email Body String</returns>
		private string GetMailReceiptBody() {
			StringBuilder sb = new StringBuilder();
			// set card number to be sent via email
			// Get Step2 data
			Step2 step2 = (Step2) Session["step2"];

			sb.Append("=============================================================================\n\n");
			if (txtCouponcode.Text.Trim().Length > 0)
				sb.Append(ConfigurationManager.AppSettings["receiptmessage1"]+"\n\n");
			else
				sb.Append(ConfigurationManager.AppSettings["receiptmessage"]+"\n\n");

			sb.Append("=============================================================================\n\n");
			sb.Append("----------- Message Information -----------------\n\n");
			sb.Append("From:\t\t\t"+ConfigurationManager.AppSettings["fromemail"]+"\n\n");
			sb.Append("Sent:\t\t\t"+DateTime.Now+"\n\n");
			sb.Append("To:\t\t\t"+step2.FooterName+"\n\n");
			sb.Append("Subject:\t\t"+step2.EmailSubject+"\n\n");
			sb.Append("Date:\t\t\t"+DateTime.Now.ToString("MM/dd/yyyy")+"\n");
			sb.Append("-------------------------------------------------\n\n\n");
			

			if (txtCouponcode.Text.Trim().Length > 0)
			{
                sb.Append("");
				//sb.Append("Coupon Code:\t\t\t"+txtCouponcode.Text+"\n\n");
			}
			else
			{
				string cn = cardNumber.Value.Substring(cardNumber.Value.Length-4);
				cn = cn.PadLeft(cardNumber.Value.Length-4,'*');

                sb.Append("----------- Billing Information -----------------\n\n");

				sb.Append("Name:\t\t\t"+Name.Value+"\n\n");
				sb.Append("Card Type:\t\t"+cardType.SelectedValue+"\n\n");
				sb.Append("Card Number:\t\t"+cn+"\n\n");
				sb.Append("Expiration Date:\t"+month.Value+"/"+year.Value+"\n\n");
				//sb.Append("CV Number:\t\t"+cvNumber.Value+"\n\n");
				sb.Append("Transaction Result:\t"+respMsg+"\n\n");
				sb.Append("Amount Charged:\t\t$"+lblAmount.Text+" (US)\n");
                sb.Append("-------------------------------------------------");
			}
			
			return sb.ToString();
		}


		/// <summary>
		/// Sends email via SMTP server
		/// </summary>
		private void SendMail () {
			Step2 step2 = (Step2) Session["step2"];
			string body = GetMailReceiptBody();
			SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SMTPServer"];
			MailMessage mm = new MailMessage();
			mm.Body = body;
			mm.BodyFormat = MailFormat.Text;
			mm.From = ConfigurationManager.AppSettings["fromemail"];					// change the from email as soon as you know the replace ment
			mm.Subject = "Receipt for Showcare emarketing wizard";						// chnage the mail subject as soon as you know the replacement
			mm.To = step2.EmailAddress;
			mm.Bcc = ConfigurationManager.AppSettings["fromemail"] + "; " + ConfigurationManager.AppSettings["bccemail"];
			SmtpMail.Send(mm);
		}


		# endregion Send Mail


		/* ==================== FLOW ====================
		 * Decide, if we want to save the record first and then process the transaction or other way around [ ask Paul ]
		 * SAVE TO DB:
		 *		1) Save the Content
		 *		2) Save the Layout
		 *		3) Setup Blast object
		 * Update the Blast's StatusCode to 'PendingAuth'
		 * PROCESS THE CC
		 *		If Approved
		 *			Update Blast's StatusCode to Pending
		 */
		private void submit_ServerClick(object sender, System.Web.UI.ImageClickEventArgs e) {
		}

		private void ClearSession () {
			try {
				Session.Remove("step1");
				Session.Remove("step2");
				//Session.Remove("step3");
				Session.Remove("step4");
			} catch (Exception) {}
		}

		private void btnSubmit_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
			// Try to save contents
			int cntid = 0;
			if ((cntid=SaveContents()) > 0) {										// Means saved successfully
				// Try to save layout
				int lid = 0;
				if ((lid=SaveMessage(cntid.ToString())) > 0) {				// Means saved successfully
					// Try to setup blast and blast-group
					int bid = 0;
					// Set current blast's status-code to [PendingAuth], until credit card is charged successfully
					if ((bid=BlastSetup(lid.ToString())) > 0) {										// Means saved blast information successfully	
						step4.ContentID = cntid.ToString();
						step4.LayoutID = lid.ToString();
						step4.BlastID = bid.ToString();
						step4.ToSession();

						bool bSend = false;

						if (txtCouponcode.Text.Trim().Length > 0)
						{
							bSend = ValidateCoupon();

                            if (!bSend)
                            {
                                lblAmount.Text = ConfigurationManager.AppSettings["defaultamount"];
                                ShowMessage("c");
                                smsg.Visible = true;
                                return;
                            }
                            else
                            {
                                lblAmount.Text = WebConfigurationManager.AppSettings["couponamount"].ToString(); 
                            }
						}
						
                        bSend = ProcessedCreditCard();  // Try to charge the credit card

						if (bSend) 
							{			
								// You are here means everything went just fine :)
								// Set current blast's status-code to [pending], since it seems like credit card has been charged successfully
								if (UpdateCurrentBlastTo("pending", bid) > 0) {			// Means status code has been set to [pending]
									// Dsiplay success message...
									ShowMessage("s");
									// Send an Email receipt to the client
									SendMail();															
									// Clear everything from the session since the blast has been setup successfully, we dont need it now.
									ClearSession();
									Response.Redirect("Confirmation.aspx");
								} else {																	// Means there was a problem setting up status code for current blast
									ShowMessage("d");
									smsg.Visible = true;
								}
							} else {																		// Means Credit Card Processing failed! :(
								// Disaply failure message...
								ShowMessage("f");
								smsg.Visible = true;
							}
					} else {																			// Means Couldn't save the blast for some reason
						ShowMessage("d");
						smsg.Visible = true;
					}
				} else {																				// Means couldn't save the message
					ShowMessage("d");
					smsg.Visible = true;
				}
			} else {																					// Means couldn't save the contents
				ShowMessage("d");
				smsg.Visible = true;
			}
		}

		private bool ValidateCoupon()
		{
			bool bValid = false;
			string couponcode = txtCouponcode.Text.Trim();

			string[] cc = ConfigurationManager.AppSettings["couponcode"].ToString().Split(',');
							
			for (int j=0;j<cc.Length;j++)
			{
				if (cc[j].ToUpper().Equals(couponcode.ToUpper()))
				{
					bValid = true;
					break;
				}
			}
			return bValid; 
		}

		private void cardType_ServerChange(object sender, System.EventArgs e) {
		}

		private void cardType_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (cardType.SelectedIndex == 2) {		// Amex selected. Make CV size 4, and, image.
				cvNumber.MaxLength = 4;	// Max length is 4.
			}
		}

        //private void rdCoupon_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    if (rdCoupon.SelectedValue == "n")
        //    {
        //        plCreditcard.Visible = true;
        //        plCoupon.Visible = false;
        //    }
        //    else
        //    {
        //        plCreditcard.Visible = false;
        //        plCoupon.Visible = true;
        //    }
        //}

        protected void Validate_Coupon_Click(object sender, EventArgs e)  
        {
            if (!ValidateCoupon())
            {
                lblAmount.Text = ConfigurationManager.AppSettings["defaultamount"];
                ShowMessage("c");
                smsg.Visible = true; 
            }
            else
            {
                lblAmount.Text = WebConfigurationManager.AppSettings["couponamount"].ToString(); 
            }
        }
	}
}
