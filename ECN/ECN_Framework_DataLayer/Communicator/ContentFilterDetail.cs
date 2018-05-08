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
    public class ContentFilterDetail
    {
        private const string ProcedureContentFilterDetailDeleteSingle = "e_ContentFilterDetail_Delete_Single";
        private const string ProcedureContentFilterDetailDeleteAll = "e_ContentFilterDetail_Delete_All";

        public static bool Exists(int filterID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ContentFilterDetail_Exists_ByFilterID";
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int filterID, int fdid, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ContentFilterDetail_Exists_ByFDID";
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.Parameters.AddWithValue("@FDID", fdid);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.ContentFilterDetail GetByFDID(int fdid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ContentFilterDetail_Select_FDID";
            cmd.Parameters.AddWithValue("@FDID", fdid);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.ContentFilterDetail Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.ContentFilterDetail retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.ContentFilterDetail();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ContentFilterDetail>.CreateBuilder(rdr);
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

        public static List<ECN_Framework_Entities.Communicator.ContentFilterDetail> GetByFilterID(int filterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ContentFilterDetail_Select_FilterID";
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.ContentFilterDetail> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.ContentFilterDetail> retList = new List<ECN_Framework_Entities.Communicator.ContentFilterDetail>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.ContentFilterDetail retItem = new ECN_Framework_Entities.Communicator.ContentFilterDetail();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ContentFilterDetail>.CreateBuilder(rdr);
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

        public static void Delete(int filterID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryFilter(
                filterID, null, customerID, userID , ProcedureContentFilterDetailDeleteAll);
        }

        public static void Delete(int filterID, int fdid, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryFilter(
                filterID, fdid, customerID, userID , ProcedureContentFilterDetailDeleteSingle);
        }

        public static int Save(ECN_Framework_Entities.Communicator.ContentFilterDetail detail)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ContentFilterDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@FDID", detail.FDID));
            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)detail.FilterID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FieldType", detail.FieldType));
            cmd.Parameters.Add(new SqlParameter("@CompareType", detail.CompareType));
            cmd.Parameters.Add(new SqlParameter("@FieldName", detail.FieldName));
            cmd.Parameters.Add(new SqlParameter("@Comparator", detail.Comparator));
            cmd.Parameters.Add(new SqlParameter("@CompareValue", detail.CompareValue));
            if (detail.FDID > 0)
            {
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)detail.UpdatedUserID ?? DBNull.Value));
               
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)detail.CreatedUserID ?? DBNull.Value));
                
            }

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static DataTable GetByContentIDFilterID(int filterID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_ContentFilterDetail_FilterID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
