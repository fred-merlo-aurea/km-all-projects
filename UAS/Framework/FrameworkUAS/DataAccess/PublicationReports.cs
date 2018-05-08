using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    [Serializable]
    class PublicationReports
    {
        public static List<Entity.PublicationReports> SelectPublication(int publicationID)
        {
            List<Entity.PublicationReports> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_PublicationReport_Select_PublicationID";
            cmd.Parameters.AddWithValue("@PublicationID", publicationID);
            retItem = GetList(cmd);
            return retItem;
        }

        private static List<Entity.PublicationReports> GetList(SqlCommand cmd)
        {
            List<Entity.PublicationReports> retList = new List<Entity.PublicationReports>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.PublicationReports retItem = new Entity.PublicationReports();
                        DynamicBuilder<Entity.PublicationReports> builder = DynamicBuilder<Entity.PublicationReports>.CreateBuilder(rdr);
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
