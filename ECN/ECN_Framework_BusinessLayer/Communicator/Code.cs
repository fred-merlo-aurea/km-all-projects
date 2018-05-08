using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Code
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Code;

        public static bool Exists()
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Code.Exists();
                scope.Complete();
            }
            return exists;
        }

        private static bool HasFeature(KMPlatform.Entity.User user)
        {
            if ((ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(user.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner)))
                return true;
            return false;
        }

        public static bool Exists(int codeID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Code.Exists(codeID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int codeID, string codeValue, string codeType, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Code.Exists(codeID, codeValue, codeType, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.Code GetByCodeID(int codeID, KMPlatform.Entity.User user)
        {
            if (!HasFeature(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Entities.Communicator.Code code = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                code = ECN_Framework_DataLayer.Communicator.Code.GetByCodeID(codeID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(code, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return code;
        }

        public static List<ECN_Framework_Entities.Communicator.Code> GetByCustomerAndCategory(ECN_Framework_Common.Objects.Communicator.Enums.CodeType ctype, KMPlatform.Entity.User user)
        {
            if (ctype == ECN_Framework_Common.Objects.Communicator.Enums.CodeType.LINKTYPE)
            {
                if (!HasFeature(user))
                    throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }
            List<ECN_Framework_Entities.Communicator.Code> codeList = new List<ECN_Framework_Entities.Communicator.Code>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                codeList = ECN_Framework_DataLayer.Communicator.Code.GetByCustomerAndCategory(ctype, user.CustomerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(codeList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return codeList;
        }

        public static List<ECN_Framework_Entities.Communicator.Code> GetByCustomerAndCategory(string type, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Code> codeList = new List<ECN_Framework_Entities.Communicator.Code>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                codeList = ECN_Framework_DataLayer.Communicator.Code.GetByCustomerAndCategory(type, user.CustomerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(codeList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return codeList;
        }

        public static List<ECN_Framework_Entities.Communicator.Code> GetByCustomerAndCategory(string type, int customerId, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Code> codeList = new List<ECN_Framework_Entities.Communicator.Code>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                codeList = ECN_Framework_DataLayer.Communicator.Code.GetByCustomerAndCategory(type, customerId);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(codeList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return codeList;
        }

        public static List<ECN_Framework_Entities.Communicator.Code> GetAllByCustomer(KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Code> codeList = new List<ECN_Framework_Entities.Communicator.Code>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                codeList = ECN_Framework_DataLayer.Communicator.Code.GetAllByCustomer(user.CustomerID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(codeList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return codeList;
        }

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.Code code, KMPlatform.Entity.User user)
        //{
        //    if (code != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (code.CustomerID.Value != user.CustomerID)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.Code> codeList, KMPlatform.Entity.User user)
        //{
        //    if (codeList != null)
        //    {
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in codeList
        //                                join c in custList on ct.CustomerID.Value equals c.CustomerID
        //                                select new { ct.CodeID };

        //            if (securityCheck.Count() != codeList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in codeList
        //                                where ct.CustomerID.Value != user.CustomerID
        //                                select new { ct.CodeID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static void Save(ECN_Framework_Entities.Communicator.Code code, KMPlatform.Entity.User user)
        {
            if (!HasFeature(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (code.CreatedUserID == null && code.UpdatedUserID != null)
                code.CreatedUserID = code.UpdatedUserID;

            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (code.CodeID > 0)
            {
                if (!Exists(code.CodeID, code.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "CodeID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(code, user);


            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(code, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                code.CodeID = ECN_Framework_DataLayer.Communicator.Code.Save(code);
                scope.Complete();
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.Code code, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!KM.Platform.User.IsChannelAdministrator(user))
                    {
                        if (!KMPlatform.BusinessLogic.User.Exists(code.CreatedUserID.Value, code.CustomerID.Value))
                            throw new ECN_Framework_Common.Objects.SecurityException();
                    }
                    else 
                    {
                        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(code.CustomerID.Value, false);

                        if (customer.BaseChannelID != ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false).BaseChannelID)
                        {
                            throw new ECN_Framework_Common.Objects.SecurityException();
                        }
                    }
                    supressscope.Complete();
                }
            }

            if (code.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                if (code.CodeType.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "CodeType is invalid"));
                else
                {
                    if (code.CodeValue.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "CodeValue is invalid"));
                    else if (Exists(code.CodeID, code.CodeValue, code.CodeType, code.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CodeValue already exists for CodeType"));
                }

                if (code.CodeDisplay.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "CodeDisplay is invalid"));

                if (code.DisplayFlag != "Y" && code.DisplayFlag != "N")
                    errorList.Add(new ECNError(Entity, Method, "DisplayFlag is invalid"));

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(code.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

                    if (code.CustomerID <= 0 && code.CreatedUserID == null)// && (user.i || !KMPlatform.BusinessLogic.User.Exists(code.CreatedUserID.Value, code.CustomerID.Value)
                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

                    if (code.CodeID > 0 && (code.UpdatedUserID == null))// || !KMPlatform.BusinessLogic.User.Exists(code.UpdatedUserID.Value, code.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
                    scope.Complete();
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Delete(int codeID, ECN_Framework_Common.Objects.Communicator.Enums.CodeType ctype, KMPlatform.Entity.User user)
        {
            if (!HasFeature(user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(codeID, user.CustomerID))
            {
                if (ctype == ECN_Framework_Common.Objects.Communicator.Enums.CodeType.LINKTYPE)
                {
                    if (!ECN_Framework_BusinessLayer.Communicator.LinkAlias.CodeUsedInLinkAlias(codeID))
                    {
                        //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                        GetByCodeID(codeID, user);
                        using (TransactionScope scope = new TransactionScope())
                        {
                            ECN_Framework_DataLayer.Communicator.Code.Delete(codeID, user.CustomerID, user.UserID);
                            scope.Complete();
                        }
                    }
                    else
                    {
                        errorList.Add(new ECNError(Entity, Method, "Code is used in link alias"));
                        throw new ECNException(errorList);
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Unsupported delete action"));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Code does not exist"));
                throw new ECNException(errorList);
            }
        }
    }
}
