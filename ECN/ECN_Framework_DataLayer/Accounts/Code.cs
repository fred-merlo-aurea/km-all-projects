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
    public class Code
    {
         public static bool Exists()
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "e_Code_Exists";
             return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
         }

        public static List<ECN_Framework_Entities.Accounts.Code> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select";
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.Code> GetByCodeType(ECN_Framework_Common.Objects.Accounts.Enums.CodeType codeType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_ByType";
            cmd.Parameters.AddWithValue("@CodeType", codeType.ToString());
            return GetList(cmd);
        }

         private static List<ECN_Framework_Entities.Accounts.Code> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.Code> retList = new List<ECN_Framework_Entities.Accounts.Code>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.Code retItem = new ECN_Framework_Entities.Accounts.Code();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Code>.CreateBuilder(rdr);
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
