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
    /// Summary description for Filter
    /// </summary>
    [TestClass]
    public class Filter
    {
        [TestMethod]
        public void GetByFilterID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Filter filter = ECNBusiness.Communicator.Filter.GetByFilterID(30930, user);
            Assert.IsNotNull(filter, "No Filter with that ID");
        }

        [TestMethod]
        public void GetByByGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.Filter> filter = ECNBusiness.Communicator.Filter.GetByGroupID(49195, false, user);
            Assert.IsNotNull(filter[0], "No Filter in that group");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Filter filter = ECNBusiness.Communicator.Filter.GetByFilterID(30946, user);
            Assert.IsNotNull(filter, "No Filter with that ID");
            filter.GroupCompareType = "OR";
            int? test = null;
            test = ECNBusiness.Communicator.Filter.Save(filter, user);
            Assert.IsNotNull(test, "Filter did not save");
        }
    }
}
