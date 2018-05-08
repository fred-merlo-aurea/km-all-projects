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
    /// Summary description for FilterGroup
    /// </summary>
    [TestClass]
    public class FilterGroup
    {
        [TestMethod]
        public void GetByFilterGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.FilterGroup fg = ECNBusiness.Communicator.FilterGroup.GetByFilterGroupID(10, user);
            Assert.IsNotNull(fg, "No FilterGroup with this FilterGroupID");
        }

        [TestMethod]
        public void GetByFilterID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.FilterGroup> fg = ECNBusiness.Communicator.FilterGroup.GetByFilterID(30936, user);
            Assert.IsNotNull(fg[0], "No FilterGroup with this FilterID");
        }

        [TestMethod]
        public void GetSortOrder()
        {
            int? sortOrder = null;
            sortOrder = ECNBusiness.Communicator.FilterGroup.GetSortOrder(30936);
            Assert.IsNotNull(sortOrder, "No SortOrder for this FilterID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.FilterGroup fg = ECNBusiness.Communicator.FilterGroup.GetByFilterGroupID(68, user);
            Assert.IsNotNull(fg, "No FilterGroup with this FilterGroupID");

            //fg.CreatedUserID = user.UserID;
            //fg.UpdatedUserID = user.UserID;
            ECNBusiness.Communicator.FilterGroup.Save(fg, user);
        }
    }
}
