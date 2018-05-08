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
    /// Summary description for Content
    /// </summary>
    [TestClass]
    public class Content
    {
        [TestMethod]
        public void GetByContentID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Content content = ECNBusiness.Communicator.Content.GetByContentID(85917, user, true);
            Assert.IsNotNull(content, "No Content ");
        }

        [TestMethod]
        public void GetByFolderID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");


            List<ECNEntity.Communicator.Content> content = ECNBusiness.Communicator.Content.GetByFolderID(4092, user, true);
            Assert.IsNotNull(content[0], "No Content in this folder");
        }

        [TestMethod]
        public void GetByTitle()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            DataSet testContent = ECNBusiness.Communicator.Content.GetByContentTitle("PBS Content", 4135, user.UserID, null, null, user, 1, 15, "assending", string.Empty);
            Assert.IsNotNull(testContent, "No Content");
        }

        [TestMethod]
        public void GetByContentSearch()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.Content> testContent = ECNBusiness.Communicator.Content.GetByContentSearch("PBS Content", 4135, user.UserID, null, null, user, true);
            Assert.IsNotNull(testContent, "No content with that title");
        }

        [TestMethod]
        public void GetContent()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            string scontent = null;
            scontent = ECNBusiness.Communicator.Content.GetContent(86840, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user);
            Assert.IsNotNull(scontent, "No content");
            
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Content content = ECNBusiness.Communicator.Content.GetByContentID(145142, user, true);
            content.UpdatedUserID = user.UserID;
            ECNBusiness.Communicator.Content.Save(content, user);
        }
    }
}
