using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastFieldsName
    {
        public static ECN_Framework_Entities.Communicator.BlastFieldsName GetByBlastFieldID(int BlastFieldID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastFieldsName_Select";
            cmd.Parameters.AddWithValue("@BlastFieldID", BlastFieldID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.BlastFieldsName> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.BlastFieldsName> retList = new List<ECN_Framework_Entities.Communicator.BlastFieldsName>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.BlastFieldsName retItem = new ECN_Framework_Entities.Communicator.BlastFieldsName();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastFieldsName>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.BlastFieldsName Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastFieldsName retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.BlastFieldsName();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastFieldsName>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }
        
        public static int Save(ECN_Framework_Entities.Communicator.BlastFieldsName blastFieldsName, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastFieldsName_Save";
            cmd.Parameters.Add(new SqlParameter("@BlastFieldID", blastFieldsName.BlastFieldID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", blastFieldsName.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@Name", blastFieldsName.Name));
            if(blastFieldsName.BlastFieldsNameID>0)
                cmd.Parameters.Add(new SqlParameter("@UserID", blastFieldsName.UpdatedUserID));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", blastFieldsName.CreatedUserID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
