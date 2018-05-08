using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareRecordPriceRange
    {
        public static List<Entity.DataCompareRecordPriceRange> SelectAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareRecordPriceRange_Select";
            return GetList(cmd);
        }
        public static Entity.DataCompareRecordPriceRange Get(SqlCommand cmd)
        {
            Entity.DataCompareRecordPriceRange retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareRecordPriceRange();
                        DynamicBuilder<Entity.DataCompareRecordPriceRange> builder = DynamicBuilder<Entity.DataCompareRecordPriceRange>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareRecordPriceRange> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareRecordPriceRange> retList = new List<Entity.DataCompareRecordPriceRange>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareRecordPriceRange retItem = new Entity.DataCompareRecordPriceRange();
                        DynamicBuilder<Entity.DataCompareRecordPriceRange> builder = DynamicBuilder<Entity.DataCompareRecordPriceRange>.CreateBuilder(rdr);
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
        public static int Save(Entity.DataCompareRecordPriceRange x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareRecordPriceRange_Save";
            cmd.Parameters.AddWithValue("@DcRecordPriceRangeId", x.DcRecordPriceRangeId);
            cmd.Parameters.AddWithValue("@MinCount", x.MinCount);
            cmd.Parameters.AddWithValue("@MaxCount", x.MaxCount);
            cmd.Parameters.AddWithValue("@MatchMergePurgeCost", x.MatchMergePurgeCost);
            cmd.Parameters.AddWithValue("@MatchPricePerRecord", x.MatchPricePerRecord);
            cmd.Parameters.AddWithValue("@LikeMergePurgeCost", x.LikeMergePurgeCost);
            cmd.Parameters.AddWithValue("@LikePricePerRecord", x.LikePricePerRecord);
            cmd.Parameters.AddWithValue("@IsMergePurgePerRecordPricing", x.IsMergePurgePerRecordPricing);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@CreatedByUserId", x.CreatedByUserId);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object) x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DateUpdated", (object) x.DateUpdated ?? DBNull.Value);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
