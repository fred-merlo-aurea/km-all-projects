using System;
using System.Configuration;
using System.Data;
using System.Web.Mail;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using ecn.common.classes;
using KM.Common;

namespace ecn.CanonTradeShow.ExSpcSalesSuppression 
{
    class MoveOptOutsToSuppression 
    {

        //public static StreamWriter logFile;
        //public static System.Text.StringBuilder logBuilder = new System.Text.StringBuilder();

        private static KMPlatform.Entity.User _User = null;

        static void Main(string[] args) 
        {
            try 
            {
                FileFunctions.LogConsoleActivity("");
                FileFunctions.LogConsoleActivity("");
                double addDays = Convert.ToDouble(ConfigurationManager.AppSettings["addDays"].ToString());

                _User = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);

                FileFunctions.LogConsoleActivity("--- JOB Started: " + DateTime.Now + " --- ");
                DataTable emailsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetUnsubscribesForCurrentBackToDay(Convert.ToInt32(addDays), Convert.ToInt32(ConfigurationManager.AppSettings["moveFromCustomerID"].ToString()));
                FileFunctions.LogConsoleActivity("Unsubscribes to Process: " + emailsDT.Rows.Count);

                if (emailsDT.Rows.Count > 0) 
                {
                    StringBuilder emailsToInsertSB = new StringBuilder();
                    emailsToInsertSB.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                    foreach (DataRow emailsDR in emailsDT.Rows) 
                    {
                        emailsToInsertSB.Append("<Emails><emailaddress>" + emailsDR["EmailAddress"] + "</emailaddress></Emails>");
                    }
                    emailsToInsertSB.Append("</XML>");

                    DataTable dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsOverrideCustomer(_User, Convert.ToInt32(ConfigurationManager.AppSettings["moveFromCustomerID"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["toCustomerIDMSListID"].ToString()), emailsToInsertSB.ToString(), "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>", "HTML", "S", true);
                } 
                else 
                {
                    FileFunctions.LogConsoleActivity(">>> MESSAGE - NO EMAILS UNSUBSCRIBED <<<");
                }
                FileFunctions.LogConsoleActivity("--- JOB Completed: " + DateTime.Now + " --- ");

            } 
            catch (Exception ex) 
            {
                FileFunctions.LogConsoleActivity("");
                FileFunctions.LogConsoleActivity("EXCEPTION: " + ex.ToString());
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ecn.CanonTradeShow.ExSpcSalesSuppression.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            }
        }

        #region Write to Log
        //public static void writeToLog(string text) {
        //    try {
        //        logFile.AutoFlush = true;
        //        logFile.WriteLine(DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + " " + text);
        //        logFile.Flush();
        //        logBuilder.Append(text + "\n");
        //    } catch { }
        //}
        #endregion

        #region Notify Admin
        //public static void notifyAdmin(string subject, string body) {
        //    try {
        //        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(ConfigurationSettings.AppSettings["smtpServer"].ToString());
        //        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(ConfigurationSettings.AppSettings["adminFromEmail"].ToString(), ConfigurationSettings.AppSettings["adminToEmail"].ToString(), subject, body);
        //        client.Send(message);
        //    } catch(Exception ex) {
        //        string a = ex.ToString();
        //    }
        //}
        #endregion
    }
}
