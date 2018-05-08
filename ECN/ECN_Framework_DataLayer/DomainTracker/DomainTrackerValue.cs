using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.DomainTracker
{
    [Serializable]
    [DataContract]
    public class DomainTrackerValue
    {
        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTrackerValue domainTrackerValue, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerValue_Save";
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerActivityID", domainTrackerValue.DomainTrackerActivityID));
            cmd.Parameters.Add(new SqlParameter("@DomainTrackerFieldsID", domainTrackerValue.DomainTrackerFieldsID));
            cmd.Parameters.Add(new SqlParameter("@Value", domainTrackerValue.Value));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()));
        }

        public static DataTable GetByProfileID(int profileID, int domainTrackerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerValue_Select_ProfileID";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            cmd.Parameters.AddWithValue("@ProfileID", profileID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }
    }
}
