using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemTemplate
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemTemplate;

        public static ECN_Framework_Entities.Communicator.CampaignItemTemplate GetByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user, bool getChildren = false)
        {
            ECN_Framework_Entities.Communicator.CampaignItemTemplate campaignItemTemplate;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignItemTemplate = ECN_Framework_DataLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(CampaignItemTemplateID, user.CustomerID);

                scope.Complete();
            }

            if (campaignItemTemplate != null && getChildren)
            {
                campaignItemTemplate.SuppressionGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateSuppressionGroup.GetByCampaignItemTemplateID(campaignItemTemplate.CampaignItemTemplateID, user);
                campaignItemTemplate.SelectedGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateGroup.GetByCampaignItemTemplateID(campaignItemTemplate.CampaignItemTemplateID);
                campaignItemTemplate.OptoutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateOptoutGroup.GetByCampaignItemTemplateID(campaignItemTemplate.CampaignItemTemplateID);
            }

            return campaignItemTemplate;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemTemplate Get_CampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user, bool getChildren = false)
        {
            ECN_Framework_Entities.Communicator.CampaignItemTemplate campaignItemTemplate;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignItemTemplate = ECN_Framework_DataLayer.Communicator.CampaignItemTemplate.GetByCampaignItemTemplateID(CampaignItemTemplateID);

                scope.Complete();
            }

            if (campaignItemTemplate != null && getChildren)
            {
                campaignItemTemplate.SuppressionGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateSuppressionGroup.GetByCampaignItemTemplateID(campaignItemTemplate.CampaignItemTemplateID, user);
                campaignItemTemplate.SelectedGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateGroup.GetByCampaignItemTemplateID(campaignItemTemplate.CampaignItemTemplateID);
                campaignItemTemplate.OptoutGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateOptoutGroup.GetByCampaignItemTemplateID(campaignItemTemplate.CampaignItemTemplateID);
            }

            return campaignItemTemplate;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> GetByCustomerID(int CustomerID, KMPlatform.Entity.User user, bool getChildren = false, string archiveFilter = "all")
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> itemList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplate>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Communicator.CampaignItemTemplate.GetByCustomerID(CustomerID, archiveFilter);
                scope.Complete();
            }

            if (itemList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplate CampaignItemTemplate in itemList)
                {
                    CampaignItemTemplate.SuppressionGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateSuppressionGroup.GetByCampaignItemTemplateID(CampaignItemTemplate.CampaignItemTemplateID, user);
                    CampaignItemTemplate.SelectedGroupList = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateGroup.GetByCampaignItemTemplateID(CampaignItemTemplate.CampaignItemTemplateID);
                }
            }

            return itemList;
        }

        public static bool Exists(ECN_Framework_Entities.Communicator.CampaignItemTemplate cit, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> lstCIT = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplate>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lstCIT = ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCustomerID(user.CustomerID, user, false, "all").Where(x => x.TemplateName.ToLower().Equals(cit.TemplateName.ToLower())).ToList();
                scope.Complete();
            }
            if (lstCIT.Count > 0)
                return true;
            else
                return false;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.CampaignItemTemplate CampaignItemTemplate, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (string.IsNullOrEmpty(CampaignItemTemplate.TemplateName))
            {
                errorList.Add(new ECNError(Entity, Method, "Template name cannot be empty"));
                throw new ECNException(errorList);
            }

            if (CampaignItemTemplate.CampaignItemTemplateID < 1)
            {
                if (Exists(CampaignItemTemplate, user))
                    errorList.Add(new ECNError(Entity, Method, "Template name already exists, please enter a new one"));
            }

            if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(CampaignItemTemplate.TemplateName))
                errorList.Add(new ECNError(Entity, Method, "Template name has invalid characters"));

            if (errorList.Count > 0)
                throw new ECNException(errorList);
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemTemplate CampaignItemTemplate, KMPlatform.Entity.User user)
        {
            Validate(CampaignItemTemplate, user);

            using (TransactionScope scope = new TransactionScope())
            {
                CampaignItemTemplate.CampaignItemTemplateID = ECN_Framework_DataLayer.Communicator.CampaignItemTemplate.Save(CampaignItemTemplate);
                scope.Complete();
            }
            return CampaignItemTemplate.CampaignItemTemplateID;
        }

        public static void Delete(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {

            ValidateForDelete(CampaignItemTemplateID);

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTemplate.Delete(CampaignItemTemplateID, user.UserID);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplateSuppressionGroup.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, user);
                scope.Complete();
            }
        }

        public static void ValidateForDelete(int templateID)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();
            if (UsedByCampaignItem(templateID))
            {
                errorList.Add(new ECNError(Entity, Method, "Template is used by Campaign Item"));
            }

            if (errorList.Count > 0)
                throw new ECNException(errorList);
        }

        public static bool UsedByCampaignItem(int templateID)
        {
            bool used = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                used = ECN_Framework_DataLayer.Communicator.CampaignItemTemplate.UsedByCampaignItem(templateID);
                scope.Complete();
            }

            return used;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> GetTemplatesBySetupLevel(int baseChannel,int? CustomerID, bool isCustomer)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplate>();

            using (TransactionScope scope = new TransactionScope())
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemTemplate.GetTemplatesBySetupLevel(baseChannel,CustomerID, isCustomer);
                scope.Complete();
            }

            return retList;
        }

        public static void ClearOutOmniDataBySetupLevel(int baseChannel,int? CustomerID, bool isCustomer, int UserID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTemplate.ClearOmniDataBySetupLevel(baseChannel,CustomerID, isCustomer, UserID);
                scope.Complete();
            }
        }
    }
}
