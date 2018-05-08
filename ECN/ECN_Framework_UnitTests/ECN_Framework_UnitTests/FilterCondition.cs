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
    /// Summary description for FilterCondition
    /// </summary>
    [TestClass]
    public class FilterCondition
    {
        [TestMethod]
        public void GetByFilterConditionID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.FilterCondition fc = ECNBusiness.Communicator.FilterCondition.GetByFilterConditionID(12, user);
            Assert.IsNotNull(fc, "No FilterCondition with this FilterConditionID");
        }

        [TestMethod]
        public void GetByFilterGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.FilterCondition> fc = ECNBusiness.Communicator.FilterCondition.GetByFilterGroupID(10, user);
            Assert.IsNotNull(fc[0], "No FilterCondition with this FilterGroupID");
        }

        [TestMethod]
        public void GetSortOrder()
        {
            int? sortOrder = null;

            sortOrder = ECNBusiness.Communicator.FilterCondition.GetSortOrder(0);
            Assert.IsNotNull(sortOrder, "No SortOrder with this FilterGroupID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.FilterCondition fc = ECNBusiness.Communicator.FilterCondition.GetByFilterConditionID(67, user);
            Assert.IsNotNull(fc, "No FilterCondition with this FilterConditionID");

            int? test = null;
            test = ECNBusiness.Communicator.FilterCondition.Save(fc, user);
            Assert.IsNotNull(test, "FilterCondition did not save");
        }
    }
}
