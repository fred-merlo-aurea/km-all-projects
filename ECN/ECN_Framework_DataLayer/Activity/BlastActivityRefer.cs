using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity
{
    [Serializable]
    public class BlastActivityRefer
    {
        public static ECN_Framework_Entities.Activity.BlastActivityRefer GetByReferID(int referID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityRefer with (nolock) where ReferID = @ReferID";
            cmd.Parameters.AddWithValue("@ReferID", referID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityRefer> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityRefer with (nolock) where BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityRefer> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityRefer with (nolock) where EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityRefer> GetByBlastIDEmailID(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastActivityRefer with (nolock) where BlastID = @BlastID and EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Activity.BlastActivityRefer Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Activity.BlastActivityRefer retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Activity.BlastActivityRefer();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityRefer>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Activity.BlastActivityRefer> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityRefer> retList = new List<ECN_Framework_Entities.Activity.BlastActivityRefer>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Activity.BlastActivityRefer retItem = new ECN_Framework_Entities.Activity.BlastActivityRefer();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivityRefer>.CreateBuilder(rdr);
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
