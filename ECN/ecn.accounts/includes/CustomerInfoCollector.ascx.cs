namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;

	
	///		Summary description for CustomerInfoCollector.
	
	public partial class CustomerInfoCollector : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}

		public void SetCustomer(Customer customer) {	
			customer.Name = GeneralContact.Company;
			customer.GeneralContact = GeneralContact.Contact;
			customer.BillingContact = BillingContact.Contact;
			customer.TechContact = techContact.Text;
			customer.TechEmail = techEmail.Text;
			customer.TechPhone = techPhone.Text;
			customer.WebAddress = string.Empty;
		}

		public Customer Customer {
			set {
				LoadCustomerInfoIntoControls(value);
			}
		}

		private void LoadCustomerInfoIntoControls(Customer customer) {	
			GeneralContact.Company = customer.Name;			
			GeneralContact.Contact = customer.GeneralContact;
			BillingContact.Company = customer.Name;
			BillingContact.Contact = customer.BillingContact;
			techContact.Text = customer.TechContact;
			techEmail.Text = customer.TechEmail;
			techPhone.Text = customer.TechPhone;
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

		protected void lnkCopy_Click(object sender, System.EventArgs e) {
			BillingContact.Contact = GeneralContact.Contact;
		}
	}
}
