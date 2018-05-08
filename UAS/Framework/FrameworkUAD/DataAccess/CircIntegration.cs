using System;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class CircIntegration
    {
        public static bool ApplyTelemarketingRules(string processCode, FrameworkUAD_Lookup.Enums.FileTypes dft, KMPlatform.Object.ClientConnections client, int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_TelemarketingRules_ProcessCode";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@FileType", dft.ToString());
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool CircFileTypeUpdateIGrpBySequence(string processCode, FrameworkUAD_Lookup.Enums.FileTypes dft, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_Telemarketing_MatchSequenceUpdateIGrpNo_ProcessCode";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@FileType", dft.ToString());
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool ApplyWebFormRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_WebFormRules_ProcessCode";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool ApplyListSource2YRRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ListSource_2yr";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool ApplyListSource3YRRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ListSource_3yr";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool ApplyListSourceOtherRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ListSource_Other";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool ApplyFieldUpdateRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId, bool qdateOverRide, bool mailPermissionOverRide, bool faxPermissionOverRide, bool phonePermissionOverRide, bool otherProductsPermissionOverRide, bool thirdPartyPermissionOverRide, bool emailRenewPermissionOverRide, bool textPermissionOverRide)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_FieldUpdate";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);            
            cmd.Parameters.AddWithValue("@OverrideQDate", qdateOverRide);
            cmd.Parameters.AddWithValue("@MailPermissionOverRide", mailPermissionOverRide);
            cmd.Parameters.AddWithValue("@FaxPermissionOverRide", faxPermissionOverRide);
            cmd.Parameters.AddWithValue("@PhonePermissionOverRide", phonePermissionOverRide);
            cmd.Parameters.AddWithValue("@OtherProductsPermissionOverRide", otherProductsPermissionOverRide);
            cmd.Parameters.AddWithValue("@ThirdPartyPermissionOverRide", thirdPartyPermissionOverRide);
            cmd.Parameters.AddWithValue("@EmailRenewPermissionOverRide", emailRenewPermissionOverRide);
            cmd.Parameters.AddWithValue("@TextPermissionOverRide", textPermissionOverRide);
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool ApplyQuickFillRules(string processCode, KMPlatform.Object.ClientConnections client, int clientId, bool qdateOverRide, bool mailPermissionOverRide, bool faxPermissionOverRide, bool phonePermissionOverRide, bool otherProductsPermissionOverRide, bool thirdPartyPermissionOverRide, bool emailRenewPermissionOverRide, bool textPermissionOverRide)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_QuickFill";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@OverrideQDate", qdateOverRide);
            cmd.Parameters.AddWithValue("@MailPermissionOverRide", mailPermissionOverRide);
            cmd.Parameters.AddWithValue("@FaxPermissionOverRide", faxPermissionOverRide);
            cmd.Parameters.AddWithValue("@PhonePermissionOverRide", phonePermissionOverRide);
            cmd.Parameters.AddWithValue("@OtherProductsPermissionOverRide", otherProductsPermissionOverRide);
            cmd.Parameters.AddWithValue("@ThirdPartyPermissionOverRide", thirdPartyPermissionOverRide);
            cmd.Parameters.AddWithValue("@EmailRenewPermissionOverRide", emailRenewPermissionOverRide);
            cmd.Parameters.AddWithValue("@TextPermissionOverRide", textPermissionOverRide);
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool ApplyPaidTransactionLogic(string processCode, KMPlatform.Object.ClientConnections client, int clientId, bool qdateOverRide, bool mailPermissionOverRide, bool faxPermissionOverRide, bool phonePermissionOverRide, bool otherProductsPermissionOverRide, bool thirdPartyPermissionOverRide, bool emailRenewPermissionOverRide, bool textPermissionOverRide)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_PaidTransaction";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@OverrideQDate", qdateOverRide);
            cmd.Parameters.AddWithValue("@MailPermissionOverRide", mailPermissionOverRide);
            cmd.Parameters.AddWithValue("@FaxPermissionOverRide", faxPermissionOverRide);
            cmd.Parameters.AddWithValue("@PhonePermissionOverRide", phonePermissionOverRide);
            cmd.Parameters.AddWithValue("@OtherProductsPermissionOverRide", otherProductsPermissionOverRide);
            cmd.Parameters.AddWithValue("@ThirdPartyPermissionOverRide", thirdPartyPermissionOverRide);
            cmd.Parameters.AddWithValue("@EmailRenewPermissionOverRide", emailRenewPermissionOverRide);
            cmd.Parameters.AddWithValue("@TextPermissionOverRide", textPermissionOverRide);
            cmd.CommandTimeout = 0;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool FieldUpdateDataMatching(KMPlatform.Object.ClientConnections client, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_FieldUpdateDataMatching";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);            
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandTimeout = 0;

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool QuickFillDataMatching(KMPlatform.Object.ClientConnections client, string processCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_AccountNumberDataMatching";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandTimeout = 0;

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static DataTable SelectCircImportSummaryOne(string ProcessCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_CIRC_Import_Results_BY_YearSummary";
            cmd.Parameters.Add(new SqlParameter("@PROCESSCODE", ProcessCode));            
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandTimeout = 0;

            return KM.Common.DataFunctions.GetDataTable(cmd);
        }

        public static DataTable SelectCircImportSummaryTwo(string ProcessCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_CIRC_Import_Results_By_CATTRANS_Summary";
            cmd.Parameters.Add(new SqlParameter("@PROCESSCODE", ProcessCode));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandTimeout = 0;

            return KM.Common.DataFunctions.GetDataTable(cmd);
        }

        public static DataTable SelectCircACSSummary(string ProcessCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_CIRC_ACS_Results";
            cmd.Parameters.Add(new SqlParameter("@ProcessCode", ProcessCode));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandTimeout = 0;

            return KM.Common.DataFunctions.GetDataTable(cmd);
        }
    }
}
