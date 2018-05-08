using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;
using ECN_Framework_DataLayer.Communicator.Helpers;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class GroupDataFields
    {
        private const string ProcedureGroupDataFieldsDeleteSingle = "e_GroupDataFields_Delete_Single";
        private const string ProcedureGroupDataFieldsDeleteAll = "e_GroupDataFields_Delete_All";

        public static bool Exists(string shortName, int? groupDataFieldsID, int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_Exists_ByShortName";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@ShortName", shortName);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", (object)groupDataFieldsID ?? DBNull.Value);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int groupDataFieldsID, int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_Exists_ByID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", groupDataFieldsID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_Exists_ByGroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        private static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetList(SqlCommand cmd)
        {
            return SqlCommandCommunicator<ECN_Framework_Entities.Communicator.GroupDataFields>.GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.GroupDataFields Get(SqlCommand cmd)
        {
            return SqlCommandCommunicator<ECN_Framework_Entities.Communicator.GroupDataFields>.Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.GroupDataFields GetByID(int groupDataFieldsID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_Select_GroupDataFieldsID";
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", groupDataFieldsID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetByGroupID(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_Select_GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetByDataFieldSetID(int groupID, int datafieldSetID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_Select_DataFieldSetID";
            cmd.Parameters.AddWithValue("@DatafieldSetID", datafieldSetID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return GetList(cmd);
        }

        public static void Delete(int groupID, int groupDataFieldsID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryGroupDataFieldsSingle(groupID, groupDataFieldsID, customerID, userID,
                            ProcedureGroupDataFieldsDeleteSingle);
        }

        public static void Delete(int groupID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryGroupDataFields(groupID, customerID, userID,
                            ProcedureGroupDataFieldsDeleteAll);
        }

        public static int Save(ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDatafields_Save";
            cmd.Parameters.Add(new SqlParameter("@GroupDataFieldsID", (object)groupDataFields.GroupDataFieldsID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", groupDataFields.GroupID));
            cmd.Parameters.Add(new SqlParameter("@ShortName", groupDataFields.ShortName));
            cmd.Parameters.Add(new SqlParameter("@LongName", groupDataFields.LongName));
            cmd.Parameters.Add(new SqlParameter("@SurveyID", (object)groupDataFields.SurveyID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DatafieldSetID", (object)groupDataFields.DatafieldSetID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsPublic", groupDataFields.IsPublic));
            cmd.Parameters.Add(new SqlParameter("@IsPrimaryKey", groupDataFields.IsPrimaryKey));
            if (groupDataFields.GroupDataFieldsID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)groupDataFields.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)groupDataFields.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.GroupDataFields> GetByCustomerID(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.GroupDataFields GetByShortName(string ShortName, int GroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * from GroupDatafields with (NOLOCK) where ShortName=@ShortName and GroupID=@GroupID and IsDeleted = 0";
            cmd.Parameters.AddWithValue("@ShortName", ShortName);
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            return Get(cmd);
        }

        public static bool UsedInFilter(int groupDataFieldsID, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_UsedInFilter";
            cmd.Parameters.AddWithValue("@GDFID", groupDataFieldsID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ActiveByGDF(string shortName, int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFields_ActiveByGDF";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@ShortName", shortName);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
    }
}
