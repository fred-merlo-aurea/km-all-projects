using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Communicator
{
    public class PersonalizationContentErrorCodes
    {
        public static List<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM ErrorCodes with(nolock)";
            
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes> retList = new List<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Content.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes retItem = new ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes();
                    DynamicBuilder<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes> builder = DynamicBuilder<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes>.CreateBuilder(rdr);
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
