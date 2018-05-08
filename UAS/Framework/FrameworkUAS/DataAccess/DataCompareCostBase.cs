using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareCostBase
    {
        public static List<Entity.DataCompareCostBase> Select()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareCost_Select";
            return GetList(cmd);
        }
        public static Entity.DataCompareCostBase Get(SqlCommand cmd)
        {
            Entity.DataCompareCostBase retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareCostBase();
                        DynamicBuilder<Entity.DataCompareCostBase> builder = DynamicBuilder<Entity.DataCompareCostBase>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareCostBase> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareCostBase> retList = new List<Entity.DataCompareCostBase>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareCostBase retItem = new Entity.DataCompareCostBase();
                        DynamicBuilder<Entity.DataCompareCostBase> builder = DynamicBuilder<Entity.DataCompareCostBase>.CreateBuilder(rdr);
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
