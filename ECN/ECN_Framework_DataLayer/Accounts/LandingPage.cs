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
    public class LandingPage
    {
        public static bool Exists(int LPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPage_Exists_ByLPID";
            cmd.Parameters.AddWithValue("@LPID", LPID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPage> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPage_Select_ALL";
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.LandingPage GetByLPID(int LPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LandingPage_Select_LPID";
            cmd.Parameters.AddWithValue("@LPID", LPID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.LandingPage Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.LandingPage retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.LandingPage();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.LandingPage>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Accounts.LandingPage> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.LandingPage> retList = new List<ECN_Framework_Entities.Accounts.LandingPage>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.LandingPage retItem = new ECN_Framework_Entities.Accounts.LandingPage();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.LandingPage>.CreateBuilder(rdr);
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
