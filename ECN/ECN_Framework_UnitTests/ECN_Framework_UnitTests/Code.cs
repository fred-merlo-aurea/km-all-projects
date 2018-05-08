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
    /// Summary description for Code
    /// </summary>
    [TestClass]
    public class Code
    {
        [TestMethod]
        public void GetByCodeID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Code code = ECNBusiness.Communicator.Code.GetByCodeID(45, user);
            Assert.IsNotNull(code, "No Code");
        }

        [TestMethod]
        public void GetByCustomerAndCategory()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.Code> code = ECNBusiness.Communicator.Code.GetByCustomerAndCategory(ECNCommon.Objects.Communicator.Enums.CodeType.BLASTCATEGORY, user);
            Assert.IsNotNull(code[0], "No Codes of type BlastCategory");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Code code = ECNBusiness.Communicator.Code.GetByCodeID(345, user);
            code.CodeValue = "UnitTest_" + DateTime.Now.ToShortDateString();
            code.CreatedUserID = user.UserID;
            code.UpdatedUserID = user.UserID;
            ECNBusiness.Communicator.Code.Save(code, user);
        }
    }
}
