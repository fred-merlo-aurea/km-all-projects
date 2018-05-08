using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECNBusiness = ECN_Framework_BusinessLayer;
using ECNEntity = ECN_Framework_Entities;
using ECNCommon = ECN_Framework_Common;
using System.Configuration;
using System.Data;

namespace ECN_Framework_UnitTests
{
    /// <summary>
    /// Summary description for LinkOwnerIndex
    /// </summary>
    [TestClass]
    public class LinkOwnerIndex
    {
        [TestMethod]
        public void GetByCustomerID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.LinkOwnerIndex> loi = ECNBusiness.Communicator.LinkOwnerIndex.GetByCustomerID(1, user);
            Assert.IsNotNull(loi[0], "No LinkOwnerIndex for this CustomerID");
        }

        [TestMethod]
        public void GetByOwnerID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.LinkOwnerIndex loi = ECNBusiness.Communicator.LinkOwnerIndex.GetByOwnerID(1, user);
            Assert.IsNotNull(loi, "No LinkOwnerIndex with this OwnerID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.LinkOwnerIndex loi = ECNBusiness.Communicator.LinkOwnerIndex.GetByOwnerID(813, user);
            Assert.IsNotNull(loi, "No LinkOwnerIndex with this OwnerID");
            loi.UpdatedUserID = user.UserID;

            ECNBusiness.Communicator.LinkOwnerIndex.Save(loi, user);
        }

    }
}
