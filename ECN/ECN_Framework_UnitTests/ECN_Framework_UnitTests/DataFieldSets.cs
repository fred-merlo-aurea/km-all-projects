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
    /// Summary description for DataFieldSets
    /// </summary>
    [TestClass]
    public class DataFieldSets
    {
        [TestMethod]
        public void GetByDataFieldsetID()
        {
            ECNEntity.Communicator.DataFieldSets dfs = ECNBusiness.Communicator.DataFieldSets.GetByDataFieldsetID(13, 8656, true);
            Assert.IsNotNull(dfs, "No DataFieldSet for this DataFieldsetID");
        }

        [TestMethod]
        public void GetByGroupID()
        {

            List<ECNEntity.Communicator.DataFieldSets> dfs = ECNBusiness.Communicator.DataFieldSets.GetByGroupID(8656);
            Assert.IsNotNull(dfs[0], "No DataFieldSets with this GroupID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            ECNEntity.Communicator.DataFieldSets set = ECNBusiness.Communicator.DataFieldSets.GetByDataFieldsetID(13, 8656, false);
            Assert.IsNotNull(set, "No DataFieldSet with this ID");
            int? save = null;
            save = ECNBusiness.Communicator.DataFieldSets.Save(set, user);
            Assert.IsNotNull(save, "Did not save");
        }
    }
}
