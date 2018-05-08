using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    [Serializable]
    public class TransactionCodeType
    {
        public static List<Entity.TransactionCodeType> Select()
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.TransactionCodeType> retItem = (List<Entity.TransactionCodeType>) CacheUtil.GetFromCache("TransactionCodeType", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData();

            //        CacheUtil.AddToCache("TransactionCodeType", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData();
            //}
        }
        public static List<Entity.TransactionCodeType> Select(bool isFree)
        {

            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.TransactionCodeType> retItem = (List<Entity.TransactionCodeType>) CacheUtil.GetFromCache("TransactionCodeTypeIsFree", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData(isFree);

            //        CacheUtil.AddToCache("TransactionCodeTypeIsFree", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData(isFree);
            //}
        }
        private static Entity.TransactionCodeType Get(SqlCommand cmd)
        {
            Entity.TransactionCodeType retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransactionCodeType();
                        DynamicBuilder<Entity.TransactionCodeType> builder = DynamicBuilder<Entity.TransactionCodeType>.CreateBuilder(rdr);
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
        private static List<Entity.TransactionCodeType> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransactionCodeType_Select";
            List<Entity.TransactionCodeType> retList = new List<Entity.TransactionCodeType>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransactionCodeType retItem = new Entity.TransactionCodeType();
                        DynamicBuilder<Entity.TransactionCodeType> builder = DynamicBuilder<Entity.TransactionCodeType>.CreateBuilder(rdr);
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
        private static List<Entity.TransactionCodeType> GetData(bool isFree)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransactionCodeType_Select_IsFree";
            cmd.Parameters.AddWithValue("@IsFree", isFree);
            List<Entity.TransactionCodeType> retList = new List<Entity.TransactionCodeType>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransactionCodeType retItem = new Entity.TransactionCodeType();
                        DynamicBuilder<Entity.TransactionCodeType> builder = DynamicBuilder<Entity.TransactionCodeType>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransactionCodeType tct)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransactionCodeType_Save";
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeTypeID", tct.TransactionCodeTypeID));
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeTypeName", tct.TransactionCodeTypeName));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
    }
}
