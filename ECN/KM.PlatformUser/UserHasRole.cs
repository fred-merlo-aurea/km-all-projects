using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;

namespace KM.Platform
{
    /// <summary>
    /// Assertions library for authenticating a Knowledge Marketing Platform User
    /// in various ways.
    /// </summary>
    public static class User
    {
        /// <summary>
        /// Tests that user is non-null and has ActiveFlag set to "Y"
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsActive(KMPlatform.Entity.User user)
        {
            if (user == null)
            {
                return false;
            }
            return user.IsActive;
            //return null != user && (user.ActiveFlag.ToUpper() ?? "") == "Y";
        }

        /// <summary>
        /// Tests that customer is non-null and has ActiveFlag set to "Y"
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool IsActive(ECN_Framework_Entities.Accounts.Customer customer)
        {
            if (customer == null)
            {
                return false;
            }
            return (customer.ActiveFlag.ToUpper() ?? "") == "Y";
        }

        /// <summary>
        /// True if user IsActive and the current security group is called "system administrator"; was: 
        /// True if the user has IsSystemAdmin set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsSystemAdministrator(KMPlatform.Entity.User user)
        {
            if (false == IsActive(user))
            {
                return false;
            }
            return user.IsPlatformAdministrator;
        }

        /// <summary>
        /// True if user IsSystemAdministrator or IsActive and the current security group is called "Base-Channel Administrator"; was: 
        /// True if the user is a System Administrator or has IsChannelAdmin set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsChannelAdministrator(KMPlatform.Entity.User user)
        {
            if (IsSystemAdministrator(user))
            {
                return true;
            }
            else if (false == IsActive(user))
            {
                return false;
            }
            return user.CurrentSecurityGroup.AdministrativeLevel.Equals(KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator);
        }

        /// <summary>
        /// True if user IsChannelAdministrator or IsActive and the current security group is called "Customer Administrator"; was: 
        /// True if the user is a System Administrator, a Channel Administrator or has IsAdmin is set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAdministrator(KMPlatform.Entity.User user)
        {
            if (IsSystemAdministrator(user) || IsChannelAdministrator(user))
            {
                return true;
            }
            else if (false == IsActive(user))
            {
                return false;
            }
            return user.CurrentSecurityGroup.AdministrativeLevel.Equals(KMPlatform.Enums.SecurityGroupAdministrativeLevel.Administrator);
        }

         /// <summary>
        /// True if user is active and HasUserAccess is set to true
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool HasUserAdministrativePrivilege(KMPlatform.Entity.User user)
        {
            return IsActive(user) && IsAdministrator(user);
        }

        /// <summary>
        /// True if the user is System Administrator or Channel Administrator
        /// or is Administrator with User Administrative Privilege
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CanAdministrateUsers(KMPlatform.Entity.User user)
        {
            return IsChannelAdministrator(user) || (IsAdministrator(user) && HasUserAdministrativePrivilege(user));
        }

