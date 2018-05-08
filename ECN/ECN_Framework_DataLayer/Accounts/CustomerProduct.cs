using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class CustomerProduct
    {
        public static bool Exists(int productDetailID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerProduct_Exists_ByProductDetailID";
            cmd.Parameters.Add(new SqlParameter("@ProductDetailID", productDetailID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerProduct> GetbyCustomerID(int CustomerID)
        {
            List<ECN_Framework_Entities.Accounts.CustomerProduct> retItemList = new List<ECN_Framework_Entities.Accounts.CustomerProduct>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerProduct_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.CustomerProduct> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.CustomerProduct> retList = new List<ECN_Framework_Entities.Accounts.CustomerProduct>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            ECN_Framework_Entities.Accounts.CustomerProduct retItem = new ECN_Framework_Entities.Accounts.CustomerProduct();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerProduct>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);
                                retList.Add(retItem);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return retList;
        }

        public static int Save(ECN_Framework_Entities.Accounts.CustomerProduct customerProduct)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerProduct_Save";
            cmd.Parameters.Add(new SqlParameter("@CustomerProductID", customerProduct.CustomerProductID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerProduct.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@ProductDetailID", customerProduct.ProductDetailID));
            cmd.Parameters.Add(new SqlParameter("@Active", customerProduct.Active));
            if (customerProduct.CustomerProductID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerProduct.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerProduct.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

        public static void Update(int CPID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerProduct_UpdateActive_ByID";
            cmd.Parameters.Add(new SqlParameter("@CustomerProductID", CPID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

    }
}
