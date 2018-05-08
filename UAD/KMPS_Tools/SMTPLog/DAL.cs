using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KMPS_Tools.DAL
{
    public static class Blasts
    {
        public static Models.Blasts GetBlastsInfo(int blastID)
        {
            Models.Blasts blast = null;
            SqlDataReader myReader = null;
            SqlConnection myConnection = null;

            try
            {
                myConnection = new SqlConnection(GetConnectionString());
                string selectSQL = "SELECT b.*,c.CustomerName FROM   Blast b JOIN ecn5_accounts..Customer c ON b.CustomerID = c.CustomerID WHERE  b.BlastID = " + blastID;
                selectSQL += " AND b.TestBlast = 'n' AND b.StatusCode = 'sent' ORDER BY b.CustomerID ASC, b.BlastID ASC";
                SqlCommand myCommand = new SqlCommand(selectSQL);
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


            return blast;
        }
        

        private static string GetConnectionString()
        {
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings connection in connections)
            {
                if (connection.Name == "Communicator")
                {
                    return connection.ConnectionString;
                }
            }

            return "";
        }
    }

    
}
