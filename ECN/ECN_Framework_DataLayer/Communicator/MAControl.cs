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
    public class MAControl
    {
        public static List<ECN_Framework_Entities.Communicator.MAControl> GetByMarketingAutomationID(int MAID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAControl_Select_MarketingAutomationID";
            cmd.Parameters.AddWithValue("@MarketingAutomationID", MAID);

            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.MAControl GetByControlID(string ControlID, int MAID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAControl_Select_ControlID";
            cmd.Parameters.AddWithValue("@ControlID", ControlID);
            cmd.Parameters.AddWithValue("@MarketingAutomationID", MAID);

            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.MAControl GetByMAControlID(int MAControlID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAControl_Select_MAControlID";
            cmd.Parameters.AddWithValue("@MAControlID", MAControlID);

            return Get(cmd);
        }

        public static bool ExistsByECNID(int formID,string ControlType,string MAState)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAControl_Exists_ByFormID";
            cmd.Parameters.AddWithValue("@FORMID", formID);
            cmd.Parameters.AddWithValue("@CONTROLTYPE", ControlType);
            cmd.Parameters.AddWithValue("@MASTATE", MAState);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static void UpdateECNID(int newID,int currentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAControl_Update_ECNID";
            cmd.Parameters.AddWithValue("@NewID", newID);
            cmd.Parameters.AddWithValue("@CurrentID", currentID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Save(ECN_Framework_Entities.Communicator.MAControl mac)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAControl_Save";

            cmd.Parameters.AddWithValue("@ControlID", mac.ControlID);
            cmd.Parameters.AddWithValue("@ECNID", mac.ECNID);
            cmd.Parameters.AddWithValue("@Text", mac.Text);
            cmd.Parameters.AddWithValue("@ExtraText", mac.ExtraText);
            cmd.Parameters.AddWithValue("@MarketingAutomationID", mac.MarketingAutomationID);
            cmd.Parameters.AddWithValue("@xPosition", mac.xPosition);
            cmd.Parameters.AddWithValue("@yPosition", mac.yPosition);
            cmd.Parameters.AddWithValue("@ControlType", mac.ControlType.ToString());
            cmd.Parameters.AddWithValue("@MAControlID", mac.MAControlID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int MAControlID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAControl_Delete";
            cmd.Parameters.AddWithValue("@MAControlID", MAControlID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        

        private static List<ECN_Framework_Entities.Communicator.MAControl> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.MAControl> retList = new List<ECN_Framework_Entities.Communicator.MAControl>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.MAControl retItem = new ECN_Framework_Entities.Communicator.MAControl();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MAControl>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);

                        if (retItem != null)
                        {

                            retItem.ControlType = (ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType), rdr["ControlType"].ToString());

                            //if (rdr["ControlType"].ToString().Equals(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem.ToString()))
                            //    retItem.ControlType = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem;
                            //else if (rdr["ControlType"].ToString().Equals(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click.ToString()))
                            //    retItem.ControlType = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click;
                            //else if (rdr["ControlType"].ToString().Equals(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click.ToString()))
                            //    retItem.ControlType = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click;
                            //else if (rdr["ControlType"].ToString().Equals(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open.ToString()))
                            //    retItem.ControlType = ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open;
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

        private static ECN_Framework_Entities.Communicator.MAControl Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.MAControl retItem = new ECN_Framework_Entities.Communicator.MAControl();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.MAControl();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MAControl>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        
                        retItem = builder.Build(rdr);
                        retItem.ControlType = (ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType), rdr["ControlType"].ToString());

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
