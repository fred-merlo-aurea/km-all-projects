using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ECN.engines.Utility
{
    public class BlastInfo
    {
        public BlastInfo()
        {
            blastID = 0;
            customerID = 0;
            customerName = "";
            emailSubject = "";
            sendTime = DateTime.MinValue;
        }
        #region Private Variables
        private int blastID;
        private int customerID;
        private string customerName;
        private string emailSubject;
        private DateTime sendTime;
        #endregion

        #region Public Properties
        public int BlastID
        {
            get
            {
                return blastID;
            }
            set
            {
                blastID = value;
            }
        }
        public int CustomerID
        {
            get
            {
                return customerID;
            }
            set
            {
                customerID = value;
            }
        }
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
            }
        }
        public string EmailSubject
        {
            get
            {
                return emailSubject;
            }
            set
            {
                emailSubject = value;
            }
        }
        public DateTime SendTime
        {
            get
            {
                return sendTime;
            }
            set
            {
                sendTime = value;
            }
        }
        #endregion

        public static BlastInfo GetBlastInfo(int blastID)
        {
            BlastInfo bl = null;
            
            SqlConnection myConnection = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand myCommand = new SqlCommand("select b.*, c.CustomerName from [blast] b join ecn5_accounts..Customer c on b.CustomerID = c.CustomerID where b.BlastID = @BlastID");
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandTimeout = 0;
            myCommand.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            myCommand.Parameters["@BlastID"].Value = blastID;
            
            SqlDataReader myReader = null;
            try
            {
                myConnection.Open();
                myCommand.Connection = myConnection;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    bl = new BlastInfo();
                    if (myReader["BlastID"] != DBNull.Value)
                    {
                        bl.BlastID = (int)myReader["BlastID"];
                    }
                    if (myReader["CustomerID"] != DBNull.Value)
                    {
                        bl.CustomerID = (int)myReader["CustomerID"];
                    }
                    if (myReader["CustomerName"] != DBNull.Value)
                    {
                        bl.CustomerName = (string)myReader["CustomerName"];
                    }
                    if (myReader["EmailSubject"] != DBNull.Value)
                    {
                        bl.EmailSubject = (string)myReader["EmailSubject"];
                    }
                    if (myReader["SendTime"] != DBNull.Value)
                    {
                        bl.SendTime = (DateTime)myReader["SendTime"];
                    }
                    break;

                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myReader.Close();
                myConnection.Close();
            }

            return bl;
        }
    }
}
