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
    /// Summary description for DomainSuppression
    /// </summary>
    [TestClass]
    public class DomainSuppression
    {
        [TestMethod]
        public void GetByDomain()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.DomainSuppression> ds = ECNBusiness.Communicator.DomainSuppression.GetByDomain("km", 1, 12, user);
            Assert.IsNotNull(ds[0], "No DomainSuppression with this CustomerID and BaseChannelID");
        }

        [TestMethod]
        public void GetByDomainSuppressionID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.DomainSuppression ds = ECNBusiness.Communicator.DomainSuppression.GetByDomainSuppressionID(28, user);
            Assert.IsNotNull(ds, "No DomainSuppression with this DomainSuppressionID");
        }

        [TestMethod]
        public void Save()
        {
            //Cant test because records with matching CustomerID's dont have matching BaseChannelID's and vice versa so the save method throws an error.
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.DomainSuppression suppression = ECNBusiness.Communicator.DomainSuppression.GetByDomainSuppressionID(55, user);
            Assert.IsNotNull(suppression, "No DomainSuppression with this DomainSuppressionID");

            DateTime test = DateTime.Now;
            ECNBusiness.Communicator.DomainSuppression.Save(suppression, user);
            ECNEntity.Communicator.DomainSuppression newSuppression = ECNBusiness.Communicator.DomainSuppression.GetByDomainSuppressionID(28, user);
            Assert.AreEqual(test.ToShortDateString(), newSuppression.UpdatedDate.Value.ToShortDateString(),"DomainSuppression NOT Saved");
        }
    }
}
