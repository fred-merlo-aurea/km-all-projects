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
    /// Summary description for ConversionLinks
    /// </summary>
    [TestClass]
    public class ConversionLinks
    {
        [TestMethod]
        public void GetByLayoutID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.ConversionLinks> cl = ECNBusiness.Communicator.ConversionLinks.GetByLayoutID(1258, user);
            Assert.IsNotNull(cl[0], "No ConversionLinks for this LayoutID");
        }

        [TestMethod]
        public void GetByLinkID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.ConversionLinks cl = ECNBusiness.Communicator.ConversionLinks.GetByLinkID(12, user, true);
            Assert.IsNotNull(cl, "No ConversionLinks with this LinkID");
        }

        [TestMethod]
        public void Save()
        {

            //ECNBusiness.Communicator.ConversionLinks.
        }
    }
}
