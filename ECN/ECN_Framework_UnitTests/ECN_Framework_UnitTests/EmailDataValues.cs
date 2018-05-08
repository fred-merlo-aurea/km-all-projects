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
    /// Summary description for EmailDataValues
    /// </summary>
    [TestClass]
    public class EmailDataValues
    {
        [TestMethod]
        public void GetByGroupDataFieldsID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //Error with CreateBuilder()
            List<ECNEntity.Communicator.EmailDataValues> edv = ECNBusiness.Communicator.EmailDataValues.GetByGroupDataFieldsID(151408, 22659040, user);
            Assert.IsNotNull(edv[0], "No EmailDataValues with this GroupDataFieldsID or EmailID");
        }

        [TestMethod]
        public void GetStandaloneUDFDataValues()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //Hit timeout exception when trying to open a connection
            DataTable edv = ECNBusiness.Communicator.EmailDataValues.GetStandaloneUDFDataValues(889, 1050201, user);
            Assert.IsNotNull(edv.Rows[0], "No EmailDataValues with this GroupID and EmailID");
        }

        [TestMethod]
        public void GetTransactionalUDFDataValues()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //Hit timeout exception when trying to open a connection
            DataTable edv = ECNBusiness.Communicator.EmailDataValues.GetTransUDFDataValues(user.CustomerID.Value, 889, "1050201", 1, user);
            Assert.IsNotNull(edv.Rows[0], "No EmailDataValues with this GroupID and EmailID");
        }
    }
}
