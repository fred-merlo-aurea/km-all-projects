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
    /// Summary description for LayoutPlans
    /// </summary>
    [TestClass]
    public class LayoutPlans
    {
        [TestMethod]
        public void GetByLayoutPlanID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //Error in CreateBuilder(), propertyinfo is null
            ECNEntity.Communicator.LayoutPlans lp = ECNBusiness.Communicator.LayoutPlans.GetByLayoutPlanID(194, user);
            Assert.IsNotNull(lp, "No LayoutPlans with this LayoutPlanID");
        }
    }
}
