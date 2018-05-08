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
    /// Summary description for TriggerPlans
    /// </summary>
    [TestClass]
    public class TriggerPlans
    {
        [TestMethod]
        public void GetByTriggerPlanID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.TriggerPlans tp = ECNBusiness.Communicator.TriggerPlans.GetByTriggerPlanID(1, user);
            Assert.IsNotNull(tp, "No TriggerPlans with this TriggerPlanID");
        }
    }
}
