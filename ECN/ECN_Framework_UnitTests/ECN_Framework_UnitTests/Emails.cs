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
    /// Summary description for Emails
    /// </summary>
    [TestClass]
    public class Emails
    {
        [TestMethod]
        public void GetEmailsByGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //Error in ECN_Framework_DataLayer/Communicator/Emails GetByGroupID()
            List<ECNEntity.Communicator.Email> emails = ECNBusiness.Communicator.Email.GetByGroupID(50085, user);
            Assert.IsNotNull(emails[0], "No emails in this group");
            
        }

        [TestMethod]
        public void GetEmailsByEmailID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            
            ECNEntity.Communicator.Email email = ECNBusiness.Communicator.Email.GetByEmailID(108544048, user);
            Assert.IsNotNull(email, "No Email with this ID");
        }

        [TestMethod]
        public void GetEmails()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //Error in ECN_Framework_DataLayer/Communicator/Emails GetByEmailIDGroupID()
            ECNEntity.Communicator.Email email = ECNBusiness.Communicator.Email.GetByEmailIDGroupID(108544048,50085, user);
            Assert.IsNotNull(email, "No Email with that ID or GroupID");
        }

        [TestMethod]
        public void Save()
        {

            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Email email = ECNBusiness.Communicator.Email.GetByEmailID(154584736, user);
            email.State = "MN";
            email.Zip = "55447";
            email.Address = "UnitTest_" + DateTime.Now.ToShortDateString();

            ECNBusiness.Communicator.Email.Save(user, email, 49195);
        }
    }
}
