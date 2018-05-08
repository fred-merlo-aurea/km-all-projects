using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataComparePricingView
    {

        public static List<Entity.DataComparePricingView> SelectForClient(int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataComparePricingView_Select_ClientId";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            return GetList(cmd);
        }

        public static List<Entity.DataComparePricingView> GetList(SqlCommand cmd)
        {
            List<Entity.DataComparePricingView> retList = new List<Entity.DataComparePricingView>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataComparePricingView retItem = new Entity.DataComparePricingView();
                        DynamicBuilder<Entity.DataComparePricingView> builder = DynamicBuilder<Entity.DataComparePricingView>.CreateBuilder(rdr);
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
