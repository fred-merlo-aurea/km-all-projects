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
    public class DataCompareDownloadView
    {
        public static List<Entity.DataCompareDownloadView> SelectForClient(int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownloadView_Select_ClientId";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            return GetList(cmd);
        }

        public static List<Entity.DataCompareDownloadView> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareDownloadView> retList = new List<Entity.DataCompareDownloadView>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareDownloadView retItem = new Entity.DataCompareDownloadView();
                        DynamicBuilder<Entity.DataCompareDownloadView> builder = DynamicBuilder<Entity.DataCompareDownloadView>.CreateBuilder(rdr);
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

