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
    /// Summary description for MessageType
    /// </summary>
    [TestClass]
    public class MessageType
    {
        [TestMethod]
        public void GetActivePriority()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.MessageType> mt = ECNBusiness.Communicator.MessageType.GetActivePriority(true, true, 12, user);
            Assert.IsNotNull(mt[0], "No MessageType for this BaseChannelID and User");
        }

        [TestMethod]
        public void GetByBaseChannelID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.MessageType> mt = ECNBusiness.Communicator.MessageType.GetByBaseChannelID(12, user);
            Assert.IsNotNull(mt[0], "No MessageType with this BaseChannelID");
        }

        [TestMethod]
        public void GetByMessageTypeID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.MessageType mt = ECNBusiness.Communicator.MessageType.GetByMessageTypeID(79, user);
            Assert.IsNotNull(mt, "No MessageType with this MessageTypeID");
        }

        [TestMethod]
        public void GetMaxSortOrder()
        {
            int? sortOrder = null;
            sortOrder = ECNBusiness.Communicator.MessageType.GetMaxSortOrder(12);
            Assert.IsNotNull(sortOrder, "No SortOrder for this BaseChannelID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.MessageType mt = ECNBusiness.Communicator.MessageType.GetByMessageTypeID(79, user);
            Assert.IsNotNull(mt, "No MessageType with this MessageTypeID");

            int? test = null;
            test = ECNBusiness.Communicator.MessageType.Save(mt, user);
            Assert.IsNotNull(test, "MessageType did not save");
        }
    }
}
