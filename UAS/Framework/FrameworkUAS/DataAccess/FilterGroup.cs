﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class FilterGroup
    {
        public static Entity.FilterGroup Get(SqlCommand cmd)
        {
            Entity.FilterGroup retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FilterGroup();
                        DynamicBuilder<Entity.FilterGroup> builder = DynamicBuilder<Entity.FilterGroup>.CreateBuilder(rdr);
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
        public static List<Entity.FilterGroup> GetList(SqlCommand cmd)
        {
            List<Entity.FilterGroup> retList = new List<Entity.FilterGroup>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FilterGroup retItem = new Entity.FilterGroup();
                        DynamicBuilder<Entity.FilterGroup> builder = DynamicBuilder<Entity.FilterGroup>.CreateBuilder(rdr);
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
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static int Save(Entity.FilterGroup x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterGroup_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterGroupId", x.FilterGroupId));
            cmd.Parameters.Add(new SqlParameter("@FilterId", x.FilterId));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", x.SortOrder));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}