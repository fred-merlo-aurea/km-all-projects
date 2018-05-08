namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	
	///		Summary description for ContactEditor2.
	
	public partial class ContactEditor2 : System.Web.UI.UserControl
	{
		
		protected void Page_Load(object sender, System.EventArgs e) {
			
		}

		
		public string Company {
			get {
				return txtCompany.Text;
			}
			set {
				txtCompany.Text = value;
			}
		}

				
		public Contact Contact {
			get {
				return GetContact();
			}
			set {		
				SetContact(value);
			}
		}
		
		public bool ShowSameAsTechContact {
			get {
				return (chkIsTechInfo.Visible);
			}
			set {
				chkIsTechInfo.Visible = value;
			}
		}

		public bool ShowSameAsBillingAddress {
			get { return chkIsBillingAddress.Visible;}
			set { chkIsBillingAddress.Visible = value;}
		}

		public bool IsTheSameAsBillingAddress {
			get { return chkIsBillingAddress.Checked;}
		}

		public bool IsTheSameAsTechContact {
			get { return chkIsTechInfo.Checked;}
		}

		private void SetContact(Contact contact) {
			if (contact == null) {
				return;
			}
			ContactName.Text = contact.FirstName;
			txtLastName.Text = contact.LastName;
			ContactTitle.Text = contact.ContactTitle;
			Phone.Text = contact.Phone;
			Fax.Text = contact.Fax;
			Email.Text = contact.Email;
			Address.Text = contact.StreetAddress;
			City.Text = contact.City;
			ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(contact.State));
			Zip.Text = contact.Zip;
			Country.Text = contact.Country;				
		}
	
		private Contact GetContact() {
			return new Contact("Mr.", string.Format("{0} {1}", ContactName.Text, txtLastName.Text), ContactTitle.Text, Phone.Text, Fax.Text, Email.Text,
				Address.Text, City.Text, ddlState.SelectedValue, Country.Text, Zip.Text);
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
