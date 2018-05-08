using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailMarketing.Site.Infrastructure.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EcnUser = KMPlatform.Entity.User;
using EcnCustomer = ECN_Framework_Entities.Accounts.Customer;
using EcnUserManager = ECN_Framework_BusinessLayer.Accounts.User;
using EcnCustomerManager = ECN_Framework_BusinessLayer.Accounts.Customer;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Application;

namespace EmailMarketing.Site.Infrastructure.Authorization.Tests
{
    [TestClass]
    public class HasRoleTests
    {
        #region IsActive
        [TestMethod]
        public void IsActiveUserTest_Null()
        {
            Assert.IsFalse(HasRole.IsActive((EcnUser)null));
        }

        [TestMethod]
        public void IsActiveUserTest_Inactive()
        {
            EcnUser u = new EcnUser { ActiveFlag = "N" };
            Assert.IsFalse(HasRole.IsActive(u));
        }

        [TestMethod]
        public void IsActiveUserTest_Active()
        {
            EcnUser u = new EcnUser { ActiveFlag = "Y" };
            Assert.IsTrue(HasRole.IsActive(u));
        }

        [TestMethod]
        public void IsActiveUserTest_Active_LowerCase()
        {
            EcnUser u = new EcnUser { ActiveFlag = "y" };
            Assert.IsTrue(HasRole.IsActive(u));
        }

        [TestMethod]
        public void IsActiveCustomerTest_Null()
        {
            Assert.IsFalse(HasRole.IsActive((EcnCustomer)null));
        }

        [TestMethod]
        public void IsActiveCustomerTest_Inactive()
        {
            EcnCustomer c = new EcnCustomer { ActiveFlag = "N" };
            Assert.IsFalse(HasRole.IsActive(c));
        }

        [TestMethod]
        public void IsActiveCustomerTest_Active()
        {
            EcnCustomer c = new EcnCustomer { ActiveFlag = "Y" };
            Assert.IsTrue(HasRole.IsActive(c));
        }

        public void IsActiveCustomerTest_Active_LowerCase()
        {
            EcnCustomer c = new EcnCustomer { ActiveFlag = "y" };
            Assert.IsTrue(HasRole.IsActive(c));
        }

        #endregion IsActive
        #region IsSystemAdministrator

        [TestMethod()]
        public void IsSystemAdministratorTest_Null()
        {
            EcnUser u = null;
            Assert.IsFalse(HasRole.IsSystemAdministrator(u));
        }

        [TestMethod()]
        public void IsSystemAdministratorTest_NotActive()
        {
            EcnUser u = new EcnUser { ActiveFlag = "n", IsSysAdmin = true };
            Assert.IsFalse(HasRole.IsSystemAdministrator(u));
        }

        [TestMethod()]
        public void IsSystemAdministratorTest_Admin()
        {
            EcnUser u = new EcnUser 
            { 
                ActiveFlag = "Y",
                IsAdmin = true,
                IsSysAdmin = false,
                IsChannelAdmin = false
            };
            Assert.IsFalse(HasRole.IsSystemAdministrator(u));
        }

        [TestMethod()]
        public void IsSystemAdministratorTest_ChannelAdmin()
        {
            EcnUser u = new EcnUser
            {
                ActiveFlag = "Y",
                IsAdmin = true,
                IsSysAdmin = false,
                IsChannelAdmin = true
            };
            Assert.IsFalse(HasRole.IsSystemAdministrator(u));
        }

        [TestMethod()]
        public void IsSystemAdministratorTest_Ok()
        {
            EcnUser u = new EcnUser 
            { 
                ActiveFlag = "Y",
                IsAdmin = false,
                IsSysAdmin = true,
                IsChannelAdmin = false
            };
            Assert.IsTrue(HasRole.IsSystemAdministrator(u));
        }

        #endregion IsSystemAdministrator
        #region IsChannelAdministrator

        [TestMethod()]
        public void IsChannelAdministratorTest_Null()
        {
            EcnUser u = null;
            Assert.IsFalse(HasRole.IsChannelAdministrator(u));
        }

