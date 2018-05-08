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
    /// Summary description for ContentFilter
    /// </summary>
    [TestClass]
    public class ContentFilter
    {
        [TestMethod]
        public void GetByContentID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.ContentFilter> contentFilter = ECNBusiness.Communicator.ContentFilter.GetByContentID(85917, user, true);
            Assert.IsNotNull(contentFilter[0], "No Content Filters for this ContentID");
        }

        [TestMethod]
        public void GetByFilterID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.ContentFilter cf = ECNBusiness.Communicator.ContentFilter.GetByFilterID(601, user, true);
            Assert.IsNotNull(cf, "No ContentFilter for this FilterID");
        }

        [TestMethod]
        public void GetByLayoutID_SlotNumber()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.ContentFilter> cf = ECNBusiness.Communicator.ContentFilter.GetByLayoutIDSlotNumber(76660, 1, user, false);
            Assert.IsNotNull(cf[0], "No ContentFilters for that LayoutID and SlotNumber");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.ContentFilter filter = ECNBusiness.Communicator.ContentFilter.GetByFilterID(636, user, false);
            filter.UpdatedUserID = user.UserID;
            filter.CreatedUserID = user.UserID;
            filter.WhereClause = "Test = 'true'";
            ECNBusiness.Communicator.ContentFilter.Save(filter, user);
        }

    }
}
