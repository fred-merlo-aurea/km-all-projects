using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class MessageType
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.MessageType;

        public static bool HasPermission(KMPlatform.Enums.Access rights, KMPlatform.Entity.User user)
        {
            if (KM.Platform.User.IsChannelAdministrator(user) || KM.Platform.User.IsSystemAdministrator(user))
                return true;
            return false;
        }

        public static ECN_Framework_Entities.Communicator.MessageType GetByMessageTypeID(int messageTypeID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.MessageType type = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                type = ECN_Framework_DataLayer.Communicator.MessageType.GetByMessageTypeID(messageTypeID);
                scope.Complete();
            }
            
            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(type, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return type;
        }

        public static ECN_Framework_Entities.Communicator.MessageType GetByMessageTypeID_UseAmbientTransaction(int messageTypeID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.MessageType type = null;
            using (TransactionScope scope = new TransactionScope())
            {
                type = ECN_Framework_DataLayer.Communicator.MessageType.GetByMessageTypeID(messageTypeID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(type, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return type;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.MessageType GetByMessageTypeID_NoAccessCheck(int messageTypeID)
        {
            ECN_Framework_Entities.Communicator.MessageType type = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                type = ECN_Framework_DataLayer.Communicator.MessageType.GetByMessageTypeID(messageTypeID);
                scope.Complete();
            }

            return type;
        }

        public static ECN_Framework_Entities.Communicator.MessageType GetByMessageTypeID_NoAccessCheck_UseAmbientTransaction(int messageTypeID)
        {
            ECN_Framework_Entities.Communicator.MessageType type = null;
            using (TransactionScope scope = new TransactionScope())
            {
                type = ECN_Framework_DataLayer.Communicator.MessageType.GetByMessageTypeID(messageTypeID);
                scope.Complete();
            }

            return type;
        }


        public static List<ECN_Framework_Entities.Communicator.MessageType> GetActivePriority(bool isActive, bool isPriority, int baseChannelID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = new List<ECN_Framework_Entities.Communicator.MessageType>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                messageTypeList = ECN_Framework_DataLayer.Communicator.MessageType.GetActivePriority(isActive, isPriority, baseChannelID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(messageTypeList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return messageTypeList;
        }

        public static List<ECN_Framework_Entities.Communicator.MessageType> GetByBaseChannelID(int baseChannelID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = new List<ECN_Framework_Entities.Communicator.MessageType>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                messageTypeList = ECN_Framework_DataLayer.Communicator.MessageType.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }


            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(messageTypeList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return messageTypeList;
        }

        public static List<ECN_Framework_Entities.Communicator.MessageType> GetByBaseChannelID_NoAccessCheck(int baseChannelID)
        {
            List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = new List<ECN_Framework_Entities.Communicator.MessageType>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                messageTypeList = ECN_Framework_DataLayer.Communicator.MessageType.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }

            return messageTypeList;
        }

        //do not use, customer id is not valid for this object
        //public static List<ECN_Framework_Entities.Communicator.MessageType> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        //{
        //    List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = new List<ECN_Framework_Entities.Communicator.MessageType>();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        messageTypeList = ECN_Framework_DataLayer.Communicator.MessageType.GetByCustomerID(customerID);
        //        scope.Complete();
        //    }


        //    if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByBaseChannel(messageTypeList, user))
        //        throw new ECN_Framework_Common.Objects.SecurityException();

        //    return messageTypeList;
        //}

        //private static bool SecurityCheck(ECN_Framework_Entities.Communicator.MessageType type, KMPlatform.Entity.User user)
        //{
        //    if (type != null)
        //    {
        //        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {     
        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var custExists = custList.Where(x => x.CustomerID == customer.CustomerID);

        //            if (!custExists.Any())
        //                return false;
        //        }
        //        else if (type.BaseChannelID != customer.BaseChannelID.Value)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private static bool SecurityCheck(List<ECN_Framework_Entities.Communicator.MessageType> typeList, KMPlatform.Entity.User user)
        //{
        //    if (typeList != null)
        //    {
        //        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

        //        if (KM.Platform.User.IsChannelAdministrator(user))
        //        {
        //            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);

        //            var securityCheck = from ct in typeList
        //                                join c in custList on ct.CustomerID.Value equals c.CustomerID
        //                                select new { ct.MessageTypeID };

        //            if (securityCheck.Count() != typeList.Count)
        //                return false;
        //        }
        //        else
        //        {
        //            var securityCheck = from ct in typeList
        //                                where ct.BaseChannelID != customer.BaseChannelID.Value
        //                                select new { ct.MessageTypeID };

        //            if (securityCheck.Any())
        //                return false;
        //        }
        //    }
        //    return true;
        //}

        public static bool Exists(int messageTypeID, int baseChannelID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.MessageType.Exists(messageTypeID, baseChannelID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(string name, int baseChannelID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.MessageType.Exists(name, baseChannelID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int messageTypeID, string name, int baseChannelID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.MessageType.Exists(messageTypeID, name, baseChannelID);
                scope.Complete();
            }
            return exists;
        }

        public static int GetMaxSortOrder(int baseChannelID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.MessageType.GetMaxSortOrder(baseChannelID);
                scope.Complete();
            }
            return count; 
        }

        public static void UpdateSortOrder(int messageTypeID, int sortOrder, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
            
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.MessageType.UpdateSortOrder(messageTypeID, sortOrder, customer.BaseChannelID.Value, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int messageTypeID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();
            ECN_Framework_Entities.Accounts.Customer customer= ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

            if (Exists(messageTypeID, customer.BaseChannelID.Value))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Layout.MessageTypeUsedInLayout(messageTypeID))
                {
                    //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                    GetByMessageTypeID(messageTypeID, user);
                    if (!KM.Platform.User.IsChannelAdministrator(user) && !KM.Platform.User.IsSystemAdministrator(user))
                    {
                        if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                            throw new ECN_Framework_Common.Objects.SecurityException();
                    }

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_DataLayer.Communicator.MessageType.Delete(messageTypeID, customer.BaseChannelID.Value, user.UserID);
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "MessageType is used in layout(s)"));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "MessageType does not exist"));
                throw new ECNException(errorList);
            }
        }

        public static void Validate(ECN_Framework_Entities.Communicator.MessageType type)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (type.BaseChannelID == -1)
            {
                errorList.Add(new ECNError(Entity, Method, "BaseChannelID is invalid"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.BaseChannel.Exists(type.BaseChannelID))
                        errorList.Add(new ECNError(Entity, Method, "BaseChannelID is invalid"));;
                    scope.Complete();
                }

                if (type.Name.Trim() == string.Empty)
                    errorList.Add(new ECNError(Entity, Method, "Name cannot be empty"));
                else if (Exists(type.MessageTypeID, type.Name, type.BaseChannelID))
                    errorList.Add(new ECNError(Entity, Method, "Name already exists in this base channel"));
                else if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(type.Name))
                    errorList.Add(new ECNError(Entity, Method, "Name contains invalid characters"));

                if (type.Threshold != true && type.Threshold != false)
                    errorList.Add(new ECNError(Entity, Method, "Threshold is invalid"));

                if (type.Priority != true && type.Priority != false)
                    errorList.Add(new ECNError(Entity, Method, "Priority is invalid"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.MessageType type, KMPlatform.Entity.User user)
        {
            if (type.CreatedUserID == null && type.UpdatedUserID != null)
                type.CreatedUserID = type.UpdatedUserID;


            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (type.MessageTypeID > 0)
            {
                if (!Exists(type.MessageTypeID, type.BaseChannelID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "MessageTypeID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(type);

            type.CustomerID = user.CustomerID;

            if (!HasPermission(KMPlatform.Enums.Access.Edit, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(type, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope())
            {
                type.MessageTypeID = ECN_Framework_DataLayer.Communicator.MessageType.Save(type);
                scope.Complete();
            }
            return type.MessageTypeID; 
        }
    }
}
