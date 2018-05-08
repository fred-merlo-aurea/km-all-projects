using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class RuleField
    {
        public static List<Object.RuleFieldSelectListItem> GetDropDownList(int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_RuleFieldSelectListItem_ClientId";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            return DynamicHelper.GetList<Object.RuleFieldSelectListItem>(cmd, ConnectionString.UAS);
        }

        //public static List<Entity.RuleField> SelectAll()
        //{
        //    List<Entity.RuleField> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_RuleField_SelectAll";
            
        //    retItem = DynamicHelper.GetList<Entity.RuleField>(cmd, DataFunctions.ConnectionString.UAS);
        //    return retItem;
        //}
        //public static Entity.RuleField Select(int clientId, string field, bool isActive)
        //{
        //    Entity.RuleField x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_RuleField_Select_ClientId_Field_IsActive";
        //    cmd.Parameters.AddWithValue("@clientId", clientId);
        //    cmd.Parameters.AddWithValue("@field", field);
        //    cmd.Parameters.AddWithValue("@isActive", isActive);
            
        //    x = DynamicHelper.Get<Entity.RuleField>(cmd, DataFunctions.ConnectionString.UAS);
        //    return x;
        //}
        //public static Entity.RuleField Select(int ruleFieldId)
        //{
        //    Entity.RuleField x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_RuleField_Select_RuleFieldId";
        //    cmd.Parameters.AddWithValue("@ruleFieldId", ruleFieldId);

        //    x = DynamicHelper.Get<Entity.RuleField>(cmd, DataFunctions.ConnectionString.UAS);
        //    return x;
        //}
        //public static Entity.RuleField Select(int clientId, string dataTable, string field, bool isActive)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_RuleField_Select_ClientId_DataTable_Field_IsActive";
        //    cmd.Parameters.AddWithValue("@clientId", clientId);
        //    cmd.Parameters.AddWithValue("@dataTable", dataTable);
        //    cmd.Parameters.AddWithValue("@field", field);
        //    cmd.Parameters.AddWithValue("@isActive", isActive);

        //    return DynamicHelper.Get<Entity.RuleField>(cmd, DataFunctions.ConnectionString.UAS);
        //}
        //private static List<Entity.RuleField> GetList(SqlCommand cmd)
        //{
        //    List<Entity.RuleField> retList = new List<Entity.RuleField>();
        //    try
        //    {
        //        using (SqlDataReader rdr = DataFunctions.ExecuteReaderNullIfEmpty(cmd, DataFunctions.ConnectionString.UAS.ToString()))
        //        {
        //            if (rdr != null)
        //            {
        //                Entity.RuleField retItem = new Entity.RuleField();
        //                DynamicBuilder<Entity.RuleField> builder = DynamicBuilder<Entity.RuleField>.CreateBuilder(rdr);
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

        //public static void CreateForClient(string xml, int clientId)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "job_RuleField_Setup";
        //    cmd.Parameters.AddWithValue("@xml", xml);
        //    cmd.Parameters.AddWithValue("@clientId", clientId);
        //    DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.UAS.ToString());
        //}

        //public static bool CreateForClient(string xml)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "job_RuleField_Setup";
        //    cmd.Parameters.AddWithValue("@xml", xml);
        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAS.ToString()).ToString()) > 0 ? true : false;
        //}
    }
}
