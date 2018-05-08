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
    public class DataFieldSets
    {
        public static bool Exists(int datafieldSetID, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists(select top 1 DataFieldSetID from DatafieldSets where DatafieldSetID = @DatafieldSetID and GroupID = @GroupID) select 1 else select 0";
            cmd.Parameters.AddWithValue("@DatafieldSetID", datafieldSetID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.DataFieldSets GetByDataFieldsetID(int datafieldSetID, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM DataFieldSets WHERE DataFieldSetID = @DataFieldSetID and GroupID = @GroupID";
            cmd.Parameters.AddWithValue("@DataFieldSetID", datafieldSetID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.DataFieldSets> GetByGroupID(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT distinct dfs.* FROM DataFieldSets dfs WHERE dfs.GroupID = @GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.DataFieldSets GetByGroupIDName(int groupID, string name)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM DataFieldSets WHERE GroupID = @GroupID and name = @Name";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@Name", name);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.DataFieldSets Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.DataFieldSets retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.DataFieldSets();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.DataFieldSets>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.DataFieldSets> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.DataFieldSets> retList = new List<ECN_Framework_Entities.Communicator.DataFieldSets>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.DataFieldSets retItem = new ECN_Framework_Entities.Communicator.DataFieldSets();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.DataFieldSets>.CreateBuilder(rdr);
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

        public static int Insert(ECN_Framework_Entities.Communicator.DataFieldSets set)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO DatafieldSets (");
            sqlBuilder.Append("GroupID,MultivaluedYN,Name");
            sqlBuilder.Append(") VALUES (");
            sqlBuilder.Append("@GroupID,@MultivaluedYN,@Name");
            sqlBuilder.Append(") ;SELECT @@IDENTITY");
            SqlCommand cmd = new SqlCommand(sqlBuilder.ToString());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@GroupID", set.GroupID);
            cmd.Parameters.AddWithValue("@MultivaluedYN", set.MultivaluedYN);
            cmd.Parameters.AddWithValue("@Name", set.Name);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));
        }

        public static int Update(ECN_Framework_Entities.Communicator.DataFieldSets set)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UDPATE DatafieldSets SET ");
            sqlBuilder.Append("GroupID=@GroupID,MultivaluedYN=@MultivaluedYN,Name=@Name ");
            sqlBuilder.Append("WHERE DatafieldSetID = @DatafieldSetID ;SELECT @DatafieldSetID");
            SqlCommand cmd = new SqlCommand(sqlBuilder.ToString());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@DatafieldSetID", set.DataFieldSetID);
            cmd.Parameters.AddWithValue("@GroupID", set.GroupID);
            cmd.Parameters.AddWithValue("@MultivaluedYN", set.MultivaluedYN);
            cmd.Parameters.AddWithValue("@Name", set.Name);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));
        }
    }
}
