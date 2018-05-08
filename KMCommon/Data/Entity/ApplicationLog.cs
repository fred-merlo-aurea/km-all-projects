using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text;

namespace KM.Common.Entity
{
    [Serializable]
    [DataContract]
    public class ApplicationLog
    {
        public ApplicationLog()
        {
            LogID = -1;
            ApplicationID = -1;
            SeverityID = -1;
            SourceMethod = string.Empty;
            Exception = string.Empty;
            LogNote = string.Empty;
            IsBug = null;
            IsUserSubmitted = null;
            GDCharityID = null;
            ECNCustomerID = null;
            SubmittedBy = string.Empty;
            SubmittedByEmail = string.Empty;
            IsFixed = null;
            FixedDate = null;
            FixedTime = null;
            FixedBy = string.Empty;
            FixedNote = string.Empty;
            LogAddedDate = null;
            LogAddedTime = null;
            LogUpdatedDate = null;
            LogUpdatedTime = null;
            NotificationSent = null;
        }
        #region Properties
        [DataMember]
        public int LogID { get; set; }
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public int SeverityID { get; set; }
        [DataMember]
        public string SourceMethod { get; set; }
        [DataMember]
        public string Exception { get; set; }
        [DataMember]
        public string LogNote { get; set; }
        [DataMember]
        public bool? IsBug { get; set; }
        [DataMember]
        public bool? IsUserSubmitted { get; set; }
        [DataMember]
        public int? GDCharityID { get; set; }
        [DataMember]
        public int? ECNCustomerID { get; set; }
        [DataMember]
        public string SubmittedBy { get; set; }
        [DataMember]
        public string SubmittedByEmail { get; set; }
        [DataMember]
        public bool? IsFixed { get; set; }
        [DataMember]
        public DateTime? FixedDate { get; set; }
        [DataMember]
        public TimeSpan? FixedTime { get; set; }
        [DataMember]
        public string FixedBy { get; set; }
        [DataMember]
        public string FixedNote { get; set; }
        [DataMember]
        public DateTime? LogAddedDate { get; set; }
        [DataMember]
        public TimeSpan? LogAddedTime { get; set; }
        [DataMember]
        public DateTime? LogUpdatedDate { get; set; }
        [DataMember]
        public TimeSpan? LogUpdatedTime { get; set; }
        [DataMember]
        public bool? NotificationSent { get; set; }
        #endregion

        //public static bool Validate(ApplicationLog appLog)
        //{
        //    if (appLog.ApplicationID <= 0)
        //        return false;
        //    else if (IsGDApp(appLog.ApplicationID) && !GDValidate(appLog))
        //        return false;
        //    if ((appLog.ApplicationID == 1 || appLog.ApplicationID == 2 || appLog.ApplicationID == 3) && !ECNValidate(appLog))
        //        return false;
        //    if (appLog.SeverityID <= 0)
        //        return false;
        //    if (appLog.Exception.Trim().Length == 0)
        //        return false;
        //    if (appLog.NotificationSent == null)
        //        return false;
        //    if (appLog.LogID > 0)
        //    {
        //        if (appLog.LogAddedDate == null)
        //            return false;
        //        if (appLog.LogAddedTime == null)
        //            return false;
        //    }
        //    return true;
        //}

        //private static bool IsGDApp(int applicationID)
        //{
        //    if (applicationID == 7 || applicationID == 8 || applicationID == 9 || applicationID == 10 || applicationID == 11 || applicationID == 12 || applicationID == 13 || applicationID == 14 ||
        //        applicationID == 15 || applicationID == 16 || applicationID == 17 || applicationID == 18 || applicationID == 19 || applicationID == 20 || applicationID == 21 || applicationID == 22 ||
        //        applicationID == 23 || applicationID == 24 || applicationID == 25)
        //        return true;
        //    else
        //        return false;
        //}
        //private static bool GDValidate(ApplicationLog appLog)
        //{
        //    if (appLog.SourceMethod.Trim().Length == 0)
        //        return false;
        //    if (appLog.LogNote.Trim().Length == 0)
        //        return false;
        //    if (appLog.IsBug == null)
        //        return false;
        //    if (appLog.IsUserSubmitted == null)
        //        return false;
        //    if (appLog.GDCharityID == null)
        //        return false;
        //    if (appLog.SubmittedBy.Trim().Length == 0)
        //        return false;
        //    if (appLog.SubmittedByEmail.Trim().Length == 0)
        //        return false;
        //    if (appLog.IsFixed == null)
        //        return false;
        //    else if (appLog.IsFixed.Value == true)
        //    {
        //        if (appLog.FixedDate == null)
        //            return false;
        //        if (appLog.FixedTime == null)
        //            return false;
        //        if (appLog.FixedBy.Trim().Length == 0)
        //            return false;
        //        if (appLog.FixedNote.Trim().Length == 0)
        //            return false;
        //    }
        //    return true;
        //}

