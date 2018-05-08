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
    public class MarketingAutomation
    {
        public static List<ECN_Framework_Entities.Communicator.MarketingAutomation> SelectByCustomerID(int CustomerID)
        {
            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingAutomation_Select_CustomerID";

            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.MarketingAutomation> SelectByBaseChannelID(int BaseChannelID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingAutomation_Select_BaseChannelID";

            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            return GetList(cmd);
        }
        public static DataSet GetAllMarketingAutomationsbySearch(int BaseChannelID, string AutomationName,string State,string SearchCriteria,int currentPage,int pageSize,string sortDirection,string sortColumn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingAutomation_GetAllMarketingAutomationsbySearch";
            if (AutomationName.Trim().Length > 0)
            {
                cmd.Parameters.AddWithValue("@AutomationName", AutomationName);
            }
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            if (State.Trim().Length > 0)
            {
                cmd.Parameters.AddWithValue("@State", State);
            }
            if (SearchCriteria.Trim().Length > 0)
            {
                cmd.Parameters.AddWithValue("@SearchCriteria", SearchCriteria);
            }
            cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
        public static ECN_Framework_Entities.Communicator.MarketingAutomation GetByMarketingAutomationID(int MAID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingAutomation_Select_MAID";
            cmd.Parameters.AddWithValue("@MarketingAutomationID", MAID);

            return Get(cmd);

        }

        public static bool Exists(int baseChannelID, string Name, int MAID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingAutomation_Exists_Name";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@Name", Name);
            if (MAID > 0)
                cmd.Parameters.AddWithValue("@MAID", MAID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.MarketingAutomation ma)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingAutomation_Save";

            cmd.Parameters.AddWithValue("@MarketingAutomationID", ma.MarketingAutomationID);
            cmd.Parameters.AddWithValue("@Name", ma.Name);
            cmd.Parameters.AddWithValue("@State", ma.State.ToString());
            cmd.Parameters.AddWithValue("@IsDeleted", ma.IsDeleted);
            cmd.Parameters.AddWithValue("@Goal",ma.Goal);
            cmd.Parameters.AddWithValue("@StartDate", ma.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", ma.EndDate);
            cmd.Parameters.AddWithValue("@JSONDiagram", ma.JSONDiagram);
            cmd.Parameters.AddWithValue("@CustomerID", ma.CustomerID);
            if(ma.MarketingAutomationID <= 0)
            {
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CreatedUserID", ma.CreatedUserID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedUserID", ma.UpdatedUserID);
            }

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());

        }

        public static List<ECN_Framework_Entities.Communicator.MarketingAutomation> CheckIfControlExists(int ECNID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType controlType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAControl_Exists_ECNID_ControlType";
            cmd.Parameters.AddWithValue("@ECNID", ECNID);
            cmd.Parameters.AddWithValue("@ControlType", controlType.ToString());

            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.MarketingAutomation> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.MarketingAutomation> retList = new List<ECN_Framework_Entities.Communicator.MarketingAutomation>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.MarketingAutomation retItem = new ECN_Framework_Entities.Communicator.MarketingAutomation();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MarketingAutomation>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        
                        if (retItem != null)
                        {
                            retItem.State = (ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus), rdr["State"].ToString());
                            
                            
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

        private static ECN_Framework_Entities.Communicator.MarketingAutomation Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.MarketingAutomation retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.MarketingAutomation();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MarketingAutomation>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        retItem.State = (ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus), rdr["State"].ToString());
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }
    }



    
}
