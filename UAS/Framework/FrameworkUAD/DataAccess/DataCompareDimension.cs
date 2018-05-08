using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class DataCompareDimension
    {
        public static Entity.DataCompareDimension Get(SqlCommand cmd)
        {
            Entity.DataCompareDimension retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareDimension();
                        DynamicBuilder<Entity.DataCompareDimension> builder = DynamicBuilder<Entity.DataCompareDimension>.CreateBuilder(rdr);
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
        public static List<Entity.DataCompareDimension> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareDimension> retList = new List<Entity.DataCompareDimension>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareDimension retItem = new Entity.DataCompareDimension();
                        DynamicBuilder<Entity.DataCompareDimension> builder = DynamicBuilder<Entity.DataCompareDimension>.CreateBuilder(rdr);
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
