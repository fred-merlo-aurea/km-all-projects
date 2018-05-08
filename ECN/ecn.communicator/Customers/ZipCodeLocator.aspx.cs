using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Net.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.common.classes;
using ecn.communicator.classes;
using ecn.communicator.classes.Customers;

namespace ecn.communicator.Customers {
	
	
	
	public partial class ZipCodeLocator : System.Web.UI.Page {
		protected void Page_Load(object sender, System.EventArgs e) {
			SendEmail(EmailID);
			Redirect(ReturnUrl);
		}


		#region Http Reqeust Vars
		private int EmailID {
			get {
				return Convert.ToInt32(Request["eid"]);
			}
		}		

		private string ReturnUrl {
			get {
				return Convert.ToString(Request["ReturnUrl"]);
			}
		}
		#endregion

		private void SendEmail(int emailID) {
			Emails email = Emails.GetEmailByID(emailID);
			if (email == null) {
				NotifyAdmin("Can't find email.", "Email ID = " + emailID);
				return;
			}			

			IZipCodeLocator locator = new SecondWindZipCodeLocator();
			int storeID = locator.GetIDOfNearestObject(email.Zip);
			Store store = Store.GetStoreByID(storeID);

			if (store == null) {
				NotifyAdmin("Can't find store.", string.Format("Zip code: {0}, Store ID: {1}", email.Zip, storeID));
				return;
			}

			try {
				
				MailMessage message = new MailMessage();
				message.To.Add(store.Email);
				message.CC.Add("Ashok@teckman.com");
				message.From = new MailAddress("renrico@2ndwindexercise.com");
				message.Subject = "Test zip code locator: To: " + store.Email;
				message.Body = GetEmailBody(email);

                SmtpClient smtp = new SmtpClient("localhost");
                smtp.Send(message);

			} catch (Exception e) {
				NotifyAdmin("Can't send out email to store.", GetEmailBody(email) + e.Message);
			}
		}

		private string GetEmailBody(Emails email) {
			string body = string.Format(@"{0} {1} has submitted a contact form.
Contact Information:
Name: {0} {1}
Address: {2}
{3},{4} {5}
Phone: {6}
Fax: {7}
Email: {8}
", email.FirstName,email.LastName, email.Address, email.City, email.State, email.Zip, email.Voice, email.Fax, email.Email); 
			return body;
		}

		
		private void Redirect(string returnUrl) {
			if (returnUrl != null && returnUrl.Trim().Length!=0) {
				Response.Redirect(returnUrl);
			}
		}

		private void NotifyAdmin(string subject, string body) {
			
			MailMessage message = new MailMessage();
			message.To.Add("Ashok@teckman.com");
			message.From = new MailAddress("domain_admin@teckman.com");
			message.Subject = "2nd wind Zip Code Location:" + subject;
			message.Body = body;

            SmtpClient smtp = new SmtpClient("localhost");
            smtp.Send(message);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    
		}
		#endregion
	}
}
