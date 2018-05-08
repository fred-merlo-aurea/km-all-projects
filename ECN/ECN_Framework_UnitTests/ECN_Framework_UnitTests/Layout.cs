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
    /// Summary description for Layout
    /// </summary>
    [TestClass]
    public class Layout
    {
        [TestMethod]
        public void GetLayoutByCustomerID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No user");
            //Timeout exception 
            List<ECNEntity.Communicator.Layout> Layouts = ECNBusiness.Communicator.Layout.GetByCustomerID(user.CustomerID.Value, user, true);
            Assert.IsNotNull(Layouts, "No layouts for this user");
        }
        [TestMethod]
        public void GetLayoutByLayoutID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Layout currLayout = ECNBusiness.Communicator.Layout.GetByLayoutID(75896, user, true);
            Assert.IsNotNull(currLayout, "No Layout");
        }
        [TestMethod]
        public void GetLayoutByLayoutName()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            
            DataSet Layout = ECNBusiness.Communicator.Layout.GetByLayoutName("test", 4092, null, null, null, user,0, 15, "assending", string.Empty);
            Assert.IsNotNull(Layout.Tables[0].Rows[0], "No layout with name'test'");
            
        }
        [TestMethod]
        public void GetLayoutSearch()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //Could hit timeout exception if search returns a large enough list of Layouts
            List<ECNEntity.Communicator.Layout> layout = ECNBusiness.Communicator.Layout.GetByLayoutSearch("test", 4092, null, null, null, user, true);
            Assert.IsNotNull(layout[0], "No Layout with name 'test'");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Layout layout = ECNBusiness.Communicator.Layout.GetByLayoutID(75896, user, true);
            Assert.IsNotNull(layout, "No Layout");
            layout.UpdatedUserID = user.UserID;

            ECNBusiness.Communicator.Layout.Save(layout, user);
        }

    }
}
