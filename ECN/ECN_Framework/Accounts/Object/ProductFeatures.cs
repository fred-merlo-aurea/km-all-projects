using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Object
{
    [Serializable]
    public class ProductFeatures
    {
        public ProductFeatures() { }
        #region Properties
        public string ProductName { get; set; }
        public string FeatureName { get; set; }
        public bool Active { get; set; }
        #endregion

        #region Data
        public static List<ProductFeatures> Get(int userID)
        {
            List<ProductFeatures> retItemList = new List<ProductFeatures>();
            string sqlQuery = "SELECT p.ProductName, pd.productdetailName as FeatureName, case when cp.Active = 'y' then 1 else 0 end as Active FROM [CustomerProduct] cp JOIN [ProductDetail] pd ON cp.ProductDetailID = pd.ProductDetailID JOIN Product p ON pd.ProductID = p.ProductID JOIN [Users] U on U.customerID = cp.customerID and U.IsDeleted = 0 where userID = @UserID and U.IsDeleted = 0 and cp.IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserID", userID);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                var builder = DynamicBuilder<ProductFeatures>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    ProductFeatures retItem = builder.Build(rdr);
                    retItemList.Add(retItem);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();

            return retItemList;
        }

        public static bool HasFeature(int userID, string product, string feature)
        {
            string sqlQuery = "SELECT COUNT(pd.ProductDetailID) FROM [CustomerProduct] cp JOIN [ProductDetail] pd ON cp.ProductDetailID = pd.ProductDetailID JOIN Product p ON pd.ProductID = p.ProductID JOIN [Users] U on U.customerID = cp.customerID and U.IsDeleted = 0 where userID = @UserID and p.ProductName = @Product and pd.ProductDetailName = @Feature and cp.IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@Product", product);
            cmd.Parameters.AddWithValue("@Feature", feature);

            if (Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString())) >= 1)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
