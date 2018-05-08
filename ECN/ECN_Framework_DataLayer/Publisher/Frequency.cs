using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Publisher
{
    [Serializable]
    public class Frequency
    {
        public static List<ECN_Framework_Entities.Publisher.Frequency> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Frequency_Select_All";
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Publisher.Frequency> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Publisher.Frequency> retList = new List<ECN_Framework_Entities.Publisher.Frequency>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Publisher.Frequency retItem = new ECN_Framework_Entities.Publisher.Frequency();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Frequency>.CreateBuilder(rdr);
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
