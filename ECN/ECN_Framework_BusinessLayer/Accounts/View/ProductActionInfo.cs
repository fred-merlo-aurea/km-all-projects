using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountsEntity = ECN_Framework_Entities.Accounts;
using AccountsEntityView = ECN_Framework_Entities.Accounts.View;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using CommunicatorEntity = ECN_Framework_Entities.Communicator;
using CommunicatorBLL = ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Entities.Accounts.View;

namespace ECN_Framework_BusinessLayer.Accounts.View
{
    public class ProductActionInfo
    {
        public static IEnumerable<AccountsEntityView.ProductActionInfo> GetProductInfoForUserAndCustomer(KMPlatform.Entity.User user, int customerId)
        {
            List<AccountsEntity.Product> products = AccountsBLL.Product.GetAll();
            List<AccountsEntity.Action> actions = AccountsBLL.Action.GetAll();

            List<AccountsEntity.CustomerProduct> customerProducts = AccountsBLL.CustomerProduct.GetbyCustomerID(customerId, false);
            List<AccountsEntity.ProductDetail> productDetails = AccountsBLL.ProductDetail.GetAll();
            List<AccountsEntity.Role> roles = AccountsBLL.Role.GetByCustomerID(customerId, false);
            List<CommunicatorEntity.Code> customerCodeList = CommunicatorBLL.Code.GetAllByCustomer(user);

            var activeCustomerFeatures = from r in roles
                                         join cp in customerProducts on r.CustomerID equals cp.CustomerID
                                         join pd in productDetails on cp.ProductDetailID equals pd.ProductDetailID
                                         join ccl in customerCodeList on cp.CustomerID equals ccl.CustomerID
                                         where cp.Active.ToUpper() == "Y"
                                         && ccl.CodeType == pd.ProductDetailDesc
                                         group pd by new { pd.ProductID, ccl.CodeValue } into g
                                         select new { ProductID = g.Key.ProductID, WebsiteAddress = g.Key.CodeValue };

            List<AccountsEntity.UserAction> userActions = AccountsBLL.UserAction.GetbyUserID(user.UserID);

            return from p in products
                   join a in actions on p.ProductID equals a.ProductID
                   join ua in userActions on a.ActionID equals ua.ActionID
                   join acf in activeCustomerFeatures on p.ProductID equals acf.ProductID
                   where p.HasWebsiteTarget == true
                   && ua.Active.ToUpper() == "Y"
                   orderby a.DisplayOrder
                   select new AccountsEntityView.ProductActionInfo(ua.UserActionID,
                                                                   a.ActionID,
                                                                   ua.Active,
                                                                   p.ProductID,
                                                                   acf.WebsiteAddress,
                                                                   p.ProductName,
                                                                   a.DisplayName,
                                                                   a.ActionCode);
        }
    }
}
