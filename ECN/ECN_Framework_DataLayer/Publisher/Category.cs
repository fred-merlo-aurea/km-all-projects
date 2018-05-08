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
    public class Category
    {
        public static List<ECN_Framework_Entities.Publisher.Category> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Category_Select_All";
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Publisher.Category> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Publisher.Category> retList = new List<ECN_Framework_Entities.Publisher.Category>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Publisher.Category retItem = new ECN_Framework_Entities.Publisher.Category();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Category>.CreateBuilder(rdr);
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
    