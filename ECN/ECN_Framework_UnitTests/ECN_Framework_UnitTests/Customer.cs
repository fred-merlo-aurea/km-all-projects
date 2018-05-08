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

using AccountsEntity = ECN_Framework_Entities.Accounts;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using CommunicatorBLL = ECN_Framework_BusinessLayer.Communicator;
using CommunicatorEntity = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_UnitTests
{
    [TestClass]
    public class Customer
    {
        [TestMethod]
        public void GetCurrentLicensesByCustomerID()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login(ConfigurationManager.AppSettings["currentUser"], ConfigurationManager.AppSettings["currentPassword"]);
            Assert.IsNotNull(user, "No User");

            ECNEntity.Accounts.License lic = ECNBusiness.Accounts.License.GetCurrentLicensesByCustomerID(1209, ECNCommon.Objects.Accounts.Enums.LicenseTypeCode.emailblock10k);

            Assert.IsNotNull(lic, "No Content");
        }

        [TestMethod]
        public void GetCustomerProductsWithACodeValue()
        {
            ECNEntity.Accounts.User user = ECNBusiness.Accounts.User.Login("sunil@knowledgemarketing.com", "winwin123");

            List<AccountsEntity.Product> products = AccountsBLL.Product.GetAll();
            List<AccountsEntity.Action> actions = AccountsBLL.Action.GetAll();
            List<AccountsEntity.RoleAction> roleActions = AccountsBLL.RoleAction.GetByRoleID(user.RoleID.Value, user.CustomerID.Value);

            List<AccountsEntity.CustomerProduct> customerProducts = AccountsBLL.CustomerProduct.GetbyCustomerID(1, false);
            List<AccountsEntity.ProductDetail> productDetails = AccountsBLL.ProductDetail.GetAll();
            List<AccountsEntity.Role> roles = AccountsBLL.Role.GetByCustomerID(1, false);
            List<CommunicatorEntity.Code> customerCodeList = CommunicatorBLL.Code.GetAllByCustomer(user);

            var activeCustomerFeatures = from r in roles
                                         join cp in customerProducts on r.CustomerID equals cp.CustomerID
                                         join pd in productDetails on cp.ProductDetailID equals pd.ProductDetailID
                                         join ccl in customerCodeList on cp.CustomerID equals ccl.CustomerID
                                         where cp.Active.ToUpper() == "Y"
                                         && ccl.CodeType == pd.ProductDetailDesc
                                         group pd by new { pd.ProductID, ccl.CodeValue } into g
                                         select new { ProductID = g.Key.ProductID, WebsiteAddress = g.Key.CodeValue };

            //List<AccountsEntity.UserAction> userActions = AccountsBLL.UserAction.GetbyUserID(user.UserID);

            Assert.IsTrue(activeCustomerFeatures.Count() > 0);
        }
    }
}
