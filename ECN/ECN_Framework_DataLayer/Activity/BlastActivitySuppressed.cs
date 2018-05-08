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
    public class BlastActivitySuppressed
    {
        public static ECN_Framework_Entities.Activity.BlastActivitySuppressed GetBySuppressID(int suppressID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, sc.SuppressedCode from BlastActivitySuppressed bas with (nolock) join SuppressedCodes sc with (nolock) on bas.SuppressedCodeID = sc.SuppressedCodeID where SuppressID = @SuppressID";
            cmd.Parameters.AddWithValue("@SuppressID", suppressID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, sc.SupressedCode from BlastActivitySuppressed bas with (nolock) join SuppressedCodes sc with (nolock) on bas.SuppressedCodeID = sc.SuppressedCodeID where BlastID = @BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> GetByEmailID(int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, sc.SuppressedCode from BlastActivitySuppressed bas with (nolock) join SuppressedCodes sc with (nolock) on bas.SuppressedCodeID = sc.SuppressedCodeID where EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> GetByBlastIDEmailID(int blastID, int emailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select bas.*, sc.SuppressedCode from BlastActivitySuppressed bas with (nolock) join SuppressedCodes sc with (nolock) on bas.SuppressedCodeID = sc.SuppressedCodeID where BlastID = @BlastID and EmailID = @EmailID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Activity.BlastActivitySuppressed Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Activity.BlastActivitySuppressed retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Activity.BlastActivitySuppressed();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivitySuppressed>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> retList = new List<ECN_Framework_Entities.Activity.BlastActivitySuppressed>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Activity.BlastActivitySuppressed retItem = new ECN_Framework_Entities.Activity.BlastActivitySuppressed();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.BlastActivitySuppressed>.CreateBuilder(rdr);
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
