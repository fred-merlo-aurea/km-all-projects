using System.Data;
using System.Data.SqlClient;
using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.Collections.Generic;

//using System.Collections.Generic;
//using System.Web.UI;
//using System.Web.UI.WebControls;

namespace ecn.activityengines
{
    public class Helper
    {
        //public static void LogCriticalError(Exception ex, string sourceMethod)
        //{
        //    KM.Common.Entity.ApplicationLog log = new KM.Common.Entity.ApplicationLog();
        //    log.ApplicationID = KM.Common.Entity.Application.GetByApplicationName(KM.Common.Entity.Application.Applications.ECN_Activity_Engine).ApplicationID;
        //    log.SeverityID = 1;
        //    log.Exception = KM.Common.Entity.ApplicationLog.FormatException(ex);
        //    log.NotificationSent = false;
        //    log.SourceMethod = sourceMethod;
        //    KM.Common.Entity.ApplicationLog.Save(ref log);
        //}

        //public static void LogNonCriticalError(string error, string sourceMethod)
        //{
        //    KM.Common.Entity.ApplicationLog log = new KM.Common.Entity.ApplicationLog();
        //    log.ApplicationID = KM.Common.Entity.Application.GetByApplicationName(KM.Common.Entity.Application.Applications.ECN_Activity_Engine).ApplicationID;
        //    log.SeverityID = 2;
        //    log.Exception = error;
        //    log.NotificationSent = false;
        //    log.SourceMethod = sourceMethod;
        //    KM.Common.Entity.ApplicationLog.Save(ref log);
        //}

        //public static bool ValidForTracking(int blastID, int emailID)
        //{
        //    bool track = false;

        //    if (blastID == 0 || emailID == 0)
        //    {
        //        return track;
        //    }

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "select COUNT(eg.EmailID) " +
        //                        "from Blast b with (nolock) " +
        //                            "join EmailGroups eg with (nolock) on b.GroupID = eg.GroupID " +
        //                        "where b.BlastID = @BlastID and eg.EmailID = @EmailID and b.StatusCode <> 'Deleted'";
        //    cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
        //    cmd.Parameters["@EmailID"].Value = emailID;
        //    cmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
        //    cmd.Parameters["@BlastID"].Value = blastID;
        //    int resultCount = 0;
        //    try
        //    {
        //        resultCount = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd).ToString());
        //        if (resultCount > 0)
        //        {
        //            track = true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return track;
        //}

        

        public static string DeCrypt_DeCode_EncryptedQueryString(string encrypted_Qstring)
        {
            string decryptedQuery = string.Empty;
            string encryptedQuery = string.Empty;
            KM.Common.Entity.Encryption ec = null;
            try
            {
                //KM.Common.Entity.Encryption ec = new KM.Common.Entity.Encryption();
                ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));