        public static bool HasService(KMPlatform.Entity.User user, KMPlatform.Enums.Services serviceCode)
        {
            try
            {
                if (false == IsActive(user))
                {
                    return false;
                }
                else if (user.CurrentClient.Services == null)
                {
                    return false;
                }
                else if (!user.CurrentClient.Services.Any(x => x.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
                else if (IsSystemAdministrator(user) || IsChannelAdministrator(user) || IsAdministrator(user))
                {
                    return true;
                }
                else if (user.CurrentSecurityGroup.Services == null)
                {
                    return false;
                }
                return user.CurrentSecurityGroup.Services.Any(a => a.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        public static bool HasServiceFeature(KMPlatform.Entity.User user, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode)
        {
            try
            {
                if (false == IsActive(user))
                {
                    return false;
                }
                else if (HasService(user, serviceCode))
                {
                    KMPlatform.Entity.Service s = user.CurrentClient.Services.Find(a => a.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase));

                    if (s == null)
                        return false;

                    if (!s.ServiceFeatures.Any(x => x.SFCode.Equals(servicefeatureCode.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        return false;
                    }
                    else if (IsSystemAdministrator(user) || IsChannelAdministrator(user) || IsAdministrator(user))
                    {
                        return true;
                    }

                    Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures> pk = Tuple.Create(serviceCode, servicefeatureCode);

                    return user.CurrentSecurityGroup.Permissions.ContainsKey(pk);
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;            
            }

        }

        public static bool HasAccess(KMPlatform.Entity.User user, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode, KMPlatform.Enums.Access accessCode)
        {
            try
            {
                if (HasServiceFeature(user, serviceCode, servicefeatureCode))
                {
                    if (IsSystemAdministrator(user) || IsChannelAdministrator(user) || IsAdministrator(user))
                    {
                        return true;
                    }

                    Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures> pk = Tuple.Create(serviceCode, servicefeatureCode);

                    if (false == user.CurrentSecurityGroup.Permissions.ContainsKey(pk))
                    {
                        return false;
                    }
                    return user.CurrentSecurityGroup.Permissions[pk].Any(x => x.Equals(accessCode));

                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        ///// <summary>
        ///// User must have all features mentioned in list
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="applicationName"></param>
        ///// <param name="features"></param>
        ///// <returns></returns>
        //public static bool HasAllProductFeatures(KMPlatform.Entity.User user, string applicationName, params string[] features)
        //{
        //    if (null == features)
        //    {
        //        return false;
        //    }
        //    else if (IsChannelAdministrator(user))
        //    {
        //        return true;
        //    }
        //    else if (false == HasAnyFeatures(user, applicationName))
        //    {
        //        return false;
        //    }
        //    return features.All(x => user.MenuFeatures[applicationName].Any(y => y.FeatureName.Equals(x, StringComparison.InvariantCultureIgnoreCase)));
        //}

        ///// <summary>
        ///// User must have at least one feature from each list
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="applicationName"></param>
        ///// <param name="features"></param>
        ///// <returns></returns>
        //public static bool HasAllProductFeatures(KMPlatform.Entity.User user, string applicationName, params IEnumerable<string>[] features)
        //{
        //    if (null == features)
        //    {
        //        return false;
        //    }
        //    else if (IsChannelAdministrator(user))
        //    {
        //        return true;
        //    }
        //    return features.All(f => HasProductFeature(user, applicationName, f.ToArray()));
        //}


        //public static bool IsAdministratorOrHasUserPermission(KMPlatform.Entity.User user, params ECN_Framework_Common.Objects.Accounts.Enums.UserPermission[] permissions)
        //{
        //    return IsAdministrator(user) || HasUserPermission(user, permissions);
        //}

        ///// <summary>true if <ul>
        ///// <li>both users are active, AND</li>
        ///// <li>master is a system administrator, or</li>
        ///// <li>user is not a system administrator and master and user are in the same channel and<ul>
        ///// <li>master is a channel administrator, or</li>
        ///// <li>user is not a channel administrator and master is an administrator with the Administrate Users 
        ///// permission and master and user are within the same customer</li></ul></li></ul></summary>
        ///// <param name="masterUser"></param>
        ///// <param name="masterCustomer"></param>
        ///// <param name="user"></param>
        ///// <param name="userCustomer"></param>
        ///// <returns>true if <ul>
        ///// <li>master is a system administrator, or</li>
        ///// <li>user is not a system administrator and master and user are in the same channel and<ul>
        ///// <li>master is a channel administrator, or</li>
        ///// <li>user is not a channel administrator and master is an administrator with the Administrate Users 
        ///// permission and master and user are within the same customer</li></ul></li></ul></returns>
        //public static bool CanImpersonate(KMPlatform.Entity.User masterUser, ECN_Framework_Entities.Accounts.Customer masterCustomer, KMPlatform.Entity.User user, ECN_Framework_Entities.Accounts.Customer userCustomer)
        //{
        //    // all parameters are required
        //    if (null == masterUser || null == masterCustomer || null == user || null == userCustomer)
        //    {
        //        return false;
        //    }

        //    // cannot impersonate an inactive user
        //    if (false == IsActive(user))
        //    {
        //        return false;
        //    }

        //    // otherwise, sys admin can do anything
        //    if (IsSystemAdministrator(masterUser))
        //    {
        //        return true;
        //    }

        //    // only allow channel change when master user is a sys admin
        //    if (false == IsSameChannel(masterCustomer, userCustomer))
        //    {
        //        return false;
        //    }

        //    // target user is a sys-admin but master user is not
        //    if (IsSystemAdministrator(user))
        //    {
        //        return false;
        //    }

        //    // allow channel admin to impersonate within a channel
        //    if (IsChannelAdministrator(masterUser))
        //    {
        //        return true;
        //    }

        //    // only allow customer change for channel (or system) admin
        //    if (false == IsSameCustomer(masterUser, user))
        //    {
        //        return false;
        //    }

        //    // target user is a channel admin but master user is not
        //    if (IsChannelAdministrator(user))
        //    {
        //        return false;
        //    }

        //    // master user must have Admin role and UserAdmin privilege
        //    if (CanAdministrateUsers(masterUser))
        //    {
        //        return true;
        //    }

        //    // master user does not have sufficient access for impersonation
        //    return false;
        //}

        //public static bool IsAdministratorOrHasProductFeature(KMPlatform.Entity.User user, string applicationName, params string[] features)
        //{
        //    if (IsAdministrator(user))
        //    {
        //        return true;
        //    }
        //    return HasProductFeature(user, applicationName, features);
        //}



        ///// <summary>
        ///// True if firstCustomer and otherOther customer are both non-null and have the
        ///// same value for BaseChannelID
        ///// </summary>
        ///// <param name="masterCustomer"></param>
        ///// <param name="customer"></param>
        ///// <returns></returns>
        //public static bool IsSameChannel(ECN_Framework_Entities.Accounts.Customer masterCustomer, ECN_Framework_Entities.Accounts.Customer customer)
        //{
        //    return IsActive(masterCustomer) && IsActive(customer) && masterCustomer.BaseChannelID == customer.BaseChannelID;
        //}

        ///// <summary>
        ///// True if both users are active and have the same CustomerID
        ///// </summary>
        ///// <param name="masterUser"></param>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //public static bool IsSameCustomer(KMPlatform.Entity.User masterUser, KMPlatform.Entity.User user)
        //{
        //    return IsActive(masterUser) && IsActive(user) && masterUser.CustomerID == user.CustomerID;
        //}

        ///// <summary>
        ///// True if authenticationTicket provides a valid (greater than zero) MasterUserID 
        ///// </summary>
        ///// <param name="authenticationTicket"></param>
        ///// <returns></returns>
        //public static bool IsChannelMaster(ECN_Framework_Entities.Application.AuthenticationTicket authenticationTicket)
        //{
        //    return authenticationTicket != null && authenticationTicket.CurrentClientID != authenticationTicket.DefaultClientID;
        //}


        ///// <summary>
        ///// True if both customer and channel reference the same BaseChannelID
        ///// </summary>
        ///// <param name="customer"></param>
        ///// <param name="channel"></param>
        ///// <returns></returns>
        //public static bool IsAdministratedChannel(ECN_Framework_Entities.Accounts.Customer customer, ECN_Framework_Entities.Accounts.BaseChannel channel)
        //{
        //    return null != customer && null != channel && customer.BaseChannelID == channel.BaseChannelID;
        //}

        //public static bool HasUserPermission(KMPlatform.Entity.User user, params ECN_Framework_Common.Objects.Accounts.Enums.UserPermission[] permissions)
        //{
        //    return IsChannelAdministrator(user) || (IsActive(user) && HasApplication(user, "Communicator") && user.MenuFeatures["Communicator"].Any(x => permissions.Any(y => x.FeatureName.Equals(y.ToString().Replace("_", " "), StringComparison.InvariantCultureIgnoreCase))));
        //}

        //public static bool HasEntityRights(KMPlatform.Entity.User user, ECN_Framework_Common.Objects.Communicator.Enums.EntityRights rights)
        //{
        //    throw new NotImplementedException();
        //}

        //public static bool HasApplicationMenuFeature(KMPlatform.Entity.User user, string applicationName, string menuFeature)
       // {
       //     if (IsSystemAdministrator(user))
       //     {
       //         return true;
       //     }
       //     else if (false == IsActive(user))
       //     {
       //         return false;
       //     }
       //     else if (false == HasApplication(user, applicationName))
       //     {
       //         return false;
       //     }
       //     return user.MenuFeatures[applicationName].Any(x => x.FeatureName.Equals(menuFeature, StringComparison.InvariantCultureIgnoreCase));
       // }

        //public static bool HasServiceFeature3(KMPlatform.Entity.User user, ECN_Framework_Common.Objects.Enums.Entity entity, object up)
        //{
            
        //    if (IsSystemAdministrator(user))
        //    {
        //        return true;
        //    }
        //    else if (false == IsActive(user))
        //    {
        //        return false;
        //    }
        //    string entityName = entity.ToString();
        //    string userPermission = up.ToString();

        //    return user.Services != null && user.Services.Any(x => x.ServiceName.Equals(entityName, StringComparison.InvariantCultureIgnoreCase) && x.ServiceFeatures != null && x.ServiceFeatures.Any(y => y.SFCode.Equals(userPermission, StringComparison.InvariantCultureIgnoreCase)));
        //}

        //public static bool HasServiceFeature2(KMPlatform.Entity.User user, ECN_Framework_Common.Objects.Enums.Entity entity, params ECN_Framework_Common.Objects.Communicator.Enums.EntityRights[] perms )
        //{

        //    if (IsSystemAdministrator(user))
        //    {
        //        return true;
        //    }
        //    else if (false == IsActive(user))
        //    {
        //        return false;
        //    }
        //    string entityName = entity.ToString();

        //    return user.Services != null && user.Services.Any(x => x.ServiceName.Equals(entityName, StringComparison.InvariantCultureIgnoreCase) && x.ServiceFeatures != null && x.ServiceFeatures.Any(y => perms.Any(z => y.SFCode.Equals(z.ToString(), StringComparison.InvariantCultureIgnoreCase))));
        //}
    }
}
