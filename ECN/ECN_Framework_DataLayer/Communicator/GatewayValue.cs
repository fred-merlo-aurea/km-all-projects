using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class GatewayValue
    {
        public static List<ECN_Framework_Entities.Communicator.GatewayValue> GetByGatewayID(int GatewayID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_GatewayValue_Select_GatewayID";
            cmd.Parameters.AddWithValue("@GatewayID", GatewayID);

            return GetList(cmd);
        }

        public static void Delete(int GatewayValueID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_GatewayValue_Delete_ID";

            cmd.Parameters.AddWithValue("@GatewayValueID", GatewayValueID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void WipeOutValues(bool Login, int GatewayID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_GatewayValue_Delete_LoginOrCapture";
            cmd.Parameters.AddWithValue("@GatewayID", GatewayID);
            if(Login)
            {
                cmd.Parameters.AddWithValue("@Login", Login);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Capture", Login);
            }

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());

        }

        public static int Save(ECN_Framework_Entities.Communicator.GatewayValue gv)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_GatewayValue_Save";
            if (gv.GatewayValueID > 0)
            {
                cmd.Parameters.AddWithValue("@GatewayValueID", gv.GatewayValueID);
                cmd.Parameters.AddWithValue("@UpdatedUserID", gv.UpdatedUserID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreatedUserID", gv.CreatedUserID);
            }
            cmd.Parameters.AddWithValue("@GatewayID", gv.GatewayID);
            cmd.Parameters.AddWithValue("@IsDeleted", gv.IsDeleted);
            cmd.Parameters.AddWithValue("@IsCaptureValue", gv.IsCaptureValue);
            cmd.Parameters.AddWithValue("@IsLoginValidator", gv.IsLoginValidator);
            cmd.Parameters.AddWithValue("@IsStatic", gv.IsStatic);
            cmd.Parameters.AddWithValue("@Label", gv.Label);
            cmd.Parameters.AddWithValue("@Field", gv.Field);
            if (gv.IsLoginValidator)
            {
                cmd.Parameters.AddWithValue("@FieldType", gv.FieldType);
                cmd.Parameters.AddWithValue("@Comparator", gv.Comparator);
                cmd.Parameters.AddWithValue("@DatePart", gv.DatePart);
                cmd.Parameters.AddWithValue("@NOT", gv.NOT);
                
            }

            cmd.Parameters.AddWithValue("@Value", gv.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.GatewayValue> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.GatewayValue> retList = new List<ECN_Framework_Entities.Communicator.GatewayValue>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.GatewayValue retItem = new ECN_Framework_Entities.Communicator.GatewayValue();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.GatewayValue>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.GatewayValue Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.GatewayValue retItem = new ECN_Framework_Entities.Communicator.GatewayValue();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {

                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.GatewayValue>.CreateBuilder(rdr);
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
