﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformationDetails
    {
        public static List<Entity.TransformationDetail> Select()
        {
            List<Entity.TransformationDetail> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationDetail_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.TransformationDetail Get(SqlCommand cmd)
        {
            Entity.TransformationDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformationDetail();
                        DynamicBuilder<Entity.TransformationDetail> builder = DynamicBuilder<Entity.TransformationDetail>.CreateBuilder(rdr);
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
        private static List<Entity.TransformationDetail> GetList(SqlCommand cmd)
        {
            List<Entity.TransformationDetail> retList = new List<Entity.TransformationDetail>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformationDetail retItem = new Entity.TransformationDetail();
                        DynamicBuilder<Entity.TransformationDetail> builder = DynamicBuilder<Entity.TransformationDetail>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformationDetail x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformationDetailID", x.TransformationDetailID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@Detail", x.Detail));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}