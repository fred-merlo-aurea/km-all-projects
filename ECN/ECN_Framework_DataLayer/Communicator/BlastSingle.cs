using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastSingle
    {
        private const string ProcedureBlastSingleDeleteEmailId = "e_BlastSingle_Delete_EmailID";

        private const string ProcedureBlastSingleDeleteNoOpenFromAbandonEmailId =
            "e_BlastSingle_Delete_NoOpenFromAbandon_EmailID";

        private const string ProcedureBlastSingleExistsByBlastEmailLayoutPlan =
            "e_BlastSingle_Exists_ByBlastEmailLayoutPlan";

        public static bool ExistsByBlastID(int blastID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_Exists_ByBlastID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ExistsByBlastSingleID(int blastSingleID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_Exists_ByBlastID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastSingleID", blastSingleID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool ExistsByBlastEmailLayoutPlan(int blastID, int emailID, int layoutPlanID, int customerID)
        {
            return CommunicatorMethodsHelper.ExecuteScalarBlastSingle(
                       blastID, emailID, layoutPlanID, customerID, ProcedureBlastSingleExistsByBlastEmailLayoutPlan) > 0;
        }

        public static int GetRefBlastID(int blastID, int emailID, int customerID, string BlastType)
        {
            int refBlastID = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_GetRefBlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastType", BlastType);
            try
            {
                refBlastID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
            }
            catch (Exception)   {}
            return refBlastID;
        }

        public static DateTime GetCancelDate_ByLayoutID(int LayoutPlanID)
        {
            DateTime cDate = DateTime.MinValue;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_GetCancelDate_LayoutPlanID";
            cmd.Parameters.AddWithValue("@LayoutPlanID", LayoutPlanID);
            try
            {
                if(cDate!=null)
                    cDate = Convert.ToDateTime(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
            }
            catch (Exception) { }
            return cDate;
        }
        public static DataTable DownloadEmailLayoutPlanID_Processed(int LayoutPlanID,string Processed)
        {
           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_GetEmails_LayoutPlanID";
            cmd.Parameters.AddWithValue("@LayoutPlanID", LayoutPlanID);
            cmd.Parameters.AddWithValue("@Processed", Processed);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
        public static DateTime GetCancelDate_ByTriggerPlan(int TriggerPlanID, int blastID)
        {
            DateTime cDate = DateTime.MinValue;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_GetCancelDate_TriggerPlan";
            cmd.Parameters.AddWithValue("@TriggerPlanID", TriggerPlanID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            try
            {
                if (cDate != null)
                    cDate = Convert.ToDateTime(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
            }
            catch (Exception) { }
            return cDate;
        }
        public static int GetRefBlastID_ByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_GetRefBlastID_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
        public static int Insert(ECN_Framework_Entities.Communicator.BlastSingle blastSingle)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_Insert";
            cmd.Parameters.Add(new SqlParameter("@BlastID", (object)blastSingle.BlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EmailID", (object)blastSingle.EmailID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SendTime", (object)blastSingle.SendTime ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LayoutPlanID", (object)blastSingle.LayoutPlanID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RefBlastID", (object)blastSingle.RefBlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UserID", (object)blastSingle.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void DeleteForLayoutPlan(int layoutPlanID,int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_Delete_LayoutPlanID";
            cmd.Parameters.AddWithValue("@LayoutPlanID", layoutPlanID);
            cmd.Parameters.AddWithValue("@UpdatedUserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
        public static void DeleteByEmailID(int EmailID, int LayoutPlanID, int UserID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryBlastSingle(
                EmailID, LayoutPlanID, UserID, ProcedureBlastSingleDeleteEmailId);
        }

        public static void DeleteNoOpenFromAbandon_EmailID(int EmailID, int LayoutPlanID, int UserID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryBlastSingle(
                EmailID, LayoutPlanID, UserID, ProcedureBlastSingleDeleteNoOpenFromAbandonEmailId);
        }
        public static void Pause_UnPause_ForLayoutPlan(int layoutPlanID, bool isPause)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_Pause_LayoutPlanID";
            cmd.Parameters.AddWithValue("@LayoutPlanID", layoutPlanID);
            cmd.Parameters.AddWithValue("@IsPause", isPause);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Pause_UnPause_ForTriggerPlan(int triggerPlanID, bool isPause)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_Pause_TriggerPlanID";
            cmd.Parameters.AddWithValue("@TriggerPlanID", triggerPlanID);
            cmd.Parameters.AddWithValue("@IsPause", isPause);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void DeleteForTriggerPlan(int triggerPlanID, int blastID,int UpdatedUserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastSingle_Delete_TriggerPlan";
            cmd.Parameters.AddWithValue("@TriggerPlanID", triggerPlanID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@UpdatedUserID", UpdatedUserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
