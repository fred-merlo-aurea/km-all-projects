using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity
{
    [Serializable]
    public class SuppressedCodes
    {
        public static bool Exists()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SuppressedCodes_Exists";
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Activity.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Activity.SuppressedCodes> GetAll()
        {
            ECN_Framework_Entities.Activity.SuppressedCodes suppressedCode = null;
            List<ECN_Framework_Entities.Activity.SuppressedCodes> suppressedCodeList = new List<ECN_Framework_Entities.Activity.SuppressedCodes>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from SuppressedCodes with (nolock)";

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.SuppressedCodes>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    suppressedCode = new ECN_Framework_Entities.Activity.SuppressedCodes();
                    suppressedCode = builder.Build(rdr);
                    suppressedCodeList.Add(suppressedCode);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return suppressedCodeList;
        }
    }
}
