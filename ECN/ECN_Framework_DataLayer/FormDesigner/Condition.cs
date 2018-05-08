using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.FormDesigner
{
    [Serializable]
    public class Condition
    {
        public static ECN_Framework_Entities.FormDesigner.Condition GetByConditionID(int CondID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Condition_Select_ConditionID";
            cmd.Parameters.AddWithValue("@ConditionID", CondID);

            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.FormDesigner.Condition> GetByConditionGroup_ID(int CondGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Condition_Select_ConditionGroupID";
            cmd.Parameters.AddWithValue("@ConditionGroupID", CondGroupID);

            return GetList(cmd);
        }

        public static void DeleteByID(int CondID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Condition_Delete_ConditionID";

            cmd.Parameters.AddWithValue("@Condition_Seq_ID", CondID);


            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.FormDesigner.ToString());
        }

        public static void DeleteByConditionGroupID(int CondGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Condition_Delete_ConditionGroupID";

            cmd.Parameters.AddWithValue("@ConditionGroup_Seq_ID", CondGroupID);


            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.FormDesigner.ToString());
        }

        public static int Save(ECN_Framework_Entities.FormDesigner.Condition c)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Condition_Save";

            cmd.Parameters.AddWithValue("@Control_ID", c.Control_ID);
            cmd.Parameters.AddWithValue("@ConditionGroup_Seq_ID", c.ConditionGroup_Seq_ID);
            cmd.Parameters.AddWithValue("@Operation_ID", c.Operation_ID);
            cmd.Parameters.AddWithValue("@Value", c.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()).ToString());
        }


        private static List<ECN_Framework_Entities.FormDesigner.Condition> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.FormDesigner.Condition> retList = new List<ECN_Framework_Entities.FormDesigner.Condition>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.FormDesigner.Condition retItem = new ECN_Framework_Entities.FormDesigner.Condition();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.Condition>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.FormDesigner.Condition Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.FormDesigner.Condition retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.FormDesigner.Condition();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.Condition>.CreateBuilder(rdr);
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
    }
}
