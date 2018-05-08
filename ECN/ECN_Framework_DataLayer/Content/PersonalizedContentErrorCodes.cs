using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Content
{
    public class PersonalizedContentErrorCodes
    {
        public static List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM ErrorCodes with(nolock)";

            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes> retList = new List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Content.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Content.PersonalizedContentErrorCodes retItem = new ECN_Framework_Entities.Content.PersonalizedContentErrorCodes();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes>.CreateBuilder(rdr);
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
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
    }
}
