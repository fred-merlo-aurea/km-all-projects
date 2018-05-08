using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    [Serializable]
    public class CategoryCodeType
    {
        public static List<Entity.CategoryCodeType> Select()
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.CategoryCodeType> retItem = (List<Entity.CategoryCodeType>) CacheUtil.GetFromCache("CategoryCodeType", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData();

            //        CacheUtil.AddToCache("CategoryCodeType", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData();
            //}
           
        }
        public static List<Entity.CategoryCodeType> Select(bool isFree)
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.CategoryCodeType> retItem = (List<Entity.CategoryCodeType>) CacheUtil.GetFromCache("CategoryCodeTypeIsFree", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData(isFree);

            //        CacheUtil.AddToCache("CategoryCodeTypeIsFree", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData(isFree);
            //}
        }
        private static Entity.CategoryCodeType Get(SqlCommand cmd)
        {
            Entity.CategoryCodeType retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.CategoryCodeType();
                        DynamicBuilder<Entity.CategoryCodeType> builder = DynamicBuilder<Entity.CategoryCodeType>.CreateBuilder(rdr);
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
        private static List<Entity.CategoryCodeType> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CategoryCodeType_Select";
            List<Entity.CategoryCodeType> retList = new List<Entity.CategoryCodeType>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.CategoryCodeType retItem = new Entity.CategoryCodeType();
                        DynamicBuilder<Entity.CategoryCodeType> builder = DynamicBuilder<Entity.CategoryCodeType>.CreateBuilder(rdr);
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

        private static List<Entity.CategoryCodeType> GetData(bool isFree)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CategoryCodeType_Select_IsFree";
            cmd.Parameters.AddWithValue("@IsFree", isFree);
            List<Entity.CategoryCodeType> retList = new List<Entity.CategoryCodeType>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.CategoryCodeType retItem = new Entity.CategoryCodeType();
                        DynamicBuilder<Entity.CategoryCodeType> builder = DynamicBuilder<Entity.CategoryCodeType>.CreateBuilder(rdr);
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

        public static int Save(Entity.CategoryCodeType cct)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CategoryCodeType_Save";
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeTypeID", cct.CategoryCodeTypeID));
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeTypeName", cct.CategoryCodeTypeName));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
    }
}
