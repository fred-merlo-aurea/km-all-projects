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
    public class ConversionLinks
    {
        private const string ProcedureConversionLinksDeleteSingle = "e_ConversionLinks_Delete_Single";
        private const string ProcedureConversionLinksDeleteAll = "e_ConversionLinks_Delete_All";
        private const string ProcedureConversionLinksExistsByName = "e_ConversionLinks_Exists_ByName";

        public static bool Exists(int layoutID, int linkID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "e_ConversionLinks_Exists_ByLinkID";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            cmd.Parameters.AddWithValue("@LinkID", linkID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int layoutID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConversionLinks_Exists_ByLayoutID";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int linkID, string linkName, int layoutID, int customerID)
        {
            return CommunicatorMethodsHelper.ExecuteScalarConversionLinks(
                       linkID, linkName, layoutID, customerID, ProcedureConversionLinksExistsByName) > 0;
        }

        public static ECN_Framework_Entities.Communicator.ConversionLinks GetByLinkID(int linkID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConversionLinks_Select_Single";
            cmd.Parameters.AddWithValue("@LinkID", linkID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ConversionLinks> GetByLayoutID(int layoutID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConversionLinks_Select_All";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ConversionLinks> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConversionLinks_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.ConversionLinks Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.ConversionLinks retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.ConversionLinks();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ConversionLinks>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.ConversionLinks> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.ConversionLinks> retList = new List<ECN_Framework_Entities.Communicator.ConversionLinks>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.ConversionLinks retItem = new ECN_Framework_Entities.Communicator.ConversionLinks();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ConversionLinks>.CreateBuilder(rdr);
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

        public static void Delete(int layoutID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryLinks(
                layoutID, null, customerID, userID, ProcedureConversionLinksDeleteAll);
        }

        public static void Delete(int layoutID, int linkID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryLinks(
                layoutID, linkID, customerID, userID, ProcedureConversionLinksDeleteSingle);
        }

        public static int Save(ECN_Framework_Entities.Communicator.ConversionLinks link)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ConversionLinks_Save";
            cmd.Parameters.Add(new SqlParameter("@LinkID", (object)link.LinkID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)link.LayoutID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", (object)link.SortOrder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LinkURL", link.LinkURL));
            cmd.Parameters.Add(new SqlParameter("@LinkParams", link.LinkParams));
            cmd.Parameters.Add(new SqlParameter("@LinkName", link.LinkName));
            cmd.Parameters.Add(new SqlParameter("@IsActive", link.IsActive));
            if (link.LinkID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)link.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)link.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
