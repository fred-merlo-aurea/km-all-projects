namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;


	
	///		Summary description for ContactEditor.
	
	public partial class ContactEditor : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}

        public ECN_Framework_Entities.Accounts.Contact Contact
        {
			get {
				return GetContact();
			}
			set {		
				SetContact(value);
			}
		}

        private void SetContact(ECN_Framework_Entities.Accounts.Contact contact)
        {
			if (contact == null) {
				return;
			}
			txtFirstName.Text = contact.FirstName;
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
			ddlSalutation.SelectedValue = contact.Salutation.ToString();
		}

        private ECN_Framework_Entities.Accounts.Contact GetContact()
        {
            return new ECN_Framework_Entities.Accounts.Contact(ddlSalutation.SelectedValue, txtFirstName.Text, txtLastName.Text, ContactTitle.Text, Phone.Text, Fax.Text, Email.Text,
				Address.Text, City.Text, ddlState.SelectedValue, Country.Text, Zip.Text);
		}
	}
}
