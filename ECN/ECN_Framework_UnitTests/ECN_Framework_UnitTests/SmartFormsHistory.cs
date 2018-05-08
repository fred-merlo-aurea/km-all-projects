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
    /// Summary description for SmartFormsHistory
    /// </summary>
    [TestClass]
    public class SmartFormsHistory
    {
        [TestMethod]
        public void GetByGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.SmartFormsHistory> sfh = ECNBusiness.Communicator.SmartFormsHistory.GetByGroupID(1361, user);
            Assert.IsNotNull(sfh[0], "No SmartFormsHistory with this GroupID");
        }

        [TestMethod]
        public void GetBySmartFormID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.SmartFormsHistory sfh = ECNBusiness.Communicator.SmartFormsHistory.GetBySmartFormID(48, 1361, user);
            Assert.IsNotNull(sfh, "NO SmartFormsHistory with this SmartFormID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.SmartFormsHistory sfh = ECNBusiness.Communicator.SmartFormsHistory.GetBySmartFormID(48, 1361, user);
            Assert.IsNotNull(sfh, "NO SmartFormsHistory with this SmartFormID");

            int? test = null;
            test = ECNBusiness.Communicator.SmartFormsHistory.Save(sfh, user);
            Assert.IsNotNull(test, "SmartFormsHistory did not save");
        }
    }
}
