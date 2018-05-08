using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GreenArrow_SMTPLog.DAL
{
    public static class Blasts
    {
        public static List<Models.Blasts> GetBlastsForCustomer()
        {
            Models.Blasts blast = null;
            List<Models.Blasts> blastList = new List<Models.Blasts>();
            SqlDataReader myReader = null;
            SqlConnection myConnection = null;

            string customerIDs = GetCustomers();
            if (customerIDs.Length > 0)
            {
                try
                {
                    myConnection = new SqlConnection(GetConnectionString());
                    string selectSQL = "SELECT b.*,c.CustomerName FROM   Blasts b JOIN ecn5_accounts..Customer c ON b.CustomerID = c.CustomerID WHERE  b.CustomerID in (" + customerIDs + ")";
                    if (ConfigurationManager.AppSettings["ProcessType"].ToString() == "SingleDay")
                    {
                        selectSQL += " AND CONVERT(VARCHAR(10),b.SendTime, 101) = CONVERT(VARCHAR(10),DATEADD(dd," + ConfigurationManager.AppSettings["DayToProcess"].ToString() + ",GETDATE()), 101)";
                    }
                    else if (ConfigurationManager.AppSettings["ProcessType"].ToString() == "DateRange")
                    {
                        selectSQL += " AND (b.SendTime BETWEEN CONVERT(DATETIME,'" + ConfigurationManager.AppSettings["StartDate"].ToString() + "') AND CONVERT(DATETIME,'" + ConfigurationManager.AppSettings["EndDate"].ToString() + "'))";
                    }
                    else if (ConfigurationManager.AppSettings["ProcessType"].ToString() == "BlastList")
                    {
                        selectSQL += " AND b.blastid in (" + ConfigurationManager.AppSettings["BlastList"].ToString() + ")";
                    }
                    else
                    {
                        throw new Exception("Unknown ProcessType in app.config");
                    }
                    selectSQL += " AND b.TestBlast = 'n' AND b.StatusCode = 'sent' ORDER BY b.CustomerID ASC, b.BlastID ASC";
                    SqlCommand myCommand = new SqlCommand(selectSQL);
                    //SqlCommand myCommand = new SqlCommand("SELECT b.*,c.CustomerName " +
                    //                                        "FROM   Blasts b JOIN ecn5_accounts..Customer c ON b.CustomerID = c.CustomerID " +
                    //                                        "WHERE  b.CustomerID in (" + customerIDs + ") AND " +
                    //                                                //single day
                    //                                               "CONVERT(VARCHAR(10),b.SendTime, 101) = CONVERT(VARCHAR(10),DATEADD(dd," + ConfigurationManager.AppSettings["DayToProcess"].ToString() + ",GETDATE()), 101)  AND b.TestBlast = 'n' AND b.StatusCode = 'sent' " +
                    //                                               //date range
                    //                                               //"(b.SendTime BETWEEN CONVERT(DATETIME,'8/16/2011 00:00:01.000') AND CONVERT(DATETIME,'9/12/2011 23:59:59.000'))  AND b.TestBlast = 'n' AND b.StatusCode = 'sent' " +
                    //                                               //single blast
                    //                                               //"b.blastid = 410590  AND b.TestBlast = 'n' AND b.StatusCode = 'sent' " +
                    //                                        "ORDER BY b.CustomerID ASC, b.BlastID ASC");
                    myCommand.CommandType = CommandType.Text;
                    myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);

                    
                
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        blast = new Models.Blasts();
                        if (myReader["BlastID"] != DBNull.Value)
                        {
                            blast.BlastID = (int)myReader["BlastID"];
                        }
                        if (myReader["CustomerID"] != DBNull.Value)
                        {
                            blast.CustomerID = (int)myReader["CustomerID"];
                        }
                        if (myReader["SendTime"] != DBNull.Value)
                        {
                            blast.SendTime = (DateTime)myReader["SendTime"];
                        }
                        if (myReader["EmailSubject"] != DBNull.Value)
                        {
                            blast.EmailSubject = (string)myReader["EmailSubject"].ToString().Trim();
                        }
                        if (myReader["EmailFrom"] != DBNull.Value)
                        {
                            blast.EmailFrom = (string)myReader["EmailFrom"].ToString().Trim();
                        }
                        if (myReader["CustomerName"] != DBNull.Value)
                        {
                            blast.CustomerName = (string)myReader["CustomerName"].ToString().Trim();
                        }
                        blastList.Add(blast);
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
            }

            return blastList;
        }

        private static string GetCustomers()
        {
            string customerIDs = "";
            try
            {
                customerIDs = ConfigurationManager.AppSettings["CustomerIDs"].ToString().Trim().Replace(" ", "");
                string baseChannelIDs = ConfigurationManager.AppSettings["BaseChannelIDs"].ToString().Trim().Replace(" ", "");
                if (baseChannelIDs.Length > 0 && baseChannelIDs.Substring(baseChannelIDs.Length - 1, 1) == ",")
                {
                    baseChannelIDs = baseChannelIDs.Substring(0, baseChannelIDs.Length - 1);
                }
                if (baseChannelIDs.Length > 0 && baseChannelIDs.Substring(0, 1) == ",")
                {
                    baseChannelIDs = baseChannelIDs.Substring(1, baseChannelIDs.Length - 1);
                }
                if (baseChannelIDs.Length > 0)
                {
                    SqlConnection myConnection = new SqlConnection(GetConnectionString());
                    SqlCommand myCommand = new SqlCommand("SELECT CustomerID from ecn5_accounts..Customer WHERE BaseChannelID in (" + baseChannelIDs + ") ORDER BY CustomerID ASC");
                    myCommand.Parameters.Add(new SqlParameter("@BaseChannelID", SqlDbType.Int));
                    myCommand.Parameters["@BaseChannelID"].Value = Convert.ToInt32(ConfigurationManager.AppSettings["BaseChannelID"]);
                    myCommand.CommandType = CommandType.Text;
                    myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);
                    SqlDataAdapter da = new SqlDataAdapter(myCommand);
                    DataSet ds = new DataSet();
                    myConnection.Open();
                    myCommand.Connection = myConnection;
                    da.Fill(ds, "spresult");
                    myConnection.Close();
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (customerIDs.Length > 0 && customerIDs.Substring(customerIDs.Length - 1, 1) != ",")
                        {
                            customerIDs += ",";
                        }
                        foreach (DataRow row in dt.Rows)
                        {
                            customerIDs += row["CustomerID"].ToString() + ",";
                        }
                    }
                }

                if (customerIDs.Length > 0 && customerIDs.Substring(customerIDs.Length - 1, 1) == ",")
                {
                    customerIDs = customerIDs.Substring(0, customerIDs.Length - 1);
                }
                if (customerIDs.Length > 0 && customerIDs.Substring(0, 1) == ",")
                {
                    customerIDs = customerIDs.Substring(1, customerIDs.Length - 1);
                }
            }
            catch (Exception)
            {
            }
            return customerIDs;
        }

        private static string GetConnectionString()
        {
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings connection in connections)
            {
                if (connection.Name == "ecn5_CommunicatorConnectionString")
                {
                    return connection.ConnectionString;
                }
            }

            return "";
        }
    }

    public static class EmailActivityLog
    {
        public static List<Models.EmailActivityLog> GetEmailsForBlast(int blastID)
        {
            Models.EmailActivityLog eal = null;
            List<Models.EmailActivityLog> ealList = new List<Models.EmailActivityLog>();

            SqlConnection myConnection = new SqlConnection(GetConnectionString());
            SqlCommand myCommand = new SqlCommand("SELECT eal.*, e.EmailAddress, REPLACE(REPLACE(REPLACE(eal.actionnotes, CHAR(10), ' '), CHAR(13), ' '), CHAR(9), ' ') as ActionNotesModified " +
                                                    "FROM   EmailActivityLog eal JOIN Emails e on eal.EmailID = e.EmailID " +
                                                    "WHERE  eal.BlastID = @BlastID AND " +
                                                           "eal.ActionTypeCode in ('send','bounce') ORDER BY e.EmailAddress ASC, eal.ActionTypeCode ASC");
            myCommand.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            myCommand.Parameters["@BlastID"].Value = blastID;
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);

            SqlDataReader myReader = null;
            try
            {
                myConnection.Open();
                myCommand.Connection = myConnection;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    eal = new Models.EmailActivityLog();
                    if (myReader["EAID"] != DBNull.Value)
                    {
                        eal.EAID = (int)myReader["EAID"];
                    }
                    if (myReader["BlastID"] != DBNull.Value)
                    {
                        eal.BlastID = (int)myReader["BlastID"];
                    }
                    if (myReader["EmailID"] != DBNull.Value)
                    {
                        eal.EmailID = (int)myReader["EmailID"];
                    }
                    if (myReader["ActionTypeCode"] != DBNull.Value)
                    {
                        eal.ActionTypeCode = (string)myReader["ActionTypeCode"].ToString().Trim();
                    }
                    if (myReader["EmailAddress"] != DBNull.Value)
                    {
                        string temp = (string)myReader["EmailAddress"].ToString().Trim();
                        if ((temp.IndexOf("@") > 0) && (temp.IndexOf("@") != (temp.Length - 1)))
                        {
                            try
                            {
                                eal.EmailName = temp.Substring(0, temp.IndexOf("@"));
                                eal.EmailDomain = temp.Substring(temp.IndexOf("@") + 1, temp.Length - (temp.IndexOf("@") + 1));
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    if (myReader["ActionDate"] != DBNull.Value)
                    {
                        eal.ActionDate = (DateTime)myReader["ActionDate"];
                    }
                    if (myReader["ActionNotesModified"] != DBNull.Value)
                    {
                        eal.ActionNotes = (string)myReader["ActionNotesModified"].ToString().Trim();
                    }
                    ealList.Add(eal);
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

            return ealList;
        }

        private static string GetConnectionString()
        {
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings connection in connections)
            {
                if (connection.Name == "ecn5_CommunicatorConnectionString")
                {
                    return connection.ConnectionString;
                }
            }

            return "";
        }
    }


}
