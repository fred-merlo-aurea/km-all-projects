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
    public class ChannelMasterSuppressionList
    {
        public static void Delete(int baseChannelID, string emailAddress, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelMasterSuppressionList_Delete";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int baseChannelID, int cmsid, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelMasterSuppressionList_Delete_CMSID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@CMSID", cmsid);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> GetByBaseChannelID(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelMasterSuppressionList_Select_BaseChannelID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> GetByBaseChannelID_Paging(int baseChannelID, string email, int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelMasterSuppressionList_Select_Paging";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> GetByEmailAddress(int baseChannelID, string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelMasterSuppressionList_Select_EmailAddress";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            return GetList(cmd);
        }

        public static DataSet GetByEmailAddress_Paging(int baseChannelID, int pageNo, int pageSize, string searchString)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelMasterSuppressionList_Select_EmailAddress_Paging";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@PageNo", pageNo);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@EmailSearchString", searchString);
            return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> retList = new List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList retItem = new ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList>.CreateBuilder(rdr);
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
    }
}