        //private static bool ECNValidate(ApplicationLog appLog)
        //{
        //    //if (appLog.ECNCustomerID == null)
        //    //    return false;
        //    return true;
        //}

        public static bool Save(ref ApplicationLog appLog)
        {
            //if (!Validate(appLog))
            //    return false;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationLog_Save";
            cmd.Parameters.AddWithValue("@LogID", appLog.LogID);
            cmd.Parameters.AddWithValue("@ApplicationID", appLog.ApplicationID);
            cmd.Parameters.AddWithValue("@SeverityID", appLog.SeverityID);
            cmd.Parameters.AddWithValue("@SourceMethod", appLog.SourceMethod);
            cmd.Parameters.AddWithValue("@Exception", appLog.Exception);
            cmd.Parameters.AddWithValue("@LogNote", appLog.LogNote);
            cmd.Parameters.AddWithValue("@IsBug", (object)appLog.IsBug ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsUserSubmitted", (object)appLog.IsUserSubmitted ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GDCharityID", (object)appLog.GDCharityID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ECNCustomerID", (object)appLog.ECNCustomerID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubmittedBy", appLog.SubmittedBy);
            cmd.Parameters.AddWithValue("@SubmittedByEmail", appLog.SubmittedByEmail);
            cmd.Parameters.AddWithValue("@IsFixed", (object)appLog.IsFixed ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FixedDate", (object)appLog.FixedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FixedTime", (object)appLog.FixedTime ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FixedBy", appLog.FixedBy);
            cmd.Parameters.AddWithValue("@FixedNote", appLog.FixedNote);
            cmd.Parameters.AddWithValue("@NotificationSent", (object)appLog.NotificationSent ?? DBNull.Value);

            int id = 0;
            int.TryParse(DataFunctions.ExecuteScalar(cmd).ToString(), out id);
            if (id > 0)
            {
                appLog.LogID = id;
                if (appLog.SeverityID == 1)
                {
                    string errorMessage = EmailFunctions.NotifyAdmin(appLog);
                    if (errorMessage == string.Empty)
                    {
                        UpdateNotified(id);
                        return true;
                    }
                    else
                    {
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError("Error sending notification email: " + errorMessage, "KM.Common.Entity.ApplicationLog.Save", appLog.ApplicationID);
                        return false;
                    }
                }
            }
            return false;
        }

        public static void UpdateNotified(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update applicationlog set notificationsent = 1 where logid = @LogID";
            cmd.Parameters.AddWithValue("@LogID", id);
            DataFunctions.ExecuteNonQuery(cmd);
        }

        public static string FormatException(Exception ex)
        {
            StringBuilder sbEx = new StringBuilder();
            sbEx.AppendLine("<table><tr><td><b>**********************</b></td></tr>");

            if (ex.HelpLink != null)
            {
                sbEx.AppendLine("<tr><td><b>-- Help Link --</b></td></tr>");
                sbEx.AppendLine(ex.HelpLink);
            }

            if (ex.TargetSite != null)
            {
                sbEx.AppendLine("<tr><td><b>-- Target Site --</b></td></tr>");
                sbEx.AppendLine("<tr><td>Name: " + ex.TargetSite.Name + "</td></tr>");
                if (ex.TargetSite.Module != null)
                {
                    sbEx.AppendLine("<tr><td>Module FullyQualifiedName: " + ex.TargetSite.Module.FullyQualifiedName + "</td></tr>");
                    sbEx.AppendLine("<tr><td>Module Name: " + ex.TargetSite.Module.Name + "</td></tr>");
                }
                if (ex.TargetSite.Module.Assembly != null)
                    sbEx.AppendLine("<tr><td>Module Assembly FullName: " + ex.TargetSite.Module.Assembly.FullName + "</td></tr>");
            }

            if (ex.Source != null)
            {
                sbEx.AppendLine("<tr><td><b>-- Source --</b></td></tr>");
                sbEx.AppendLine("<tr><td>" + ex.Source + "</td></tr>");
            }

            if (ex.Data != null)
            {
                sbEx.AppendLine("<tr><td><b>-- Data --</b></td></tr>");
                foreach (var d in ex.Data)
                {
                    sbEx.AppendLine("<tr><td>" + d.ToString() + "</td></tr>");
                }
            }

            if (ex.Message != null)
            {
                sbEx.AppendLine("<tr><td><b>-- Message --</b></td></tr>");
                sbEx.AppendLine("<tr><td>" + ex.Message + "</td></tr>");
            }

            if (ex.InnerException != null)
            {
                sbEx.AppendLine("<tr><td><b>-- InnerException --</b></td></tr>");
                sbEx.AppendLine("<tr><td>" + ex.InnerException.ToString() + "</td></tr>");
            }

            if (ex.StackTrace != null)
            {
                sbEx.AppendLine("<tr><td><b>-- Stack Trace --</b></td></tr>");
                sbEx.AppendLine("<tr><td>" + ex.StackTrace + "</td></tr>");
            }
            sbEx.AppendLine("<tr><td><b>**********************</b></td></tr></table>");

            return sbEx.ToString();
        }

        public static int LogError(string errorMessage, string sourceMethod, int applicationId, Severity.SeverityLevel severity, string note = "", int charityId = -1, int customerId = -1)
        {
            var applicationLog = new ApplicationLog
            {
                ApplicationID = applicationId,
                SeverityID = (int)severity,
                Exception = errorMessage,
                NotificationSent = false,
                SourceMethod = sourceMethod,
                IsBug = true,
                IsUserSubmitted = false,
                LogNote = note,
                GDCharityID = charityId,
                ECNCustomerID = customerId
            };

            Save(ref applicationLog);

            return applicationLog.LogID;
        }

        public static void LogCriticalError(Exception exception, string sourceMethod, Application.Applications application, string note = "", int charityId = -1, int customerId = -1)
        {
            LogCriticalError(exception, sourceMethod, (int)application, note, charityId, customerId);
        }

        public static int LogCriticalError(Exception exception, string sourceMethod, int applicationId, string note = "", int charityId = -1, int customerId = -1)
        {
            return LogError(
                FormatException(exception),
                sourceMethod,
                applicationId,
                Severity.SeverityLevel.Critical,
                note,
                charityId,
                customerId);
        }

        public static void LogNonCriticalError(Exception exception, string sourceMethod, int applicationId, string note = "", int charityId = -1, int customerId = -1)
        {
            LogNonCriticalError(FormatException(exception), sourceMethod, applicationId, note, charityId, customerId);
        }

        public static int LogNonCriticalError(string error, string sourceMethod, int applicationId, string note = "", int charityId = -1, int customerId = -1)
        {
            return LogError(
                error,
                sourceMethod,
                applicationId,
                Severity.SeverityLevel.Non_Critical,
                note,
                charityId,
                customerId);
        }

        public static List<ApplicationLog> Select(int? applicationID)
        {
            List<ApplicationLog> retList = new List<ApplicationLog>();
            string sqlQuery = "e_ApplicationLog_Select";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            if (applicationID != null)
                cmd.Parameters.AddWithValue("@ApplicationID", applicationID.Value);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);

            var builder = DynamicBuilder<ApplicationLog>.CreateBuilder(rdr);
            while (rdr.Read())
            {
                ApplicationLog x = builder.Build(rdr);
                retList.Add(x);
            }

            cmd.Connection.Close();
            return retList;
        }

        public static List<ApplicationLog> SelectWithDateRange(int? applicationID, DateTime startDate, DateTime endDate)
        {
            List<ApplicationLog> retList = new List<ApplicationLog>();
            string sqlQuery = "e_ApplicationLog_Select_DateRange";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            if (applicationID != null)
                cmd.Parameters.AddWithValue("@ApplicationID", applicationID.Value);
            SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);

            var builder = DynamicBuilder<ApplicationLog>.CreateBuilder(rdr);
            while (rdr.Read())
            {
                ApplicationLog x = builder.Build(rdr);
                retList.Add(x);
            }

            cmd.Connection.Close();
            return retList;
        }
    }
}
