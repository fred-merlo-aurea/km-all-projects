using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;

namespace KM.Common
{
    public class EmailFunctions
    {
        

        public static string NotifyAdmin(string subject, string messagetext)
        {
            int applicationID = 0;
            string smtpServer = string.Empty;
            string fromEmail = string.Empty;

            string errorMessage = string.Empty;

            if (ConfigurationManager.AppSettings["KMCommon_Application"] == null || ConfigurationManager.AppSettings["KMCommon_SmtpServer"] == null)
            {
                errorMessage = "Config settings missing for EmailFunctions.NotifyAdmin";
            }
            else
            {
                int.TryParse(ConfigurationManager.AppSettings["KMCommon_Application"], out applicationID);
                smtpServer = ConfigurationManager.AppSettings["KMCommon_SmtpServer"];

                if (applicationID == 0 || smtpServer == string.Empty)
                {
                    errorMessage = "Config settings invalid for EmailFunctions.NotifyAdmin";
                }
                else
                {
                    Entity.Application app = Entity.Application.GetByApplicationID(applicationID);

                    if (app == null)
                    {
                        errorMessage = "Application not found for EmailFunctions.NotifyAdmin";
                    }
                    else
                    {
                        List<Entity.User> userList = Entity.User.GetByApplicationID(applicationID);

                        if (userList.Count == 0)
                        {
                            errorMessage = "Users not found for EmailFunctions.NotifyAdmin";
                        }
                        else
                        {
                            MailMessage message = new MailMessage();
                            message.From = new MailAddress(app.FromEmailAddress);
                            message.Subject = subject;
                            message.Body = messagetext;
                            message.IsBodyHtml = true;

                            bool firstRecord = true;
                            foreach (Entity.User user in userList)
                            {
                                if (firstRecord)
                                {
                                    message.To.Add(user.EmailAddress);
                                    firstRecord = false;
                                }
                                message.CC.Add(user.EmailAddress);
                            }

                            SmtpClient client = new SmtpClient(smtpServer);
                            try
                            {
                                client.Send(message);
                                errorMessage = string.Empty;
                            }
                            catch (Exception ex)
                            {
                                errorMessage = ex.ToString();
                            }                            
                        }
                    }
                }
            }
            return errorMessage;
        }

        public static string NotifyAdmin(KM.Common.Entity.ApplicationLog al)
        {
            int applicationID = 0;
            string smtpServer = string.Empty;
            string fromEmail = string.Empty;

            string errorMessage = string.Empty;

            try
            {
                if (ConfigurationManager.AppSettings["KMCommon_Application"] == null || ConfigurationManager.AppSettings["KMCommon_SmtpServer"] == null)
                {
                    errorMessage = "Config settings missing for EmailFunctions.NotifyAdmin";
                }
                else
                {
                    int.TryParse(ConfigurationManager.AppSettings["KMCommon_Application"], out applicationID);
                    if (applicationID != al.ApplicationID)
                        applicationID = al.ApplicationID;
                    smtpServer = ConfigurationManager.AppSettings["KMCommon_SmtpServer"];

                    if (applicationID == 0 || smtpServer == string.Empty)
                    {
                        errorMessage = "Config settings invalid for EmailFunctions.NotifyAdmin";
                    }
                    else
                    {
                        Entity.Application app = Entity.Application.GetByApplicationID(applicationID);

                        if (app == null)
                        {
                            errorMessage = "Application not found for EmailFunctions.NotifyAdmin";
                        }
                        else
                        {
                            List<Entity.User> userList = Entity.User.GetByApplicationID(applicationID);

                            if (userList.Count == 0)
                            {
                                errorMessage = "Users not found for EmailFunctions.NotifyAdmin";
                            }
                            else
                            {
                                MailMessage message = new MailMessage();
                                message.From = new MailAddress(app.FromEmailAddress);
                                message.Subject = "Severity 1 Application Error - LogID: " + al.LogID.ToString() + " for Application: " + app.ApplicationName;
                                message.Body = al.Exception + "<BR>Log Note: " + al.LogNote;
                                message.IsBodyHtml = true;
                                message.Priority = MailPriority.High;

                                bool firstRecord = true;
                                foreach (Entity.User user in userList)
                                {
                                    if (firstRecord)
                                    {
                                        message.To.Add(user.EmailAddress);
                                        firstRecord = false;
                                    }
                                    else
                                    {
                                        message.CC.Add(user.EmailAddress);
                                    }
                                }

                                SmtpClient client = new SmtpClient(smtpServer);
                                client.Send(message);
                                errorMessage = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            return errorMessage;
        }

        public static void SendMailMessage(string from, List<string> toList, string subject, string body, bool isHtml, MailPriority mp, string mailServer)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from);
            foreach (string s in toList)
            {
                msg.To.Add(s);
            }
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = isHtml;
            msg.Priority = mp;

            SmtpClient smtp = new SmtpClient(mailServer);
            smtp.Send(msg);
        }
    }
}
