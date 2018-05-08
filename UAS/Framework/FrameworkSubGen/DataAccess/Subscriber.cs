using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM;
using KM.Common;
using KM.Common.Data;

namespace FrameworkSubGen.DataAccess
{
    public class Subscriber
    {
        public static bool SaveBulkXml(string xml)
        {
            bool success = false;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Subscriber_SaveBulkXml";
                cmd.Parameters.AddWithValue("@xml", xml);
                cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());

                success = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Subscriber", "SaveBulkXml");
            }
            return success;
        }
        public static List<Entity.Subscriber> FindSubscribers(Dictionary<string,string> parameters)
        {
            List<Entity.Subscriber> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Subscriber_Select_Dynamic";
            string whereClause = " where ";
            int total = parameters.Count;
            int item = 1;
            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                if(item == total)
                    whereClause += kvp.Key + " = '" + kvp.Value + "' ";
                else
                    whereClause += kvp.Key + " = '" + kvp.Value + "' and ";
                item++;
            }
            cmd.Parameters.AddWithValue("@whereClause", whereClause);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.Subscriber Get(SqlCommand cmd)
        {
            Entity.Subscriber retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Subscriber();
                        DynamicBuilder<Entity.Subscriber> builder = DynamicBuilder<Entity.Subscriber>.CreateBuilder(rdr);
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
        private static List<Entity.Subscriber> GetList(SqlCommand cmd)
        {
            List<Entity.Subscriber> retList = new List<Entity.Subscriber>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.SubGenData.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Subscriber retItem = new Entity.Subscriber();
                        DynamicBuilder<Entity.Subscriber> builder = DynamicBuilder<Entity.Subscriber>.CreateBuilder(rdr);
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
        public static bool Save(Entity.Subscriber sub)
        {
            List<Entity.Subscriber> list = new List<Entity.Subscriber>();
            list.Add(sub);
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.Subscriber>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());
            SqlBulkCopy bc = default(SqlBulkCopy);
            try
            {
                conn.Open();
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "Subscriber");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("subscriber_id", "subscriber_id");
                bc.ColumnMappings.Add("account_id", "account_id");
                bc.ColumnMappings.Add("renewal_code", "renewal_code");
                bc.ColumnMappings.Add("email", "email");
                bc.ColumnMappings.Add("password", "password");
                bc.ColumnMappings.Add("password_md5", "password_md5");
                bc.ColumnMappings.Add("first_name", "first_name");
                bc.ColumnMappings.Add("last_name", "last_name");
                bc.ColumnMappings.Add("source", "source");
                bc.ColumnMappings.Add("create_date", "create_date");
                bc.ColumnMappings.Add("delete_date", "delete_date");
                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Subscriber", "Save");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
        public static bool Save(List<Entity.Subscriber> list)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.Subscriber>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.SubGenData.ToString());
            SqlBulkCopy bc = default(SqlBulkCopy);
            try
            {
                conn.Open();
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "Subscriber");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("subscriber_id", "subscriber_id");
                bc.ColumnMappings.Add("account_id", "account_id");
                bc.ColumnMappings.Add("renewal_code", "renewal_code");
                bc.ColumnMappings.Add("email", "email");
                bc.ColumnMappings.Add("password", "password");
                bc.ColumnMappings.Add("password_md5", "password_md5");
                bc.ColumnMappings.Add("first_name", "first_name");
                bc.ColumnMappings.Add("last_name", "last_name");
                bc.ColumnMappings.Add("source", "source");
                bc.ColumnMappings.Add("create_date", "create_date");
                bc.ColumnMappings.Add("delete_date", "delete_date");
                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                BusinessLogic.API.Authentication.SaveApiLog(ex, "FrameworkSubGen.DataAccess.Subscriber", "Save");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
    }
}
