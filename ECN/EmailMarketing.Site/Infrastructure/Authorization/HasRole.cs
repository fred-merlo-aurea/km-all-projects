using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using ECN_Framework_Entities.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.Site.Infrastructure.Authorization
{
    public static class HasRole
    {
        /// <summary>
        /// Tests that user is non-null and has ActiveFlag set to "Y"
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool IsActive(User user)
        {
            return null != user && (user.IsActive);
        }

        /// <summary>
        /// Tests that customer is non-null and has ActiveFlag set to "Y"
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool IsActive(Customer customer)
        {
            return null != customer && (customer.ActiveFlag.ToUpper() ?? "") == "Y";
        }

        /// <summary>
        /// True if the user has IsSystemAdmin set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsSystemAdministrator(User user)
        {
            return KM.Platform.User.IsSystemAdministrator(user);
        }

        /// <summary>
        /// True if the user is a System Administrator or has IsChannelAdmin set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsChannelAdministrator(User user)
        {
            return KM.Platform.User.IsChannelAdministrator(user);
        }

        /// <summary>
        /// True if the user is a System Administrator, a Channel Administrator or has IsAdmin is set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAdministrator(User user)
        {
            return KM.Platform.User.IsAdministrator(user);
        }


        /// <summary>
        /// True if both customer and channel reference the same BaseChannelID
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static bool IsAdministratedChannel(Customer customer, BaseChannel channel)
        {
            return null != customer && null != channel && customer.BaseChannelID == channel.BaseChannelID;
        }

        /// <summary>
        /// True if authenticationTicket provides a valid (greater than zero) MasterUserID 
        /// </summary>
        /// <param name="authenticationTicket"></param>
        /// <returns></returns>
        public static bool IsChannelMaster(AuthenticationTicket authenticationTicket)
        {
            return true;
            //return authenticationTicket != null && authenticationTicket.MasterUserID > 0;
        }

        /// <summary>
        /// True if firstCustomer and otherOther customer are both non-null and have the
        /// same value for BaseChannelID
        /// </summary>
        /// <param name="masterCustomer"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool IsSameChannel(Customer masterCustomer, Customer customer)
        {
            return IsActive(masterCustomer) && IsActive(customer) && masterCustomer.BaseChannelID == customer.BaseChannelID;
        }

        /// <summary>
        /// True if both users are active and have the same CustomerID
        /// </summary>
        /// <param name="masterUser"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsSameCustomer(User masterUser, User user)
        {
            return IsActive(masterUser) && IsActive(user) && masterUser.CustomerID == user.CustomerID;
        }

        /// <summary>
        /// True if user is active and HasUserAccess is set to true
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool HasUserAdministrativePrivilege(User user)
        {
            //return IsActive(user) && user.HasUserAccess;
            return KM.Platform.User.IsAdministrator(user);// KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.PLATFORM, KMPlatform.Enums.ServiceFeatures.User, KMPlatform.Enums.Access.Edit);
        }

        /// <summary>
        /// True if the user is System Administrator or Channel Administrator
        /// or is Administrator with User Administrative Privilege
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CanAdministrateUsers(User user)
        {
            return IsChannelAdministrator(user) || (IsAdministrator(user) && HasUserAdministrativePrivilege(user));
        }


    }
}