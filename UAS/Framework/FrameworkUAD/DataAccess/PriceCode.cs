using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class PriceCode
    {
        public static List<Entity.PriceCode> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PriceCode_Select";
            return GetList(cmd);
        }
        public static List<Entity.PriceCode> Select(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PriceCode_Select_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            return GetList(cmd);
        }
        public static Entity.PriceCode Select(string priceCode, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PriceCode_Select_PriceCode_PublicationID";
            cmd.Parameters.AddWithValue("@PriceCode", priceCode);
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            return Get(cmd);
        }
        private static Entity.PriceCode Get(SqlCommand cmd)
        {
            Entity.PriceCode retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.PriceCode();
                        DynamicBuilder<Entity.PriceCode> builder = DynamicBuilder<Entity.PriceCode>.CreateBuilder(rdr);
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
        private static List<Entity.PriceCode> GetList(SqlCommand cmd)
        {
            List<Entity.PriceCode> retList = new List<Entity.PriceCode>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.PriceCode retItem = new Entity.PriceCode();
                        DynamicBuilder<Entity.PriceCode> builder = DynamicBuilder<Entity.PriceCode>.CreateBuilder(rdr);
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

        public static int Save(Entity.PriceCode x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_PriceCode_Save";
            cmd.Parameters.Add(new SqlParameter("@PriceCodeID", x.PriceCodeID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", x.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@PriceCode", x.PriceCodes));
            cmd.Parameters.Add(new SqlParameter("@Term", x.Term));
            cmd.Parameters.Add(new SqlParameter("@USCopyRate", x.US_CopyRate));
            cmd.Parameters.Add(new SqlParameter("@CANCopyRate", x.CAN_CopyRate));
            cmd.Parameters.Add(new SqlParameter("@FORCopyRate", x.FOR_CopyRate));
            cmd.Parameters.Add(new SqlParameter("@USPrice", x.US_Price));
            cmd.Parameters.Add(new SqlParameter("@CANPrice", x.CAN_Price));
            cmd.Parameters.Add(new SqlParameter("@FORPrice", x.FOR_Price));
            cmd.Parameters.Add(new SqlParameter("@QFOfferCode", x.QFOfferCode));
            cmd.Parameters.Add(new SqlParameter("@FoxProPriceCode", x.FoxProPriceCode));
            cmd.Parameters.Add(new SqlParameter("@Description", x.Description));
            cmd.Parameters.Add(new SqlParameter("@DeliverabilityID", x.DeliverabilityID));
            cmd.Parameters.Add(new SqlParameter("@TotalIssues", x.TotalIssues));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
