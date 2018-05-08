using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class Bundle
    {
        private const string ClassName = "FrameworkSubGen.DataAccess.Bundle";
        private const string CommandTextSaveBulkXml = "e_Bundle_SaveBulkXml";

        public static bool SaveBulkXml(string xml)
        {
            return DataAccessBase.SaveBulkXml(xml, CommandTextSaveBulkXml, ClassName);
        }

        public static Entity.Bundle Select(string name, int accountId)
        {
            Entity.Bundle retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Bundle_Select_Name_AccountId";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@accountId", accountId);
            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.Bundle Select(int bundleId)
        {
            Entity.Bundle retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Bundle_Select_Name_BundleId";
            cmd.Parameters.AddWithValue("@bundleId", bundleId);
            retItem = Get(cmd);
            return retItem;
        }
        private static Entity.Bundle Get(SqlCommand cmd)
        {
            Entity.Bundle retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Bundle();
                        DynamicBuilder<Entity.Bundle> builder = DynamicBuilder<Entity.Bundle>.CreateBuilder(rdr);
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
        private static List<Entity.Bundle> GetList(SqlCommand cmd)
        {
            List<Entity.Bundle> retList = new List<Entity.Bundle>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Bundle retItem = new Entity.Bundle();
                        DynamicBuilder<Entity.Bundle> builder = DynamicBuilder<Entity.Bundle>.CreateBuilder(rdr);
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
