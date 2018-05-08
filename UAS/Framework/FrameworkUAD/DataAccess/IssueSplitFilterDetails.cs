using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameworkUAD.Entity;
using KMPlatform.Object;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class IssueSplitFilterDetails
    {
        internal static List<Entity.IssueSplitFilterDetails>  SelectFilterID(int filterID, ClientConnections client)
        {
            List<Entity.IssueSplitFilterDetails> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplitFilterDetails_Select_FilterID";
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        private static List<Entity.IssueSplitFilterDetails> GetList(SqlCommand cmd)
        {
            List<Entity.IssueSplitFilterDetails> retList = new List<Entity.IssueSplitFilterDetails>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.IssueSplitFilterDetails retItem = new Entity.IssueSplitFilterDetails();
                        DynamicBuilder<Entity.IssueSplitFilterDetails> builder = DynamicBuilder<Entity.IssueSplitFilterDetails>.CreateBuilder(rdr);
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
