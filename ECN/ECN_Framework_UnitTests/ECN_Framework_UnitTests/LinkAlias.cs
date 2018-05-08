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
    /// Summary description for LinkAlias
    /// </summary>
    [TestClass]
    public class LinkAlias
    {
        [TestMethod]
        public void GetByAliasID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.LinkAlias la = ECNBusiness.Communicator.LinkAlias.GetByAliasID(74626, user, true);
            Assert.IsNotNull(la, "No LinkAlias with this AliasID");
        }

        [TestMethod]
        public void GetByContentID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.LinkAlias> la = ECNBusiness.Communicator.LinkAlias.GetByContentID(145051, user, true);
            Assert.IsNotNull(la[0], "No LinkAlias with this ContentID");
        }

        [TestMethod]
        public void GetByLink()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.LinkAlias> la = ECNBusiness.Communicator.LinkAlias.GetByLink(145051, "http://www.sealedairmedical.com/", user, false);
            Assert.IsNotNull(la[0], "No LinkAlias with this ContentID and Link");

        }

        [TestMethod]
        public void GetByOwnerID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.LinkAlias> la = ECNBusiness.Communicator.LinkAlias.GetByOwnerID(483, user, true);
            Assert.IsNotNull(la[0], "No LinkAlias with this OwnerID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.LinkAlias la = ECNBusiness.Communicator.LinkAlias.GetByAliasID(74626, user, false);
            Assert.IsNotNull(la, "No LinkAlias with this AliasID");

            int? test = null;
            test = ECNBusiness.Communicator.LinkAlias.Save(la, user);
            Assert.IsNotNull(test, "LinkAlias did not save");
        }
    }
}
