using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class CustomerTemplate
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CustomerTemplate;

        public static bool Exists(int CTID)
        {
            return ECN_Framework_DataLayer.Accounts.CustomerTemplate.Exists(CTID);
        }

        public static ECN_Framework_Entities.Accounts.CustomerTemplate GetByTypeID(int customerID, string templateType, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.CustomerTemplate template = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                template = ECN_Framework_DataLayer.Accounts.CustomerTemplate.GetByTypeID(customerID, templateType);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(template, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return template;
        }

        public static ECN_Framework_Entities.Accounts.CustomerTemplate GetByTypeID_NoAccessCheck(int customerID, string templateType)
        {
            ECN_Framework_Entities.Accounts.CustomerTemplate template = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                template = ECN_Framework_DataLayer.Accounts.CustomerTemplate.GetByTypeID(customerID, templateType);
                scope.Complete();
            }

            return template;
        }

        public static ECN_Framework_Entities.Accounts.CustomerTemplate GetByCTID(int CTID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.CustomerTemplate template = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                template = ECN_Framework_DataLayer.Accounts.CustomerTemplate.GetByCTID(CTID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && !SecurityCheck(template, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return template;
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerTemplate> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.CustomerTemplate> ltemplate = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ltemplate = ECN_Framework_DataLayer.Accounts.CustomerTemplate.GetByCustomerID(customerID);
                scope.Complete();
            }

            return ltemplate;
        }        

        private static bool SecurityCheck(ECN_Framework_Entities.Accounts.CustomerTemplate template, KMPlatform.Entity.User user)
        {
            if (template != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

                    if (!custExists.Any())
                        return false;
                }
                else if (template.CustomerID.Value != user.CustomerID)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool SecurityCheck(List<ECN_Framework_Entities.Accounts.CustomerTemplate> lcustomerTemplate, KMPlatform.Entity.User user)
        {
            if (lcustomerTemplate != null)
            {
                if (KM.Platform.User.IsChannelAdministrator(user))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

                    List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

                    var securityCheck = from t in lcustomerTemplate
                                        join c in custList on t.CustomerID equals c.CustomerID
                                        select new { t.CTID };

                    if (securityCheck.Count() != lcustomerTemplate.Count)
                        return false;
                }
                else
                {
                    var securityCheck = from t in lcustomerTemplate
                                        where t.CustomerID != user.CustomerID
                                        select new { t.CTID };

                    if (securityCheck.Any())
                        return false;
                }
            }
            return true;
        }

        public static void Save(ECN_Framework_Entities.Accounts.CustomerTemplate customerTemplate, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;

            if (customerTemplate.CTID > 0)
            {
                if (!Exists(customerTemplate.CTID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "Template is invalid"));
                    throw new ECNException(errorList);
                }
            }

            Validate(customerTemplate, user);
            customerTemplate.CTID = ECN_Framework_DataLayer.Accounts.CustomerTemplate.Save(customerTemplate);
        }

        public static void Validate(ECN_Framework_Entities.Accounts.CustomerTemplate customerTemplate, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user) && KM.Platform.User.IsChannelAdministrator(user))
            {
                using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerTemplate.CustomerID.Value, false).BaseChannelID != ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false).BaseChannelID)
                    {
                        throw new ECN_Framework_Common.Objects.SecurityException();
                    }
                    supressscope.Complete();
                }
            }

            if (customerTemplate.CTID <= 0 && customerTemplate.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (customerTemplate.CTID > 0 && customerTemplate.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int CTID, KMPlatform.Entity.User user)
        {
            ECN_Framework_DataLayer.Accounts.CustomerTemplate.Delete(CTID, user.UserID);
        }

        public static string SelectTemplate(int customerID, string templateType, string column, KMPlatform.Entity.User user)
        {
            string header = string.Empty;            

            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, false);
            ECN_Framework_Entities.Accounts.BaseChannel baseChannel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(customer.BaseChannelID.Value);
            ECN_Framework_Entities.Accounts.CustomerTemplate template = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID(customerID, templateType, user);

            // Figure out if they want the header or the footer
            if ("footer" == column)
            {
                header = template.FooterSource;
            }
            else if ("header" == column)
            {
                header = template.HeaderSource;
            }
            else
            {
                header = template.HeaderSource;
            }

            if (header.Trim() == string.Empty)
            {
                if (baseChannel.MasterCustomerID != null)
                {
                    template = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID(baseChannel.MasterCustomerID.Value, templateType, user);
                    header = template.HeaderSource;
                }
            }

            if (header.Trim() == string.Empty)
            {
                template = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByTypeID(1, templateType, user);
                header = template.HeaderSource;
            }
            return header;
        }
    }
}
