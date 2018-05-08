using System;
//using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common.Data;

//using System.Text;
//using System.Threading.Tasks;

namespace KMPlatform.DataAccess
{
    public class ECN
    {
        public static int getCustomerIDbyClientID(int clientID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "select customerID from Customer  with (NOLOCK) where PlatformClientID = @PlatformClientID  and IsDeleted=0";
                cmd.Parameters.AddWithValue("@PlatformClientID", clientID);

                return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.Accounts.ToString()));
            }
            catch
            {
                return 0;
            }

        }

        public static int getClientIDbyCustomerID(int customerID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "select PlatformClientID from Customer with (NOLOCK) where CustomerID = @CustomerID  and IsDeleted=0";
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.Accounts.ToString()));
            }
            catch
            {
                return 0;
            }

        }

        public static int getClientGroupIDbyBaseChannelID(int basechannelID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "select PlatformClientgroupID from Basechannel  with (NOLOCK) where BasechannelID = @basechannelID  and IsDeleted=0";
                cmd.Parameters.AddWithValue("@basechannelID", basechannelID);

                return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.Accounts.ToString()));
            }
            catch
            {
                return 0;
            }

        }

        
    }
}
