using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using KM.Common;
using KM.Common.Utilities.Email;

namespace KMPlatform.BusinessLogic
{
    public class ApplicationLog
    {
        public List<Entity.ApplicationLog> SelectApplication(int applicationID)
        {
            List<Entity.ApplicationLog> x = null;
            x = DataAccess.ApplicationLog.SelectApplication(applicationID).ToList();
            return x;
        }
        public List<Entity.ApplicationLog> SelectApplicationWithDateRange(int applicationId, DateTime startDate, DateTime endDate)
        {
            List<Entity.ApplicationLog> x = null;
            x = DataAccess.ApplicationLog.SelectApplicationWithDateRange(applicationId, startDate, endDate).ToList();
            return x;
        }
        public List<Entity.ApplicationLog> SelectWithDateRange(DateTime startDate, DateTime endDate)
        {
            List<Entity.ApplicationLog> x = null;
            x = DataAccess.ApplicationLog.SelectWithDateRange(startDate, endDate).ToList();
            return x;
        }
        public int Save(Entity.ApplicationLog x, Enums.Applications app, Enums.SeverityTypes severity, string subject = "")
        {
            if(string.IsNullOrEmpty(subject))
            {
                if (System.Configuration.ConfigurationManager.AppSettings["ErrorSubject"] != null)
                    subject = System.Configuration.ConfigurationManager.AppSettings["ErrorSubject"].ToString();
                else
                    subject = app.ToString() + " " + severity.ToString() + " Error";
            }
                
            if (x.LogAddedDate == null)
                x.LogAddedDate = DateTime.Now;
            if (x.LogAddedTime == null)
                x.LogAddedTime = DateTime.Now.TimeOfDay;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ApplicationLogId = DataAccess.ApplicationLog.Save(x, app, severity);
                scope.Complete();
            }

            if (x.ApplicationLogId > 0)
            {
                if (severity == Enums.SeverityTypes.Critical)
                {
                    try
                    {
                        BusinessLogic.Application appWorker = new Application();
                        Entity.Application sendApp = appWorker.Select().SingleOrDefault(a => a.ApplicationName.Equals(app.ToString().Replace("_"," ")));

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("ApplicationLogId: " + x.ApplicationLogId.ToString());
                        sb.AppendLine("Application: " + app.ToString());
                        sb.AppendLine("ClientId: " + x.ClientId.ToString());
                        sb.AppendLine("Source Method: " + x.SourceMethod.ToString());
                        sb.AppendLine(System.Environment.NewLine);
                        sb.AppendLine("Log Note: " + x.LogNote.ToString());
                        sb.AppendLine(System.Environment.NewLine);
                        sb.AppendLine("Exception: " + x.Exception.ToString());

                        var emailService = new EmailService(new EmailClient(), new ConfigurationProvider());
                        var emailMessage = new EmailMessage
                        {
                            From = sendApp?.FromEmailAddress,
                            Subject = subject,
                            Body = sb.ToString(),
                            IsHtml = false
                        };
                        emailMessage.To.Add(sendApp.ErrorEmailAddress);
                        emailService.SendEmail(emailMessage);

                        UpdateNotified(x.ApplicationLogId);
                    }
                    catch(Exception ex)
                    {
                        LogNonCriticalError(ex, "KMPlatform.BusinessLogic.ApplicationLog.Save", app, "Error sending notification email", x.ClientId, subject);
                    }
                }
            }

            return x.ApplicationLogId;
        }
        public bool UpdateNotified(int applicationLogId)
        {
            return DataAccess.ApplicationLog.UpdateNotified(applicationLogId);
        }
        public int LogError(Exception ex, string sourceMethod, BusinessLogic.Enums.Applications application, BusinessLogic.Enums.SeverityTypes severity, string note = "", int clientId = -1, string subject = "")
        {
            Entity.ApplicationLog log = new Entity.ApplicationLog();
            log.Exception = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            log.SourceMethod = sourceMethod;
            log.IsBug = true;
            log.LogNote = note;
            log.ClientId = clientId;
            return Save(log,application,severity, subject);
        }
        public int LogCriticalError(string ex, string sourceMethod, BusinessLogic.Enums.Applications application, string note = "", int clientId = -1, string subject = "")
        {
            Entity.ApplicationLog log = new Entity.ApplicationLog();
            //log.Exception = Core.Utilities.StringFunctions.FormatException(ex);
            log.Exception = ex;
            log.SourceMethod = sourceMethod;
            log.IsBug = true;
            log.LogNote = note;
            log.ClientId = clientId;
            return Save(log, application, BusinessLogic.Enums.SeverityTypes.Critical, subject);
        }
        public int LogNonCriticalError(Exception ex, string sourceMethod, BusinessLogic.Enums.Applications application, string note = "", int clientId = -1, string subject = "")
        {
            Entity.ApplicationLog log = new Entity.ApplicationLog();
            log.Exception = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            log.SourceMethod = sourceMethod;
            log.IsBug = true;
             log.LogNote = note;
            log.ClientId = clientId;
            return Save(log, application, BusinessLogic.Enums.SeverityTypes.Non_Critical, subject);
        }
        public int LogNonCriticalErrorNote(string note, string sourceMethod, BusinessLogic.Enums.Applications application, int clientId = -1, string subject = "")
        {
            Entity.ApplicationLog log = new Entity.ApplicationLog();
            log.Exception = string.Empty;
            log.SourceMethod = sourceMethod;
            log.IsBug = true;
            log.LogNote = note;
            log.ClientId = clientId;
            return Save(log, application, BusinessLogic.Enums.SeverityTypes.Non_Critical, subject);
        }
        public int SaveNote(string note, string sourceMethod, BusinessLogic.Enums.Applications application, int clientId = -1, string subject = "")
        {
            Entity.ApplicationLog log = new Entity.ApplicationLog();
            log.Exception = string.Empty;
            log.SourceMethod = sourceMethod;
            log.IsBug = false;
            log.LogNote = note;
            log.ClientId = clientId;
            return Save(log, application, BusinessLogic.Enums.SeverityTypes.Non_Critical, subject);
        }
    }
}
