using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareDownloadDetail
    {
        public static int Save(int dcDownloadID, string subscriptionIdsXml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownloadDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@DcDownloadId", dcDownloadID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionIds", subscriptionIdsXml));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
