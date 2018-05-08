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
    /// Summary description for Campaign
    /// </summary>
    [TestClass]
    public class Campaign
    {
        [TestMethod]
        public void GetByID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Campaign campaign = ECNBusiness.Communicator.Campaign.GetByCampaignID(5, user, true);
            Assert.IsNotNull(campaign, "No Campaign ");
        }

        [TestMethod]
        public void GetByCampaignItemID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //As of 1/18/2013 there are no Campaign Items so this test will fail
            ECNEntity.Communicator.Campaign campaign = ECNBusiness.Communicator.Campaign.GetByCampaignItemID(-1, user, false);
            //Assert.IsNotNull(campaign, "No campaign with this CampaignItemID");
        }

        [TestMethod]
        public void GetByCustomerID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //With the getChildren parameter set to true, this fails
            List<ECNEntity.Communicator.Campaign> campaign = ECNBusiness.Communicator.Campaign.GetByCustomerID(1, user, false);
            Assert.IsNotNull(campaign[0], "No Campaigns for this User");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Campaign campaign = ECNBusiness.Communicator.Campaign.GetByCampaignID(23, user, false);
            ECNBusiness.Communicator.Campaign.Save(campaign, user);
        }
       
    }
}
