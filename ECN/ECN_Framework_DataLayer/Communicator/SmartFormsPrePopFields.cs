using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class SmartFormsPrePopFields
    {
        public static void Delete(int sfid, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsPrePopFields_Delete_SFID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SFID", sfid);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int sfid, int prePopFieldID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsPrePopFields_Delete_PrePopFieldID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SFID", sfid);
            cmd.Parameters.AddWithValue("@PrePopFieldID", prePopFieldID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static ECN_Framework_Entities.Communicator.SmartFormsPrePopFields GetByPrePopFieldID(int prePopFieldID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsPrePopFields_Select_PrePopFieldID";
            cmd.Parameters.AddWithValue("@PrePopFieldID", prePopFieldID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields> GetBySFID(int sfid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsPrePopFields_Select_SFID";
            cmd.Parameters.AddWithValue("@SFID", sfid);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields> retList = new List<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.SmartFormsPrePopFields retItem = new ECN_Framework_Entities.Communicator.SmartFormsPrePopFields();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.SmartFormsPrePopFields Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.SmartFormsPrePopFields retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.SmartFormsPrePopFields();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        public static int Save(ECN_Framework_Entities.Communicator.SmartFormsPrePopFields prePopFields)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsPrePopFields_Save";
            cmd.Parameters.Add(new SqlParameter("@PrePopFieldID", prePopFields.PrePopFieldID));
            cmd.Parameters.Add(new SqlParameter("@SFID", (object)prePopFields.SFID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ProfileFieldName", prePopFields.ProfileFieldName));
            cmd.Parameters.Add(new SqlParameter("@DisplayName", prePopFields.DisplayName));
            cmd.Parameters.Add(new SqlParameter("@DataType", prePopFields.DataType));
            cmd.Parameters.Add(new SqlParameter("@ControlType", prePopFields.ControlType));
            cmd.Parameters.Add(new SqlParameter("@DataValues", prePopFields.DataValues));
            cmd.Parameters.Add(new SqlParameter("@Required", prePopFields.Required));
            cmd.Parameters.Add(new SqlParameter("@PrePopulate", prePopFields.PrePopulate));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", (object)prePopFields.SortOrder ?? DBNull.Value));
            if (prePopFields.PrePopFieldID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)prePopFields.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)prePopFields.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static bool ExistsDisplayName(int sfid, int prePopFieldID, string displayName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsPrePopFields_Exists_ByDisplayName";
            cmd.Parameters.AddWithValue("@SFID", sfid);
            cmd.Parameters.AddWithValue("@PrePopFieldID", prePopFieldID);
            cmd.Parameters.AddWithValue("@DisplayName", displayName);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ExistsProfileFieldName(int sfid, int prePopFieldID, string profileFieldName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsPrePopFields_Exists_ByProfileFieldName";
            cmd.Parameters.AddWithValue("@SFID", sfid);
            cmd.Parameters.AddWithValue("@PrePopFieldID", prePopFieldID);
            cmd.Parameters.AddWithValue("@ProfileFieldName", profileFieldName);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static DataTable GetColumnNames(int sfid, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsPrePopFields_GetColumnNames";
            cmd.Parameters.AddWithValue("@SFID", sfid);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
