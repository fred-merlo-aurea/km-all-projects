using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class QuoteOption
    {
        public static List<ECN_Framework_Entities.Accounts.QuoteOption> GetByLicenseType(int basechannelID, ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum licenseType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuoteOption_Select_BaseChannelID_LicenseType";
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", basechannelID));
            cmd.Parameters.Add(new SqlParameter("@LicenseType", licenseType));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.QuoteOption> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.QuoteOption> retList = new List<ECN_Framework_Entities.Accounts.QuoteOption>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.QuoteOption retItem = new ECN_Framework_Entities.Accounts.QuoteOption();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.QuoteOption>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        retList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
    }
}
