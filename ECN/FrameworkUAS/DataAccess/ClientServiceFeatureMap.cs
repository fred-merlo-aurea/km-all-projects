using System;
//using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common.Data;

//using System.Text;
//using System.Threading.Tasks;

namespace KMPlatform.DataAccess
{
    public class ClientServiceFeatureMap
    {
        public static int Save(Entity.ClientServiceFeatureMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientServiceFeatureMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientServiceFeatureMapID", x.ClientServiceFeatureMapID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@ServiceFeatureID", x.ServiceFeatureID));
            cmd.Parameters.Add(new SqlParameter("@IsEnabled", x.IsEnabled));
            cmd.Parameters.Add(new SqlParameter("@Rate", x.Rate));
            cmd.Parameters.Add(new SqlParameter("@RateDurationInMonths", x.RateDurationInMonths));
            cmd.Parameters.Add(new SqlParameter("@RateStartDate", (object)x.RateStartDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RateExpireDate", (object)x.RateExpireDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}
