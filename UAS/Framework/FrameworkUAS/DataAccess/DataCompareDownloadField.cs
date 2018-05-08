using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareDownloadField
    {
        public static int Save(Entity.DataCompareDownloadField x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownloadField_Save";
            cmd.Parameters.Add(new SqlParameter("@DcDownloadId", x.DcDownloadId));
            cmd.Parameters.Add(new SqlParameter("@DcDownloadFieldCodeId", x.DcDownloadFieldCodeId));
            cmd.Parameters.Add(new SqlParameter("@ColumnID", x.ColumnID));
            cmd.Parameters.Add(new SqlParameter("@ColumnName", x.ColumnName));
            cmd.Parameters.Add(new SqlParameter("@IsDescription", x.IsDescription));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
