using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class ClientSubscription
    {
        public static List<Object.Subscription> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Object.Subscription> Select(string email, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscription_Select_Email";
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static List<Object.Subscription> GetList(SqlCommand cmd)
        {
            var retList = new List<Object.Subscription>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        var retItem = new Object.Subscription();
                        DynamicBuilder<Object.Subscription> builder = DynamicBuilder<Object.Subscription>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
    }
}
