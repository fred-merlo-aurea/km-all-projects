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
    public class ConditionGroup
    {
        public static ECN_Framework_Entities.FormDesigner.ConditionGroup GetByConditionGroupID(int CondGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConditionGroup_Select_ConditionGroupID";
            cmd.Parameters.AddWithValue("@ConditionGroupID", CondGroupID);

            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.FormDesigner.ConditionGroup> GetByMainGroupID(int CondGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConditionGroup_Select_MainGroupID";
            cmd.Parameters.AddWithValue("@MainGroupID", CondGroupID);

            return GetList(cmd);
        }


        public static int Save(ECN_Framework_Entities.FormDesigner.ConditionGroup condGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConditionGroup_Save";

            cmd.Parameters.AddWithValue("@ConditionGroup_Seq_ID", condGroup.ConditionGroup_Seq_ID);
            cmd.Parameters.AddWithValue("@MainGroup_ID", condGroup.MainGroup_ID);
            cmd.Parameters.AddWithValue("@LogicGroup", condGroup.LogicGroup);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()).ToString());
        }

        public static void Delete(int condGroup_ID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConditionGroup_Delete_ConditionGroupID";

            cmd.Parameters.AddWithValue("@ConditionGroup_Seq_ID", condGroup_ID);
            

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.FormDesigner.ToString());
        }


        private static List<ECN_Framework_Entities.FormDesigner.ConditionGroup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.FormDesigner.ConditionGroup> retList = new List<ECN_Framework_Entities.FormDesigner.ConditionGroup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.FormDesigner.ConditionGroup retItem = new ECN_Framework_Entities.FormDesigner.ConditionGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.ConditionGroup>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.FormDesigner.ConditionGroup Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.FormDesigner.ConditionGroup retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.FormDesigner.ConditionGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.ConditionGroup>.CreateBuilder(rdr);
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