        [TestMethod()]
        public void IsChannelAdministratorTest_NotActive()
        {
            EcnUser u = new EcnUser { 
                ActiveFlag = "n", 
                IsAdmin = false,
                IsSysAdmin = false,
                IsChannelAdmin = true
            };
            Assert.IsFalse(HasRole.IsChannelAdministrator(u));
        }

        [TestMethod()]
        public void IsChannelAdministratorTest_Admin()
        {
            EcnUser u = new EcnUser 
            { 
                ActiveFlag = "Y", 
                IsAdmin = true,
                IsSysAdmin = false,
                IsChannelAdmin = false 
            };
            Assert.IsFalse(HasRole.IsChannelAdministrator(u));
        }

        public void IsChannelAdministratorTest_SysAdmin()
        {
            EcnUser u = new EcnUser
            {
                ActiveFlag = "Y",
                IsAdmin = false,
                IsSysAdmin = true,
                IsChannelAdmin = false
            };
            Assert.IsTrue(HasRole.IsChannelAdministrator(u));
        }

        [TestMethod()]
        public void IsChannelAdministratorTest_Ok()
        {
            EcnUser u = new EcnUser
            {
                ActiveFlag = "Y",
                IsAdmin = false,
                IsSysAdmin = false,
                IsChannelAdmin = true
            };
            Assert.IsTrue(HasRole.IsChannelAdministrator(u));
        }

        #endregion IsChannelAdministrator
        #region IsAdministrator

        [TestMethod()]
        public void IsAdministratorTest_Null()
        {
            EcnUser u = null;
            Assert.IsFalse(HasRole.IsAdministrator(u));
        }

        [TestMethod()]
        public void IsAdministratorTest_NotActive()
        {
            EcnUser u = new EcnUser 
            { 
                ActiveFlag = "n", 
                IsAdmin = true,
                IsSysAdmin = false,
                IsChannelAdmin = false
            };
            Assert.IsFalse(HasRole.IsAdministrator(u));
        }


        [TestMethod()]
        public void IsAdministratorTest_Ok()
        {
            EcnUser u = new EcnUser
            {
                ActiveFlag = "Y",
                IsAdmin = true,
                IsSysAdmin = false,
                IsChannelAdmin = false
            };
            Assert.IsTrue(HasRole.IsAdministrator(u));
        }

        [TestMethod()]
        public void IsAdministratorTest_SysAdmin()
        {
            EcnUser u = new EcnUser
            {
                ActiveFlag = "Y",
                IsAdmin = false,
                IsSysAdmin = true,
                IsChannelAdmin = false
            };
            Assert.IsTrue(HasRole.IsAdministrator(u));
        }

        [TestMethod()]
        public void IsAdministratorTest_ChannelAdmin()
        {
            EcnUser u = new EcnUser
            {
                ActiveFlag = "Y",
                IsAdmin = false,
                IsSysAdmin = false,
                IsChannelAdmin = true
            };
            Assert.IsTrue(HasRole.IsAdministrator(u));
        }

        #endregion IsAdministrator
        #region IsAdministratedChannel
        
        [TestMethod()]
        public void IsAdministratedChannelTest_Null()
        {
            BaseChannel b = new BaseChannel { BaseChannelID = 1 };
            EcnCustomer c = new EcnCustomer { BaseChannelID = 1 };
            Assert.IsFalse(HasRole.IsAdministratedChannel(c, null),"null base channel");
            Assert.IsFalse(HasRole.IsAdministratedChannel(null, b), "null customer");
        }

        [TestMethod()]
        public void IsAdministratedChannelTest_Mismatch()
        {
            BaseChannel b = new BaseChannel { BaseChannelID = 1 };
            EcnCustomer c = new EcnCustomer { BaseChannelID = 2 };
            Assert.IsFalse(HasRole.IsAdministratedChannel(c, b));
        }

        [TestMethod()]
        public void IsAdministratedChannelTest_Ok()
        {
            BaseChannel b = new BaseChannel { BaseChannelID = 1 };
            EcnCustomer c = new EcnCustomer { BaseChannelID = 1 };
            Assert.IsTrue(HasRole.IsAdministratedChannel(c, b));
        }

