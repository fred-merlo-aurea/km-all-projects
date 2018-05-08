using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    [DataContract]
    public class LandingPageAssignContent
    {
        public static List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> GetByLPAID(int LPAID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssignContent_Select_LPAID";
            cmd.Parameters.AddWithValue("@LPAID", LPAID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssignContent GetByLPACID(int LPACID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssignContent_Select_LPACID";
            cmd.Parameters.AddWithValue("@LPACID", LPACID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> GetByLPOID(int LPOID, int LPAID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssignContent_Select_LPOID_LPAID";
            cmd.Parameters.AddWithValue("@LPOID", LPOID);
            cmd.Parameters.AddWithValue("@LPAID", LPAID);
            return GetList(cmd);
        }
        private static ECN_Framework_Entities.Accounts.LandingPageAssignContent Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.LandingPageAssignContent retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.LandingPageAssignContent();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.LandingPageAssignContent>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> retList = new List<ECN_Framework_Entities.Accounts.LandingPageAssignContent>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.LandingPageAssignContent retItem = new ECN_Framework_Entities.Accounts.LandingPageAssignContent();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.LandingPageAssignContent>.CreateBuilder(rdr);
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


        public static void Delete(int LPAID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssignContent_Delete_LPAID";
            cmd.Parameters.AddWithValue("@LPAID", LPAID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static int Save(ECN_Framework_Entities.Accounts.LandingPageAssignContent lpac, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssignContent_Save";
            cmd.Parameters.Add(new SqlParameter("@Display", lpac.Display));
            cmd.Parameters.Add(new SqlParameter("@LPAID", lpac.LPAID));
            cmd.Parameters.Add(new SqlParameter("@LPOID", lpac.LPOID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Parameters.AddWithValue("@SortOrder", lpac.SortOrder.HasValue ? lpac.SortOrder.Value : (object)DBNull.Value) ;
            cmd.Parameters.AddWithValue("@IsDeleted", lpac.IsDeleted.HasValue ? lpac.IsDeleted : (object)DBNull.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()).ToString()); 
        }

        public static DataTable GetReasons(int CustomerID, DateTime fromDate, DateTime toDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageAssignContent_GetSelectedReasons";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@FromDate", fromDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@ToDate", toDate.ToShortDateString());

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }
    }
}
