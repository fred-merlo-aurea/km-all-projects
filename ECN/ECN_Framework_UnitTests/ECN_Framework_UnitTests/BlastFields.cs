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
    /// Summary description for BlastFields
    /// </summary>
    [TestClass]
    public class BlastFields
    {
        [TestMethod]
        public void GetByBlastID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.BlastFields bf = ECNBusiness.Communicator.BlastFields.GetByBlastID(1568850, user);
            Assert.IsNotNull(bf, "No Blast Fields for this Blast");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.BlastFields bf = new ECNEntity.Communicator.BlastFields();
            
            bf.CreatedUserID = 1568;
            bf.UpdatedUserID = 1568;
            bf.CustomerID = 1;
            bf.Field1 = "Unittest_" + DateTime.Now.ToShortDateString();
            bf.BlastID = 1568827;

            ECNBusiness.Communicator.BlastFields.Save(bf, user);
            
        }

        
    }
}