        #endregion IsAdministratedChannel
        #region IsChannelMaster

        [TestMethod()]
        public void IsChannelMasterTest_Null()
        {
            Assert.IsFalse(HasRole.IsChannelMaster(null));
        }

        [TestMethod()]
        public void IsChannelMasterTest_MasterIdZero()
        {
            AuthenticationTicket t = new AuthenticationTicket() { 
                //MasterUserID = 0
            };
            Assert.IsFalse(HasRole.IsChannelMaster(t));
        }

        [TestMethod()]
        public void IsChannelMasterTest_Ok()
        {
            AuthenticationTicket t = new AuthenticationTicket() { 
                //MasterUserID = 1 
            };
            Assert.IsTrue(HasRole.IsChannelMaster(t));
        }

        #endregion IsChannelMaster
        #region IsSameChannel
        
        [TestMethod()]
        public void IsSameChannelTest_Null()
        {
            EcnCustomer c = new EcnCustomer { BaseChannelID = 1 };
            Assert.IsFalse(HasRole.IsSameChannel(null, c), "first customer null");
            Assert.IsFalse(HasRole.IsSameChannel(c, null), "second customer null");
            Assert.IsFalse(HasRole.IsSameChannel(null, null), "both null");
        }

        [TestMethod()]
        public void IsSameChannelTest_Mismatch()
        {
            EcnCustomer c1 = new EcnCustomer { BaseChannelID = 1, ActiveFlag = "Y" };
            EcnCustomer c2 = new EcnCustomer { BaseChannelID = 2, ActiveFlag = "Y" };
            Assert.IsFalse(HasRole.IsSameChannel(c1, c2));
        }

        [TestMethod()]
        public void IsSameChannelTest_Inactive()
        {
            EcnCustomer c1 = new EcnCustomer { BaseChannelID = 1, ActiveFlag = "N" };
            EcnCustomer c2 = new EcnCustomer { BaseChannelID = 1, ActiveFlag = "Y" };
            Assert.IsFalse(HasRole.IsSameChannel(c1, c2), "first inactive");
            Assert.IsFalse(HasRole.IsSameChannel(c2, c1), "second inactive");
        }

        [TestMethod()]
        public void IsSameChannelTest_Ok()
        {
            EcnCustomer c1 = new EcnCustomer { BaseChannelID = 1, ActiveFlag = "Y" };
            EcnCustomer c2 = new EcnCustomer { BaseChannelID = 1, ActiveFlag = "Y" };
            Assert.IsTrue(HasRole.IsSameChannel(c1, c2));
        }

        #endregion IsSameChannel
        #region IsSameCustomer

        [TestMethod()]
        public void IsSameCustomerTest_Null()
        {
            EcnUser u = new EcnUser { CustomerID = 1, ActiveFlag = "Y" };
            Assert.IsFalse(HasRole.IsSameCustomer(null, u), "first user null");
            Assert.IsFalse(HasRole.IsSameCustomer(u, null), "second user null");
            Assert.IsFalse(HasRole.IsSameCustomer(null, null), "both null");
        }

        public void IsSameCustomerTest_Inactive()
        {
            EcnUser u1 = new EcnUser { CustomerID = 1, ActiveFlag = "N" };
            EcnUser u2 = new EcnUser { CustomerID = 1, ActiveFlag = "Y" };
            Assert.IsFalse(HasRole.IsSameCustomer(u1, u2), "first user inactive");
            Assert.IsFalse(HasRole.IsSameCustomer(u2, u1), "second user inactive");
            Assert.IsFalse(HasRole.IsSameCustomer(u1, u1), "both inactive");
        }

        public void IsSameCustomerTest_DifferentCustomers()
        {
            EcnUser u1 = new EcnUser { CustomerID = 1, ActiveFlag = "Y" };
            EcnUser u2 = new EcnUser { CustomerID = 2, ActiveFlag = "Y" };
            Assert.IsFalse(HasRole.IsSameCustomer(u1, u2));
        }

