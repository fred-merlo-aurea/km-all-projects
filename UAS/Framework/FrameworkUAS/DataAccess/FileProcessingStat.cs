using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class FileProcessingStat
    {
        public static bool Save(Entity.FileProcessingStat fps)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileProcessingStat_Save";
            cmd.Parameters.AddWithValue("@ClientId", fps.ClientId);
            cmd.Parameters.AddWithValue("@FileCount", fps.FileCount);
            cmd.Parameters.AddWithValue("@ProfileCount", fps.ProfileCount);
            cmd.Parameters.AddWithValue("@DemographicCount", fps.DemographicCount);
            cmd.Parameters.AddWithValue("@ProcessDate", fps.ProcessDate);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static Entity.FileProcessingStat Select(DateTime processDate)
        {
            Entity.FileProcessingStat retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileProcessingStat_Select_ProcessDate";
            cmd.Parameters.AddWithValue("@ProcessDate", processDate);
            retItem = Get(cmd);
            return retItem;
        }
        public static List<Entity.FileProcessingStat> SelectDateRange(DateTime startDate, DateTime endDate)
        {
            List<Entity.FileProcessingStat> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileProcessingStat_Select_DateRange";
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            retItem = GetList(cmd);
            return retItem;
        }

        public static FrameworkUAS.Entity.FileProcessingStat Get(SqlCommand cmd)
        {
            FrameworkUAS.Entity.FileProcessingStat retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new FrameworkUAS.Entity.FileProcessingStat();
                        DynamicBuilder<FrameworkUAS.Entity.FileProcessingStat> builder = DynamicBuilder<FrameworkUAS.Entity.FileProcessingStat>.CreateBuilder(rdr);
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
        public static List<FrameworkUAS.Entity.FileProcessingStat> GetList(SqlCommand cmd)
        {
            List<FrameworkUAS.Entity.FileProcessingStat> retList = new List<FrameworkUAS.Entity.FileProcessingStat>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        FrameworkUAS.Entity.FileProcessingStat retItem = new FrameworkUAS.Entity.FileProcessingStat();
                        DynamicBuilder<FrameworkUAS.Entity.FileProcessingStat> builder = DynamicBuilder<FrameworkUAS.Entity.FileProcessingStat>.CreateBuilder(rdr);
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
    }
}
