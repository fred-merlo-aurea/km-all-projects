using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Transactions;
using ECN.Framework.BusinessLayer.Helpers;
using ECN.Framework.BusinessLayer.Interfaces;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class AccessCheck
    {
        private const string CustomerIDPropertyName = "CustomerID";
        private const string BaseChannelID = "BaseChannelID";
        private static IUser _user;
        private static ICustomer _customer;

        static AccessCheck()
        {
            _user = new UserAdapter();
            _customer = new CustomerAdapter();
        }

        public static void Initialize(IUser user, ICustomer customer)
        {
            _user = user;
            _customer = customer;
        }

        /// <summary>
        /// use this method if there are no Service/servicefeature/access tied to the entity (for example : code table)
        /// (or) if HasAccess was checked prior to this call this method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toCheck"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CanAccessByCustomer<T>(T toCheck, KMPlatform.Entity.User user)
        {
            if (!_user.IsSystemAdministrator(user) && toCheck != null)
            {
                if (toCheck.GetType().IsGenericType)
                {
                    var newList = CreateList(toCheck);
                    return CanAccessByCustomerList(newList, user);
                }
                else
                {
                    var customerID = GetCustomerID(toCheck);
                    if (customerID == null)
                    {
                        return false;
                    }
                    else if (_user.IsChannelAdministrator(user))
                    {
                        return IsCustomerExists(user, customerID);
                    }
                    else if (customerID.Value != user.CustomerID)
                    {
                        var valid = false;
                        using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            var customer = Accounts.Customer.GetByCustomerID(customerID.Value, false);
                            if (user.UserClientSecurityGroupMaps.Exists(x => 
                                x.ClientID == customer.PlatformClientID && x.IsActive))
                            {
                                valid = true;
                            }
                            scope.Complete();
                        }
                        return valid;
                    }
                }
            }

            return true;
        }

        private static bool IsCustomerExists(User user, int? customerID)
        {
            IList<Customer> customerList;
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                var customer = _customer.GetByCustomerID(user.CustomerID, false);
                customerList = _customer.GetByBaseChannelID(customer.BaseChannelID.Value);
                scope.Complete();
            }
            var custExists = customerList.Where(x => x.CustomerID == customerID.Value);
            return custExists.Any();
        }

        private static int? GetCustomerID<T>(T toCheck)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.Name == CustomerIDPropertyName)
                {
                    return prop.GetValue(toCheck) as int?;
                }
            }

            return null;
        }

        private static dynamic CreateList<T>(T toCheck)
        {
            var listType = toCheck.GetType();
            var objectType = listType.GetGenericArguments().Any()
                ? listType.GetGenericArguments()[0]
                : null;

            var newList = Activator.CreateInstance(typeof(List<>).MakeGenericType(objectType));
            newList = toCheck;
            return newList;
        }

        private static bool CanAccessByCustomerList<T>(IList<T> toCheckList, KMPlatform.Entity.User user)
        {
            if (toCheckList != null && !KM.Platform.User.IsSystemAdministrator(user))
            {
                if (toCheckList.Count == 0)
                    return true;

                //if (up == ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.NoCheck || ECN_Framework_BusinessLayer.Accounts.User.HasPermission(user.UserID, up.ToString()) || user.IsChannelAdmin || user.IsAdmin)

                List<int> listCustomers = new List<int>();
                foreach (T toCheck in toCheckList)
                {

                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                    foreach (PropertyDescriptor prop in properties)
                    {
                        if (prop.Name == "CustomerID")
                        {
                            int customerID = (int)prop.GetValue(toCheck);
                            if (!listCustomers.Contains(customerID))
                            {
                                listCustomers.Add(customerID);
                            }
                            break;
                        }
                    }
                }

                if (listCustomers.Count == 0)
                    return false;
                else
                {
                    if (KM.Platform.User.IsChannelAdministrator(user))
                    {
                        ECN_Framework_Entities.Accounts.Customer customer; ;
                        List<ECN_Framework_Entities.Accounts.Customer> custList;
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
                            custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);
                            scope.Complete();
                        }

                        List<ECN_Framework_Entities.Accounts.Customer> checkList = custList.Where(x => listCustomers.Contains(x.CustomerID)).ToList();// 

                        if (checkList.Count != listCustomers.Count)
                            return false;
                    }
                    else
                    {
                        if (listCustomers.Count > 1 || listCustomers[0] != user.CustomerID)
                            return false;
                    }
                }
            }
            return true;
        }

        internal static bool CanAccessByBaseChannel<T>(T toCheck, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.IsSystemAdministrator(user) && toCheck != null)
            {
                if (toCheck.GetType().IsGenericType)
                {
                    Type listType = toCheck.GetType();
                    Type objectType = listType.GetGenericArguments()[0];
                    dynamic newList = Activator.CreateInstance(typeof(List<>).MakeGenericType(objectType));
                    newList = toCheck;
                    return CanAccessByBaseChannelList(newList, user);
                }
                else
                {
                    //if (up == ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.NoCheck || ECN_Framework_BusinessLayer.Accounts.User.HasPermission(user.UserID, up.ToString()) || user.IsChannelAdmin || user.IsAdmin)
                    
                        int? baseChannelID = null;
                        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                        foreach (PropertyDescriptor prop in properties)
                        {
                            if (prop.Name == "BaseChannelID")
                            {
                                baseChannelID = (int)prop.GetValue(toCheck);
                                break;
                            }
                        }

                        if (baseChannelID == null)
                            return false;
                        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
                        if (baseChannelID.Value != customer.BaseChannelID.Value)
                            return false;
                    
                }
            }
            return true;
        }

        private static bool CanAccessByBaseChannelList<T>(IList<T> toCheckList, User user)
        {
            if (toCheckList != null && !KM.Platform.User.IsSystemAdministrator(user))
            {
                if (toCheckList.Count == 0)
                {
                    return true;
                }

                return VerifyAccessByBaseChannelList(toCheckList, user);
            }

            return true;
        }


        public static bool CanAccessByCustomer<T>(T toCheck, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode, KMPlatform.Enums.Access accessCode, KMPlatform.Entity.User user)
        {
            if (!_user.IsSystemAdministrator(user) && toCheck != null)
            {
                if (toCheck.GetType().IsGenericType)
                {
                    var newList = CreateList(toCheck);
                    return CanAccessByCustomerList(newList, serviceCode, servicefeatureCode, accessCode, user);
                }
                else
                {
                    if (_user.HasAccess(user, serviceCode, servicefeatureCode, accessCode))
                    {
                        var customerID = GetCustomerID(toCheck);
                        if (customerID == null)
                        {
                            return false;
                        }
                        else if (_user.IsChannelAdministrator(user))
                        {
                            return IsCustomerExists(user, customerID);
                        }
                        else if (customerID.Value != user.CustomerID)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool CanAccessByCustomerList<T>(IList<T> toCheckList, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode, KMPlatform.Enums.Access accessCode, KMPlatform.Entity.User user)
        {
            if (toCheckList != null && !KM.Platform.User.IsSystemAdministrator(user))
            {
                if (toCheckList.Count == 0)
                    return true;

                //if (up == ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.NoCheck || ECN_Framework_BusinessLayer.Accounts.User.HasPermission(user.UserID, up.ToString()) || user.IsChannelAdmin || user.IsAdmin)
                if (KM.Platform.User.HasAccess(user, serviceCode, servicefeatureCode, accessCode))
                {
                    List<int> listCustomers = new List<int>();
                    foreach (T toCheck in toCheckList)
                    {
                        
                        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                        foreach (PropertyDescriptor prop in properties)
                        {
                            if (prop.Name == "CustomerID")
                            {
                                int customerID = (int)prop.GetValue(toCheck);
                                if (!listCustomers.Contains(customerID))
                                {
                                    listCustomers.Add(customerID);
                                }
                                break;
                            }
                        }
                    }

                    if (listCustomers.Count == 0)
                        return false;
                    else
                    {
                        if (KM.Platform.User.IsChannelAdministrator(user))
                        {
                            ECN_Framework_Entities.Accounts.Customer customer; ;
                            List<ECN_Framework_Entities.Accounts.Customer> custList;
                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                            {
                                customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
                                custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(customer.BaseChannelID.Value);
                                scope.Complete();
                            }

                            List<ECN_Framework_Entities.Accounts.Customer> checkList = custList.Where(x => listCustomers.Contains(x.CustomerID)).ToList();// 

                            if (checkList.Count != listCustomers.Count)
                                return false;
                        }
                        else
                        {
                            if (listCustomers.Count > 1 || listCustomers[0] != user.CustomerID)
                                return false;
                        }
                    }
                }
                else
                    return false;
            }
            return true;
        }

        internal static bool CanAccessByBaseChannel<T>(T toCheck, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode, KMPlatform.Enums.Access accessCode, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.IsSystemAdministrator(user) && toCheck != null)
            {
                if (toCheck.GetType().IsGenericType)
                {
                    Type listType = toCheck.GetType();
                    Type objectType = listType.GetGenericArguments()[0];
                    dynamic newList = Activator.CreateInstance(typeof(List<>).MakeGenericType(objectType));
                    newList = toCheck;
                    return CanAccessByBaseChannelList(newList, serviceCode, servicefeatureCode, accessCode, user);
                }
                else
                {
                    //if (up == ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.NoCheck || ECN_Framework_BusinessLayer.Accounts.User.HasPermission(user.UserID, up.ToString()) || user.IsChannelAdmin || user.IsAdmin)
                    if (KM.Platform.User.HasAccess(user, serviceCode, servicefeatureCode, accessCode))
                    {
                        int? baseChannelID = null;
                        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                        foreach (PropertyDescriptor prop in properties)
                        {
                            if (prop.Name == "BaseChannelID")
                            {
                                baseChannelID = (int)prop.GetValue(toCheck);
                                break;
                            }
                        }

                        if (baseChannelID == null)
                            return false;
                        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
                        if (baseChannelID.Value != customer.BaseChannelID.Value)
                            return false;
                    }
                    else
                        return false;
                }
            }
            return true;
        }

        private static bool CanAccessByBaseChannelList<T>(IList<T> toCheckList, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode, KMPlatform.Enums.Access accessCode, User user)
        {
            if (toCheckList != null && !KM.Platform.User.IsSystemAdministrator(user))
            {
                if (toCheckList.Count == 0)
                {
                    return true;
                }

                if (KM.Platform.User.HasAccess(user, serviceCode, servicefeatureCode, accessCode))
                {
                    return VerifyAccessByBaseChannelList(toCheckList, user);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static bool VerifyAccessByBaseChannelList<T>(IList<T> toCheckList, User user)
        {
            var listBaseChannels = new List<int>();
            foreach (T toCheck in toCheckList)
            {
                var properties = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.Name == BaseChannelID)
                    {
                        var baseChannelID = (int)prop.GetValue(toCheck);
                        if (!listBaseChannels.Contains(baseChannelID))
                        {
                            listBaseChannels.Add(baseChannelID);
                        }
                        break;
                    }
                }
            }

            if (listBaseChannels.Count == 0)
            {
                return false;
            }
            else
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var customer = Accounts.Customer.GetByCustomerID(user.CustomerID, false);
                    if (!listBaseChannels.Contains(customer.BaseChannelID.Value))
                    {
                        return false;
                    }
                    scope.Complete();
                }
            }

            return true;
        }
    }
}
