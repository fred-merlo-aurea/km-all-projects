using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class ValueOption
    {
        public static bool SaveBulkXml(string xml)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_ValueOption_SaveBulkXml";
                cmd.Parameters.AddWithValue("@xml", xml);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.ValueOption", "SaveBulkXml");
            }
            return success;
        }
        public static List<Entity.ValueOption> Select (int field_id)
        {
            List<Entity.ValueOption> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ValueOption_Select";
            cmd.Parameters.AddWithValue("@field_id", field_id);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ValueOption Get(SqlCommand cmd)
        {
            Entity.ValueOption retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ValueOption();
                        DynamicBuilder<Entity.ValueOption> builder = DynamicBuilder<Entity.ValueOption>.CreateBuilder(rdr);
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
        private static List<Entity.ValueOption> GetList(SqlCommand cmd)
        {
            List<Entity.ValueOption> retList = new List<Entity.ValueOption>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ValueOption retItem = new Entity.ValueOption();
                        DynamicBuilder<Entity.ValueOption> builder = DynamicBuilder<Entity.ValueOption>.CreateBuilder(rdr);
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
