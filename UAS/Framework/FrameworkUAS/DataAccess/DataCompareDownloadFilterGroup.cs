using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;


namespace FrameworkUAS.DataAccess
{
    public class DataCompareDownloadFilterGroup
    {
        public static List<Entity.DataCompareDownloadFilterGroup> SelectForDownload(int dcDownloadId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownloadFilterGroup_Select_DcDownloadId";
            cmd.Parameters.AddWithValue("@dcDownloadId", dcDownloadId);
            return GetList(cmd);
        }

        public static List<Entity.DataCompareDownloadFilterGroup> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareDownloadFilterGroup> retList = new List<Entity.DataCompareDownloadFilterGroup>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareDownloadFilterGroup retItem = new Entity.DataCompareDownloadFilterGroup();
                        DynamicBuilder<Entity.DataCompareDownloadFilterGroup> builder = DynamicBuilder<Entity.DataCompareDownloadFilterGroup>.CreateBuilder(rdr);
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

        public static int Save(Entity.DataCompareDownloadFilterGroup x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownloadFilterGroup_Save";
            cmd.Parameters.Add(new SqlParameter("@DcDownloadId", x.DcDownloadId));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
