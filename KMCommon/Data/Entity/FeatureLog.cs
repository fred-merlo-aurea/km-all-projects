using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace KM.Common.Entity
{
    [Serializable]
    [DataContract]
    public class FeatureLog
    {
        private const string FeatureLogApplicationIdKey = "@ApplicationID";
        private const string FeatureLogProductLineKey = "@ProductLine";
        private const string FeatureLogTargetAppKey = "@TargetApp";
        private const string FeatureLogEnteredByKey = "@EnteredBy";
        private const string FeatureLogEnteredDateKey = "@EnteredDate";
        private const string FeatureLogRequestByKey = "@RequestBy";
        private const string FeatureLogRequestDateKey = "@RequestDate";
        private const string FeatureLogFeatureNameKey = "@FeatureName";
        private const string FeatureLogFetureDescriptionKey = "@FetureDescription";
        private const string FeatureLogTargetReleaseDateKey = "@TargetReleaseDate";
        private const string FeatureLogIsQuotedKey = "@IsQuoted";
        private const string FeatureLogQuotedHoursKey = "@QuotedHours";
        private const string FeatureLogIsApporovedKey = "@IsApporoved";
        private const string FeatureLogApprovedByKey = "@ApprovedBy";
        private const string FeatureLogApprovedDateKey = "@ApprovedDate";
        private const string FeatureLogIsStartedKey = "@IsStarted";
        private const string FeatureLogStartedDateKey = "@StartedDate";
        private const string FeatureLogIsCompletedKey = "@IsCompleted";
        private const string FeatureLogCompletedDateKey = "@CompletedDate";
        private const string FeatureLogDevLeadKey = "@DevLead";
        private const string FeatureLogDevNotesKey = "@DevNotes";
        private const string FeatureLogFeaturePriorityKey = "@FeaturePriority";
        private const string FeatureLogDateAddedKey = "@DateAdded";
        private const string FeatureLogDateUpdatedKey = "@DateUpdated";
        private const string FeatureLogUpdatedByKey = "@UpdatedBy";
        private const string FeatureLogFLogIdKey = "@FLogID";
        private const string InsertCommandText = "e_FeatureLog_Insert";
        private const string UpdateCommandText = "e_FeatureLog_Update";

        public FeatureLog() { }
        #region Properties
        [DataMember]
        public int FLogID { get; set; }
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public string ProductLine { get; set; }
        [DataMember]
        public string TargetApp { get; set; }
        [DataMember]
        public string EnteredBy { get; set; }
        [DataMember]
        public DateTime EnteredDate { get; set; }
        [DataMember]
        public string RequestBy { get; set; }
        [DataMember]
        public DateTime RequestDate { get; set; }
        [DataMember]
        public string FeatureName { get; set; }
        [DataMember]
        public string FetureDescription { get; set; }
        [DataMember]
        public DateTime TargetReleaseDate { get; set; }
        [DataMember]
        public bool IsQuoted { get; set; }
        [DataMember]
        public double QuotedHours { get; set; }
        [DataMember]
        public bool IsApporoved { get; set; }
        [DataMember]
        public string ApprovedBy { get; set; }
        [DataMember]
        public DateTime ApprovedDate { get; set; }
        [DataMember]
        public bool IsStarted { get; set; }
        [DataMember]
        public DateTime StartedDate { get; set; }
        [DataMember]
        public bool IsCompleted { get; set; }
        [DataMember]
        public DateTime CompletedDate { get; set; }
        [DataMember]
        public string DevLead { get; set; }
        [DataMember]
        public string DevNotes { get; set; }
        [DataMember]
        public int FeaturePriority { get; set; }
        [DataMember]
        public DateTime DateAdded { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        #endregion
        #region Data
        private static List<FeatureLog> GetAll()
        {
            //ORDER BY IsCompleted,FeaturePriority
            List<FeatureLog> retList = new List<FeatureLog>();
            string sqlQuery = "e_FeatureLog_SelectAll";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);

            var builder = DynamicBuilder<FeatureLog>.CreateBuilder(rdr);
            while (rdr.Read())
            {
                FeatureLog x = builder.Build(rdr);
                retList.Add(x);
            }

            rdr.Close();
            rdr.Dispose();
            cmd.Connection.Close();

            return retList;
        }

        public static int Insert(FeatureLog featureLog)
        {
            int insertId;
            using (var sqlCommand = CreateCommand(InsertCommandText, featureLog))
            {
                sqlCommand.Parameters.AddWithValue(FeatureLogEnteredByKey, featureLog.EnteredBy);
                sqlCommand.Parameters.AddWithValue(FeatureLogEnteredDateKey, featureLog.EnteredDate);
                sqlCommand.Parameters.AddWithValue(FeatureLogDateAddedKey, featureLog.DateAdded);

                int.TryParse(DataFunctions.ExecuteScalar(sqlCommand).ToString(), out insertId);
            }

            return insertId;
        }

        public static bool Update(FeatureLog featureLog)
        {
            using (var sqlCommand = CreateCommand(UpdateCommandText, featureLog))
            {
                sqlCommand.Parameters.AddWithValue(FeatureLogFLogIdKey, featureLog.FLogID);
                sqlCommand.Parameters.AddWithValue(FeatureLogDateUpdatedKey, featureLog.DateUpdated);
                sqlCommand.Parameters.AddWithValue(FeatureLogUpdatedByKey, featureLog.UpdatedBy);

                return DataFunctions.ExecuteNonQuery(sqlCommand);
            }
        }

        private static SqlCommand CreateCommand(string commandText, FeatureLog featureLog)
        {
            var sqlCommand = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = commandText
            };

            AddCommonSqlParameter(sqlCommand.Parameters, featureLog);

            return sqlCommand;
        }

        private static void AddCommonSqlParameter(SqlParameterCollection parameterCollection, FeatureLog featureLog)
        {
            parameterCollection.AddWithValue(FeatureLogApplicationIdKey, featureLog.ApplicationID);
            parameterCollection.AddWithValue(FeatureLogProductLineKey, featureLog.ProductLine);
            parameterCollection.AddWithValue(FeatureLogTargetAppKey, featureLog.TargetApp);
            parameterCollection.AddWithValue(FeatureLogRequestByKey, featureLog.RequestBy);
            parameterCollection.AddWithValue(FeatureLogRequestDateKey, featureLog.RequestDate);
            parameterCollection.AddWithValue(FeatureLogFeatureNameKey, featureLog.FeatureName);
            parameterCollection.AddWithValue(FeatureLogFetureDescriptionKey, featureLog.FetureDescription);
            parameterCollection.AddWithValue(FeatureLogTargetReleaseDateKey, featureLog.TargetReleaseDate);
            parameterCollection.AddWithValue(FeatureLogIsQuotedKey, featureLog.IsQuoted);
            parameterCollection.AddWithValue(FeatureLogQuotedHoursKey, featureLog.QuotedHours);
            parameterCollection.AddWithValue(FeatureLogIsApporovedKey, featureLog.IsApporoved);
            parameterCollection.AddWithValue(FeatureLogApprovedByKey, featureLog.ApprovedBy);
            parameterCollection.AddWithValue(FeatureLogApprovedDateKey, featureLog.ApprovedDate);
            parameterCollection.AddWithValue(FeatureLogIsStartedKey, featureLog.IsStarted);
            parameterCollection.AddWithValue(FeatureLogStartedDateKey, featureLog.StartedDate);
            parameterCollection.AddWithValue(FeatureLogIsCompletedKey, featureLog.IsCompleted);
            parameterCollection.AddWithValue(FeatureLogCompletedDateKey, featureLog.CompletedDate);
            parameterCollection.AddWithValue(FeatureLogDevLeadKey, featureLog.DevLead);
            parameterCollection.AddWithValue(FeatureLogDevNotesKey, featureLog.DevNotes);
            parameterCollection.AddWithValue(FeatureLogFeaturePriorityKey, featureLog.FeaturePriority);
        }
        #endregion
    }
}
