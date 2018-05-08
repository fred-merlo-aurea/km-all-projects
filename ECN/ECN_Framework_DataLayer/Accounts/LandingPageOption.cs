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
    public class LandingPageOption
    {
        public static bool Exists(int LPOID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageOption_Exists_ByLPOID";
            cmd.Parameters.AddWithValue("@LPOID", LPOID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPageOption> GetByLPID(int LPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageOption_Select_LPID";
            cmd.Parameters.AddWithValue("@LPID", LPID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.LandingPageOption GetByLPOID(int LPOID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPageOption_Select_LPOID";
            cmd.Parameters.AddWithValue("@LPID", LPOID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.LandingPageOption Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.LandingPageOption retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.LandingPageOption();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.LandingPageOption>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Accounts.LandingPageOption> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageOption> retList = new List<ECN_Framework_Entities.Accounts.LandingPageOption>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.LandingPageOption retItem = new ECN_Framework_Entities.Accounts.LandingPageOption();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.LandingPageOption>.CreateBuilder(rdr);
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
