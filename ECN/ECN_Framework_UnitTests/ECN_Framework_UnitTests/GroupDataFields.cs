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
    /// Summary description for GroupDataFields
    /// </summary>
    [TestClass]
    public class GroupDataFields
    {
        [TestMethod]
        public void GetByDataFieldSetID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.GroupDataFields> gdf = ECNBusiness.Communicator.GroupDataFields.GetByDataFieldSetID(40531, 611, user);
            Assert.IsNotNull(gdf[0], "No GroupDataFields with this GroupID or DataFieldSetID");
        }

        [TestMethod]
        public void GetByGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.GroupDataFields> gdf = ECNBusiness.Communicator.GroupDataFields.GetByGroupID(40531, user);
            Assert.IsNotNull(gdf[0], "No GroupDataFields with this GroupID");
        }

        [TestMethod]
        public void GetByID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.GroupDataFields gdf = ECNBusiness.Communicator.GroupDataFields.GetByID(151606, 40531, user);
            Assert.IsNotNull(gdf, "No GroupDataFields with this ID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.GroupDataFields gdf = ECNBusiness.Communicator.GroupDataFields.GetByID(61518, 50085, user);
            Assert.IsNotNull(gdf, "No GroupDataFields with this ID");
            gdf.CreatedUserID = user.UserID;
            gdf.UpdatedUserID = user.UserID;
            int? test = null;
            test = ECNBusiness.Communicator.GroupDataFields.Save(gdf, user);
            Assert.IsNotNull(test, "GroupDataField did not save");
        }
    }
}
