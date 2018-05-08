using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RuleFieldPredefinedValue
    {
        //public static List<Entity.RuleFieldPredefinedValue> SelectAll()
        //{
        //    List<Entity.RuleFieldPredefinedValue> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_RuleFieldPredefinedValue_SelectAll";
            
        //    retItem = GetList(cmd);
        //    return retItem;
        //}
        //public static List<Entity.RuleFieldPredefinedValue> Select(int ruleFieldId)
        //{
        //    List<Entity.RuleFieldPredefinedValue> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_RuleFieldPredefinedValue_Select_RuleFieldId";
        //    cmd.Parameters.AddWithValue("@ruleFieldId", ruleFieldId);

        //    retItem = DynamicHelper.GetList<Entity.RuleFieldPredefinedValue>(cmd, DataFunctions.ConnectionString.UAS);
        //    return retItem;
        //}
        //private static Entity.RuleFieldPredefinedValue Get(SqlCommand cmd)
        //{
        //    Entity.RuleFieldPredefinedValue retItem = null;
        //    try
        //    {
        //        using (SqlDataReader rdr = DataFunctions.ExecuteReaderNullIfEmpty(cmd, DataFunctions.ConnectionString.UAS.ToString()))
        //        {
        //            if (rdr != null)
        //            {
        //                retItem = new Entity.RuleFieldPredefinedValue();
        //                DynamicBuilder<Entity.RuleFieldPredefinedValue> builder = DynamicBuilder<Entity.RuleFieldPredefinedValue>.CreateBuilder(rdr);
        //                while (rdr.Read())
        //                {
        //                    retItem = builder.Build(rdr);
        //                }
        //                rdr.Close();
        //                rdr.Dispose();
        //            }
        //        }
        //    }
        //    catch { }
        //    finally
        //    {
        //        cmd.Connection.Close();
        //        cmd.Dispose();
        //    }
        //    return retItem;
        //}
        //private static List<Entity.RuleFieldPredefinedValue> GetList(SqlCommand cmd)
        //{
        //    List<Entity.RuleFieldPredefinedValue> retList = new List<Entity.RuleFieldPredefinedValue>();
        //    try
        //    {
        //        using (SqlDataReader rdr = DataFunctions.ExecuteReaderNullIfEmpty(cmd, DataFunctions.ConnectionString.UAS.ToString()))
        //        {
        //            if (rdr != null)
        //            {
        //                Entity.RuleFieldPredefinedValue retItem = new Entity.RuleFieldPredefinedValue();
        //                DynamicBuilder<Entity.RuleFieldPredefinedValue> builder = DynamicBuilder<Entity.RuleFieldPredefinedValue>.CreateBuilder(rdr);
        //                while (rdr.Read())
        //                {
        //                    retItem = builder.Build(rdr);
        //                    if (retItem != null)
        //                    {
        //                        retList.Add(retItem);
        //                    }
        //                }
        //                rdr.Close();
        //                rdr.Dispose();
        //            }
        //        }
        //    }
        //    catch { }
        //    finally
        //    {
        //        cmd.Connection.Close();
        //        cmd.Dispose();
        //    }
        //    return retList;
        //}+
        //public static List<Entity.RuleFieldPredefinedValue> CreateForClient(string xml, int clientId, KMPlatform.Object.ClientConnections clientCon)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "job_RuleFieldPredefinedValue_Setup";
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(clientCon);
        //    cmd.Parameters.AddWithValue("@xml", xml);
        //    cmd.Parameters.AddWithValue("@clientId", clientId);

        //    List<Entity.RuleFieldPredefinedValue> retList = new List<Entity.RuleFieldPredefinedValue>();
        //    try
        //    {
        //        using (SqlDataReader rdr = DataFunctions.ExecuteReaderNullIfEmpty(cmd))
        //        {
        //            if (rdr != null)
        //            {
        //                Entity.RuleFieldPredefinedValue retItem = new Entity.RuleFieldPredefinedValue();
        //                DynamicBuilder<Entity.RuleFieldPredefinedValue> builder = DynamicBuilder<Entity.RuleFieldPredefinedValue>.CreateBuilder(rdr);
        //                while (rdr.Read())
        //                {
        //                    retItem = builder.Build(rdr);
        //                    if (retItem != null)
        //                    {
        //                        retList.Add(retItem);
        //                    }
        //                }
        //                rdr.Close();
        //                rdr.Dispose();
        //            }
        //        }
        //    }
        //    catch { }
        //    finally
        //    {
        //        cmd.Connection.Close();
        //        cmd.Dispose();
        //    }
        //    return retList;
        //}
        //public static bool SaveBulkSqlInsert(List<Entity.RuleFieldPredefinedValue> list)
        //{
        //    DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.RuleFieldPredefinedValue>.ToDataTable(list);
        //    bool done = true;
        //    SqlConnection conn = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());
        //    SqlBulkCopy bc = default(SqlBulkCopy);
        //    try
        //    {
        //        conn.Open();
        //        bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
        //        bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "RuleFieldPredefinedValue");
        //        bc.BatchSize = 0;
        //        bc.BulkCopyTimeout = 0;

        //        bc.ColumnMappings.Add("RuleFieldId", "RuleFieldId");
        //        bc.ColumnMappings.Add("ItemText", "ItemText");
        //        bc.ColumnMappings.Add("ItemValue", "ItemValue");
        //        bc.ColumnMappings.Add("ItemOrder", "ItemOrder");
        //        bc.ColumnMappings.Add("IsActive", "IsActive");
        //        bc.ColumnMappings.Add("CreatedDate", "CreatedDate");
        //        bc.ColumnMappings.Add("CreatedByUserId", "CreatedByUserId");
        //        bc.ColumnMappings.Add("UpdatedByUserId", "UpdatedByUserId");

        //        bc.WriteToServer(dt);
        //        bc.Close();
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        done = false;
        //        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();

        //        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
        //        {
        //            string pattern = @"\d+";
        //            Match match = Regex.Match(ex.Message.ToString(), pattern);
        //            var index = Convert.ToInt32(match.Value) - 1;

        //            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
        //            var sortedColumns = fi.GetValue(bc);
        //            var items = (object[]) sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

        //            FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
        //            var metadata = itemdata.GetValue(items[index]);

        //            var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
        //            var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
        //            var formatEx = String.Format("Column: {0} contains data with a length greater than: {1}", column, length);
        //            fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, formatEx.ToString(), "RuleFieldPredefinedValue.SaveBulkSqlInsert"));
        //        }

        //        fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "RuleFieldPredefinedValue.SaveBulkSqlInsert"));
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }

        //    return done;
        //}

        public static List<Object.RuleFieldNeedValue> GetRuleFieldsNeedingValues()
        {
            List<Object.RuleFieldNeedValue> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_RuleFieldNeedValue";

            retItem = GetNeedValueList(cmd);
            return retItem;
        }
        private static List<Object.RuleFieldNeedValue> GetNeedValueList(SqlCommand cmd)
        {
            List<Object.RuleFieldNeedValue> retList = new List<Object.RuleFieldNeedValue>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.RuleFieldNeedValue retItem = new Object.RuleFieldNeedValue();
                        DynamicBuilder<Object.RuleFieldNeedValue> builder = DynamicBuilder<Object.RuleFieldNeedValue>.CreateBuilder(rdr);
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
