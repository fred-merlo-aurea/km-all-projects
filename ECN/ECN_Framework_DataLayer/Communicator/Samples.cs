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
    public class Sample
    {
        public static bool Exists(int sampleID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 SampleID from [Sample] with (nolock) where CustomerID = @CustomerID and SampleID = @SampleID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.Sample GetBySampleID(int sampleID)
        {
            ECN_Framework_Entities.Communicator.Sample retItem = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [Sample] with (nolock) WHERE SampleID = @SampleID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Sample();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Sample>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.Sample GetByWinningBlastID(int blastID)
        {
            ECN_Framework_Entities.Communicator.Sample retItem = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [Sample] with (nolock) WHERE WinningBlastID = @WinningBlastID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@WinningBlastID", blastID);
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Sample();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Sample>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Communicator.Sample sample)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Sample_Save";
            cmd.Parameters.Add(new SqlParameter("@SampleName", sample.SampleName));
            cmd.Parameters.Add(new SqlParameter("@WinningBlastID", (object)sample.WinningBlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SampleID", sample.SampleID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", sample.CustomerID.Value));
            if (sample.SampleID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)sample.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)sample.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@DidNotReceiveAB", sample.DidNotReceiveAB);
            cmd.Parameters.AddWithValue("@DeliveredOrOpened", sample.DeliveredOrOpened);
            cmd.Parameters.Add(new SqlParameter("@ABWinnerType", (object)sample.ABWinnerType));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int sampleID, int customerID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update [Sample] SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE SampleID = @SampleID AND CustomerID = @CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SampleID", sampleID);
            cmd.Parameters.AddWithValue("@UpdatedUserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetAvailableSamples(int customerID, int CampaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Sample_GetAvailable";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
