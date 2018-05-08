using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    [Serializable]
    public class CategoryCode
    {
        public static List<Entity.CategoryCode> Select()
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.CategoryCode> retItem = (List<Entity.CategoryCode>) CacheUtil.GetFromCache("CategoryCode", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData();

            //        CacheUtil.AddToCache("CategoryCode", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData();
            //}
            
        }
        public static List<Entity.CategoryCode> SelectActiveIsFree(bool isFree)
        {

            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.CategoryCode> retItem = (List<Entity.CategoryCode>) CacheUtil.GetFromCache("CategoryCodeIsFree", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData(isFree);

            //        CacheUtil.AddToCache("CategoryCodeIsFree", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData(isFree);
            //}
        }
        private static Entity.CategoryCode Get(SqlCommand cmd)
        {
            Entity.CategoryCode retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.CategoryCode();
                        DynamicBuilder<Entity.CategoryCode> builder = DynamicBuilder<Entity.CategoryCode>.CreateBuilder(rdr);
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
        private static List<Entity.CategoryCode> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CategoryCode_Select";
            List<Entity.CategoryCode> retList = new List<Entity.CategoryCode>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.CategoryCode retItem = new Entity.CategoryCode();
                        DynamicBuilder<Entity.CategoryCode> builder = DynamicBuilder<Entity.CategoryCode>.CreateBuilder(rdr);
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
        private static List<Entity.CategoryCode> GetData(bool isFree)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CategoryCode_Active_IsFree";
            cmd.Parameters.AddWithValue("@IsFree", isFree);
            List<Entity.CategoryCode> retList = new List<Entity.CategoryCode>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.CategoryCode retItem = new Entity.CategoryCode();
                        DynamicBuilder<Entity.CategoryCode> builder = DynamicBuilder<Entity.CategoryCode>.CreateBuilder(rdr);
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
        public static int Save(Entity.CategoryCode cc)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CategoryCode_Save";
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeID", cc.CategoryCodeID));
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeName", cc.CategoryCodeName));
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeTypeID", cc.CategoryCodeTypeID));
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeValue", cc.CategoryCodeValue));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
    }
}
