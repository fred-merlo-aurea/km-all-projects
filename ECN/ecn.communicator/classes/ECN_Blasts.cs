using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;

namespace ecn.communicator.classes
{
    public class ECN_Blasts
    {
        public ECN_Blasts()
        {
            BlastID = -1;
            CustomerID = -1;
            EmailSubject = string.Empty;
            EmailFrom = string.Empty;
            EmailFromName = string.Empty;
            SendTime = DateTime.MinValue;
            AttemptTotal = -1;
            SendTotal = -1;
            SendBytes = -1;
            StatusCode = string.Empty;
            BlastType = string.Empty;
            CodeID = -1;
            LayoutID = -1;
            GroupID = -1;
            FinishTime = DateTime.MinValue;
            SuccessTotal = -1;
            BlastLog = string.Empty;
            UserID = -1;
            FilterID = -1;
            Spinlock = string.Empty;
            ReplyTo = string.Empty;
            TestBlast = string.Empty;
            BlastFrequency = string.Empty;
            RefBlastID = string.Empty;
            BlastSuppression = string.Empty;
            AddOptOuts_to_MS = false;
            DynamicFromName = string.Empty;
            DynamicFromEmail = string.Empty;
            DynamicReplyToEmail = string.Empty;
        }

        #region Properties
        public int BlastID { get; set; }
        public int CustomerID { get; set; }
        public string EmailSubject { get; set; }
        public string EmailFrom { get; set; }
        public string EmailFromName { get; set; }
        public DateTime  SendTime { get; set; }
        public int AttemptTotal { get; set; }
        public int SendTotal { get; set; }
        public int SendBytes { get; set; }
        public string StatusCode { get; set; }
        public string BlastType { get; set; }
        public int CodeID { get; set; }
        public int LayoutID { get; set; }
        public int GroupID { get; set; }
        public DateTime  FinishTime { get; set; }
        public int SuccessTotal { get; set; }
        public string BlastLog { get; set; }
        public int UserID { get; set; }
        public int FilterID { get; set; }
        public string Spinlock { get; set; }
        public string ReplyTo { get; set; }
        public string TestBlast { get; set; }
        public string BlastFrequency { get; set; }
        public string RefBlastID { get; set; }
        public string BlastSuppression { get; set; }
        public bool AddOptOuts_to_MS { get; set; }
        public string DynamicFromName { get; set; }
        public string DynamicFromEmail { get; set; }
        public string DynamicReplyToEmail { get; set; }
        #endregion

        #region DataAccess
        public static List<ECN_Blasts> GetBlastByCustomerID(int customerId)
        {
            var ecnBlasts = new List<ECN_Blasts>();
            var sqlQuery = $" SELECT *  FROM Blasts  WHERE CustomerID={customerId} ";
            var connection = GetSqlConn();
            var command = new SqlCommand(sqlQuery, connection);

            connection.Open();
            var reader = command.ExecuteReader();
            
            var dummy = new ECN_Blasts
            {
                EmailSubject = "-- Select ECN Blast --",
                BlastID = -1
            };
            ecnBlasts.Add(dummy);

            while (reader.Read())
            {
                var item = new ECN_Blasts();
                item.BlastID = GetInt32(reader, "BlastID", item.BlastID);
                item.CustomerID = GetInt32(reader, "CustomerID", item.CustomerID);
                item.EmailSubject = GetString(reader, "EmailSubject", item.EmailSubject);
                item.EmailFrom = GetString(reader, "EmailFrom", item.EmailFrom);
                item.EmailFromName = GetString(reader, "EmailFromName", item.EmailFromName);
                item.SendTime = GetDateTime(reader, "SendTime", item.SendTime);
                item.AttemptTotal = GetInt32(reader, "AttemptTotal", item.AttemptTotal);
                item.SendTotal = GetInt32(reader, "SendTotal", item.SendTotal);
                item.SendBytes = GetInt32(reader, "SendBytes", item.SendBytes);
                item.StatusCode = GetString(reader, "StatusCode", item.StatusCode);
                item.BlastType = GetString(reader, "BlastType", item.BlastType);
                item.CodeID = GetInt32(reader, "CodeID", item.CodeID);
                item.LayoutID = GetInt32(reader, "LayoutID", item.LayoutID);
                item.GroupID = GetInt32(reader, "GroupID", item.GroupID);
                item.FinishTime = GetDateTime(reader, "FinishTime", item.FinishTime);
                item.SuccessTotal = GetInt32(reader, "SuccessTotal", item.SuccessTotal);
                item.BlastLog = GetString(reader, "BlastLog", item.BlastLog);
                item.UserID = GetInt32(reader, "UserID", item.UserID);
                item.FilterID = GetInt32(reader, "FilterID", item.FilterID);
                item.Spinlock = GetString(reader, "Spinlock", item.Spinlock);
                item.ReplyTo = GetString(reader, "ReplyTo", item.ReplyTo);
                item.TestBlast = GetString(reader, "TestBlast", item.TestBlast);
                item.BlastFrequency = GetString(reader, "BlastFrequency", item.BlastFrequency);
                item.RefBlastID = GetString(reader, "RefBlastID", item.RefBlastID);
                item.BlastSuppression = GetString(reader, "BlastSuppression", item.BlastSuppression);
                item.AddOptOuts_to_MS = GetBoolean(reader, "AddOptOuts_to_MS", item.AddOptOuts_to_MS);
                item.DynamicFromName = GetString(reader, "DynamicFromName", item.DynamicFromName);
                item.DynamicFromEmail = GetString(reader, "DynamicFromEmail", item.DynamicFromEmail);
                item.DynamicReplyToEmail = GetString(reader, "DynamicReplyToEmail", item.DynamicReplyToEmail);

                ecnBlasts.Add(item);
            }
            reader.Close();
            return ecnBlasts;
        }

        private static int GetInt32(SqlDataReader rdr, string name, int defaultValue)
        {
            var index = rdr.GetOrdinal(name);
            if (!rdr.IsDBNull(index))
            {
                return rdr.GetInt32(index);
            }

            return defaultValue;
        }

        private static string GetString(SqlDataReader rdr, string name, string defaultValue)
        {
            var index = rdr.GetOrdinal(name);
            if (!rdr.IsDBNull(index))
            {
                return rdr.GetString(index);
            }

            return defaultValue;
        }

        private static DateTime GetDateTime(SqlDataReader rdr, string name, DateTime defaultValue)
        {
            var index = rdr.GetOrdinal(name);
            if (!rdr.IsDBNull(index))
            {
                return rdr.GetDateTime(index);
            }

            return defaultValue;
        }

        private static bool GetBoolean(SqlDataReader rdr, string name, bool defaultValue)
        {
            var index = rdr.GetOrdinal(name);
            if (!rdr.IsDBNull(index))
            {
                return rdr.GetBoolean(index);
            }

            return defaultValue;
        }

        static SqlConnection GetSqlConn()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connString"].ToString());
            return conn;
        }
        #endregion
    }
}
