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
    /// Summary description for BlastPlans
    /// </summary>
    [TestClass]
    public class BlastPlans
    {
        [TestMethod]
        public void GetByBlastID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.BlastPlans> bp = ECNBusiness.Communicator.BlastPlans.GetByBlastID(328651, "end", user);
            Assert.IsNotNull(bp[0], "No BlastPlans for that BlastID");
        }
        [TestMethod]
        public void GetByBlastPlanID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.BlastPlans bp = ECNBusiness.Communicator.BlastPlans.GetByBlastPlanID(898, user);
            Assert.IsNotNull(bp, "No BlastPlans for that BlastPlanID");
        }

        [TestMethod]
        public void GetByCustomerID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.BlastPlans> bp = ECNBusiness.Communicator.BlastPlans.GetByCustomerID(1, user);
            Assert.IsNotNull(bp[0], "No BlastPlans for this CustomerID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.BlastPlans blastPlans = ECNBusiness.Communicator.BlastPlans.GetByBlastPlanID(898, user);
            int? test = null;
            test = ECNBusiness.Communicator.BlastPlans.Save(blastPlans, user);
            Assert.IsNotNull(test, "BlastPlan did not save");
        }
    }
}
