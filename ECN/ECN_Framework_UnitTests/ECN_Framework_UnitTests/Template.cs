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
    /// Summary description for Template
    /// </summary>
    [TestClass]
    public class Template
    {
        [TestMethod]
        public void GetByChannelID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.Template> template = ECNBusiness.Communicator.Template.GetByBaseChannelID(12, user);
            Assert.IsNotNull(template[0], "No templates");
        }
        [TestMethod]
        public void GetByStyleCode()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            List<ECNEntity.Communicator.Template> template = ECNBusiness.Communicator.Template.GetByStyleCode(12, "newsletter", user);
            Assert.IsNotNull(template[0], "No Templates");
        }

        [TestMethod]
        public void GetByTemplateID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Template template = ECNBusiness.Communicator.Template.GetByTemplateID(61, user);
            Assert.IsNotNull(template, "No Template");
            
        }

        [TestMethod]
        public void Save()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Communicator.Template template = ECNBusiness.Communicator.Template.GetByTemplateID(61, user);
            Assert.IsNotNull(template, "No Template");
            template.UpdatedUserID = user.UserID;
            int? test = null;
            test = ECNBusiness.Communicator.Template.Save(template, user);
            Assert.IsNotNull(test, "Template did not save");
        }
    }
}
