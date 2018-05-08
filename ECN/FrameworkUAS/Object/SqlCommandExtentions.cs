using System.Collections.Generic;
using System.Data.SqlClient;
using KM.Common.Data;
using KMPlatform.DataAccess;
using DataFunctions = KM.Common.DataFunctions;

namespace KMPlatform.Object
{
    public static class SqlCommandExtentions
    {
        public static List<T> GetList<T>(this SqlCommand cmd)
        {
            List<T> retList = new List<T>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        T retItem = default(T);
                        var builder = KM.Common.DynamicBuilder<T>.CreateBuilder(rdr);
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
