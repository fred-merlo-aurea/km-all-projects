using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.DomainTracker
{
    [Serializable]
    [DataContract]
    public class DomainTrackerFields
    {
        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTrackerFields item)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerFields_Save";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerID", (object)item.DomainTrackerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FieldName", item.FieldName));
            cmd.Parameters.Add(new SqlParameter("@Source", item.Source));
            cmd.Parameters.Add(new SqlParameter("@SourceID", item.SourceID));
            cmd.Parameters.Add(new SqlParameter("@UserID", (object)item.CreatedUserID ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()));
        }

        public static void Delete(int domainTrackerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerFields_Delete_All";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static void Delete(int domainTrackerFieldsID, int domainTrackerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerFields_Delete_Single";
            cmd.Parameters.AddWithValue("@DomainTrackerFieldsID", domainTrackerFieldsID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static DataTable GetByDomainTrackerID_DT(int domainTrackerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerFields_Select_DomainTrackerID";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        public static List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields> GetByDomainTrackerID(int domainTrackerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerFields_Select_DomainTrackerID";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            return GetList(cmd);
        }

        public static bool Exists(string FieldName, int DomainTrackerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerFields_Exists_Name_DomainTrackerID";
            cmd.Parameters.AddWithValue("@FieldName", FieldName);
            cmd.Parameters.AddWithValue("@DomainTrackerID", DomainTrackerID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()).ToString()) > 0 ? true : false;
        }

        private static List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields> retList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.DomainTracker.DomainTrackerFields retItem = new ECN_Framework_Entities.DomainTracker.DomainTrackerFields();
                    var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.DomainTrackerFields>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.DomainTracker.DomainTrackerFields Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.DomainTracker.DomainTrackerFields retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.DomainTracker.DomainTrackerFields();
                    var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.DomainTrackerFields>.CreateBuilder(rdr);
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
    }
}
