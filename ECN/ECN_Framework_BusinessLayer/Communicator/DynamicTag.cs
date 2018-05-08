using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class DynamicTag
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.DynamicTag;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.DynamicTag;
        public static void Validate(ECN_Framework_Entities.Communicator.DynamicTag DynamicTag)
        {           
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            if (DynamicTag.Tag.Trim().Equals(string.Empty))
                errorList.Add(new ECNError(Entity, Method, "Tag cannot be empty"));
            if (Exists(DynamicTag.Tag, DynamicTag.CustomerID.Value, DynamicTag.DynamicTagID))
                errorList.Add(new ECNError(Entity, Method, "Dynamic Tag already exists"));
            if (DynamicTag.Tag.Contains("."))
                errorList.Add(new ECNError(Entity, Method, "Period (.) is not allowed in Dynamic Tag"));
            if (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(DynamicTag.ContentID.Value,DynamicTag.CustomerID.Value))
                errorList.Add(new ECNError(Entity, Method, "Invalid Content"));
            if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(DynamicTag.Tag))
                errorList.Add(new ECNError(Entity, Method, "Tag has invalid characters"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static bool Exists(string Tag, int CustomerID, int DynamicTagID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.DynamicTag.Exists(Tag, CustomerID, DynamicTagID);
                scope.Complete();
            }
            return exists;
        }

        public static int Save(ECN_Framework_Entities.Communicator.DynamicTag DynamicTag, KMPlatform.Entity.User user)
        {
            DynamicTag.Tag = DynamicTag.Tag.Replace(" ", "_");
            Validate(DynamicTag);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(DynamicTag, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                DynamicTag.DynamicTagID = ECN_Framework_DataLayer.Communicator.DynamicTag.Save(DynamicTag);
                if (DynamicTag.DynamicTagRulesList != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.DynamicTagRule DynamicTagRule in DynamicTag.DynamicTagRulesList)
                    {
                        DynamicTagRule.DynamicTagID = DynamicTag.DynamicTagID;
                        ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.Save(DynamicTagRule, user);
                    }
                }
                scope.Complete();
            }

            return DynamicTag.DynamicTagID;
        }
        
        public static string ReplaceDynamicTags(string HTML, int emailID, int groupID, KMPlatform.Entity.User user)
        {
            List<string> toParse = new List<string>();
            toParse.Add(HTML);
            List<string> dynamicTagsList = ECN_Framework_BusinessLayer.Communicator.Content.GetTags(toParse);
            foreach (string tag in dynamicTagsList)
            {
                int replacement_ContentID = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetContentIDforTag(tag, user, emailID, groupID);
                ECN_Framework_Entities.Communicator.Content replacement_content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(replacement_ContentID, user, false);
                HTML = StringFunctions.Replace(HTML, "ECN.DynamicTag." + tag + ".ECN.DynamicTag", replacement_content.ContentSource);
            }
            return HTML;
        }

        public static string ReplaceDynamicTags_NoAccessCheck(string HTML,int customerID, int emailID, int groupID)
        {
            List<string> toParse = new List<string>();
            toParse.Add(HTML);
            List<string> dynamicTagsList = ECN_Framework_BusinessLayer.Communicator.Content.GetTags(toParse);
            foreach (string tag in dynamicTagsList)
            {
                int replacement_ContentID = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetContentIDforTag_NoAccessCheck(tag,customerID, emailID, groupID);
                ECN_Framework_Entities.Communicator.Content replacement_content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(replacement_ContentID, false);
                HTML = StringFunctions.Replace(HTML, "ECN.DynamicTag." + tag + ".ECN.DynamicTag", replacement_content.ContentSource);
            }
            return HTML;
        }

        public static void Delete(int DynamicTagID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByDynamicTagID(DynamicTagID, user, false);
            
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();
            if (IsUsedInContent(DynamicTag.Tag, DynamicTag.CustomerID.Value))
            {
                errorList.Add(new ECNError(Entity, Method, "This Tag is being used in Content"));
                throw new ECNException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.DeleteByDynamicTagID(DynamicTagID, user);
                ECN_Framework_DataLayer.Communicator.DynamicTag.Delete(DynamicTagID, user.UserID);
                scope.Complete();
            }
        }

        private static bool IsUsedInContent(string Tag, int customerID)
        {
            bool exists = false;
          
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.DynamicTag.IsUsedInContent(Tag, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static List<ECN_Framework_Entities.Communicator.DynamicTag> GetByCustomerID(int customerID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.DynamicTag> DynamicTagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DynamicTagList = ECN_Framework_DataLayer.Communicator.DynamicTag.GetByCustomerID(customerID);
                if (DynamicTagList != null && DynamicTagList.Count > 0 && getChildren)
                {
                    foreach (ECN_Framework_Entities.Communicator.DynamicTag DynamicTag in DynamicTagList)
                    {
                        DynamicTag.DynamicTagRulesList = ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.GetByDynamicTagID(DynamicTag.DynamicTagID, user, getChildren);
                    }
                }
                scope.Complete();
            }
            return DynamicTagList;
        }
        public static List<ECN_Framework_Entities.Communicator.DynamicTag> GetByContentID(int contentID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.DynamicTag> DynamicTagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DynamicTagList = ECN_Framework_DataLayer.Communicator.DynamicTag.GetByContentID(contentID);
                if (DynamicTagList != null && DynamicTagList.Count > 0 && getChildren)
                {
                    foreach (ECN_Framework_Entities.Communicator.DynamicTag DynamicTag in DynamicTagList)
                    {
                        DynamicTag.DynamicTagRulesList = ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.GetByDynamicTagID(DynamicTag.DynamicTagID, user, getChildren);
                    }
                }
                scope.Complete();
            }
            return DynamicTagList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.DynamicTag GetByTag_NoAccessCheck(string Tag,int CustomerID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DynamicTag = ECN_Framework_DataLayer.Communicator.DynamicTag.GetByTag(Tag, CustomerID);
                if (DynamicTag != null && getChildren)
                {
                    DynamicTag.DynamicTagRulesList = ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.GetByDynamicTagID_NoAccessCheck(DynamicTag.DynamicTagID, getChildren);
                }
                scope.Complete();
            }
            return DynamicTag;
        }

        public static ECN_Framework_Entities.Communicator.DynamicTag GetByTag_NoAccessCheck_UseAmbientTransaction(string Tag, int CustomerID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = null;
            using (TransactionScope scope = new TransactionScope())
            {
                DynamicTag = ECN_Framework_DataLayer.Communicator.DynamicTag.GetByTag(Tag, CustomerID);
                if (DynamicTag != null && getChildren)
                {
                    DynamicTag.DynamicTagRulesList = ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.GetByDynamicTagID_NoAccessCheck_UseAmbientTransaction(DynamicTag.DynamicTagID, getChildren);
                }
                scope.Complete();
            }
            return DynamicTag;
        }

        public static ECN_Framework_Entities.Communicator.DynamicTag GetByTag(string Tag, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DynamicTag = ECN_Framework_DataLayer.Communicator.DynamicTag.GetByTag(Tag, user.CustomerID);
                if (DynamicTag != null && getChildren)
                {
                    DynamicTag.DynamicTagRulesList = ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.GetByDynamicTagID(DynamicTag.DynamicTagID, user, getChildren);
                }
                scope.Complete();
            }
            return DynamicTag;
        }

        public static ECN_Framework_Entities.Communicator.DynamicTag GetByTag_UseAmbientTransaction(string Tag, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = null;
            using (TransactionScope scope = new TransactionScope())
            {
                DynamicTag = ECN_Framework_DataLayer.Communicator.DynamicTag.GetByTag(Tag, user.CustomerID);
                if (DynamicTag != null && getChildren)
                {
                    DynamicTag.DynamicTagRulesList = ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.GetByDynamicTagID(DynamicTag.DynamicTagID, user, getChildren);
                }
                scope.Complete();
            }
            return DynamicTag;
        }

        public static bool ContentUsedInDynamicTag(int ContentID, int CustomerID)
        {
            bool exists = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.DynamicTag.ContentUsedInDynamicTag(ContentID, CustomerID);
                scope.Complete();
            }
            return exists;
        }

        public static int GetContentIDforTag(string Tag, KMPlatform.Entity.User user, int? emailID =null, int? groupID=null)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = GetByTag(Tag,user, true);
            int contentID = 0;
            if (emailID != null && groupID!=null)
            {
                bool IsApplicable = false;                
                var DynamicTagRulesList = (from src in DynamicTag.DynamicTagRulesList
                             orderby src.Priority
                             select src).ToList();
                foreach (ECN_Framework_Entities.Communicator.DynamicTagRule dynamicTagRule in DynamicTagRulesList)
                {
                    IsApplicable = ECN_Framework_BusinessLayer.Communicator.Rule.IsApplicable(dynamicTagRule.RuleID.Value, emailID.Value, groupID.Value);
                    if (IsApplicable)
                    {
                        contentID = dynamicTagRule.ContentID.Value;
                        break;
                    }
                }
                if(contentID==0)
                    contentID = DynamicTag.ContentID.Value;
            }
            else
            {
                contentID =  DynamicTag.ContentID.Value;
            }
            return contentID;
        }

        public static int GetContentIDforTag_NoAccessCheck(string Tag, int customerID, int? emailID = null, int? groupID = null)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = GetByTag_NoAccessCheck(Tag, customerID, true);
            int contentID = 0;
            if (emailID != null && groupID != null)
            {
                bool IsApplicable = false;
                var DynamicTagRulesList = (from src in DynamicTag.DynamicTagRulesList
                                           orderby src.Priority
                                           select src).ToList();
                foreach (ECN_Framework_Entities.Communicator.DynamicTagRule dynamicTagRule in DynamicTagRulesList)
                {
                    IsApplicable = ECN_Framework_BusinessLayer.Communicator.Rule.IsApplicable(dynamicTagRule.RuleID.Value, emailID.Value, groupID.Value);
                    if (IsApplicable)
                    {
                        contentID = dynamicTagRule.ContentID.Value;
                        break;
                    }
                }
                if (contentID == 0)
                    contentID = DynamicTag.ContentID.Value;
            }
            else
            {
                contentID = DynamicTag.ContentID.Value;
            }
            return contentID;
        }

        public static ECN_Framework_Entities.Communicator.DynamicTag GetByDynamicTagID(int DynamicTagID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.DynamicTag DynamicTag = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DynamicTag = ECN_Framework_DataLayer.Communicator.DynamicTag.GetByDynamicTagID(DynamicTagID);
                if (DynamicTag != null && getChildren)
                {
                    DynamicTag.DynamicTagRulesList = ECN_Framework_BusinessLayer.Communicator.DynamicTagRule.GetByDynamicTagID(DynamicTag.DynamicTagID, user, getChildren);
                }
                scope.Complete();
            }
            return DynamicTag;
        }
    }
}
