using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using ecn.common.classes;
using ecn.communicator.classes;
using ecn.accounts.classes;

namespace ecn.accounts.classes {

	public class NotifyBillingDept {

		#region Properties
		private static bool _notifyBillingAdmin	= false;
		public static bool NotifyBillingAdmin {
			get { return (_notifyBillingAdmin);}
			set { _notifyBillingAdmin = value;	}
		}

		private static Staff _staff	= null;
		public static Staff StaffInfo {
			get { return (_staff);}
			set { _staff = value;	}
		}

		private static string _subject	= "";
		public static string EmailSubject {
			get { return (_subject);}
			set { _subject = value;	}
		}

		private static string _body	= "";
		public static string EmailBody {
			get { return (_body);}
			set { _body = value;	}
		}

		#endregion

		//public NotifyBillingDept() {}

		public static void notifyNBDonQuoteStatus(){
			MailMessage message = new MailMessage();
			message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["NotifyBillingDept_FROM_EMAIL"]);
			
            message.To.Add(StaffInfo.FromEmailAddress);

			if(NotifyBillingAdmin){
				message.CC.Add(System.Configuration.ConfigurationManager.AppSettings["NotifyBillingDept_EMAIL"]);
			}
			message.Subject = EmailSubject;
			message.Body = EmailBody;
			message.IsBodyHtml= true;
			message.Priority = MailPriority.High;
            
            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
			//end sending mail to the Account Executive.
		}

		public static void notifyBillingDept(){
			
			MailMessage message = new MailMessage();
			message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["NotifyBillingDept_FROM_EMAIL"]);
			message.To.Add(System.Configuration.ConfigurationManager.AppSettings["NotifyBillingDept_EMAIL"]);
			message.Subject = EmailSubject;
			message.Body = EmailBody;
            message.IsBodyHtml = true;
			message.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
			//end sending mail to the Account Executive.
		}
	}
}
