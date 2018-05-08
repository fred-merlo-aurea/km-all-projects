using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Template
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Template;

        public static ECN_Framework_Entities.Communicator.Template GetByTemplateID(int templateID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Template template = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                template = ECN_Framework_DataLayer.Communicator.Template.GetByTemplateID(templateID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(template, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return template;
        }

        public static ECN_Framework_Entities.Communicator.Template GetByTemplateID_UseAmbientTransaction(int templateID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Template template = null;
            using (TransactionScope scope = new TransactionScope())
            {
                template = ECN_Framework_DataLayer.Communicator.Template.GetByTemplateID(templateID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(template, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return template;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.Template GetByTemplateID_NoAccessCheck(int templateID)
        {
            ECN_Framework_Entities.Communicator.Template template = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                template = ECN_Framework_DataLayer.Communicator.Template.GetByTemplateID(templateID);
                scope.Complete();
            }

            
            return template;
        }

        public static ECN_Framework_Entities.Communicator.Template GetByTemplateID_NoAccessCheck_UseAmbientTransaction(int templateID)
        {
            ECN_Framework_Entities.Communicator.Template template = null;
            using (TransactionScope scope = new TransactionScope())
            {
                template = ECN_Framework_DataLayer.Communicator.Template.GetByTemplateID(templateID);
                scope.Complete();
            }


            return template;
        }

        public static List<ECN_Framework_Entities.Communicator.Template> GetByStyleCode(int baseChannelID, string styleCode, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Template> templateList = new List<ECN_Framework_Entities.Communicator.Template>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                templateList = ECN_Framework_DataLayer.Communicator.Template.GetByStyleCode(baseChannelID, styleCode);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(templateList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return templateList;
        }

        public static List<ECN_Framework_Entities.Communicator.Template> GetByStyleCode_NoAccessCheck(int baseChannelID, string styleCode)
        {
            List<ECN_Framework_Entities.Communicator.Template> templateList = new List<ECN_Framework_Entities.Communicator.Template>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                templateList = ECN_Framework_DataLayer.Communicator.Template.GetByStyleCode(baseChannelID, styleCode);
                scope.Complete();
            }

            return templateList;
        }

        public static List<ECN_Framework_Entities.Communicator.Template> GetByBaseChannelID(int baseChannelID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Template> templateList = new List<ECN_Framework_Entities.Communicator.Template>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                templateList = ECN_Framework_DataLayer.Communicator.Template.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(templateList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return templateList;
        }

        public static bool Exists(int templateID, int baseChannelID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Template.Exists(templateID, baseChannelID);
                scope.Complete();
            }
            return exists;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.Template template, KMPlatform.Entity.User user)
        //{
        //    if (template != null)
        //    {
        //        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (template.BaseChannelID.Value != customer.BaseChannelID.Value)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.Template> templateList, KMPlatform.Entity.User user)
        //{
        //    if (templateList != null)
        //    {
        //        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in templateList
        //                                join c in custList on ct.BaseChannelID equals c.BaseChannelID.Value
        //                                select new { ct.TemplateID };

        //            if (securityCheck.Count() != templateList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in templateList
        //                                where ct.BaseChannelID != customer.BaseChannelID.Value
        //                                select new { ct.TemplateID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Validate(ECN_Framework_Entities.Communicator.Template template, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (template.TemplateName.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "Template Name is missing"));
            else if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(template.TemplateName))
                errorList.Add(new ECNError(Entity, Method, "Template Name has invalid characters"));

            if (template.TemplateID <= 0 && (template.CreatedUserID == null))  // || !KMPlatform.BusinessLogic.User.ExistsByBaseChannelID(template.CreatedUserID.Value, template.BaseChannelID.Value)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (template.TemplateID > 0 && (template.UpdatedUserID == null)) //  || !KMPlatform.BusinessLogic.User.ExistsByBaseChannelID(template.UpdatedUserID.Value, template.BaseChannelID.Value)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (!template.TemplateSource.Contains("%%slot1%%"))
                errorList.Add(new ECNError(Entity, Method, "%%slot1%% is required in HTML Source."));

            if (!template.TemplateText.Contains("%%slot1%%"))
                errorList.Add(new ECNError(Entity, Method, "%%slot1%% is required in Text Source."));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
       
        }

        public static int Save(ECN_Framework_Entities.Communicator.Template template, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (template.TemplateID > 0)
            {
                if (!Exists(template.TemplateID, template.BaseChannelID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "TemplateID is invalid"));
                    throw new ECNException(errorList);
                }
            }

            Validate(template, user);

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(template, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                template.TemplateID = ECN_Framework_DataLayer.Communicator.Template.Save(template);
                scope.Complete();
            }
            return template.TemplateID;
        }

        public static void Delete(int templateID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            //this does validation
            ECN_Framework_Entities.Communicator.Template template = GetByTemplateID(templateID, user);
            if (template == null)
            {
                errorList.Add(new ECNError(Entity, Method, "Template does not exist"));
                throw new ECNException(errorList);
            }
            else
            {
                if (ECN_Framework_BusinessLayer.Communicator.Layout.TemplateUsedInLayout(template.TemplateID))
                {
                    errorList.Add(new ECNError(Entity, Method, "Template is used in message(s)"));
                    throw new ECNException(errorList);
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.Template.Delete(templateID, user.UserID);
                        scope.Complete();
                    }
                }
            }
        }

        public static ECN_Framework_Entities.Communicator.Template GetByNumberOfSlots(int numberOfSlots, int baseChannelID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Template template = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                template = ECN_Framework_DataLayer.Communicator.Template.GetByNumberOfSlots(numberOfSlots, baseChannelID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(template, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return template;
        }

    }
}
