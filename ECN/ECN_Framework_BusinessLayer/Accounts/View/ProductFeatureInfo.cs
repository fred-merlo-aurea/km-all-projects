using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountsEntity = ECN_Framework_Entities.Accounts;
using AccountsEntityView = ECN_Framework_Entities.Accounts.View;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;

namespace ECN_Framework_BusinessLayer.Accounts.View
{
    public static class ProductFeatureInfo
    {
        public static IEnumerable<AccountsEntityView.ProductFeatureInfo> GetProductFeatureInfo(int roleID, int customerID)
        {
            List<AccountsEntity.Product> products = AccountsBLL.Product.GetAll();
            List<AccountsEntity.Action> actions = AccountsBLL.Action.GetAll();
            List<AccountsEntity.RoleAction> roleActions = AccountsBLL.RoleAction.GetByRoleID(roleID, customerID);
            var activeCustomerProductFeatureIDs = GetActiveCustomerFeatureProductIDs(customerID);

            return from p in products
                   join a in actions on p.ProductID equals a.ProductID
                   join ra in roleActions on a.ActionID equals ra.ActionID
                   join acf in activeCustomerProductFeatureIDs on p.ProductID equals acf.ProductID
                   orderby a.DisplayOrder
                   select new AccountsEntityView.ProductFeatureInfo(0,
                                                                    a.ActionID,
                                                                    ra.Active,
                                                                    p.ProductID,
                                                                    p.ProductName,
                                                                    a.DisplayName,
                                                                    a.ActionCode,
                                                                    a.DisplayOrder);
        }

        public static IEnumerable<AccountsEntityView.ProductFeatureInfo> GetProductFeatureInfo(int roleID, int customerID, int userID)
        {
            var productFeatureList = GetProductFeatureInfo(roleID, customerID);
            List<AccountsEntity.UserAction> userActions = AccountsBLL.UserAction.GetbyUserID(userID);

            return from p in productFeatureList
                   join ua in userActions on p.ActionID equals ua.ActionID
                   orderby p.DisplayOrder
                   select new AccountsEntityView.ProductFeatureInfo(ua.UserActionID,
                                                                    p.ActionID,
                                                                    ua.Active,
                                                                    p.ProductID,
                                                                    p.ProductName,
                                                                    p.DisplayName,
                                                                    p.ActionCode,
                                                                    p.DisplayOrder);
        }

        public static IEnumerable<AccountsEntityView.ProductInfo> GetActiveCustomerFeatureProductIDs(int customerID)
        {
            List<AccountsEntity.CustomerProduct> customerProducts = AccountsBLL.CustomerProduct.GetbyCustomerID(customerID, false);
            List<AccountsEntity.ProductDetail> productDetails = AccountsBLL.ProductDetail.GetAll();
            List<AccountsEntity.Role> roles = AccountsBLL.Role.GetByCustomerID(customerID, false);

            return from pd in productDetails
                   join cp in customerProducts on pd.ProductDetailID equals cp.ProductDetailID
                   join r in roles on cp.CustomerID equals r.CustomerID
                   where cp.Active.ToUpper() == "Y"
                   group pd.ProductID by pd.ProductID into g
                   select new AccountsEntityView.ProductInfo(g.Key.Value);
        }
    }
}
