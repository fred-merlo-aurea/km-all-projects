using System;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Configuration;

namespace ecn.communicator.classes
{
    class CustomerMTA
    {
        private const string ColumnNameCustomerId = "CustomerID";
        private const string ColumnNameMtaId = "MTAID";
        private const string ColumnNameMtaName = "MTAName";
        private const string ColumnNameDomainName = "DomainName";
        private const string ColumnNameIsDefault = "IsDefault";
        
        #region Private Properties
        private int _MTAID;
        private int _CustomerID;
        private string _MTAName;
        private string _DomainName;
        private bool _IsDefault;
        private List<CustomerIP> _CIP;
        #endregion

        #region Public Properties
        public int MTAID// { get; set; }
        {
            get
            {
                return _MTAID;
            }
            set
            {
                _MTAID = value;
            }
        }
        public int CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                _CustomerID = value;
            }
        }
        public string MTAName
        {
            get
            {
                return _MTAName;
            }
            set
            {
                _MTAName = value;
            }
        }
        public string DomainName
        {
            get
            {
                return _DomainName;
            }
            set
            {
                _DomainName = value;
            }
        }
        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }
            set
            {
                _IsDefault = value;
            }
        }
        public List<CustomerIP> CIP
        {
            get
            {
                return _CIP;
            }
            set
            {
                _CIP = value;
            }
        }
        #endregion

        #region Public Methods
        public static void UpdateMTA(CustomerMTA mta)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText =   "update CustomerMTA set CustomerID = @CustomerID, MTAName = @MTAName, DomainName = @DomainName, IsDefault = @IsDefault " +
                                "where MTAID = @MTAID";
            cmd.Parameters.Add(new SqlParameter("@MTAID", mta._MTAID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", mta._CustomerID));
            cmd.Parameters.Add(new SqlParameter("@MTAName", mta._MTAName));
            cmd.Parameters.Add(new SqlParameter("@DomainName", mta._DomainName));
            cmd.Parameters.Add(new SqlParameter("@IsDefault", mta._IsDefault));
            DataFunctions.Execute(ConfigurationManager.AppSettings["connString"], cmd);
        }
        public static List<CustomerMTA> GetMTAs()
        {
            List<CustomerMTA> mtaList = new List<CustomerMTA>();
            string sqlSelect =  "Select MTAID, CustomerID, MTAName, DomainName, IsDefault " +
                                "From CustomerMTA " +
                                "Order By MTAName ASC";

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var mtaCustomer = ReadCustomerMta(reader);
                    mtaList.Add(mtaCustomer);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return mtaList;
        }
        public static List<CustomerMTA> GetMTAs(int customerID)
        {
            List<CustomerMTA> mtaList = new List<CustomerMTA>();
            string sqlSelect = "Select MTAID, CustomerID, MTAName, DomainName, IsDefault " +
                                "From CustomerMTA " +
                                "Where CustomerID = @CustomerID " +
                                "Order By MTAName ASC";

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var mtaCustomer = ReadCustomerMta(reader);
                    mtaList.Add(mtaCustomer);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return mtaList;
        }
        public static CustomerMTA GetMTAOrDefault(int customerID, string domainName)
        {
            CustomerMTA mtaDefault = null;
            CustomerMTA mtaReturn = null;
            string sqlSelect = "Select m.MTAID, CustomerID, MTAName, DomainName, IsDefault From MTACustomer mc join MTA m on m.mtaID = mc.mtaID Where CustomerID = @CustomerID Order By MTAName ASC";

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var mta = ReadCustomerMta(reader);

                    if (mta.IsDefault)
                    {
                        mtaDefault = mta;
                    }
                    if (string.Compare(mta._DomainName.Trim(), domainName.Trim(), true) == 0)
                    {
                        mtaReturn = mta;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            if (mtaReturn == null)
            {
                mtaReturn = mtaDefault;
            }

            return mtaReturn;
        }
        #endregion

        public static CustomerMTA ReadCustomerMta(SqlDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var customer = new CustomerMTA();

            if (reader[ColumnNameMtaId] != DBNull.Value)
            {
                customer._MTAID = (int) reader[ColumnNameMtaId];
            }

            if (reader[ColumnNameCustomerId] != DBNull.Value)
            {
                customer._CustomerID = (int) reader[ColumnNameCustomerId];
            }

            if (reader[ColumnNameMtaName] != DBNull.Value)
            {
                customer._MTAName = (string) reader[ColumnNameMtaName];
            }

            if (reader[ColumnNameDomainName] != DBNull.Value)
            {
                customer._DomainName = (string) reader[ColumnNameDomainName];
            }

            if (reader[ColumnNameIsDefault] != DBNull.Value)
            {
                customer._IsDefault = Convert.ToBoolean(reader[ColumnNameIsDefault].ToString());
            }

            customer._CIP = CustomerIP.GetIPs(customer._MTAID);
            return customer;
        }
    }
   
    public class CustomerIP
    {
        private const string ColumnNameIpId = "IPID";
        private const string ColumnNameMtaId = "MTAID";
        private const string ColumnNameIpAddress = "IPAddress";
        private const string ColumnNameHostName = "HostName";

        #region Private Properties
        private int _IPID;
        private int _MTAID;
        private string _IPAddress;
        private string _HostName;
        #endregion

        #region Public Properties
        public int IPID
        {
            get
            {
                return _IPID;
            }
            set
            {
                _IPID = value;
            }
        }
        public int MTAID// { get; set; }
        {
            get
            {
                return _MTAID;
            }
            set
            {
                _MTAID = value;
            }
        }
        public string IPAddress
        {
            get
            {
                return _IPAddress;
            }
            set
            {
                _IPAddress = value;
            }
        }
        public string HostName
        {
            get
            {
                return _HostName;
            }
            set
            {
                _HostName = value;
            }
        }
        #endregion

        #region Public Methods
        public static void UpdateIP(CustomerIP ip)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update MTAIP set MTAID = @MTAID, IPAddress = @IPAddress, HostName = @HostName " +
                                "where IPID = @IPID";
            cmd.Parameters.Add(new SqlParameter("@MTAID", ip._MTAID));
            cmd.Parameters.Add(new SqlParameter("@IPID", ip._IPID));
            cmd.Parameters.Add(new SqlParameter("@IPAddress", ip._IPAddress));
            cmd.Parameters.Add(new SqlParameter("@HostName", ip._HostName));
            DataFunctions.Execute(ConfigurationManager.AppSettings["connString"], cmd);
        }
        public static List<CustomerIP> GetIPs(int mtaID)
        {
            List<CustomerIP> cipList = new List<CustomerIP>();
            string sqlSelect = "Select IPID, MTAID, IPAddress, HostName " +
                                "From MTAIP " +
                                "Where MTAID = @MTAID " +
                                "Order By HostName ASC";

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@MTAID", mtaID));
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var cip = ReadCustomerIp(reader);
                    cipList.Add(cip);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return cipList;
        }
        public static CustomerIP GetIP(int ipID)
        {
            CustomerIP cip = null;
            string sqlSelect = "Select IPID, MTAID, IPAddress, HostName " +
                                "From CustomerIP " +
                                "Where IPID = @IPID " +
                                "Order By HostName ASC";

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@IPID", ipID));
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cip = ReadCustomerIp(reader);
                    break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return cip;
        }

        public static CustomerIP ReadCustomerIp(SqlDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var customerIp = new CustomerIP();
            
            if (reader[ColumnNameIpId] != DBNull.Value)
            {
                customerIp._IPID = (int)reader[ColumnNameIpId];
            }

            if (reader[ColumnNameMtaId] != DBNull.Value)
            {
                customerIp._MTAID = (int)reader[ColumnNameMtaId];
            }

            if (reader[ColumnNameIpAddress] != DBNull.Value)
            {
                customerIp._IPAddress = (string)reader[ColumnNameIpAddress];
            }

            if (reader[ColumnNameHostName] != DBNull.Value)
            {
                customerIp._HostName = (string)reader[ColumnNameHostName];
            }

            return customerIp;
        }
    }
    #endregion
}
