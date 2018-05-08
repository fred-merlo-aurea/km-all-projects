using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class FilterDetailSelectedValue
    {
        public static Entity.FilterDetailSelectedValue Get(SqlCommand cmd)
        {
            Entity.FilterDetailSelectedValue retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FilterDetailSelectedValue();
                        DynamicBuilder<Entity.FilterDetailSelectedValue> builder = DynamicBuilder<Entity.FilterDetailSelectedValue>.CreateBuilder(rdr);
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
        public static List<Entity.FilterDetailSelectedValue> GetList(SqlCommand cmd)
        {
            List<Entity.FilterDetailSelectedValue> retList = new List<Entity.FilterDetailSelectedValue>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FilterDetailSelectedValue retItem = new Entity.FilterDetailSelectedValue();
                        DynamicBuilder<Entity.FilterDetailSelectedValue> builder = DynamicBuilder<Entity.FilterDetailSelectedValue>.CreateBuilder(rdr);
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
        public static List<Entity.FilterDetailSelectedValue> Select(int filterDetailID)
        {
            List<Entity.FilterDetailSelectedValue> retItem = new List<Entity.FilterDetailSelectedValue>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterDetailSelectedValue_Select_FilterDetailID";
            cmd.Parameters.Add(new SqlParameter("@FilterDetailID", filterDetailID));

            retItem = GetList(cmd);

            return retItem;
        }
        public static int Save(Entity.FilterDetailSelectedValue x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterDetailSelectedValue_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterDetailSelectedValueId", x.FilterDetailSelectedValueId));
            cmd.Parameters.Add(new SqlParameter("@FilterDetailId", x.FilterDetailId));
            cmd.Parameters.Add(new SqlParameter("@SelectedValue", x.SelectedValue));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
