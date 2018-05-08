using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareCostUser
    {
        public static List<Entity.DataCompareCostUser> Select(int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareCostToUser_Select_UserId";
            cmd.Parameters.AddWithValue("@UserId", userId);
            return GetList(cmd);
        }
        public static Entity.DataCompareCostUser Get(SqlCommand cmd)
        {
            Entity.DataCompareCostUser retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareCostUser();
                        DynamicBuilder<Entity.DataCompareCostUser> builder = DynamicBuilder<Entity.DataCompareCostUser>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareCostUser> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareCostUser> retList = new List<Entity.DataCompareCostUser>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareCostUser retItem = new Entity.DataCompareCostUser();
                        DynamicBuilder<Entity.DataCompareCostUser> builder = DynamicBuilder<Entity.DataCompareCostUser>.CreateBuilder(rdr);
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
