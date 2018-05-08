using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class LinkOwnerIndex
    {
        private const string ProcedureLinkOwnerIndexSelectSingle = "e_LinkOwnerIndex_Select_Single";
        private const string ProcedureLinkOwnerIndexSelectAll = "e_LinkOwnerIndex_Select_All";
        private const string ProcedureLinkOwnerIndexDeleteAll = "e_LinkOwnerIndex_Delete_All";

        public static bool Exists(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "e_LinkOwnerIndex_Exists_ByCustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int linkOwnerIndexID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkOwnerIndex_Exists_ByOwnerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LinkOwnerIndexID", linkOwnerIndexID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        private static List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> retList = new List<ECN_Framework_Entities.Communicator.LinkOwnerIndex>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.LinkOwnerIndex retItem = new ECN_Framework_Entities.Communicator.LinkOwnerIndex();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkOwnerIndex>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.LinkOwnerIndex Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LinkOwnerIndex retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.LinkOwnerIndex();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkOwnerIndex>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.LinkOwnerIndex GetByOwnerID(int linkOwnerIndexID)
        {
            var cmd = CommunicatorMethodsHelper.GetLinkOwner(linkOwnerIndexID, null, ProcedureLinkOwnerIndexSelectSingle);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkOwnerIndex> GetByCustomerID(int customerID)
        {
            var cmd = CommunicatorMethodsHelper.GetLinkOwner(null, customerID, ProcedureLinkOwnerIndexSelectAll);
            return GetList(cmd);
        }

        public static void Delete(int linkOwnerIndexID, int customerID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkOwnerIndex_Delete_Single";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LinkOwnerIndexID", linkOwnerIndexID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryLinks(
                null, null, customerID, userID, ProcedureLinkOwnerIndexDeleteAll);
        }

        public static int Save(ECN_Framework_Entities.Communicator.LinkOwnerIndex linkOwner)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkOwnerIndex_Save";
            cmd.Parameters.Add(new SqlParameter("@LinkOwnerIndexID", linkOwner.LinkOwnerIndexID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", linkOwner.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@LinkOwnerName", linkOwner.LinkOwnerName));
            cmd.Parameters.Add(new SqlParameter("@LinkOwnerCode", linkOwner.LinkOwnerCode));
            cmd.Parameters.Add(new SqlParameter("@ContactFirstName", linkOwner.ContactFirstName));
            cmd.Parameters.Add(new SqlParameter("@ContactLastName", linkOwner.ContactLastName));
            cmd.Parameters.Add(new SqlParameter("@ContactPhone", linkOwner.ContactPhone));
            cmd.Parameters.Add(new SqlParameter("@ContactEmail", linkOwner.ContactEmail));
            cmd.Parameters.Add(new SqlParameter("@Address", linkOwner.Address));
            cmd.Parameters.Add(new SqlParameter("@City", linkOwner.City));
            cmd.Parameters.Add(new SqlParameter("@State", linkOwner.State));
            cmd.Parameters.Add(new SqlParameter("@IsActive", (object)linkOwner.IsActive ?? DBNull.Value));
            if (linkOwner.LinkOwnerIndexID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)linkOwner.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)linkOwner.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
