using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class GroupDataFieldsDefault
    {

        public static ECN_Framework_Entities.Communicator.GroupDataFieldsDefault GetByGDFID(int GDFID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFieldsDefault_GetByUDFID";
            cmd.Parameters.AddWithValue("@GDFID", GDFID);

            return Get(cmd);
        }

        public static void Delete(int GDFID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFieldsDefault_Delete";
            cmd.Parameters.AddWithValue("@GDFID", GDFID);

            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Save(ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupDataFieldsDefault_Save";
            cmd.Parameters.AddWithValue("@GDFID", gdfd.GDFID);
            if (!string.IsNullOrEmpty(gdfd.DataValue))
                cmd.Parameters.AddWithValue("@DataValue", gdfd.DataValue);
            else
                cmd.Parameters.AddWithValue("@SystemValue", gdfd.SystemValue);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.GroupDataFieldsDefault> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFieldsDefault> retList = new List<ECN_Framework_Entities.Communicator.GroupDataFieldsDefault>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.GroupDataFieldsDefault retItem = new ECN_Framework_Entities.Communicator.GroupDataFieldsDefault();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.GroupDataFieldsDefault>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.GroupDataFieldsDefault Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.GroupDataFieldsDefault retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.GroupDataFieldsDefault();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.GroupDataFieldsDefault>.CreateBuilder(rdr);
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
    }
}
