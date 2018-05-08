using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareDownloadFilterDetail
    {
        public static List<Entity.DataCompareDownloadFilterDetail> SelectForFilterGroup(int dcFilterGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownloadFilterDetail_Select_DcFilterGroupID";
            cmd.Parameters.AddWithValue("@dcFilterGroupID", dcFilterGroupID);
            return GetList(cmd);
        }

        public static List<Entity.DataCompareDownloadFilterDetail> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareDownloadFilterDetail> retList = new List<Entity.DataCompareDownloadFilterDetail>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareDownloadFilterDetail retItem = new Entity.DataCompareDownloadFilterDetail();
                        DynamicBuilder<Entity.DataCompareDownloadFilterDetail> builder = DynamicBuilder<Entity.DataCompareDownloadFilterDetail>.CreateBuilder(rdr);
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

        public static int Save(Entity.DataCompareDownloadFilterDetail x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownloadFilterDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@DcFilterGroupId", x.DcFilterGroupId));
            cmd.Parameters.Add(new SqlParameter("@FilterType", x.FilterType));
            cmd.Parameters.Add(new SqlParameter("@Group", x.Group));
            cmd.Parameters.Add(new SqlParameter("@Name", x.Name));
            cmd.Parameters.Add(new SqlParameter("@Values", x.Values));
            cmd.Parameters.Add(new SqlParameter("@SearchCondition", x.SearchCondition));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
