using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class AcsShippingDetail
    {
        public static Entity.AcsShippingDetail Get(SqlCommand cmd)
        {
            Entity.AcsShippingDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.AcsShippingDetail();
                        DynamicBuilder<Entity.AcsShippingDetail> builder = DynamicBuilder<Entity.AcsShippingDetail>.CreateBuilder(rdr);
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
        public static List<Entity.AcsShippingDetail> GetList(SqlCommand cmd)
        {
            List<Entity.AcsShippingDetail> retList = new List<Entity.AcsShippingDetail>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.AcsShippingDetail retItem = new Entity.AcsShippingDetail();
                        DynamicBuilder<Entity.AcsShippingDetail> builder = DynamicBuilder<Entity.AcsShippingDetail>.CreateBuilder(rdr);
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

        public static int Save(Entity.AcsShippingDetail x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AcsShippingDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@AcsShippingDetailId", x.AcsShippingDetailId));
            cmd.Parameters.Add(new SqlParameter("@CustomerNumber", x.CustomerNumber));
            cmd.Parameters.Add(new SqlParameter("@AcsDate", x.AcsDate));
            cmd.Parameters.Add(new SqlParameter("@ShipmentNumber", x.ShipmentNumber));
            cmd.Parameters.Add(new SqlParameter("@AcsTypeId", x.AcsTypeId));
            cmd.Parameters.Add(new SqlParameter("@AcsId", x.AcsId));
            cmd.Parameters.Add(new SqlParameter("@AcsName", x.AcsName));
            cmd.Parameters.Add(new SqlParameter("@ProductCode", x.ProductCode));
            cmd.Parameters.Add(new SqlParameter("@Description", x.Description));
            cmd.Parameters.Add(new SqlParameter("@Quantity", x.Quantity));
            cmd.Parameters.Add(new SqlParameter("@UnitCost", x.UnitCost));
            cmd.Parameters.Add(new SqlParameter("@TotalCost", x.TotalCost));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@IsBilled", x.IsBilled));
            cmd.Parameters.Add(new SqlParameter("@BilledDate", (object)x.BilledDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BilledByUserID", (object)x.BilledByUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@ProcessCode", x.ProcessCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);


            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
