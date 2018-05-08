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
    public class Customer
    {
        private static string _CacheRegion = "Customer";
        public static bool Exists(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Customer_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static bool Exists(string customerName,int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Customer_Exists_ByName";
            cmd.Parameters.AddWithValue("@CustomerName", customerName);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Accounts.Customer GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Customer_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            ECN_Framework_Entities.Accounts.Customer retItem = null;

            //errors for salutation enum
            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                retItem = (ECN_Framework_Entities.Accounts.Customer)KM.Common.CacheUtil.GetFromCache(customerID.ToString(), _CacheRegion);
                if (retItem == null)
                {
                    retItem = Get(cmd);
                    KM.Common.CacheUtil.AddToCache(customerID.ToString(), retItem, _CacheRegion);
                }
            }
            else
                retItem = Get(cmd);

            return retItem;
        }

        public static ECN_Framework_Entities.Accounts.Customer GetByUserID(int userID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Customer_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Accounts.Customer GetByClientID(int clientID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Customer_Select_PlatformClientID";
            cmd.Parameters.AddWithValue("@PlatformClientID", clientID);
            return Get(cmd);
        }


        private static ECN_Framework_Entities.Accounts.Customer Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.Customer retItem = null;

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            retItem = new ECN_Framework_Entities.Accounts.Customer();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Customer>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);
                                string sal = rdr["Salutation"].ToString();
                                retItem.Salutation = sal.Contains(ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Mr.ToString()) ? ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Mr : sal.Contains(ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Ms.ToString()) ? ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Ms : ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
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
            

            return retItem;
        }

        public static List<ECN_Framework_Entities.Accounts.Customer> GetByBaseChannelID(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Select_Customer_ChannelID";
            cmd.Parameters.AddWithValue("@ChannelID", baseChannelID);
            return GetList(cmd, baseChannelID);
        }

        private static List<ECN_Framework_Entities.Accounts.Customer> GetList(SqlCommand cmd, int baseChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Customer> retList = new List<ECN_Framework_Entities.Accounts.Customer>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.Customer retItem = new ECN_Framework_Entities.Accounts.Customer();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Customer>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            if (retItem.BaseChannelID == baseChannelID)
                            {
                                string sal = rdr["Salutation"].ToString();
                                retItem.Salutation = sal.Contains(ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Mr.ToString()) ? ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Mr : sal.Contains(ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Ms.ToString()) ? ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Ms : ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
                                retList.Add(retItem);
                            }
                            else
                            {
                                retList = null;
                                throw new SecurityException("SECURITY VIOLATION!");
                            }
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static int Save(ECN_Framework_Entities.Accounts.Customer customer)
        {
            //errors for salutation enum
            //if (customer.CustomerID > 0)
            //    DeleteCache(customer.CustomerID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Customer_Save";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customer.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", customer.BaseChannelID));
            cmd.Parameters.Add(new SqlParameter("@CustomerName", customer.CustomerName));
            cmd.Parameters.Add(new SqlParameter("@ActiveFlag", customer.ActiveFlag));
            cmd.Parameters.Add(new SqlParameter("@Address", customer.GeneralContant.StreetAddress));
            cmd.Parameters.Add(new SqlParameter("@City", customer.GeneralContant.City));
            cmd.Parameters.Add(new SqlParameter("@State", customer.GeneralContant.State));
            cmd.Parameters.Add(new SqlParameter("@Country", customer.GeneralContant.Country));
            cmd.Parameters.Add(new SqlParameter("@Zip", customer.GeneralContant.Zip));
            cmd.Parameters.Add(new SqlParameter("@Salutation", customer.GeneralContant.Salutation.ToString()));
            cmd.Parameters.Add(new SqlParameter("@ContactName", customer.GeneralContant.ContactName));
            cmd.Parameters.Add(new SqlParameter("@FirstName", customer.GeneralContant.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", customer.GeneralContant.LastName));
            cmd.Parameters.Add(new SqlParameter("@ContactTitle", customer.GeneralContant.ContactTitle));
            cmd.Parameters.Add(new SqlParameter("@Email", customer.GeneralContant.Email));
            cmd.Parameters.Add(new SqlParameter("@Phone", customer.GeneralContant.Phone));
            cmd.Parameters.Add(new SqlParameter("@Fax", customer.GeneralContant.Fax));
            cmd.Parameters.Add(new SqlParameter("@WebAddress", customer.WebAddress));
            cmd.Parameters.Add(new SqlParameter("@TechContact", customer.TechContact));
            cmd.Parameters.Add(new SqlParameter("@TechEmail", customer.TechEmail));
            cmd.Parameters.Add(new SqlParameter("@TechPhone", customer.TechPhone));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionsEmail", customer.SubscriptionsEmail));
            cmd.Parameters.Add(new SqlParameter("@CommunicatorChannelID", DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CollectorChannelID", DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatorChannelID", DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PublisherChannelID", DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CharityChannelID", DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AccountsLevel", customer.AccountsLevel));
            cmd.Parameters.Add(new SqlParameter("@CommunicatorLevel", customer.CommunicatorLevel));
            cmd.Parameters.Add(new SqlParameter("@CollectorLevel", customer.CollectorLevel));
            cmd.Parameters.Add(new SqlParameter("@CreatorLevel", customer.CreatorLevel));
            cmd.Parameters.Add(new SqlParameter("@PublisherLevel", customer.PublisherLevel));
            cmd.Parameters.Add(new SqlParameter("@CharityLevel", customer.CharityLevel));
            cmd.Parameters.Add(new SqlParameter("@CustomerType", customer.CustomerType));
            cmd.Parameters.Add(new SqlParameter("@DemoFlag", customer.DemoFlag));
            cmd.Parameters.Add(new SqlParameter("@IsStrategic", customer.IsStrategic));
            cmd.Parameters.Add(new SqlParameter("@AccountExecutiveID", (object)customer.AccountExecutiveID??DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AccountManagerID", (object)customer.AccountManagerID??DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Customer_udf1", customer.customer_udf1));
            cmd.Parameters.Add(new SqlParameter("@Customer_udf2", customer.customer_udf2));
            cmd.Parameters.Add(new SqlParameter("@Customer_udf3", customer.customer_udf3));
            cmd.Parameters.Add(new SqlParameter("@Customer_udf4", customer.customer_udf4));
            cmd.Parameters.Add(new SqlParameter("@Customer_udf5", customer.customer_udf5));
            cmd.Parameters.Add(new SqlParameter("@ABWinnerType", customer.ABWinnerType));
            cmd.Parameters.Add(new SqlParameter("@BlastConfigID", customer.BlastConfigID));
            cmd.Parameters.Add(new SqlParameter("@DefaultBlastAsTest", customer.DefaultBlastAsTest.Value));
            cmd.Parameters.Add(new SqlParameter("@PlatformClientID", customer.PlatformClientID));

            if(customer.MSCustomerID != null)
                cmd.Parameters.Add(new SqlParameter("@MSCustomerID", customer.MSCustomerID));
            if (customer.CustomerID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customer.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customer.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

        private static void DeleteCache(int customerID)
        {
            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                if (KM.Common.CacheUtil.GetFromCache(customerID.ToString(), _CacheRegion) != null)
                    KM.Common.CacheUtil.RemoveFromCache(customerID.ToString(), _CacheRegion);
            }
        }

        //public void CreateDefaultFeatures(int CustomerID)
        //{

        //    List<ECN_Framework_Entities.Accounts.ProductDetail> lProductDetails = ProductDetail.GetData();
        //    foreach (ECN_Framework_Entities.Accounts.ProductDetail pd in lProductDetails)
        //    {
        //        if (DataFunctions.ExecuteScalar("accounts", "select ProductDetailID from CustomerProduct where CustomerID = " + CustomerID + " and ProductDetailID = " + pd.ProductDetailID) == null)
        //        {
        //            DataFunctions.Execute("accounts", "insert into CustomerProduct (CustomerID, ProductDetailID, ModifyDate) values (" + CustomerID + "," + pd.ProductDetailID + ",getdate())");
        //        }
        //    }
        //}

        //public void CreateDefaulRole()
        //{
        //    string role_id = DataFunctions.ExecuteScalar("accounts", "insert into Roles (CustomerID, RoleName) values (" + ID.ToString() + ",'Everything');SELECT @@IDENTITY").ToString();

        //    DataTable action_ids = DataFunctions.GetDataTable("select * from Action", ConfigurationManager.AppSettings["act"].ToString());
        //    foreach (DataRow aid in action_ids.Rows)
        //    {
        //        // added the following condition 'cos makes the "Create Approval Messages" check active by default & people forget to UNCheck it. Ref. Iris mail via Darin on 10/23/07
        //        //- ashok 10/23/07
        //        if (aid["ActionCode"].ToString().Trim().Equals("approvalblast"))
        //        {
        //            DataFunctions.Execute("accounts", "insert into RoleActions (RoleID , ActionID, Active) values (" + role_id + "," + aid["ActionID"].ToString() + ",'N')");
        //        }
        //        else
        //        {
        //            DataFunctions.Execute("accounts", "insert into RoleActions (RoleID , ActionID, Active) values (" + role_id + "," + aid["ActionID"].ToString() + ",'Y')");
        //        }
        //    }
        //}


        //public void CreateMasterSupressionGroup()
        //{
        //    AssertCustomerIsSaved("Create master supression group");
        //    string communicator_db = ConfigurationManager.AppSettings["communicatordb"];
        //    DataFunctions.Execute("insert into " + communicator_db + ".dbo.Groups (CustomerID , GroupName, OwnerTypeCode,MasterSupression) values (" + ID.ToString() + ",'Master Supression','customer',1)");
        //}
    }
}
