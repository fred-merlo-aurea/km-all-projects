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
    public class EmailDirect
    {
        public static int Save(ECN_Framework_Entities.Communicator.EmailDirect ed)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDirect_Save";
            if (ed.EmailDirectID > 0)
                cmd.Parameters.AddWithValue("@EmailDirectID", ed.EmailDirectID);
            cmd.Parameters.AddWithValue("@CustomerID", ed.CustomerID);
            cmd.Parameters.AddWithValue("@Source", ed.Source);
            cmd.Parameters.AddWithValue("@Process", ed.Process);
            cmd.Parameters.AddWithValue("@Status", ed.Status.ToString());
            cmd.Parameters.AddWithValue("@SendTime", ed.SendTime);
            cmd.Parameters.AddWithValue("@EmailAddress", ed.EmailAddress);
            cmd.Parameters.AddWithValue("@FromEmailAddress", ed.FromEmailAddress);
            cmd.Parameters.AddWithValue("@FromName", ed.FromName);
            cmd.Parameters.AddWithValue("@EmailSubject", ed.EmailSubject);
            cmd.Parameters.AddWithValue("@ReplyEmailAddress", ed.ReplyEmailAddress);
            cmd.Parameters.AddWithValue("@Content", ed.Content);
            if (ed.EmailDirectID > 0)
                cmd.Parameters.AddWithValue("@UpdatedUserID", ed.UpdatedUserID);
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", ed.CreatedUserID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static ECN_Framework_Entities.Communicator.EmailDirect GetNextToSend()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDirect_GetNextToSend";
            return Get(cmd);
        }

        public static void UpdateStatus(int EmailDirectID, ECN_Framework_Common.Objects.EmailDirect.Enums.Status Status)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDirect_Update_Status";
            cmd.Parameters.AddWithValue("@EmailDirectID", EmailDirectID);
            cmd.Parameters.AddWithValue("@Status", Status.ToString());

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static ECN_Framework_Entities.Communicator.EmailDirect GetByEmailDirectID(int EmailDirectID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDirect_Select_EmailDirectID";
            cmd.Parameters.AddWithValue("@EmailDirectID", EmailDirectID);

            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDirect> GetByCustomerID(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailDirect_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.EmailDirect Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.EmailDirect retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.EmailDirect();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailDirect>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.EmailDirect> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.EmailDirect> retList = new List<ECN_Framework_Entities.Communicator.EmailDirect>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.EmailDirect retItem = new ECN_Framework_Entities.Communicator.EmailDirect();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailDirect>.CreateBuilder(rdr);
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
