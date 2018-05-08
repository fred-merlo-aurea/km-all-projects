using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECNBusiness = ECN_Framework_BusinessLayer;
using ECNEntity = ECN_Framework_Entities;
using ECNCommon = ECN_Framework_Common;
using System.Configuration;
using System.Data;

namespace ECN_Framework_UnitTests
{
    [TestClass]
    public class Group
    {
        [TestMethod]
        public void GroupGet()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            Assert.IsNotNull(user, "User login error");

            List<ECNEntity.Communicator.Group> groupList = ECNBusiness.Communicator.Group.GetByCustomerID(user.CustomerID.Value, user);
            
            Assert.IsNotNull(groupList, "NO groups for this User");

        }

        [TestMethod]
        public void GroupGetByFolderID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            Assert.IsNotNull(user, "User Login Error");
            
            List<ECNEntity.Communicator.Group> groupList = ECNBusiness.Communicator.Group.GetByFolderID(4134, user);
            Assert.IsNotNull(groupList[0], "No groups in this folder");
        }
        [TestMethod]
        public void GroupGetByGroupID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Group group = ECNBusiness.Communicator.Group.GetByGroupID(50085, user);
            Assert.IsNotNull(group, "No group for this user");
        }
        [TestMethod]
        public void GroupGetByProfileName()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            Assert.IsNotNull(user, "No User");

            System.Data.DataTable group = ECNBusiness.Communicator.Group.GetByProfileName("test", "test", user);
            Assert.IsNotNull(group.Rows[0], "No groups with that name");
        }

        [TestMethod]
        public void GroupGetByGroupName()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            List<ECNEntity.Communicator.Group> tempGroup = ECNBusiness.Communicator.Group.GetByCustomerID(user.CustomerID.Value, user);
            
            System.Data.DataTable group = ECNBusiness.Communicator.Group.GetByGroupName("test", "test", user);
            Assert.IsNotNull(group, "No Groups with that group Name");
        }

        [TestMethod]
        public void GroupGetByDR()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            Assert.IsNotNull(user, "No User");
            DataTable group = ECNBusiness.Communicator.Group.GetGroupDR(user.CustomerID.Value, user.UserID, user);
            Assert.IsNotNull(group.Rows[0], "No Groups for this user");


        }

        [TestMethod]
        public void GroupGetNonTransactional()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            Assert.IsNotNull(user, "No User");
            DataTable group = ECNBusiness.Communicator.Group.GetNONTransactional(user.CustomerID.Value, user);

            Assert.IsNotNull(group.Rows[0], "NO Group for that user");

        }

        [TestMethod]
        public void GroupGetSubscribers()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            Assert.IsNotNull(user, "No User");
            
            DataTable group = ECNBusiness.Communicator.Group.GetSubscribers(4091, user);
            Assert.IsNotNull(group.Rows[0], "No Group for this folder");
        }
        [TestMethod]
        public void GroupGetTransactional()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"].ToString(), ConfigurationManager.AppSettings["currentPassword"].ToString());
            Assert.IsNotNull(user, "No User");
            DataTable group = ECNBusiness.Communicator.Group.GetTransactional(user.CustomerID.Value, user);
            Assert.IsNotNull(group.Rows[0], "No group for this user");
        }

        [TestMethod]
        public void GroupSave()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login("sunil@knowledgemarketing.com", "winwin123");
            Assert.IsNotNull(user, "User login error");

            List<ECNEntity.Communicator.Group> groupList = ECNBusiness.Communicator.Group.GetByCustomerID(user.CustomerID.Value, user);

            Parallel.ForEach(groupList, group =>
            {
                try
                {
                    group.FolderID = -1;
                    group.UpdatedUserID = user.UserID;
                    group.CreatedUserID = group.CreatedUserID;
                    ECNBusiness.Communicator.Group.Save(group, user);

                    System.Diagnostics.Debug.WriteLine(group.GroupName + " saved");
                }
                catch (ECNCommon.Objects.ECNException ecnex)
                {

                    StringBuilder sb = new StringBuilder();

                    foreach (ECNCommon.Objects.ECNError err in ecnex.ErrorList)
                    {
                        sb.Append(err.ErrorMessage + "<BR />");
                    }
                    System.Diagnostics.Debug.WriteLine("Error Saving Group " + group.GroupName + "  : " + sb.ToString());
                    Assert.Fail(" SAVE ERROR" + sb);
                }

            });
        }

        [TestMethod]
        public void GroupValidation()
        {
            ECNEntity.Accounts.User u = ECNBusiness.Accounts.User.Login("sunil@knowledgemarketing.com", "winwin123");
            Assert.IsNotNull(u, "User login error");

            List<ECNEntity.Communicator.Group> g = ECNBusiness.Communicator.Group.GetByCustomerID(u.CustomerID.Value, u);

            Parallel.ForEach(g, grp =>
            {

                bool gexist = ECNBusiness.Communicator.Group.Exists(grp.GroupID, u.CustomerID.Value);

                System.Diagnostics.Debug.WriteLine(grp.GroupName + " group exists");
            });
        }
    }
}
