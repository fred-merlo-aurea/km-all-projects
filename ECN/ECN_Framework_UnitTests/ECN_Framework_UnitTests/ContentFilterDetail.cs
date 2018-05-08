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
    /// Summary description for ContentFilterDetail
    /// </summary>
    [TestClass]
    public class ContentFilterDetail
    {
        [TestMethod]
        public void GetByContentIDFilterID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");
            //No records for CustomerID = 1
            DataTable cfd = ECNBusiness.Communicator.ContentFilterDetail.GetByContentIDFilterID(601, user);
            Assert.IsNotNull(cfd.Rows[0], "No ContentFilterDetail for this FilterID");
        }

        [TestMethod]
        public void GetByFDID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.ContentFilterDetail cfd = ECNBusiness.Communicator.ContentFilterDetail.GetByFDID(1067, user);
            Assert.IsNotNull(cfd, "No ContentFilterDetail for this FDID");
        }

        [TestMethod]
        public void GetByFilterID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.ContentFilterDetail> cfd = ECNBusiness.Communicator.ContentFilterDetail.GetByFilterID(667, user);
            Assert.IsNotNull(cfd[0], "No ContentFilterDetail for this FilterID");
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.GetByUserID(1664, false);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.ContentFilterDetail detail = ECNBusiness.Communicator.ContentFilterDetail.GetByFDID(1067, user);
            
            detail.CreatedUserID = user.UserID;
            
            detail.FieldType = "VARCHAR";
            
            detail.UpdatedUserID = user.UserID;
            detail.CompareValue = "Test";

            Assert.IsNotNull(detail, "No ContentFilterDetail for this FDID");
            
            ECNBusiness.Communicator.ContentFilterDetail.Save(detail, user);
        }
    }
}
