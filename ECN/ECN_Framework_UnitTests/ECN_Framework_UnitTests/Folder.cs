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
    /// Summary description for Folder
    /// </summary>
    [TestClass]
    public class Folder
    {
        [TestMethod]
        public void GetFolderByCustomerID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.Folder> folder = ECNBusiness.Communicator.Folder.GetByCustomerID(user.CustomerID.Value, user);
            Assert.IsNotNull(folder, "no folders for that user");
        }

        [TestMethod]
        public void GetFolderByFolderID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Folder testFolder = ECNBusiness.Communicator.Folder.GetByFolderID(4092, user);
            Assert.IsNotNull(testFolder, "No Folder with that FolderID");
        }

        [TestMethod]
        public void GetFolderByType()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.Folder> folder = ECNBusiness.Communicator.Folder.GetByType(user.CustomerID.Value, "CNT", user);
            Assert.IsNotNull(folder[0], "No Folders for user");
        }

        [TestMethod]
        public void GetFolderTree()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            DataTable folder = ECNBusiness.Communicator.Folder.GetFolderTree(user.CustomerID.Value, user.UserID, "CNT", user);
            Assert.IsNotNull(folder.Rows[0], "No Folders");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Folder testFolder = ECNBusiness.Communicator.Folder.GetByFolderID(4806, user);
            Assert.IsNotNull(testFolder, "No Folder with that FolderID");

            testFolder.UpdatedUserID = user.UserID;

            ECNBusiness.Communicator.Folder.Save(testFolder, user);
        }
    }
}
