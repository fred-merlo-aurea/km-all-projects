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
    public class GlobalMasterSuppressionList
    {
        public static void Delete(string emailAddress, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GlobalMasterSuppressionList_Delete";
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int gsid, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GlobalMasterSuppressionList_Delete_GSID";
            cmd.Parameters.AddWithValue("@GSID", gsid);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GlobalMasterSuppressionList_Select_All";
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> GetByEmailAddress(string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GlobalMasterSuppressionList_Select_EmailAddress";
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            return GetList(cmd);
        }

        public static DataSet GetByEmailAddress_Paging(int pageNo, int pageSize, string searchString)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GlobalMasterSuppressionList_Select_EmailAddress_Paging";
            cmd.Parameters.AddWithValue("@PageNo", pageNo);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@EmailSearchString", searchString);
            return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> Search_Paging(int pageNo, int pageSize, string searchString, string sortColumn, string sortDirection)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GlobalMasterSuppressionList_Search_Paging";
            cmd.Parameters.AddWithValue("@PageIndex", pageNo);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@EmailSearchString", searchString);
            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> retList = new List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList retItem = new ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList>.CreateBuilder(rdr);
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