        public void IsSameCustomerTest_Ok()
        {
            EcnUser u1 = new EcnUser { CustomerID = 1, ActiveFlag = "Y" };
            EcnUser u2 = new EcnUser { CustomerID = 1, ActiveFlag = "Y" };
            Assert.IsTrue(HasRole.IsSameCustomer(u1, u2));
        }

        #endregion IsSameUser
        #region HasUserAdministrativePrivilege

        [TestMethod()]
        public void HasUserAdministrativePrivilegeTest_Null()
        {
            //EcnUser u = new EcnUser { IsAdmin = true, HasUserAccess = true, ActiveFlag = "Y" };
            EcnUser u = null;
            Assert.IsFalse(HasRole.HasUserAdministrativePrivilege(u));
        }

        [TestMethod()]
        public void HasUserAdministrativePrivilegeTest_NotPrivileged()
        {
            EcnUser u = new EcnUser { IsAdmin = true, HasUserAccess = false, ActiveFlag = "Y" };
            Assert.IsFalse(HasRole.HasUserAdministrativePrivilege(u));
        }

        [TestMethod()]
        public void HasUserAdministrativePrivilegeTest_Ok()
        {
            EcnUser u = new EcnUser { IsAdmin = true, HasUserAccess = true, ActiveFlag = "Y" };
            Assert.IsTrue(HasRole.HasUserAdministrativePrivilege(u));
        }

        #endregion HasUserAdministrativePrivilege
        #region CanAdministrateUsers

        [TestMethod()]
        public void CanAdministrateUsersTest_Null()
        {
            EcnUser u = null;
            Assert.IsFalse(HasRole.CanAdministrateUsers(u));
        }

        [TestMethod()]
        public void CanAdministrateUsersTest_NotAdmin()
        {
            EcnUser u = new EcnUser { IsAdmin = false, HasUserAccess = true, ActiveFlag = "Y" };
            Assert.IsFalse(HasRole.CanAdministrateUsers(u));
        }

        [TestMethod()]
        public void CanAdministrateUsersTest_NotPrivileged()
        {
            EcnUser u = new EcnUser { IsAdmin = true, HasUserAccess = false, ActiveFlag = "Y" };
            Assert.IsFalse(HasRole.CanAdministrateUsers(u));
        }

        [TestMethod()]
        public void CanAdministrateUsersTest_Ok()
        {
            EcnUser u = new EcnUser { IsAdmin = true, HasUserAccess = true, ActiveFlag = "Y" };
            Assert.IsTrue(HasRole.CanAdministrateUsers(u));
        }

        [TestMethod()]
        public void CanAdministrateUsersTest_SystemAdministrator()
        {
            EcnUser u = new EcnUser { IsSysAdmin = true, IsAdmin = false, HasUserAccess = false, ActiveFlag = "Y" };
            Assert.IsTrue(HasRole.CanAdministrateUsers(u));
        }

        [TestMethod()]
        public void CanAdministrateUsersTest_ChannelAdministrator()
        {
            //  TODO:  Confirm:  Channel Admin does not require the UserAccess privilege
            EcnUser u = new EcnUser { IsChannelAdmin = true, IsAdmin = false, HasUserAccess = false, ActiveFlag = "Y" };
            Assert.IsTrue(HasRole.CanAdministrateUsers(u));
        }

        #endregion CanAdministrateUsers
        #region CanImpersonateTest

        [TestMethod()]
        public void CanImpersonateTest_Null()
        {
            User nullUser = null, user = new EcnUser { ActiveFlag = "Y", IsSysAdmin = true, HasUserAccess = true};
            Customer nullCustomer = null, customer = new EcnCustomer { ActiveFlag = "Y", CustomerID = 1, BaseChannelID = 1 };
            Assert.IsFalse(HasRole.CanImpersonate(nullUser, customer, user, customer), "master null");
            Assert.IsFalse(HasRole.CanImpersonate(user, nullCustomer, user, customer), "master customer null");
            Assert.IsFalse(HasRole.CanImpersonate(user, customer, nullUser, customer), "user null");
            Assert.IsFalse(HasRole.CanImpersonate(user, customer, user, nullCustomer), "customer null");
        }

