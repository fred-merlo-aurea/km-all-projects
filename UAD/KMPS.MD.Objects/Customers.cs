using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class Customers
    {
        public Customers() { }
        #region Properties
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        #endregion

        #region Data
        public static List<Customers> GetCustomersByBaseChannelID(int BaseChannelID)
        {
            List<Customers> retList = new List<Customers>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select CustomerID, CustomerName FROM Customer where IsDeleted = 0 and ActiveFlag='Y' and BaseChannelID = @BaseChannelID order by CustomerName", conn);
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", BaseChannelID));
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Customers> builder = DynamicBuilder<Customers>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Customers x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static List<Customers> GetAllCustomers()
        {
            List<Customers> retList = new List<Customers>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select CustomerID, CustomerName FROM Customer where IsDeleted = 0 and ActiveFlag='Y' order by CustomerName", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Customers> builder = DynamicBuilder<Customers>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Customers x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static List<Customers> GetCustomersByIDs(string CustomerIDs)
        {
            List<Customers> retList = new List<Customers>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT CustomerID, CustomerName FROM Customer c WHERE IsDeleted = 0 and c.customerID in (" + CustomerIDs + ") order by  CustomerName", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Customers> builder = DynamicBuilder<Customers>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Customers x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }
        #endregion
    }
}
        