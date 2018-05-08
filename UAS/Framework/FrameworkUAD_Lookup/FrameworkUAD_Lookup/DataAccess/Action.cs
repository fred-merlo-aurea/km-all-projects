using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    [Serializable]
    public class Action
    {
        //public static bool Exists(int actionTypeID,int categoryCodeID, int transactionCodeID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_Action_Exists";
        //    cmd.Parameters.AddWithValue("@ActionTypeID", actionTypeID);
        //    cmd.Parameters.AddWithValue("@CategoryCodeID", categoryCodeID);
        //    cmd.Parameters.AddWithValue("@TransactionCodeID", transactionCodeID);
        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAF_Circulation.ToString())) > 0 ? true : false;
        //}

        public static List<Entity.Action> Select()
        {
           

            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Entity.Action> retItem = (List<Entity.Action>) CacheUtil.GetFromCache("Action", DatabaseName);

                if (retItem == null)
                {
                    retItem = GetData();

                    CacheUtil.AddToCache("Action", retItem, DatabaseName);
                }

                return retItem;
            }
            else
            {
                return GetData();
            }
        }
        public static Entity.Action Select(int categoryCodeID, int transactionCodeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Action_Select_CatCodeID_TranCodeID";
            cmd.Parameters.AddWithValue("@CategoryCodeID", categoryCodeID);
            cmd.Parameters.AddWithValue("@TransactionCodeID", transactionCodeID);
            return Get(cmd);
        }
        public static Entity.Action Select(int actionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Action_Select_ActionID";
            cmd.Parameters.AddWithValue("@ActionID", actionID);
            return Get(cmd);
        }
        private static Entity.Action Get(SqlCommand cmd)
        {
            Entity.Action retItem = null;
            try
            {
                using (var rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Action();
                        DynamicBuilder<Entity.Action> builder = DynamicBuilder<Entity.Action>.CreateBuilder(rdr);
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
        private static List<Entity.Action> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Action_Select";
            List<Entity.Action> retList = new List<Entity.Action>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Action retItem = new Entity.Action();
                        DynamicBuilder<Entity.Action> builder = DynamicBuilder<Entity.Action>.CreateBuilder(rdr);
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
            catch(Exception ex) {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static int Save(Entity.Action action)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Action_Save";
            cmd.Parameters.Add(new SqlParameter("@ActionID", action.ActionID));
            cmd.Parameters.Add(new SqlParameter("@ActionTypeID", action.ActionTypeID));
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeID", action.CategoryCodeID));
            cmd.Parameters.Add(new SqlParameter("@Note", action.Note));
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeID", action.TransactionCodeID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
    }
}
