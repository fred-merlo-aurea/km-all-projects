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
    public class LinkTrackingParamOption
    {
        private static string _CacheRegion = "LinkTrackingParamOption";
        public static ECN_Framework_Entities.Communicator.LinkTrackingParamOption GetByLTPOID(int LTPOID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Select_LTPOID";
            cmd.Parameters.AddWithValue("@LTPOID", LTPOID);

            ECN_Framework_Entities.Communicator.LinkTrackingParamOption retItem = null;

            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch (Exception)   {}

            if (isCacheEnabled)
            {
                retItem = (ECN_Framework_Entities.Communicator.LinkTrackingParamOption)KM.Common.CacheUtil.GetFromCache(LTPOID.ToString(), _CacheRegion);
                if (retItem == null)
                {
                    retItem = Get(cmd);
                    if (retItem != null)
                        KM.Common.CacheUtil.AddToCache(LTPOID.ToString(), retItem, _CacheRegion);
                }
            }
            else
                retItem = Get(cmd);
            return retItem;
        }
        
        public static ECN_Framework_Entities.Communicator.LinkTrackingParamOption GetLTPOIDByCustomerID(int LTPID,string Value,int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Select_LTPIDCustomerID";
            cmd.Parameters.AddWithValue("@LTPID", LTPID);
            cmd.Parameters.AddWithValue("@Value", Value);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Get(cmd);
        }
        public static ECN_Framework_Entities.Communicator.LinkTrackingParamOption GetLTPOIDByBaseChannelID(int LTPID, string Value, int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Select_LTPIDBaseChannelID";
            cmd.Parameters.AddWithValue("@LTPID", LTPID);
            cmd.Parameters.AddWithValue("@Value", Value);
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            return Get(cmd);
        }
        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> GetByLTPID(int LTPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Select_LTPID";
            cmd.Parameters.AddWithValue("@LTPID", LTPID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> Getall()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Select_All";
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> Get_CustomerID_LTPID(int CustomerID, int LTPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Select_CustomerID_LTPID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@LTPID", LTPID);

            return GetList(cmd);

        }

        public static DataTable GetDT_CustomerID_LTPID(int CustomerID, int LTPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_LinkTrackingParamOption_Select_CustomerID_LTPID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@LTPID", LTPID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> Get_BaseChannelID_LTPID(int BaseChannelID, int LTPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Select_BaseChannelID_LTPID";
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            cmd.Parameters.AddWithValue("@LTPID", LTPID);

            return GetList(cmd);

        }

        public static DataTable GetDT_BaseChannelID_LTPID(int BaseChannelID, int LTPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_LinkTrackingParamOption_Select_BaseChannelID_LTPID";
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            cmd.Parameters.AddWithValue("@LTPID", LTPID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Insert(ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Insert";
            cmd.Parameters.AddWithValue("@LTPID", ltpo.LTPID);
            cmd.Parameters.AddWithValue("@DisplayName", ltpo.DisplayName);
            cmd.Parameters.AddWithValue("@ColumnName", (object)ltpo.ColumnName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Value", ltpo.Value);
            cmd.Parameters.AddWithValue("@IsActive", ltpo.IsActive);
            cmd.Parameters.AddWithValue("@CustomerID", (object)ltpo.CustomerID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BaseChannelID", (object)ltpo.BaseChannelID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsDynamic", ltpo.IsDynamic);
            cmd.Parameters.AddWithValue("@IsDefault", ltpo.IsDefault);
            cmd.Parameters.AddWithValue("@CreatedUserID", ltpo.CreatedUserID);
            cmd.Parameters.AddWithValue("@CreatedDate", ltpo.CreatedDate);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));

        }

        public static void Update(ECN_Framework_Entities.Communicator.LinkTrackingParamOption ltpo)
        {
            if (ltpo.LTPOID > 0)
                DeleteCache(ltpo.LTPOID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_Update";
            cmd.Parameters.AddWithValue("@LTPOID", ltpo.LTPOID);
            cmd.Parameters.AddWithValue("@LTPID", ltpo.LTPID);
            cmd.Parameters.AddWithValue("@DisplayName", ltpo.DisplayName);
            cmd.Parameters.AddWithValue("@ColumnName", (object)ltpo.ColumnName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Value", ltpo.Value);
            cmd.Parameters.AddWithValue("@IsActive", ltpo.IsActive);
            cmd.Parameters.AddWithValue("@IsDeleted", ltpo.IsDeleted);
            cmd.Parameters.AddWithValue("@CustomerID", (object)ltpo.CustomerID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BaseChannelID", (object)ltpo.BaseChannelID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsDynamic", ltpo.IsDynamic);
            cmd.Parameters.AddWithValue("@IsDefault", ltpo.IsDefault);
            cmd.Parameters.AddWithValue("@UpdatedUserID", ltpo.UpdatedUserID);
            cmd.Parameters.AddWithValue("@UpdatedDate", ltpo.UpdatedDate);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static void DeleteCache(int LTPOID)
        {
            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                if (KM.Common.CacheUtil.GetFromCache(LTPOID.ToString(), _CacheRegion) != null)
                    KM.Common.CacheUtil.RemoveFromCache(LTPOID.ToString(), _CacheRegion);
            }
        }

        public static void ResetCustDefault(int LTPID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_ResetCustDefault";
            cmd.Parameters.AddWithValue("@LTPID", LTPID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void ResetBaseDefault(int LTPID, int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamOption_ResetBaseDefault";
            cmd.Parameters.AddWithValue("@LTPID", LTPID);
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption> retList = new List<ECN_Framework_Entities.Communicator.LinkTrackingParamOption>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.LinkTrackingParamOption retItem = new ECN_Framework_Entities.Communicator.LinkTrackingParamOption();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingParamOption>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.LinkTrackingParamOption Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParamOption retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.LinkTrackingParamOption();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingParamOption>.CreateBuilder(rdr);
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
