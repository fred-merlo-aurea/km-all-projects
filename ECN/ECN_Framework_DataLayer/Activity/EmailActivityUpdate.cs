using System.Data;
using System.Data.SqlClient;

namespace ECN_Framework_DataLayer.Activity
{
    public class EmailActivityUpdate
    {
        public static void UpdateEmailActivity(
            string oldEmailAddress,
            string newEmailAddress,
            int groupID,
            int customerID,
            int formID,
            string comments)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_EmailActivityUpdate";
                cmd.Parameters.AddWithValue("@OldEmailAddress", oldEmailAddress);
                cmd.Parameters.AddWithValue("@NewEmailAddress", newEmailAddress);
                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@FormID", formID);
                cmd.Parameters.AddWithValue("@Comments", comments);
                DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Activity.ToString());
            }
        }
    }
}
