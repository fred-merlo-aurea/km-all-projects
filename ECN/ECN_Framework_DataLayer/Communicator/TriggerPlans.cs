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
    public class TriggerPlans
    {
        public static bool Exists(int triggerPlanID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TriggerPlans_Exist";
            cmd.Parameters.AddWithValue("@TriggerPlanID", triggerPlanID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            int exists = -1;
            int.TryParse(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString(), out exists);

            if (exists > 0)
                return true;
            else
                return false;
        }
        public static ECN_Framework_Entities.Communicator.TriggerPlans GetByTriggerPlanID(int triggerPlanID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TriggerPlans_Select_TriggerPlanID";
            cmd.Parameters.AddWithValue("@TriggerPlanID", triggerPlanID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.TriggerPlans> GetNoOpenByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TriggerPlans_Select_GetNoOpenByBlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.TriggerPlans GetByRefTriggerID(int refTriggerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TriggerPlans_Select_RefTriggerID";
            cmd.Parameters.AddWithValue("@RefTriggerID", refTriggerID);
            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.TriggerPlans> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.TriggerPlans> retList = new List<ECN_Framework_Entities.Communicator.TriggerPlans>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.TriggerPlans retItem = new ECN_Framework_Entities.Communicator.TriggerPlans();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.TriggerPlans>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.TriggerPlans Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.TriggerPlans retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.TriggerPlans();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.TriggerPlans>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Communicator.TriggerPlans triggerPlan)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TriggerPlans_Save";
            cmd.Parameters.Add(new SqlParameter("@TriggerPlanID", triggerPlan.TriggerPlanID));
            cmd.Parameters.Add(new SqlParameter("@RefTriggerID", (object)triggerPlan.refTriggerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EventType", triggerPlan.EventType));
            cmd.Parameters.Add(new SqlParameter("@BlastID", (object)triggerPlan.BlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Period", (object)triggerPlan.Period ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Criteria", triggerPlan.Criteria));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)triggerPlan.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ActionName", triggerPlan.ActionName));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)triggerPlan.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Status", triggerPlan.Status));
            if (triggerPlan.TriggerPlanID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)triggerPlan.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)triggerPlan.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int triggerPlanID, int customerID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TriggerPlans_Delete";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@TriggerPlanID", triggerPlanID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
