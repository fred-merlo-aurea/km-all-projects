using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class Filter
    {
        public static Entity.Filter Get(SqlCommand cmd)
        {
            Entity.Filter retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Filter();
                        DynamicBuilder<Entity.Filter> builder = DynamicBuilder<Entity.Filter>.CreateBuilder(rdr);
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

        public static List<Entity.Filter> GetList(SqlCommand cmd)
        {
            return KM.Common.DataFunctions.GetList<Entity.Filter>(cmd, ConnectionString.UAS.ToString());
        }

        public static List<Entity.Filter> Select(int publicationID)
        {
            List<Entity.Filter> retItem = new List<Entity.Filter>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Select_PublicationID";
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));

            retItem = GetList(cmd);

            return retItem;
        }
        public static List<Entity.Filter> SelectClient(int clientID)
        {
            List<Entity.Filter> retItem = new List<Entity.Filter>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Select";
            cmd.Parameters.Add(new SqlParameter("@ClientID", clientID));

            retItem = GetList(cmd);

            return retItem;
        }
        public static int Save(Entity.Filter x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Save";
            cmd.Parameters.Add(new SqlParameter("@FilterId", x.FilterId));
            cmd.Parameters.Add(new SqlParameter("@FilterName", x.FilterName));
            cmd.Parameters.Add(new SqlParameter("@ProductId", x.ProductId));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@BrandId", x.BrandId));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@FilterGroupID", x.FilterGroupID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
