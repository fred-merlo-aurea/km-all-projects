using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    [Serializable]
    public class TransactionCode
    {
        public static List<Entity.TransactionCode> Select()
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.TransactionCode> retItem = (List<Entity.TransactionCode>) CacheUtil.GetFromCache("TransactionCode", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData();

            //        CacheUtil.AddToCache("TransactionCode", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData();
            //}
        }
        public static List<Entity.TransactionCode> SelectActiveIsFree(bool isFree)
        {
            
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.TransactionCode> retItem = (List<Entity.TransactionCode>) CacheUtil.GetFromCache("TransactionCode", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData(isFree);

            //        CacheUtil.AddToCache("TransactionCode", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData(isFree);
            //}
        }
        private static Entity.TransactionCode Get(SqlCommand cmd)
        {
            Entity.TransactionCode retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransactionCode();
                        DynamicBuilder<Entity.TransactionCode> builder = DynamicBuilder<Entity.TransactionCode>.CreateBuilder(rdr);
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
        private static List<Entity.TransactionCode> GetData()
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransactionCode_Select";
            List<Entity.TransactionCode> retList = new List<Entity.TransactionCode>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransactionCode retItem = new Entity.TransactionCode();
                        DynamicBuilder<Entity.TransactionCode> builder = DynamicBuilder<Entity.TransactionCode>.CreateBuilder(rdr);
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
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        private static List<Entity.TransactionCode> GetData(bool isFree)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransactionCode_Active_IsFree";
            cmd.Parameters.AddWithValue("@IsFree", isFree);
            List<Entity.TransactionCode> retList = new List<Entity.TransactionCode>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransactionCode retItem = new Entity.TransactionCode();
                        DynamicBuilder<Entity.TransactionCode> builder = DynamicBuilder<Entity.TransactionCode>.CreateBuilder(rdr);
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static int Save(Entity.TransactionCode tc)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransactionCode_Save";
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeID", tc.TransactionCodeID));
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeName", tc.TransactionCodeName));
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeTypeID", tc.TransactionCodeTypeID));
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeValue", tc.TransactionCodeValue));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
    }
}
