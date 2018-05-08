using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class CodeType
    {
        #region Selects
        public static List<Entity.CodeType> Select()
        {
           if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Entity.CodeType> retItem = (List<Entity.CodeType>) CacheUtil.GetFromCache("CodeType", DatabaseName);

                if (retItem == null)
                {
                    retItem = GetData();

                    CacheUtil.AddToCache("CodeType", retItem, DatabaseName);
                }

                return retItem;
            }
            else
            {
                return GetData();
            }
        }
        public static Entity.CodeType Select(int codeTypeId)
        {
            Entity.CodeType retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeType_Select_CodeTypeId";
            cmd.Parameters.AddWithValue("@CodeTypeId", codeTypeId);

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.CodeType Select(Enums.CodeType codeType)
        {
            Entity.CodeType retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeType_Select_CodeTypeName";
            cmd.Parameters.AddWithValue("@CodeTypeName", codeType.ToString().Replace("_", " "));

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.CodeType Get(SqlCommand cmd)
        {
            Entity.CodeType retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.CodeType();
                        DynamicBuilder<Entity.CodeType> builder = DynamicBuilder<Entity.CodeType>.CreateBuilder(rdr);
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
        public static List<Entity.CodeType> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeType_Select";
            List<Entity.CodeType> retList = new List<Entity.CodeType>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.CodeType retItem = new Entity.CodeType();
                        DynamicBuilder<Entity.CodeType> builder = DynamicBuilder<Entity.CodeType>.CreateBuilder(rdr);
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
        #endregion

        public static int Save(Entity.CodeType x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeType_Save";
            cmd.Parameters.Add(new SqlParameter("@CodeTypeId", x.CodeTypeId));
            cmd.Parameters.Add(new SqlParameter("@CodeTypeName", x.CodeTypeName));
            cmd.Parameters.Add(new SqlParameter("@CodeTypeDescription", x.CodeTypeDescription));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
    }
}
