using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class FilterDetail
    {
        public static Entity.FilterDetail Get(SqlCommand cmd)
        {
            Entity.FilterDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FilterDetail();
                        DynamicBuilder<Entity.FilterDetail> builder = DynamicBuilder<Entity.FilterDetail>.CreateBuilder(rdr);
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
        public static List<Entity.FilterDetail> GetList(SqlCommand cmd)
        {
            List<Entity.FilterDetail> retList = new List<Entity.FilterDetail>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FilterDetail retItem = new Entity.FilterDetail();
                        DynamicBuilder<Entity.FilterDetail> builder = DynamicBuilder<Entity.FilterDetail>.CreateBuilder(rdr);
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
        public static List<Entity.FilterDetail> Select(int filterID)
        {
            List<Entity.FilterDetail> retItem = new List<Entity.FilterDetail>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterDetail_Select_FilterID";
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));

            retItem = GetList(cmd);

            return retItem;
        }
        public static int Save(Entity.FilterDetail x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterDetailId", x.FilterDetailId));
            cmd.Parameters.Add(new SqlParameter("@FilterId", x.FilterId));
            cmd.Parameters.Add(new SqlParameter("@FilterTypeId", x.FilterTypeId));
            cmd.Parameters.Add(new SqlParameter("@FilterField", x.FilterField));
            cmd.Parameters.Add(new SqlParameter("@AdHocFromField", (object)x.AdHocFromField ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AdHocToField", (object)x.AdHocToField ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AdHocFieldValue", (object)x.AdHocFieldValue ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterObjectType", (object)x.FilterObjectType ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SearchCondition", x.SearchCondition));
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", x.FilterGroupID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
