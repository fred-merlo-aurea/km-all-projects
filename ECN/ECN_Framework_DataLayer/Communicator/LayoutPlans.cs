using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;
using ECN_Framework_DataLayer.Communicator.Helpers;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class LayoutPlans
    {
        private const string ProcedureLayoutPlansDeleteByLPID = "e_LayoutPlans_Delete_ByLPID";
        private const string ProcedureLayoutPlansDeleteSingle = "e_LayoutPlans_Delete_Single";

        public static ECN_Framework_Entities.Communicator.LayoutPlans GetByLayoutPlanID(int layoutPlanID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlans_Select_LayoutPlanID";
            cmd.Parameters.AddWithValue("@LayoutPlanID", layoutPlanID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByLayoutID(int layoutID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlans_Select_LayoutID";
            cmd.Parameters.AddWithValue("@layoutID", layoutID);
            cmd.Parameters.AddWithValue("@customerID", customerID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByGroupID(int groupID,int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlans_Select_GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetBySmartFormID(int SmartFormID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlans_Select_SmartFormID";
            cmd.Parameters.AddWithValue("@SmartFormID", SmartFormID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByType(int layoutID, int customerID, string eventType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlans_Select_Type";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@EventType", eventType);
            return GetList(cmd);
        }
        public static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetByFormTokenUID(Guid TokenUID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlans_Select_TokenUID";
            cmd.Parameters.AddWithValue("@TokenUID", TokenUID);
            return GetList(cmd);
        }
        
        private static ECN_Framework_Entities.Communicator.LayoutPlans Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LayoutPlans retItem = null;

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (rdr != null)
                    {
                        try
                        {
                            retItem = new ECN_Framework_Entities.Communicator.LayoutPlans();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LayoutPlans>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return retItem;
        }

        private static List<ECN_Framework_Entities.Communicator.LayoutPlans> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.LayoutPlans> retList = new List<ECN_Framework_Entities.Communicator.LayoutPlans>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (rdr != null)
                    {
                        try
                        {
                            ECN_Framework_Entities.Communicator.LayoutPlans retItem = new ECN_Framework_Entities.Communicator.LayoutPlans();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LayoutPlans>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);

                                retList.Add(retItem);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return retList;
        }

        public static bool Exists(int layoutPlanID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlan_Exists";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LayoutPlanID", layoutPlanID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int groupID, string criteria)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlan_Exists_GroupID";            
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@Criteria", criteria);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.LayoutPlans layoutPlan)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlans_Save";
            cmd.Parameters.Add(new SqlParameter("@LayoutPlanID", layoutPlan.LayoutPlanID));
            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)layoutPlan.LayoutID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EventType", layoutPlan.EventType));
            cmd.Parameters.Add(new SqlParameter("@BlastID", (object)layoutPlan.BlastID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Period", (object)layoutPlan.Period ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Criteria", layoutPlan.Criteria));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)layoutPlan.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ActionName", layoutPlan.ActionName));
            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)layoutPlan.GroupID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Status", layoutPlan.Status));
            cmd.Parameters.Add(new SqlParameter("@SmartFormID",(object)layoutPlan.SmartFormID ?? DBNull.Value));
            if (layoutPlan.LayoutPlanID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)layoutPlan.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)layoutPlan.CreatedUserID ?? DBNull.Value));

            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)layoutPlan.CampaignItemID ?? DBNull.Value));
            if(layoutPlan.TokenUID.HasValue)
                cmd.Parameters.AddWithValue("@TokenUID", layoutPlan.TokenUID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int layoutPlanID, int customerID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LayoutPlans_Delete_All";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LayoutPlanID", layoutPlanID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int layoutId, int layoutPlanId, int customerId, int userId)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryLayoutPlans(layoutPlanId, userId, layoutId, customerId, ProcedureLayoutPlansDeleteSingle);
        }

        public static void Delete(int layoutPlanId, int userId)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryLayoutPlans(layoutPlanId, userId, ProcedureLayoutPlansDeleteByLPID);
        }

        public static DataTable GetGroupLayoutPlanSummary(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_LayoutPlans_GetGroupLayoutPlanSummary";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetCampaignLayoutPlanSummary(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_LayoutPlans_GetCampaignLayoutPlanSummary";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
