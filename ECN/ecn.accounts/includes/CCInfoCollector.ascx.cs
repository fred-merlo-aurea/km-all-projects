namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	using ecn.common.classes.billing;


	public partial class CCInfoCollector : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}

		public CreditCard  CreditCard{
			get {
				CreditCard cc= new CreditCard(txtCreditCardNumber.Text, ddlCreditCardType.SelectedValue, new DateTime(Convert.ToInt32(ddlYear.SelectedValue),Convert.ToInt32(ddlMonth.SelectedValue), 1) , txtSecurityNumber.Text);
				cc.BillingContact = BillingContact.Contact;
				return cc;
			}
			set {				
				BillingContact.Contact = value.BillingContact;
				LoadCCInfoIntoControls(value);		
			}
		}

		public bool AgreeToUseRecurringService {
			get { return chkAgree.Checked;}
		}

		public ContactEditor2 ContactEditor {
			get { return BillingContact;}			
		}

		private void LoadCCInfoIntoControls(CreditCard cc) {
			txtCreditCardNumber.Text = cc.CardNumber;
			ddlCreditCardType.SelectedIndex = ddlCreditCardType.Items.IndexOf(ddlCreditCardType.Items.FindByValue(cc.CardType));
			ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByValue(cc.ExpirationDate.Year.ToString()));
			ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(cc.ExpirationDate.Month.ToString()));
			txtSecurityNumber.Text = cc.SecurityNumber;
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
	}
}