                encryptedQuery = System.Web.HttpUtility.UrlDecode(encrypted_Qstring);
                decryptedQuery = KM.Common.Encryption.Decrypt(encryptedQuery, ec);
            }
            catch (Exception)
            {
                string[] corrections = ConfigurationManager.AppSettings["encryption_correctionAppendages"].Split(',');
                foreach(string correction in corrections)
                {
                    try
                    {
                        // If it fails, it will throw and continue.
                        // If it doesn't, it should break and return.
                        decryptedQuery = KM.Common.Encryption.Decrypt(encryptedQuery + correction, ec); 
                        break;
                    }
                    catch (Exception) { }
                }
            }
            return decryptedQuery;
        }

        public static string Encrypt_UrlEncode_QueryString(string qstring)
        {
            string encryptedQuery = string.Empty;
            try
            {
                //KM.Common.Entity.Encryption ec = new KM.Common.Entity.Encryption();
                KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(qstring, ec));
                //encryptedQuery = System.Web.HttpUtility.UrlEncode(ec.Encrypt(qstring, ec.PassPhrase, ec.SaltValue, ec.HashAlgorithm, ec.PasswordIterations, ec.InitVector, ec.KeySize));
            }
            catch (Exception)
            {
            }
            return encryptedQuery;
        }

    }



    public class Enums
    {
        public enum ErrorMessage
        {
            HardError,
            InvalidLink,
            PageNotFound,
            Timeout,
            Unknown
        }
    }

    public class ActivityError
    {
        public static string GetErrorMessage(Enums.ErrorMessage messageType, string message = "")
        {
            string errorMessage = string.Empty;
            switch (messageType)
            {
                case Enums.ErrorMessage.HardError:
                    errorMessage = "Sorry! We're having trouble processing your request right now.<br><br>If you typed the link in your email browser, please either retype the link making note of case sensitivity ('a' and 'A' are different), or click on the link in the original email you received.<br><br>If you clicked on the link in a text version email, it may have been broken by your email program. Please reply to the email you received and let us know which link caused a problem. We’ll correct the problem and send you a replacement link.<br><br>Sincerely,<br>Customer Support";
                    break;
                case Enums.ErrorMessage.InvalidLink:
                    errorMessage = "We're sorry, but the link you are requesting appears to be invalid.<br><br>If you typed the link in your email browser, please either retype the link making note of case sensitivity ('a' and 'A' are different), or click on the link in the original email you received.<br><br>If you clicked on the link in a text version email, it may have been broken by your email program. Please reply to the email you received and let us know which link caused a problem. We’ll correct the problem and send you a replacement link.<br><br>Sincerely,<br>Customer Support";
                    break;
                case Enums.ErrorMessage.PageNotFound:
                    errorMessage = "Sorry! The page you have requested does not exist.<br><br>If you typed the link in your email browser, please either retype the link making note of case sensitivity ('a' and 'A' are different), or click on the link in the original email you received.<br><br>If you clicked on the link in a text version email, it may have been broken by your email program. Please reply to the email you received and let us know which link caused a problem. We’ll correct the problem and send you a replacement link.<br><br>Sincerely,<br>Customer Support";
                    break;
                case Enums.ErrorMessage.Timeout:
                    errorMessage = "Sorry! We're having trouble processing your request right now.<br><br>If you typed the link in your email browser, please either retype the link making note of case sensitivity ('a' and 'A' are different), or click on the link in the original email you received.<br><br>If you clicked on the link in a text version email, it may have been broken by your email program. Please reply to the email you received and let us know which link caused a problem. We’ll correct the problem and send you a replacement link.<br><br>Sincerely,<br>Customer Support";
                    break;
                default:
                    if (message != string.Empty)
                        errorMessage = message;
                    else
                        errorMessage = "Sorry! We're having trouble processing your request right now.<br><br>If you typed the link in your email browser, please either retype the link making note of case sensitivity ('a' and 'A' are different), or click on the link in the original email you received.<br><br>If you clicked on the link in a text version email, it may have been broken by your email program. Please reply to the email you received and let us know which link caused a problem. We’ll correct the problem and send you a replacement link.<br><br>Sincerely,<br>Customer Support";
                    break;
            }

            return errorMessage;
        }

        public static string CreateMessage(Exception e, HttpRequest request, string variables)
        {
            string message = string.Empty;

            string taskStack = e.Message + "\n" + e.Source + "\n" + e.StackTrace + "\n" + e.InnerException;

            string addressbarURL = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + request.RawUrl.ToString();
            
            StringBuilder notifyEmailBody = new StringBuilder();

            notifyEmailBody.AppendLine("<table width=100% border=0><tr><td style=\"font-family:'verdana'; font-size:11;\">");
            notifyEmailBody.AppendLine("<b>" + e.Message.ToString() + "</b><br>");
            notifyEmailBody.AppendLine(variables);
            string spyinfo = "";
            if(request != null)
            {
                if (request.UserHostAddress != null)
                {
                    spyinfo = "[" + request.UserHostAddress + "]";
                }
                if (request.UserAgent != null)
                {
                    if (spyinfo.Length > 0)
                    {
                        spyinfo += " / ";
                    }
                    spyinfo += "[" + request.UserAgent + "]";
                }   
            }
            notifyEmailBody.AppendLine("<br><b>SPY Info:</b>&nbsp;" + spyinfo);
            notifyEmailBody.AppendLine("<br><b>Raw URL Path:</b>&nbsp;" + addressbarURL);
            if (request.UrlReferrer != null)
            {
                notifyEmailBody.AppendLine("<br><b>Referring URL:</b>&nbsp;" + request.UrlReferrer.ToString());
            }
            notifyEmailBody.AppendLine("<br><br><b>Task Stack:</b><br>" + taskStack.Replace("\n", "<br>").ToString());
            notifyEmailBody.AppendLine("</td></tr></table>");

            return notifyEmailBody.ToString();
        }
    }
}