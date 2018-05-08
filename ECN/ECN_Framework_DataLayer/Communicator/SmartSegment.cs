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
    public class SmartSegment
    {
        private static ECN_Framework_Entities.Communicator.SmartSegment Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.SmartSegment retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.SmartSegment();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SmartSegment>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.SmartSegment> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.SmartSegment> retList = new List<ECN_Framework_Entities.Communicator.SmartSegment>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.SmartSegment retItem = new ECN_Framework_Entities.Communicator.SmartSegment();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SmartSegment>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.SmartSegment GetSmartSegmentByID(int smartSegmentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM SmartSegment WHERE SmartSegmentID = @SmartSegmentID";
            cmd.Parameters.AddWithValue("@SmartSegmentID", smartSegmentID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.SmartSegment> GetSmartSegments()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM SmartSegment";
            return GetList(cmd);
        }

        public static int GetNewIDFromOldID(int oldSmartSegmentID)
        {
            int newID = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT SmartSegmentID FROM SmartSegment WHERE SmartSegmentOldID = @SmartSegmentOldID";
                cmd.Parameters.AddWithValue("@SmartSegmentOldID", oldSmartSegmentID);
                newID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));
            }
            catch (Exception)
            {
            }

            return newID;
        }

        public static bool SmartSegmentExists(int smartSegmentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 SmartSegmentID from SmartSegment where SmartSegmentID = @SmartSegmentID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@SmartSegmentID", smartSegmentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool SmartSegmentOldExists(int smartSegmentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 SmartSegmentID from SmartSegment where SmartSegmentOldID = @SmartSegmentOldID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@SmartSegmentOldID", smartSegmentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
    }
}
