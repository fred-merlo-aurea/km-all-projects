using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class Customer
    {
        private static readonly string Customer_CacheName = "CACHE_CUSTOMER_";
        private static readonly string BaseChannel_CustomerList_CacheName = "CACHE_BASECHANNEL_CUSTOMERLIST_";
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Customer;

        public static bool Exists(int customerID)
        {
            return ECN_Framework_DataLayer.Accounts.Customer.Exists(customerID);
        }

        public static bool Exists(string customerName, int customerID)
        {
            return ECN_Framework_DataLayer.Accounts.Customer.Exists(customerName,customerID);
        }

        public static ECN_Framework_Entities.Accounts.Customer GetByCustomerID(int customerID, bool getChildren)
        {
            ECN_Framework_Entities.Accounts.Customer customer = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                customer = ECN_Framework_DataLayer.Accounts.Customer.GetByCustomerID(customerID);
                scope.Complete();
            }
            if (customer != null && getChildren)
            {
                //campaign.CampaignItems = CampaignItem.GetByCampaignID(campaignID, customerID, getChildren);
            }
            if (customer == null || customer.CustomerID != customerID)
            {
                customer = null;
                throw new SecurityException("SECURITY VIOLATION!");
            }
            return customer;
        }

        public static ECN_Framework_Entities.Accounts.Customer GetByCustomerID(int customerID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Accounts.Customer customer = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                customer = GetByCustomerID(customerID, false);
                scope.Complete();
            }
            if (customer != null && getChildren)
            {
                customer.BillingContact = Contact.GetByCustomerID(customerID, user);
            }
            return customer;
        }

        public static ECN_Framework_Entities.Accounts.Customer GetByUserID(int userID, int customerID, bool getChildren)
        {
            ECN_Framework_Entities.Accounts.Customer customer = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                customer = ECN_Framework_DataLayer.Accounts.Customer.GetByUserID(userID, customerID);
                scope.Complete();
            }
            if (customer != null && getChildren)
            {
                //campaign.CampaignItems = CampaignItem.GetByCampaignID(campaignID, customerID, getChildren);
            }
            if (customer == null || customer.CustomerID != customerID)
            {
                customer = null;
                throw new SecurityException("SECURITY VIOLATION!");
            }
            return customer;
        }

        public static ECN_Framework_Entities.Accounts.Customer GetByClientID(int clientID, bool getChildren)
        {
            ECN_Framework_Entities.Accounts.Customer customer = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                customer = ECN_Framework_DataLayer.Accounts.Customer.GetByClientID(clientID);
                scope.Complete();
            }
            if (customer != null && getChildren)
            {
                //campaign.CampaignItems = CampaignItem.GetByCampaignID(campaignID, customerID, getChildren);
            }
            if (customer == null)
            {
                customer = null;
                throw new SecurityException("SECURITY VIOLATION!");
            }
            return customer;
        }


        public static List<ECN_Framework_Entities.Accounts.Customer> GetByBaseChannelID(int baseChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> customerList = new List<ECN_Framework_Entities.Accounts.Customer>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                customerList = ECN_Framework_DataLayer.Accounts.Customer.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }
            return customerList;
        }

        public static bool IsAuthorized(int customerID, string product)
        {
            int authLevel = 0;
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_DataLayer.Accounts.Customer.GetByCustomerID(customerID);
            if (customer == null || customer.CustomerID != customerID)
            {
                customer = null;
                throw new SecurityException("SECURITY VIOLATION!");
            }
            try
            {
                switch (product)
                {
                    case "communicator":
                        authLevel = Convert.ToInt32(customer.CommunicatorLevel);
                        break;
                    case "collector":
                        authLevel = Convert.ToInt32(customer.CollectorLevel);
                        break;
                    case "creator":
                        authLevel = Convert.ToInt32(customer.CreatorLevel);
                        break;
                }
            }
            catch (FormatException fe)
            {
                string devnull = fe.ToString();
                authLevel = 0;
            }
            return authLevel > 0;
        }

        public static void ClearCustomersCache_ByChannelID(int baseChannelID)
        {
            string cacheKey = BaseChannel_CustomerList_CacheName + baseChannelID.ToString();
            ECN_Framework_Common.Functions.CacheHelper.ClearCache(cacheKey);
        }

        public static List<ECN_Framework_Entities.Accounts.Customer> GetCustomersByChannelID(int baseChannelID)
        {
            string cacheKey = BaseChannel_CustomerList_CacheName + baseChannelID.ToString();

            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled())
            {
                return GetByBaseChannelID(baseChannelID);
            }
            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(cacheKey) == null)
            {
                List<ECN_Framework_Entities.Accounts.Customer> customers = GetByBaseChannelID(baseChannelID);
                ECN_Framework_Common.Functions.CacheHelper.AddToCache(cacheKey, customers);
                return customers;
            }
            else
            {
                return (List<ECN_Framework_Entities.Accounts.Customer>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(cacheKey);
            }
        }

        public static bool HasProductFeature(int customerID, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode)
        {
            ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, false);

            return KMPlatform.BusinessLogic.Client.HasServiceFeature(c.PlatformClientID, serviceCode, servicefeatureCode);

            #region OLD ECN customer code //SUNIL // 10/27/2015
            //try
            //{
            //    SqlCommand cmdhasProductFeature = new SqlCommand("e_ProductFeature_ProductName_FeatureName_CustomerID");
            //    cmdhasProductFeature.CommandType = CommandType.StoredProcedure;
            //    cmdhasProductFeature.Parameters.AddWithValue("@ProductName", product_name);
            //    cmdhasProductFeature.Parameters.AddWithValue("@FeatureName", feature_name);
            //    cmdhasProductFeature.Parameters.AddWithValue("@customerID", customerID);
            //    string my_answer = ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(cmdhasProductFeature, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()).ToString();

            //    if (my_answer.Equals("y"))
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            //catch
            //{
            //    return false;
            //}
            #endregion
        }

        public static void Validate(ECN_Framework_Entities.Accounts.Customer customer, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
            using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (!KM.Platform.User.IsSystemAdministrator(user) && KM.Platform.User.IsChannelAdministrator(user))
                {
                    if (customer.BaseChannelID != ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false).BaseChannelID)
                    {
                        throw new ECN_Framework_Common.Objects.SecurityException();
                    }
                }
                supressscope.Complete();
            }

            if (customer.CustomerID <= 0  && customer.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (customer.CustomerID > 0 && customer.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (string.IsNullOrWhiteSpace(customer.CustomerName))
                errorList.Add(new ECNError(Entity, Method, "Customer Name is missing"));
            else if (ECN_Framework_BusinessLayer.Accounts.Customer.Exists(customer.CustomerName, customer.CustomerID))
                errorList.Add(new ECNError(Entity, Method, "Customer Name already exists"));

            if (customer.GeneralContant == null)
                errorList.Add(new ECNError(Entity, Method, "General Contact is missing"));

            if (customer.BillingContact == null)
                errorList.Add(new ECNError(Entity, Method, "Billing Contact is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.FirstName))
                errorList.Add(new ECNError(Entity, Method, "First Name is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.LastName))
                errorList.Add(new ECNError(Entity, Method, "Last Name is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.ContactTitle))
                errorList.Add(new ECNError(Entity, Method, "Title is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.Phone))
                errorList.Add(new ECNError(Entity, Method, "Phone is missing"));

            //if (string.IsNullOrWhiteSpace(customer.GeneralContant.Fax))
            //    errorList.Add(new ECNError(Entity, Method, "Fax is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.Email))
                errorList.Add(new ECNError(Entity, Method, "Email is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.StreetAddress))
                errorList.Add(new ECNError(Entity, Method, "Address is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.City))
                errorList.Add(new ECNError(Entity, Method, "City is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.Zip))
                errorList.Add(new ECNError(Entity, Method, "Zip is missing"));

            if (string.IsNullOrWhiteSpace(customer.GeneralContant.Country))
                errorList.Add(new ECNError(Entity, Method, "Country is missing"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Accounts.Customer customer, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Functions.CacheHelper.ClearCache(BaseChannel_CustomerList_CacheName);
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;

            if (customer.CustomerID > 0)
            {
                if (!Exists(customer.CustomerID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    throw new ECNException(errorList);
                }
            }

            bool Update = false;
            Validate(customer, user);

            using (TransactionScope scope = new TransactionScope())
            {
                if (customer.CustomerID > 0)
                {
                    Update = true;
                }

                customer.CustomerID = ECN_Framework_DataLayer.Accounts.Customer.Save(customer);
                if (customer.BillingContact != null)
                {
                    ECN_Framework_Entities.Accounts.Contact contact = new ECN_Framework_Entities.Accounts.Contact();
                    contact = customer.BillingContact;
                    contact.CustomerID = customer.CustomerID;
                    contact.CreatedUserID = user.UserID;
                    ECN_Framework_BusinessLayer.Accounts.Contact.Save(contact, user);
                }

                //CreateDefaultFeatures

                //List<ECN_Framework_Entities.Accounts.ProductDetail> lProductDetail = ECN_Framework_BusinessLayer.Accounts.ProductDetail.GetAll();
                //foreach (ECN_Framework_Entities.Accounts.ProductDetail item in lProductDetail)
                //{
                //    if (!CustomerProduct.Exists(item.ProductDetailID, customer.CustomerID))
                //    {
                //        ECN_Framework_Entities.Accounts.CustomerProduct customerProduct = new ECN_Framework_Entities.Accounts.CustomerProduct();
                //        customerProduct.CustomerID = customer.CustomerID;
                //        customerProduct.ProductDetailID = item.ProductDetailID;
                //        customerProduct.Active = "n";
                //        customerProduct.CreatedUserID = user.UserID;

                //        CustomerProduct.Save(customerProduct, user);
                //    }
                //}

                if (!Update)
                {
                    // CreateDefaulRole
                    //Commenting this out because we're using security groups Jwelter 12/28/2015
                    //ECN_Framework_Entities.Accounts.Role role = new ECN_Framework_Entities.Accounts.Role();
                    //role.CustomerID = customer.CustomerID;
                    //role.RoleName = "Everything";
                    //role.CreatedUserID = user.UserID;

                    //role.RoleID = ECN_Framework_BusinessLayer.Accounts.Role.Save(role, user);

                    //List<ECN_Framework_Entities.Accounts.Action> laction = ECN_Framework_BusinessLayer.Accounts.Action.GetAll();
                    //foreach (ECN_Framework_Entities.Accounts.Action item in laction)
                    //{
                    //    ECN_Framework_Entities.Accounts.RoleAction roleAction = new ECN_Framework_Entities.Accounts.RoleAction();
                    //    roleAction.RoleID = role.RoleID;
                    //    roleAction.ActionID = item.ActionID;
                    //    roleAction.Active = "Y";

                    //    if (item.ActionCode.Trim().ToLower().Equals("approvalblast"))
                    //        roleAction.Active = "N";

                    //    if (item.ActionCode.Trim().ToLower().Equals("ecnwiz"))
                    //        roleAction.Active = "N";

                    //    if (item.ActionCode.Trim().ToLower().Equals("msgwiz"))
                    //        roleAction.Active = "N";

                    //    if (item.ActionCode.Trim().ToLower().Equals("maf"))
                    //        roleAction.Active = "N";

                    //    if (item.ActionCode.Trim().ToLower().Equals("wqt"))
                    //        roleAction.Active = "N";

                    //    roleAction.CreatedUserID = user.UserID;
                    //    RoleAction.Save(roleAction, user);
                    //}

                    //CreateMasterSupressionGroup

                    using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
                        group.CustomerID = customer.CustomerID;
                        group.GroupName = "Master Suppression";
                        group.OwnerTypeCode = "customer";
                        group.MasterSupression = 1;
                        group.CreatedUserID = user.UserID;
                        group.PublicFolder = 0;
                        group.FolderID = 0;
                        group.AllowUDFHistory = "N";
                        group.IsSeedList = false;

                        ECN_Framework_BusinessLayer.Communicator.Group.Save(group, user);

                        supressscope.Complete();
                    }
                }

                scope.Complete();
            }

            //sunil - TODO - 10/23/2015
            //#region create FormsUser as Customer Admin
            //if (!Update)
            //{
            //    KMPlatform.Entity.User u = new KMPlatform.Entity.User();
            //    u.UserName = "F0rm5U5er";
            //    u.Password = System.Guid.NewGuid().ToString().Substring(0, 7);
            //    u.ActiveFlag = "Y";
            //    u.IsChannelAdmin = false;
            //    u.IsAdmin = true;
            //    u.IsSysAdmin = false;
            //    u.CustomerID = customer.CustomerID;
            //    ECN_Framework_Entities.Accounts.Role role = ECN_Framework_BusinessLayer.Accounts.Role.GetByCustomerID(customer.CustomerID, false).Where(s => (s.RoleName == "Everything")).FirstOrDefault();
            //    u.RoleID = role.RoleID;
            //    u.AccountsOptions = "001000";
            //    u.CommunicatorOptions = "000";
            //    u.CreatorOptions = "000";
            //    u.CollectorOptions = "000";
            //    u.AcceptTermsDate = System.DateTime.Now;
            //    u.CreatedUserID = user.UserID;

            //    for (int i = 0; i < 15; i++)
            //    {
            //        ECN_Framework_Entities.Accounts.UserAction ua = new ECN_Framework_Entities.Accounts.UserAction();
            //        ua.UserID = -1;
            //        ua.UserActionID = 0;
            //        switch (i)
            //        {
            //            case 0:
            //                ua.ActionID = 15;
            //                break;
            //            case 1:
            //                ua.ActionID = 14;
            //                break;
            //            case 2:
            //                ua.ActionID = 13;
            //                break;
            //            case 3:
            //                ua.ActionID = 4;
            //                break;
            //            case 4:
            //                ua.ActionID = 5;
            //                break;
            //            case 5:
            //                ua.ActionID = 6;
            //                break;
            //            case 6:
            //                ua.ActionID = 7;
            //                break;
            //            case 7:
            //                ua.ActionID = 8;
            //                break;
            //            case 8:
            //                ua.ActionID = 9;
            //                break;
            //            case 9:
            //                ua.ActionID = 10;
            //                break;
            //            case 10:
            //                ua.ActionID = 11;
            //                break;
            //            case 11:
            //                ua.ActionID = 18;
            //                break;
            //            case 12:
            //                ua.ActionID = 12;
            //                break;
            //            case 13:
            //                ua.ActionID = 16;
            //                break;
            //            case 14:
            //                ua.ActionID = 17;
            //                break;
            //            default:
            //                break;
            //        }
            //        ua.Active = "Y";
            //        u.UserAction.Add(ua);
            //    }
            //    KMPlatform.BusinessLogic.User.Save(u, user, customer.BaseChannelID.Value);
            //}
            //#endregion  create FormsUser as Customer Admin

            return customer.CustomerID;
        }
     }
}
