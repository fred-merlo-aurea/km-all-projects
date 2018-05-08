using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECNBusiness = ECN_Framework_BusinessLayer;
using ECNEntity = ECN_Framework_Entities;
using ECNCommon = ECN_Framework_Common;
using System.Configuration;
using System.Data;

namespace ECN_Framework_UnitTests
{
    /// <summary>
    /// Summary description for Samples
    /// </summary>
    [TestClass]
    public class Samples
    {
        [TestMethod]
        public void GetBySampleID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //PropertyInfo is null in DataFunctions.CreateBuilder()
            ECNEntity.Communicator.Sample sample = ECNBusiness.Communicator.Sample.GetBySampleID(6593, user);
            Assert.IsNotNull(sample, "No Samples");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Sample sample = ECNBusiness.Communicator.Sample.GetBySampleID(6593, user);
            Assert.IsNotNull(sample, "No Samples");
            sample.UpdatedUserID = user.UserID;
            //PropertyInfo is null in DataFunctions.CreateBuilder()
            ECNBusiness.Communicator.Sample.Save(sample, user);
        }
    }
}
