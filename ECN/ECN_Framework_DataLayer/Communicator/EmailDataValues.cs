using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class EmailDataValues
    {
        private const string ProcedureEmailDataValuesDeleteSingle = "e_EmailDataValues_Delete_Single";

        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupDataFieldsID(int groupDataFieldsID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDataValues_Select_GroupDataFieldsID_EmailID";
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", groupDataFieldsID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupDataFieldsID(int groupDataFieldsID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDataValues_Select_GroupDataFieldsID";
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", groupDataFieldsID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupID(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDataValues_Select_GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.EmailDataValues> retList = new List<ECN_Framework_Entities.Communicator.EmailDataValues>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.EmailDataValues retItem = new ECN_Framework_Entities.Communicator.EmailDataValues();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailDataValues>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static void Delete(int groupID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryEmailDataValues(
                groupID, null, customerID, userID, ProcedureEmailDataValuesDeleteSingle);
        }

        public static void Delete(int groupID, int groupDataFieldsID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryEmailDataValues(
                groupID, groupDataFieldsID, customerID, userID, ProcedureEmailDataValuesDeleteSingle);
        }

        public static DataTable GetTransUDFDataValues(int customerID, int groupID, string emailID, int datafieldSetID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDataValues_GetTransUDFDataValues";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@UDFEmailID", emailID);
            cmd.Parameters.AddWithValue("@DatafieldSetID", datafieldSetID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetStandaloneUDFDataValues(int groupID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDataValues_GetStandaloneUDFDataValues";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int RecordTopicsValue(int blastID, int emailID, string values)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDataValues_RecordTopicsValue";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            cmd.Parameters.AddWithValue("@Values", values);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static bool HasResponses(int SurveyID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 EmailDataValuesID from Emaildatavalues edv join Groupdatafields gdf on edv.groupdatafieldsID =  gdf.groupdatafieldsID  where surveyID = @SurveyID)  select 1 else select 0";
            cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static void UpdateGridStatementID(int ParticipantID, int GroupDataFieldsID, int GridStatementID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update Emaildatavalues set SurveyGridID=@GridStatementID  where emailID = @ParticipantID and GroupDataFieldsID=@GroupDataFieldsID";
            cmd.Parameters.AddWithValue("@ParticipantID", ParticipantID);
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", GroupDataFieldsID);
            cmd.Parameters.AddWithValue("@GridStatementID", GridStatementID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());     
        }

        public static void DeleteByEmailID(int GroupID, int EmailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Emaildatavalues where emailID = @EmailID and GroupDatafieldsID IN (select GroupDatafieldsID from GroupDataFields gdf where gdf.GroupID=@GroupID)";
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());   
        }

        public static void DeleteByEmailID(int GroupID, int EmailID, int GroupDataFieldsID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Emaildatavalues where emailID = @EmailID and GroupDatafieldsID=@GroupDataFieldsID";
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", GroupID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());   
        }

        public static void SaveCheckBox(int emailID, int groupDataFieldsID, string value, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Response_SaveCheckbox";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", groupDataFieldsID);
            cmd.Parameters.AddWithValue("@Answer", value);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static string WATT_GetNextTokenForSubscriber(string Token, int IssueID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_WATT_GetNextTokenForSubscriber";
            cmd.Parameters.AddWithValue("@Token", Token);
            cmd.Parameters.AddWithValue("@IssueID", IssueID);
            try
            {
                string token = DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString();
                return token;
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        public static bool WATT_SubscriberExists(string Token, int IssueID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_WATT_SubscriberExists";
            cmd.Parameters.AddWithValue("@Token", Token);
            cmd.Parameters.AddWithValue("@IssueID", IssueID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static string WATT_GetTokenForSubscriber(string emailAddress, string shortName, int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Watt_GetTokenForSubscriber";
            cmd.Parameters.AddWithValue("@ShortName", shortName);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            return DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString();
        }
    }
}
