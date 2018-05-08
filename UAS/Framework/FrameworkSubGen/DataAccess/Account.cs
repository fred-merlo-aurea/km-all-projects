using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class Account
    {
        private const string ClassName = "FrameworkSubGen.DataAccess.Account";
        private const string CommandTextSaveBulkXml = "e_Account_SaveBulkXml";

        public static bool SaveBulkXml(string xml)
        {
            return DataAccessBase.SaveBulkXml(xml, CommandTextSaveBulkXml, ClassName);
        }

        public static Entity.Account Select(string KMFtpFolder)
        {
            Entity.Account retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Account_Select_KMFtpFolder";
            cmd.Parameters.AddWithValue("@KMFtpFolder", KMFtpFolder);
            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.Account Select(int kmClientId)
        {
            Entity.Account retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Account_Select_KMClientId";
            cmd.Parameters.AddWithValue("@KMClientId", kmClientId);
            retItem = Get(cmd);
            return retItem;
        }
        public static List<Entity.Account> Select()
        {
            List<Entity.Account> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Account_Select";
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.Account Get(SqlCommand cmd)
        {
            Entity.Account retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Account();
                        DynamicBuilder<Entity.Account> builder = DynamicBuilder<Entity.Account>.CreateBuilder(rdr);
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
        private static List<Entity.Account> GetList(SqlCommand cmd)
        {
            List<Entity.Account> retList = new List<Entity.Account>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Account retItem = new Entity.Account();
                        DynamicBuilder<Entity.Account> builder = DynamicBuilder<Entity.Account>.CreateBuilder(rdr);
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
