using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Threading;
using ecn.common.classes;
using ecn.communicator.classes;

namespace ecn.EmailDirectEngine
{
    public class Program
    {
        KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);

        static void Main(string[] args)
        {
            Console.WriteLine("Email Direct Engine Start: " + DateTime.Now.ToString());
            while (true)
            {
                ECN_Framework_Entities.Communicator.EmailDirect edToSend = ECN_Framework_BusinessLayer.Communicator.EmailDirect.GetNextToSend();
                if (edToSend != null && edToSend.EmailDirectID > 0 && edToSend.Status.ToLower() == ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Pending.ToString().ToLower())
                {
                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.EmailDirect.UpdateStatus(edToSend.EmailDirectID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Active);
                        ProcessEmailDirect(edToSend);
                        Console.WriteLine("EmailDirectID: " + edToSend.EmailDirectID + " Finished: " + DateTime.Now.ToString());
                    }
                    catch (ECN_Framework_Common.Objects.ECNException excEx)
                    {
                        string note = string.Empty;
                        foreach (ECN_Framework_Common.Objects.ECNError ecnError in excEx.ErrorList)
                        {
                            note = note + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                        }
                        KM.Common.Entity.ApplicationLog.LogCriticalError(excEx, string.Format("EmailDirect engine({0}) encountered an exception when ProcessEmailDirect.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), string.Format("An exception Happened when handling EmailDirect ID = {5} and Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0} Validation Error: {5}{0}",
                            string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                            excEx.Message, excEx.Source, excEx.StackTrace, excEx.InnerException,
                             note));
                        ECN_Framework_BusinessLayer.Communicator.EmailDirect.UpdateStatus(edToSend.EmailDirectID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Error);
                        Console.WriteLine(excEx.Message);
                    }
                    catch (Exception e)
                    {
                        KM.Common.Entity.ApplicationLog.LogCriticalError(e, string.Format("EmailDirect engine({0}) encountered an exception when ProcessEmailDirect.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), string.Format("An exception Happened when handling EmailDirect ID = {5}, Process = {6}, Source = {7}, and Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}",
                            string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                            e.Message, e.Source, e.StackTrace, e.InnerException, edToSend.EmailDirectID.ToString(),edToSend.Process, edToSend.Source
                             ));
                        ECN_Framework_BusinessLayer.Communicator.EmailDirect.UpdateStatus(edToSend.EmailDirectID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Error);
                        Console.WriteLine(e.Message);
                    }
                }

                Thread.Sleep(2000);
            }
        }

        private static string CreateNote()
        {
            StringBuilder sbNote = new StringBuilder();

            return sbNote.ToString();
        }

        private static void ProcessEmailDirect(ECN_Framework_Entities.Communicator.EmailDirect EmailDirect)
        {

            Console.WriteLine(string.Format("Email Direct Engine({1}) starts at {0}", DateTime.Now.ToString(), System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
            Console.WriteLine(string.Format("With additonal StatusCode = {0}, EmailDirectID = {1}", EmailDirect.Status, EmailDirect.EmailDirectID));

            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"].ToString()));
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            string[] emails = EmailDirect.EmailAddress.Split(',');
            foreach (string s in emails)
            {
                message.To.Add(s);
            }
            message.Priority = MailPriority.High;
            MailAddress ma = new MailAddress(EmailDirect.FromEmailAddress, !string.IsNullOrEmpty(EmailDirect.FromName) ? EmailDirect.FromName : "");
            
            message.From = ma;

            message.Subject = EmailDirect.EmailSubject;
            message.ReplyToList.Add(new MailAddress(EmailDirect.ReplyEmailAddress));
            //message.Body = EmailDirect.Content;
            message.Body = SetupEmailContent(EmailDirect.Content, EmailDirect.EmailSubject, EmailDirect.EmailDirectID);

            smtp.Send(message);
            ECN_Framework_BusinessLayer.Communicator.EmailDirect.UpdateStatus(EmailDirect.EmailDirectID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Sent);

        }

        public static int ProcessEmailDirectWithAttachments(ECN_Framework_Entities.Communicator.EmailDirect ed)
        {
            int edID = -1;
            try
            {
                Console.WriteLine(string.Format("Email Direct Engine({0}) starts at {1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToString()));
                
                if (ed.EmailDirectID < 0)
                    edID = ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                else
                    edID = ed.EmailDirectID;

                Console.WriteLine(string.Format("With additional StatusCode = {0}, EmailDirectID = {1}", ed.Status, edID));
               
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"].ToString()));
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                string[] emails = ed.EmailAddress.Split(',');
                foreach(string s in emails)
                {
                    message.To.Add(s);
                }
                
                message.From = new MailAddress(string.IsNullOrEmpty(ed.FromEmailAddress) ? "emaildirect@ecn5.com" : ed.FromEmailAddress,ed.FromName);
                
                message.Subject = ed.EmailSubject;
                message.ReplyToList.Add(new MailAddress(ed.ReplyEmailAddress));
                message.Body = SetupEmailContent(ed.Content, ed.EmailSubject, edID);
                if (ed.CCAddresses != null && ed.CCAddresses.Count > 0)
                {
                    foreach (string email in ed.CCAddresses)
                    {
                        message.CC.Add(email);
                    }
                }

                if (ed.Attachments != null)
                {
                    foreach (Attachment attachment in ed.Attachments)
                    {
                        message.Attachments.Add(attachment);
                    }
                }
                smtp.Send(message);
                ECN_Framework_BusinessLayer.Communicator.EmailDirect.UpdateStatus(edID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Sent);

                Console.WriteLine("EmailDirectID: " + edID.ToString() + " Finished: " + DateTime.Now.ToString());
                return edID;
            }
            catch (ECN_Framework_Common.Objects.ECNException excEx)
            {
                string note = string.Empty;
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in excEx.ErrorList)
                {
                    note = note + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                KM.Common.Entity.ApplicationLog.LogCriticalError(excEx, string.Format("EmailDirect engine({0}) encountered an exception when ProcessEmailDirect.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), string.Format("An exception Happened when handling EmailDirect ID = {5} and Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0} Validation Error: {5}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    excEx.Message, excEx.Source, excEx.StackTrace, excEx.InnerException,
                     note));
                ECN_Framework_BusinessLayer.Communicator.EmailDirect.UpdateStatus(ed.EmailDirectID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Error);
                Console.WriteLine(excEx.Message);

                return -1;
            }
            catch (Exception e)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(e, string.Format("EmailDirect engine({0}) encountered an exception when ProcessEmailDirect.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), string.Format("An exception Happened when handling EmailDirect ID = {5}, Process = {6}, Source = {7}, and Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    e.Message, e.Source, e.StackTrace, e.InnerException, edID.ToString(), ed.Process, ed.Source
                     ));
                ECN_Framework_BusinessLayer.Communicator.EmailDirect.UpdateStatus(edID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status.Error);
                Console.WriteLine(e.Message);

                return -1;
            }
            

        }

        private static string SetupEmailContent(string content, string subject, int EmailDirectID)
        {
            string htmlBody = TemplateFunctions.addOpensImageForDirectEmail(content, EmailDirectID);
            return htmlBody;
        }
        
      
    }
}
