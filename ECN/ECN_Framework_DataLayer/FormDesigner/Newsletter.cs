using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.FormDesigner
{
    [Serializable]
    public class Newsletter
    {
        public static bool ActiveByGDF(int groupDataFieldsID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Newsletter_ActiveByGDF";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", groupDataFieldsID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.FormDesigner.ToString())) > 0 ? true : false;
        }
    }
}
