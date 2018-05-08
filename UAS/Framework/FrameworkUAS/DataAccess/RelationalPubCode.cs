using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RelationalPubCode
    {
        public static List<Entity.RelationalPubCode> Select(int clientID)
        {
            List<Entity.RelationalPubCode> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RelationalPubCode_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.RelationalPubCode> Select(int clientID, string specialFileName)
        {
            List<Entity.RelationalPubCode> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RelationalPubCode_Select_ClientID_SpecialFileName";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@SpecialFileName", specialFileName);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.RelationalPubCode Get(SqlCommand cmd)
        {
            Entity.RelationalPubCode retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.RelationalPubCode();
                        DynamicBuilder<Entity.RelationalPubCode> builder = DynamicBuilder<Entity.RelationalPubCode>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
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
            return retItem;
        }
        private static List<Entity.RelationalPubCode> GetList(SqlCommand cmd)
        {
            List<Entity.RelationalPubCode> retList = new List<Entity.RelationalPubCode>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.RelationalPubCode retItem = new Entity.RelationalPubCode();
                        DynamicBuilder<Entity.RelationalPubCode> builder = DynamicBuilder<Entity.RelationalPubCode>.CreateBuilder(rdr);
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
