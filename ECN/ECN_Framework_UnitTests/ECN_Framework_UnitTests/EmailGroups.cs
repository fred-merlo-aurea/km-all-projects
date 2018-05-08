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
    /// Summary description for EmailGroups
    /// </summary>
    [TestClass]
    public class EmailGroups
    {
        [TestMethod]
        public void GetByBounceScore()
        {
            
            DataTable eg = ECNBusiness.Communicator.EmailGroup.GetByBounceScore(1, 50085, 3, "<");
            Assert.IsNotNull(eg, "No email groups for customerID = 1 with groupID = 50085 with less than 3 bounces");
        }

        [TestMethod]
        public void GetByEmailID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.EmailGroup> eg = ECNBusiness.Communicator.EmailGroup.GetByEmailID(108102227, user);
            Assert.IsNotNull(eg[0], "NO Email Group with that EMail ID");
        }

        [TestMethod]
        public void GetByEmailIDGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.EmailGroup eg = ECNBusiness.Communicator.EmailGroup.GetByEmailIDGroupID(108102227, 53747, user);
            Assert.IsNotNull(eg, "No email group with this EmailID and GroupID");
        }

        [TestMethod]
        public void GetByGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.EmailGroup> eg = ECNBusiness.Communicator.EmailGroup.GetByGroupID(50085, user);
            Assert.IsNotNull(eg[0], "No EmailGroup with that GroupID");
        }

        [TestMethod]
        public void GetBySearchStringPaging()
        {
            DataSet eg = ECNBusiness.Communicator.EmailGroup.GetBySearchStringPaging(1, 50085, 1, 20, "");
            Assert.IsNotNull(eg.Tables[1].Rows[0], "No EmailGroups");
        }

        [TestMethod]
        public void GetByUserID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            DataTable eg = ECNBusiness.Communicator.EmailGroup.GetByUserID(1, user.UserID);
            Assert.IsNotNull(eg.Rows[0], "No EmailGroups for this user");
        }

        [TestMethod]
        public void GetColumnNames()
        {
            DataTable eg = ECNBusiness.Communicator.EmailGroup.GetColumnNames();
            Assert.IsNotNull(eg.Rows[0], "No Columns");
        }

        [TestMethod]
        public void GetGroupEmailProfilesWithUDF()
        {
            DataTable eg = ECNBusiness.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(50085, 1, "", "'S'");
            Assert.IsNotNull(eg.Rows[0], "No EmailProfiles");
        }
        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.EmailGroup eg = ECNBusiness.Communicator.EmailGroup.GetByEmailIDGroupID(108102227, 53747, user);
            Assert.IsNotNull(eg, "No email group with this EmailID and GroupID");

            //ECNBusiness.Communicator.EmailGroup.Save(eg, false, "", "", user);
        }
    }
}
