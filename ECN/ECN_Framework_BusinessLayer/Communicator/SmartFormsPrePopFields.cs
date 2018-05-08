using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class SmartFormsPrePopFields
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.SmartFormsPrePop;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.SmartFormsPrePopFields;

        public static bool ExistsDisplayName(int sfid, int prePopFieldID, string displayName)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.SmartFormsPrePopFields.ExistsDisplayName(sfid, prePopFieldID, displayName);
                scope.Complete();
            }
            return exists;
        }

        public static bool ExistsProfileFieldName(int sfid, int prePopFieldID, string profileFieldName)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.SmartFormsPrePopFields.ExistsProfileFieldName(sfid, prePopFieldID, profileFieldName);
                scope.Complete();
                return exists;
            }
        }

        public static void Delete(int sfid, KMPlatform.Entity.User user)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            GetBySFID(sfid, user);

            if (!KM.Platform.User.IsSystemAdministrator(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.SmartFormsPrePopFields.Delete(sfid, user.CustomerID, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int sfid, int prePopFieldID, KMPlatform.Entity.User user)
        {
            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
            GetByPrePopFieldID(prePopFieldID, user);

            if (!KM.Platform.User.IsSystemAdministrator(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.SmartFormsPrePopFields.Delete(sfid, prePopFieldID, user.CustomerID, user.UserID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields> GetBySFID(int sfid, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields> prePopFieldsList = new List<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                prePopFieldsList = ECN_Framework_DataLayer.Communicator.SmartFormsPrePopFields.GetBySFID(sfid);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(prePopFieldsList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return prePopFieldsList;
        }

        public static ECN_Framework_Entities.Communicator.SmartFormsPrePopFields GetByPrePopFieldID(int prePopFieldID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.SmartFormsPrePopFields prePopFields = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                prePopFields = ECN_Framework_DataLayer.Communicator.SmartFormsPrePopFields.GetByPrePopFieldID(prePopFieldID);
                scope.Complete();
            }

            if (!KM.Platform.User.IsSystemAdministrator(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(prePopFields, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return prePopFields;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.SmartFormsPrePopFields prePopFields, KMPlatform.Entity.User user)
        //{
        //    if (prePopFields != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (prePopFields.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.SmartFormsPrePopFields> prePopFieldsList, KMPlatform.Entity.User user)
        //{
        //    if (prePopFieldsList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in prePopFieldsList
        //                                join c in custList on ct.CustomerID.Value equals c.CustomerID
        //                                select new { ct.PrePopFieldID };

        //            if (securityCheck.Count() != prePopFieldsList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in prePopFieldsList
        //                                where ct.CustomerID.Value != user.CustomerID
        //                                select new { ct.PrePopFieldID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Validate(ECN_Framework_Entities.Communicator.SmartFormsPrePopFields prePopFields)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (prePopFields.ProfileFieldName.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "ProfileFieldName is invalid"));

            if (prePopFields.DisplayName.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "DisplayName is invalid"));

            if (prePopFields.CustomerID == null)
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(prePopFields.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (prePopFields.CreatedUserID == null || (prePopFields.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(prePopFields.CreatedUserID.Value, false))))
                    {
                        if (prePopFields.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(prePopFields.CreatedUserID.Value, prePopFields.CustomerID.Value))
                            errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));
                    }

                    if (prePopFields.PrePopFieldID > 0 && (prePopFields.UpdatedUserID == null || (prePopFields.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(prePopFields.UpdatedUserID.Value, false)))))
                    {
                        if (prePopFields.PrePopFieldID > 0 && (prePopFields.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(prePopFields.UpdatedUserID.Value, prePopFields.CustomerID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    }

                    scope.Complete();
                }
            }

            if (prePopFields.SFID == null)
                errorList.Add(new ECNError(Entity, Method, "SFID is invalid"));
            else
            {
                if (ExistsProfileFieldName(prePopFields.SFID.Value, prePopFields.PrePopFieldID, prePopFields.ProfileFieldName))
                    errorList.Add(new ECNError(Entity, Method, "ProfileFieldName is invalid"));
                if (ExistsDisplayName(prePopFields.SFID.Value, prePopFields.PrePopFieldID, prePopFields.DisplayName))
                    errorList.Add(new ECNError(Entity, Method, "DisplayName is invalid"));
            }

            if (prePopFields.ControlType.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "ControlType is invalid"));

            if (prePopFields.Required.Trim().ToUpper() != "Y" && prePopFields.Required.Trim().ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "Required is invalid"));

            if (prePopFields.PrePopulate.Trim().ToUpper() != "Y" && prePopFields.PrePopulate.Trim().ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "PrePopulate is invalid"));

            if (prePopFields.SortOrder == null)
                errorList.Add(new ECNError(Entity, Method, "SortOrder is invalid"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }         
        }

        public static int Save(ECN_Framework_Entities.Communicator.SmartFormsPrePopFields prePopFields, KMPlatform.Entity.User user)
        {
            Validate(prePopFields);

            if (!KM.Platform.User.IsSystemAdministrator(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(prePopFields, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                prePopFields.PrePopFieldID = ECN_Framework_DataLayer.Communicator.SmartFormsPrePopFields.Save(prePopFields);
                scope.Complete();
            }
            return prePopFields.PrePopFieldID;
        }

        public static DataTable GetColumnNames(int sfid, int groupID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.SmartFormsPrePopFields.GetColumnNames(sfid, groupID);
                scope.Complete();
            }
            return dt;
        }
    }
}
