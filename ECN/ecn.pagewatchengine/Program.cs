using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Threading;
using ecn.common.classes;
using ecn.communicator.classes;
using System.Collections.Generic;
using System.Linq;

namespace ecn.pagewatchengine
{
    class PageWatchEngine
    {
        static void Main(string[] args)
        {
            try
            {
                List<AdminEmail> aeList = new List<AdminEmail>();
                List<PageWatch> pwList = PageWatch.GetPageRecordsForEngine(PageWatch.ScheduleType.Scheduled, PageWatchTag.ChangeType.Unchanged);
                if (pwList.Count > 0)
                {
                    Console.WriteLine(string.Format("{0} Page Watch records to check", pwList.Count.ToString()));
                    foreach (PageWatch pw in pwList)
                    {
                        string errorMessage = PageWatchTag.ValidateAllTags(pw.PWT, pw.URL);
                        if (errorMessage == string.Empty)
                        {
                            foreach (PageWatchTag pwt in pw.PWT)
                            {
                                Console.WriteLine(string.Format("Processing Tag ID {0}", pwt.PageWatchTagID.ToString()));
                                string currentHTML = PageWatchTag.GetCurrentHTML(pwt.WatchTag, pw.URL);
                                string previousHTML = pwt.PreviousHTML;
                                if (!ecn.communicator.classes.PageWatchTag.CompareHTML(previousHTML, currentHTML))
                                {
                                    PageWatchTag.UpdateTagRecord(pwt.PageWatchTagID);
                                    if (aeList.Exists(x => x.AdminUserID == pw.AdminUserID) == true)
                                    {
                                        aeList.Single(x => x.AdminUserID == pw.AdminUserID).PageChanges.Add("Page Name: " + pw.Name + " Tag Name: " + pwt.Name + "<br />");
                                    }
                                    else
                                    {
                                        AdminEmail aeNew = new AdminEmail();
                                        aeNew.AdminUserID = pw.AdminUserID;
                                        aeNew.UserName = pw.UserName;
                                        List<string> toAdd = new List<string>();
                                        toAdd.Add("Page Name: " + pw.Name + " Tag Name: " + pwt.Name + "<br />");
                                        aeNew.PageChanges = toAdd;
                                        aeList.Add(aeNew);
                                    }
                                }
                            }
                            PageWatch.UpdatePageRecord(pw.PageWatchID, pw.NextScheduleTime);
                        }
                        else
                        {
                            Console.WriteLine("Error: " + errorMessage);
                            PageWatch.SetInactive(pw.PageWatchID);
                            SendEmailNotification("Page Watch Engine encountered a problem", errorMessage + " for PageWatchID: " + pw.PageWatchID.ToString());
                        }
                    }
                    if (aeList.Count > 0)
                    {
                        SendAdminEmail(aeList, false);
                    }
                }
                else
                {
                    Console.WriteLine("No Page Watches to check");
                }
            }
            catch (Exception e)
            {
                SendEmailNotification("Page Watch engine encountered an exception",
                    string.Format("Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    e.Message, e.Source, e.StackTrace, e.InnerException));
            }
        }

        public static void ProcessReminders()
        {
            try
            {
                List<AdminEmail> aeList = new List<AdminEmail>();
                List<PageWatch> pwList = PageWatch.GetPageRecordsForEngine(PageWatch.ScheduleType.Both, PageWatchTag.ChangeType.Changed );
                if (pwList.Count > 0)
                {
                    Console.WriteLine(string.Format("{0} Page Watch reminders records to check", pwList.Count.ToString()));
                    foreach (PageWatch pw in pwList)
                    {
                        foreach (PageWatchTag pwt in pw.PWT)
                        {
                            if ((((DateTime.Now - pwt.DateChanged).Hours > 1)) && (((DateTime.Now - pwt.DateChanged).Hours % 8 == 0)))
                            {
                                Console.WriteLine(string.Format("Processing reminder Tag ID {0}", pwt.PageWatchTagID.ToString()));
                                
                                if (aeList.Exists(x => x.AdminUserID == pw.AdminUserID) == true)
                                {
                                    aeList.Single(x => x.AdminUserID == pw.AdminUserID).PageChanges.Add("Page Name: " + pw.Name + " Tag Name: " + pwt.Name + "<br />");
                                }
                                else
                                {
                                    AdminEmail aeNew = new AdminEmail();
                                    aeNew.AdminUserID = pw.AdminUserID;
                                    aeNew.UserName = pw.UserName;
                                    List<string> toAdd = new List<string>();
                                    toAdd.Add("Page Name: " + pw.Name + " Tag Name: " + pwt.Name + "<br />");
                                    aeNew.PageChanges = toAdd;
                                    aeList.Add(aeNew);
                                }                                
                            }
                        }
                    }
                    if (aeList.Count > 0)
                    {
                        SendAdminEmail(aeList, true);
                    }
                }
                else
                {
                    Console.WriteLine("No Page Watches to check");
                }

            }
            catch (Exception e)
            {
                SendEmailNotification("Page Watch engine encountered an exception",
                    string.Format("Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    e.Message, e.Source, e.StackTrace, e.InnerException));
            }
        }

        public static void SendAdminEmail(List<AdminEmail> aeList, bool isReminder)
        {
            foreach (AdminEmail ae in aeList)
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(ConfigurationManager.AppSettings["EngineSendFrom"]);
                message.To.Add(ae.UserName);
                message.IsBodyHtml = true;
                if (isReminder)
                {
                    message.Subject = ConfigurationManager.AppSettings["EngineSubject"];
                }
                else
                {
                    message.Subject = ConfigurationManager.AppSettings["EngineSubject"] + " - REMINDER";
                }
                string body = "<html><body><p>The Following Pages/Tags have changed: <br />";
                int i = 1;
                foreach (string tagChanged in ae.PageChanges)
                {
                    body += i.ToString() + ") " + tagChanged;
                    i++;
                }
                body += "<br />Please log in to <a href='http://www.ecn5.com'>ECN</a> to see the changes.</p></body></html>";
                message.Body = body;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
                smtp.Send(message);
            }
        }

        public static void SendEmailNotification(string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["AdminSendFrom"]);
            message.To.Add(ConfigurationManager.AppSettings["AdminSendTo"]);
            message.Subject = subject;
            message.Body = body;

            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
        }
    }
}
