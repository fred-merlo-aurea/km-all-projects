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
    public class FilterCondition
    {
        private const string ProcedureFilterConditionDeleteFilterGroupId = "e_FilterCondition_Delete_FilterGroupID";
        private const string ProcedureFilterConditionDeleteFilterConditionId = "e_FilterCondition_Delete_FilterConditionID";

        public static bool Exists(int filterConditionID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterCondition_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FilterConditionID", filterConditionID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        private static ECN_Framework_Entities.Communicator.FilterCondition Get(SqlCommand cmd)
        {
            return SqlCommandCommunicator<ECN_Framework_Entities.Communicator.FilterCondition>.Get(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.FilterCondition> GetList(SqlCommand cmd)
        {
            return SqlCommandCommunicator<ECN_Framework_Entities.Communicator.FilterCondition>.GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.FilterCondition GetByFilterConditionID(int filterConditionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterCondition_Select_FilterConditionID";
            cmd.Parameters.AddWithValue("@FilterConditionID", filterConditionID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.FilterCondition> GetByFilterGroupID(int filterGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterCondition_Select_FilterGroupID";
            cmd.Parameters.AddWithValue("@FilterGroupID", filterGroupID);
            return GetList(cmd);
        }

        public static void Delete(int filterGroupID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryFilterCondition(filterGroupID, customerID, userID,
                        ProcedureFilterConditionDeleteFilterGroupId);
        }

        public static void Delete(int filterGroupID, int filterConditionID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryFilterCondition(filterGroupID, filterConditionID, customerID, userID,
                        ProcedureFilterConditionDeleteFilterConditionId);
        }

        public static int Save(ECN_Framework_Entities.Communicator.FilterCondition filterCondition)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterCondition_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterConditionID", (object)filterCondition.FilterConditionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", (object)filterCondition.FilterGroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", (object)filterCondition.SortOrder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Field", filterCondition.Field));
            cmd.Parameters.Add(new SqlParameter("@FieldType", filterCondition.FieldType));
            cmd.Parameters.Add(new SqlParameter("@Comparator", filterCondition.Comparator));
            cmd.Parameters.Add(new SqlParameter("@CompareValue", filterCondition.CompareValue));
            cmd.Parameters.Add(new SqlParameter("@DatePart", filterCondition.DatePart));
            cmd.Parameters.Add(new SqlParameter("@NotComparator", (object)filterCondition.NotComparator ?? DBNull.Value));
            if (filterCondition.FilterConditionID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)filterCondition.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)filterCondition.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static int GetSortOrder(int filterGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterCondition_GetSortOrder";
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", filterGroupID));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void UpdateSortOrder(int filterConditionID, int sortOrder, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterCondition_UpdateSortOrder_FilterConditionID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@FilterConditionID", filterConditionID);
            cmd.Parameters.AddWithValue("@SortOrder", sortOrder);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
