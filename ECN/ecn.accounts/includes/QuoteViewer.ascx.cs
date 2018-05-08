namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Configuration;
	using System.Web;
	using System.Net.Mail;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	using ecn.common.classes.billing;

	
	///		Summary description for QuoteViewer.
	
	public partial class QuoteViewer : System.Web.UI.UserControl {

		protected void Page_Load(object sender, System.EventArgs e) {
		}	

		private string TermsAndConditionLink {
			get { return "<a href=http://www.knowledgemarketing.com/terms.php>KMLLC Services Agreement.</a>";}
		}

		public void ShowHeader(Quote quote, User user) {

            if (quote.Customer.ID > 0)
            {
                chkAgree2.Checked = true;
                phOtherInfo.Visible = false;
            }

			//Staff Staffcreated = Staff.GetStaffByUserID(quote.CreatedUserID);
            KMPlatform.Entity.User Staffcreated = new KMPlatform.BusinessLogic.User().SelectUser(quote.CreatedUserID);
			string testingParagraph = string.Format(ConfigurationManager.AppSettings["testAccountVerbiage"].ToString(),TermsAndConditionLink);

			string approveQuote = ConfigurationManager.AppSettings["approveVerbiage"].ToString();

            ltlDearCustomer.Text = string.Format(ConfigurationManager.AppSettings["mainVerbiage"].ToString(), quote.FirstName, quote.Notes, quote.HasTestAccount ? testingParagraph : approveQuote, Staffcreated.FirstName);

			testingLogin.Visible =  quote.HasTestAccount;
			UserEditor.Visible = !quote.HasTestAccount;
			if (quote.HasTestAccount) {
				testingLogin.TermsAndConditionLink = TermsAndConditionLink;
				testingLogin.LoginUrl = string.Format("../includes/login.aspx?user={0}&password={1}", quote.TestUserName, quote.TestPassword);
				lblTerms.Text = "I agree to the Knowledge Marketing Core Services Agreement and the related Service <a href='http://www.knowledgemarketing.com/terms.php' target='_blank'>Terms and Conditions</a>.";
			} else {
				UserEditor.User = user;
				if (quote.Customer.IsNew) {
					lblTerms.Text = "I agree to the Knowledge Marketing Core Services Agreement and the related Service <a href='http://www.knowledgemarketing.com/terms.php' target='_blank'>Terms and Conditions</a>.";
				} else {
					lblTerms.Text = "I agree that this quote will be governed by our ongoing Knowledge Marketing Core Services Agreement and related Service <a href='http://www.knowledgemarketing.com/terms.php' target='_blank'>Terms and Conditions</a>.";
				}
			}			
		}

		public ContactEditor2 ContactEditor {
			get { return Contact2;}
		}

		public UserInfoCollector UserInfoEditor {
			get { return UserEditor;}
		}

		public bool AgreeToTermsAndConditions {
			get { return chkAgree2.Checked;}
		}		

		public DateTime StartDate{
			get { return DateTime.Now.AddDays(Convert.ToInt32(ddlStartDate.SelectedValue));}
		}

		private Quote _quote;
		public Quote Quote {
			get {
				return (this._quote);
			}
			set {
				this._quote = value;
				DisplayQuote(_quote);
			}
		}


		private void DisplayQuote(Quote quote) {	
			InitializeQuoteControl(quote);
			DisplayTotal(quote);
		}
	
		private void InitializeQuoteControl(Quote quote) {
			dgdQuoteItems.DataSource = quote.Items;
			dgdQuoteItems.DataBind();
		}

		private void DisplayTotal(Quote quote) {
			bool hasSaving = quote.OneTimeTotalSaving>0||quote.MonthTotalSaving>0||quote.QuarterTotalSaving>0||quote.AnnualTotalSaving>0;
		    
			double oneTimeTotal = quote.OneTimeTotal;
			double monthlyTotal = quote.MonthTotal;
			double quarterlyTotal = quote.QuarterTotal;
			double annualTotal = quote.AnnualTotal;

			if (hasSaving) {
				oneTimeTotal += quote.OneTimeTotalSaving;
				monthlyTotal += quote.MonthTotalSaving;
				quarterlyTotal += quote.QuarterTotalSaving;
				annualTotal += quote.AnnualTotalSaving;
			}
            
			/// display total
			lblOneTimeFees.Text = oneTimeTotal.ToString("$###,##0.00");
			lblMonthlyFees.Text = monthlyTotal.ToString("$###,##0.00");
			lblQuarterlyFees.Text = quarterlyTotal.ToString("$###,##0.00");
			lblAnnualFees.Text = annualTotal.ToString("$###,##0.00");			
			
			lblDiscount.Visible = lblNetAmount.Visible = hasSaving;
			/// Show discount:
			lblOneTimeSaving.Text = (!hasSaving)?" ":quote.OneTimeTotalSaving.ToString("($###,##0.00)");
			lblMonthlySaving.Text = (!hasSaving)?" ":quote.MonthTotalSaving.ToString("($###,##0.00)");
			lblQuarterlySaving.Text = (!hasSaving)?" ":quote.QuarterTotalSaving.ToString("($###,##0.00)");
			lblAnnualSaving.Text = (!hasSaving)?" ":quote.AnnualTotalSaving.ToString("($###,##0.00)");		
			
			/// Show Net Amount:
			lblOneTimeNetAmount.Text = (!hasSaving)?" ":quote.OneTimeTotal.ToString("$###,##0.00");
			lblMonthlyNetAmount.Text = (!hasSaving)?" ":quote.MonthTotal.ToString("$###,##0.00");
			lblQuarterlyNetAmount.Text = (!hasSaving)?" ":quote.QuarterTotal.ToString("$###,##0.00");
			lblAnnualNetAmount.Text = (!hasSaving)?" ":quote.AnnualTotal.ToString("$###,##0.00");
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion

		private void dgdQuoteItems_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
