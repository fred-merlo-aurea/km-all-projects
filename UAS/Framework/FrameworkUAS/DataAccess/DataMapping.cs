using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataMapping
    {
        public static List<Entity.DataMapping> Select()
        {
            List<Entity.DataMapping> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataMapping_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.DataMapping Get(SqlCommand cmd)
        {
            Entity.DataMapping retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataMapping();
                        DynamicBuilder<Entity.DataMapping> builder = DynamicBuilder<Entity.DataMapping>.CreateBuilder(rdr);
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
        private static List<Entity.DataMapping> GetList(SqlCommand cmd)
        {
            List<Entity.DataMapping> retList = new List<Entity.DataMapping>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataMapping retItem = new Entity.DataMapping();
                        DynamicBuilder<Entity.DataMapping> builder = DynamicBuilder<Entity.DataMapping>.CreateBuilder(rdr);
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

        public static int Save(Entity.DataMapping x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataMapping_Save";
            cmd.Parameters.Add(new SqlParameter("@DataMappingID", x.DataMappingID));
            cmd.Parameters.Add(new SqlParameter("@FieldMapping", x.FieldMapping));
            cmd.Parameters.Add(new SqlParameter("@IncomingValue", x.IncomingValue));
            cmd.Parameters.Add(new SqlParameter("@MAFValue", x.MAFValue));
            cmd.Parameters.Add(new SqlParameter("@Ignore", x.Ignore));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
