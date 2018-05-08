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
    public class BlastPlans
    {
        public static bool Exists(int contentID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastPlans_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastPlanID", contentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastPlans> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastPlans_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.BlastPlans> GetByBlastID(int blastID, string eventType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastPlans_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@EventType", eventType);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.BlastPlans GetByBlastPlanID(int blastPlanID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastPlans_Select_BlastPlanID";
            cmd.Parameters.AddWithValue("@BlastPlanID", blastPlanID);
            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.BlastPlans> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.BlastPlans> retList = new List<ECN_Framework_Entities.Communicator.BlastPlans>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.BlastPlans retItem = new ECN_Framework_Entities.Communicator.BlastPlans();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastPlans>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.BlastPlans Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastPlans retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.BlastPlans();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastPlans>.CreateBuilder(rdr);
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

        public static void Delete(int blastPlanID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastPlans_Delete_Single";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastPlanID", blastPlanID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Save(ECN_Framework_Entities.Communicator.BlastPlans blastPlan)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastPlans_Save";
            cmd.Parameters.Add(new SqlParameter("@BlastPlanID", (object)blastPlan.BlastPlanID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastID", (object)blastPlan.BlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)blastPlan.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)blastPlan.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EventType", blastPlan.EventType));
            cmd.Parameters.Add(new SqlParameter("@Period", (object)blastPlan.Period ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastDay", (object)blastPlan.BlastDay ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PlanType", blastPlan.PlanType));
            if (blastPlan.BlastPlanID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)blastPlan.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)blastPlan.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
