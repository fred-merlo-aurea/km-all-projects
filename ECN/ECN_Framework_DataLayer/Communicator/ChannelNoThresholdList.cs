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
    public class ChannelNoThresholdList
    {
        public static void Delete(int baseChannelID, string emailAddress, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelNoThresholdList_Delete";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
        
        public static void DeleteByCNTID(int baseChannelID, int CNTID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelNoThresholdList_DeleteByCNTID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@CNTID", CNTID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
        private static List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> retList = new List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.ChannelNoThresholdList retItem = new ECN_Framework_Entities.Communicator.ChannelNoThresholdList();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.ChannelNoThresholdList>.CreateBuilder(rdr);
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

        public static List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> GetByBaseChannelID(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelNoThresholdList_Select_BaseChannelID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> GetByEmailAddress(int baseChannelID, string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelNoThresholdList_Select_EmailAddress";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> GetByEmailAddress_Paging(int baseChannelID, string emailAddress, int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ChannelNoThresholdList_Select_EmailAddress_Paging";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
            return GetList(cmd);
        }
    }


}
