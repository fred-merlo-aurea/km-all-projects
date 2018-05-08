using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;


namespace ECN_Framework_DataLayer.Communicator
{
    public class UnicodeLookup
    {
        public static ECN_Framework_Entities.Communicator.UnicodeLookup GetByUnicodeNum(int num)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UnicodeLookup_Select_UnicodeNum";
            cmd.Parameters.AddWithValue("@UnicodeNumber", num.ToString());

            return Get(cmd);

        }

        public static List<ECN_Framework_Entities.Communicator.UnicodeLookup> GetAllActive()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UnicodeLookup_Select_Active";

            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.UnicodeLookup Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.UnicodeLookup retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.UnicodeLookup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.UnicodeLookup>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.UnicodeLookup GetByID(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UnicodeLookup_Select_Id";
            cmd.Parameters.AddWithValue("@Id", id.ToString());

            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.UnicodeLookup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.UnicodeLookup> retList = new List<ECN_Framework_Entities.Communicator.UnicodeLookup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.UnicodeLookup retItem = new ECN_Framework_Entities.Communicator.UnicodeLookup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.UnicodeLookup>.CreateBuilder(rdr);
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