        [TestMethod()]
        public void CanImpersonateTest_InactiveMaster()
        {
            User masterUser = new EcnUser { ActiveFlag = "N", IsSysAdmin = true, IsChannelAdmin = true, IsAdmin = true, HasUserAccess = true }, 
                       user = new EcnUser { ActiveFlag = "Y" };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsFalse(HasRole.CanImpersonate(masterUser, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_SystemAdministrator()
        {
            User masterUser = new EcnUser { ActiveFlag = "Y", IsSysAdmin = true },
                       user = new EcnUser { ActiveFlag = "Y" };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsTrue(HasRole.CanImpersonate(masterUser, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_SystemAdministrator_ChannelChange()
        {
            User masterUser = new EcnUser { ActiveFlag = "Y", IsSysAdmin = true },
                       user = new EcnUser { ActiveFlag = "Y" };
            Customer masterCustomer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 },
                           customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 2 };
            Assert.IsTrue(HasRole.CanImpersonate(masterUser, masterCustomer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_NonSystemAdministrator_ChannelChange()
        {
            User masterUser = new EcnUser { ActiveFlag = "Y", IsChannelAdmin = true },
                       user = new EcnUser { ActiveFlag = "Y" };
            Customer masterCustomer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 },
                           customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 2 };
            Assert.IsFalse(HasRole.CanImpersonate(masterUser, masterCustomer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_BecomeSystemAdministrator()
        {
            User masterUser = new EcnUser { ActiveFlag = "Y" },
                       user = new EcnUser { ActiveFlag = "Y", IsSysAdmin = true };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsFalse(HasRole.CanImpersonate(masterUser, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_ChannelAdministrator()
        {
            User masterUser = new EcnUser { ActiveFlag = "Y", IsChannelAdmin = true },
                       user = new EcnUser { ActiveFlag = "Y" };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsTrue(HasRole.CanImpersonate(masterUser, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_NonChannelAdministrator_CustomerChange()
        {
            User masterUser = new EcnUser { CustomerID = 1, ActiveFlag = "Y", IsAdmin = true, HasUserAccess = true },
                       user = new EcnUser { CustomerID = 2, ActiveFlag = "Y" };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsFalse(HasRole.CanImpersonate(masterUser, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_BecomeChannelAdministrator()
        {
            User masterUser = new EcnUser { ActiveFlag = "Y", IsAdmin = true, HasUserAccess = true },
                       user = new EcnUser { ActiveFlag = "Y", IsChannelAdmin = true };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsFalse(HasRole.CanImpersonate(masterUser, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_AdministratorWithUserAccess()
        {
            User masterUser = new EcnUser { ActiveFlag = "Y", IsAdmin = true, HasUserAccess = true },
                       user = new EcnUser { ActiveFlag = "Y" };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsTrue(HasRole.CanImpersonate(masterUser, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_AdministratorWithoutUserAccess()
        {
            User masterUser = new EcnUser { ActiveFlag = "Y", IsAdmin = true, HasUserAccess = false },
                       user = new EcnUser { ActiveFlag = "Y" };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsFalse(HasRole.CanImpersonate(masterUser, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_NonAdministrator()
        {
            User user = new EcnUser { ActiveFlag = "Y" };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsFalse(HasRole.CanImpersonate(user, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_NonAdministratorWithUserAccess()
        {
            User user = new EcnUser { ActiveFlag = "Y", HasUserAccess = true };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsFalse(HasRole.CanImpersonate(user, customer, user, customer));
        }

        [TestMethod()]
        public void CanImpersonateTest_ImpersonateSelf() //test the test
        {
            User user = new EcnUser { ActiveFlag = "Y", IsAdmin = true, HasUserAccess = true };
            Customer customer = new EcnCustomer { ActiveFlag = "Y", BaseChannelID = 1 };
            Assert.IsTrue(HasRole.CanImpersonate(user, customer, user, customer));
        }

        #endregion CanImpersonateTest

    }
}
