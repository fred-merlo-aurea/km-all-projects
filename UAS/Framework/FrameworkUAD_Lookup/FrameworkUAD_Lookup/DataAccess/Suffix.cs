using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class Suffix
    {
        public static List<Entity.Suffix> Select()
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.Suffix> retItem = (List<Entity.Suffix>) CacheUtil.GetFromCache("Suffix", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData();

            //        CacheUtil.AddToCache("Suffix", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData();
            //}
        }
        public static List<Entity.Suffix> Select(int SuffixCodeTypeID)
        {
            //if (CacheUtil.IsCacheEnabled())
            //{
            //    string DatabaseName = "UAD_LOOKUP";

            //    List<Entity.Suffix> retItem = (List<Entity.Suffix>) CacheUtil.GetFromCache("SuffixCodeTypeID", DatabaseName);

            //    if (retItem == null)
            //    {
            //        retItem = GetData(SuffixCodeTypeID);

            //        CacheUtil.AddToCache("SuffixCodeTypeID", retItem, DatabaseName);
            //    }

            //    return retItem;
            //}
            //else
            //{
                return GetData(SuffixCodeTypeID);
            //}
        }

        public static int Save(Entity.Suffix x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Suffix_Save";
            cmd.Parameters.Add(new SqlParameter("@SuffixID", x.SuffixID));
            cmd.Parameters.Add(new SqlParameter("@SuffixCodeTypeID", x.SuffixCodeTypeID));
            cmd.Parameters.Add(new SqlParameter("@SuffixName", x.SuffixName));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }

        public static List<Entity.Suffix> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Suffix_Select";
            List<Entity.Suffix> retList = new List<Entity.Suffix>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Suffix retItem = new Entity.Suffix();
                        DynamicBuilder<Entity.Suffix> builder = DynamicBuilder<Entity.Suffix>.CreateBuilder(rdr);
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
        public static List<Entity.Suffix> GetData(int SuffixCodeTypeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Suffix_SuffixCodeTypeID";
            cmd.Parameters.AddWithValue("@SuffixID", SuffixCodeTypeID);
            List<Entity.Suffix> retList = new List<Entity.Suffix>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Suffix retItem = new Entity.Suffix();
                        DynamicBuilder<Entity.Suffix> builder = DynamicBuilder<Entity.Suffix>.CreateBuilder(rdr);
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
    }
}
