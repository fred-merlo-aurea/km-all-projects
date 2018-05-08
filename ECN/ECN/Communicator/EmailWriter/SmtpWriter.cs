//using ecn.common.classes;
//using ecn.communicator.classes;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ecn.communicator.classes.EmailWriter
//{

//    #region Class SmtpWriter
//    public class SmtpWriter
//    {

//        #region Class properties
//        private string _serverSmtp = "";
//        public string SmtpServer
//        {
//            get { return _serverSmtp; }
//            set { _serverSmtp = value; }
//        }

//        private int _portSmtp = 25;
//        public int SmtpPort
//        {
//            get { return _portSmtp; }
//            set { _portSmtp = value; }
//        }

//        private string _userSmtp = "";
//        public string SmtpUser
//        {
//            get { return _userSmtp; }
//            set { _userSmtp = value; }
//        }

//        private int _resetCon = 121;
//        public int ResetConnection
//        {
//            get { return _resetCon; }
//            set { _resetCon = value; }
//        }
//        #endregion


//        public SmtpWriter()
//        {
//        }

//        public void SendEmail(string EmailFrom, string EmailTo, string EmailMessage, string EmailLog)
//        {
//            try
//            {
//                EmailHolder tmp = new EmailHolder();
//                tmp.frm_eml = EmailFrom;
//                tmp.to_eml = EmailTo;
//                tmp.msg = EmailMessage;
//                tmp.log_line = EmailLog;
//                count++;
//            }
//            catch (Exception ex)
//            {
//                SendEmailNotification("Error in IronPortEmailWriter.SmtpWriter.SendEmail(string EmailFrom, string EmailTo, string EmailMessage, string EmailLog)", ex.ToString());
//                throw;
//            }
//        }

//        public void SendEmail(DataRow dr, DataRow[] drArray)
//        {

            
//        }

//        private static void SendEmailNotification(string subject, string body)
//        {
//            MailMessage message = new MailMessage();
//            message.From = new MailAddress("domain_admin@teckman.com");
//            message.To.Add(ConfigurationManager.AppSettings["SendTo"]);
//            message.Subject = "Engine: " + System.AppDomain.CurrentDomain.FriendlyName.ToString() + " - " + subject;
//            message.Body = body;

//            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
//            smtp.Send(message);
//        }
//    }
//}
//#endregion