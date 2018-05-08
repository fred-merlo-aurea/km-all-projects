using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameworkUAD.Entity;
using KM.Common;
using KMPlatform.Object;

namespace FrameworkUAD.DataAccess
{
    public class IssueSplitFilter
    {
        public static int Save(Entity.IssueSplitFilter x,DataTable dtFilterDetails, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplitFilter_FilterDetails_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@FilterID",  x.FilterID ));
            cmd.Parameters.Add(new SqlParameter("@PubId", x.PubId ));
            cmd.Parameters.Add(new SqlParameter("@FilterName", x.FilterName));;
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", (object) x.CreatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object) x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TVP_IssueSplitFilterDetails", dtFilterDetails));
           
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        internal static Entity.IssueSplitFilter SelectFilterID(int filterID, ClientConnections client)
        {
            Entity.IssueSplitFilter retItem = new Entity.IssueSplitFilter();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplitFilter_Select_FilterID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            retItem = Get(cmd);
            return retItem;
        }
        private static Entity.IssueSplitFilter Get(SqlCommand cmd)
        {
            Entity.IssueSplitFilter retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.IssueSplitFilter();
                        DynamicBuilder<Entity.IssueSplitFilter> builder = DynamicBuilder<Entity.IssueSplitFilter>.CreateBuilder(rdr);
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
       
    }
}
