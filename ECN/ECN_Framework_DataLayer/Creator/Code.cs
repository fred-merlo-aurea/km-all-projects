using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Creator
{
    [Serializable]
    public class Code
    {
        public static List<ECN_Framework_Entities.Creator.Code> GetByCodeType(string CodeType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_ByType";
            cmd.Parameters.AddWithValue("@CodeType", CodeType);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Creator.Code> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Creator.Code> retList = new List<ECN_Framework_Entities.Creator.Code>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Creator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Creator.Code retItem = new ECN_Framework_Entities.Creator.Code();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Creator.Code>.CreateBuilder(rdr);
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
