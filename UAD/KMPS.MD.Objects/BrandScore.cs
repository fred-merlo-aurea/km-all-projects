using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;

namespace KMPS.MD.Objects
{
    public class BrandScore
    {
        #region CRUD
        public static void UpdateBrandScrore(KMPlatform.Object.ClientConnections clientconnection, int BrandID)
        {
            SqlCommand cmd = new SqlCommand("spUpdateBrandScore");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
