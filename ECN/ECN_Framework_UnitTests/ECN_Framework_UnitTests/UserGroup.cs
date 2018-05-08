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
    /// Summary description for UserGroup
    /// </summary>
    [TestClass]
    public class UserGroup
    {
        [TestMethod]
        public void GetByUserID()
        {
            List<ECNEntity.Communicator.UserGroup> userGroups = ECNBusiness.Communicator.UserGroup.Get(1728);
            Assert.IsNotNull(userGroups[0], "No UserGroups for that User");
        }

        [TestMethod]
        public void GetSingle()
        {
            ECNEntity.Communicator.UserGroup userGroup = ECNBusiness.Communicator.UserGroup.GetSingle(1728, 4516);
            Assert.IsNotNull(userGroup, "No UserGroup for that User/Group");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.UserGroup userGroup = ECNBusiness.Communicator.UserGroup.GetSingle(1728, 1559);
            Assert.IsNotNull(userGroup, "No UserGroup for that User/Group");

            int? test = null;
            test = ECNBusiness.Communicator.UserGroup.Save(userGroup, user);
            Assert.IsNotNull(test, "UserGroup did not save");
        }
        
    }
}
