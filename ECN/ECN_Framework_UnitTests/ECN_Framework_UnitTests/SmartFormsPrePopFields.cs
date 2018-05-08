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
    /// Summary description for SmartFormsPrePopFields
    /// </summary>
    [TestClass]
    public class SmartFormsPrePopFields
    {
        [TestMethod]
        public void GetByPrePopFieldID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.SmartFormsPrePopFields ppf = ECNBusiness.Communicator.SmartFormsPrePopFields.GetByPrePopFieldID(154, user);
            Assert.IsNotNull(ppf, "No PrePopFields with this PrePopFieldID");
        }

        [TestMethod]
        public void GetBySFID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.SmartFormsPrePopFields> ppf = ECNBusiness.Communicator.SmartFormsPrePopFields.GetBySFID(636, user);
            Assert.IsNotNull(ppf[0], "No PrePopFields with this SFID");
        }

        [TestMethod]
        public void GetColumnNames()
        {
            DataTable ppf = ECNBusiness.Communicator.SmartFormsPrePopFields.GetColumnNames(636, 154);
            Assert.IsNotNull(ppf.Rows[0], "No Column Names for this PrePopFields");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.SmartFormsPrePopFields ppf = ECNBusiness.Communicator.SmartFormsPrePopFields.GetByPrePopFieldID(154, user);
            Assert.IsNotNull(ppf, "No PrePopFields with this PrePopFieldID");
            
            //Invalid UserID??
            
            int? test = null;
            test = ECNBusiness.Communicator.SmartFormsPrePopFields.Save(ppf, user);
            Assert.IsNotNull(test, "PrePopField did not save");
        }
    }
}
