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
    public class CampaignItemTemplate
    {
        public static ECN_Framework_Entities.Communicator.CampaignItemTemplate GetByCampaignItemTemplateID(int CampaignItemTemplateID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplate_Select";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemTemplate GetByCampaignItemTemplateID(int CampaignItemTemplateID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplate_Select_CampaignItemTemplateID";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);            
            return Get(cmd);
        }

        public static void Delete(int CampaignItemTemplateID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplate_Delete";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemTemplate CampaignItemTemplate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplate_Save";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemTemplateID", CampaignItemTemplate.CampaignItemTemplateID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", CampaignItemTemplate.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@TemplateName", CampaignItemTemplate.TemplateName));
            cmd.Parameters.Add(new SqlParameter("@BlastField1", (object)CampaignItemTemplate.BlastField1 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField2", (object)CampaignItemTemplate.BlastField2 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField3", (object)CampaignItemTemplate.BlastField3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField4", (object)CampaignItemTemplate.BlastField4 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField5", (object)CampaignItemTemplate.BlastField5 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture1", (object)CampaignItemTemplate.Omniture1 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture2", (object)CampaignItemTemplate.Omniture2 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture3", (object)CampaignItemTemplate.Omniture3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture4", (object)CampaignItemTemplate.Omniture4 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture5", (object)CampaignItemTemplate.Omniture5 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture6", (object)CampaignItemTemplate.Omniture6 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture7", (object)CampaignItemTemplate.Omniture7 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture8", (object)CampaignItemTemplate.Omniture8 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture9", (object)CampaignItemTemplate.Omniture9 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Omniture10", (object)CampaignItemTemplate.Omniture10 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FromEmail", (object)CampaignItemTemplate.FromEmail ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FromName", (object)CampaignItemTemplate.FromName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ReplyTo", (object)CampaignItemTemplate.ReplyTo ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Subject", (object)CampaignItemTemplate.Subject ?? DBNull.Value));
            if (CampaignItemTemplate.CampaignItemTemplateID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)CampaignItemTemplate.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)CampaignItemTemplate.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@Archived", (object)CampaignItemTemplate.Archived ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LayoutID", (object)CampaignItemTemplate.LayoutID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OptOutMasterSuppression", (object)CampaignItemTemplate.OptOutMasterSuppression ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OptOutSpecificGroup", (object)CampaignItemTemplate.OptOutSpecificGroup ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OmnitureCustomerSetup", CampaignItemTemplate.OmnitureCustomerSetup);
            cmd.Parameters.AddWithValue("@CampaignID", (object)CampaignItemTemplate.CampaignID ?? DBNull.Value);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> GetByCustomerID(int CustomerID, string archiveFilter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplate_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            if (!string.IsNullOrEmpty(archiveFilter))
            {
                cmd.Parameters.AddWithValue("@ArchiveFilter", archiveFilter);
            }
            return GetList(cmd);
        }

        public static bool UsedByCampaignItem(int templateID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplate_UsedByCampaignItem";
            cmd.Parameters.AddWithValue("@TemplateID", templateID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> GetTemplatesBySetupLevel(int BaseChannelID,int? CustomerID, bool isCustomer)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplate_Select_SetupLevel";
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            if (CustomerID.HasValue)
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID.Value);
            cmd.Parameters.AddWithValue("@IsCustomer", isCustomer);

            return GetList(cmd);
        }

        public static void ClearOmniDataBySetupLevel(int BaseChannelID,int? CustomerID, bool isCustomer, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplate_ClearOmniData";
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            if (CustomerID.HasValue)
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID.Value);
            cmd.Parameters.AddWithValue("@IsCustomer", isCustomer);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplate>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemTemplate retItem = new ECN_Framework_Entities.Communicator.CampaignItemTemplate();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemTemplate>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.CampaignItemTemplate Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItemTemplate retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CampaignItemTemplate();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemTemplate>.CreateBuilder(rdr);
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


    }
}
